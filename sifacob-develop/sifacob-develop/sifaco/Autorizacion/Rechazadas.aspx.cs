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
    public partial class Rechazadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "16";
            if (!IsPostBack)
            {
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
                parametros.Add("@AUTORIZADO","2");
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
                  }).GroupBy(x=>x.Correlativo).Select(group => group.First()).ToList();
            return re;
        }

        public string DatosAlterados(string origen, int id)
        {
            Common cls = new Common();
            return cls.ValuesAltered(origen, id.ToString());
        }

    }


}