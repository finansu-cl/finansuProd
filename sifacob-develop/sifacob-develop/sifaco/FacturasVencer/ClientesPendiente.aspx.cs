using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;

namespace sifaco.FacturasVencer
{
    public partial class ClientesPendiente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "19";
            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-5).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
                SqlDataAdapter adapter = GetAdapterFacturas(DateTime.Now.AddDays(-5), DateTime.Now);
                rptClienteFactura.DataSource = GetFacturas(adapter).GroupBy(x => x.Nombre).Select(group => group.First()).OrderBy(x=>x.Nombre).ToList();
                rptClienteFactura.DataBind();
                decimal mot = GetFacturas(adapter).Sum(x => x.MontoMora);
                txtMTP.Text = mot.ToString("N");
            }
        }

        public SqlDataAdapter GetAdapterFacturas()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_PENDIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
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

        public SqlDataAdapter GetAdapterFacturas(DateTime? fechaD, DateTime? fechaH)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_PENDIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@FECHA_INI", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_FIN", fechaH.ToString());
                tipoDatos.Add("datetime");
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


        public SqlDataAdapter GetAdapterFacturasXId(string idCliente, string rutDeudor)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_PENDIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                if (ddlBusquedaPor.SelectedValue.ToString() == "1")
                {
                    parametros.Add("@ID_CLIENTE", idCliente);
                    tipoDatos.Add("int");
                }
                else 
                {
                    parametros.Add("@RUT_DEUDOR", rutDeudor);
                    tipoDatos.Add("varchar");
                }
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

        public List<Facturas> GetFacturas(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Facturas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Facturas()
                  {
                      ID = item.Field<int>("ID"),
                      idTipoFac = item.Field<int>("ID_TIPO_FACTURA"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      Tipo = item.Field<string>("Tipo"),
                      NumFactura = item.Field<string>("NUM_FACTURA"),
                      Monto = item.Field<decimal>("MONTO_TOTAL"),
                      Plazo = item.Field<int>("PLAZO"),
                      Utilidad = item.Field<decimal>("MONTO_INTERES"),
                      MontoGirable = item.Field<decimal>("MONTO_GIRABLE"),
                      MontoAnticipo = item.Field<decimal>("MONTO_ANTICIPO"),
                      MontoPendiente = item.Field<decimal>("MONTO_PENDIENTE"),
                      RutDeudor = item.Field<string>("RUT_DEUDOR"),
                      Deudor = item.Field<string>("DEUDOR"),
                      IdEdoFactura = item.Field<int>("ID_EDO_FACTURA"),
                      EstadoFactura = item.Field<string>("ESTADO"),
                      Notificacion = item.Field<string>("FLG_NOTIFICACION"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      Operacion = item.Field<DateTime?>("OPERACION"),
                      MontoMora = item.Field<decimal>("INTERES_MORA"),
                      MontoReembolso = item.Field<decimal>("REEMBOLSO"),
                      VenceEn = item.Field<int>("VENCE_EN"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal>("TASA"),
                      Observacion = item.Field<string>("OBSERVACION"),
                      Devuelto = (item.Field<string>("DEVUELTO") == null) ? "2" : item.Field<string>("DEVUELTO")
                  }).ToList();
            return re;
        }

        public List<Facturas> GetFacturasD(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Facturas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Facturas()
                  {
                      ID = item.Field<int>("ID"),
                      idTipoFac = item.Field<int>("ID_TIPO_FACTURA"),
                      Rut = item.Field<string>("RUT_DEUDOR"),
                      Nombre = item.Field<string>("DEUDOR"),
                      Tipo = item.Field<string>("Tipo"),
                      NumFactura = item.Field<string>("NUM_FACTURA"),
                      Monto = item.Field<decimal>("MONTO_TOTAL"),
                      Plazo = item.Field<int>("PLAZO"),
                      Utilidad = item.Field<decimal>("MONTO_INTERES"),
                      MontoGirable = item.Field<decimal>("MONTO_GIRABLE"),
                      MontoAnticipo = item.Field<decimal>("MONTO_ANTICIPO"),
                      MontoPendiente = item.Field<decimal>("MONTO_PENDIENTE"),
                      RutDeudor = item.Field<string>("RUT_DEUDOR"),
                      //Deudor = item.Field<string>("DEUDOR"),
                      IdEdoFactura = item.Field<int>("ID_EDO_FACTURA"),
                      EstadoFactura = item.Field<string>("ESTADO"),
                      Notificacion = item.Field<string>("FLG_NOTIFICACION"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      Operacion = item.Field<DateTime?>("OPERACION"),
                      MontoMora = item.Field<decimal>("INTERES_MORA"),
                      MontoReembolso = item.Field<decimal>("REEMBOLSO"),
                      VenceEn = item.Field<int>("VENCE_EN"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal>("TASA")
                  }).ToList();
            return re;
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

        public string EditarEdoNotiFactura(string id, string valEdo, string idEdoFacAct)
        {
            try
            {
                if (idEdoFacAct == "1")
                {
                    int correlativo = GetLastCorrelativo();
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "usr_fnns.SP_MOD_EDO_FACTURA";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    ArrayList tipoDatos = new ArrayList();
                    parametros.Add("@ID", id.ToString());
                    tipoDatos.Add("int");
                    parametros.Add("@ID_EDO_FACTURA", valEdo);
                    tipoDatos.Add("int");
                    parametros.Add("@ORIGEN", "FACTURAS VENCER");
                    parametros.Add("@CORRELATIVO", correlativo.ToString());
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("int");
                    cls.GetConnectionToDbNonQuery(conn, spName, parametros, tipoDatos);
                }
                return "";
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return "";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);

            if (ddlBusquedaPor.SelectedValue.ToString() == "1")
            {
                SqlDataAdapter adapter = GetAdapterFacturas(fechaD, fechaH);
                rptClienteFactura.DataSource = GetFacturas(adapter).GroupBy(x => x.Nombre).Select(group => group.First()).ToList();
                rptClienteFactura.DataBind();
                ltrAfec.Text = "Cliente";
                ltrAfec3.Text = "Cliente";
            }
            else 
            {
                SqlDataAdapter adapter = GetAdapterFacturas(fechaD, fechaH);
                rptClienteFactura.DataSource = GetFacturasD(adapter).GroupBy(x => x.RutDeudor).Select(group => group.First()).ToList();
                rptClienteFactura.DataBind();
                ltrAfec.Text = "Deudor";
                ltrAfec3.Text = "Deudor";
            }

        }

        public SqlDataAdapter GetAdapterMoraXId(string idCliente, string rutDeudor)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_TOTAL_PENDIENTE";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                if (ddlBusquedaPor.SelectedValue.ToString() == "1")
                {
                    parametros.Add("@ID_CLIENTE", idCliente);
                    tipoDatos.Add("int");
                }
                else
                {
                    parametros.Add("@RUT_DEUDOR", rutDeudor);
                    tipoDatos.Add("varchar");
                }
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

        public string GetTotalMoraByClDe(string idCliente, string rutDeudor)
        {
            SqlDataAdapter adapter = GetAdapterMoraXId(idCliente, rutDeudor);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Facturas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Facturas()
                  {
                      MontoMora = item.Field<decimal>("INTERES_MORA")
                  }).ToList();
            return re.LastOrDefault().MontoMora.ToString("N");

        }

        public string GetTotalSaldoPendienteByClDe(string idCliente, string rutDeudor)
        {

            decimal mora = Convert.ToDecimal(GetTotalMoraByClDe(idCliente, rutDeudor));
            SqlDataAdapter adapter = GetAdapterMoraXId(idCliente, rutDeudor);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Facturas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Facturas()
                  {
                      MontoPendiente = item.Field<decimal>("MONTO_PENDIENTE")
                  }).ToList();
            decimal pendiente = re.LastOrDefault().MontoPendiente - mora;
            //return re.LastOrDefault().MontoPendiente.ToString("N");
            return (pendiente < 0)? "0,00": pendiente.ToString("N");

        }

        public string FactorasXAfect(string idCliente, string rutDeudor, int count)
        {
            SqlDataAdapter adapter = GetAdapterFacturasXId(idCliente, rutDeudor);
            List<Facturas> facturas = GetFacturas(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var fac in facturas)
            {
                string sel1 = "";
                string sel2 = "";
                string style2 = "style='background:yellow;'";

                switch (fac.Devuelto)
                {
                    case "1":
                        sel1 = "selected='selected'";
                        break;
                    case "2":
                        sel2 = "selected='selected'";
                        break;
                }

                decimal pendiente = fac.MontoPendiente - fac.MontoMora;
                filas.Append("<tr " + style2 + ">");
                filas.Append("<td>");
                filas.Append(fac.NumFactura);
                filas.Append("</td>");
                filas.Append("<td>");
                if(ddlBusquedaPor.SelectedValue.ToString() == "1")
                    filas.Append(fac.Deudor);
                else
                    filas.Append(fac.Nombre);
                filas.Append("</td>");
                filas.Append("<td id='td" + fac.ID + "'>");
                filas.Append((fac.MontoRestante == 0) ? fac.Monto.ToString("N") : fac.MontoRestante.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>$");
                filas.Append((pendiente < 0) ? "0,00" : pendiente.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd-MM-yyyy}", fac.Operacion));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd-MM-yyyy}", fac.Vencimiento));
                filas.Append("</td>");
                filas.Append("<td style='color:black;' align='center'>");
                filas.Append("<select class='disabled-guest' id='ddl" + fac.ID + "' style='width:110px;'><option value='1' " + sel1 + ">SI</option><option " + sel2 + " value='2'>NO</option></select>");
                filas.Append("<script type='text/javascript'>$(document).ready(function () {");
                filas.Append("$('#ddl" + fac.ID + "').change(function () {");
                //filas.Append("SaveValueDev(" + fac.ID + ",$('#ddl" + fac.ID + "').val(),true);");
                filas.Append("SaveValueDev(" + fac.ID + ",'D',$('#ddl" + fac.ID + "').val());");
                filas.Append("});");
                filas.Append("});</script>");
                filas.Append("</td>");
                filas.Append("</tr>");
                //filas.Append("<script type='text/javascript'>MiClick('#vc_" + count.ToString() + "','#dc_" + count.ToString() + "');</script>");

            }
            //filas.Append("<tr>");
            //filas.Append("<td colspan='7' style='text-align: right;'>");
            //filas.Append("<input type='button' value='Guardar' id='btn_" + idCliente + "_" + count + "' />");
            //filas.Append("<script type='text/javascript'>$(document).ready(function () {");
            //filas.Append("$('#btn_" + idCliente + "_" + count + "').click(function () {");
            //filas.Append("location.reload();");
            //filas.Append("});");
            //filas.Append("});</script>");
            //filas.Append("</td>");
            //filas.Append("</tr>");
            return filas.ToString();
        }


    }
}