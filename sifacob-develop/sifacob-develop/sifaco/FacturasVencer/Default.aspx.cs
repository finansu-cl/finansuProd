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

namespace sifaco.FacturasVencer
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "7";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterFacturas("");
                rptClienteFactura.DataSource = GetFacturas(adapter);
                rptClienteFactura.DataBind();
            }
        }

        public SqlDataAdapter GetAdapterFacturas(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES";
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

        public List<Facturas> GetFacturas(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Facturas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Facturas()
                  {
                      ID = item.Field<int>("ID"),
                      idTipoFac = item.Field<int>("ID_TIPO_FACTURA"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      Tipo = item.Field<string>("Tipo"),
                      NumFactura = item.Field<string>("NUM_FACTURA"),
                      Monto = item.Field<decimal>("MONTO_TOTAL"),
                      Plazo = item.Field<int>("PLAZO"),
                      Utilidad = item.Field<decimal>("MONTO_INTERES"),
                      MontoGirable = item.Field<decimal>("MONTO_GIRABLE"),
                      MontoAnticipo = item.Field<decimal>("MONTO_ANTICIPO"),
                      MontoPendiente = item.Field<decimal>("MONTO_PENDIENTE"),
                      RutDeudor = item.Field<string>("RUT_DEUDOR"),
                      Deudor = item.Field<string>("DEUDOR"),
                      IdEdoFactura = item.Field<int>("ID_EDO_FACTURA"),
                      EstadoFactura = item.Field<string>("ESTADO"),
                      Notificacion = item.Field<string>("FLG_NOTIFICACION"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      Operacion = item.Field<DateTime?>("OPERACION"),
                      MontoMora = item.Field<decimal>("INTERES_MORA"),
                      MontoReembolso = item.Field<decimal>("REEMBOLSO"),
                      VenceEn = item.Field<int>("VENCE_EN"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal>("TASA")
                  }).ToList();
            return re;
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

        public string EditarEdoNotiFactura(string id, string valEdo, string idEdoFacAct)
        {
            try
            {
                if (idEdoFacAct == "1")
                {
                    int correlativo = GetLastCorrelativo();
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "usr_fnns.SP_MOD_EDO_FACTURA";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    ArrayList tipoDatos = new ArrayList();
                    parametros.Add("@ID", id.ToString());
                    tipoDatos.Add("int");
                    parametros.Add("@ID_EDO_FACTURA", valEdo);
                    tipoDatos.Add("int");
                    parametros.Add("@ORIGEN", "FACTURAS VENCER");
                    parametros.Add("@CORRELATIVO", correlativo.ToString());
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("int");
                    cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);
                }
                return "";
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return "";
            }
        }

    }
}