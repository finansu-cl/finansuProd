using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Configuration;
using System.Collections;
using System.Data;

namespace sifaco.ToPrint
{
    public partial class Default : System.Web.UI.Page
    {
        public string ID
        {
            get { return txtIdSimulacion.Text; }
            set { txtIdSimulacion.Text = value; }
        }

        public string FechaSim
        {
            get { return txtFecSimulacion.Text; }
            set { txtFecSimulacion.Text = value; }
        }
        public int EstadoSim
        {
            get { return Convert.ToInt32(ddlEdoSim.SelectedValue); }
            set { ddlEdoSim.SelectedValue = value.ToString(); }
        }
        public decimal? Tasa
        {
            get { return Convert.ToDecimal(txtTasa.Text); }
            set { txtTasa.Text = value.ToString(); }
        }
        public decimal? Anticipo
        {
            get { return Convert.ToDecimal(txtAnticipo.Text); }
            set { txtAnticipo.Text = value.ToString(); }
        }
        public decimal? SaldoPendiente
        {
            get { return Convert.ToInt32(txtSalPendiente.Text); }
            set { txtSalPendiente.Text = value.ToString(); }
        }

        public decimal? GastosOperacion
        {
            get { return Convert.ToDecimal(txtGasOperacion.Text); }
            set { txtGasOperacion.Text = string.Format("{0:n0}", value); }
        }

        public decimal? MontoTotal
        {
            get { return Convert.ToDecimal(txtMonTotal.Text); }
            set { txtMonTotal.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }
        public decimal? Utilidad
        {
            get { return Convert.ToDecimal(txtMonInteres.Text); }
            set { txtMonInteres.Text = string.Format("{0:n0}", value); }
        }

        public decimal? MontoAnticipo
        {
            get { return Convert.ToDecimal(txtMonAnticipo.Text); }
            set { txtMonAnticipo.Text = string.Format("{0:n0}", value); }
        }

        public decimal? MontoPendiente
        {
            get { return Convert.ToDecimal(txtMonPendiente.Text); }
            set { txtMonPendiente.Text = string.Format("{0:n0}", value); }
        }
        public decimal? PrecioCesion
        {
            get { return Convert.ToDecimal(txtPreCesion.Text); }
            set { txtPreCesion.Text = string.Format("{0:n0}", value); }
        }

        public decimal? MontoGirable
        {
            get { return Convert.ToDecimal(txtMonGirable.Text); }
            set { txtMonGirable.Text = string.Format("{0:n0}", value); }
        }

        public bool EnabledAnticipo
        {
            get { return txtAnticipo.Visible; }
            set { txtAnticipo.Visible = value; }
        }
        public bool EnabledTasa
        {
            get { return txtTasa.Visible; }
            set { txtTasa.Visible = value; }
        }
        public bool EnabledGastos
        {
            get { return txtGasOperacion.Visible; }
            set { txtGasOperacion.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtTasa.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtAnticipo.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            //txtPlazo.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            txtGasOperacion.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtGasOperacion.Attributes.Add("onblur", "formatomiles(this.id);");
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterEdoSim("SP_SEL_EDO_SIMULACION", "", "");
                ddlEdoSim.DataSource = GetTipoFactura(adapter);
                ddlEdoSim.DataTextField = "Estado";
                ddlEdoSim.DataValueField = "Id";
                ddlEdoSim.DataBind();
            }

            string idCliente = Request.QueryString["cid"].ToString();
            SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
            Clientes cliente = GetClientes(adapterC).LastOrDefault();
            ltrCliente.Text = cliente.Nombre;
            ltrRutCliente.Text = cliente.Rut;

            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null && (Request.QueryString["flag"] != null || Convert.ToBoolean(Request.QueryString["flag"])))
            {
                string idSimulacion = Request.QueryString["sid"].ToString();
                idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapter = GetAdapter(idCliente, idSimulacion);
                rptFacturas.DataSource = GetFacturas(adapter);
                rptFacturas.DataBind();
                //CalculoSimulacion();
                FechaSim = DateTime.Now.ToShortDateString();
                SqlDataAdapter adapter2 = GetAdapterSelLastSim(idSimulacion, null);
                Simulaciones sim = GetSimulaciones(adapter2).LastOrDefault();
                if (sim != null)
                {
                    EstadoSim = sim.IdEdoSim;
                    FechaSim = sim.Fecha.ToShortDateString();
                    Tasa = sim.Tasa;
                    Anticipo = sim.Anticipo;
                    SaldoPendiente = sim.SaldoPendiente;
                    GastosOperacion = sim.GastosOper;
                    MontoTotal = sim.MontoTotal;
                    Utilidad = sim.Utilidad;
                    MontoAnticipo = sim.MontoAnticipo;
                    MontoPendiente = sim.MontoPendiente;
                    PrecioCesion = sim.PrecioCes;
                    MontoGirable = sim.MontoGirable;
                    //btnLastSim.Visible = false;
                }
            }

            if (Request.QueryString["ts"] != null && Request.QueryString["as"] != null && Request.QueryString["ss"] != null && Request.QueryString["pf"] != null && Request.QueryString["mtf"] != null && Request.QueryString["uf"] != null && Request.QueryString["maf"] != null && Request.QueryString["mpf"] != null && Request.QueryString["mgf"] != null && Request.QueryString["gos"] != null && Request.QueryString["mts"] != null && Request.QueryString["mps"] != null && Request.QueryString["us"] != null && Request.QueryString["mas"] != null && Request.QueryString["pcs"] != null && Request.QueryString["mgs"] != null && Request.QueryString["flag"] != null)
            {
                
                FechaSim = DateTime.Now.ToShortDateString();
                Tasa = Convert.ToDecimal(Request.QueryString["ts"].ToString());
                Anticipo = Convert.ToDecimal(Request.QueryString["as"].ToString());
                SaldoPendiente = Convert.ToDecimal(Request.QueryString["ss"].ToString());
                GastosOperacion = Convert.ToDecimal(Request.QueryString["gos"].ToString());
                MontoTotal = Convert.ToDecimal(Request.QueryString["mts"].ToString());
                Utilidad = Convert.ToDecimal(Request.QueryString["us"].ToString());
                MontoAnticipo = Convert.ToDecimal(Request.QueryString["mas"].ToString());
                MontoPendiente = Convert.ToDecimal(Request.QueryString["mps"].ToString());
                PrecioCesion = Convert.ToDecimal(Request.QueryString["pcs"].ToString());
                MontoGirable = Convert.ToDecimal(Request.QueryString["mgs"].ToString());
            }
        }

        public SqlDataAdapter GetAdapter(string idCliente, string idSimulacion)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "SP_SEL_FACTURAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente);
                parametros.Add("@ID_SIMULACION", idSimulacion);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
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
                      Deudor = item.Field<string>("DEUDOR")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterSelLastSim(string id, string idCliente)
        {
            try
            {
                if (idCliente == null)
                {
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "SP_SEL_SIMULACIONES";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("@ID", id);
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("int");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                    return reader;
                }
                else
                {
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "SP_SEL_SIMULACIONES";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("@ID_ULT_SIM", id);
                    parametros.Add("@ID_CLIENTE", idCliente);
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("int");
                    tipoDatos.Add("int");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn,  spName, parametros, tipoDatos);
                    return reader;
                }
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
                      Tasa = item.Field<decimal>("TASA"),
                      Anticipo = item.Field<decimal>("Anticipo"),
                      SaldoPendiente = item.Field<decimal>("SALDO_PENDIENTE"),
                      Plazo = item.Field<int>("PLAZO"),
                      GastosOper = item.Field<decimal>("GASTOS_OPERACION"),
                      Utilidad = item.Field<decimal>("MONTO_INTERES"),
                      MontoTotal = item.Field<decimal>("MONTO_FACTURA"),
                      PrecioCes = item.Field<decimal>("PRECIO_CESION"),
                      MontoGirable = item.Field<decimal>("MONTO_GIRABLE"),
                      MontoAnticipo = item.Field<decimal>("MONTO_ANTICIPO"),
                      MontoPendiente = item.Field<decimal>("MONTO_PENDIENTE"),
                      Fecha = item.Field<DateTime>("FECHA_CREA")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterEdoSim(string spName, string paramName, string paramValue)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
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

        public List<EstadoSimulacion> GetTipoFactura(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadoSimulacion> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadoSimulacion()
                  {
                      ID = item.Field<int>("ID"),
                      Estado = item.Field<string>("ESTADO")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterClientes(string param, string valParam, string tipoParam)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "SP_SEL_CLIENTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add(param, valParam);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add(tipoParam);
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
                      IdPersona = item.Field<int>("ID_PERSONA"),
                      IdEmpresa = item.Field<int>("ID_EMPRESA"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("NOMBRE"),
                  }).ToList();
            return re;
        }


    }
}