using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Collections;
using System.Configuration;
using System.Data;
using Novacode;
using System.IO;
using System.Globalization;


namespace sifaco.Factoring
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "4";
            if (!IsPostBack)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
                Clientes cliente = GetClientes(adapterC).LastOrDefault();

                ltrCliente.Text = cliente.Nombre;
                ltrRutCliente.Text = cliente.Rut;
                if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null && (Request.QueryString["flag"] == null || !Convert.ToBoolean(Request.QueryString["flag"])))
                {
                    string idSimulacion = Request.QueryString["sid"].ToString();
                    idCliente = Request.QueryString["cid"].ToString();
                    aPrint.Attributes.Add("onclick", "window.open('../ToPrint/?cid=" + idCliente + "&sid=" + idSimulacion + "&flag=true', this.target, 'width=300,height=400'); return false;");
                    SqlDataAdapter adapter = GetAdapter(idCliente, idSimulacion);
                    var getFac = GetFacturas(adapter);
                    rptFacturas.DataSource = getFac;
                    rptFacturas.DataBind();
                    if (getFac != null)
                        btnCrearCarta.Enabled = true;
                    //Sim.FechaSim = DateTime.Now.ToShortDateString();
                    SqlDataAdapter adapter2 = GetAdapterSelLastSim(idSimulacion, idCliente);
                    Simulaciones sim = GetSimulaciones(adapter2).LastOrDefault();

                    if (sim == null)
                        btnLastSim.Enabled = false;
                    else
                    {
                        Session.Remove("EstadoSimulacion");
                        Session["EstadoSimulacion"] = sim.IdEdoSim;
                        Sim.EstadoSim = sim.IdEdoSim;
                        Sim.FechaSim = sim.Fecha.ToShortDateString();
                        Sim.Tasa = sim.Tasa;
                        Sim.Anticipo = sim.Anticipo;
                        Sim.SaldoPendiente = sim.SaldoPendiente;
                        //Sim.Plazo = sim.Plazo;
                        Sim.GastosOperacion = sim.GastosOper;
                        Sim.MontoTotal = sim.MontoTotal;
                        Sim.Utilidad = sim.Utilidad;
                        Sim.MontoAnticipo = sim.MontoAnticipo;
                        Sim.MontoPendiente = sim.MontoPendiente;
                        Sim.PrecioCesion = sim.PrecioCes;
                        Sim.MontoGirable = sim.MontoGirable;
                        Sim.Comision = sim.Comision;
                        Sim.Iva = sim.Iva;
                        Sim.NvaTasa = sim.NuevaTasa;
                    }
                    //CalculoSimulacion();
                }
                if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null && (Request.QueryString["flag"] != null || Convert.ToBoolean(Request.QueryString["flag"])))
                {
                    string idSimulacion = Request.QueryString["sid"].ToString();
                    idCliente = Request.QueryString["cid"].ToString();
                    aPrint.Attributes.Add("onclick", "window.open('../ToPrint/?cid=" + idCliente + "&sid=" + idSimulacion + "&flag=true', this.target, 'width=800,height=900'); return false;");
                    SqlDataAdapter adapter = GetAdapter(idCliente, idSimulacion);
                    var getFac = GetFacturas(adapter);
                    rptFacturas.DataSource = getFac;
                    rptFacturas.DataBind();
                    if (getFac != null)
                        btnCrearCarta.Enabled = true;
                    Sim.FechaSim = DateTime.Now.ToShortDateString();
                    SqlDataAdapter adapter2 = GetAdapterSelLastSim(idSimulacion, null);
                    Simulaciones sim = GetSimulaciones(adapter2).LastOrDefault();
                    btnCrearDocs.Enabled = true;
                    if (sim == null)
                        btnLastSim.Enabled = false;
                    else 
                    {
                        Session.Remove("EstadoSimulacion");
                        Session["EstadoSimulacion"] = sim.IdEdoSim;
                        Sim.EstadoSim = sim.IdEdoSim;
                        Sim.FechaSim = sim.Fecha.ToShortDateString();
                        Sim.Tasa = sim.Tasa;
                        Sim.Anticipo = sim.Anticipo;
                        Sim.SaldoPendiente = sim.SaldoPendiente;
                        //Sim.Plazo = sim.Plazo;
                        Sim.GastosOperacion = sim.GastosOper;
                        Sim.MontoTotal = sim.MontoTotal;
                        Sim.Utilidad = sim.Utilidad;
                        Sim.MontoAnticipo = sim.MontoAnticipo;
                        Sim.MontoPendiente = sim.MontoPendiente;
                        Sim.PrecioCesion = sim.PrecioCes;
                        Sim.MontoGirable = sim.MontoGirable;
                        Sim.Comision = sim.Comision;
                        Sim.Iva = sim.Iva;
                        Sim.NvaTasa = sim.NuevaTasa;
                        btnLastSim.Visible = false;
                    }
                    //CalculoSimulacion();
                }

                if (!Page.IsPostBack)
                {
                    SqlDataAdapter adapter1 = GetAdapterDocumentos("001");
                    rptCrearDocs.DataSource = GetDocumentos(adapter1);
                    rptCrearDocs.DataBind();
                }

                //if (Session["rol"] != null && Session["rol"].ToString() == "user")
                //{
                //    foreach (RepeaterItem item in rptFacturas.Items)
                //    {
                //        Button btnEdit = (Button)item.FindControl("btnEdit");
                //        Button btnDelete = (Button)item.FindControl("btnDelete");
                //        btnEdit.Enabled = false;
                //        btnEdit.ToolTip = "Usted No posee permisos para modificar";
                //        btnDelete.Enabled = false;
                //        btnDelete.ToolTip = "Usted No posee permisos para modificar";
                //    }
                //}
                //if (Session["rol"] != null && Session["rol"].ToString() == "analista")
                //{
                //    foreach (RepeaterItem item in rptFacturas.Items)
                //    {
                //        Button btnDelete = (Button)item.FindControl("btnDelete");
                //        btnDelete.Enabled = false;
                //        btnDelete.ToolTip = "Usted No posee permisos para modificar";
                //    }
                //}

            }
        }

        public SqlDataAdapter GetAdapter(string idCliente, string idSimulacion)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS";
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
                      Deudor = item.Field<string>("DEUDOR"),
                      DireccionDeudor = item.Field<string>("DIRECCION_DEUDOR"),
                      ComunaDeudor = item.Field<string>("COMUNA_DEUDOR"),
                      Emision = item.Field<DateTime?>("FECHA_EMISION")

                  }).ToList();
            return re;
        }

        public int GetAdapterCreateFac(string idTipoFac, string idCliente, string idSimulacion, string monto, string numFac, string deudor, string rutDeudor, int plazo, decimal utilidad, decimal montoGir, decimal montoAnt, decimal montoPend, string direccionDeudor, string comunaDeudor, DateTime? fechaEmision)
        {
            try
            {
                string monto2 = monto.Replace(".", "");
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_CREATE_FACTURAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_TIPO_FACTURA", idTipoFac);
                parametros.Add("@ID_CLIENTE", idCliente);
                parametros.Add("@ID_SIMULACION", idSimulacion);
                parametros.Add("@MONTO_TOTAL", monto2);
                parametros.Add("@PLAZO", plazo.ToString());
                parametros.Add("@MONTO_INTERES", utilidad.ToString().Replace(".", ""));
                parametros.Add("@MONTO_GIRABLE", montoGir.ToString().Replace(".", ""));
                parametros.Add("@MONTO_ANTICIPO", montoAnt.ToString().Replace(".", ""));
                parametros.Add("@MONTO_PENDIENTE", montoPend.ToString().Replace(".", ""));
                parametros.Add("@NUM_FACTURA", numFac);
                parametros.Add("@DEUDOR", deudor);
                parametros.Add("@RUT", rutDeudor);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@DIRECCION_DEUDOR", direccionDeudor);
                parametros.Add("@COMUNA_DEUDOR", comunaDeudor);
                parametros.Add("@FECHA_EMISION", Convert.ToDateTime(fechaEmision).ToShortDateString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("float");
                tipoDatos.Add("int");
                tipoDatos.Add("numeric");
                tipoDatos.Add("numeric");
                tipoDatos.Add("numeric");
                tipoDatos.Add("numeric");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idFactura = Convert.ToInt32(dt.Rows[0]["ID"]);
                return idFactura;
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

        public void GetAdapterEditSim(int id, int idCliente, int edoSim, decimal? tasa, decimal? anticipo, decimal? salPendiente, int plazo, decimal? gastosOpe, decimal? utilidad, decimal? preCesion, decimal? montoGir, decimal? montoAnt, decimal? montoPend, decimal? montoTotal, decimal? comision, decimal? iva, string fechaSim)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_SIMULACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@ID_EDO_SIM", edoSim.ToString());
                parametros.Add("@TASA", tasa.ToString().Replace(".",""));
                parametros.Add("@ANTICIPO", anticipo.ToString());
                parametros.Add("@SALDO_PENDIENTE", salPendiente.ToString());
                parametros.Add("@PLAZO", plazo.ToString());
                parametros.Add("@GASTOS_OPERACION", gastosOpe.ToString().Replace(".", ""));
                parametros.Add("@MONTO_INTERES", utilidad.ToString().Replace(".", ""));
                parametros.Add("@MONTO_FACTURA", montoTotal.ToString().Replace(".", ""));
                parametros.Add("@PRECIO_CESION", preCesion.ToString().Replace(".", ""));
                parametros.Add("@MONTO_GIRABLE", montoGir.ToString().Replace(".", ""));
                parametros.Add("@MONTO_ANTICIPO", montoAnt.ToString().Replace(".", ""));
                parametros.Add("@MONTO_PENDIENTE", montoPend.ToString().Replace(".", ""));
                parametros.Add("@COMISION", comision.ToString().Replace(".", ""));
                parametros.Add("@IVA", iva.ToString().Replace(".", ""));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "FACTORING");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                parametros.Add("@FECHA_CREA", Convert.ToDateTime(fechaSim).ToShortDateString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("decimal18");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                tipoDatos.Add("datetime");
                cls.GetConnectionToDbInsert(conn, spName, parametros, tipoDatos);
                /*DataTable dt = new DataTable();
                reader.Fill(dt);
                int idFactura = Convert.ToInt32(dt.Rows[0]["ID"]);
                return idFactura;*/
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

        public void GetAdapterEditTasaSim(int id, int idCliente, decimal? tasa)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_SIMULACION_TASA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@TASA", tasa.ToString().Replace(".", ""));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "FACTORING");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                cls.GetConnectionToDbInsert(conn, spName, parametros, tipoDatos);
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

        protected void btnGuardarFac_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string idSimulacion = Request.QueryString["sid"].ToString();
                CalculoFactura();
                int idFac = GetAdapterCreateFac(fac.TipoFac, idCliente, idSimulacion, fac.Monto, fac.NumFac, fac.Deudor, fac.Rut,fac.Plazo,fac.Utilidad,fac.MontoGirable,fac.MontoAnticipo,fac.MontoPendiente, fac.DireccionDeudor, fac.ComunaDeudor,fac.FechaEmision);
                SqlDataAdapter adapter = GetAdapter(idCliente, idSimulacion);
                rptFacturas.DataSource = GetFacturas(adapter);
                rptFacturas.DataBind();
            }
            LimpiarForm();
            CalculoSimulacion();
        }

        public SqlDataAdapter GetAdapterSelFactura(string id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        protected void rptFacturas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                // e.CommandArgument;
                try
                {
                    SqlDataAdapter adapter = GetAdapterSelFactura(e.CommandArgument.ToString());
                    var factura = GetFacturas(adapter).FirstOrDefault();
                    fac.ID = factura.ID.ToString();
                    fac.Rut = factura.RutDeudor;
                    fac.Deudor = factura.Deudor;
                    fac.TipoFac = factura.idTipoFac.ToString();
                    fac.NumFac = factura.NumFactura;
                    fac.Monto = decimal.Parse(factura.Monto.ToString()).ToString("N");
                    fac.Plazo = factura.Plazo;
                    fac.MontoAnticipo = factura.MontoAnticipo;
                    fac.MontoPendiente = factura.MontoPendiente;
                    fac.MontoGirable = factura.MontoGirable;
                    fac.Utilidad = factura.Utilidad;
                    fac.DireccionDeudor = factura.DireccionDeudor;
                    fac.ComunaDeudor = factura.ComunaDeudor;
                    fac.FechaEmision = factura.Emision;
                    btnModificarFac.Visible = true;
                    btnGuardarFac.Visible = false;

                }
                catch (Exception ex)
                {
                }
            }
            if (e.CommandName == "delete")
            {
                try
                {
                    ltHidden.Text = e.CommandArgument.ToString();
                    plhDeleteQuestion.Visible = true;
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

        }

        public void EditarFactura(string rut, string idTipoFac, string deudor, string numFac, string monto, string id, string plazo, string utilidad, string montoGirable, string montoAnticipo, string montoPendiente, string direccionDeudor, string comunaDeudor, DateTime? fechaEmision)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                string monto2 = monto.Replace(".", "");
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_FACTURA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@ID_TIPO_FACTURA", idTipoFac);
                parametros.Add("@RUT", rut);
                parametros.Add("@DEUDOR", deudor);
                parametros.Add("@NUM_FACTURA", numFac);
                parametros.Add("@MONTO_TOTAL", monto2);
                parametros.Add("@PLAZO", plazo);
                parametros.Add("@MONTO_INTERES", utilidad.Replace(".", ""));
                parametros.Add("@MONTO_GIRABLE", montoGirable.Replace(".", ""));
                parametros.Add("@MONTO_ANTICIPO", montoAnticipo.Replace(".", ""));
                parametros.Add("@MONTO_PENDIENTE", montoPendiente.Replace(".", ""));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "FACTORING");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                parametros.Add("@DIRECCION_DEUDOR", direccionDeudor);
                parametros.Add("@COMUNA_DEUDOR", comunaDeudor);
                parametros.Add("@FECHA_EMISION", fechaEmision.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("decimal");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
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

        protected void btnModificarFac_Click(object sender, EventArgs e)
        {
            CalculoFactura();
            EditarFactura(fac.Rut, fac.TipoFac, fac.Deudor, fac.NumFac, fac.Monto, fac.ID, fac.Plazo.ToString(), fac.Utilidad.ToString(), fac.MontoGirable.ToString(), fac.MontoAnticipo.ToString(), fac.MontoPendiente.ToString(), fac.DireccionDeudor, fac.ComunaDeudor, fac.FechaEmision);
            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string idSimulacion = Request.QueryString["sid"].ToString();
                SqlDataAdapter adapter = GetAdapter(idCliente, idSimulacion);
                rptFacturas.DataSource = GetFacturas(adapter);
                rptFacturas.DataBind();
                btnModificarFac.Visible = false;
                btnGuardarFac.Visible = true;

            }
            LimpiarForm();
            CalculoSimulacion();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ltHidden.Text;
                if (id != "")
                {
                    GetAdapterNonQuery(id);
                    if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
                    {
                        string idCliente = Request.QueryString["cid"].ToString();
                        string idSimulacion = Request.QueryString["sid"].ToString();
                        SqlDataAdapter adapter = GetAdapter(idCliente,idSimulacion);
                        rptFacturas.DataSource = GetFacturas(adapter);
                        rptFacturas.DataBind();
                    }
                }
                plhDeleteQuestion.Visible = false;
                ltHidden.Text = "";
                CalculoSimulacion();
                GuardarCalculoSimulacion();
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

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_FACTURA";
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

        public void LimpiarForm()
        {
            fac.Rut = "";
            fac.Deudor = "";
            fac.TipoFac = "-1";
            fac.NumFac = "";
            fac.Monto = "0";
            fac.ID = "0";
            fac.Plazo = 0;
            fac.Utilidad = 0;
            fac.MontoAnticipo = 0;
            fac.MontoGirable = 0;
            fac.MontoPendiente = 0;
            fac.DireccionDeudor = "";
            fac.ComunaDeudor = "-- Seleccione --";
            fac.FechaEmision = DateTime.Now;

        }

        public void SumMontoFac() 
        {
            decimal montoTotal = 0;
            decimal montoUtilidad = 0;
            decimal montoAnticipo = 0;
            decimal montoPendiente = 0;
            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string idSimulacion = Request.QueryString["sid"].ToString();
                SqlDataAdapter adapter = GetAdapter(idCliente,idSimulacion);
                List<Facturas> fac = GetFacturas(adapter);
                montoTotal = fac.Sum(x => x.Monto);
                montoAnticipo = fac.Sum(x => x.MontoAnticipo);
                montoPendiente = fac.Sum(x => x.MontoPendiente);
                montoUtilidad = fac.Sum(x => x.Utilidad);
                Sim.MontoTotal = montoTotal;
                Sim.MontoAnticipo = montoAnticipo;
                Sim.MontoPendiente = montoPendiente;
                Sim.Utilidad = montoUtilidad;
            }
            //return retorno;
        }

        public void CalculoSimulacion() 
        {
            /*Sim.MontoTotal =*/ 
            if (Sim.Tasa != 0 /*&& Sim.Plazo != 0*/)
            {
                SumMontoFac();
                Sim.SaldoPendiente = 100 - Sim.Anticipo;
                Sim.MontoAnticipo = (Sim.Anticipo * Sim.MontoTotal) / 100;
                if(Sim.Comision>0)
                    Sim.Iva = (19 * Sim.Comision) / 100;
                Sim.MontoPendiente = Sim.MontoTotal - Sim.MontoAnticipo;
                /*Sim.Utilidad = ((((Sim.Tasa / 100) / 30) * Sim.Plazo) * Sim.MontoAnticipo);*/
                Sim.PrecioCesion = Sim.MontoAnticipo - Sim.Utilidad;
                Sim.MontoGirable = Sim.PrecioCesion - Sim.GastosOperacion-Sim.Comision-Sim.Iva;
            }

        }

        public void CalculoFactura()
        {
            if (Sim.Tasa != 0 && Sim.Anticipo != 0)
            {
                fac.MontoAnticipo = (Convert.ToDecimal(Sim.Anticipo??0) * Convert.ToDecimal(fac.Monto)) / 100;
                fac.MontoPendiente = Convert.ToDecimal(fac.Monto) - fac.MontoAnticipo;
                fac.Utilidad = ((((Convert.ToDecimal(Sim.Tasa??0) / 100) / 30) * fac.Plazo) * fac.MontoAnticipo);
                fac.MontoGirable = fac.MontoAnticipo - fac.Utilidad;
            }

        }

        public SqlDataAdapter GetAdapterSelLastSim(string id, string idCliente)
        {
            try
            {
                if (idCliente == null)
                {
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "usr_fnns.SP_SEL_SIMULACIONES";
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
                    string spName = "usr_fnns.SP_SEL_SIMULACIONES";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("@ID_ULT_SIM", id);
                    parametros.Add("@ID_CLIENTE", idCliente);
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("int");
                    tipoDatos.Add("int");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
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
                      NuevaTasa = item.Field<decimal>("NVA_TASA"),
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
                      Comision = item.Field<decimal>("COMISION"),
                      Iva = item.Field<decimal>("IVA"),
                      Fecha = item.Field<DateTime>("FECHA_CREA"),
                      FechaPrimeraOp = item.Field<DateTime?>("FECHA_PRIMERA")
                  }).ToList();
            return re;
        }

        protected void btnGuardarSim_Click(object sender, EventArgs e)
        {
            GuardarCalculoSimulacion();
        }

        private void GuardarCalculoSimulacion() 
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
            {
                int idSimulacion = Convert.ToInt32(Request.QueryString["sid"].ToString());
                int idCliente = Convert.ToInt32(Request.QueryString["cid"].ToString());
                if (Sim.CambioTasa)
                {
                    GetAdapterEditTasaSim(idSimulacion, idCliente, Sim.NvaTasa);
                }
                else
                {
                    CalculoSimulacion();
                    GetAdapterEditSim(idSimulacion, idCliente, Sim.EstadoSim, Sim.Tasa, Sim.Anticipo, Sim.SaldoPendiente, 0, Sim.GastosOperacion, Sim.Utilidad, Sim.PrecioCesion, Sim.MontoGirable, Sim.MontoAnticipo, Sim.MontoPendiente, Sim.MontoTotal, Sim.Comision, Sim.Iva, Sim.FechaSim);
                }
                Response.Redirect("../Factoring/?cid=" + idCliente + "&sid=" + idSimulacion + "&flag=true");
            }
        }

        protected void btnLastSim_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
            {
                string idSimulacion = Request.QueryString["sid"].ToString();
                string idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapter = GetAdapterSelLastSim(idSimulacion,idCliente);
                Simulaciones sim = GetSimulaciones(adapter).LastOrDefault();
                Response.Redirect("../Factoring/?cid=" + idCliente + "&sid=" + sim.ID.ToString());
            }
        }

        protected void btnCrearDocs_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (RepeaterItem ri in rptCrearDocs.Items)
            {
                CheckBox chk = (CheckBox)ri.FindControl("chkDoc");
                HiddenField hd = (HiddenField)ri.FindControl("hdItem");
                if (chk.Checked)
                {
                    if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
                    {
                        CreateDocumentWord(hd.Value, "","","", false);
                    }
                    count = count + 1;
                }
            }
            if (count == 0)
            {
                Response.Write("<script>alert('Debe seleccionar al menos 1 documento');</script>");
            }
        }

        protected void btnCrearCarta_Click(object sender, EventArgs e)
        {
            int count = 0;
            string numFac = "";
            string dirDeud = "";
            string comDeud = "";
            foreach (RepeaterItem ri in rptFacturas.Items)
            {
                CheckBox chk = (CheckBox)ri.FindControl("chkCarta");
                HiddenField hd = (HiddenField)ri.FindControl("hdNumFac");
                HiddenField hd1 = (HiddenField)ri.FindControl("hdDirDeud");
                HiddenField hd2 = (HiddenField)ri.FindControl("hdComDeud");
                if (chk.Checked)
                {
                    if (Request.QueryString["cid"] != null && Request.QueryString["sid"] != null)
                    {
                        numFac += hd.Value + ",";
                        dirDeud = hd1.Value;
                        comDeud = hd2.Value;                            

                    }
                    count = count + 1;
                }
            }
            if (count == 0)
            {
                Response.Write("<script>alert('Debe seleccionar al menos 1 factura');</script>");
            }
            else 
            {
                CreateDocumentWord("CARTA_NOTARIA", numFac,dirDeud,comDeud, true);
            }
        }

        public void CreateDocumentWord(string nameTemplate, string numFacConcat, string dirDeud, string comunDeud, bool cartaNotifica) 
        {
            Common cls = new Common();
            string idCliente = Request.QueryString["cid"].ToString();
            string idSimulacion = Request.QueryString["sid"].ToString();
            SqlDataAdapter adapterF = GetAdapter(idCliente, idSimulacion);
            List<Facturas> facturas = GetFacturas(adapterF);
            SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
            Clientes cliente = GetClientes(adapterC).LastOrDefault();
            SqlDataAdapter adapterS = GetAdapterSelLastSim(idSimulacion, null);
            Simulaciones simulacion = GetSimulaciones(adapterS).LastOrDefault();
            SqlDataAdapter adapterDB = GetAdapterSelDatosBanco(idCliente);
            DatosBanco datosBanco = GetDatosBanco(adapterDB).FirstOrDefault();

            System.Diagnostics.Debug.WriteLine("SIMULACION");
            System.Diagnostics.Debug.WriteLine(simulacion);
            System.Diagnostics.Debug.WriteLine("SIMULACION");

            if (simulacion == null)
            {
                Response.Write("<script>alert('Crear documentos ha fallado: No se ha creado una simulación aún');</script>");
            } else
            {
                //string nameTemplate = "CESION_100";
                string nombreCliente = "";
                string fechaDdMm = "";
                string fechaDdMmNum = "";
                string tipoEmpresa = "";
                string giroComercial = "";
                string rutCliente = "";
                string datosRep = "";
                string sustantivoDir = "";
                string direccion = "";
                string precioCesion = "";
                string deudor = "";
                string representante = "";
                string saldoPendiente = "";
                string fechaEscritura = "";
                string notaria = "";
                string montoTotal = "";
                string rutRep = "";
                string nacionalidad = "";
                string edoCivil = "";
                string sustantivoPer = "";
                string numRutCliente = "";
                string banco = "";
                string numCta = "";
                string plazo = "";
                string anticipo = "";
                string salPendiente = "";
                string montoGir = "";
                string nombreDest = "";
                string rutDest = "";
                string numFact = "";
                string dirDeudor = "";
                string comunaDeudor = "";
                string fechaPrimeraOperacion = "";
                string rutLetras = "";
                string rutLetras2 = "";
                if (cliente.IdPersona == 0)
                {
                    #region Documentos para Empresa
                    if (nameTemplate == "CESION")
                    {
                        if (simulacion.Anticipo == 100)
                            nameTemplate = "CESION_100";
                        else
                            nameTemplate = "CESION_95";
                    }

                    SqlDataAdapter adapterE = GetAdapterSelEmpresa(cliente.Rut);
                    Empresa empresas = GetEmpresa(adapterE).LastOrDefault();
                    SqlDataAdapter adapterR = GetAdapterRepresentantes(empresas.ID.ToString());
                    List<Persona> representantes = GetPersonas(adapterR);
                    string represen = "";
                    string repDatos = "";
                    string sust = "";
                    int num = 0;
                    foreach (var rep in representantes)
                    {
                        num++;
                        if (num == 1)
                            sust = "domiciliado";
                        if (num == 2)
                            sust = "ambos domiciliados";
                        if (num > 2)
                            sust = "todos domiciliados";

                        string sinDon = "";
                        string edo = "";
                        if (rep.Sexo == "M")
                        {
                            sinDon = "don ";
                        }
                        else
                        {
                            if (num == 1)
                                sust = "domiciliada";
                            sinDon = "doña ";
                        }

                        switch (rep.EdoCivil)
                        {
                            case "SO":
                                edo = "soltero";
                                break;
                            case "CA":
                                edo = "casado";
                                break;
                            case "VI":
                                edo = "viudo";
                                break;
                            case "DI":
                                edo = "divorciado";
                                break;
                            case "CC":
                                edo = "conviviente civil";
                                break;
                        }

                        string[] rutR = rep.Rut.Replace(".", "").Split('-');
                        rutLetras = cls.NumberToWords(Convert.ToInt32(rutR[0]));
                        rutLetras += " guion ";
                        if (rutR[1].ToString() == "k" || rutR[1].ToString() == "K")
                            rutLetras += "K";
                        else
                            rutLetras += cls.NumberToWords(Convert.ToInt32(rutR[1]));
                        repDatos += sinDon + rep.Nombre + ", " + rep.Nacionalidad + ", " + edo + ", factor de comercio, Cédula Nacional de identidad número " + rutLetras + " ";
                        represen += rep.Nombre + ",";
                        rutRep += rep.Rut + ", ";
                        nacionalidad += rep.Nacionalidad + ",";
                        edoCivil += edo + ",";
                        rutLetras2 += rutLetras + ",";

                    }
                    string nombreDeudor = "";
                    foreach (var fact in facturas)
                    {
                        nombreDeudor += fact.Deudor + ", ";
                        numFact += fact.NumFactura + ", ";
                        dirDeudor += fact.DireccionDeudor + ", ";
                        comunaDeudor += fact.ComunaDeudor + ", ";
                    }
                    nombreCliente = empresas.RazonSocial;
                    fechaDdMmNum = String.Format("{0:dd MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                    fechaDdMmNum += " de " + String.Format("{0:yyyy}", DateTime.Now);
                    fechaDdMm = cls.NumberToWords(Convert.ToInt32(String.Format("{0:dd}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                    fechaDdMm += " de " + String.Format("{0:MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                    fechaDdMm += " de " + cls.NumberToWords(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                    fechaPrimeraOperacion = cls.NumberToWords(Convert.ToInt32(String.Format("{0:dd}", simulacion.FechaPrimeraOp)));//HAY QUE FORMATEARLA BIEN
                    fechaPrimeraOperacion += " de " + String.Format("{0:MMMM}", simulacion.FechaPrimeraOp);//HAY QUE FORMATEARLA BIEN
                    fechaPrimeraOperacion += " de " + cls.NumberToWords(Convert.ToInt32(String.Format("{0:yyyy}", simulacion.FechaPrimeraOp)));//HAY QUE FORMATEARLA BIEN
                    tipoEmpresa = empresas.Tipo;//DEBE TRAER EL TIPO DE EMPRESA
                    giroComercial = empresas.Giro.ToLower();
                    string[] rutC = empresas.Rut.Replace(".", "").Split('-');
                    string rutCLetras = cls.NumberToWords(Convert.ToInt32(rutC[0]));
                    rutCLetras += " guion ";
                    int n;
                    bool isNumeric = int.TryParse(rutC[1], out n);
                    rutCLetras += (isNumeric) ? cls.NumberToWords(Convert.ToInt32(rutC[1])) : rutC[1];
                    rutCliente = rutCLetras;
                    datosRep = repDatos;
                    sustantivoDir = sust;
                    string[] dir = empresas.Direccion.Split(';');
                    SqlDataAdapter adapterD = GetAdapterDireccion(dir[0].ToString(), dir[1].ToString(), dir[2].ToString());
                    direccion = dir[3].ToString() + " " + GetDireccion(adapterD).LastOrDefault().Direcciones;
                    precioCesion = decimal.Parse(simulacion.PrecioCes.ToString()).ToString("N");
                    saldoPendiente = decimal.Parse(simulacion.MontoPendiente.ToString()).ToString("N");
                    salPendiente = simulacion.SaldoPendiente.ToString();
                    montoGir = decimal.Parse(simulacion.MontoGirable.ToString()).ToString("N");
                    deudor = nombreDeudor;
                    representante = represen;
                    //fechaEscritura = String.Format("{0:dd MMMM yyyy}",Convert.ToDateTime(empresas.FechaEscritura));
                    fechaEscritura = cls.NumberToWords(Convert.ToInt32(String.Format("{0:dd}", Convert.ToDateTime(empresas.FechaEscritura))));//HAY QUE FORMATEARLA BIEN
                    fechaEscritura += " de " + String.Format("{0:MMMM}", Convert.ToDateTime(empresas.FechaEscritura));//HAY QUE FORMATEARLA BIEN
                    fechaEscritura += " de " + cls.NumberToWords(Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(empresas.FechaEscritura))));//HAY QUE FORMATEARLA BIEN
                    notaria = empresas.NombreNotaria;
                    montoTotal = decimal.Parse(simulacion.MontoTotal.ToString()).ToString("N");
                    banco = datosBanco.Banco;
                    numCta = datosBanco.NumCuenta;
                    nombreDest = datosBanco.Destinatario;
                    rutDest = datosBanco.Rut;
                    numRutCliente = empresas.Rut;
                    //plazo = Sim.Plazo.ToString();
                    anticipo = Sim.Anticipo.ToString();
                    #endregion
                }
                else
                {
                    #region Documentos para Personas
                    if (nameTemplate == "CESION")
                    {
                        if (simulacion.Anticipo == 100)
                            nameTemplate = "CESION_P_100";
                        else
                            nameTemplate = "CESION_P_95";
                    }
                    if (nameTemplate == "MANDATO")
                    {
                        nameTemplate = "MANDATO_P";
                    }
                    if (nameTemplate == "AVALISTA")
                    {
                        nameTemplate = "AVALISTA_P";
                    }
                    if (nameTemplate == "PODER")
                    {
                        nameTemplate = "PODER_P";
                    }
                    if (nameTemplate == "UAF")
                    {
                        nameTemplate = "UAF_P";
                    }
                    if (nameTemplate == "CONTRATO_MARCO")
                    {
                        nameTemplate = "CONTRATO_MARCO_P";
                    }
                    SqlDataAdapter adapterP = GetAdapterSelPersona(cliente.Rut);
                    Persona persona = GetPersonas(adapterP).LastOrDefault();
                    string sinDon = "";
                    string edo = "";
                    if (persona.Sexo == "M")
                    {
                        sinDon = "don ";
                    }
                    else
                    {
                        sinDon = "doña ";
                    }
                    switch (persona.EdoCivil)
                    {
                        case "SO":
                            edo = "soltero";
                            break;
                        case "CA":
                            edo = "casado";
                            break;
                        case "VI":
                            edo = "viudo";
                            break;
                        case "DI":
                            edo = "divorciado";
                            break;
                        case "CC":
                            edo = "conviviente civil";
                            break;
                    }
                    string nombreDeudor = "";
                    foreach (var fact in facturas)
                    {
                        nombreDeudor += fact.Deudor + ", ";
                        numFact += fact.NumFactura + ", ";
                        dirDeudor += fact.DireccionDeudor + ", ";
                        comunaDeudor += fact.ComunaDeudor + ", ";
                    }
                    nombreCliente = persona.Nombre;
                    fechaDdMmNum = String.Format("{0:dd MMMM}", DateTime.Now);
                    fechaDdMmNum += " de " + String.Format("{0:yyyy}", DateTime.Now);
                    fechaDdMm = cls.NumberToWords(Convert.ToInt32(String.Format("{0:dd}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                    fechaDdMm += " de " + String.Format("{0:MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                    fechaDdMm += " de " + cls.NumberToWords(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                    string[] rutC = persona.Rut.Replace(".", "").Split('-');
                    string rutCLetras = cls.NumberToWords(Convert.ToInt32(rutC[0]));
                    rutCLetras += " guion ";
                    rutCLetras += cls.NumberToWords(Convert.ToInt32(rutC[1]));
                    rutCliente = rutCLetras;
                    string[] dir = persona.Direccion.Split(';');
                    SqlDataAdapter adapterD = GetAdapterDireccion(dir[0].ToString(), dir[1].ToString(), dir[2].ToString());
                    direccion = dir[3].ToString() + " " + GetDireccion(adapterD).LastOrDefault().Direcciones;
                    precioCesion = decimal.Parse(simulacion.PrecioCes.ToString()).ToString("N");
                    saldoPendiente = decimal.Parse(simulacion.MontoPendiente.ToString()).ToString("N");
                    salPendiente = simulacion.SaldoPendiente.ToString();
                    montoGir = decimal.Parse(simulacion.MontoGirable.ToString()).ToString("N");
                    deudor = nombreDeudor;
                    montoTotal = decimal.Parse(simulacion.MontoTotal.ToString()).ToString("N");
                    edoCivil = edo;
                    sustantivoPer = sinDon;
                    nacionalidad = persona.Nacionalidad;
                    numRutCliente = persona.Rut;
                    banco = datosBanco.Banco;
                    numCta = datosBanco.NumCuenta;
                    nombreDest = datosBanco.Destinatario;
                    rutDest = datosBanco.Rut;
                    //plazo = Sim.Plazo.ToString();
                    anticipo = Sim.Anticipo.ToString();
                    #endregion
                    //SqlDataAdapter adapterP = getada
                }
                if (cartaNotifica)
                {
                    numFact = numFacConcat;
                    dirDeudor = dirDeud;
                    comunaDeudor = comunDeud;
                }
                ReplaceWord(nameTemplate, nombreCliente, fechaDdMm, tipoEmpresa, giroComercial, rutCliente, datosRep, sustantivoDir, direccion, precioCesion, deudor, representante, saldoPendiente, fechaEscritura, notaria, montoTotal, rutRep, nacionalidad, edoCivil, sustantivoPer, numRutCliente, banco, numCta, facturas, plazo, anticipo, salPendiente, montoGir, nombreDest, rutDest, fechaDdMmNum, numFact, dirDeudor, comunaDeudor, rutLetras2, fechaPrimeraOperacion);
                //if (nameTemplate == "CESION_100" || nameTemplate == "CESION_95" || nameTemplate == "CESION_P_100" || nameTemplate == "CESION_P_95")
                //{
                //    ifr1.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                //if (nameTemplate == "AVALISTA" || nameTemplate == "AVALISTA_P")
                //{
                //    ifr2.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                //if (nameTemplate == "PODER" || nameTemplate == "PODER_P")
                //{
                //    ifr3.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                //if (nameTemplate == "CONTRATO_MARCO" || nameTemplate == "CONTRATO_MARCO_P")
                //{
                //    ifr4.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                //if (nameTemplate == "UAF" || nameTemplate == "UAF_P")
                //{
                //    ifr5.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                //if (nameTemplate == "MANDATO" || nameTemplate == "MANDATO_P")
                //{
                //    ifr6.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                if (nameTemplate == "CARTA_GUIA" || nameTemplate == "CARTA_GUIA_P")
                {
                    ifr7.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                }
                //if (nameTemplate == "DATOS_TRANSFERENCIAS")
                //{
                //    ifr8.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                //}
                if (nameTemplate != "CARTA_GUIA" && nameTemplate != "CARTA_GUIA_P")
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 100000);
                    footIfr.Text += "<iframe id='" + nameTemplate + "_" + randomNumber + "' frameborder='0' style='height:10px;' src='../Handler/DownloadFile.ashx?fname=" + nameTemplate + "'></iframe>";
                    //ifr9.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
                }
            }
            
        }

        public void ReplaceWord(string nameTemplate, string nombreCliente, string fechaDdMm, string tipoEmpresa, string giroComercial, string rutCliente, string datosRep, string sustantivoDir, string direccion, string precioCesion, string deudor, string representante, string saldoPendiente, string fechaEscr, string notaria, string montoTotal, string rutRep, string nacionalidad, string edoCivil, string sustantivoPer, string numRutCliente, string banco, string numCta, List<Facturas> facturas, string plazo, string anticipo, string salPendiente, string montoGir, string nombreDest, string rutDest, string fechaNum, string numFact, string dirDeudor, string comunaDeudor, string rutRepLetras,string fechaPrimeraOpe)
        {
            //Templates
            DocX document = DocX.Load(Server.MapPath("../App_Data/Template/" + nameTemplate + ".docx"));
            #region tratamiento de datos para representantes legales
            edoCivil = edoCivil.Trim().TrimEnd(',');
            nacionalidad = nacionalidad.Trim().TrimEnd(',');
            representante = representante.Trim().TrimEnd(',');
            rutRep = rutRep.Trim().TrimEnd(',');
            rutRepLetras = rutRepLetras.Trim().TrimEnd(',');
            string edoCivilRep1 = "";
            string edoCivilRep2 = "";
            string edoCivilRep3 = "";
            string edoCivilRep4 = "";
            string nacionalidadRep1 = "";
            string nacionalidadRep2 = "";
            string nacionalidadRep3 = "";
            string nacionalidadRep4 = "";
            string nombreRep1 = "";
            string nombreRep2 = "";
            string nombreRep3 = "";
            string nombreRep4 = "";
            string rutRep1 = "";
            string rutRep2 = "";
            string rutRep3 = "";
            string rutRep4 = "";
            string rutRepLetras1 = "";
            string rutRepLetras2 = "";
            string rutRepLetras3 = "";
            string rutRepLetras4 = "";
            string[] edoCivil0 = null;
            string[] nacionalidad0 = null;
            string[] represnatnte0 = null;
            string[] rut0 = null;
            string[] rutLetras0 = null;

            if (rutRep.Contains(","))
            {
                edoCivil0 = edoCivil.Split(',');
                nacionalidad0 = nacionalidad.Split(',');
                represnatnte0 = representante.Split(',');
                rut0 = rutRep.Split(',');
                rutLetras0 = rutRepLetras.Split(',');
            }
            else 
            {
                edoCivil0 = new string[] {edoCivil};
                nacionalidad0 = new string[] {nacionalidad};
                represnatnte0 = new string[] {representante};
                rut0 = new string[] {rutRep};
                rutLetras0 = new string[] {rutRepLetras};
            }

            switch (edoCivil0.Count()) 
            {
                case 1:
                    edoCivilRep1 = edoCivil0[0];
                    nacionalidadRep1 = nacionalidad0[0];
                    nombreRep1 = represnatnte0[0];
                    rutRep1 = rut0[0];
                    rutRepLetras1 = rutLetras0[0];
                    break;
                case 2:
                    edoCivilRep1 = edoCivil0[0];
                    nacionalidadRep1 = nacionalidad0[0];
                    nombreRep1 = represnatnte0[0];
                    rutRep1 = rut0[0];
                    rutRepLetras1 = rutLetras0[0];
                    edoCivilRep2 = edoCivil0[1];
                    nacionalidadRep2 = nacionalidad0[1];
                    nombreRep2 = represnatnte0[1];
                    rutRep2 = rut0[1];
                    rutRepLetras2 = rutLetras0[1];
                    break;
                case 3:
                    edoCivilRep1 = edoCivil0[0];
                    nacionalidadRep1 = nacionalidad0[0];
                    nombreRep1 = represnatnte0[0];
                    rutRep1 = rut0[0];
                    rutRepLetras1 = rutLetras0[0];
                    edoCivilRep2 = edoCivil0[1];
                    nacionalidadRep2 = nacionalidad0[1];
                    nombreRep2 = represnatnte0[1];
                    rutRep2 = rut0[1];
                    rutRepLetras2 = rutLetras0[1];
                    edoCivilRep3 = edoCivil0[2];
                    nacionalidadRep3 = nacionalidad0[2];
                    nombreRep3 = represnatnte0[2];
                    rutRep3 = rut0[2];
                    rutRepLetras3 = rutLetras0[2];
                    break;
                case 4:
                    edoCivilRep1 = edoCivil0[0];
                    nacionalidadRep1 = nacionalidad0[0];
                    nombreRep1 = represnatnte0[0];
                    rutRep1 = rut0[0];
                    rutRepLetras1 = rutLetras0[0];
                    edoCivilRep2 = edoCivil0[1];
                    nacionalidadRep2 = nacionalidad0[1];
                    nombreRep2 = represnatnte0[1];
                    rutRep2 = rut0[1];
                    rutRepLetras2 = rutLetras0[1];
                    edoCivilRep3 = edoCivil0[2];
                    nacionalidadRep3 = nacionalidad0[2];
                    nombreRep3 = represnatnte0[2];
                    rutRep3 = rut0[2];
                    rutRepLetras3 = rutLetras0[2];
                    edoCivilRep4 = edoCivil0[3];
                    nacionalidadRep4 = nacionalidad0[3];
                    nombreRep4 = represnatnte0[3];
                    rutRep4 = rut0[3];
                    rutRepLetras4 = rutLetras0[3];
                    break;
            }
            #endregion
            #region Proceso Empresa
            //if (nameTemplate == "CESION_95" || nameTemplate == "CESION_100")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<Fechaddmm>>", fechaNum);
            //    document.ReplaceText("<<TipoEmpresa>>", tipoEmpresa);
            //    document.ReplaceText("<<GiroComercial>>", giroComercial);
            //    document.ReplaceText("<<RutCliente>>", numRutCliente);
            //    document.ReplaceText("<<DatosRepresentantes>>", datosRep);
            //    document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //    document.ReplaceText("<<PrecioCesion>>", precioCesion);
            //    document.ReplaceText("<<Deudor>>", deudor);
            //    document.ReplaceText("<<RepresentanteLegal>>", representante);
            //    if (nameTemplate == "CESION_95")
            //    {
            //        document.ReplaceText("<<Anticipo>>", anticipo);
            //        document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
            //    }
            //}
            //if (nameTemplate == "CONTRATO_MARCO") 
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<Fechaddmm>>", fechaDdMm);
            //    document.ReplaceText("<<DatosRepresentantes>>", datosRep);
            //    document.ReplaceText("<<TipoEmpresa>>", tipoEmpresa);
            //    document.ReplaceText("<<GiroComercial>>", giroComercial);
            //    document.ReplaceText("<<RutCliente>>", rutCliente);
            //    document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //    document.ReplaceText("<<RepresentanteLegal>>", representante);
            //    document.ReplaceText("<<FechaEscritura>>", fechaEscr);
            //    document.ReplaceText("<<Notaria>>", notaria);
            //}
            //if (nameTemplate == "AVALISTA")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<MontoTotal>>", montoTotal);
            //}
            //if (nameTemplate == "MANDATO")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<DatosRepresentantes>>", datosRep);
            //    document.ReplaceText("<<RutCliente>>", numRutCliente);
            //    document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //}
            //if (nameTemplate == "PODER")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<DatosRepresentantes>>", datosRep);
            //    document.ReplaceText("<<RutCliente>>", rutCliente);
            //}
            //if (nameTemplate == "UAF")
            //{
            //    document.ReplaceText("<<RepresentanteLegal>>", representante);
            //    document.ReplaceText("<<RutRep>>", rutRep);
            //    document.ReplaceText("<<Nacionalidad>>", nacionalidad);
            //    document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
            //}
            #endregion
            #region Proceso Personas
            //if (nameTemplate == "CESION_P_95" || nameTemplate == "CESION_P_100")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<Fechaddmm>>", fechaNum);
            //    document.ReplaceText("<<Nacionalidad>>", nacionalidad);
            //    document.ReplaceText("<<EdoCivil>>", edoCivil);
            //    document.ReplaceText("<<RutCliente>>", numRutCliente);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //    document.ReplaceText("<<PrecioCesion>>", precioCesion);
            //    document.ReplaceText("<<Deudor>>", deudor);
            //    if (nameTemplate == "CESION_P_95")
            //    {
            //        document.ReplaceText("<<Anticipo>>", anticipo);
            //        document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
            //    }
            //}
            //if (nameTemplate == "AVALISTA_P")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<MontoTotal>>", montoTotal);
            //}
            //if (nameTemplate == "MANDATO_P")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<SustantivoPer>>", sustantivoPer);
            //    document.ReplaceText("<<RutCliente>>", numRutCliente);
            //    document.ReplaceText("<<Nacionalidad>>", nacionalidad);
            //    document.ReplaceText("<<EdoCivil>>", edoCivil);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //}
            //if (nameTemplate == "PODER_P")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<RutCliente>>", rutCliente);
            //}
            //if (nameTemplate == "UAF_P")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<RutCliente>>", numRutCliente);
            //    document.ReplaceText("<<Nacionalidad>>", nacionalidad);
            //    document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
            //}
            //if (nameTemplate == "CONTRATO_MARCO_P")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<Fechaddmm>>", fechaDdMm);
            //    document.ReplaceText("<<SustantivoPer>>", sustantivoPer);
            //    document.ReplaceText("<<RutCliente>>", rutCliente);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //    document.ReplaceText("<<Nacionalidad>>", nacionalidad);
            //    document.ReplaceText("<<EdoCivil>>", edoCivil);
            //}

            #endregion
            if (nameTemplate == "CARTA_GUIA")
            {
                document.ReplaceText("<<NombreCliente>>", nombreCliente);
                document.ReplaceText("<<RutCliente>>", numRutCliente);
                document.ReplaceText("<<Direccion>>", direccion);
                document.ReplaceText("<<Banco>>", banco);
                document.ReplaceText("<<NumCta>>", numCta);
                document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
                document.ReplaceText("<<MontoTotal>>", montoTotal);

                int num = 0;
                foreach (var fact in facturas)
                {
                    num++;
                    int plz = fact.Plazo;
                    DateTime fechaVenc = DateTime.Now.AddDays(plz);
                    document.ReplaceText("<<NumFac"+num+">>", fact.NumFactura);
                    document.ReplaceText("<<RutDeudor" + num + ">>", fact.RutDeudor);
                    document.ReplaceText("<<Deudor" + num + ">>", fact.Deudor);
                    document.ReplaceText("<<MontoFac" + num + ">>", fact.Monto.ToString("N"));
                    document.ReplaceText("<<Dia" + num + ">>", String.Format("{0:dd}", fechaVenc));
                    document.ReplaceText("<<Mes" + num + ">>", String.Format("{0:MM}", fechaVenc));
                    document.ReplaceText("<<Ano" + num + ">>", String.Format("{0:yyyy}", fechaVenc));
                }
                if (num < 31) 
                {
                    while(num<=31)
                    {
                        num++;
                        document.ReplaceText("<<NumFac" + num + ">>", "");
                        document.ReplaceText("<<RutDeudor" + num + ">>", "");
                        document.ReplaceText("<<Deudor" + num + ">>", "");
                        document.ReplaceText("<<MontoFac" + num + ">>", "");
                        document.ReplaceText("<<Dia" + num + ">>", "");
                        document.ReplaceText("<<Mes" + num + ">>", "");
                        document.ReplaceText("<<Ano" + num + ">>", "");
                    }
                }
            }

            //if (nameTemplate == "DATOS_TRANSFERENCIAS")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreDest);
            //    document.ReplaceText("<<RutCliente>>", rutDest);
            //    document.ReplaceText("<<Banco>>", banco);
            //    document.ReplaceText("<<NumCta>>", numCta);
            //    document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
            //    document.ReplaceText("<<MontoGir>>", montoGir);
            //    document.ReplaceText("<<SalPendiente>>", salPendiente);
            //    document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
            //}
            #region Template AUTOMATICA
            //if (nameTemplate != "CESION_100" && nameTemplate != "CESION_95" && nameTemplate != "CESION_P_100" && nameTemplate != "CESION_P_95" &&
            //    nameTemplate != "AVALISTA" && nameTemplate != "AVALISTA_P" && nameTemplate != "PODER" && nameTemplate != "PODER_P" &&
            //    nameTemplate != "CONTRATO_MARCO" && nameTemplate != "CONTRATO_MARCO_P" && nameTemplate != "UAF" && nameTemplate != "UAF_P" &&
            //    nameTemplate != "MANDATO" && nameTemplate != "MANDATO_P" && nameTemplate != "CARTA_GUIA" && nameTemplate != "CARTA_GUIA_P" &&
            //    nameTemplate != "DATOS_TRANSFERENCIAS")
            if (nameTemplate != "CARTA_GUIA" && nameTemplate != "CARTA_GUIA_P")
            {
                document.ReplaceText("<<NombreClienteDestino>>", nombreDest);
                document.ReplaceText("<<NombreCliente>>", nombreCliente);
                document.ReplaceText("<<RutClienteDestino>>", rutDest);
                document.ReplaceText("<<RutClienteNumero>>", numRutCliente);
                document.ReplaceText("<<RutClienteLetras>>", rutCliente);
                document.ReplaceText("<<FechaddmmNumero>>", fechaNum);
                document.ReplaceText("<<Fechaddmm>>", fechaDdMm);
                document.ReplaceText("<<Banco>>", banco);
                document.ReplaceText("<<NumCta>>", numCta);
                document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
                document.ReplaceText("<<MontoGir>>", montoGir);
                document.ReplaceText("<<SalPendiente>>", salPendiente);
                document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
                document.ReplaceText("<<TipoEmpresa>>", tipoEmpresa);
                document.ReplaceText("<<GiroComercial>>", giroComercial);
                document.ReplaceText("<<DatosRepresentantes>>", datosRep);
                document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
                document.ReplaceText("<<Direccion>>", direccion);
                document.ReplaceText("<<PrecioCesion>>", precioCesion);
                document.ReplaceText("<<Deudor>>", deudor);
                document.ReplaceText("<<FechaEscritura>>", fechaEscr);
                document.ReplaceText("<<Notaria>>", notaria);
                document.ReplaceText("<<SustantivoPer>>", sustantivoPer);
                document.ReplaceText("<<MontoTotal>>", montoTotal);
                document.ReplaceText("<<Anticipo>>", anticipo);
                document.ReplaceText("<<NumeroFactura>>", numFact);
                document.ReplaceText("<<DireccionDeudor>>", dirDeudor);
                document.ReplaceText("<<ComunaDeudor>>", comunaDeudor);
                document.ReplaceText("<<DiaSemana>>", DateTime.Now.ToString("dddd", new CultureInfo("es-ES")));
                document.ReplaceText("<<fechaPrimeraOperacion>>", fechaPrimeraOpe);
                document.ReplaceText("<<rutRepresentanteLetras>>", rutRepLetras);
                document.ReplaceText("<<RutRep>>", rutRep);
                document.ReplaceText("<<Nacionalidad>>", nacionalidad);
                document.ReplaceText("<<EdoCivil>>", edoCivil);
                document.ReplaceText("<<RepresentanteLegal>>", representante);
                document.ReplaceText("<<rutRepresentanteLetras1>>", rutRepLetras1);
                document.ReplaceText("<<RutRep1>>", rutRep1);
                document.ReplaceText("<<Nacionalidad1>>", nacionalidadRep1);
                document.ReplaceText("<<EdoCivil1>>", edoCivilRep1);
                document.ReplaceText("<<RepresentanteLegal1>>", nombreRep1);
                document.ReplaceText("<<rutRepresentanteLetras2>>", rutRepLetras2);
                document.ReplaceText("<<RutRep2>>", rutRep2);
                document.ReplaceText("<<Nacionalidad2>>", nacionalidadRep2);
                document.ReplaceText("<<EdoCivil2>>", edoCivilRep2);
                document.ReplaceText("<<RepresentanteLegal2>>", nombreRep2);
                document.ReplaceText("<<rutRepresentanteLetras3>>", rutRepLetras3);
                document.ReplaceText("<<RutRep3>>", rutRep3);
                document.ReplaceText("<<Nacionalidad3>>", nacionalidadRep3);
                document.ReplaceText("<<EdoCivil3>>", edoCivilRep3);
                document.ReplaceText("<<RepresentanteLegal3>>", nombreRep3);
                document.ReplaceText("<<rutRepresentanteLetras4>>", rutRepLetras4);
                document.ReplaceText("<<RutRep4>>", rutRep4);
                document.ReplaceText("<<Nacionalidad4>>", nacionalidadRep4);
                document.ReplaceText("<<EdoCivil4>>", edoCivilRep4);
                document.ReplaceText("<<RepresentanteLegal4>>", nombreRep4);
            }
            #endregion
            document.SaveAs(Server.MapPath("../App_Data/Downloads/" + nameTemplate + ".docx"));
            document.Dispose();
        }

        public SqlDataAdapter GetAdapterClientes(string param, string valParam, string tipoParam)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
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

        public SqlDataAdapter GetAdapterNotaria()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DET_NOTARIA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_NOTARIA", "1");
                ArrayList tipoDatos = new ArrayList();
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

        public SqlDataAdapter GetAdapterRepresentantes(string idEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_EMPRESAS_REPRESENTANTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", idEmpresa);
                ArrayList tipoDatos = new ArrayList();
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
        
        public SqlDataAdapter GetAdapterDireccion(string idRegion,string idCiudad,string idComuna)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DIRECCION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_REGION", idRegion);
                parametros.Add("@ID_CIUDAD", idCiudad);
                parametros.Add("@ID_COMUNA", idComuna);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
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

        public SqlDataAdapter GetAdapterSelEmpresa(string rut)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_EMPRESAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT", rut);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterDocumentos(string funcionalidad)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@FUNCIONALIDAD", funcionalidad);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterSelPersona(string rut)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PERSONAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT", rut);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public List<Direccion> GetDireccion(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Direccion> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Direccion()
                  {
                      Direcciones = item.Field<string>("DIRECCION")
                  }).ToList();
            return re;
        }

        public List<Documentos> GetDocumentos(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Documentos> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Documentos()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre"),
                      Template = item.Field<string>("Template"),
                      Funcionalidad = item.Field<string>("funcionalidad")
                  }).ToList();
            return re;
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

        public List<Persona> GetPersonas(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Persona> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Persona()
                  {
                      ID = item.Field<int>("ID"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      Profesion = item.Field<string>("Profesion"),
                      Nacionalidad = item.Field<string>("Nacionalidad"),
                      EdoCivil = item.Field<string>("Edo_civil"),
                      Sexo = item.Field<string>("Sexo"),
                      Email = item.Field<string>("Email"),
                      Direccion = item.Field<string>("Direccion"),
                      Telefono = item.Field<string>("Telefono"),
                      Celular = item.Field<string>("Celular")
                  }).ToList();
            return re;
        }

        public List<Empresa> GetEmpresa(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Empresa> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Empresa()
                  {
                      ID = item.Field<int>("ID"),
                      IdTipoEmpresa = item.Field<int>("Id_tipo_Empresa"),
                      Tipo = item.Field<string>("Tipo"),
                      Rut = item.Field<string>("Rut"),
                      RazonSocial = item.Field<string>("Razon_Social"),
                      Giro = item.Field<string>("Giro_Comercial"),
                      NombreNotaria = item.Field<string>("Nombre_Notaria"),
                      FechaEscritura = item.Field<DateTime>("Fecha_Escritura").ToString(),
                      Direccion = item.Field<string>("Direccion"),
                      Telefono = item.Field<string>("Telefono")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterSelDatosBanco(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DATOS_BANCO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente);
                ArrayList tipoDatos = new ArrayList();
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

        public List<DatosBanco> GetDatosBanco(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<DatosBanco> re = null;
            re = (from item in dt.AsEnumerable()
                  select new DatosBanco()
                  {
                      ID = item.Field<int>("ID"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      IdBanco = item.Field<int>("ID_BANCO"),
                      Banco = item.Field<string>("Nombre"),
                      Rut = item.Field<string>("Rut"),
                      Destinatario = item.Field<string>("Destinatario"),
                      NumCuenta = item.Field<string>("NUM_CUENTA"),
                      Email = item.Field<string>("CORREO")
                  }).ToList();
            return re;
        }

        public List<DetalleNotaria> GetDetalleNotaria(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<DetalleNotaria> re = null;
            re = (from item in dt.AsEnumerable()
                  select new DetalleNotaria()
                  {
                      ID = item.Field<int>("ID"),
                      Descripcion = item.Field<string>("Descripcion"),
                      Valor = item.Field<double>("valor")
                  }).ToList();
            return re;
        }


    }
}