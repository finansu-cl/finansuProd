using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;

namespace sifaco.Controls
{
    public partial class Contrato : System.Web.UI.UserControl
    {

        public string ID
        {
            get { return txtIdContrato.Text; }
            set { txtIdContrato.Text = value; }
        }

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }
        public string Funcionalidad
        {
            get { return ddlFunc.SelectedValue; }
            set { ddlFunc.SelectedValue = value; }
        }
        

        public string ContratoD
        {
            get { return Path.GetFileName(fuContrato.FileName); }
            //set { fuContrato.FileName = value; }
        }

        public Stream ContratoStream
        {
            get { return fuContrato.PostedFile.InputStream; }
            //set { fuContrato.FileName = value; }
        }

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rol"].ToString() == "guest")
            {
                txtNombre.Attributes.Add("readonly", "readonly");
                ddlFunc.Attributes.Add("readonly", "readonly");
                fuContrato.Attributes.Add("disabled", "disabled");
            }
        }

    }
}