using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

namespace sifaco.Controls
{
    public partial class Direccion : System.Web.UI.UserControl
    {

        public string Region
        {
            get { return ddlRegion.SelectedValue; }
            set { ddlRegion.SelectedValue = value; }
        }
        public string Ciudad
        {
            get { return ddlCiudad.SelectedValue; }
            set {
                if (value == "-1")
                {
                    ddlCiudad.Items.Clear();
                    ddlCiudad.Items.Add("-- Sleccione --");
                    ddlComuna.Items.Clear();
                    ddlComuna.Items.Add("-- Sleccione --");
                }
                else
                {
                    SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_CIUDAD", "@ID_REGION", Region, false);
                    ddlCiudad.Items.Clear();
                    ddlCiudad.Items.Add("-- Sleccione --");
                    ddlComuna.Items.Clear();
                    ddlComuna.Items.Add("-- Sleccione --");
                    ddlCiudad.DataSource = GetCiudad(adapter);
                    ddlCiudad.DataTextField = "Nombre";
                    ddlCiudad.DataValueField = "Id";
                    ddlCiudad.DataBind();
                    ddlCiudad.SelectedValue = value;
                }
                }
        }
        public string Comuna
        {
            get { return ddlComuna.SelectedValue; }
            set {
                    if (value != "-1")
                    {
                        SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_COMUNA", "@ID_CIUDAD", Ciudad, false);
                        ddlComuna.Items.Clear();
                        ddlComuna.Items.Add("-- Sleccione --");
                        ddlComuna.DataSource = GetComuna(adapter);
                        ddlComuna.DataTextField = "Nombre";
                        ddlComuna.DataValueField = "Id";
                        ddlComuna.DataBind();
                        ddlComuna.SelectedValue = value;
                    }
                    else 
                    {
                        ddlComuna.Items.Clear();
                        ddlComuna.Items.Add("-- Sleccione --");
                    }
                }
        }
        public string Direcciones
        {
            get { return txtDireccion.Text; }
            set { txtDireccion.Text = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_REGION", "", "", true);
                ddlRegion.DataSource = GetRegion(adapter);
                ddlRegion.DataTextField = "Nombre";
                ddlRegion.DataValueField = "Id";
                ddlRegion.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_CIUDAD", "@ID_REGION", Region, false);
            ddlCiudad.Items.Clear();
            ddlCiudad.Items.Add("-- Sleccione --");
            ddlComuna.Items.Clear();
            ddlComuna.Items.Add("-- Sleccione --");
            ddlCiudad.DataSource = GetCiudad(adapter);
            ddlCiudad.DataTextField = "Nombre";
            ddlCiudad.DataValueField = "Id";
            ddlCiudad.DataBind();
        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = GetAdapter("usr_fnns.SP_SEL_COMUNA", "@ID_CIUDAD", Ciudad, false);
            ddlComuna.Items.Clear();
            ddlComuna.Items.Add("-- Sleccione --");
            ddlComuna.DataSource = GetComuna(adapter);
            ddlComuna.DataTextField = "Nombre";
            ddlComuna.DataValueField = "Id";
            ddlComuna.DataBind();
        }

        public SqlDataAdapter GetAdapter(string spName, string paramName, string paramValue, bool region) 
        {
            try
            {
                if (region)
                {
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    ArrayList tipoDatos = new ArrayList();
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                    return reader;
                }
                else
                {
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add(paramName, paramValue);
                    ArrayList tipoDatos = new ArrayList();
                    tipoDatos.Add("int");
                    SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                    return reader;
                }
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

        public List<Region> GetRegion(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Region> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Region()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return re;
        }

        public List<Region> GetCiudad(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Region> ci = null;
            ci = (from item in dt.AsEnumerable()
                  select new Region()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return ci;
        }

        public List<Region> GetComuna(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Region> co = null;
            co = (from item in dt.AsEnumerable()
                  select new Region()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return co;
        }
    }
}