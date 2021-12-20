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

namespace sifaco.Simulacion
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "4";
            Common cls = new Common();
            if (!IsPostBack)
            {
                SqlDataAdapter adapterC = GetAdapterCliente();
                ddlCliente.DataSource = GetClientes(adapterC);
                ddlCliente.DataTextField = "Nombre";
                ddlCliente.DataValueField = "ID";
                ddlCliente.DataBind();

                SqlDataAdapter adapter2 = GetAdapterSimulaciones();
                rptSimulaciones.DataSource = GetSimulaciones(adapter2);
                rptSimulaciones.DataBind();

                //if (Session["rol"] != null && Session["rol"].ToString() == "user")
                //{
                //    foreach (RepeaterItem item in rptSimulaciones.Items)
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
                //    foreach (RepeaterItem item in rptSimulaciones.Items)
                //    {
                //        Button btnDelete = (Button)item.FindControl("btnDelete");
                //        btnDelete.Enabled = false;
                //        btnDelete.ToolTip = "Usted No posee permisos para modificar";
                //    }
                //}

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
                    Response.Redirect("../Factoring/?cid=" + idCliente + "&sid=" + id+"&flag=true");

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

        public SqlDataAdapter GetAdapterSimulaciones()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_SIMULACIONES";
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
                      Tasa = item.Field<decimal?>("TASA")??0,
                      Anticipo = item.Field<decimal?>("Anticipo") ?? 0,
                      SaldoPendiente = item.Field<decimal?>("SALDO_PENDIENTE") ?? 0,
                      //Plazo = item.Field<int>("PLAZO"),
                      GastosOper = item.Field<decimal?>("GASTOS_OPERACION") ?? 0,
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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ltHidden.Text;
                string idCliente = ltHidden2.Text;
                if (id != "")
                {
                    GetAdapterNonQuery(id);
                    ActualizarNumOperacion(idCliente, "0");
                    SqlDataAdapter adapter2 = GetAdapterSimulaciones();
                    rptSimulaciones.DataSource = GetSimulaciones(adapter2);
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
                string spName = "usr_fnns.SP_DEL_SIMULACION";
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

        public void ActualizarNumOperacion(string Id, string numOper)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_NUM_OPERACION_CLIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", Id);
                parametros.Add("@BOOL", numOper);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "NUMERO OPERACION");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterCliente()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES_SIMULACION";
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

        public SqlDataAdapter GetAdapterSimulacionesFiltro(DateTime fechaD, DateTime fechaH, int idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_SIMULACIONES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                tipoDatos.Add("int");
                parametros.Add("@FECHA_INI", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_FIN", fechaH.ToString());
                tipoDatos.Add("datetime");
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);
            int idCliente = Convert.ToInt32(ddlCliente.SelectedValue.ToString());

            if (txtFechaD.Text == "" && txtFechaH.Text == "" && idCliente < 0)
                Response.Redirect(Request.Url.ToString());
            else
            {
                SqlDataAdapter adapter = GetAdapterSimulacionesFiltro(fechaD, fechaH, idCliente);
                rptSimulaciones.DataSource = GetSimulaciones(adapter).OrderByDescending(x => x.Fecha).ToList();
                rptSimulaciones.DataBind();
            }
        }

    }
}