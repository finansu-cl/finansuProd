using System;
using System.Web;
using System.Web.Services;

namespace sifaco.Home
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["datos"] = true;
        }


        [WebMethod()]
        public static bool KeepActiveSession()
        {
            if (HttpContext.Current.Session["datos"] != null)
                return true;
            else
                return false;
        }
    }
}