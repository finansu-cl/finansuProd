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
using System.Net;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Web.Configuration;

namespace sifaco.Contratos
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "20";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterDocumentos("");
                rptGrid.DataSource = GetDocumentos(adapter);
                rptGrid.DataBind();
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

        public SqlDataAdapter GetAdapterDocumentosById(string id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id);
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (UploadFile(con.ContratoD, con.ContratoStream))
                {
                    int idDocumento = InsertDocumento(con.Nombre, con.Funcionalidad, con.ContratoD.Replace(".docx",""), "1");
                    SqlDataAdapter adapter = GetAdapterDocumentos("");
                    rptGrid.DataSource = GetDocumentos(adapter);
                    rptGrid.DataBind();
                    LimpiarForm();
                }
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
                EditarDocumento(con.ID, con.Nombre, con.Funcionalidad, "AUTOMATICA","1");
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                LimpiarForm();
                Response.Redirect("../Contratos/");
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

        public int InsertDocumento(string nombre, string funcionalidad, string template, string url)
        {
            int id = 0;
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_CREATE_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@TEMPLATE", template);
                parametros.Add("@FUNCIONALIDAD", funcionalidad);
                parametros.Add("@URL", url);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                id = Convert.ToInt32(dt.Rows[0]["ID"]);
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

        public void EditarDocumento(string id, string nombre, string funcionalidad, string template, string url)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@TEMPLATE", template);
                parametros.Add("@FUNCIONALIDAD", funcionalidad);
                parametros.Add("@URL", url);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "DOCUMENTOS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
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

        public void GetAdapterNonQuery(string Id)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", Id);
                ArrayList tipoDatos = new ArrayList();
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
                    string idDocumento = e.CommandArgument.ToString();
                    SqlDataAdapter adapter = GetAdapterDocumentosById(idDocumento);
                    var documento = GetDocumentos(adapter).SingleOrDefault();
                    con.ID = documento.ID.ToString();
                    con.Nombre = documento.Nombre;
                    con.Funcionalidad = documento.Funcionalidad;
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
                    SqlDataAdapter adapter = GetAdapterDocumentos("");
                    rptGrid.DataSource = GetDocumentos(adapter);
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

        protected bool FTPUpload(string filePath, Stream stream)
        {
            string usuario = WebConfigurationManager.AppSettings["ftpUser"].ToString();
            string clave = WebConfigurationManager.AppSettings["ftpPassword"].ToString();
            string server = WebConfigurationManager.AppSettings["ftpServer"].ToString();

            bool retorno = false;
            //FTP Server URL.
            string ftp = server;

            //FTP Folder name. Leave blank if you want to upload to root folder.
            string ftpFolder = "public_html/finansu/Template/";

            byte[] fileBytes = null;

            //Read the FileName and convert it to Byte array.
            string fileName = Path.GetFileName(filePath);
            using (StreamReader fileStream = new StreamReader(stream))
            {
                fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                fileStream.Close();
            }

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential(usuario, clave);
                request.ContentLength = fileBytes.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                request.ServicePoint.ConnectionLimit = fileBytes.Length;
                request.EnableSsl = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                //lblMessage.Text += fileName + " uploaded.<br />";
                response.Close();
                retorno = true;
                return retorno;
            }
            catch (WebException ex)
            {
                return false;
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }

        public bool UploadFile(string filePath, Stream stream)
        {
            try
            {
                string usuario = WebConfigurationManager.AppSettings["ftpUser"].ToString();
                string clave = WebConfigurationManager.AppSettings["ftpPassword"].ToString();
                string ruta = WebConfigurationManager.AppSettings["ftpRoot"].ToString();
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ruta+filePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(usuario, clave);

                // Copy the contents of the file to the request stream.
                //StreamReader sourceStream = new StreamReader(stream);
                //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                //sourceStream.Close();
                //request.ContentLength = fileContents.Length;

                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(fileContents, 0, fileContents.Length);
                //requestStream.Close();
                using (StreamReader sourceStream = new StreamReader(stream))
                {
                   using(Stream requestStream = request.GetRequestStream())
                   {
                       stream.CopyTo(requestStream);
                   }
                 }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void LimpiarForm()
        {
            con.Nombre = "";
            con.Funcionalidad = "-1";
            //con.Contrato = null;
        }
    }
}