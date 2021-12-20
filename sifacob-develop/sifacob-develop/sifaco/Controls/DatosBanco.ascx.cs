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

namespace sifaco.Controls
{
    public partial class DatosBanco : System.Web.UI.UserControl
    {
        public string IdCliente
        {
            get { return txtIdCliente.Text; }
            set { txtIdCliente.Text = value; }
        }

        public string Bancos
        {
            get { return ddlBancos.SelectedValue; }
            set { ddlBancos.SelectedValue = value; }
        }
        public string Destinatario
        {
            get { return txtDestinatario.Text; }
            set { txtDestinatario.Text = value; }
        }
        public string RutDest
        {
            get { return txtRutDest.Text; }
            set { txtRutDest.Text = value; }
        }
        public string NumCta
        {
            get { return txtNumCta.Text; }
            set { txtNumCta.Text = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutDest.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            txtNumCta.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapter();
                ddlBancos.DataSource = GetBancos(adapter);
                ddlBancos.DataTextField = "Nombre";
                ddlBancos.DataValueField = "ID";
                ddlBancos.DataBind();
            }

        }

        public List<Bancos> GetBancos(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Bancos> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Bancos()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return re;
        }


        public SqlDataAdapter GetAdapter()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_BANCO";
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


    }
}