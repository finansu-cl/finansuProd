using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Clases;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Text;

namespace sifaco.BuscadorCobranza
{
    public partial class Simulacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "23";
            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-5).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
                //SqlDataAdapter adapter = GetAdapterFacturas("-1", DateTime.Now.AddDays(-5), DateTime.Now, "", -1);
                //rptClienteFactura.DataSource = GetFacturas(adapter);
                //rptClienteFactura.DataBind();

                SqlDataAdapter adapterC = GetAdapterCliente();
                ddlCliente.DataSource = GetClientes(adapterC);
                ddlCliente.DataTextField = "Nombre";
                ddlCliente.DataValueField = "ID";
                ddlCliente.DataBind();

                SqlDataAdapter adapter2 = GetAdapterSimulaciones("-1", DateTime.Now.AddDays(-5), DateTime.Now, -1);
                rptClienteFactura.DataSource = GetSimulaciones(adapter2);
                rptClienteFactura.DataBind();

                decimal go = GetSimulaciones(adapter2).Sum(x => x.GastosOper) ?? 0;
                decimal co = GetSimulaciones(adapter2).Sum(x => x.Comision) ?? 0;
                decimal iv = GetSimulaciones(adapter2).Sum(x => x.Iva) ?? 0;
                decimal mg = GetSimulaciones(adapter2).Sum(x => x.MontoGirable) ?? 0;
                decimal mc = GetSimulaciones(adapter2).Sum(x => x.PrecioCes) ?? 0;
                txtMC.Text = mc.ToString("N");
                txtGO.Text = go.ToString("N");
                txtCO.Text = co.ToString("N");
                txtIV.Text = iv.ToString("N");
                txtMG.Text = mg.ToString("N");
            }
        }

        public SqlDataAdapter GetAdapterSimulaciones(string idCliente, DateTime? fechaD, DateTime? fechaH, int idEdoSimulacion)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_SIMULACIONES_FILTRO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                tipoDatos.Add("int");
                parametros.Add("@FECHA_INI", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_FIN", fechaH.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@ID_EDO_SIM", idEdoSimulacion.ToString());
                tipoDatos.Add("int");
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

        public List<Simulaciones> GetSimulaciones(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Simulaciones> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Simulaciones()
                  {
                      ID = item.Field<int>("ID"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      IdEdoSim = item.Field<int>("ID_EDO_SIM"),
                      Estado = item.Field<string>("Estado"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal?>("TASA") ?? 0,
                      Anticipo = item.Field<decimal?>("Anticipo") ?? 0,
                      SaldoPendiente = item.Field<decimal?>("SALDO_PENDIENTE") ?? 0,
                      //Plazo = item.Field<int>("PLAZO"),
                      GastosOper = item.Field<decimal?>("GASTOS_OPERACION") ?? 0,
                      Comision = item.Field<decimal?>("COMISION") ?? 0,
                      Iva = item.Field<decimal?>("IVA") ?? 0,
                      Utilidad = item.Field<decimal?>("MONTO_INTERES") ?? 0,
                      MontoTotal = item.Field<decimal?>("MONTO_FACTURA") ?? 0,
                      PrecioCes = item.Field<decimal?>("PRECIO_CESION") ?? 0,
                      MontoGirable = item.Field<decimal?>("MONTO_GIRABLE") ?? 0,
                      MontoAnticipo = item.Field<decimal?>("MONTO_ANTICIPO") ?? 0,
                      MontoPendiente = item.Field<decimal?>("MONTO_PENDIENTE") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterFacturas(string idCliente, DateTime? fechaD, DateTime? fechaH, string deudor, int tippFecha)
        {
            try
            {
                if (fechaD == null)
                    fechaD = Convert.ToDateTime("1753-01-01 00:00:00");
                if(fechaH == null)
                    fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");

                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_INFORMACION_FILTROS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                tipoDatos.Add("int");
                parametros.Add("@FECHA_DESDE", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_HASTA", fechaH.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@DEUDOR", deudor.ToString());
                tipoDatos.Add("varchar");
                parametros.Add("@TIPO_FECHA", tippFecha.ToString());
                tipoDatos.Add("int");
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
                      Nombre = item.Field<string>("Nombre"),
                      NumFactura = item.Field<string>("NUM_FACTURA"),
                      Monto = item.Field<decimal>("MONTO_TOTAL"),
                      Plazo = item.Field<int>("PLAZO"),
                      Utilidad = item.Field<decimal>("UTILIDAD"),
                      MontoGirable = item.Field<decimal>("MONTO_GIRABLE"),
                      MontoMora = item.Field<decimal>("INTERES_MORA"),
                      Deudor = item.Field<string>("DEUDOR"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      Operacion = item.Field<DateTime?>("OPERACION"),
                      Pago = item.Field<DateTime?>("FECHA_PAGO"),
                      Emision = item.Field<DateTime?>("FECHA_EMISION"),
                      VenceEn = item.Field<int>("DIAS_ATRASO"),
                      Tasa = item.Field<decimal>("TASA")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterCliente()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
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
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return re;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string idCliente = ddlCliente.SelectedValue.ToString();
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00");;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if(txtFechaD.Text != "")
                 fechaD = Convert.ToDateTime(txtFechaD.Text);
            if(txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);
            int tipoFecha = 0;//Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString());
            SqlDataAdapter adapter = GetAdapterSimulaciones(idCliente, fechaD, fechaH, tipoFecha);
            rptClienteFactura.DataSource = GetSimulaciones(adapter);
            rptClienteFactura.DataBind();
            decimal go = GetSimulaciones(adapter).Sum(x => x.GastosOper) ?? 0;
            decimal co = GetSimulaciones(adapter).Sum(x => x.Comision) ?? 0;
            decimal iv = GetSimulaciones(adapter).Sum(x => x.Iva) ?? 0;
            decimal mg = GetSimulaciones(adapter).Sum(x => x.MontoGirable) ?? 0;
            decimal mc = GetSimulaciones(adapter).Sum(x => x.PrecioCes) ?? 0;
            txtMC.Text = mc.ToString("N");
            txtGO.Text = go.ToString("N");
            txtCO.Text = co.ToString("N");
            txtIV.Text = iv.ToString("N");
            txtMG.Text = mg.ToString("N");

        }

    }
}