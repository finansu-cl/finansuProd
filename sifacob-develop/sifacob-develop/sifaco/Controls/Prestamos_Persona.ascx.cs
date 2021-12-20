using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class Prestamos_Persona : System.Web.UI.UserControl
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
            get { return txtProf.Text; }
            set { txtProf.Text = value; }
        }
        public string Empresa
        {
            get { return txtEmp.Text; }
            set { txtEmp.Text = value; }
        }
        public string RutEmpresa
        {
            get { return txtRutEmp.Text; }
            set { txtRutEmp.Text = value; }
        }

        public string RegionEmp
        {
            get { return dirEmp.Region; }
            set { dirEmp.Region = value; }
        }
        public string CiudadEmp
        {
            get { return dirEmp.Ciudad; }
            set { dirEmp.Ciudad = value; }
        }
        public string ComunaEmp
        {
            get { return dirEmp.Comuna; }
            set { dirEmp.Comuna = value; }
        }
        public string DireccionEmpresa
        {
            get { return dirEmp.Direcciones; }
            set
            {
                string[] direccion = value.Split(';');
                dirEmp.Region = direccion[0];
                dirEmp.Ciudad = direccion[1];
                dirEmp.Comuna = direccion[2];
                dirEmp.Direcciones = direccion[3];
            }
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
            set
            {
                string[] direccion = value.Split(';');
                dir.Region = direccion[0];
                dir.Ciudad = direccion[1];
                dir.Comuna = direccion[2];
                dir.Direcciones = direccion[3];
            }
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}