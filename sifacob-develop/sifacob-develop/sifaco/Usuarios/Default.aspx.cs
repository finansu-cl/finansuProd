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

namespace sifaco.Usuarios
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "13";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetUsuarios(adapter);
                rptGrid.DataBind();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                int idUser = InsertUsuario(user.Email,user.Clave,user.Perfil,user.Nombre,(user.Activo)?"1":"0",user.Imagen);
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetUsuarios(adapter);
                rptGrid.DataBind();
                LimpiarForm();
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
                Usuario usuario = null;
                if (user.Clave == "")
                {
                    SqlDataAdapter adap = GetAdapterSelUsuario(Convert.ToInt32(user.ID));
                    usuario = GetUsuarios(adap).SingleOrDefault();
                    usuario.Clave = usuario.Clave;
                }
                else 
                {
                    usuario = new Usuario();
                    usuario.Clave = user.Clave;
                }
                EditarUsuario(user.ID, usuario.Clave, user.Perfil, user.Nombre, (user.Activo) ? "1" : "0", user.Imagen);
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                user.EmailEnabled = true;
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetUsuarios(adapter);
                rptGrid.DataBind();
                LimpiarForm();
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
            user.EmailEnabled = true;
        }

        public int InsertUsuario(string email, string clave, string perfil, string nombre, string activo, string imgPerfil) 
        {
            int id = 0;
            try
            {   
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Personas
                string spName = "usr_fnns.SP_CREATE_USER";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("@EMAIL", email);
                    parametros.Add("@CLAVE", clave);
                    parametros.Add("@PERFIL", perfil);
                    parametros.Add("@NOMBRE", nombre);
                    parametros.Add("@ACTIVO", activo);
                    parametros.Add("@PERFIL_IMG", imgPerfil);
                    parametros.Add("@NAME_USER", Session["login"].ToString());
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("int");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                    DataTable dt = new DataTable();
                    reader.Fill(dt);
                    int idPersona = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
                return id;
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

        public SqlDataAdapter GetAdapter()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_USUARIOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                //parametros.Add("@EMAIL","0");
                ArrayList tipoDatos = new ArrayList();
                //tipoDatos.Add("int");
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

        public void EditarUsuario(string id, string clave, string perfil, string nombre, string activo, string imgPerfil)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_USER";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@CLAVE", clave);
                parametros.Add("@PERFIL", perfil);
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@ACTIVO", activo);
                parametros.Add("@PERFIL_IMG", imgPerfil);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "USUARIOS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
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

        public List<Usuario> GetUsuarios(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Usuario> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Usuario()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre"),
                      Email = item.Field<string>("EMAIL"),
                      Clave = item.Field<string>("CLAVE"),
                      Perfil = item.Field<string>("PERFIL"),
                      PerfilImg = item.Field<string>("PERFIL_IMG"),
                      Activo = item.Field<int>("ACTIVO")
                  }).ToList();
            return re;
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
                    activity.Attributes.Remove("class");
                    activity.Attributes.Add("class", "tab-pane");
                    timeline.Attributes.Remove("class");
                    timeline.Attributes.Add("class", "active tab-pane");
                    string arg = e.CommandArgument.ToString();
                    int idUser = Convert.ToInt32(arg);
                    SqlDataAdapter adapter = GetAdapterSelUsuario(idUser);
                    var usuario = GetUsuarios(adapter).FirstOrDefault();
                    user.ID = usuario.ID.ToString();
                    user.Nombre = usuario.Nombre;
                    user.Perfil = usuario.Perfil;
                    user.Imagen = usuario.PerfilImg;
                    user.Email = usuario.Email;
                    user.Activo = (usuario.Activo== 1)?true:false;
                    user.EmailEnabled = false;
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

        public SqlDataAdapter GetAdapterSelUsuario(int id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_USUARIOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID",id.ToString());
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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ltHidden.Text;
                if (id != "")
                {
                    GetAdapterNonQuery(id);
                    SqlDataAdapter adapter = GetAdapter();
                    rptGrid.DataSource = GetUsuarios(adapter);
                    rptGrid.DataBind();
                    LimpiarForm();
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

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_USER";
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
            user.Email = "";
            user.Nombre = "";
            user.Perfil = "admin";
            user.Imagen = "";
            user.Activo = false;
            user.Clave = "";
            user.ClaveConfirm = "";
        }

    }


}