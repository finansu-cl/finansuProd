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
using System.Globalization;
using System.Threading;

namespace sifaco.Controls
{
    public partial class DatosPrestamo : System.Web.UI.UserControl
    {

        public string ID 
        {
            get { return txtIdPrestamo.Text; }
            set { txtIdPrestamo.Text = value; }
        }

       
        public decimal Monto
        {
            //get { return Convert.ToDecimal(txtMonto.Text); }
            get { return CustomParseMonto(txtMonto.Text); }
            set { txtMonto.Text = string.Format("{0:n0}", value); }
            //set { txtMonto.Text = value.ToString(); }
        }

        public int Plazo
        {
            get { return Convert.ToInt32(txtPlazo.Text); }
            set { txtPlazo.Text = value.ToString(); }
        }

        public int NumCuotas
        {
            get { return Convert.ToInt32(txtNumCuotas.Text); }
            set { txtNumCuotas.Text = value.ToString(); }
        }

        public int MesGracia
        {
            get { return Convert.ToInt32(txtMGracia.Text); }
            set { txtMGracia.Text = value.ToString(); }
        }

        public decimal Tasa
        {
            //get { return Convert.ToDecimal(txtTasa.Text); }
            get { return CustomParse(txtTasa.Text); }
            set { txtTasa.Text = value.ToString(); }
        }

        public decimal Cuota
        {
            //get { return Convert.ToDecimal(txtCuota.Text); }
            get { return CustomParseMonto(txtCuota.Text); }
            set { txtCuota.Text = value.ToString(); }
        }

        public DateTime FechaPres
        {
            get { return Convert.ToDateTime(txtFecPres.Text); }
            set { txtFecPres.Text = value.ToString(); }
        }

        //public DatosPrestamo()
        //{
        //    var culture = new CultureInfo("en-EN");
        //    Thread.CurrentThread.CurrentCulture = culture;
        //    Thread.CurrentThread.CurrentUICulture = culture;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            txtMonto.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtMonto.Attributes.Add("onblur", "formatomiles(this.id);");
            txtNumCuotas.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtPlazo.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtMGracia.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtTasa.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtCuota.Attributes.Add("onKeyPress", "return soloNum(event, this.id)");
            txtCuota.Attributes.Add("onblur", "formatomiles(this.id);");
            txtCuota.Attributes.Add("readonly", "readonly");

            if(Session["rol"].ToString() == "guest")
            {
                txtMonto.Attributes.Add("readonly", "readonly");
                txtPlazo.Attributes.Add("readonly", "readonly");
                txtNumCuotas.Attributes.Add("readonly", "readonly");
                txtTasa.Attributes.Add("readonly", "readonly");
                txtMGracia.Attributes.Add("readonly", "readonly");
                txtFecPres.Attributes.Add("readonly", "readonly");
            }

            if (!IsPostBack) 
            {

            }
        }

        public decimal CustomParse(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return 0.0m;
            return val / 10000;
        }

        public decimal CustomParseMonto(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return 0.0m;
            return val / 100;
        }
    }
}