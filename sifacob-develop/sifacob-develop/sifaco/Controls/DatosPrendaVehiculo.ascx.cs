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
    public partial class DatosPrendaVehiculo : System.Web.UI.UserControl
    {
        public string IdCliente
        {
            get { return txtIdCliente.Text; }
            set { txtIdCliente.Text = value; }
        }

        public string FechaEscritura
        {
            get { return txtFecha.Text; }
            set { txtFecha.Text = value; }
        }

        public string Notaria
        {
            get { return txtNotaria.Text; }
            set { txtNotaria.Text = value; }
        }

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Rut
        {
            get { return txtRut.Text; }
            set { txtRut.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutDest.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            //txtNumCta.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}