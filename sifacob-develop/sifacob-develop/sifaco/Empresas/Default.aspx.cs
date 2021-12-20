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

namespace sifaco.Empresas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "2";
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
                string direccion = emp.Region + ";" + emp.Ciudad + ";" + emp.Comuna + ";" + emp.Direccion;
                int idCliente = InsertClienteEmpresa(emp.TipoEmp, emp.Rut, emp.RazonSocial, emp.GiroComercial, emp.NombreNotaria, direccion,emp.Telefono, emp.FechaEscritura, DatosBanco.Bancos, DatosBanco.Destinatario, DatosBanco.RutDest, DatosBanco.NumCta,DatosAval.Rut, DatosAval.Nombre);
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetClientes(adapter);
                rptGrid.DataBind();
                LimpiarForm();
                Response.Redirect("../Empresas/");
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
                string direccion = emp.Region + ";" + emp.Ciudad + ";" + emp.Comuna + ";" + emp.Direccion;
                EditarCliente(emp.TipoEmp, emp.Rut, emp.RazonSocial, emp.GiroComercial, emp.NombreNotaria, direccion, emp.Telefono, emp.FechaEscritura, DatosBanco.Bancos, DatosBanco.Destinatario, DatosBanco.RutDest, DatosBanco.NumCta, Convert.ToInt32(DatosBanco.IdCliente), Convert.ToInt32(emp.ID), DatosAval.Rut, DatosAval.Nombre);
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                LimpiarForm();
                Response.Redirect("../Empresas/");
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

        public int InsertClienteEmpresa(string idTipoEmp, string rut, string razon, string giro, string nombreNotaria, string direccion, string tlf, string fechaEscritura, string banco, string destinatario, string rutDest, string numCta, string rutAval, string nombreAval)
        {
            int id = 0;
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Empresa
                string spName = "usr_fnns.SP_CREATE_EMPRESAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT", rut);
                parametros.Add("@ID_TIPO_EMPRESA", idTipoEmp);
                parametros.Add("@RAZON_SOCIAL", razon);
                parametros.Add("@DIRECCION", direccion);
                parametros.Add("@TELEFONO", tlf);
                parametros.Add("@GIRO_COMERCIAL", giro);
                parametros.Add("@NOMBRE_NOTARIA", nombreNotaria);
                parametros.Add("@FECHA_ESCRITURA", fechaEscritura);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idEmpresa = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
                #region Parametros Cliente
                string spName1 = "usr_fnns.SP_CREATE_CLIENTES";
                Dictionary<string, string> parametros1 = new Dictionary<string, string>();
                parametros1.Add("@ID_PERSONA", "0");
                parametros1.Add("@ID_EMPRESA", idEmpresa.ToString());
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
                parametros2.Add("@CORREO", "");
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
                parametros.Add("@RUT_PERSONA", "0");
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

        public void EditarCliente(string idTipoEmp, string rut, string razon, string giro, string nombreNotaria, string direccion, string telefono, string fechaEscritura, string banco, string destinatario, string rutDest, string numCta, int idCliente, int id, string rutAval, string nombreAval)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EMPRESAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@ID_TIPO_EMPRESA", idTipoEmp);
                parametros.Add("@RUT", rut);
                parametros.Add("@RAZON_SOCIAL", razon);
                parametros.Add("@GIRO_COMERCIAL", giro);
                parametros.Add("@NOMBRE_NOTARIA", nombreNotaria);
                parametros.Add("@DIRECCION", direccion);
                parametros.Add("@FECHA_ESCRITURA", fechaEscritura);
                parametros.Add("@TELEFONO", telefono);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "CLIENTES EMPRESAS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);
                #region Parametros Datos Banco
                string spName2 = "usr_fnns.SP_MOD_DATOS_BANCO";
                Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                parametros2.Add("@ID_CLIENTE", idCliente.ToString());
                parametros2.Add("@ID_BANCO", banco);
                parametros2.Add("@RUT", rutDest);
                parametros2.Add("@DESTINATARIO", destinatario);
                parametros2.Add("@NUM_CUENTA", numCta);
                parametros2.Add("@EMAIL", "");
                parametros2.Add("@NAME_USER", Session["login"].ToString());
                parametros2.Add("@ORIGEN", "CLIENTES EMPRESAS");
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
                string spName3 = "usr_fnns.SP_MOD_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@RUT", rutAval);
                parametros3.Add("@NOMBRE", nombreAval);
                parametros3.Add("@NAME_USER", Session["login"].ToString());
                parametros3.Add("@ORIGEN", "CLIENTES EMPRESAS");
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
                string spName = "usr_fnns.SP_DEL_CLIENTE_EMP";
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
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string id = arg[0].ToString();
                    string idCliente = arg[1].ToString();
                    SqlDataAdapter adapter = GetAdapterRepresentantes(id);
                    Persona persona = GetPersonas(adapter).LastOrDefault();
                    if (persona == null)
                        Response.Write("<script>alert('Debe crear al menos un REPRESENTANTE LEGAL para la empresa');</script>");
                    else 
                    {
                        if (idCliente != null)
                        {
                            ActualizarNumOperacion(idCliente,"1");
                            int idSim = GetAdapterCreateSim(Convert.ToInt32(idCliente), 1, 3, 95, 5, 0, 0, 0, 0, 0, 0, 0, 0,0,0);
                            Response.Redirect("../Factoring/?cid=" + idCliente + "&sid=" + idSim.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            if (e.CommandName == "pres")
            {
                int idPres = GetAdapterCreatePrestamo(Convert.ToInt32(e.CommandArgument.ToString()), 0, 0, 0, 0);
                Response.Redirect("../Prestamo/?cid=" + e.CommandArgument.ToString() + "&pid=" + idPres.ToString());
            }


            if (e.CommandName == "edit")
            {
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
                    DatosBanco.IdCliente = idCliente;
                    SqlDataAdapter adapter = GetAdapterSelEmpresa(rut);
                    SqlDataAdapter adapter2 = GetAdapterSelDatosBanco(idCliente);
                    SqlDataAdapter adapter3 = GetAdapterSelDatosAval(idCliente);
                    var empresa = GetEmpresa(adapter).FirstOrDefault();
                    var datosBanco = GetDatosBanco(adapter2).FirstOrDefault();
                    var datosAval = GetDatosAval(adapter3).FirstOrDefault();
                    emp.ID = empresa.ID.ToString();
                    emp.TipoEmp = empresa.IdTipoEmpresa.ToString();
                    emp.Rut = empresa.Rut;
                    emp.RazonSocial = empresa.RazonSocial;
                    emp.GiroComercial = empresa.Giro;
                    emp.NombreNotaria = empresa.NombreNotaria;
                    emp.Direccion = empresa.Direccion;
                    emp.FechaEscritura = empresa.FechaEscritura;
                    emp.Telefono = empresa.Telefono;
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
            if (e.CommandName == "add")
            {
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(';');
                    string id = arg[0].ToString();
                    string nombre = arg[1].ToString();
                    string idCliente = arg[2].ToString();
                    Response.Redirect("../Representantes/?empid=" + id + "&nemp="+nombre+"&cid="+idCliente);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
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
                    Response.Redirect("../Empresas/");
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
            emp.Rut = "";
            emp.RazonSocial = "";
            emp.TipoEmp = "-1";
            emp.GiroComercial = "";
            emp.Direccion = "-1;-1;-1;";
            emp.NombreNotaria = "";
            emp.FechaEscritura = "";
            emp.Telefono = "";
            DatosBanco.Bancos = "-1";
            DatosBanco.Destinatario = "";
            DatosBanco.RutDest = "";
            DatosBanco.NumCta = "";
            DatosBanco.IdCliente = "0";
            emp.ID = "0";
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
                parametros.Add("@ORIGEN", "EMPRESAS");
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