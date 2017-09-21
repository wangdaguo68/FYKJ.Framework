namespace FYKJ.Service
{
    using Castle.DynamicProxy;
    using System;
    using Log;
    using Framework.Contract;

    internal class InvokeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                if (exception is BusinessException)
                {
                    throw;
                }
                var message = new {
                    exception = exception.Message,
                    exceptionContext = new { method = invocation.Method.ToString(), arguments = invocation.Arguments, returnValue = invocation.ReturnValue }
                };
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, message, exception);
                throw;
            }
        }
    }
}

