using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace sifaco.Autorizacion
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "14";
            if (!IsPostBack)
            {
                GetAdapterNonQueryVencida();
                SqlDataAdapter adapter = GetAdapter();
                rptGrid.DataSource = GetOperacion(adapter); 
                rptGrid.DataBind();
               
            }
        }

        public SqlDataAdapter GetAdapter()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_OPERACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@AUTORIZADO","4");
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

        public int GetLastCorrelativo()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CORRELATIVO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int re = 0;
                re = (from item in dt.AsEnumerable()
                      select new Correlativos()
                      {
                          Correlativo = item.Field<int>("CORRELATIVO")
                      }).LastOrDefault().Correlativo;
                return re;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return 0;
            }
        }

        public List<Operacion> GetOperacion(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Operacion> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Operacion()
                  {
                      ID = item.Field<int>("ID"),
                      IdOrigen = item.Field<int>("ID_ORIGEN"),
                      IdDestino = item.Field<int?>("ID_DESTINO"),
                      Origen = item.Field<string>("ORIGEN"),
                      TipoOperacion = item.Field<string>("TIPO_OPERACION"),
                      UsuarioMod = item.Field<string>("USER_MOD"),
                      FechaMod = item.Field<DateTime?>("FECHA_MOD"),
                      Autorizado = item.Field<int>("AUTORIZADO"),
                      UsuarioAut = item.Field<string>("USER_AUT"),
                      FechaAut = item.Field<DateTime?>("FECHA_AUT"),
                      Correlativo = item.Field<int>("CORRELATIVO")
                  }).GroupBy(x=>x.IdOrigen).Select(group => group.First()).ToList();
            return re;
        }

        public string DatosAlterados(string origen,int id) 
        {
            Common cls = new Common();
            return cls.ValuesAltered(origen, id.ToString());
        }

        protected void rptGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                // e.CommandArgument;
                try
                {
                    string arg = e.CommandArgument.ToString();
                    int idOperacion = Convert.ToInt32(arg);
                    GetAdapterNonQuery(idOperacion.ToString(), "1");
                }
                catch (Exception ex)
                {
                }
            }
            if (e.CommandName == "delete")
            {
                try
                {
                    string arg = e.CommandArgument.ToString();
                    int idOperacion = Convert.ToInt32(arg);
                    GetAdapterNonQuery(idOperacion.ToString(), "2");
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            SqlDataAdapter adapter = GetAdapter();
            rptGrid.DataSource = GetOperacion(adapter);
            rptGrid.DataBind();
        }

        public void GetAdapterNonQuery(string Id, string aut)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_OPERACION_REVERSO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_OPER", Id);
                parametros.Add("@AUTORIZADO", aut);
                parametros.Add("@NAME_USR", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
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

        public void GetAdapterNonQueryVencida()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_VENCIMIENTO_AUTORIZADO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
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
    }


}