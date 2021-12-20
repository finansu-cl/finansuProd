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
    public partial class DatosGarantiaVehiculo : System.Web.UI.UserControl
    {
        public string IdCliente
        {
            get { return txtIdCliente.Text; }
            set { txtIdCliente.Text = value; }
        }

        public string Tipo
        {
            get { return txtTipo.Text; }
            set { txtTipo.Text = value; }
        }
        public string Marca
        {
            get { return txtMarca.Text; }
            set { txtMarca.Text = value; }
        }
        public string Modelo
        {
            get { return txtModelo.Text; }
            set { txtModelo.Text = value; }
        }
        public string Ano
        {
            get { return txtAno.Text; }
            set { txtAno.Text = value; }
        }

        public string Motor
        {
            get { return txtMotor.Text; }
            set { txtMotor.Text = value; }
        }

        public string Chasis
        {
            get { return txtChasis.Text; }
            set { txtChasis.Text = value; }
        }

        public string Color
        {
            get { return txtColor.Text; }
            set { txtColor.Text = value; }
        }

        public string Patente
        {
            get { return txtPatente.Text; }
            set { txtPatente.Text = value; }
        }

        public string Rvm
        {
            get { return txtRVM.Text; }
            set { txtRVM.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutDest.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            //txtNumCta.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}