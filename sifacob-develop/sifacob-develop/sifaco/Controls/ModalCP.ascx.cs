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
    public partial class ModalCP : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Enable the GridView paging option and  
                // specify the page size. 
                gvPerson.AllowPaging = true;
                gvPerson.PageSize = 15;
                // Enable the GridView sorting option. 
                gvPerson.AllowSorting = true;
                // Initialize the sorting expression. 
                ViewState["SortExpression"] = "ID ASC";
                // Populate the GridView. 
                BindGridView();
            }
        }

        private void BindGridView()
        {
            // Get the connection string from Web.config.  
            // When we use Using statement,  
            // we don't need to explicitly dispose the object in the code,  
            // the using statement takes care of it. 
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString))
            {
                if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                {
                    string idCliente = Request.QueryString["cid"].ToString();
                    SqlDataAdapter adapterC = GetAdapterGarantiasXClientes(Convert.ToInt32(idCliente));
                    DataSet dsPerson = new DataSet();
                    adapterC.Fill(dsPerson, "Person");
                    DataView dvPerson = dsPerson.Tables["Person"].DefaultView;
                    dvPerson.Sort = ViewState["SortExpression"].ToString();
                    gvPerson.DataSource = dvPerson;
                    gvPerson.DataBind();
                }
            }
        }

        // GridView.RowDataBound Event 
        protected void gvPerson_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Make sure the current GridViewRow is a data row. 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Make sure the current GridViewRow is either  
                // in the normal state or an alternate row. 
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    // Add client-side confirmation when deleting. 
                    ((LinkButton)e.Row.Cells[1].Controls[0]).Attributes["onclick"] = "if(!confirm('Esta seguro de eliminar la garantía?')) return false;";
                }
                //e.Row.Cells[2].Visible = false;

            }
        }

        // GridView.PageIndexChanging Event 
        protected void gvPerson_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page.  
            gvPerson.PageIndex = e.NewPageIndex;
            // Rebind the GridView control to  
            // show data in the new page. 
            BindGridView();
        }

        // GridView.RowEditing Event 
        protected void gvPerson_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Make the GridView control into edit mode  
            // for the selected row.  
            gvPerson.EditIndex = e.NewEditIndex;
            // Rebind the GridView control to show data in edit mode. 
            BindGridView();
        }

        // GridView.RowCancelingEdit Event 
        protected void gvPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Exit edit mode. 
            gvPerson.EditIndex = -1;
            // Rebind the GridView control to show data in view mode. 
            BindGridView();
        }

        // GridView.RowUpdating Event 
        protected void gvPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string id = gvPerson.Rows[e.RowIndex].Cells[2].Text;
                string tipo = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt1")).Text;
                string marca = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt2")).Text;
                string modelo = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt3")).Text;
                string ano = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt4")).Text;
                string motor = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt5")).Text;
                string chasis = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt6")).Text;
                string color = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt7")).Text;
                string patente = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt8")).Text;
                string rvm = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt9")).Text;
                string notaria = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt110")).Text;
                string nombre = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt120")).Text;
                string rut = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt130")).Text;
                string fecha = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txt140")).Text;
                EditarGarantiaVCliente(Convert.ToInt32(id), Convert.ToInt32(idCliente), tipo, marca, modelo, ano, motor, chasis, color, patente, rvm, notaria, nombre, rut, fecha);
                // Append the parameters. 
            }
            // Exit edit mode. 
            gvPerson.EditIndex = -1;
            // Rebind the GridView control to show data after updating. 
            BindGridView();
        }

        // GridView.RowDeleting Event 
        protected void gvPerson_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string id = gvPerson.Rows[e.RowIndex].Cells[2].Text;
                EliminarGarantiaVCliente(id);
            }
            // Rebind the GridView control to show data after deleting. 
            BindGridView();
        }

        // GridView.Sorting Event 
        protected void gvPerson_Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
            // If the sorting column is the same as the previous one,  
            // then change the sort order. 
            if (strSortExpression[0] == e.SortExpression)
            {
                if (strSortExpression[1] == "ASC")
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            // If sorting column is another column,   
            // then specify the sort order to "Ascending". 
            else
            {
                ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
            }
            // Rebind the GridView control to show sorted data. 
            BindGridView();
        }

        public void EliminarGarantiaVCliente(string Id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_GARANTIAS_V";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", Id);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
            }
        }
        
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                InsertGarantiaVCliente(Convert.ToInt32(idCliente), DatosVeh.Tipo, DatosVeh.Marca, DatosVeh.Modelo, DatosVeh.Ano, DatosVeh.Motor, DatosVeh.Chasis, DatosVeh.Color, DatosVeh.Patente, DatosVeh.Rvm, Prenda.Notaria, Prenda.Nombre, Prenda.Rut, Prenda.FechaEscritura);
            }
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            LimpiarForm();
        }

        //protected void lbtnSubmit2_Click(object sender, EventArgs e)
        //{
        //    if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
        //    {
        //        string idCliente = Request.QueryString["cid"].ToString();
        //        InsertGarantiaVCliente(Convert.ToInt32(idCliente),"", "", "", "", "", "", "","", "", Prenda.Notaria, Prenda.Nombre, Prenda.Rut, Prenda.FechaEscritura);
        //    }
        //}
        //protected void lbtnCancel2_Click(object sender, EventArgs e)
        //{
        //    LimpiarForm();
        //}

        public void InsertGarantiaVCliente(int idCliente, string tipo, string marca, string modelo, string ano, string motor, string chasis, string color, string patente, string rvm, string notaria, string nombreInscrito, string rutInscrito, string fechaEscritura)
        {
            try
            {

                if (fechaEscritura == "")
                    fechaEscritura = DateTime.Now.ToString();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Garantia V
                string spName = "usr_fnns.SP_CREATE_GARANTIAS_V";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@TIPO", tipo);
                parametros.Add("@MARCA", marca);
                parametros.Add("@MODELO", modelo);
                parametros.Add("@ANO", ano);
                parametros.Add("@MOTOR", motor);
                parametros.Add("@CHASIS", chasis);
                parametros.Add("@COLOR", color);
                parametros.Add("@PATENTE", patente);
                parametros.Add("@RVM", rvm);
                parametros.Add("@NOTARIA", notaria);
                parametros.Add("@NOMBRE_INSCRITO", nombreInscrito);
                parametros.Add("@RUT_INSCRITO", rutInscrito);
                parametros.Add("@FECHA_ESCRITURA", fechaEscritura);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idGarantia = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                //return id;
            }
        }

        public void EditarGarantiaVCliente(int id, int idCliente, string tipo, string marca, string modelo, string ano, string motor, string chasis, string color, string patente, string rvm, string notaria, string nombreInscrito, string rutInscrito, string fechaEscritura)
        {
            try
            {

                if (fechaEscritura == "")
                    fechaEscritura = DateTime.Now.ToString();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_GARANTIAS_V";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@TIPO", tipo);
                parametros.Add("@MARCA", marca);
                parametros.Add("@MODELO", modelo);
                parametros.Add("@ANO", ano);
                parametros.Add("@MOTOR", motor);
                parametros.Add("@CHASIS", chasis);
                parametros.Add("@COLOR", color);
                parametros.Add("@PATENTE", patente);
                parametros.Add("@RVM", rvm);
                parametros.Add("@NOTARIA", notaria);
                parametros.Add("@NOMBRE_INSCRITO", nombreInscrito);
                parametros.Add("@RUT_INSCRITO", rutInscrito);
                parametros.Add("@FECHA_ESCRITURA", fechaEscritura);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                tipoDatos.Add("varchar");
                cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
            }
        }

        public SqlDataAdapter GetAdapterGarantiasXClientes(int idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_GARANTIAS_V";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                return reader;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return null;
            }
        }

        public void LimpiarForm()
        {
            DatosVeh.Tipo = "";
            DatosVeh.Marca = "";
            DatosVeh.Modelo = "";
            DatosVeh.Ano = "";
            DatosVeh.Motor = "";
            DatosVeh.Chasis = "";
            DatosVeh.Color = "";
            DatosVeh.Patente = "";
            DatosVeh.Rvm = "";
            Prenda.Notaria = "";
            Prenda.Nombre = "";
            Prenda.Rut = "";
            Prenda.FechaEscritura = "";
        }

    }
}