using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class DatosAval : System.Web.UI.UserControl
    {
        public string ID
        {
            get { return txtIdCliente.Text; }
            set { txtIdCliente.Text = value; }
        }

        public string Rut
        {
            get { return txtRutAval.Text; }
            set { txtRutAval.Text = value; }
        }
        public string Nombre
        {
            get { return txtNombreAval.Text; }
            set { txtNombreAval.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtRutAval.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}