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
    public partial class DatosGarantiaHipotecario : System.Web.UI.UserControl
    {
        public string IdCliente
        {
            get { return txtIdCliente.Text; }
            set { txtIdCliente.Text = value; }
        }

        public string Deslindes
        {
            get { return txtDeslindes.Text; }
            set { txtDeslindes.Text = value; }
        }
        public string NombreCP
        {
            get { return txtNombreCP.Text; }
            set { txtNombreCP.Text = value; }
        }
        public string Notario
        {
            get { return txtNombreN.Text; }
            set { txtNombreN.Text = value; }
        }
        public string Rol
        {
            get { return txtRol.Text; }
            set { txtRol.Text = value; }
        }

        public string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        public string Fojas
        {
            get { return txtFojas.Text; }
            set { txtFojas.Text = value; }
        }

        public string UbicacionCbrs
        {
            get { return txtUbicacionCbrs.Text; }
            set { txtUbicacionCbrs.Text = value; }
        }

        public string Ano
        {
            get { return txtAno.Text; }
            set { txtAno.Text = value; }
        }

        public string Comuna
        {
            get { return txtComuna.Text; }
            set { txtComuna.Text = value; }
        }

        public string FechaEscritura
        {
            get { return txtFechaEscritura.Text; }
            set { txtFechaEscritura.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutDest.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            //txtNumCta.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}