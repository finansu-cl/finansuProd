using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;

namespace sifaco.CobranzaFacturas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["fid"] != null && Request.QueryString["val"] != null && Request.QueryString["edo"] != null)
            //{
            //    if (!IsPostBack)
            //    {
            //        EditarEdoNotiFactura(Request.QueryString["fid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["edo"].ToString());
            //        StringBuilder sbJson = new StringBuilder();
            //        sbJson.Append("[{")
            //            .Append("    \"ok\": \"ok\"")
            //            .Append("}]");

            //        Page.Controls.Clear();
            //        string callback = Request["callback"];
            //        Response.Write(callback + "('" + sbJson.ToString() + "')");
            //    }
            //}

            //if (Request.QueryString["cid"] != null && Request.QueryString["val"] != null && Request.QueryString["edo"] != null)
            //{
            //    HtmlGenericControl cntrl = (HtmlGenericControl)Page.FindControl("f" + Request.QueryString["cid"].ToString());
            //    cntrl.Visible = true;
            //}
            Session["menu"] = "5";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterClientes(null, null,null);
                rptClienteFactura.DataSource = GetClientes(adapter);
                rptClienteFactura.DataBind();
            }
        }

        public SqlDataAdapter GetAdapterClientes(string param, string valParam, string tipoParam)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES_FACTURAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                return reader;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return null;
            }
        }

        public List<Clientes> GetClientes(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Clientes> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Clientes()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre"),
                      NumOperacion = item.Field<int>("NUM_OPERACION"),
                      Rut = item.Field<string>("Rut"),
                      MontoFactoring = item.Field<decimal>("Monto"),
                      MontoMora = item.Field<decimal>("MONTO_MORA")
                  }).ToList();
            return re;
        }



    }
}