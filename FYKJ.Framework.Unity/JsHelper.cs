using System.Web;
using System.Web.UI;

namespace FYKJ.Framework.Utility
{
    public class JsHelper
    {
        public static void Alert(object message)
        {
            string format = "<Script language='JavaScript'>\r\n                    alert('{0}');  \r\n                  </Script>";
            HttpContext.Current.Response.Write(string.Format(format, message));
        }

        public static void Alert(string message)
        {
            message = StringUtil.DeleteUnVisibleChar(message);
            string s = "<Script language='JavaScript'>\r\n                    alert('" + message + "');</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void AlertAndRedirect(string message, string toURL)
        {
            string format = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(format, message, toURL));
        }

        public static void CloseOpenerWindow()
        {
            string s = "<Script language='JavaScript'>\r\n                    window.opener.close();  \r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void CloseParentWindow()
        {
            string s = "<Script language='JavaScript'>\r\n                    window.parent.close();  \r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void CloseWindow()
        {
            string s = "<Script language='JavaScript'>\r\n                    window.close();  \r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }

        public static void GoHistory(int value)
        {
            string format = "<Script language='JavaScript'>\r\n                    history.go({0});  \r\n                  </Script>";
            HttpContext.Current.Response.Write(string.Format(format, value));
        }

        public static void GotoParentWindow(string parentWindowUrl)
        {
            string s = "<Script language='JavaScript'>\r\n                    this.parent.location.replace('" + parentWindowUrl + "');</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void JavaScriptFrameHref(string FrameName, string url)
        {
            string s = "<Script language='JavaScript'>\r\n\t\t\t\t\t\r\n                    @obj.location.replace(\"{0}\");\r\n                  </Script>";
            s = string.Format(s.Replace("@obj", FrameName), url);
            HttpContext.Current.Response.Write(s);
        }

        public static void JavaScriptLocationHref(string url)
        {
            string format = "<Script language='JavaScript'>\r\n                    window.location.replace('{0}');\r\n                  </Script>";
            format = string.Format(format, url);
            HttpContext.Current.Response.Write(format);
        }

        public static void JavaScriptResetPage(string strRows)
        {
            string s = "<Script language='JavaScript'>\r\n                    window.parent.CenterFrame.rows='" + strRows + "';</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void JavaScriptSetCookie(string strName, string strValue)
        {
            string s = "<script language=Javascript>\r\n\t\t\tvar the_cookie = '" + strName + "=" + strValue + "'\r\n\t\t\tvar dateexpire = 'Tuesday, 01-Dec-2020 12:00:00 GMT';\r\n\t\t\t//document.cookie = the_cookie;//写入Cookie<BR>} <BR>\r\n\t\t\tdocument.cookie = the_cookie + '; expires='+dateexpire;\t\t\t\r\n\t\t\t</script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void JscriptSender(Page page)
        {
            page.ClientScript.RegisterHiddenField("__EVENTTARGET", "");
            page.ClientScript.RegisterHiddenField("__EVENTARGUMENT", "");
            string script = "\t\t\r\n<script language=Javascript>\r\n      function KendoPostBack(eventTarget, eventArgument) \r\n      {\r\n\t\t\t\tvar theform = document.forms[0];\r\n\t\t\t\ttheform.__EVENTTARGET.value = eventTarget;\r\n\t\t\t\ttheform.__EVENTARGUMENT.value = eventArgument;\r\n\t\t\t\ttheform.submit();\r\n\t\t\t}\r\n</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), "sds", script);
        }

        public static string JSStringFormat(string s)
        {
            return s.Replace("\r", @"\r").Replace("\n", @"\n").Replace("'", @"\'").Replace("\"", "\\\"");
        }

        public static void OpenWebForm(string url)
        {
            string s = "<Script language='JavaScript'>\r\n\t\t\t//window.open('" + url + "');\r\n\t\t\twindow.open('" + url + "','','height=0,width=0,top=0,left=0,location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');\r\n\t\t\t</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void OpenWebForm(string url, bool isFullScreen)
        {
            string s = "<Script language='JavaScript'>";
            if (isFullScreen)
            {
                s = (((s + "var iWidth = 0;") + "var iHeight = 0;" + "iWidth=window.screen.availWidth-10;") + "iHeight=window.screen.availHeight-50;" + "var szFeatures ='width=' + iWidth + ',height=' + iHeight + ',top=0,left=0,location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no';") + "window.open('" + url + "','',szFeatures);";
            }
            else
            {
                s = s + "window.open('" + url + "','','height=0,width=0,top=0,left=0,location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');";
            }
            s = s + "</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void OpenWebForm(string url, string formName)
        {
            string s = "<Script language='JavaScript'>\r\n\t\t\t//window.open('" + url + "','" + formName + "');\r\n\t\t\twindow.open('" + url + "','" + formName + "','height=0,width=0,top=0,left=0,location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');\r\n\t\t\t</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void OpenWebForm(string url, string name, string future)
        {
            string s = "<Script language='JavaScript'>\r\n                     window.open('" + url + "','" + name + "','" + future + "')\r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void RefreshOpener()
        {
            string s = "<Script language='JavaScript'>\r\n                    opener.location.reload();\r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void RefreshParent()
        {
            string s = "<Script language='JavaScript'>\r\n                    parent.location.reload();\r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void ReplaceOpenerParentFrame(string frameName, string frameWindowUrl)
        {
            string s = "<Script language='JavaScript'>\r\n                    window.opener.parent." + frameName + ".location.replace('" + frameWindowUrl + "');</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void ReplaceOpenerParentWindow(string openerParentWindowUrl)
        {
            string s = "<Script language='JavaScript'>\r\n                    window.opener.parent.location.replace('" + openerParentWindowUrl + "');</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void ReplaceOpenerWindow(string openerWindowUrl)
        {
            string s = "<Script language='JavaScript'>\r\n                    window.opener.location.replace('" + openerWindowUrl + "');</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static void ReplaceParentWindow(string parentWindowUrl, string caption, string future)
        {
            string s = "";
            if ((future != null) && (future.Trim() != ""))
            {
                s = "<script language=javascript>this.parent.location.replace('" + parentWindowUrl + "','" + caption + "','" + future + "');</script>";
            }
            else
            {
                s = "<script language=javascript>var iWidth = 0 ;var iHeight = 0 ;iWidth=window.screen.availWidth-10;iHeight=window.screen.availHeight-50;\r\n\t\t\t\t\t\t\tvar szFeatures = 'dialogWidth:'+iWidth+';dialogHeight:'+iHeight+';dialogLeft:0px;dialogTop:0px;center:yes;help=no;resizable:on;status:on;scroll=yes';this.parent.location.replace('" + parentWindowUrl + "','" + caption + "',szFeatures);</script>";
            }
            HttpContext.Current.Response.Write(s);
        }

        public static void RtnRltMsgbox(object message, string strWinCtrl)
        {
            string format = "<Script language='JavaScript'>\r\n\t\t\t\t\t strWinCtrl = true;\r\n                     strWinCtrl = if(!confirm('" + message + "'))return false;</Script>";
            HttpContext.Current.Response.Write(string.Format(format, message));
        }

        public static void SetHtmlElementValue(string formName, string elementName, string elementValue)
        {
            string s = "<Script language='JavaScript'>if(document." + formName + "." + elementName + "!=null){document." + formName + "." + elementName + ".value =" + elementValue + ";}</Script>";
            HttpContext.Current.Response.Write(s);
        }

        public static string ShowModalDialogJavascript(string webFormUrl)
        {
            return ("<script language=javascript>\r\n\t\t\t\t\t\t\tvar iWidth = 0 ;\r\n\t\t\t\t\t\t\tvar iHeight = 0 ;\r\n\t\t\t\t\t\t\tiWidth=window.screen.availWidth-10;\r\n\t\t\t\t\t\t\tiHeight=window.screen.availHeight-50;\r\n\t\t\t\t\t\t\tvar szFeatures = 'dialogWidth:'+iWidth+';dialogHeight:'+iHeight+';dialogLeft:0px;dialogTop:0px;center:yes;help=no;resizable:on;status:on;scroll=yes';\r\n\t\t\t\t\t\t\tshowModalDialog('" + webFormUrl + "','',szFeatures);</script>");
        }

        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            return ("<script language=javascript>\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\tshowModalDialog('" + webFormUrl + "','','" + features + "');</script>");
        }

        public static void ShowModalDialogWindow(string webFormUrl)
        {
            string s = ShowModalDialogJavascript(webFormUrl);
            HttpContext.Current.Response.Write(s);
        }

        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string s = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(s);
        }

        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            string features = "dialogWidth:" + width.ToString() + "px;dialogHeight:" + height.ToString() + "px;dialogLeft:" + left.ToString() + "px;dialogTop:" + top.ToString() + "px;center:yes;help=no;resizable:no;status:no;scroll=no";
            ShowModalDialogWindow(webFormUrl, features);
        }
    }
}

