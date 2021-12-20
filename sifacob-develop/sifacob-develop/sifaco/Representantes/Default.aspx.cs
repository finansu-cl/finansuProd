using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace sifaco.Representantes
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Request.QueryString["empid"] != null)
                {
                    string id = Request.QueryString["empid"].ToString();
                    SqlDataAdapter adapter = GetAdapter(id);
                    List<Persona> sourcePersona = GetPersonas(adapter);
                    rptGrid.DataSource = sourcePersona;
                    rptGrid.DataBind();

                    if (sourcePersona.Count > 0) 
                    {
                        btnSimular.Enabled = true;
                    }
                }

                if (Request.QueryString["nemp"] != null)
                    ltrNameEmp.Text = Request.QueryString["nemp"].ToString();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = Request.QueryString["empid"].ToString();
                string nombre = Request.QueryString["nemp"].ToString();
                string idCliente = Request.QueryString["cid"].ToString();
                int idRepresentante = InsertClientePersona(per.Rut, per.Nombre, per.EdoCivil,per.Sexo, per.Profesion,per.Nacionalidad, "-1;-1;-1;", per.Email, per.Telefono, per.Celular, id); 
                SqlDataAdapter adapter = GetAdapter(id);
                List<Persona> sourcePersona = GetPersonas(adapter);
                rptGrid.DataSource = sourcePersona;
                rptGrid.DataBind();
                if (sourcePersona.Count > 0)
                {
                    btnSimular.Enabled = true;
                }
                LimpiarForm();
                Response.Redirect("../Representantes/?empid=" + id + "&nemp=" + nombre + "&cid=" + idCliente);
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

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = Request.QueryString["empid"].ToString();
                string nombre = Request.QueryString["nemp"].ToString();
                string idCliente = Request.QueryString["cid"].ToString();
                EditarCliente(per.Rut, per.Nombre, per.EdoCivil,per.Sexo, per.Profesion,per.Nacionalidad, "-1;-1;-1;", per.Email, per.Telefono, per.Celular, per.ID, id);
                SqlDataAdapter adapter = GetAdapter(id);
                List<Persona> sourcePersona = GetPersonas(adapter);
                rptGrid.DataSource = sourcePersona;
                rptGrid.DataBind();
                if (sourcePersona.Count > 0)
                {
                    btnSimular.Enabled = true;
                }
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                LimpiarForm();
                Response.Redirect("../Representantes/?empid=" + id + "&nemp=" + nombre + "&cid=" + idCliente);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LimpiarForm();
            btnAceptar.Visible = true;
            btnModificar.Visible = false;
        }

        public int InsertClientePersona(string rut, string nombre, string edoCivil, string sexo, string profesion,string nacionalidad, string direccion, string email, string telefono, string celular, string idEmpresa) 
        {
            int id = 0;
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Personas
                string spName = "usr_fnns.SP_CREATE_PERSONAS";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("@RUT", rut);
                    parametros.Add("@NOMBRE", nombre);
                    parametros.Add("@EDO_CIVIL", edoCivil);
                    parametros.Add("@SEXO", sexo);
                    parametros.Add("@NACIONALIDAD", nacionalidad);
                    parametros.Add("@PROFESION", profesion);
                    parametros.Add("@DIRECCION", direccion);
                    parametros.Add("@EMAIL", email);
                    parametros.Add("@TELEFONO", telefono);
                    parametros.Add("@CELULAR", celular);
                    parametros.Add("@NAME_USER", Session["login"].ToString());
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                    DataTable dt = new DataTable();
                    reader.Fill(dt);
                    int idPersona = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
                #region Parametros Cliente
                    string spName1 = "usr_fnns.SP_CREATE_REPRESENTANTE";
                    Dictionary<string, string> parametros1 = new Dictionary<string, string>();
                    parametros1.Add("@ID_PERSONA", idPersona.ToString());
                    parametros1.Add("@ID_EMPRESA", idEmpresa.ToString());
                    parametros1.Add("@NAME_USER", Session["login"].ToString());
                    ArrayList tipoDatos1 = new ArrayList();
                    tipoDatos1.Add("int");
                    tipoDatos1.Add("int");
                    tipoDatos1.Add("varchar");
                    SqlDataAdapter reader1 = cls.GetConnectionToDb(conn, spName1, parametros1, tipoDatos1);
                    DataTable dt1 = new DataTable();
                    reader1.Fill(dt1);
                    int idRepresentante = Convert.ToInt32(dt1.Rows[0]["ID"]);
                #endregion
                return idRepresentante;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return id;
            }
        }

        public SqlDataAdapter GetAdapter(string idEmpresa)
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

        public void EditarCliente(string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string id, string idEmpresa)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_PERSONAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id);
                parametros.Add("@RUT", rut);
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@EDO_CIVIL", edoCivil); 
                parametros.Add("@SEXO", sexo);
                parametros.Add("@PROFESION", profesion);
                parametros.Add("@NACIONALIDAD", nacionalidad);
                parametros.Add("@DIRECCION", direccion);
                parametros.Add("@EMAIL", email);
                parametros.Add("@TELEFONO", telefono);
                parametros.Add("@CELULAR", celular);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "REPRESENTANTES");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
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

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_REPRESENTANTE";
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

        protected void rptGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                // e.CommandArgument;
                try
                {
                    tab1.Attributes.Remove("class");
                    tab2.Attributes.Add("class", "active");
                    timeline.Attributes.Remove("class");
                    timeline.Attributes.Add("class", "tab-pane");
                    activity.Attributes.Remove("class");
                    activity.Attributes.Add("class", "active tab-pane");
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string rut = arg[0].ToString();
                    string idCliente = arg[1].ToString();
                    SqlDataAdapter adapter = GetAdapterSelPersona(rut);
                    var persona = GetPersonas(adapter).FirstOrDefault();
                    per.ID = persona.ID.ToString();
                    per.Rut = persona.Rut;
                    per.Nombre = persona.Nombre;
                    per.EdoCivil = persona.EdoCivil;
                    per.Sexo = persona.Sexo;
                    per.Profesion = persona.Profesion;
                    per.Nacionalidad = persona.Nacionalidad;
                    per.Email = persona.Email;
                    per.Telefono = persona.Telefono;
                    per.Celular = persona.Celular;
                    btnModificar.Visible = true;
                    btnAceptar.Visible = false;

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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ltHidden.Text;
                if (id != "")
                {
                    GetAdapterNonQuery(id);
                    if (Request.QueryString["empid"] != null)
                    {
                        string idEmp = Request.QueryString["empid"].ToString();
                        SqlDataAdapter adapter = GetAdapter(idEmp);
                        List<Persona> sourcePersona = GetPersonas(adapter);
                        rptGrid.DataSource = sourcePersona;
                        rptGrid.DataBind();
                        if (sourcePersona.Count > 0)
                        {
                            btnSimular.Enabled = true;
                        }
                        else
                        {
                            btnSimular.Enabled = false;
                        }
                    }
                }
                plhDeleteQuestion.Visible = false;
                ltHidden.Text = "";
                Response.Redirect("../Representantes/");
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

        public void LimpiarForm() 
        {
            per.Rut = "";
            per.Nombre = "";
            per.EdoCivil="SO";
            per.Sexo = "M";
            per.Profesion="";
            per.Nacionalidad = "";
            per.Email=""; 
            per.Telefono=""; 
            per.Celular=""; 
            per.ID = "0";
        }

        protected void btnSimular_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                int idSim = GetAdapterCreateSim(Convert.ToInt32(idCliente), 1, 3, 95, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                ActualizarNumOperacion(idCliente, "1");
                Response.Redirect("../Factoring/?cid=" + idCliente+"&sid="+idSim.ToString());
            }
        }

        public int GetAdapterCreateSim(int idCliente, int edoSim, decimal tasa, int anticipo, int salPendiente, int plazo, decimal gastosOpe, decimal utilidad, decimal preCesion, decimal montoGir, decimal montoAnt, decimal montoPend, decimal montoTotal, decimal comision, decimal iva)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_CREATE_SIMULACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@ID_EDO_SIM", edoSim.ToString());
                parametros.Add("@TASA", tasa.ToString().Replace(".", ""));
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
                parametros.Add("@COMISION", montoPend.ToString().Replace(".", ""));
                parametros.Add("@IVA", montoPend.ToString().Replace(".", ""));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
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
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idSimulacion = Convert.ToInt32(dt.Rows[0]["ID"]);
                return idSimulacion;
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


    }


}