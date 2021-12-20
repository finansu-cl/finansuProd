using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Clases;
using System.Collections;

namespace sifaco.Controls
{
    public partial class ModalMI : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        if (!IsPostBack) 
            { 
                GetAvalXCliente();
                GetComparecienteXCliente();
            } 
        } 
 
        public void InsertCompareciente(int idCliente, string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string representacion, string rutEmpresa, string direccionEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Personas
                string spName = "usr_fnns.SP_CREATE_COMPARECIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
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
                parametros.Add("@REPRESENTACION", representacion);
                parametros.Add("@RUT_EMPRESA", rutEmpresa);
                parametros.Add("@DIRECCION_EMPRESA", direccionEmpresa);
                parametros.Add("@NAME_USER", Session["login"].ToString());
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
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idCompareciente = Convert.ToInt32(dt.Rows[0]["ID"]);
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

        public void InsertAval(int idCliente, string rutAval, string nombreAval)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Datos Aval
                string spName3 = "SP_CREATE_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@NOMBRE", nombreAval);
                parametros3.Add("@RUT", rutAval);
                ArrayList tipoDatos3 = new ArrayList();
                tipoDatos3.Add("int");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                SqlDataAdapter reader3 = cls.GetConnectionToDb(conn, spName3, parametros3, tipoDatos3);
                DataTable dt3 = new DataTable();
                reader3.Fill(dt3);
                int idDatosAval = Convert.ToInt32(dt3.Rows[0]["ID"]);
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

        public void LimpiarFormCompa()
        {
            PrestamoPersona.Rut = "";
            PrestamoPersona.Nombre = "";
            PrestamoPersona.EdoCivil = "";
            PrestamoPersona.Nacionalidad = "";
            PrestamoPersona.Direccion = "";
            PrestamoPersona.Empresa = "";
            PrestamoPersona.RutEmpresa = "";
            PrestamoPersona.DireccionEmpresa = "";
        }

        public void LimpiarFormAval()
        {
            datosAval.Rut= "";
            datosAval.Nombre = "";
        }

        protected void lbPP1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string direccion = PrestamoPersona.Region + ";" + PrestamoPersona.Ciudad + ";" + PrestamoPersona.Comuna + ";" + PrestamoPersona.Direccion;
                string direccionEmpresa = PrestamoPersona.RegionEmp + ";" + PrestamoPersona.CiudadEmp + ";" + PrestamoPersona.ComunaEmp + ";" + PrestamoPersona.DireccionEmpresa;
                InsertCompareciente(Convert.ToInt32(idCliente), PrestamoPersona.Rut, PrestamoPersona.Nombre, PrestamoPersona.EdoCivil, "", PrestamoPersona.Profesion, PrestamoPersona.Nacionalidad, direccion, "", "", "", PrestamoPersona.Empresa, PrestamoPersona.RutEmpresa, direccionEmpresa);
            }
        }

        protected void lbPPU1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string direccion = PrestamoPersona.Region + ";" + PrestamoPersona.Ciudad + ";" + PrestamoPersona.Comuna + ";" + PrestamoPersona.Direccion;
                string direccionEmpresa = PrestamoPersona.RegionEmp + ";" + PrestamoPersona.CiudadEmp + ";" + PrestamoPersona.ComunaEmp + ";" + PrestamoPersona.DireccionEmpresa;
                EditarCompareciente(Convert.ToInt32(idCliente), PrestamoPersona.Rut, PrestamoPersona.Nombre, PrestamoPersona.EdoCivil, "", PrestamoPersona.Profesion, PrestamoPersona.Nacionalidad, direccion, "", "", "", PrestamoPersona.Empresa, PrestamoPersona.RutEmpresa, direccionEmpresa);
            }
        }

        protected void lbPP2_Click(object sender, EventArgs e)
        {
            LimpiarFormCompa();
        }

        protected void lbA1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                InsertAval(Convert.ToInt32(idCliente), datosAval.Rut,datosAval.Nombre);
            }
        }

        protected void lbAU1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                EditarAval(Convert.ToInt32(idCliente), datosAval.Rut, datosAval.Nombre);
            }
        }

        protected void lbA2_Click(object sender, EventArgs e)
        {
            LimpiarFormAval();
        }

        private void GetAvalXCliente() 
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapter3 = GetAdapterSelDatosAval(idCliente);
                var datos = GetDatosAval(adapter3).LastOrDefault();
                if (datos == null || datos.Rut == "")
                {
                    lbAU1.Visible = false;
                    lbA1.Visible = true;
                }
                else
                {
                    lbAU1.Visible = true;
                    lbA1.Visible = false;
                    datosAval.Rut = datos.Rut;
                    datosAval.Nombre = datos.Nombre;
                    datosAval.ID = datos.ID.ToString();
                }
            }

        }

        private void GetComparecienteXCliente() 
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapter = GetAdapterSelCompareciente(Convert.ToInt32(idCliente));
                var datos = GetCompareciente(adapter).LastOrDefault();
                if (datos == null || datos.Rut == "")
                {
                    lbPPU1.Visible = false;
                    lbPP1.Visible = true;
                }
                else
                {
                    lbPPU1.Visible = true;
                    lbPP1.Visible = false;
                    PrestamoPersona.Rut = datos.Rut;
                    PrestamoPersona.Nombre = datos.Nombre;
                    PrestamoPersona.EdoCivil = datos.EdoCivil;
                    PrestamoPersona.Nacionalidad = datos.Nacionalidad;
                    PrestamoPersona.Profesion = datos.Profesion;
                    PrestamoPersona.Empresa = datos.Empresa;
                    PrestamoPersona.RutEmpresa = datos.RutEmpresa;
                    PrestamoPersona.DireccionEmpresa = datos.DireccionEmpresa;
                    PrestamoPersona.Direccion = datos.Direccion;
                }

            }
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

        public void EditarAval(int idCliente, string rutAval, string nombreAval)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Datos Aval
                string spName3 = "SP_MOD_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@RUT", rutAval);
                parametros3.Add("@NOMBRE", nombreAval);
                ArrayList tipoDatos3 = new ArrayList();
                tipoDatos3.Add("int");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
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

        public void EditarCompareciente(int idCliente, string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string representacion, string rutEmpresa, string direccionEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_COMPARECIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
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
                parametros.Add("@REPRESENTACION", representacion);
                parametros.Add("@RUT_EMPRESA", rutEmpresa);
                parametros.Add("@DIRECCION_EMPRESA", direccionEmpresa);
                parametros.Add("@NAME_USER", Session["login"].ToString());
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
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterSelCompareciente(int idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_COMPARECIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
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

        public List<Compareciente> GetCompareciente(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Compareciente> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Compareciente()
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
                      Celular = item.Field<string>("Celular"),
                      Empresa = item.Field<string>("Representacion"),
                      RutEmpresa = item.Field<string>("Rut_Empresa"),
                      DireccionEmpresa = item.Field<string>("Direccion_Empresa")
                  }).ToList();
            return re;
        }

    }
}