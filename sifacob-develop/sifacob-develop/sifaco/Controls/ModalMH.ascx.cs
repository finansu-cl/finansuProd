using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Clases;
using System.Collections;

namespace sifaco.Controls
{
    public partial class ModalMH : System.Web.UI.UserControl
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
                GetComparecienteXCliente();
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
                string deslindes = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH1")).Text; 
                string nombreCp = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH2")).Text; 
                string notario = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH3")).Text; 
                string fojas = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH4")).Text; 
                string numero = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH5")).Text; 
                string ubicacionCbrs = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH6")).Text; 
                string ano = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH7")).Text; 
                string rol = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH8")).Text; 
                string fechaEscritura = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH9")).Text;
                string comuna = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtH0")).Text; 
                EditarGarantiaHCliente(Convert.ToInt32(id), Convert.ToInt32(idCliente), deslindes, nombreCp, notario, fojas, numero, ubicacionCbrs, ano, comuna, rol, fechaEscritura);
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
                EliminarGarantiaHCliente(id);
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

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                InsertGarantiaHCliente(Convert.ToInt32(idCliente), DatosHip.Deslindes, DatosHip.NombreCP, DatosHip.Notario, DatosHip.Fojas, DatosHip.Numero, DatosHip.UbicacionCbrs, DatosHip.Ano, DatosHip.Comuna, DatosHip.Rol, DatosHip.FechaEscritura);
                LimpiarForm();
            }
            // Rebind the GridView control to show inserted data. 
            BindGridView();
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            LimpiarForm();
        }

        public void InsertGarantiaHCliente(int idCliente, string deslindes, string nombreCPropiedad, string nombreNotario, string fojas, string numero, string ubicacionCbrs, string ano, string comuna, string rol, string fechaEscritura)
        {
            try
            {

                if (fechaEscritura == "")
                    fechaEscritura = DateTime.Now.ToString();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Garantia H
                string spName = "usr_fnns.SP_CREATE_GARANTIAS_H";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@DESLINDES", deslindes);
                parametros.Add("@NOMBRE_COMPRO_PROPIEDAD", nombreCPropiedad);
                parametros.Add("@NOMBRE_NOTARIO", nombreNotario);
                parametros.Add("@FOJAS", fojas);
                parametros.Add("@NUMERO", numero);
                parametros.Add("@UBICACION_CBRS", ubicacionCbrs);
                parametros.Add("@ANO", ano);
                parametros.Add("@COMUNA", comuna);
                parametros.Add("@ROL", rol);
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

        public void EditarGarantiaHCliente(int id,int idCliente, string deslindes, string nombreCPropiedad, string nombreNotario, string fojas, string numero, string ubicacionCbrs, string ano, string comuna, string rol, string fechaEscritura)
        {
            try
            {

                if (fechaEscritura == "")
                    fechaEscritura = DateTime.Now.ToString();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_GARANTIAS_H";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id.ToString());
                parametros.Add("@DESLINDES", deslindes);
                parametros.Add("@NOMBRE_COMPRO_PROPIEDAD", nombreCPropiedad);
                parametros.Add("@NOMBRE_NOTARIO", nombreNotario);
                parametros.Add("@FOJAS", fojas);
                parametros.Add("@NUMERO", numero);
                parametros.Add("@UBICACION_CBRS", ubicacionCbrs);
                parametros.Add("@ANO", ano);
                parametros.Add("@COMUNA", comuna);
                parametros.Add("@ROL", rol);
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
                string spName = "usr_fnns.SP_SEL_GARANTIAS_H";
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

        public void EliminarGarantiaHCliente(string Id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_GARANTIAS_H";
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

        public void InsertCompareciente(int idCliente, string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string representacion, string rutEmpresa, string direccionEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Personas
                string spName = "usr_fnns.SP_CREATE_COMPARECIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@RUT", rut);
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@EDO_CIVIL", edoCivil);
                parametros.Add("@SEXO", sexo);
                parametros.Add("@NACIONALIDAD", nacionalidad);
                parametros.Add("@PROFESION", profesion);
                parametros.Add("@DIRECCION", direccion);
                parametros.Add("@EMAIL", email);
                parametros.Add("@TELEFONO", telefono);
                parametros.Add("@CELULAR", celular);
                parametros.Add("@REPRESENTACION", representacion);
                parametros.Add("@RUT_EMPRESA", rutEmpresa);
                parametros.Add("@DIRECCION_EMPRESA", direccionEmpresa);
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
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idCompareciente = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
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

        public void LimpiarForm()
        {
            DatosHip.Deslindes = "";
            DatosHip.NombreCP = "";
            DatosHip.Notario = "";
            DatosHip.Ano = "";
            DatosHip.Fojas = "";
            DatosHip.Numero = "";
            DatosHip.UbicacionCbrs = "";
            DatosHip.Comuna = "";
            DatosHip.Rol = "";
            DatosHip.FechaEscritura = "";
        }

        public void LimpiarFormCompa()
        {
            PrestamoPersona.Rut = "";
            PrestamoPersona.Nombre = "";
            PrestamoPersona.EdoCivil = "";
            PrestamoPersona.Nacionalidad = "";
            PrestamoPersona.Direccion = "";
            PrestamoPersona.Empresa = "";
            PrestamoPersona.RutEmpresa = "";
            PrestamoPersona.DireccionEmpresa = "";
        }

        protected void lbPP1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string direccion = PrestamoPersona.Region + ";" + PrestamoPersona.Ciudad + ";" + PrestamoPersona.Comuna + ";" + PrestamoPersona.Direccion;
                string direccionEmpresa = PrestamoPersona.RegionEmp + ";" + PrestamoPersona.CiudadEmp + ";" + PrestamoPersona.ComunaEmp + ";" + PrestamoPersona.DireccionEmpresa;
                InsertCompareciente(Convert.ToInt32(idCliente), PrestamoPersona.Rut, PrestamoPersona.Nombre, PrestamoPersona.EdoCivil, "", PrestamoPersona.Profesion, PrestamoPersona.Nacionalidad, direccion, "", "", "", PrestamoPersona.Empresa, PrestamoPersona.RutEmpresa, direccionEmpresa);
            }
        }

        protected void lbPPU1_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string direccion = PrestamoPersona.Region + ";" + PrestamoPersona.Ciudad + ";" + PrestamoPersona.Comuna + ";" + PrestamoPersona.Direccion;
                string direccionEmpresa = PrestamoPersona.RegionEmp + ";" + PrestamoPersona.CiudadEmp + ";" + PrestamoPersona.ComunaEmp + ";" + PrestamoPersona.DireccionEmpresa;
                EditarCompareciente(Convert.ToInt32(idCliente), PrestamoPersona.Rut, PrestamoPersona.Nombre, PrestamoPersona.EdoCivil, "", PrestamoPersona.Profesion, PrestamoPersona.Nacionalidad, direccion, "", "", "", PrestamoPersona.Empresa, PrestamoPersona.RutEmpresa, direccionEmpresa);
            }
        }

        protected void lbPP2_Click(object sender, EventArgs e)
        {
            LimpiarFormCompa();
        }

        private void GetComparecienteXCliente() 
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                SqlDataAdapter adapter = GetAdapterSelCompareciente(Convert.ToInt32(idCliente));
                var datos = GetCompareciente(adapter).LastOrDefault();
                if (datos == null || datos.Rut == "")
                {
                    lbPPU1.Visible = false;
                    lbPP1.Visible = true;
                }
                else
                {
                    lbPPU1.Visible = true;
                    lbPP1.Visible = false;
                    PrestamoPersona.Rut = datos.Rut;
                    PrestamoPersona.Nombre = datos.Nombre;
                    PrestamoPersona.EdoCivil = datos.EdoCivil;
                    PrestamoPersona.Nacionalidad = datos.Nacionalidad;
                    PrestamoPersona.Empresa = datos.Empresa;
                    PrestamoPersona.RutEmpresa = datos.RutEmpresa;
                    PrestamoPersona.Profesion = datos.Profesion;
                    PrestamoPersona.DireccionEmpresa = datos.DireccionEmpresa;
                    PrestamoPersona.Direccion = datos.Direccion;
                }

            }
        }

        public void EditarCompareciente(int idCliente, string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string representacion, string rutEmpresa, string direccionEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_COMPARECIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                parametros.Add("@RUT", rut);
                parametros.Add("@NOMBRE", nombre);
                parametros.Add("@EDO_CIVIL", edoCivil);
                parametros.Add("@SEXO", sexo);
                parametros.Add("@NACIONALIDAD", nacionalidad);
                parametros.Add("@PROFESION", profesion);
                parametros.Add("@DIRECCION", direccion);
                parametros.Add("@EMAIL", email);
                parametros.Add("@TELEFONO", telefono);
                parametros.Add("@CELULAR", celular);
                parametros.Add("@REPRESENTACION", representacion);
                parametros.Add("@RUT_EMPRESA", rutEmpresa);
                parametros.Add("@DIRECCION_EMPRESA", direccionEmpresa);
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
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterSelCompareciente(int idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_COMPARECIENTE";
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

        public List<Compareciente> GetCompareciente(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Compareciente> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Compareciente()
                  {
                      ID = item.Field<int>("ID"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      Profesion = item.Field<string>("Profesion"),
                      Nacionalidad = item.Field<string>("Nacionalidad"),
                      EdoCivil = item.Field<string>("Edo_civil"),
                      Sexo = item.Field<string>("Sexo"),
                      Email = item.Field<string>("Email"),
                      Direccion = item.Field<string>("Direccion"),
                      Telefono = item.Field<string>("Telefono"),
                      Celular = item.Field<string>("Celular"),
                      Empresa = item.Field<string>("Representacion"),
                      RutEmpresa = item.Field<string>("Rut_Empresa"),
                      DireccionEmpresa = item.Field<string>("Direccion_Empresa")
                  }).ToList();
            return re;
        }

    }
}