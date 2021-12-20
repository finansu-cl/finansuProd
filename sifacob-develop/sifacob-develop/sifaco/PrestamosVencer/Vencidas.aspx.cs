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

namespace sifaco.PrestamosVencer
{
    public partial class Vencidas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "22";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterFacturas("");
                rptClienteFactura.DataSource = GetTablaAmortizacion(adapter);
                rptClienteFactura.DataBind();
            }
        }

        public SqlDataAdapter GetAdapterFacturas(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PRESTAMOS_CLIENTES_MORA";
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

        public List<PrestamosA> GetTablaAmortizacion(SqlDataAdapter adapter)
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
                      MesGracia = item.Field<int?>("MES_GRACIA") ?? 0,
                      Cuota = item.Field<decimal?>("CUOTA") ?? 0,
                      Monto = item.Field<decimal?>("MONTO") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA"),
                      IdEdoPres = item.Field<int>("ID_EDO_PRES"),
                      NumCuota = item.Field<decimal>("NUM_CUOTA"),
                      SaldoInicial = item.Field<decimal?>("MONTO_INI"),
                      Intereses = item.Field<decimal?>("INTERESES"),
                      Capital = item.Field<decimal?>("CAPITAL"),
                      SaldoFinal = item.Field<decimal>("MONTO_FIN"),
                      Mora = item.Field<decimal>("INTERES_MORA"),
                      FechaPago = item.Field<DateTime?>("FECHA_PAGO"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      VenceEn = item.Field<int>("DIAS_ATRASO")
                  }).ToList();
            return re;
        }

    }
}