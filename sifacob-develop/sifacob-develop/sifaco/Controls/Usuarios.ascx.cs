using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class Usuarios : System.Web.UI.UserControl
    {
        public string ID
        {
            get { return txtIdUsuarios.Text; }
            set { txtIdUsuarios.Text = value; }
        }

        public string Perfil
        {
            get { return ddlPerfil.SelectedValue; }
            set { ddlPerfil.SelectedValue = value; }
        }

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }
        public string Imagen 
        {
            get { return txtUsrlImg.Text; }
            set { txtUsrlImg.Text = value; }
        }
        public string Clave
        {
            get { return txtClave.Text; }
            set { txtClave.Text = value; }
        }
        public string ClaveConfirm
        {
            get { return txtCClave.Text; }
            set { txtCClave.Text = value; }
        }
        public bool Activo
        {
            get { return chkActi.Checked; }
            set { chkActi.Checked = value; }
        }
        
        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public bool EmailEnabled
        {
            get { return txtEmail.Enabled; }
            set { txtEmail.Enabled = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutPer.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            //txtTelefono.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            //txtCelular.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}