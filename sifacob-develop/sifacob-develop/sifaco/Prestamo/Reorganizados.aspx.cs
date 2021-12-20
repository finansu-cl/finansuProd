using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

namespace sifaco.Prestamos
{
    public partial class Reorganizados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "24";
            Common cls = new Common();
            if (!IsPostBack)
            {
                SqlDataAdapter adapter2 = GetAdapterPrestamos();
                rptSimulaciones.DataSource = GetPrestamos(adapter2);
                rptSimulaciones.DataBind();

            }
        }

        protected void rptSimulaciones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                // e.CommandArgument;
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string id = arg[0].ToString();
                    string idCliente = arg[1].ToString();
                    Response.Redirect("../Prestamo/Reorganizado.aspx?cid=" + idCliente + "&pid=" + id);

                }
                catch (Exception ex)
                {
                }
            }
            if (e.CommandName == "delete")
            {
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string id = arg[0].ToString();
                    string idCliente = arg[1].ToString();
                    ltHidden.Text = id;
                    ltHidden2.Text = idCliente;
                    plhDeleteQuestion.Visible = true;
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

        }

        public SqlDataAdapter GetAdapterPrestamos()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PRESTAMOS_REORGANIZADOS";
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

        public List<PrestamosA> GetPrestamos(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<PrestamosA> re = null;
            re = (from item in dt.AsEnumerable()
                  select new PrestamosA()
                  {
                      ID = item.Field<int>("ID"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal?>("TASA") ?? 0,
                      Plazo = item.Field<int>("PLAZO"),
                      Cuota = item.Field<decimal?>("CUOTA") ?? 0,
                      Monto = item.Field<decimal?>("MONTO") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA")
                  }).ToList();
            return re;
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ltHidden.Text;
                string idCliente = ltHidden2.Text;
                if (id != "")
                {
                    GetAdapterNonQuery(id);
                    SqlDataAdapter adapter2 = GetAdapterPrestamos();
                    rptSimulaciones.DataSource = GetPrestamos(adapter2);
                    rptSimulaciones.DataBind();
                }
                plhDeleteQuestion.Visible = false;
                ltHidden.Text = "";
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                //return 0;
            }
        }

        public int GetLastCorrelativo()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CORRELATIVO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int re = 0;
                re = (from item in dt.AsEnumerable()
                      select new Correlativos()
                      {
                          Correlativo = item.Field<int>("CORRELATIVO")
                      }).LastOrDefault().Correlativo;
                return re;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return 0;
            }
        }

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_PRESTAMO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", Id);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                parametros.Add("@EST_OPER", "2");
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
            }
        }

    }
}