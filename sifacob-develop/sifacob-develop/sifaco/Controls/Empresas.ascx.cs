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

namespace sifaco.Controls
{
    public partial class Empresas : System.Web.UI.UserControl
    {

        public string ID
        {
            get { return txtIdEmpresa.Text; }
            set { txtIdEmpresa.Text = value; }
        }

        public string Rut
        {
            get { return txtRutEm.Text; }
            set { txtRutEm.Text = value; }
        }
        public string RazonSocial
        {
            get { return txtRazon.Text; }
            set { txtRazon.Text = value; }
        }
        public string TipoEmp
        {
            get { return ddlTipoEmp.SelectedValue; }
            set { ddlTipoEmp.SelectedValue = value; }
        }
        public string GiroComercial
        {
            get { return txtGiro.Text; }
            set { txtGiro.Text = value; }
        }
        public string Region
        {
            get { return dir.Region; }
            set { dir.Region = value; }
        }
        public string Ciudad
        {
            get { return dir.Ciudad; }
            set { dir.Ciudad = value; }
        }
        public string Comuna
        {
            get { return dir.Comuna; }
            set { dir.Comuna = value; }
        }

        public string Direccion
        {
            get { return dir.Direcciones; }
            set
            {
                string[] direccion = value.Split(';');
                dir.Region = direccion[0];
                dir.Ciudad = direccion[1];
                dir.Comuna = direccion[2];
                dir.Direcciones = direccion[3];
            }
        }

        public string Telefono
        {
            get { return txtTlf.Text; }
            set { txtTlf.Text = value; }
        }

        public string NombreNotaria
        {
            get { return txtNomNotaria.Text; }
            set { txtNomNotaria.Text = value; }
        }

        public string FechaEscritura
        {
            get { return txtFecha.Text; }
            set { txtFecha.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtTlf.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterTipoEmpresa("usr_fnns.SP_SEL_TIPO_EMPRESA", "", "");
                ddlTipoEmp.DataSource = GetTipoEmpresa(adapter);
                ddlTipoEmp.DataTextField = "Tipo";
                ddlTipoEmp.DataValueField = "Id";
                ddlTipoEmp.DataBind();
            }
        }

        public SqlDataAdapter GetAdapterTipoEmpresa(string spName, string paramName, string paramValue)
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

        public List<TipoEmpresa> GetTipoEmpresa(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<TipoEmpresa> re = null;
            re = (from item in dt.AsEnumerable()
                  select new TipoEmpresa()
                  {
                      ID = item.Field<int>("ID"),
                      Tipo = item.Field<string>("Tipo")
                  }).ToList();
            return re;
        }

    }
}