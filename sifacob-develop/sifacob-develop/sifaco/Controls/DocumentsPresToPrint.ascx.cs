using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class DocumentsPresToPrint : System.Web.UI.UserControl
    {
        public bool Mutuo
        {
            get { return chkMutuo.Checked; }
            set { chkMutuo.Checked = value; }
        }
        public bool Pagare
        {
            get { return chkPagare.Checked; }
            set { chkPagare.Checked = value; }
        }
        public bool Tabla
        {
            get { return chkTabla.Checked; }
            set { chkTabla.Checked = value; }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}