using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sifaco.Handler
{
    /// <summary>
    /// Descripción breve de DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["fname"] != null && context.Request.QueryString["fname"] != "")
            {
                string fileName = context.Request.QueryString["fname"].ToString();
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                response.AddHeader("Content-Disposition",
                                   "attachment; filename=" + fileName + ".docx;");
                response.TransmitFile(HttpContext.Current.Server.MapPath("../App_Data/Downloads/" + fileName + ".docx"));
                response.Flush();
                response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}