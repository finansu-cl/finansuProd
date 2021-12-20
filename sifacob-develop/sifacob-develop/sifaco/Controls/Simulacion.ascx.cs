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
using System.Text;

namespace sifaco.Controls
{
    public partial class Simulacion : System.Web.UI.UserControl
    {

        public string ID
        {
            get { return txtIdSimulacion.Text; }
            set { txtIdSimulacion.Text = value; }
        }

        public string FechaSim
        {
            get { return txtFecSimulacion.Text; }
            set { txtFecSimulacion.Text = value; }
        }
        public int EstadoSim
        {
            get { return Convert.ToInt32(ddlEdoSim.SelectedValue); }
            set { ddlEdoSim.SelectedValue = value.ToString(); }
        }
        public decimal? Tasa
        {
            get { return Convert.ToDecimal(txtTasa.Text); }
            set { txtTasa.Text = value.ToString(); }
        }
        public decimal? NvaTasa
        {
            get { return Convert.ToDecimal(txtNvaTasa.Text); }
            set { txtNvaTasa.Text = value.ToString(); }
        }
        public decimal? Anticipo
        {
            get { return Convert.ToDecimal(txtAnticipo.Text); }
            set { txtAnticipo.Text = value.ToString(); }
        }
        public decimal? SaldoPendiente
        {
            get { return Convert.ToDecimal(txtSalPendiente.Text); }
            set { txtSalPendiente.Text = value.ToString(); }
        }

        //public int Plazo
        //{
        //    get { return Convert.ToInt32(txtPlazo.Text); }
        //    set { txtPlazo.Text = value.ToString(); }
        //}

        
        public decimal? GastosOperacion
        {
            get { return Convert.ToDecimal(txtGasOperacion.Text); }
            set { txtGasOperacion.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }
        public decimal? Comision
        {
            get { return Convert.ToDecimal(txtComision.Text); }
            set { txtComision.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }
        public decimal? Iva
        {
            get { return Convert.ToDecimal(txtIva.Text); }
            set { txtIva.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }

        public decimal? MontoTotal
        {
            get { return Convert.ToDecimal(txtMonTotal.Text); }
            set { txtMonTotal.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }
        public decimal? Utilidad
        {
            get { return Convert.ToDecimal(txtMonInteres.Text); }
            set { txtMonInteres.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }

        public decimal? MontoAnticipo
        {
            get { return Convert.ToDecimal(txtMonAnticipo.Text); }
            set { txtMonAnticipo.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }

        public decimal? MontoPendiente
        {
            get { return Convert.ToDecimal(txtMonPendiente.Text); }
            set { txtMonPendiente.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }
        public decimal? PrecioCesion
        {
            get { return Convert.ToDecimal(txtPreCesion.Text); }
            set { txtPreCesion.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }

        public decimal? MontoGirable
        {
            get { return Convert.ToDecimal(txtMonGirable.Text); }
            set { txtMonGirable.Text = Convert.ToDecimal(value.ToString()).ToString("N"); }
        }

        public bool CambioTasa
        {
            get { return chkCambio.Checked; }
            set { chkCambio.Checked = value; }
        }

        public bool VisibleNvaTasa
        {
            get { return txtNvaTasa.Visible; }
            set { txtNvaTasa.Visible = value; }
        }

        //public bool EnabledPlazo
        //{
        //    get { return txtPlazo.Visible; }
        //    set { txtPlazo.Visible = value; }
        //}
        public bool EnabledAnticipo
        {
            get { return txtAnticipo.Visible; }
            set { txtAnticipo.Visible = value; }
        }
        public bool EnabledTasa
        {
            get { return txtTasa.Visible; }
            set { txtTasa.Visible = value; }
        }
        public bool EnabledGastos
        {
            get { return txtGasOperacion.Visible; }
            set { txtGasOperacion.Visible = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            txtTasa.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtAnticipo.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            //txtPlazo.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            txtGasOperacion.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtGasOperacion.Attributes.Add("onblur", "formatomiles(this.id);");
            txtIva.Attributes.Add("onblur", "formatomiles(this.id);");
            txtComision.Attributes.Add("onblur", "formatomiles(this.id);");
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterEdoSim("usr_fnns.SP_SEL_EDO_SIMULACION", "", "");
                ddlEdoSim.DataSource = GetTipoFactura(adapter);
                ddlEdoSim.DataTextField = "Estado";
                ddlEdoSim.DataValueField = "Id";
                ddlEdoSim.DataBind();

            }

        }

        public SqlDataAdapter GetAdapterEdoSim(string spName, string paramName, string paramValue)
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

        public List<EstadoSimulacion> GetTipoFactura(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadoSimulacion> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadoSimulacion()
                  {
                      ID = item.Field<int>("ID"),
                      Estado = item.Field<string>("ESTADO")
                  }).ToList();
            return re;
        }

        protected void chkCambio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCambio.Checked)
            {
                txtTasa.Visible = false;
                txtNvaTasa.Visible = true;
                txtAnticipo.Enabled = false;
                txtGasOperacion.Enabled = false;
                txtComision.Enabled = false;
            }
            else 
            {
                txtTasa.Visible = true;
                txtNvaTasa.Visible = false;
                txtAnticipo.Enabled = true;
                txtGasOperacion.Enabled = true;
                txtComision.Enabled = true;
            }
        }

    }
}