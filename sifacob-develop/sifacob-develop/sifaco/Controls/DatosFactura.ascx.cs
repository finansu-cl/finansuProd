using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Collections;
using System.Data;
using System.Configuration;

namespace sifaco.Controls
{
    public partial class DatosFactura : System.Web.UI.UserControl
    {

        public string ID 
        {
            get { return txtIdFactura.Text; }
            set { txtIdFactura.Text = value; }
        }

        public string Rut
        {
            get { return txtRutD.Text; }
            set { txtRutD.Text = value; }
        }
        public string Deudor
        {
            get { return txtDeudor.Text; }
            set { txtDeudor.Text = value; }
        }
        public string DireccionDeudor
        {
            get { return txtDireccionFac.Text; }
            set { txtDireccionFac.Text = value; }
        }
        public string ComunaDeudor
        {
            get { return ddlComuna.SelectedItem.Text; }
            set
            {
                if (value != "-- Sleccione --")
                {
                    SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_COMUNA");
                    ddlComuna.Items.Clear();
                    ddlComuna.Items.Add("-- Sleccione --");
                    ddlComuna.DataSource = GetComuna(adapter);
                    ddlComuna.DataTextField = "Nombre";
                    ddlComuna.DataValueField = "Id";
                    ddlComuna.DataBind();
                    ddlComuna.SelectedItem.Text = value;
                }
                else
                {
                    ddlComuna.Items.Clear();
                    ddlComuna.Items.Add("-- Sleccione --");
                }
            }
        }
        public string TipoFac
        {
            get { return ddlTipoFac.SelectedValue; }
            set { ddlTipoFac.SelectedValue = value; }
        }
        public string NumFac
        {
            get { return txtNumFac.Text; }
            set { txtNumFac.Text = value; }
        }

        public string Monto
        {
            get { return txtMonto.Text; }
            set { txtMonto.Text = decimal.Parse(value).ToString("N"); }
        }

        public int Plazo
        {
            get { return Convert.ToInt32(txtPlazoFac.Text); }
            set { txtPlazoFac.Text = value.ToString(); }
        }

        public decimal Utilidad
        {
            get { return Convert.ToDecimal(txtUtilidadFac.Text); }
            set { txtUtilidadFac.Text = string.Format("{0:n0}", value); }
        }

        public decimal MontoAnticipo
        {
            get { return Convert.ToDecimal(txtAnticipoFac.Text); }
            set { txtAnticipoFac.Text = string.Format("{0:n0}", value); }
        }

        public decimal MontoPendiente
        {
            get { return Convert.ToDecimal(txtSalPendienteFac.Text); }
            set { txtSalPendienteFac.Text = string.Format("{0:n0}", value); }
        }

        public decimal MontoGirable
        {
            get { return Convert.ToDecimal(txtGirableFac.Text); }
            set { txtGirableFac.Text = string.Format("{0:n0}", value); }
        }

        public DateTime? FechaEmision 
        {
            get { return Convert.ToDateTime(txtFechaEmision.Text); }
            set { txtFechaEmision.Text = string.Format("{0:dd/MM/yyyy}", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtMonto.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtMonto.Attributes.Add("onblur", "formatomiles(this.id);");
            txtPlazoFac.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtUtilidadFac.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtUtilidadFac.Attributes.Add("onblur", "formatomiles(this.id);");
            txtAnticipoFac.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtAnticipoFac.Attributes.Add("onblur", "formatomiles(this.id);");
            txtSalPendienteFac.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtSalPendienteFac.Attributes.Add("onblur", "formatomiles(this.id);");
            txtGirableFac.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtGirableFac.Attributes.Add("onblur", "formatomiles(this.id);");
            txtNumFac.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            if (!IsPostBack) 
            {
                SqlDataAdapter adapter = GetAdapterTipoFac("usr_fnns.SP_SEL_TIPO_FACTURA", "", "");
                ddlTipoFac.DataSource = GetTipoFactura(adapter);
                ddlTipoFac.DataTextField = "Tipo";
                ddlTipoFac.DataValueField = "Id";
                ddlTipoFac.DataBind();
                SqlDataAdapter adapter2 = GetAdapter("usr_fnns.SP_SEL_COMUNA");
                ddlComuna.DataSource = GetComuna(adapter2);
                ddlComuna.DataTextField = "Nombre";
                ddlComuna.DataValueField = "Id";
                ddlComuna.DataBind();
            }

        }

        public SqlDataAdapter GetAdapterTipoFac(string spName, string paramName, string paramValue)
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

        public List<TipoFactura> GetTipoFactura(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<TipoFactura> re = null;
            re = (from item in dt.AsEnumerable()
                  select new TipoFactura()
                  {
                      ID = item.Field<int>("ID"),
                      Tipo = item.Field<string>("Tipo")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapter(string spName)
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

        public List<Region> GetComuna(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Region> co = null;
            co = (from item in dt.AsEnumerable()
                  select new Region()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return co;
        }

    }
}