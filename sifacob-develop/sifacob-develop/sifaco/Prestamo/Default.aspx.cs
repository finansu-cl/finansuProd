using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Clases;
using System.Collections;
using System.Configuration;
using System.Data;
using Novacode;
using System.IO;
using System.Text;
using System.Reflection;
using System.Web.UI.HtmlControls;


namespace sifaco.Prestamo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "10";
            if (Request.QueryString["aid"] != null && Request.QueryString["val"] != null && Request.QueryString["val2"] != null)
            {
                if (!IsPostBack)
                {
                    int _oldEdoVal = string.IsNullOrEmpty(Request.QueryString["oldedoval"]) ? -1 : Convert.ToInt32(Request.QueryString["oldedoval"].ToString());
                    string fecha = string.IsNullOrEmpty(Request.QueryString["fecha"]) ? "" : Request.QueryString["fecha"].ToString();
                    EditarEdoCuota(Request.QueryString["aid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val2"].ToString(), "", fecha, _oldEdoVal);
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
                if (Request.QueryString["aid"] != null && Request.QueryString["flg"] != null && Request.QueryString["obser"] != null)
                {
                    if (!IsPostBack)
                    {
                        int _oldEdoVal = string.IsNullOrEmpty(Request.QueryString["oldedoval"]) ? -1 : Convert.ToInt32(Request.QueryString["oldedoval"].ToString());
                        string fecha = string.IsNullOrEmpty(Request.QueryString["fecha"]) ? "" : Request.QueryString["fecha"].ToString();
                        EditarEdoCuota(Request.QueryString["aid"].ToString(), "", "", Request.QueryString["flg"].ToString(), Request.QueryString["obser"].ToString(), _oldEdoVal);
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

                if (Request.QueryString["aid"] != null && Request.QueryString["val"] != null)
                {
                    if (!IsPostBack)
                    {
                        int _oldEdoVal = string.IsNullOrEmpty(Request.QueryString["oldedoval"]) ? -1 : Convert.ToInt32(Request.QueryString["oldedoval"].ToString());
                        string fecha = string.IsNullOrEmpty(Request.QueryString["fecha"]) ? "" : Request.QueryString["fecha"].ToString();
                        EditarEdoCuota(Request.QueryString["aid"].ToString(), Request.QueryString["val"].ToString(),"","","", _oldEdoVal);
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

            if (!IsPostBack)
            {
                try
                {
                    
                    if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                    {
                        string idCliente = Request.QueryString["cid"].ToString();
                        SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
                        Clientes cliente = GetClientes(adapterC).LastOrDefault();

                        ltrCliente.Text = cliente.Nombre;
                        ltrRutCliente.Text = cliente.Rut;

                        string idPrestamo = Request.QueryString["pid"].ToString();
                        idCliente = Request.QueryString["cid"].ToString();
                        SqlDataAdapter adapter2 = GetAdapterPrestamos(idPrestamo);
                        PrestamosA prestamo = GetPrestamos(adapter2).LastOrDefault();

                        if (pres != null)
                        {
                            Session["NumCuotasEval"] = prestamo.NumeroCuotas ?? 0;
                            pres.Monto = prestamo.Monto ?? 0;
                            pres.Tasa = prestamo.Tasa ?? 0;
                            pres.Plazo = prestamo.Plazo ?? 0;
                            pres.Cuota = prestamo.Cuota ?? 0;
                            pres.ID = prestamo.ID.ToString();
                            pres.MesGracia = prestamo.MesGracia ?? 0;
                            pres.FechaPres = prestamo.Fecha;
                            pres.NumCuotas = prestamo.NumeroCuotas ?? 0;
                            SqlDataAdapter adapter = GetAdapter(idPrestamo);
                            rptPrestamos.DataSource = GetTablaAmortizacion(adapter);
                            rptPrestamos.DataBind();

                        }

                    }
                    if (!Page.IsPostBack)
                    {
                        SqlDataAdapter adapter1 = GetAdapterDocumentos("002");
                        rptCrearDocs.DataSource = GetDocumentos(adapter1);
                        rptCrearDocs.DataBind();
                    }

                    //if (Session["rol"] != null && Session["rol"].ToString() == "user")
                    //{
                    //    foreach (RepeaterItem item in rptPrestamos.Items)
                    //    {
                    //        Button btnEdit = (Button)item.FindControl("btnEdit");
                    //        btnEdit.Enabled = false;
                    //        btnEdit.ToolTip = "Usted No posee permisos para modificar";
                    //    }
                    //}
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

        public void UpdateShareNoInterest(string id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ENTRE 1");
                System.Diagnostics.Debug.WriteLine("ENTRE ID:" + id);
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_DEL_AMORTIZACION_0_INTERES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CUOTA_PADRE", id);
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

        public void EditarEdoCuota(string id, string valEdo, string fechaPago, string flg, string observacion, int? oldEdoVal = null)
        {
            System.Diagnostics.Debug.WriteLine("id:" + id);
            System.Diagnostics.Debug.WriteLine("valEdo:" + valEdo);
            System.Diagnostics.Debug.WriteLine("fechaPago:" + fechaPago);
            System.Diagnostics.Debug.WriteLine("flg:" + flg);
            System.Diagnostics.Debug.WriteLine("observacion:" + observacion);
            System.Diagnostics.Debug.WriteLine("oldEdoVal:" + oldEdoVal);

            if (Session["login"] != null)
            {
                try
                {
                    if (oldEdoVal != null && oldEdoVal == 12 && valEdo != "34")
                    {
                        UpdateShareNoInterest(id);
                    }

                    int correlativo = GetLastCorrelativo();
                    Common cls = new Common();
                    string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                    string spName = "usr_fnns.SP_MOD_EDO_CUOTA";
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    ArrayList tipoDatos = new ArrayList();
                    parametros.Add("@ID", id.ToString());
                    tipoDatos.Add("int");
                    if (flg == "")
                    {
                        if (valEdo == "4")
                        {
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(".","").Replace(",",".").ToString()*/);
                            tipoDatos.Add("decimal");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(observacion).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "14")
                        {
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(".","").Replace(",",".").ToString()*/);
                            tipoDatos.Add("decimal");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(observacion).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "11")
                        {
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(",", ".")*/);
                            tipoDatos.Add("decimal");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(observacion).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "3")
                        {
                            if (fechaPago == "")
                            {
                                fechaPago = Convert.ToDateTime("1753-01-01 00:00:00").ToString();
                            }
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "33")
                        {

                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "34")
                        {

                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else if (valEdo == "12")
                        {
                            System.Diagnostics.Debug.WriteLine("ENTRE 0");
                            System.Diagnostics.Debug.WriteLine("ENTRE 0 valEdo:" + valEdo);
                            System.Diagnostics.Debug.WriteLine("ENTRE 0 oldEdoVal:" + oldEdoVal);
                            //Primero se genera nueva amortización asociada a la actual pero sin intereses y se actualizan las amortizaciones siguientes (fecha vencimiento original + 30 días)
                            string spName2 = "usr_fnns.SP_CREATE_AMORTIZACION_0_INTERES";
                            Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                            ArrayList tipoDatos2 = new ArrayList();
                            parametros2.Add("@ID_CUOTA", id.ToString());
                            tipoDatos2.Add("int");
                            cls.GetConnectionToDbNonQuery(conn, spName2, parametros2, tipoDatos2);
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////


                            //Ahora para actualizar la amortización actual///////////////////////////////////////////////////////////
                            DateTime fecha = DateTime.Now;
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fecha).ToString());
                            tipoDatos.Add("datetime");
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                        else if (valEdo == "10")
                        {
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                            parametros.Add("@FECHA_PAGO", Convert.ToDateTime(observacion).ToString());
                            tipoDatos.Add("datetime");
                        }
                        else
                        {
                            parametros.Add("@ID_EDO_PRES", valEdo);
                            tipoDatos.Add("int");
                        }
                    }
                    else
                    {
                        parametros.Add("@FLG_DES", flg);
                        tipoDatos.Add("varchar");
                        parametros.Add("@OBSERVACION", observacion);
                        tipoDatos.Add("varchar");
                    }
                    parametros.Add("@NAME_USER", Session["login"].ToString());
                    parametros.Add("@ORIGEN", "PRESTAMOS");
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
            
        }

        public int EditarEdoCuotaReturn(string id, string valEdo, string fechaPago, string flg, string observacion, int? oldEdoVal = null)
        {
            try
            {

                if (oldEdoVal != null && oldEdoVal == 12)
                {
                    UpdateShareNoInterest(id);
                }

                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EDO_CUOTA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", id.ToString());
                tipoDatos.Add("int");
                if (flg == "")
                {
                    if (valEdo == "4")
                    {
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(".","").Replace(",",".").ToString()*/);
                        tipoDatos.Add("decimal");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(observacion).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else if (valEdo == "11")
                    {
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@PAGO_PARCIAL", fechaPago/*.Replace(",", ".")*/);
                        tipoDatos.Add("decimal");
                    }
                    else if (valEdo == "3")
                    {
                        if (fechaPago == "")
                        {
                            fechaPago = Convert.ToDateTime("1753-01-01 00:00:00").ToString();
                        }
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                        tipoDatos.Add("datetime");
                    }
                    else if (valEdo == "12")
                    {

                        //Primero se genera nueva amortización asociada a la actual pero sin intereses y se actualizan las amortizaciones siguientes (fecha vencimiento original + 30 días)
                        string spName2 = "usr_fnns.SP_CREATE_AMORTIZACION_0_INTERES";
                        Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                        ArrayList tipoDatos2 = new ArrayList();
                        parametros2.Add("@ID_CUOTA", id.ToString());
                        tipoDatos2.Add("int");
                        cls.GetConnectionToDbNonQuery(conn, spName2, parametros2, tipoDatos2);
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////


                        //Ahora para actualizar la amortización actual///////////////////////////////////////////////////////////
                        DateTime fecha = DateTime.Now;
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fecha).ToString());
                        tipoDatos.Add("datetime");
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else if (valEdo == "10")
                    {
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                        parametros.Add("@OBSERVACION", observacion);
                        tipoDatos.Add("int");
                    }
                    else
                    {
                        parametros.Add("@ID_EDO_PRES", valEdo);
                        tipoDatos.Add("int");
                    }
                }
                else
                {
                    parametros.Add("@FLG_DES", flg);
                    tipoDatos.Add("varchar");
                    parametros.Add("@OBSERVACION", observacion);
                    tipoDatos.Add("varchar");
                }
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@ORIGEN", "PRESTAMOS");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                tipoDatos.Add("varchar");
                tipoDatos.Add("varchar");
                tipoDatos.Add("int");
                SqlDataAdapter reader = cls.GetConnectionToDb(conn, spName, parametros, tipoDatos);
                DataTable dt = new DataTable();
                reader.Fill(dt);
                int idPrestamo = Convert.ToInt32(dt.Rows[0]["ID"]);
                return idPrestamo;
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

        public SqlDataAdapter GetAdapter(string idPrestamo)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_AMORTIZACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_PRESTAMO", idPrestamo);
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

        public List<PrestamosA> GetTablaAmortizacion(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<PrestamosA> re = null;
            re = (from item in dt.AsEnumerable()
                  select new PrestamosA()
                  {
                      ID = item.Field<int>("ID"),
                      IdEdoPres = item.Field<int>("ID_EDO_PRES"),
                      NumCuota = item.Field<decimal?>("NUM_CUOTA"),
                      SaldoInicial = item.Field<decimal?>("MONTO_INI"),
                      Cuota = item.Field<decimal?>("CUOTA"),
                      Intereses = item.Field<decimal?>("INTERESES"),
                      Capital = item.Field<decimal?>("CAPITAL"),
                      CapitalParcial = item.Field<decimal?>("CAPITAL_PARCIAL"),
                      Parcial = item.Field<decimal?>("PAGO_PARCIAL"),
                      SaldoFinal = item.Field<decimal>("MONTO_FIN"),
                      Mora = item.Field<decimal>("INTERES_MORA"),
                      FechaPago = item.Field<DateTime?>("FECHA_PAGO"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      Observacion = item.Field<string>("OBSERVACION"),
                      Tasa = item.Field<decimal?>("TASA"),
                      NumTotalCuota = item.Field<int>("NUM_CUOTAS"),
                      Monto = item.Field<decimal>("MONTO"),
                      PagoParcialMora = item.Field<decimal>("PAGO_PARCIAL_MORA"),
                      MontoRestanteMora = item.Field<decimal>("MONTO_RESTANTE_MORA"),
                      PagoParcialIntereses = item.Field<decimal>("PAGO_PARCIAL_INTERESES"),
                      MontoRestanteIntereses = item.Field<decimal>("MONTO_RESTANTE_INTERESES")
                  }).ToList();
            return re;
        }

        public void GetAdapterCreateAmortizacion(string idPrestamo, string idEdoPres, string numCuota, decimal montoIni, decimal cuota, decimal intereses, decimal capital, decimal montoFin, decimal mora, DateTime vencimiento, int gracia)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_CREATE_AMORTIZACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_PRESTAMO", idPrestamo);
                parametros.Add("@ID_EDO_PRES", idEdoPres);
                parametros.Add("@NUM_CUOTA", numCuota);
                parametros.Add("@MONTO_INI", montoIni.ToString().Replace(".", "."));
                parametros.Add("@CUOTA", cuota.ToString().Replace(".", "."));
                parametros.Add("@INTERESES", intereses.ToString().Replace(".", "."));
                parametros.Add("@CAPITAL", capital.ToString().Replace(".", "."));
                parametros.Add("@MONTO_FIN", montoFin.ToString().Replace(".", "."));
                parametros.Add("@INTERES_MORA", mora.ToString().Replace(".", "."));
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@VENCIMIENTO", vencimiento.AddMonths(gracia).ToShortDateString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                cls.GetConnectionToDbInsert(conn, spName, parametros, tipoDatos);
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

        public void EditarPrestamo(string id, string idCliente, string tasa, string plazo, string monto, string cuota, string gracia, DateTime fechaPrest, int numCuotas)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                string monto2 = monto.Replace(".", "");
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_PRESTAMO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id);
                parametros.Add("@ID_CLIENTE", idCliente);
                parametros.Add("@TASA", tasa);
                parametros.Add("@MES_GRACIA", gracia);
                parametros.Add("@PLAZO", plazo);
                parametros.Add("@MONTO", monto);
                parametros.Add("@CUOTA", cuota);
                parametros.Add("@NAME_USER", Session["login"].ToString());
                parametros.Add("@FECHA_PRESTAMO", fechaPrest.ToShortDateString());
                parametros.Add("@NUM_CUOTAS", numCuotas.ToString());
                parametros.Add("@ORIGEN", "REPRESENTANTES");
                parametros.Add("@CORRELATIVO", correlativo.ToString());
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("int");
                tipoDatos.Add("int");
                tipoDatos.Add("decimal");
                tipoDatos.Add("decimal");
                tipoDatos.Add("varchar");
                tipoDatos.Add("datetime");
                tipoDatos.Add("int");
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

        protected void btnModificarFac_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string idPrestamo = Request.QueryString["pid"].ToString();
                EditarPrestamo(idPrestamo, idCliente, pres.Tasa.ToString(), pres.Plazo.ToString(), pres.Monto.ToString(),pres.Cuota.ToString(), pres.MesGracia.ToString(),pres.FechaPres,pres.NumCuotas);
                CalculoPrestamo(pres.FechaPres);
                SqlDataAdapter adapter = GetAdapter(idPrestamo);
                rptPrestamos.DataSource = GetTablaAmortizacion(adapter);
                rptPrestamos.DataBind();
            }
        }

        public void CalculoPrestamo(DateTime fechaPres)
        {
            string idPrestamo = Request.QueryString["pid"].ToString();
            decimal monto = pres.Monto;
            decimal cuota = pres.Cuota;
            decimal tasa = pres.Tasa;
            int plazo = pres.NumCuotas;//Math.Ceiling(Convert.ToDouble(pres.Plazo)/Convert.ToDouble(30.00));
            int plz = pres.Plazo;
            int constPlazo = 30; //equivale a 1 mes
            int res = pres.Plazo % constPlazo;
            int ent = pres.Plazo / constPlazo;
            int diaPlazo = plz / plazo;
            int diaPlazoFinal = (plz - (diaPlazo * (plazo - 1)));
            int mesGracia = pres.MesGracia;
            ArrayList si = new ArrayList();
            DateTime venc = fechaPres;
            for (int i = 1; i <= plazo; i++) 
            {
                if (i != plazo)
                    venc = venc.AddDays(diaPlazo);
                else
                    venc = venc.AddDays(diaPlazoFinal);
                //venc = venc.AddMonths(1);
                /*if (ent >= i)
                {*/
                    if (i == 1)
                        si.Add(monto);  //pos 0
                    else
                    {
                        si.Add(Convert.ToDecimal(si[i - 2]) - (cuota - (Convert.ToDecimal(si[i - 2]) * (tasa / 100))));
                    }

                    GetAdapterCreateAmortizacion(idPrestamo, "1", i.ToString(), Convert.ToDecimal(si[i - 1]), cuota, Convert.ToDecimal(si[i - 1]) * (tasa / 100), cuota - (Convert.ToDecimal(si[i - 1]) * (tasa / 100)), (i != plazo) ? Convert.ToDecimal(si[i - 1]) - (cuota - (Convert.ToDecimal(si[i - 1]) * (tasa / 100))) : 0, 0, venc, mesGracia);
               /* }
                else 
                {

                    if (i == 1)
                        si.Add(monto);
                    else
                    {
                        si.Add(Convert.ToDecimal(si[i - 2]) - (cuota - (Convert.ToDecimal(si[i - 2]) * (tasa / 100))));
                    }

                    cuota = Convert.ToDecimal(si[i - 1]) + (((Convert.ToDecimal(si[i - 1]) * (tasa / 100)) / 30) * res);
                    GetAdapterCreateAmortizacion(idPrestamo, "1", i.ToString(), Convert.ToDecimal(si[i - 1]), cuota, (((Convert.ToDecimal(si[i - 1]) * (tasa / 100)) / 30) * res), cuota - (((Convert.ToDecimal(si[i - 1]) * (tasa / 100)) / 30) * res), (i != plazo) ? Convert.ToDecimal(si[i - 1]) - (cuota - (((Convert.ToDecimal(si[i - 1]) * (tasa / 100)) / 30) * res)) : 0, 0, venc, mesGracia);
                }*/
                
            }
        }

        public SqlDataAdapter GetAdapterPrestamos(string id)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PRESTAMOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", id);
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

        public List<PrestamosA> GetPrestamos(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<PrestamosA> re = null;
            re = (from item in dt.AsEnumerable()
                  select new PrestamosA()
                  {
                      ID = item.Field<int>("ID"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("Nombre"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Tasa = item.Field<decimal?>("TASA") ?? 0,
                      Plazo = item.Field<int>("PLAZO"),
                      NumeroCuotas = item.Field<int?>("NUM_CUOTAS") ?? 0,
                      MesGracia = item.Field<int?>("MES_GRACIA") ?? 0,
                      Cuota = item.Field<decimal?>("CUOTA") ?? 0,
                      Monto = item.Field<decimal?>("MONTO") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA")
                  }).ToList();
            return re;
        }

        //protected void btnCrearDocs_Click(object sender, EventArgs e)
        //{
        //    if (!print.Pagare && !print.Tabla)
        //    {
        //        Response.Write("<script>alert('Debe seleccionar al menos 1 documento');</script>");
        //    }
        //    else
        //    {
        //        if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
        //        {
        //            if (print.Mutuo)
        //            {
        //                CreateDocumentWord("MUTUO");
        //            }
        //            if (print.Pagare)
        //            {
        //                CreateDocumentWord("PAGARE");
        //            }
        //            if (print.Tabla)
        //            {
        //                CreateDocumentWord("TABLA");
        //            }
        //        }
        //    }
        //}
        protected void btnCrearDocs_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (RepeaterItem ri in rptCrearDocs.Items)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)ri.FindControl("chkDoc");
                HiddenField hd = (HiddenField)ri.FindControl("hdItem");
                if (chk.Checked)
                {
                    if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                    {
                        CreateDocumentWord(hd.Value);
                    }
                    count = count + 1;
                }
            }
            if (count == 0)
            {
                Response.Write("<script>alert('Debe seleccionar al menos 1 documento');</script>");
            }
        }

        //protected void btnCrearDocsEsp_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string direccion = PrestamoPersona.Region + ";" + PrestamoPersona.Ciudad + ";" + PrestamoPersona.Comuna + ";" + PrestamoPersona.Direccion;
        //        /*int idCliente = InsertClientePersona(PrestamoPersona.Rut, PrestamoPersona.Nombre, PrestamoPersona.EdoCivil, PrestamoPersona.Nacionalidad, direccion, PrestamoPersona.RutEmpresa, PrestamoPersona.Empresa, datosAval.Rut, datosAval.Nombre);
        //        SqlDataAdapter adapter = GetAdapter();
        //        rptGrid.DataSource = GetClientes(adapter);
        //        rptGrid.DataBind();
        //        LimpiarForm();*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string m = ex.Message;
        //        string logFile = "~/Log/ErrorLog.txt";
        //        logFile = HttpContext.Current.Server.MapPath(logFile);
        //        Common cls = new Common();
        //        cls.LogFile(m, logFile, ex);
        //        //return 0;
        //    }

        //    //int count = 0;
        //    //foreach (RepeaterItem ri in rptCrearDocs.Items)
        //    //{
        //    //    CheckBox chk = (CheckBox)ri.FindControl("chkDoc");
        //    //    HiddenField hd = (HiddenField)ri.FindControl("hdItem");
        //    //    if (chk.Checked)
        //    //    {
        //    //        if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
        //    //        {
        //    //            CreateDocumentWord(hd.Value);
        //    //        }
        //    //        count = count + 1;
        //    //    }
        //    //}
        //    //if (count == 0)
        //    //{
        //    //    Response.Write("<script>alert('Debe seleccionar al menos 1 documento');</script>");
        //    //}
        //}

        public int InsertClientePersona(string rut, string nombre, string edoCivil, string sexo, string profesion, string nacionalidad, string direccion, string email, string telefono, string celular, string banco, string destinatario, string rutDest, string numCta, string rutAval, string nombreAval)
        {
            int id = 0;
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                #region Parametros Personas
                string spName = "usr_fnns.SP_CREATE_PERSONAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
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
                parametros.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos = new ArrayList();
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
                int idPersona = Convert.ToInt32(dt.Rows[0]["ID"]);
                #endregion
                #region Parametros Cliente
                string spName1 = "usr_fnns.SP_CREATE_CLIENTES";
                Dictionary<string, string> parametros1 = new Dictionary<string, string>();
                parametros1.Add("@ID_PERSONA", idPersona.ToString());
                parametros1.Add("@ID_EMPRESA", "0");
                parametros1.Add("@NUM_OPERACION", "1");
                parametros1.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos1 = new ArrayList();
                tipoDatos1.Add("int");
                tipoDatos1.Add("int");
                tipoDatos1.Add("int");
                tipoDatos1.Add("varchar");
                SqlDataAdapter reader1 = cls.GetConnectionToDb(conn, spName1, parametros1, tipoDatos1);
                DataTable dt1 = new DataTable();
                reader1.Fill(dt1);
                int idCliente = Convert.ToInt32(dt1.Rows[0]["ID"]);
                #endregion
                #region Parametros Datos Banco
                string spName2 = "usr_fnns.SP_CREATE_DATOS_BANCO";
                Dictionary<string, string> parametros2 = new Dictionary<string, string>();
                parametros2.Add("@ID_CLIENTE", idCliente.ToString());
                parametros2.Add("@ID_BANCO", banco);
                parametros2.Add("@NUM_CUENTA", numCta);
                parametros2.Add("@DESTINATARIO", destinatario);
                parametros2.Add("@RUT", rutDest);
                parametros2.Add("@CORREO", email);
                parametros2.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos2 = new ArrayList();
                tipoDatos2.Add("int");
                tipoDatos2.Add("int");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                tipoDatos2.Add("varchar");
                SqlDataAdapter reader2 = cls.GetConnectionToDb(conn, spName2, parametros2, tipoDatos2);
                DataTable dt2 = new DataTable();
                reader2.Fill(dt2);
                int idDatosBanco = Convert.ToInt32(dt2.Rows[0]["ID"]);
                #endregion
                #region Parametros Datos Aval
                string spName3 = "usr_fnns.SP_CREATE_AVAL";
                Dictionary<string, string> parametros3 = new Dictionary<string, string>();
                parametros3.Add("@ID_CLIENTE", idCliente.ToString());
                parametros3.Add("@NOMBRE", nombreAval);
                parametros3.Add("@RUT", rutAval);
                parametros3.Add("@NAME_USER", Session["login"].ToString());
                ArrayList tipoDatos3 = new ArrayList();
                tipoDatos3.Add("int");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                tipoDatos3.Add("varchar");
                SqlDataAdapter reader3 = cls.GetConnectionToDb(conn, spName3, parametros3, tipoDatos3);
                DataTable dt3 = new DataTable();
                reader3.Fill(dt3);
                int idDatosAval = Convert.ToInt32(dt3.Rows[0]["ID"]);
                #endregion
                return idCliente;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                string logFile = "~/Log/ErrorLog.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);
                Common cls = new Common();
                cls.LogFile(m, logFile, ex);
                return id;
            }
        }

        public SqlDataAdapter GetAdapter()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT_EMPRESA", "0");
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

        public void CreateDocumentWord(string nameTemplate) 
        {
            Common cls = new Common();
            string idCliente = Request.QueryString["cid"].ToString();
            string idPrestamo = Request.QueryString["pid"].ToString();
            SqlDataAdapter adapterPres = GetAdapter(idPrestamo);
            List<PrestamosA> prestamo = GetTablaAmortizacion(adapterPres);
            SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
            Clientes cliente = GetClientes(adapterC).LastOrDefault();
            SqlDataAdapter adapterS = null;//GetAdapterSelLastSim(idSimulacion, null);
            Simulaciones simulacion = null;// GetSimulaciones(adapterS).LastOrDefault();
            SqlDataAdapter adapterDB = GetAdapterSelDatosBanco(idCliente);
            DatosBanco datosBanco = GetDatosBanco(adapterDB).FirstOrDefault();
            SqlDataAdapter adapterCom = GetAdapterSelCompareciente(Convert.ToInt32(idCliente));
            Compareciente datosCom = GetCompareciente(adapterCom).FirstOrDefault();
            SqlDataAdapter adapterAval = GetAdapterSelDatosAval(idCliente);
            Avales datosAval = GetDatosAval(adapterAval).FirstOrDefault();
            SqlDataAdapter adapterGarantiaV = GetAdapterGarantiasVXClientes(Convert.ToInt32(idCliente));
            PrendasV datosVGarantia = GetGarantiaV(adapterGarantiaV).FirstOrDefault();
            SqlDataAdapter adapterGarantiaH = GetAdapterGarantiasHXClientes(Convert.ToInt32(idCliente));
            PrendasH datosHGarantia = GetGarantiaH(adapterGarantiaH).FirstOrDefault();
            string nombreCliente = "";
            string fechaDdMm = "";
            string primerVencimiento = "";
            string tipoEmpresa = "";
            string giroComercial = "";
            string rutCliente = "";
            string datosRep = "";
            string sustantivoDir = "";
            string direccion = "";
            string precioCesion = "";
            string deudor = "";
            string representante = "";
            string saldoPendiente = "";
            string fechaEscritura = "";
            string notaria = "";
            string montoTotal = "";
            string rutRep = "";
            string nacionalidad = "";
            string edoCivil = "";
            string sustantivoPer = "";
            string numRutCliente = "";
            string banco = "";
            string numCta = "";
            string plazo = "";
            string anticipo = "";
            string salPendiente = "";
            string montoGir = "";
            string nombreAval = "";
            string rutAval = "";
            string nombreCom = "";
            string rutCom = "";
            string profesionCom = ""; 
            string nacionalidadCom = "";
            string edoCivilCom = "";
            string sexoCom = "";
            string emailCom = "";
            string direccionCom = ""; 
            string telefonoCom = "";
            string celularCom = "";
            string empresaCom = "";
            string rutEmpresaCom = "";
            string direccionEmpresaCom = "";
            string rutInscritoGV = "";
            string nombreInscritoGV = "";
            string tipoGV = "";
            string marcaGV = "";
            string modeloGV = "";
            string anoGV = "";
            string motorGV = "";
            string chasisGV = "";
            string colorGV = "";
            string patenteGV = "";
            string rvmGV = "";
            string notariaGV = "";
            string fechaEscrituraGV = "";
            string deslindesGH = "";
            string nombreComproGH = "";
            string nombreNotarioGH = "";
            string fojasGH = "";
            string numeroGH = "";
            string ubicacionCbrsGH = "";
            string anoGH = "";
            string comunaGH = "";
            string rolGH = "";
            string fechaEscrituraGH = "";
            string numHipoteca = "";
            string avalSiNo = "";
            ArrayList ordinales = cls.NumOrdinales();
            //PRENDA_DESPLAZAMIENTO
            //CANCELACION_PRENDA
            //MUTUO_HIPOTECARIO
            //MANDATO_IRREVOCABLE
            if (datosCom != null)
            {
                #region Set DatosCom
                string[] dirCom = datosCom.Direccion.Split(';');
                SqlDataAdapter adapterDCOm = GetAdapterDireccion(dirCom[0].ToString(), dirCom[1].ToString(), dirCom[2].ToString());
                direccionCom = dirCom[3].ToString() + " " + GetDireccion(adapterDCOm).LastOrDefault().Direcciones;
                string[] dirComEmp = datosCom.DireccionEmpresa.Split(';');
                SqlDataAdapter adapterDCOmEmp = GetAdapterDireccion(dirComEmp[0].ToString(), dirComEmp[1].ToString(), dirComEmp[2].ToString());
                direccionEmpresaCom = dirComEmp[3].ToString() + " " + GetDireccion(adapterDCOmEmp).LastOrDefault().Direcciones;

                nombreCom = datosCom.Nombre;
                rutCom = datosCom.Rut;
                profesionCom = datosCom.Profesion;
                nacionalidadCom = datosCom.Nacionalidad;
                edoCivilCom = datosCom.EdoCivil;
                sexoCom = datosCom.Sexo;
                emailCom = datosCom.Email;
                telefonoCom = datosCom.Telefono;
                celularCom = datosCom.Celular;
                empresaCom = datosCom.Empresa;
                rutEmpresaCom = datosCom.RutEmpresa;
                #endregion
            }
            if (datosHGarantia != null)
            {
                #region Set DatosHGarantia
                deslindesGH = datosHGarantia.Deslindes;
                nombreComproGH = datosHGarantia.NombreCompro;
                nombreNotarioGH = datosHGarantia.NombreNotario;
                fojasGH = datosHGarantia.Fojas;
                numeroGH = datosHGarantia.Numero;
                ubicacionCbrsGH = datosHGarantia.UbicacionCbrs;
                anoGH = datosHGarantia.Ano;
                comunaGH = datosHGarantia.Comuna;
                rolGH = datosHGarantia.Rol;
                fechaEscrituraGH = datosHGarantia.FechaEscritura.ToString();
                numHipoteca = ordinales[Convert.ToInt32(GetGarantiaH(adapterGarantiaH).Count.ToString())-1].ToString();
                #endregion
            }
            if (datosVGarantia != null) 
            {
                #region Set DatosVGarantia
                rutInscritoGV = datosVGarantia.RutInscrito;
                nombreInscritoGV = datosVGarantia.NombreInscrito;
                tipoGV = datosVGarantia.Tipo;
                marcaGV = datosVGarantia.Marca;
                modeloGV = datosVGarantia.Modelo;
                anoGV = datosVGarantia.Ano;
                motorGV = datosVGarantia.Motor;
                chasisGV = datosVGarantia.Chasis;
                colorGV = datosVGarantia.Color;
                patenteGV = datosVGarantia.Patente;
                rvmGV = datosVGarantia.Rvm;
                notariaGV = datosVGarantia.Notaria;
                fechaEscrituraGV = datosVGarantia.FechaEscritura.ToString();
                #endregion
            }

            if (cliente.IdPersona == 0)
            {
                #region Documentos para Empresa
                SqlDataAdapter adapterE = GetAdapterSelEmpresa(cliente.Rut);
                Empresa empresas = GetEmpresa(adapterE).LastOrDefault();
                SqlDataAdapter adapterR = GetAdapterRepresentantes(empresas.ID.ToString());
                List<Persona> representantes = GetPersonas(adapterR);
                string represen = "";
                string repDatos = "";
                string sust = "";
                int num = 0;
                foreach (var rep in representantes)
                {
                    num++;
                    if (num == 1)
                        sust = "domiciliado";
                    if (num == 2)
                        sust = "ambos domiciliados";
                    if (num > 2)
                        sust = "todos domiciliados";

                    string sinDon = "";
                    string edo = "";
                    if (rep.Sexo == "M")
                    {
                        sinDon = "don ";
                    }
                    else
                    {
                        if (num == 1)
                            sust = "domiciliada";
                        sinDon = "doña ";
                    }

                    switch (rep.EdoCivil)
                    {
                        case "SO":
                            edo = "soltero";
                            break;
                        case "CA":
                            edo = "casado";
                            break;
                        case "VI":
                            edo = "viudo";
                            break;
                        case "DI":
                            edo = "divorciado";
                            break;
                        case "CC":
                            edo = "conviviente civil";
                            break;
                    }

                    string[] rutR = rep.Rut.Replace(".", "").Split('-');
                    string rutLetras = cls.NumberToWords(Convert.ToInt32(rutR[0]));
                    rutLetras += " guion ";
                    if (rutR[1].ToString() == "k" || rutR[1].ToString() == "K")
                        rutLetras += "K";
                    else
                        rutLetras += cls.NumberToWords(Convert.ToInt32(rutR[1]));
                    repDatos += sinDon + rep.Nombre + ", " + rep.Nacionalidad + ", " + edo + ", factor de comercio, Cédula Nacional de identidad número " + rutLetras + " ";
                    represen += rep.Nombre + ", ";
                    rutRep += rep.Rut + ", ";
                    nacionalidad += rep.Nacionalidad + ", ";
                }
                string nombreDeudor = "";
                nombreCliente = empresas.RazonSocial;
                fechaDdMm = String.Format("{0:dd MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                tipoEmpresa = empresas.Tipo;//DEBE TRAER EL TIPO DE EMPRESA
                giroComercial = empresas.Giro;
                string[] rutC = empresas.Rut.Replace(".", "").Split('-');
                string rutCLetras = cls.NumberToWords(Convert.ToInt32(rutC[0]));
                rutCLetras += " guion ";
                int n;
                bool isNumeric = int.TryParse(rutC[1], out n);
                rutCLetras += (isNumeric) ? cls.NumberToWords(Convert.ToInt32(rutC[1])) : rutC[1];
                rutCliente = rutCLetras;
                datosRep = repDatos;
                sustantivoDir = sust;
                string[] dir = empresas.Direccion.Split(';');
                SqlDataAdapter adapterD = GetAdapterDireccion(dir[0].ToString(), dir[1].ToString(), dir[2].ToString());
                direccion = dir[3].ToString() + " " + GetDireccion(adapterD).LastOrDefault().Direcciones;
                deudor = nombreDeudor;
                representante = represen;
                fechaEscritura = String.Format("{0:dd MMMM yyyy}",Convert.ToDateTime(empresas.FechaEscritura));
                notaria = empresas.NombreNotaria;
                montoTotal = decimal.Parse(pres.Monto.ToString()).ToString("N");
                banco = datosBanco.Banco;
                numCta = datosBanco.NumCuenta;
                numRutCliente = empresas.Rut;
                plazo = pres.Plazo.ToString();
                #endregion
            }
            else
            {
                #region Documentos para Personas
                SqlDataAdapter adapterP = GetAdapterSelPersona(cliente.Rut);
                Persona persona = GetPersonas(adapterP).LastOrDefault();
                string sinDon = "";
                string edo = "";
                if (persona.Sexo == "M")
                {
                    sinDon = "don ";
                }
                else
                {
                    sinDon = "doña ";
                }
                switch (persona.EdoCivil)
                {
                    case "SO":
                        edo = "soltero";
                        break;
                    case "CA":
                        edo = "casado";
                        break;
                    case "VI":
                        edo = "viudo";
                        break;
                    case "DI":
                        edo = "divorciado";
                        break;
                    case "CC":
                        edo = "conviviente civil";
                        break;
                }
                string nombreDeudor = "";

                nombreCliente = persona.Nombre;
                fechaDdMm = String.Format("{0:dd MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                string[] rutC = persona.Rut.Replace(".", "").Split('-');
                string rutCLetras = cls.NumberToWords(Convert.ToInt32(rutC[0]));
                rutCLetras += " guion ";
                if (rutC[1].ToString() == "k" || rutC[1].ToString() == "K")
                    rutCLetras += "K";
                else
                    rutCLetras += cls.NumberToWords(Convert.ToInt32(rutC[1]));

                rutCliente = rutCLetras;
                string[] dir = persona.Direccion.Split(';');
                SqlDataAdapter adapterD = GetAdapterDireccion(dir[0].ToString(), dir[1].ToString(), dir[2].ToString());
                direccion = dir[3].ToString() + " " + GetDireccion(adapterD).LastOrDefault().Direcciones;
                deudor = nombreDeudor;
                montoTotal = decimal.Parse(pres.Monto.ToString()).ToString("N");
                edoCivil = edo;
                sustantivoPer = sinDon;
                nacionalidad = persona.Nacionalidad;
                numRutCliente = persona.Rut;
                banco = datosBanco.Banco;
                numCta = datosBanco.NumCuenta;
                plazo = pres.Plazo.ToString();
                #endregion
            }
            if (datosAval != null)
            {
                #region Set DatosAval
                nombreAval = datosAval.Nombre;
                rutAval = datosAval.Rut;
                avalSiNo = "Presente a este acto " + datosAval.Nombre + ", " + datosAval.Rut + ", quien viene en constituirse en aval fiador y codeudor solidario de todas y cada una de las obligaciones que " + nombreCliente + " adquieran en el futuro o hayan adquirido con SERVICIOS FINANCIEROS SUMAR SpA.";
                #endregion
            }

            ReplaceWord(nameTemplate, nombreCliente, fechaDdMm, tipoEmpresa, giroComercial, rutCliente, datosRep, sustantivoDir, direccion, precioCesion, deudor, representante, saldoPendiente, fechaEscritura, notaria, montoTotal, rutRep, nacionalidad, edoCivil, sustantivoPer, numRutCliente, banco, numCta, prestamo, plazo, anticipo, salPendiente, montoGir, nombreAval, rutAval, nombreCom,rutCom, profesionCom, nacionalidadCom, edoCivilCom, sexoCom, emailCom, direccionCom, telefonoCom, celularCom, empresaCom, rutEmpresaCom, direccionEmpresaCom, rutInscritoGV, nombreInscritoGV, tipoGV, marcaGV, modeloGV, anoGV, motorGV, chasisGV, colorGV, patenteGV, rvmGV, notariaGV, fechaEscrituraGV, deslindesGH, nombreComproGH, nombreNotarioGH, fojasGH, numeroGH, ubicacionCbrsGH, anoGH, comunaGH, rolGH, fechaEscrituraGH, numHipoteca, avalSiNo);
            if (nameTemplate == "MUTUO_ACUERDO")
            {
                ifr1.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
            }
            if (nameTemplate == "PAGARE")
            {
                ifr2.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
            }
            if (nameTemplate == "TABLA")
            {
                ifr3.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
            }
            if (nameTemplate != "MUTUO_ACUERDO" && nameTemplate != "PAGARE" && nameTemplate != "TABLA")
            {
                ifr9.Attributes.Add("src", "../Handler/DownloadFile.ashx?fname=" + nameTemplate);
            }
        }

        public void ReplaceWord(string nameTemplate, string nombreCliente, string fechaDdMm, string tipoEmpresa, string giroComercial, string rutCliente, string datosRep, string sustantivoDir, string direccion, string precioCesion, string deudor, string representante, string saldoPendiente, string fechaEscr, string notaria, string montoTotal, string rutRep, string nacionalidad, string edoCivil, string sustantivoPer, string numRutCliente, string banco, string numCta, List<PrestamosA> prestamo, string plazo, string cuota, string salPendiente, string montoGir, string nombreAval, string rutAval, string nombreCom, string rutCom, string profesionCom, string nacionalidadCom, string edoCivilCom, string sexoCom, string emailCom, string direccionCom, string telefonoCom, string celularCom, string empresaCom, string rutEmpresaCom, string direccionEmpresaCom, string rutInscritoGV, string nombreInscritoGV, string tipoGV, string marcaGV, string modeloGV, string anoGV, string motorGV, string chasisGV, string colorGV, string patenteGV, string rvmGV, string notariaGV, string fechaEscrituraGV, string deslindesGH, string nombreComproGH, string nombreNotarioGH, string fojasGH, string numeroGH, string ubicacionCbrsGH, string anoGH, string comunaGH, string rolGH, string fechaEscrituraGH, string numHipoteca, string avalSiNo)
        {
            //Templates
            Common cls = new Common();
            DocX document = DocX.Load(Server.MapPath("../App_Data/Template/" + nameTemplate + ".docx"));

            if (nameTemplate == "TABLA")
            {
                document.ReplaceText("<<Tasa>>", pres.Tasa.ToString()+"%");
                document.ReplaceText("<<Plazo>>", plazo);
                document.ReplaceText("<<Monto>>", montoTotal);
                int num = 0;
                foreach (var tab in prestamo)
                {
                    num++;
                    int? plz = tab.Plazo;
                    DateTime? fechaVenc = tab.Vencimiento;
                    document.ReplaceText("<<ValorCuota>>", "$"+Convert.ToDecimal(tab.Cuota.ToString()).ToString("N"));
                    document.ReplaceText("<<Cuota" + num + ">>", tab.NumCuota.ToString());
                    document.ReplaceText("<<ValCuota" + num + ">>", "$"+Convert.ToDecimal(tab.Cuota.ToString()).ToString("N"));
                    document.ReplaceText("<<Monto" + num + ">>", "$"+Convert.ToDecimal(tab.SaldoInicial.ToString()).ToString("N"));
                    document.ReplaceText("<<Interes" + num + ">>", "$"+Convert.ToDecimal(tab.Intereses.ToString()).ToString("N"));
                    document.ReplaceText("<<Capital" + num + ">>", "$"+Convert.ToDecimal(tab.Capital.ToString()).ToString("N"));
                    document.ReplaceText("<<SaldoFinal" + num + ">>", "$"+Convert.ToDecimal(tab.SaldoFinal.ToString()).ToString("N"));

                }
                if (num < 61) 
                {
                    while(num<=60)
                    {
                        num++;
                        document.ReplaceText("<<Cuota" + num + ">>", "");
                        document.ReplaceText("<<ValCuota" + num + ">>", "");
                        document.ReplaceText("<<Monto" + num + ">>", "");
                        document.ReplaceText("<<Interes" + num + ">>","");
                        document.ReplaceText("<<Capital" + num + ">>", "");
                        document.ReplaceText("<<SaldoFinal" + num + ">>", "");
                    }
                }
            }

            if (nameTemplate == "PAGARE")
            {
                ArrayList ordinales = cls.NumOrdinales();
                document.ReplaceText("<<NombreCliente>>", nombreCliente);
                document.ReplaceText("<<RutCliente>>", numRutCliente);
                document.ReplaceText("<<Direccion>>", direccion);
                document.ReplaceText("<<PlazoLetras>>", cls.NumberToWords(Convert.ToInt32(plazo)));
                document.ReplaceText("<<Plazo>>", plazo);
                document.ReplaceText("<<Monto>>", montoTotal);

                int num = 0;
                foreach (var tab in prestamo)
                {
                    num++;
                    int? plz = tab.Plazo;
                    DateTime? fechaVenc = tab.Vencimiento;
                    document.ReplaceText("<<numCuotaL" + num + ">>", ordinales[Convert.ToInt32(tab.NumCuota)-1].ToString());
                    document.ReplaceText("<<cuota" + num + ">>", "$"+Convert.ToDecimal(tab.Cuota.ToString()).ToString("N"));
                    document.ReplaceText("<<FechaVencL" + num + ">>", String.Format("{0:dd MMMM yyyy}",Convert.ToDateTime(tab.Vencimiento)));
                }
                if (num < 61) 
                {
                    while(num<=60)
                    {
                        num++;
                        document.ReplaceText("<<numCuotaL" + num + ">>", "");
                        document.ReplaceText("<<cuota" + num + ">>", "");
                        document.ReplaceText("<<FechaVencL" + num + ">>", "");
                    }
                }
            }
            #region Template AUTOMATICA
            if (nameTemplate != "MUTUO_ACUERDO" && nameTemplate != "PAGARE" && nameTemplate != "TABLA")
            {

                var prest = prestamo.FirstOrDefault();
                string[] valCuotaL = prest.Cuota.ToString().Replace(".", "").Split(',');
                string valLetras = cls.NumberToWords(Convert.ToInt32(valCuotaL[0]));
                int diaVenc = prestamo.FirstOrDefault().Vencimiento.Value.Day;
                string primerVencimiento = cls.NumberToWords(diaVenc);//HAY QUE FORMATEARLA BIEN
                string[] monL = prest.Monto.ToString().Replace(".", "").Split(',');
                string montoLetras = cls.NumberToWords(Convert.ToInt32(monL[0])) + " con " + cls.NumberToWords(Convert.ToInt32(monL[1]));
                primerVencimiento += " de " + String.Format("{0:MMMM}", prestamo.FirstOrDefault().Vencimiento); //HAY QUE FORMATEARLA BIEN
                primerVencimiento += " de " + cls.NumberToWords(prestamo.FirstOrDefault().Vencimiento.Value.Year);//HAY QUE FORMATEARLA BIEN
                valLetras += " coma ";
                valLetras += cls.NumberToWords(Convert.ToInt32(valCuotaL[1]));
                fechaDdMm = cls.NumberToWords(Convert.ToInt32(String.Format("{0:dd}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                fechaDdMm += " de " + String.Format("{0:MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                fechaDdMm += " de " + cls.NumberToWords(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)));//HAY QUE FORMATEARLA BIEN
                document.ReplaceText("<<NombreCliente>>", nombreCliente);
                document.ReplaceText("<<diaVencimiento>>", cls.NumberToWords(diaVenc));
                document.ReplaceText("<<primerVencimiento>>", primerVencimiento);
                document.ReplaceText("<<RutClienteNumero>>", numRutCliente);
                document.ReplaceText("<<RutClienteLetras>>", rutCliente);
                document.ReplaceText("<<FechaPrestamo>>", String.Format("{0:dd/MM/yyyy}",prest.Fecha));
                document.ReplaceText("<<FechaLetras>>", fechaDdMm);
                document.ReplaceText("<<Banco>>", banco);
                document.ReplaceText("<<NumCta>>", numCta);
                document.ReplaceText("<<Fecha>>", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
                document.ReplaceText("<<MontoGir>>", montoGir);
                document.ReplaceText("<<SalPendiente>>", salPendiente);
                document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
                document.ReplaceText("<<TipoEmpresa>>", tipoEmpresa);
                document.ReplaceText("<<GiroComercial>>", giroComercial);
                document.ReplaceText("<<DatosRepresentantes>>", datosRep);
                document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
                document.ReplaceText("<<Direccion>>", direccion);
                document.ReplaceText("<<PrecioCesion>>", precioCesion);
                document.ReplaceText("<<Deudor>>", deudor);
                document.ReplaceText("<<RepresentanteLegal>>", representante);
                document.ReplaceText("<<FechaEscritura>>", fechaEscr);
                document.ReplaceText("<<Notaria>>", notaria);
                document.ReplaceText("<<RutRep>>", rutRep);
                document.ReplaceText("<<Nacionalidad>>", nacionalidad);
                document.ReplaceText("<<EdoCivil>>", edoCivil);
                document.ReplaceText("<<SustantivoPer>>", sustantivoPer);
                document.ReplaceText("<<MontoTotal>>", montoTotal);
                document.ReplaceText("<<Tasa>>", prest.Tasa.ToString());
                document.ReplaceText("<<Plazo>>", plazo);
                document.ReplaceText("<<ValorCuota>>", valLetras);
                document.ReplaceText("<<Cuota>>", prest.Cuota.ToString());
                document.ReplaceText("<<PlazoLetras>>", cls.NumberToWords(Convert.ToInt32(plazo)));
                document.ReplaceText("<<numCuotaL>>", cls.NumberToWords(Convert.ToInt32(prest.NumTotalCuota)));
                document.ReplaceText("<<nombreAval>>", nombreAval);
                document.ReplaceText("<<rutAval>>", rutAval);
                document.ReplaceText("<<nombreCom>>", nombreCom);
                document.ReplaceText("<<rutCom>>", rutCom);
                document.ReplaceText("<<profesionCom>>", profesionCom);
                document.ReplaceText("<<nacionalidadCom>>", nacionalidadCom);
                document.ReplaceText("<<edoCivilCom>>", edoCivilCom);
                document.ReplaceText("<<sexoCom>>", sexoCom);
                document.ReplaceText("<<emailCom>>", emailCom);
                document.ReplaceText("<<direccionCom>>", direccionCom);
                document.ReplaceText("<<telefonoCom>>", telefonoCom);
                document.ReplaceText("<<celularCom>>", celularCom);
                document.ReplaceText("<<empresaCom>>", empresaCom);
                document.ReplaceText("<<rutEmpresaCom>>", rutEmpresaCom);
                document.ReplaceText("<<direccionEmpresaCom>>", direccionEmpresaCom);
                document.ReplaceText("<<rutInscritoGV>>", rutInscritoGV);
                document.ReplaceText("<<nombreInscritoGV>>", nombreInscritoGV);
                document.ReplaceText("<<tipoGV>>", tipoGV);
                document.ReplaceText("<<marcaGV>>", marcaGV);
                document.ReplaceText("<<modeloGV>>", modeloGV);
                document.ReplaceText("<<anoGV>>", anoGV);
                document.ReplaceText("<<motorGV>>", motorGV);
                document.ReplaceText("<<chasisGV>>", chasisGV);
                document.ReplaceText("<<colorGV>>", colorGV);
                document.ReplaceText("<<patenteGV>>", patenteGV);
                document.ReplaceText("<<rvmGV>>", rvmGV);
                document.ReplaceText("<<notariaGV>>", notariaGV);
                document.ReplaceText("<<fechaEscrituraGV>>", fechaEscrituraGV);
                document.ReplaceText("<<deslindesGH>>", deslindesGH);
                document.ReplaceText("<<nombreComproGH>>", nombreComproGH);
                document.ReplaceText("<<nombreNotarioGH>>", nombreNotarioGH);
                document.ReplaceText("<<fojasGH>>", fojasGH);
                document.ReplaceText("<<numeroGH>>", numeroGH);
                document.ReplaceText("<<ubicacionCbrsGH>>", ubicacionCbrsGH);
                document.ReplaceText("<<anoGH>>", anoGH);
                document.ReplaceText("<<comunaGH>>", comunaGH);
                document.ReplaceText("<<rolGH>>", rolGH);
                document.ReplaceText("<<fechaEscrituraGH>>", fechaEscrituraGH);
                document.ReplaceText("<<MontoLetras>>", montoLetras);
                document.ReplaceText("<<numHipoteca>>", numHipoteca);
                document.ReplaceText("<<avalSiNo>>", avalSiNo);
            }
            #endregion

            document.SaveAs(Server.MapPath("../App_Data/Downloads/" + nameTemplate + ".docx"));
            document.Dispose();
        }

        public SqlDataAdapter GetAdapterClientes(string param, string valParam, string tipoParam)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CLIENTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add(param, valParam);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add(tipoParam);
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

        public SqlDataAdapter GetAdapterRepresentantes(string idEmpresa)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_EMPRESAS_REPRESENTANTES";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", idEmpresa);
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
        
        public SqlDataAdapter GetAdapterDireccion(string idRegion,string idCiudad,string idComuna)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DIRECCION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_REGION", idRegion);
                parametros.Add("@ID_CIUDAD", idCiudad);
                parametros.Add("@ID_COMUNA", idComuna);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("int");
                tipoDatos.Add("int");
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

        public SqlDataAdapter GetAdapterSelEmpresa(string rut)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_EMPRESAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT", rut);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public SqlDataAdapter GetAdapterSelPersona(string rut)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PERSONAS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@RUT", rut);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public List<Direccion> GetDireccion(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Direccion> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Direccion()
                  {
                      Direcciones = item.Field<string>("DIRECCION")
                  }).ToList();
            return re;
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
                      IdPersona = item.Field<int>("ID_PERSONA"),
                      IdEmpresa = item.Field<int>("ID_EMPRESA"),
                      Rut = item.Field<string>("Rut"),
                      Nombre = item.Field<string>("NOMBRE"),
                  }).ToList();
            return re;
        }

        public List<Persona> GetPersonas(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Persona> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Persona()
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
                      Celular = item.Field<string>("Celular")
                  }).ToList();
            return re;
        }

        public List<Empresa> GetEmpresa(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Empresa> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Empresa()
                  {
                      ID = item.Field<int>("ID"),
                      IdTipoEmpresa = item.Field<int>("Id_tipo_Empresa"),
                      Tipo = item.Field<string>("Tipo"),
                      Rut = item.Field<string>("Rut"),
                      RazonSocial = item.Field<string>("Razon_Social"),
                      Giro = item.Field<string>("Giro_Comercial"),
                      NombreNotaria = item.Field<string>("Nombre_Notaria"),
                      FechaEscritura = item.Field<DateTime>("Fecha_Escritura").ToString(),
                      Direccion = item.Field<string>("Direccion"),
                      Telefono = item.Field<string>("Telefono")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterSelDatosBanco(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DATOS_BANCO";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente);
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

        public List<DatosBanco> GetDatosBanco(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<DatosBanco> re = null;
            re = (from item in dt.AsEnumerable()
                  select new DatosBanco()
                  {
                      ID = item.Field<int>("ID"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      IdBanco = item.Field<int>("ID_BANCO"),
                      Banco = item.Field<string>("Nombre"),
                      Rut = item.Field<string>("Rut"),
                      Destinatario = item.Field<string>("Destinatario"),
                      NumCuenta = item.Field<string>("NUM_CUENTA"),
                      Email = item.Field<string>("CORREO")
                  }).ToList();
            return re;
        }

        protected void btnPrestamoEspecial_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                Response.Redirect("Especial.aspx?cid=" + Request.QueryString["cid"].ToString() + "&pid=" + Request.QueryString["pid"].ToString());
            }
        }

        protected void btnPrestamoReorganizado_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
            {
                string idCliente = Request.QueryString["cid"].ToString();
                string idPrestamo = Request.QueryString["pid"].ToString();

                int idPres = EditarEdoCuotaReturn(idPrestamo, "10", "", "", idCliente, null);
                Response.Redirect("../Prestamo/?cid=" + idCliente + "&pid=" + idPres.ToString());

            }
        }

        public SqlDataAdapter GetAdapterDocumentos(string funcionalidad)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_DOCUMENTOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@FUNCIONALIDAD", funcionalidad);
                ArrayList tipoDatos = new ArrayList();
                tipoDatos.Add("varchar");
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

        public List<Documentos> GetDocumentos(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Documentos> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Documentos()
                  {
                      ID = item.Field<int>("ID"),
                      Nombre = item.Field<string>("Nombre"),
                      Template = item.Field<string>("Template"),
                      Funcionalidad = item.Field<string>("funcionalidad")
                  }).ToList();
            return re;
        }

        public SqlDataAdapter GetAdapterGarantiasVXClientes(int idCliente)
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

        public SqlDataAdapter GetAdapterSelDatosAval(string idCliente)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_AVAL";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID_CLIENTE", idCliente);
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

        public SqlDataAdapter GetAdapterGarantiasHXClientes(int idCliente)
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

        public List<PrendasV> GetGarantiaV(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<PrendasV> re = null;
            re = (from item in dt.AsEnumerable()
                  select new PrendasV()
                  {
                      ID = item.Field<int>("ID"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      RutInscrito = item.Field<string>("Rut_inscrito"),
                      NombreInscrito = item.Field<string>("Nombre_Inscrito"),
                      Tipo = item.Field<string>("Tipo"),
                      Marca = item.Field<string>("Marca"),
                      Modelo = item.Field<string>("Modelo"),
                      Ano = item.Field<string>("Ano"),
                      Motor = item.Field<string>("Motor"),
                      Chasis = item.Field<string>("Chasis"),
                      Color = item.Field<string>("Color"),
                      Patente = item.Field<string>("Patente"),
                      Rvm = item.Field<string>("Rvm"),
                      Notaria = item.Field<string>("Notaria"),
                      FechaEscritura = item.Field<DateTime?>("Fecha_Escritura")
                  }).ToList();
            return re;
        }

        public List<PrendasH> GetGarantiaH(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<PrendasH> re = null;
            re = (from item in dt.AsEnumerable()
                  select new PrendasH()
                  {
                      ID = item.Field<int>("ID"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Deslindes = item.Field<string>("Deslindes"),
                      NombreCompro = item.Field<string>("Nombre_Compro_Propiedad"),
                      NombreNotario = item.Field<string>("Nombre_Notario"),
                      Fojas = item.Field<string>("Fojas"),
                      Numero = item.Field<string>("Numero"),
                      UbicacionCbrs = item.Field<string>("UBICACION_CBRS"),
                      Ano = item.Field<string>("Ano"),
                      Comuna = item.Field<string>("Comuna"),
                      Rol = item.Field<string>("Rol"),
                      FechaEscritura = item.Field<DateTime?>("Fecha_Escritura")
                  }).ToList();
            return re;
        }

        public List<Avales> GetDatosAval(SqlDataAdapter adapter)
        {
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                List<Avales> re = null;
                re = (from item in dt.AsEnumerable()
                      select new Avales()
                      {
                          ID = item.Field<int>("ID"),
                          IdCliente = item.Field<int>("ID_CLIENTE"),
                          Rut = item.Field<string>("Rut"),
                          Nombre = item.Field<string>("Nombre")
                      }).ToList();
                return re;
            }
            catch
            {
                return null;
            }
        }

        public SqlDataAdapter GetAdapterPagosParciales(string idCuota)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PAGOS_PARCIALES_PRESTAMOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", idCuota);
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

        public List<PagosParciales> GetDatosPagosParciales(SqlDataAdapter adapter)
        {
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                List<PagosParciales> re = null;
                re = (from item in dt.AsEnumerable()
                      select new PagosParciales()
                      {
                          ID = item.Field<int>("ID"),
                          IdPrestamo = item.Field<int>("ID_PRESTAMO"),
                          IdAmortizacion = item.Field<int>("ID_AMORTIZACION"),
                          MontoParcial = item.Field<decimal?>("MONTO_PARCIAL"),
                          FechaPagoParcial = item.Field<DateTime?>("FECHA_PAGO_PARCIAL")
                      }).ToList();
                return re;
            }
            catch
            {
                return null;
            }
        }

        public string GetPagosParciales(string idCuota)
        {
            var adapter = GetAdapterPagosParciales(idCuota);
            var pagos = GetDatosPagosParciales(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var pago in pagos)
            {
                filas.Append("<tr>");
                filas.Append("<td>$");
                filas.Append(decimal.Parse(pago.MontoParcial.ToString()).ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd-MM-yyyy}", pago.FechaPagoParcial));
                filas.Append("</td>");
                filas.Append("</tr>");

            }
            return filas.ToString();
        }

        public SqlDataAdapter GetAdapterPagosCapitalesParciales(string idCuota)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_PAGOS_CAPITAL_PARCIAL";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("@ID", idCuota);
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

        public string GetPagosCapitalesParciales(string idCuota)
        {
            var adapter = GetAdapterPagosCapitalesParciales(idCuota);
            var pagos = GetDatosPagosParciales(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var pago in pagos)
            {
                filas.Append("<tr>");
                filas.Append("<td>$");
                filas.Append(decimal.Parse(pago.MontoParcial.ToString()).ToString("N"));
                filas.Append("</td>");
                filas.Append("<td>");
                filas.Append(string.Format("{0:dd-MM-yyyy}", pago.FechaPagoParcial));
                filas.Append("</td>");
                filas.Append("</tr>");

            }
            return filas.ToString();
        }

        protected void chkDoc_Checked(object sender, EventArgs e)
        {
            foreach (RepeaterItem ri in rptCrearDocs.Items)
            {
                CheckBox chk = (CheckBox)ri.FindControl("chkDoc");
                HiddenField hd = (HiddenField)ri.FindControl("hdItem");
                if (chk.Checked)
                {
                    if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                    {
                        Response.Write("<script>alert('" + hd.Value + "');</script>");
                    }
                   
                }
            }
        }
    }
}