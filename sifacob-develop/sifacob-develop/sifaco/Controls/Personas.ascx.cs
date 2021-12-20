using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class Personas : System.Web.UI.UserControl
    {
        public string ID
        {
            get { return txtIdPersonas.Text; }
            set { txtIdPersonas.Text = value; }
        }

        public string Rut
        {
            get{ return txtRutPer.Text; }
            set { txtRutPer.Text = value; }
        }
        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }
        public string Sexo 
        {
            get { return ddlSexo.SelectedValue; }
            set { ddlSexo.SelectedValue = value; }
        }
        public string EdoCivil
        {
            get { return ddlEdoCivil.SelectedValue; }
            set { ddlEdoCivil.SelectedValue = value; }
        }
        public string Nacionalidad
        {
            get { return txtNac.Text; }
            set { txtNac.Text = value; }
        }
        public string Profesion
        {
            get { return txtProfesion.Text; }
            set { txtProfesion.Text = value; }
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
            set {
                string[] direccion = value.Split(';');
                dir.Region = direccion[0];
                dir.Ciudad = direccion[1];
                dir.Comuna = direccion[2];
                dir.Direcciones = direccion[3];
                }
        }

        public string Telefono
        {
            get { return txtTelefono.Text; }
            set { txtTelefono.Text = value; }
        }
        public string Celular
        {
            get { return txtCelular.Text; }
            set { txtCelular.Text = value; }
        }
        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRutPer.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            txtTelefono.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
            txtCelular.Attributes.Add("onKeyPress", "return soloNumSinPuntos(event, this.id)");
        }
    }
}