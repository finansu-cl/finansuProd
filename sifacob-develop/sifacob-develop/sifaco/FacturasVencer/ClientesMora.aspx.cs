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
    public partial class ClientesMora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "9";
            if (!IsPostBack)
            {
                SqlDataAdapter adapter = GetAdapterFacturas();
                rptClienteFactura.DataSource = GetFacturas(adapter).GroupBy(x => x.Nombre).Select(group => group.First()).OrderBy(x=>x.Nombre).ToList();
                try
                {
                    rptClienteFactura.DataBind();
                }catch(Exception ex)
                {
                    var m = ex.Message;
                }
                SqlDataAdapter adapterC = GetAdapterCliente();
                ddlCliente.DataSource = GetClientes(adapterC);
                ddlCliente.DataTextField = "Nombre";
                ddlCliente.DataValueField = "ID";
                ddlCliente.DataBind();

                decimal mot = GetFacturas(adapter).Sum(x => x.MontoMora);
                decimal motpp = GetFacturas(adapter).Where(x => x.MoraPagada == "3").Sum(x => x.MontoMoraParcial);
                decimal motp = GetFacturas(adapter).Where(x=> x.MoraPagada == "1").Sum(x => x.MontoMora);
                decimal motnp = GetFacturas(adapter).Where(x => x.MoraPagada == "2").Sum(x => x.MontoMora);
                decimal mototalNP = (mot - (motnp + motpp + motp)) + motnp;
                txtMTP.Text = mot.ToString("N");
                txtMTPP.Text = motpp.ToString("N");
                txtMTP2.Text = motp.ToString("N");
                txtMTNP.Text = mototalNP.ToString("N");
                if (Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString()) != 3)
                    divMPP.Visible = false;
            }
        }

        public SqlDataAdapter GetAdapterFacturas()
        {
            try
            {
                string fechaD = DateTime.Now.AddDays(-5).ToShortDateString();
                string fechaH = DateTime.Now.ToShortDateString();
                txtFechaD.Text = fechaD;
                txtFechaH.Text = fechaH;

                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_MORA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@FECHA_DESDE", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_HASTA", fechaH.ToString());
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

        public SqlDataAdapter GetAdapterFacturasFiltros(DateTime fechaD, DateTime fechaH, int tipoFecha, int idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_MORA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                tipoDatos.Add("int");
                parametros.Add("@FECHA_DESDE", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_HASTA", fechaH.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@TIPO_FECHA", tipoFecha.ToString());
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

        public SqlDataAdapter GetAdapterFacturasXId(string idCliente, string rutDeudor, int tipoFecha)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES_MORA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente);
                tipoDatos.Add("int");
                parametros.Add("@TIPO_FECHA", tipoFecha.ToString());
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
                      MoraPagada = item.Field<string>("MORA_PAGADA"),
                      FechaPagoMoraParcial = item.Field<DateTime?>("FECHA_PAGO_MORA"),
                      MontoMoraParcial = item.Field<decimal>("MORA_PAGADA_PARCIAL")
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
            int tipoFecha = Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString());
            int idCliente = Convert.ToInt32(ddlCliente.SelectedValue.ToString());

            SqlDataAdapter adapter = GetAdapterFacturasFiltros(fechaD, fechaH, tipoFecha, idCliente);
            rptClienteFactura.DataSource = GetFacturas(adapter).GroupBy(x => x.Nombre).Select(group => group.First()).ToList();
            rptClienteFactura.DataBind();
            ltrAfec.Text = "Cliente";
            ltrAfec2.Text = "Cliente";
            ltrAfec3.Text = "Cliente";
            decimal mot = GetFacturas(adapter).Sum(x => x.MontoMora);
            decimal motpp = GetFacturas(adapter).Where(x => x.MoraPagada == "3").Sum(x => x.MontoMoraParcial);
            decimal motp = GetFacturas(adapter).Where(x => x.MoraPagada == "1").Sum(x => x.MontoMora);
            decimal motnp = GetFacturas(adapter).Where(x => x.MoraPagada == "2").Sum(x => x.MontoMora);
            decimal mototalNP = (mot - (motnp + motpp + motp)) + motnp;
            decimal mpp = mot - motpp;
            txtMTP.Text = mot.ToString("N");
            txtMTPP.Text = motpp.ToString("N");
            txtMTP2.Text = motp.ToString("N");
            txtMTNP.Text = mototalNP.ToString("N");
            txtMPP.Text = mpp.ToString("N");

            if (tipoFecha == -1)
            {
                divMTPP.Visible = true;
                divMTNP.Visible = true;
                divMPP.Visible = false;
                divMTP2.Visible = true;
            }

            if (tipoFecha == 1) 
            {
                divMTPP.Visible = false;
                divMTNP.Visible = false;
                divMPP.Visible = false;
                divMTP2.Visible = true;
            }
            if (tipoFecha == 2)
            {
                divMTPP.Visible = false;
                divMTP2.Visible = false;
                divMPP.Visible = false;
                divMTNP.Visible = true;
            }
            if (tipoFecha == 3)
            {
                divMTP2.Visible = false;
                divMTNP.Visible = false;
                divMTPP.Visible = true;
                divMPP.Visible = true;
            }
            
        }

        public SqlDataAdapter GetAdapterMoraXId(string idCliente, string rutDeudor, int tipoFecha, DateTime fechaD, DateTime fechaH)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_TOTAL_MORA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                    parametros.Add("@ID_CLIENTE", idCliente);
                    tipoDatos.Add("int");
                    parametros.Add("@FECHA_DESDE", fechaD.ToString());
                    tipoDatos.Add("datetime");
                    parametros.Add("@FECHA_HASTA", fechaH.ToString());
                    tipoDatos.Add("datetime");
                    parametros.Add("@TIPO_FECHA", tipoFecha.ToString());
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

        public string GetTotalMoraByClDe(string idCliente, string rutDeudor, int tipoFecha)
        {
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);

            SqlDataAdapter adapter = GetAdapterMoraXId(idCliente, rutDeudor, tipoFecha,fechaD,fechaH);
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

        public string GetTotalSaldoPendienteByClDe(string idCliente, string rutDeudor, int tipoFecha)
        {
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);

            decimal mora = Convert.ToDecimal(GetTotalMoraByClDe(idCliente, rutDeudor, tipoFecha));
            SqlDataAdapter adapter = GetAdapterMoraXId(idCliente, rutDeudor, tipoFecha, fechaD, fechaH);
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

        public string FactorasXAfect(string idCliente, string rutDeudor, int count, int criteria)
        {
            SqlDataAdapter adapter = GetAdapterFacturasXId(idCliente, rutDeudor, criteria);
            List<Facturas> facturas = GetFacturas(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var fac in facturas)
            {
                string sel1 = "";
                string sel2 = "";
                string sel3 = "";
                string style2 = "style='background:yellow;'";

                switch (fac.MoraPagada)
                {
                    case "1":
                        sel1 = "selected='selected'";
                        break;
                    case "2":
                        sel2 = "selected='selected'";
                        break;
                    case "3":
                        sel3 = "selected='selected'";
                        break;
                }

                switch (fac.IdEdoFactura)
                {
                    case 1:
                        if (fac.Vencimiento < DateTime.Now)
                        {
                            style2 = "style='background:red;color:white;'";
                        }
                        break;
                    case 2:
                        style2 = "style='background:red;color:white;'";
                        break;
                    case 3:
                        style2 = "style='background:green;color:white;'";
                        break;
                    case 4:
                        style2 = "style='background:orange;color:white;'";
                        break;
                    case 5:
                        style2 = "style='background:violet;color:white;'";
                        break;

                }

                decimal pendiente = fac.MontoPendiente - fac.MontoMora;
                filas.Append("<tr " + style2 + ">");
                filas.Append("<td>");
                filas.Append(fac.NumFactura);
                filas.Append("</td>");
                filas.Append("<td>");
                //if(ddlBusquedaPor.SelectedValue.ToString() == "1")
                //    filas.Append(fac.Deudor);
                //else
                    filas.Append(fac.Nombre);
                filas.Append("</td>");
                filas.Append("<td id='td" + fac.ID + "'>");
                filas.Append((fac.MontoRestante == 0) ? fac.Monto.ToString("N") : fac.MontoRestante.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>$");
                filas.Append((fac.MontoMora - fac.MontoMoraParcial).ToString("N"));
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

                filas.Append("<select id='ddl" + fac.ID + "' style='width:110px;'><option value='-1' style='color:black;'>-- Seleccione --</option><option " + sel1 + " value='1'>PAGADA</option><option " + sel2 + " value='2'>NO PAGADA</option><option " + sel3 + " value='3'>PAGO PARCIAL</option></select>");
                filas.Append("<input type='text' id='datePick" + fac.ID + "' style='display:none;'/><br><input type='button' id='btnGuar" + fac.ID + "' value='Guardar' style='display:none;'/>");
                //filas.Append("<input onKeyPress='return soloNum(event, this.id)' onblur='formatomiles(this.id);' type='text' id='pagoParcial" + fac.ID + "' style='display:none;'/><br><input type='button' id='btnGuarP" + fac.ID + "' value='Guardar' style='display:none;'/>");
                //if (fac.IdEdoFactura == 4)
                //    filas.Append("<a id='pP" + fac.ID + "' style='cursor:pointer;color:black;font-weight:bold;'>" + fac.MontoParcial.ToString("N") + "</a>");
                filas.Append("<script type='text/javascript'>$(document).ready(function () {");
                filas.Append("$(function () {$('#datePick" + fac.ID + "').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });$('#datePick" + fac.ID + "').inputmask(); });");
                filas.Append("$('#btnGuar" + fac.ID + "').click(function () {");
                filas.Append("SaveValueDev2(" + fac.ID + ",'M',$('#ddl" + fac.ID + "').val(),$('#datePick" + fac.ID + "').val());");//(" + fac.ID + ",3,$('#datePick" + fac.ID + "').val(),true);");
                filas.Append("});");
                //filas.Append("$('#btnGuarP" + fac.ID + "').click(function () {");
                //filas.Append("SaveValuePay(" + fac.ID + ",4,$('#pagoParcial" + fac.ID + "').val(),true);");
                //filas.Append("});");
                filas.Append("$('#ddl" + fac.ID + "').change(function () {");
                filas.Append("SaveValueDev(" + fac.ID + ",'M',$('#ddl" + fac.ID + "').val(),'');");
                filas.Append("});");
                filas.Append("$('#btnSaveObser" + fac.ID + "').click(function () {");
                filas.Append("SaveValueObser(" + fac.ID + ",'T',$('#txtObser" + fac.ID + "').val());");
                filas.Append("});");
                filas.Append("});</script>");
                filas.Append("</td>");
                filas.Append("<td style='color:Black;'><textarea id='txtObser" + fac.ID + "' rows='5' cols='20'>");
                filas.Append(fac.Observacion);
                filas.Append("</textarea><input type='button' id='btnSaveObser" + fac.ID + "' value='Guardar'/></td>");
                filas.Append("</tr>");
                //filas.Append("<script type='text/javascript'>MiClick('#vc_" + count.ToString() + "','#dc_" + count.ToString() + "');</script>");

            }
            return filas.ToString();
        }

        public SqlDataAdapter GetAdapterCliente()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
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

        public List<Clientes> GetClientes(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Clientes> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Clientes()
                  {
                      ID = item.Field<int>("ID"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre")
                  }).ToList();
            return re;
        }

    }
}