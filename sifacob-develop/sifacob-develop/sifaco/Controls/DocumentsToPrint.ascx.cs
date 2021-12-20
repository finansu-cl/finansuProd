using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class DocumentsToPrint : System.Web.UI.UserControl
    {
        public bool Cesion
        {
            get { return chkCesion.Checked; }
            set { chkCesion.Checked = value; }
        }
        public bool Contrato
        {
            get { return chkCMarco.Checked; }
            set { chkCMarco.Checked = value; }
        }
        public bool Avalista
        {
            get { return chkAvalista.Checked; }
            set { chkAvalista.Checked = value; }
        }
        public bool Mandato
        {
            get { return chkMandato.Checked; }
            set { chkMandato.Checked = value; }
        }
        public bool Poder
        {
            get { return chkPoder.Checked; }
            set { chkPoder.Checked = value; }
        }
        public bool Uaf
        {
            get { return chkUaf.Checked; }
            set { chkUaf.Checked = value; }
        }
        public bool Carta
        {
            get { return chkCarta.Checked; }
            set { chkCarta.Checked = value; }
        }
        public bool DatosT
        {
            get { return chkDF.Checked; }
            set { chkDF.Checked = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}