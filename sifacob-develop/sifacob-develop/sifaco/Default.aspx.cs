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

namespace sifaco
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Cookies.Clear(); 
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usr = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            if (usr != "" && pwd != "")
            {
                SqlDataAdapter adapter = GetAdapter(usr, pwd);
                Usuario user = GetUser(adapter).LastOrDefault();
                if (user.Error != "USUARIO O CLAVE INCORRECTA")
                {
                    string imgPerfil = "~/Styles/img/img-default.png";

                    if(!String.IsNullOrEmpty(user.PerfilImg))
                    {
                        imgPerfil = user.PerfilImg;
                    }

                    Session["activo"] = user.Activo;
                    Session["imgPerfil"] = imgPerfil;
                    Session["rol"] = user.Perfil;
                    Session["login"] = user.Nombre;
                    HttpCookie cookie = new HttpCookie("activo");
                    cookie.Value = user.Activo.ToString();
                    cookie.Expires = DateTime.Now.AddHours(10);  
                    Response.Cookies.Add(cookie); 
                    Response.Redirect("Home/");
                }
                else 
                {
                    Response.Write("<script>alert('Usuario y/o Clave incorrectos');</script>");
                }

            }
            else
            {
                Response.Write("<script>alert('Debe completar todos los campos');</script>");
            }
        }

        public SqlDataAdapter GetAdapter(string email, string clave)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_USUARIO_AUT";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@EMAIL", email);
                parametros.Add("@CLAVE", clave);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public List<Usuario> GetUser(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Usuario> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Usuario()
                  {
                      ID = item.Field<int>("ID"),
                      Clave = item.Field<string>("Clave"),
                      Perfil = item.Field<string>("Perfil"),
                      Nombre = item.Field<string>("Nombre"),
                      Activo = item.Field<int>("Activo"),
                      PerfilImg = item.Field<string>("Perfil_img"),
                      //FechaCrea = item.Field<string>("Fecha_crea"),
                      Error = item.Field<string>("error")
                  }).ToList();
            return re;
        }

    }
}
