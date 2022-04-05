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
using System.Reflection;

namespace sifaco.CobranzaFacturas
{
    public partial class DetalleFactura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "6";

            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-5).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
            }

            if (Request.QueryString["fid"] != null && Request.QueryString["val"] != null && Request.QueryString["val2"] != null && Request.QueryString["edo"] != null)
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["val"].ToString() == "4")
                        EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["edo"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val2"].ToString(), "true");
                    else
                        EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val2"].ToString(), Request.QueryString["edo"].ToString());
                    StringBuilder sbJson = new StringBuilder();
                    sbJson.Append("[{")
                        .Append("    \"ok\": \"ok\"")
                        .Append("}]");

                    Page.Controls.Clear();
                    string callback = Request["callback"];
                    Response.Write(callback + "('" + sbJson.ToString() + "')");
                    PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    Isreadonly.SetValue(Request.QueryString, false, null);
                    Request.QueryString.Clear();
                }
            }
            else
            {
                if (Request.QueryString["fid"] != null && Request.QueryString["flg"] != null && Request.QueryString["obser"] != null && Request.QueryString["fecha"] == null)
                {
                    if (!IsPostBack)
                    {
                        EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["flg"].ToString(), Request.QueryString["obser"].ToString(), "", "false");
                        StringBuilder sbJson = new StringBuilder();
                        sbJson.Append("[{")
                            .Append("    \"ok\": \"ok\"")
                            .Append("}]");

                        Page.Controls.Clear();
                        string callback = Request["callback"];
                        Response.Write(callback + "('" + sbJson.ToString() + "')");
                        PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                        Isreadonly.SetValue(Request.QueryString, false, null);
                        Request.QueryString.Clear();
                    }
                }

                if (Request.QueryString["fid"] != null && Request.QueryString["flg"] != null && Request.QueryString["obser"] != null && Request.QueryString["fecha"] != null)
                {
                    if (!IsPostBack)
                    {
                        EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["flg"].ToString(), Request.QueryString["obser"].ToString(), Request.QueryString["fecha"].ToString(), "false");
                        StringBuilder sbJson = new StringBuilder();
                        sbJson.Append("[{")
                            .Append("    \"ok\": \"ok\"")
                            .Append("}]");

                        Page.Controls.Clear();
                        string callback = Request["callback"];
                        Response.Write(callback + "('" + sbJson.ToString() + "')");
                        PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                        Isreadonly.SetValue(Request.QueryString, false, null);
                        Request.QueryString.Clear();
                    }
                }

                if (Request.QueryString["fid"] != null && Request.QueryString["val"] != null && Request.QueryString["edo"] != null)
                {
                    if (Request.QueryString["edo"].ToString() == "M")
                    {
                        if (!IsPostBack)
                        {
                            EditarValorMoraManual(Request.QueryString["fid"].ToString(), Request.QueryString["val"].ToString());
                            StringBuilder sbJson = new StringBuilder();
                            sbJson.Append("[{")
                                .Append("    \"ok\": \"ok\"")
                                .Append("}]");

                            Page.Controls.Clear();
                            string callback = Request["callback"];
                            Response.Write(callback + "('" + sbJson.ToString() + "')");
                            PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                            Isreadonly.SetValue(Request.QueryString, false, null);
                            Request.QueryString.Clear();
                        }
                    }
                    else if (Request.QueryString["edo"].ToString() == "33")
                    {
                        if (!IsPostBack)
                        {
                            EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["edo"].ToString(), Request.QueryString["val"].ToString(), "true");
                            StringBuilder sbJson = new StringBuilder();
                            sbJson.Append("[{")
                                .Append("    \"ok\": \"ok\"")
                                .Append("}]");

                            Page.Controls.Clear();
                            string callback = Request["callback"];
                            Response.Write(callback + "('" + sbJson.ToString() + "')");
                            PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                            Isreadonly.SetValue(Request.QueryString, false, null);
                            Request.QueryString.Clear();
                        }
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            EditarEdoNotiFacturaManual(Request.QueryString["fid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val"].ToString(), "", Request.QueryString["edo"].ToString());
                            StringBuilder sbJson = new StringBuilder();
                            sbJson.Append("[{")
                                .Append("    \"ok\": \"ok\"")
                                .Append("}]");

                            Page.Controls.Clear();
                            string callback = Request["callback"];
                            Response.Write(callback + "('" + sbJson.ToString() + "')");
                            PropertyInfo Isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                            Isreadonly.SetValue(Request.QueryString, false, null);
                            Request.QueryString.Clear();
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["cid"] != null)
                {
                    SqlDataAdapter adapter = GetAdapterClientes(Request.QueryString["cid"].ToString());
                    rptClienteFactura.DataSource = GetClientes(adapter);
                    rptClienteFactura.DataBind();
                }
            }
        }

        public SqlDataAdapter GetAdapterClientes(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES_FACTURAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", idCliente);
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

        public List<Clientes> GetClientes(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Clientes> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Clientes()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre"),
                      NumOperacion = item.Field<int>("NUM_OPERACION"),
                      Rut = item.Field<string>("Rut"),
                      MontoFactoring = item.Field<decimal>("Monto")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterFacturas(string idCliente)
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
                DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
                if (txtFechaD.Text != "")
                    fechaD = Convert.ToDateTime(txtFechaD.Text);
                if (txtFechaH.Text != "")
                    fechaH = Convert.ToDateTime(txtFechaH.Text + " 23:59:59.997");

                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_FACTURAS_CLIENTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente);
                tipoDatos.Add("int");
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
                      Pago = item.Field<DateTime?>("FECHA_PAGO"),
                      FechaPagoParcial = item.Field<DateTime?>("FECHA_PAGO_PARCIAL"),
                      MontoMora = item.Field<decimal>("INTERES_MORA"),
                      MontoReembolso = item.Field<decimal>("REEMBOLSO"),
                      MontoRestante = item.Field<decimal>("MONTO_RESTANTE"),
                      MontoParcial = item.Field<decimal>("PAGO_PARCIAL"),
                      Emision = item.Field<DateTime?>("FECHA_EMISION"),
                      Observacion = item.Field<string>("OBSERVACION")
                  }).ToList();
            return re;
        }

        public string FacturasPorCliente(string idCliente)
        {
            string fechaE = "";
            SqlDataAdapter adapter = GetAdapterFacturas(idCliente);
            List<Facturas> facturas = GetFacturas(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var fac in facturas)
            {

                string sel1 = "";
                string sel2 = "";
                string sel3 = "";
                string sel4 = "";
                string sel5 = "";
                string sel6 = "";
                string sel7 = "";
                string sel8 = "";
                string sel9 = "";
                string style2 = "";

                switch (fac.IdEdoFactura)
                {
                    case 1:
                        sel1 = "selected='selected'";
                        if (fac.Vencimiento < DateTime.Now)
                        {
                            style2 = "style='background:red;color:white;'";
                            EditarEdoNotiFactura(fac.ID.ToString(), "", "2", "", "true");
                        }
                        break;
                    case 2:
                        sel2 = "selected='selected'";
                        style2 = "style='background:red;color:white;'";
                        break;
                    case 3:
                        sel3 = "selected='selected'";
                        style2 = "style='background:green;color:white;'";
                        break;
                    case 4:
                        sel6 = "selected='selected'";
                        style2 = "style='background:orange;color:white;'";
                        break;
                    case 5:
                        sel7 = "selected='selected'";
                        style2 = "style='background:violet;color:white;'";
                        break;
                    case 6:
                        sel8 = "selected='selected'";
                        style2 = "style='background:blue;color:white;'";
                        break;
                    case 7:
                        sel9 = "selected='selected'";
                        style2 = "style='background:gray;color:black;'";
                        break;

                }

                filas.Append("<tr " + style2 + ">");
                filas.Append("<td align='center'>");
                filas.Append(fac.NumFactura.ToString());
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(fac.Deudor.ToString());
                filas.Append(" (" + fac.RutDeudor.ToString() + ")");
                filas.Append("</td>");
                filas.Append("<td>$");
                filas.Append(fac.Plazo.ToString());
                filas.Append("Días</td>");
                filas.Append("<td id='td" + fac.ID + "'>");
                filas.Append((fac.MontoRestante == 0) ? fac.Monto.ToString("N") : fac.MontoRestante.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(fac.MontoPendiente.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                //filas.Append("<input type='text' id='txtMora" + fac.ID + "' value='" + fac.MontoMora.ToString("N") + "' /><br><input type='button' id='btnGuarMora" + fac.ID + "' value='Guardar'/>");
                filas.Append(fac.MontoMora.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(fac.MontoReembolso.ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd/MM/yyyy}", fac.Operacion));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd/MM/yyyy}", fac.Vencimiento));
                filas.Append("</td>");
                filas.Append("<td style='color:black;' align='center'>");

                filas.Append("<select id='ddl" + fac.ID + "' style='width:110px;'>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || fac.IdEdoFactura == 1)
                    filas.Append("<option " + sel1 + " value='1' style='color:black;'>SIN VENCER</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" ||
                    (fac.IdEdoFactura != 3 && fac.IdEdoFactura != 4 &&
                    fac.IdEdoFactura != 5 && fac.IdEdoFactura != 6 && fac.IdEdoFactura != 7))
                    filas.Append("<option " + sel2 + " value='2' style='background:red;color:white;'>VENCIDA</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (fac.IdEdoFactura != 6 && fac.IdEdoFactura != 7))
                    filas.Append("<option " + sel3 + " value='3' style='background:green;color:white;'>PAGADA</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (fac.IdEdoFactura != 3 && fac.IdEdoFactura != 6 && fac.IdEdoFactura != 7))
                    filas.Append("<option " + sel6 + " value='4' style='background:orange;color:white;'>PAGO PARCIAL</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (fac.IdEdoFactura != 3 && fac.IdEdoFactura != 6 && fac.IdEdoFactura != 7))
                    filas.Append("<option " + sel7 + " value='5' style='background:violet;color:white;'>PAGADO SIN INTERESES DE MORA</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (fac.IdEdoFactura != 3 && fac.IdEdoFactura != 7))
                    filas.Append("<option " + sel8 + " value='6' style='background:blue;color:white;'>EN JUICIO</option>");
                if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (fac.IdEdoFactura != 3 && fac.IdEdoFactura != 6))
                    filas.Append("<option " + sel9 + " value='7' style='background:gray;color:black;'>INCOBRABLE</option>");
                filas.Append("</select>");

                filas.Append("<input type='text' id='datePick" + fac.ID + "' style='display:none;'/><br><input type='button' id='btnGuar" + fac.ID + "' value='Guardar' style='display:none;'/>");
                filas.Append("<input onKeyPress='return soloNum(event, this.id)' onblur='formatomiles(this.id);' type='text' id='pagoParcial" + fac.ID + "' style='display:none;'/><br><input type='text' id='datePick3" + fac.ID + "' style='display:none;'/><br><input type='button' id='btnGuarP" + fac.ID + "' value='Guardar' style='display:none;'/>");
                if (fac.IdEdoFactura == 4)
                    filas.Append("<a id='pP" + fac.ID + "' style='cursor:pointer;color:black;font-weight:bold;'>" + fac.MontoParcial.ToString("N") + "</a>");
                filas.Append("</td>");
                filas.Append("<td><span id='mas4" + fac.ID + "'>");
                filas.Append((fac.Pago == null && fac.FechaPagoParcial == null) ? "NA" : (fac.Pago != null) ? string.Format("{0:dd/MM/yyyy}", fac.Pago) : string.Format("{0:dd/MM/yyyy}", fac.FechaPagoParcial));
                filas.Append("</span>");
                filas.Append("<input type='text' id='datePick4" + fac.ID + "' style='display:none;color:black;'/><br><input type='button' id='btnGuar4" + fac.ID + "' value='Guardar Fecha' style='display:none;color:black;'/>");
                filas.Append("</td>");
                filas.Append("<td style='color:white;'>");
                switch (fac.Notificacion)
                {
                    case "SI":
                        sel4 = "selected='selected'";
                        break;
                    case "NO":
                        sel5 = "selected='selected'";
                        break;
                }
                if (fac.Emision == null)
                    fechaE = string.Format("{0:dd/MM/yyyy}", "9999-12-31");
                else
                    fechaE = string.Format("{0:dd/MM/yyyy}", fac.Emision);

                //filas.Append("<select id='ddlN" + fac.ID + "'><option " + sel4 + " value='1'>SI</option><option " + sel5 + " value='2'>NO</option></select>");
                //filas.Append("<input type='text' id='datePick2" + fac.ID + "' value='" + fechaE + "'/><br><input type='button' id='btnGuar2" + fac.ID + "' value='Guardar' />");
                filas.Append(fechaE);
                filas.Append("</td>");
                filas.Append("<td style='color:Black;'><textarea id='txtObser" + fac.ID + "' rows='5' cols='20'>");
                filas.Append(fac.Observacion);
                filas.Append("</textarea><input type='button' id='btnSaveObser" + fac.ID + "' value='Guardar'/>");
                filas.Append("<script type='text/javascript'>$(document).ready(function () {");
                filas.Append("$(function () {$('#datePick" + fac.ID + "').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });$('#datePick" + fac.ID + "').inputmask(); });");
                filas.Append("$(function () {$('#datePick2" + fac.ID + "').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });$('#datePick2" + fac.ID + "').inputmask(); });");
                filas.Append("$(function () {$('#datePick3" + fac.ID + "').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });$('#datePick2" + fac.ID + "').inputmask(); });");
                filas.Append("$(function () {$('#datePick4" + fac.ID + "').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' }); });");
                filas.Append("$('#btnGuar2" + fac.ID + "').click(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#datePick2" + fac.ID + "').val(),false);");
                //filas.Append("SaveDateNoti(" + fac.ID + ",3,$('#datePick2" + fac.ID + "').val(),true);");
                filas.Append("});");
                filas.Append("$('#btnGuar4" + fac.ID + "').click(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#datePick4" + fac.ID + "').val(),33);");
                filas.Append("});");

                filas.Append("$('#btnGuar" + fac.ID + "').click(function () {");
                filas.Append("SaveValuePay(" + fac.ID + ",3,$('#datePick" + fac.ID + "').val(),true,'');");
                filas.Append("});");
                filas.Append("$('#btnGuarMora" + fac.ID + "').click(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#txtMora" + fac.ID + "').val(),'M');");
                filas.Append("});");
                filas.Append("$('#btnGuarP" + fac.ID + "').click(function () {");
                filas.Append("SaveValuePay(" + fac.ID + ",4,$('#pagoParcial" + fac.ID + "').val(),true,$('#datePick3" + fac.ID + "').val());");
                filas.Append("});");
                filas.Append("$('#btnSaveObser" + fac.ID + "').click(function () {");
                filas.Append("SaveValueObser(" + fac.ID + ",'T',$('#txtObser" + fac.ID + "').val());");
                filas.Append("});");
                filas.Append("$('#pP" + fac.ID + "').click(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#ddl" + fac.ID + "').val(),true);");
                filas.Append("});");
                filas.Append("$('#mas4" + fac.ID + "').click(function () {");
                filas.Append("$('#datePick4" + fac.ID + "').show();");
                filas.Append("$('#btnGuar4" + fac.ID + "').show();");
                filas.Append("});");
                filas.Append("$('#ddl" + fac.ID + "').change(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#ddl" + fac.ID + "').val(),true);");
                filas.Append("});");
                filas.Append("$('#ddlN" + fac.ID + "').change(function () {");
                filas.Append("SaveValue(" + fac.ID + ",$('#ddlN" + fac.ID + "').val(),false);");
                filas.Append("});");
                filas.Append("});</script>");

                filas.Append("</td>");

                filas.Append("</tr>");

            }
            return filas.ToString();
        }

        public SqlDataAdapter GetAdapterEdoFac(string spName, string paramName, string paramValue)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
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

        public List<EstadoFactura> GetEdoFactura(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadoFactura> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadoFactura()
                  {
                      ID = item.Field<int>("ID"),
                      Estado = item.Field<string>("ESTADO")
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

        public void EditarEdoNotiFactura(string id, string valNotificacion, string valEdo, string fechaPago, string flg)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EDO_FACTURA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", id.ToString());
                tipoDatos.Add("int");
                if (flg == "true")
                {
                    if (valEdo == "3")
                    {
                        //Convert.ToDateTime(txtFechaH.Text);
                        if (fechaPago == "")
                        {
                            fechaPago = Convert.ToDateTime("1753-01-01 00:00:00").ToString();
                        }
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else if (valEdo == "4")
                    {
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(".","").Replace(",",".").ToString()*/);
                        tipoDatos.Add("decimal");
                    }
                    else
                    {
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                    }
                }
                else
                {
                    if (valNotificacion == "T")
                    {
                        parametros.Add("@FLG_NOTIFICACION", valNotificacion);
                        tipoDatos.Add("varchar");
                        parametros.Add("@OBSERVACION", valEdo);
                        tipoDatos.Add("varchar");
                    }
                    else
                    {
                        parametros.Add("@FLG_NOTIFICACION", valNotificacion);
                        tipoDatos.Add("varchar");
                    }
                }
                parametros.Add("@ORIGEN", "COBRANZA FACTURAS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                tipoDatos.Add("varchar");
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

        public void EditarEdoNotiFacturaManual(string id, string valNotificacion, string valEdo, string fechaPago, string flg)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EDO_FACTURA_MANUAL";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", id.ToString());
                tipoDatos.Add("int");
                if (flg == "true")
                {
                    if (valEdo == "3")
                    {
                        //Convert.ToDateTime(txtFechaH.Text);
                        if (fechaPago == "")
                        {
                            fechaPago = Convert.ToDateTime("1753-01-01 00:00:00").ToString();
                        }
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else if (valEdo == "4")
                    {
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(".","").Replace(",",".").ToString()*/);
                        tipoDatos.Add("decimal");
                        parametros.Add("@FECHA_PAGO_PARCIAL", Convert.ToDateTime(valNotificacion).ToString());
                        tipoDatos.Add("datetime");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(valNotificacion).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else if (valEdo == "33")
                    {
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else
                    {
                        parametros.Add("@ID_EDO_FACTURA", valEdo);
                        tipoDatos.Add("int");
                    }
                }
                else
                {
                    if (valNotificacion == "T")
                    {
                        parametros.Add("@FLG_NOTIFICACION", valNotificacion);
                        tipoDatos.Add("varchar");
                        parametros.Add("@OBSERVACION", valEdo);
                        tipoDatos.Add("varchar");
                    }
                    else
                    {
                        if (valNotificacion == "D")
                        {
                            parametros.Add("@FLG_NOTIFICACION", valNotificacion);
                            tipoDatos.Add("varchar");
                            parametros.Add("@DEVOLUCION", valEdo);
                            tipoDatos.Add("varchar");
                        }
                        else
                        {
                            if (valNotificacion == "M")
                            {
                                parametros.Add("@FLG_NOTIFICACION", valNotificacion);
                                tipoDatos.Add("varchar");
                                parametros.Add("@MORA_PAGADA", valEdo);
                                tipoDatos.Add("varchar");
                                parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                                tipoDatos.Add("datetime");
                            }
                            else
                            {
                                parametros.Add("@FLG_NOTIFICACION", "F");
                                tipoDatos.Add("varchar");
                                parametros.Add("@FECHA_EMISION", Convert.ToDateTime(valNotificacion).ToString());
                                tipoDatos.Add("datetime");
                            }
                        }
                    }
                }
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "COBRANZA FACTURAS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
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

        public void EditarValorMoraManual(string id, string montoMora)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EDO_MORA_MANUAL";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", id.ToString());
                tipoDatos.Add("int");
                parametros.Add("@MONTO_MORA", montoMora/*.Replace(".","").Replace(",",".").ToString()*/);
                tipoDatos.Add("decimal");
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "COBRANZA FACTURAS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null)
            {
                SqlDataAdapter adapter = GetAdapterClientes(Request.QueryString["cid"].ToString());
                rptClienteFactura.DataSource = GetClientes(adapter);
                rptClienteFactura.DataBind();
            }
        }
    }
}