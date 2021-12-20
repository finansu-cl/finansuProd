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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "8";
            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-5).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
                SqlDataAdapter adapter = GetAdapterFacturas("-1", DateTime.Now.AddDays(-5), DateTime.Now, "", -1);
                rptClienteFactura.DataSource = GetFacturas(adapter);
                rptClienteFactura.DataBind();

                SqlDataAdapter adapterC = GetAdapterCliente();
                ddlCliente.DataSource = GetClientes(adapterC);
                ddlCliente.DataTextField = "Nombre";
                ddlCliente.DataValueField = "ID";
                ddlCliente.DataBind();

                decimal mt = GetFacturas(adapter).Sum(x => x.Monto);
                decimal mot = GetFacturas(adapter).Sum(x => x.MontoMora);
                decimal ut = GetFacturas(adapter).Sum(x => x.Utilidad);
                decimal gt = GetFacturas(adapter).Sum(x => x.MontoGirable);
                int ft = GetFacturas(adapter).Count;
                txtMT.Text = mt.ToString("N"); 
                txtMoT.Text = mot.ToString("N");
                txtUT.Text = ut.ToString("N");
                txtGT.Text = gt.ToString("N");
                txtFT.Text = ft.ToString();
           }
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
                      Nombre = item.Field<string>("NOMBRE"),
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
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00");
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if(txtFechaD.Text != "")
                 fechaD = Convert.ToDateTime(txtFechaD.Text);
            if(txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);
            string deudor = txtDeudor.Text;
            int tipoFecha = Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString());
            SqlDataAdapter adapter = GetAdapterFacturas(idCliente, fechaD, fechaH, deudor, tipoFecha);
            rptClienteFactura.DataSource = GetFacturas(adapter);
            rptClienteFactura.DataBind();
            decimal mt = GetFacturas(adapter).Sum(x => x.Monto);
            decimal ut = GetFacturas(adapter).Sum(x => x.Utilidad);
            decimal gt = GetFacturas(adapter).Sum(x => x.MontoGirable);
            decimal mot = GetFacturas(adapter).Sum(x => x.MontoMora);
            int ft = GetFacturas(adapter).Count;
            txtMoT.Text = mot.ToString("N");
            txtMT.Text = mt.ToString("N");
            txtUT.Text = ut.ToString("N");
            txtGT.Text = gt.ToString("N");
            txtFT.Text = ft.ToString();

        }

    }
}