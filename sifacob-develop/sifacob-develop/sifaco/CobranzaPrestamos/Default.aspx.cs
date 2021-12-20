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


namespace sifaco.CobranzaPrestamos
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
                    EditarEdoCuota(Request.QueryString["aid"].ToString(), Request.QueryString["val"].ToString(), Request.QueryString["val2"].ToString());
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
                if (Request.QueryString["aid"] != null && Request.QueryString["val"] != null)
                {
                    if (!IsPostBack)
                    {
                        EditarEdoCuota(Request.QueryString["aid"].ToString(), Request.QueryString["val"].ToString(),"");
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
                    string idCliente = Request.QueryString["cid"].ToString();
                    SqlDataAdapter adapterC = GetAdapterClientes("@ID", idCliente, "int");
                    Clientes cliente = GetClientes(adapterC).LastOrDefault();

                    ltrCliente.Text = cliente.Nombre;
                    ltrRutCliente.Text = cliente.Rut;
                    if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                    {
                        string idPrestamo = Request.QueryString["pid"].ToString();
                        idCliente = Request.QueryString["cid"].ToString();
                        SqlDataAdapter adapter2 = GetAdapterPrestamos(idPrestamo);
                        PrestamosA prestamo = GetPrestamos(adapter2).LastOrDefault();

                        if (pres != null)
                        {
                            pres.Monto = prestamo.Monto ?? 0;
                            pres.Tasa = prestamo.Tasa ?? 0;
                            pres.Plazo = prestamo.Plazo ?? 0;
                            pres.Cuota = prestamo.Cuota ?? 0;
                            pres.ID = prestamo.ID.ToString();
                            pres.MesGracia = prestamo.MesGracia ?? 0;

                            SqlDataAdapter adapter = GetAdapter(idPrestamo);
                            rptPrestamos.DataSource = GetTablaAmortizacion(adapter);
                            rptPrestamos.DataBind();
                        }

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

        public void EditarEdoCuota(string id, string valEdo, string fechaPago)
        {
            try
            {
                int correlativo = GetLastCorrelativo();
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_MOD_EDO_CUOTA";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID", id.ToString());
                tipoDatos.Add("int");
                if (valEdo == "3")
                {
                    if (fechaPago == "")
                    {
                        fechaPago = Convert.ToDateTime("1753-01-01 00:00:00").ToString();
                    }
                    parametros.Add("@ID_EDO_PRES", valEdo);
                    tipoDatos.Add("int");
                    parametros.Add("@FECHA_PAGO", Convert.ToDateTime(fechaPago).ToString());
                    tipoDatos.Add("datetime");
                    parametros.Add("@NAME_USER", Session["login"].ToString());
                    parametros.Add("@ORIGEN", "PRESTAMOS COBRANZA");
                    parametros.Add("@CORRELATIVO", correlativo.ToString());
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("int");
                }
                else
                {
                    parametros.Add("@ID_EDO_PRES", valEdo);
                    tipoDatos.Add("int");
                    parametros.Add("@NAME_USER", Session["login"].ToString());
                    parametros.Add("@ORIGEN", "PRESTAMOS COBRANZA");
                    parametros.Add("@CORRELATIVO", correlativo.ToString());
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("varchar");
                    tipoDatos.Add("int");
                }
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
                      NumCuota = item.Field<decimal>("NUM_CUOTA"),
                      SaldoInicial = item.Field<decimal?>("MONTO_INI"),
                      Cuota = item.Field<decimal?>("CUOTA"),
                      Intereses = item.Field<decimal?>("INTERESES"),
                      Capital = item.Field<decimal?>("CAPITAL"),
                      SaldoFinal = item.Field<decimal>("MONTO_FIN"),
                      Mora = item.Field<decimal>("INTERES_MORA") - item.Field<decimal>("PAGO_PARCIAL_MORA"),
                      FechaPago = item.Field<DateTime?>("FECHA_PAGO"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO")
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
                parametros.Add("@MONTO_INI", montoIni.ToString().Replace(".", ""));
                parametros.Add("@CUOTA", cuota.ToString().Replace(".", ""));
                parametros.Add("@INTERESES", intereses.ToString().Replace(".", ""));
                parametros.Add("@CAPITAL", capital.ToString().Replace(".", ""));
                parametros.Add("@MONTO_FIN", montoFin.ToString().Replace(".", ""));
                parametros.Add("@INTERES_MORA", mora.ToString().Replace(".", ""));
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

        public void EditarPrestamo(string id, string idCliente, string tasa, string plazo, string monto, string cuota, string gracia)
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
                EditarPrestamo(idPrestamo, idCliente, pres.Tasa.ToString(), pres.Plazo.ToString(), pres.Monto.ToString(),pres.Cuota.ToString(), pres.MesGracia.ToString());
                CalculoPrestamo();
                SqlDataAdapter adapter = GetAdapter(idPrestamo);
                rptPrestamos.DataSource = GetTablaAmortizacion(adapter);
                rptPrestamos.DataBind();
            }
        }

        public void CalculoPrestamo()
        {
            string idPrestamo = Request.QueryString["pid"].ToString();
            decimal monto = pres.Monto;
            decimal cuota = pres.Cuota;
            decimal tasa = pres.Tasa;
            int plazo = pres.Plazo;
            int mesGracia = pres.MesGracia;
            ArrayList si = new ArrayList();
            DateTime venc = DateTime.Now;
            for (int i = 1; i <= plazo; i++) 
            {
                venc = venc.AddMonths(1);
                if (i == 1)
                    si.Add(monto);
                else 
                {
                    si.Add(Convert.ToDecimal(si[i-2])-(cuota - (Convert.ToDecimal(si[i-2]) * (tasa / 100))));
                }

                
                GetAdapterCreateAmortizacion(idPrestamo, "1", i.ToString(), Convert.ToDecimal(si[i - 1]), cuota, Convert.ToDecimal(si[i - 1]) * (tasa / 100), cuota - (Convert.ToDecimal(si[i - 1]) * (tasa / 100)), (i != plazo) ? Convert.ToDecimal(si[i - 1]) - (cuota - (Convert.ToDecimal(si[i - 1]) * (tasa / 100))) : 0, 0, venc, mesGracia);
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
                      MesGracia = item.Field<int?>("MES_GRACIA") ?? 0,
                      Cuota = item.Field<decimal?>("CUOTA") ?? 0,
                      Monto = item.Field<decimal?>("MONTO") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA")
                  }).ToList();
            return re;
        }

       /* protected void btnCrearDocs_Click(object sender, EventArgs e)
        {
            if (!print.Pagare && !print.Tabla)
            {
                Response.Write("<script>alert('Debe seleccionar al menos 1 documento');</script>");
            }
            else
            {
                if (Request.QueryString["cid"] != null && Request.QueryString["pid"] != null)
                {
                    if (print.Mutuo)
                    {
                        CreateDocumentWord("MUTUO");
                    }
                    if (print.Pagare)
                    {
                        CreateDocumentWord("PAGARE");
                    }
                    if (print.Tabla)
                    {
                        CreateDocumentWord("TABLA");
                    }
                }
            }
        }*/

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
            //string nameTemplate = "CESION_100";
            string nombreCliente = "";
            string fechaDdMm = "";
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
                //foreach (var fact in facturas)
                //{
                //    nombreDeudor += fact.Deudor + ", ";
                //}
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
                //precioCesion = decimal.Parse(simulacion.PrecioCes.ToString()).ToString("N");
                //saldoPendiente = decimal.Parse(simulacion.MontoPendiente.ToString()).ToString("N");
                //salPendiente = simulacion.SaldoPendiente.ToString();
                //montoGir = decimal.Parse(simulacion.MontoGirable.ToString()).ToString("N");
                deudor = nombreDeudor;
                representante = represen;
                fechaEscritura = String.Format("{0:dd MMMM yyyy}",Convert.ToDateTime(empresas.FechaEscritura));
                notaria = empresas.NombreNotaria;
                montoTotal = decimal.Parse(pres.Monto.ToString()).ToString("N");
                banco = datosBanco.Banco;
                numCta = datosBanco.NumCuenta;
                numRutCliente = empresas.Rut;
                plazo = pres.Plazo.ToString();
                //anticipo = Sim.Anticipo.ToString();
                #endregion
            }
            else
            {
                #region Documentos para Personas
                //if (nameTemplate == "CESION")
                //{
                //    if (simulacion.Anticipo == 100)
                //        nameTemplate = "CESION_P_100";
                //    else
                //        nameTemplate = "CESION_P_95";
                //}
                //if (nameTemplate == "MANDATO")
                //{
                //    nameTemplate = "MANDATO_P";
                //}
                //if (nameTemplate == "AVALISTA")
                //{
                //    nameTemplate = "AVALISTA_P";
                //}
                //if (nameTemplate == "PODER")
                //{
                //    nameTemplate = "PODER_P";
                //}
                //if (nameTemplate == "UAF")
                //{
                //    nameTemplate = "UAF_P";
                //}
                //if (nameTemplate == "CONTRATO_MARCO")
                //{
                //    nameTemplate = "CONTRATO_MARCO_P";
                //}
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
                //foreach (var fact in facturas)
                //{
                //    nombreDeudor += fact.Deudor + ", ";
                //}
                nombreCliente = persona.Nombre;
                fechaDdMm = String.Format("{0:dd MMMM}", DateTime.Now);//HAY QUE FORMATEARLA BIEN
                string[] rutC = persona.Rut.Replace(".", "").Split('-');
                string rutCLetras = cls.NumberToWords(Convert.ToInt32(rutC[0]));
                rutCLetras += " guion ";
                rutCLetras += cls.NumberToWords(Convert.ToInt32(rutC[1]));
                rutCliente = rutCLetras;
                string[] dir = persona.Direccion.Split(';');
                SqlDataAdapter adapterD = GetAdapterDireccion(dir[0].ToString(), dir[1].ToString(), dir[2].ToString());
                direccion = dir[3].ToString() + " " + GetDireccion(adapterD).LastOrDefault().Direcciones;
                //precioCesion = decimal.Parse(simulacion.PrecioCes.ToString()).ToString("N");
                //saldoPendiente = decimal.Parse(simulacion.MontoPendiente.ToString()).ToString("N");
                //salPendiente = simulacion.SaldoPendiente.ToString();
                //montoGir = decimal.Parse(simulacion.MontoGirable.ToString()).ToString("N");
                deudor = nombreDeudor;
                montoTotal = decimal.Parse(pres.Monto.ToString()).ToString("N");
                edoCivil = edo;
                sustantivoPer = sinDon;
                nacionalidad = persona.Nacionalidad;
                numRutCliente = persona.Rut;
                banco = datosBanco.Banco;
                numCta = datosBanco.NumCuenta;
                plazo = pres.Plazo.ToString();
                //anticipo = Sim.Anticipo.ToString();
                #endregion
                //SqlDataAdapter adapterP = getada
            }
            ReplaceWord(nameTemplate, nombreCliente, fechaDdMm, tipoEmpresa, giroComercial, rutCliente, datosRep, sustantivoDir, direccion, precioCesion, deudor, representante, saldoPendiente, fechaEscritura, notaria, montoTotal, rutRep, nacionalidad, edoCivil, sustantivoPer, numRutCliente, banco, numCta, prestamo, plazo, anticipo, salPendiente, montoGir);
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
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //Response.Redirect(url);
        }

        public void ReplaceWord(string nameTemplate, string nombreCliente, string fechaDdMm, string tipoEmpresa, string giroComercial, string rutCliente, string datosRep, string sustantivoDir, string direccion, string precioCesion, string deudor, string representante, string saldoPendiente, string fechaEscr, string notaria, string montoTotal, string rutRep, string nacionalidad, string edoCivil, string sustantivoPer, string numRutCliente, string banco, string numCta, List<PrestamosA> prestamo, string plazo, string cuota, string salPendiente, string montoGir)
        {
            //Templates
            Common cls = new Common();
            DocX document = DocX.Load(Server.MapPath("~/Template/" + nameTemplate + ".docx"));
            //if (nameTemplate == "CESION_95" || nameTemplate == "CESION_100")
            //{
            //    document.ReplaceText("<<NombreCliente>>", nombreCliente);
            //    document.ReplaceText("<<Fechaddmm>>", fechaDdMm);
            //    document.ReplaceText("<<TipoEmpresa>>", tipoEmpresa);
            //    document.ReplaceText("<<GiroComercial>>", giroComercial);
            //    document.ReplaceText("<<RutCliente>>", rutCliente);
            //    document.ReplaceText("<<DatosRepresentantes>>", datosRep);
            //    document.ReplaceText("<<SustantivoDireccion>>", sustantivoDir);
            //    document.ReplaceText("<<Direccion>>", direccion);
            //    document.ReplaceText("<<PrecioCesion>>", precioCesion);
            //    document.ReplaceText("<<Deudor>>", deudor);
            //    document.ReplaceText("<<RepresentanteLegal>>", representante);
            //    if (nameTemplate == "CESION_95")
            //    {
            //        document.ReplaceText("<<Anticipo>>", anticipo);
            //        document.ReplaceText("<<SaldoPendiente>>", saldoPendiente);
            //    }
            //}

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

            document.SaveAs(Server.MapPath("~/Downloads/" + nameTemplate + ".docx"));
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

    }
}