using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FYKJ.Framework.Contract;

namespace FYKJ.Framework.DAL
{
    public class DbContextBase : DbContext, IDataRepository
    {
        public DbContextBase(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbContextBase(string connectionString, IAuditable auditLogger) : this(connectionString)
        {
            AuditLogger = auditLogger;
        }

        public void Delete<T>(T entity) where T : ModelBase
        {
            Entry(entity).State = EntityState.Deleted;
            SaveChanges();
        }

        public T Find<T>(params object[] keyValues) where T : ModelBase
        {
            return Set<T>().Find(keyValues);
        }

        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase
        {
            if (conditions == null)
            {
                return Set<T>().ToList();
            }
            return Set<T>().Where(conditions).ToList();
        }

        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex) where T : ModelBase
        {
            IQueryable<T> source = (conditions == null) ? Set<T>() : Set<T>().Where(conditions);
            return source.OrderByDescending(orderBy).ToPagedList(pageIndex, pageSize);
        }

        public T Insert<T>(T entity) where T : ModelBase
        {
            Set<T>().Add(entity);
            SaveChanges();
            return entity;
        }

        public override int SaveChanges()
        {
            WriteAuditLog();
            return base.SaveChanges();
        }

        public T Update<T>(T entity) where T : ModelBase
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
            SaveChanges();
            return entity;
        }

        internal void WriteAuditLog()
        {
            if (AuditLogger != null)
            {
                using (IEnumerator<DbEntityEntry<ModelBase>> enumerator = ChangeTracker.Entries<ModelBase>().Where(delegate (DbEntityEntry<ModelBase> p)
                {
                    if ((p.State != EntityState.Added) && (p.State != EntityState.Deleted))
                    {
                        return (p.State == EntityState.Modified);
                    }
                    return true;
                }).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        DbEntityEntry<ModelBase> dbEntry = enumerator.Current;
                        string operaterName;
                        if (dbEntry.Entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), false).SingleOrDefault() is AuditableAttribute)
                        {
                            operaterName = WCFContext.Current.Operater.Name;
                            Task.Factory.StartNew(delegate
                            {
                                TableAttribute attribute = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                                string tableName = (attribute != null) ? attribute.Name : dbEntry.Entity.GetType().Name;
                                string moduleName = dbEntry.Entity.GetType().FullName.Split('.').Skip(1).FirstOrDefault();
                                AuditLogger.WriteLog(dbEntry.Entity.ID, operaterName, moduleName, tableName, dbEntry.State.ToString(), dbEntry.Entity);
                            });
                        }
                    }
                }
            }
        }

        public IAuditable AuditLogger { get; set; }

        public void Insert<T>(params T[] models) where T : ModelBase
        {
            foreach (var model in models)
            {
                Set<T>().Add(model);
                SaveChanges();
            }
        }

        public  List<T> GetSearchList<T>(Expression<Func<T, bool>> where) where T:ModelBase
        {
            var item = Set<T>().Where(where).ToList();
            return item;
        }

        /// <summary>
        ///     Content:根据条件查询动态排序且分页，字段和排序类型均可变
        ///     Time:2016.03.03
        ///     Author:王达国
        /// </summary>
        /// <param name="db">数据库db</param>
        /// <param name="where"></param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderByExpression">排序字段和排序类型实体</param>
        /// <returns></returns>
        public static IList DynamicOrder<T>(DbContext db, Expression<Func<T, bool>> where, int pageIndex, int pageSize,
            params OrderModelField[] orderByExpression) where T:ModelBase
        {
            var query = db.Set<T>().Where(where);
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");
            if (orderByExpression == null || orderByExpression.Length <= 0)
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (var t in orderByExpression)
            {
                //根据属性名获取属性
                PropertyInfo property;
                if (t.PropertyName == null || t.OrderType == null)
                {
                    property = typeof(T).GetProperties()[0];
                }
                else
                {
                    property = typeof(T).GetProperty(t.PropertyName);
                }
                //创建一个访问属性的表达式
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var orderName = t.OrderType == "desc" ? "OrderByDescending" : "OrderBy";
                var resultExp = Expression.Call(typeof(Queryable), orderName,
                    new[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                query = query.Provider.CreateQuery<T>(resultExp);
            }
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        ///     Content:根据结果集动态排序且分页，字段和排序类型均可变
        ///     Time:2016.03.03
        ///     Author:王达国
        /// </summary>
        /// <param name="query">结果集</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderByExpression">排序字段和排序类型实体</param>
        /// <returns></returns>
        public static IList DynamicOrder<T>(IQueryable<T> query, int pageIndex, int pageSize,
            params OrderModelField[] orderByExpression) where T:ModelBase
        {
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");
            if (orderByExpression == null || orderByExpression.Length <= 0)
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (var t in orderByExpression)
            {
                //根据属性名获取属性
                PropertyInfo property;
                if (t.PropertyName == null || t.OrderType == null)
                {
                    property = typeof(T).GetProperties()[0];
                }
                else
                {
                    property = typeof(T).GetProperty(t.PropertyName);
                }
                //创建一个访问属性的表达式
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var orderName = t.OrderType == "desc" ? "OrderByDescending" : "OrderBy";
                var resultExp = Expression.Call(typeof(Queryable), orderName,
                    new[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                query = query.Provider.CreateQuery<T>(resultExp);
            }
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }


        /// <summary>
        /// </summary>
        public struct OrderModelField
        {
            /// <summary>
            ///     属性名称
            /// </summary>
            public string PropertyName { get; set; }

            /// <summary>
            ///     是否降序
            /// </summary>
            public string OrderType { get; set; }
        }
    }
}

