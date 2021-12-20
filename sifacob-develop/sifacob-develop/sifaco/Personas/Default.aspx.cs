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

namespace sifaco.Personas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "1";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetClientes(adapter);
                rptGrid.DataBind();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string direccion = per.Region + ";" + per.Ciudad + ";" + per.Comuna + ";" + per.Direccion;
                int idCliente = InsertClientePersona(per.Rut, per.Nombre, per.EdoCivil, per.Sexo, per.Profesion, per.Nacionalidad, direccion, per.Email, per.Telefono, per.Celular, DatosBanco.Bancos, DatosBanco.Destinatario, DatosBanco.RutDest, DatosBanco.NumCta, DatosAval.Rut, DatosAval.Nombre);
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetClientes(adapter);
                rptGrid.DataBind();
                LimpiarForm();
                Response.Redirect("../Personas/");
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

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                string direccion = per.Region + ";" + per.Ciudad + ";" + per.Comuna + ";" + per.Direccion;
                EditarCliente(per.Rut, per.Nombre, per.EdoCivil, per.Sexo, per.Profesion,per.Nacionalidad, direccion, per.Email, per.Telefono, per.Celular, DatosBanco.Bancos, DatosBanco.Destinatario, DatosBanco.RutDest, DatosBanco.NumCta, Convert.ToInt32(DatosBanco.IdCliente), Convert.ToInt32(per.ID), DatosAval.Rut, DatosAval.Nombre);
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetClientes(adapter);
                rptGrid.DataBind();
                LimpiarForm();
                Response.Redirect("../Personas/");
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

        public int InsertClientePersona(string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad,string direccion, string email, string telefono, string celular, string banco, string destinatario, string rutDest, string numCta, string rutAval, string nombreAval) 
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
                string spName1 = "usr_fnns.SP_CREATE_CLIENTES";
                Dictionary<string, string> parametros1 = new Dictionary<string, string>();
                parametros1.Add("@ID_PERSONA", idPersona.ToString());
                parametros1.Add("@ID_EMPRESA", "0");
                parametros1.Add("@NUM_OPERACION", "1");
                parametros1.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos1 = new ArrayList();
                tipoDatos1.Add("int");
                tipoDatos1.Add("int");
                tipoDatos1.Add("int");
                tipoDatos1.Add("varchar");
                SqlDataAdapter reader1 = cls.GetConnectionToDb(conn, spName1, parametros1, tipoDatos1);
                DataTable dt1 = new DataTable();
                reader1.Fill(dt1);
                int idCliente = Convert.ToInt32(dt1.Rows[0]["ID"]);
                #endregion
                #region Parametros Datos Banco
                string spName2 = "usr_fnns.SP_CREATE_DATOS_BANCO";
                Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                parametros2.Add("@ID_CLIENTE", idCliente.ToString());
                parametros2.Add("@ID_BANCO", banco);
                parametros2.Add("@NUM_CUENTA", numCta);
                parametros2.Add("@DESTINATARIO", destinatario);
                parametros2.Add("@RUT", rutDest);
                parametros2.Add("@CORREO", email);
                parametros2.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos2 = new ArrayList();
                tipoDatos2.Add("int");
                tipoDatos2.Add("int");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                SqlDataAdapter reader2 = cls.GetConnectionToDb(conn, spName2, parametros2, tipoDatos2);
                DataTable dt2 = new DataTable();
                reader2.Fill(dt2);
                int idDatosBanco = Convert.ToInt32(dt2.Rows[0]["ID"]);
                #endregion
                #region Parametros Datos Aval
                string spName3 = "usr_fnns.SP_CREATE_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@NOMBRE", nombreAval);
                parametros3.Add("@RUT", rutAval);
                parametros3.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos3 = new ArrayList();
                tipoDatos3.Add("int");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                SqlDataAdapter reader3 = cls.GetConnectionToDb(conn, spName3, parametros3, tipoDatos3);
                DataTable dt3 = new DataTable();
                reader3.Fill(dt3);
                int idDatosAval = Convert.ToInt32(dt3.Rows[0]["ID"]);
                #endregion
                return idCliente;
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

        public SqlDataAdapter GetAdapter()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT_EMPRESA","0");
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

        public SqlDataAdapter GetAdapterSelDatosAval(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_AVAL";
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

        public void EditarCliente(string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string banco, string destinatario, string rutDest, string numCta, int idCliente, int id, string rutAval, string nombreAval)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_PERSONAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
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
                parametros.Add("@ORIGEN", "PERSONAS");
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
                #region Parametros Datos Banco
                string spName2 = "SP_MOD_DATOS_BANCO";
                Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                parametros2.Add("@ID_CLIENTE", idCliente.ToString());
                parametros2.Add("@ID_BANCO", banco);
                parametros2.Add("@RUT", rutDest);
                parametros2.Add("@DESTINATARIO", destinatario);
                parametros2.Add("@NUM_CUENTA", numCta);
                parametros2.Add("@EMAIL", email);
                parametros2.Add("@NAME_USER", Session["login"].ToString());
                parametros2.Add("@ORIGEN", "PERSONAS");
                parametros2.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos2 = new ArrayList();
                tipoDatos2.Add("int");
                tipoDatos2.Add("int");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("int");
                cls.GetConnectionToDbNonQuery(conn, spName2, parametros2, tipoDatos2);
                #endregion
                #region Parametros Datos Aval
                string spName3 = "SP_MOD_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@RUT", rutAval);
                parametros3.Add("@NOMBRE", nombreAval);
                parametros3.Add("@NAME_USER", Session["login"].ToString());
                parametros3.Add("@ORIGEN", "PERSONAS");
                parametros3.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos3 = new ArrayList();
                tipoDatos3.Add("int");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("int");
                cls.GetConnectionToDbNonQuery(conn, spName3, parametros3, tipoDatos3);
                #endregion

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
                      IdCliente = item.Field<int?>("ID_CLIENTE"),
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
                      Rut = item.Field<string>("Rut"),
                      Destinatario = item.Field<string>("Destinatario"),
                      NumCuenta = item.Field<string>("NUM_CUENTA"),
                      Email = item.Field<string>("CORREO")
                  }).ToList();
            return re;
        }

        public List<Avales> GetDatosAval(SqlDataAdapter adapter)
        {
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                List<Avales> re = null;
                re = (from item in dt.AsEnumerable()
                      select new Avales()
                      {
                          ID = item.Field<int>("ID"),
                          IdCliente = item.Field<int>("ID_CLIENTE"),
                          Rut = item.Field<string>("Rut"),
                          Nombre = item.Field<string>("Nombre")
                      }).ToList();
                return re;
            }
            catch 
            {
                return null;
            }
        }

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_CLIENTE_PER";
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

            if (e.CommandName == "sim")
            {
                int idSim = GetAdapterCreateSim(Convert.ToInt32(e.CommandArgument.ToString()), 1, 3, 95, 5, 0, 0, 0, 0, 0, 0, 0, 0,0,0);
                ActualizarNumOperacion(e.CommandArgument.ToString(), "1");
                Response.Redirect("../Factoring/?cid=" + e.CommandArgument.ToString() + "&sid=" + idSim.ToString());
            }

            if (e.CommandName == "pres")
            {
                int idPres = GetAdapterCreatePrestamo(Convert.ToInt32(e.CommandArgument.ToString()), 0, 0, 0, 0);
                Response.Redirect("../Prestamo/?cid=" + e.CommandArgument.ToString() + "&pid=" + idPres.ToString());
            }

            if (e.CommandName == "edit")
            {
                // e.CommandArgument;
                try
                {
                    tab1.Attributes.Remove("class");
                    tab2.Attributes.Add("class","active");
                    timeline.Attributes.Remove("class");
                    timeline.Attributes.Add("class", "tab-pane");
                    activity.Attributes.Remove("class");
                    activity.Attributes.Add("class", "active tab-pane");
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string rut = arg[0].ToString();
                    string idCliente = arg[1].ToString();
                    DatosBanco.IdCliente = idCliente;
                    SqlDataAdapter adapter = GetAdapterSelPersona(rut);
                    SqlDataAdapter adapter2 = GetAdapterSelDatosBanco(idCliente);
                    SqlDataAdapter adapter3 = GetAdapterSelDatosAval(idCliente);
                    var persona = GetPersonas(adapter).FirstOrDefault();
                    var datosBanco = GetDatosBanco(adapter2).FirstOrDefault();
                    var datosAval = GetDatosAval(adapter3).FirstOrDefault();
                    per.ID = persona.ID.ToString();
                    per.Rut = persona.Rut;
                    per.Nombre = persona.Nombre;
                    per.EdoCivil = persona.EdoCivil;
                    per.Sexo = persona.Sexo;
                    per.Profesion = persona.Profesion;
                    per.Nacionalidad = persona.Nacionalidad;
                    per.Direccion = persona.Direccion;
                    per.Email = persona.Email;
                    per.Telefono = persona.Telefono;
                    per.Celular = persona.Celular;
                    DatosBanco.Bancos = datosBanco.IdBanco.ToString();
                    DatosBanco.NumCta = datosBanco.NumCuenta;
                    DatosBanco.Destinatario = datosBanco.Destinatario;
                    DatosBanco.RutDest = datosBanco.Rut;
                    DatosAval.Nombre = (datosAval == null) ? "" : datosAval.Nombre;
                    DatosAval.Rut = (datosAval == null) ? "" : datosAval.Rut;
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
                    SqlDataAdapter adapter = GetAdapter();
                    rptGrid.DataSource = GetClientes(adapter);
                    rptGrid.DataBind();
                    LimpiarForm();
                    Response.Redirect("../Personas/");
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

        public void LimpiarForm() 
        {
            per.Rut = "";
            per.Nombre = "";
            per.EdoCivil="SO";
            per.Sexo = "M";
            per.Direccion = "-1;-1;-1;";
            per.Profesion="";
            per.Nacionalidad = "";
            per.Nacionalidad = "";
            per.Email=""; 
            per.Telefono=""; 
            per.Celular=""; 
            DatosBanco.Bancos="-1"; 
            DatosBanco.Destinatario=""; 
            DatosBanco.RutDest=""; 
            DatosBanco.NumCta=""; 
            DatosBanco.IdCliente ="0";
            per.ID = "0";
            DatosAval.Nombre = "";
            DatosAval.Rut = "";
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

        public int GetAdapterCreatePrestamo(int idCliente, decimal tasa, int plazo, decimal monto, decimal cuota)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_CREATE_PRESTAMO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@TASA", tasa.ToString().Replace(".", ""));
                parametros.Add("@PLAZO", plazo.ToString());
                parametros.Add("@MONTO", monto.ToString().Replace(".", ""));
                parametros.Add("@CUOTA", cuota.ToString().Replace(".", ""));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idPrestamo = Convert.ToInt32(dt.Rows[0]["ID"]);
                return idPrestamo;
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
                parametros.Add("@ORIGEN", "REPRESENTANTES");
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