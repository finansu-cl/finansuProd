using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Clases;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Text;

namespace sifaco.BuscadorCobranza
{
    public partial class Prestamo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["menu"] = "18";
            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-5).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
                SqlDataAdapter adapterP = GetAdapterPrestamos("-1", DateTime.Now.AddDays(-5), DateTime.Now, -1);
                var src = GetPrestamos(adapterP).GroupBy(x => x.ID).Select(group => group.First()).ToList();

                //FUNCION PARA CORREGIR MONTOS DE MORA
                src.ForEach(i =>
                {
                    FixMora(i.ID.ToString());
                });

                SqlDataAdapter adapterP2 = GetAdapterPrestamos("-1", DateTime.Now.AddDays(-5), DateTime.Now, -1);
                var src2 = GetPrestamos(adapterP2).GroupBy(x => x.ID).Select(group => group.First()).ToList();

                rptPrestamo.DataSource = src2;
                rptPrestamo.DataBind();

                SqlDataAdapter adapterC = GetAdapterCliente();
                ddlCliente.DataSource = GetClientes(adapterC);
                ddlCliente.DataTextField = "Nombre";
                ddlCliente.DataValueField = "ID";
                ddlCliente.DataBind();

                decimal mtp = GetPrestamos(adapterP2).GroupBy(count => count.ID).Select(group => group.LastOrDefault()).Sum(x => x.Monto) ?? 0; //GetPrestamos(adapterP).Sum(x => x.Monto) ?? 0;
                decimal motp = GetPrestamos(adapterP2).Sum(x => x.Mora) ?? 0;
                decimal utp = GetPrestamos(adapterP2).Sum(x => x.Intereses) ?? 0;
                decimal cpt = GetPrestamos(adapterP2).Sum(x => x.Capital) ?? 0;
                decimal gtp = 0;
                int ftp = GetPrestamos(adapterP2).GroupBy(count => count.ID).Select(group => group.LastOrDefault()).ToList().Count;
                txtMTP.Text = mtp.ToString("N");
                txtMoTP.Text = motp.ToString("N");
                txtUTP.Text = utp.ToString("N");
                txtCPT.Text = cpt.ToString("N");
                txtGTP.Text = gtp.ToString("N");
                txtPT.Text = ftp.ToString();
            }
        }

        public string CuotasXPrestamos(string idPrestamo)
        {
            string idEdoCuota = (ddlEdoFactura.SelectedValue == "-1")?"0": ddlEdoFactura.SelectedValue;
            string idTipoFecha = "5" + idEdoCuota;
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text + " 23:59:59.997");

            SqlDataAdapter adapter = GetAdapterPrestamos(idPrestamo, fechaD, fechaH, Convert.ToInt32(idTipoFecha));
            List<PrestamosA> prestamo = GetPrestamos(adapter);
            StringBuilder filas = new StringBuilder();
            foreach (var pre in prestamo)
            {

                string style2 = "";

                switch (pre.IdEdoPres)
                {
                    case 1:
                        if (pre.Vencimiento < DateTime.Now)
                            style2 = "style='background:red;color:white;'";
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
                    case 6:
                        style2 = "style='background:blue;color:white;'";
                        break;
                    case 7:
                        style2 = "style='background:gray;color:black;'";
                        break;
                    case 9:
                        style2 = "style='background:#9cb9f9;color:black;'";
                        break;
                    case 11:
                        style2 = "style='background:#caef00;color:black;'";
                        break;

                }


                filas.Append("<tr " + style2 + ">");
                    filas.Append("<td>");
                    filas.Append(pre.NumCuota.ToString().ToString().Replace(",00", "").Replace(",", ".").Replace(".10", ".1").Replace(".20", ".2").Replace(".30", ".3").Replace(".40", ".4"));
                    filas.Append("</td>");
                    filas.Append("<td>$");
                    filas.Append(decimal.Parse(pre.Cuota.ToString()).ToString("N"));
                    filas.Append("</td>");
                    filas.Append("<td>$");
                    filas.Append(decimal.Parse(pre.Intereses.ToString()).ToString("N"));
                    filas.Append("</td>");
                    filas.Append("<td>$");
                    filas.Append(decimal.Parse(pre.Capital.ToString()).ToString("N"));
                    filas.Append("</td>");
                    filas.Append("<td>$");
                    filas.Append(decimal.Parse(pre.Mora.ToString()).ToString("N"));
                    filas.Append("</td>");
                    filas.Append("<td>");
                    filas.Append(string.Format("{0:dd-MM-yyyy}", pre.Fecha));
                    filas.Append("</td>");
                    filas.Append("<td>");
                    filas.Append(string.Format("{0:dd-MM-yyyy}", pre.Vencimiento));
                    filas.Append("</td>");
                    filas.Append("<td>");
                    filas.Append((pre.FechaPago == null) ? "N/A" : string.Format("{0:dd-MM-yyyy}", pre.FechaPago));
                    filas.Append("</td>");
                    filas.Append("<td>");
                    filas.Append(pre.VenceEn);
                    filas.Append(" Días</td>");
                filas.Append("</tr>");

            }
            return filas.ToString();
        }

        public SqlDataAdapter GetAdapterPrestamos(string idCliente, DateTime? fechaD, DateTime? fechaH, int tippFecha)
        {
            try
            {
                if (fechaD == null)
                    fechaD = Convert.ToDateTime("1753-01-01 00:00:00");
                if(fechaH == null)
                    fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");

                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_INFORMACION_FILTROS_PRESTAMOS";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_CLIENTE", idCliente.ToString());
                tipoDatos.Add("int");
                parametros.Add("@FECHA_DESDE", fechaD.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@FECHA_HASTA", fechaH.ToString());
                tipoDatos.Add("datetime");
                parametros.Add("@TIPO_FECHA", tippFecha.ToString());
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
                      Nombre = item.Field<string>("Nombre"),
                      IdCliente = item.Field<int>("ID_CLIENTE"),
                      Monto = item.Field<decimal?>("MONTO_TOTAL") ?? 0,
                      Tasa = item.Field<decimal?>("TASA") ?? 0,
                      Plazo = item.Field<int>("PLAZO"),
                      NumCuota = item.Field<decimal?>("NUM_CUOTA") ?? 0,
                      NumTotalCuota = item.Field<int?>("NUM_CUOTAS") ?? 0,
                      Cuota = item.Field<decimal?>("CUOTA") ?? 0,
                      Intereses = item.Field<decimal?>("UTILIDAD") ?? 0,
                      Capital = item.Field<decimal?>("CAPITAL") ?? 0,
                      Mora = item.Field<decimal?>("INTERES_MORA") ?? 0,
                      Fecha = item.Field<DateTime>("FECHA_CREA"),
                      FechaPago = item.Field<DateTime?>("FECHA_PAGO"),
                      Vencimiento = item.Field<DateTime?>("VENCIMIENTO"),
                      IdEdoPres = item.Field<int?>("ID_EDO_PRES") ?? 0,
                      VenceEn = item.Field<int>("DIAS_ATRASO"),
                      EstadoOpera = item.Field<int?>("EST_OPER") ?? 0
                  }).ToList();
            return re;
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string idCliente = ddlCliente.SelectedValue.ToString();
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00");;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if(txtFechaD.Text != "")
                 fechaD = Convert.ToDateTime(txtFechaD.Text);
            if(txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text+" 23:59:59.997");
            int tipoFecha = Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString());

            SqlDataAdapter adapterP = GetAdapterPrestamos(idCliente, fechaD, fechaH, tipoFecha);
            //var src = GetPrestamos(adapterP).Where(x => x.EstadoOpera == 1).GroupBy(x => x.ID).Select(group => group.First()).ToList();
            var src = GetPrestamos(adapterP).GroupBy(x => x.ID).Select(a => new
            {
                a.First().ID,
                a.First().Nombre,
                //Monto = a.Sum(b => b.Capital),
                Monto = a.First().Monto,
                Intereses = a.Sum(b => b.Intereses),
                Mora = a.Sum(b => b.Mora),
                a.First().Plazo,
                a.First().Tasa,
                a.First().NumTotalCuota,
                a.First().Fecha
            }).ToList();

            //FUNCION PARA CORREGIR MONTOS DE MORA
            src.ForEach(i =>
            {
                FixMora(i.ID.ToString());
            });

            SqlDataAdapter adapterP2 = GetAdapterPrestamos(idCliente, fechaD, fechaH, tipoFecha);
            var src2 = GetPrestamos(adapterP2).GroupBy(x => x.ID).Select(a => new
            {
                a.First().ID,
                a.First().Nombre,
                //Monto = a.Sum(b => b.Capital),
                Monto = a.First().Monto,
                Intereses = a.Sum(b => b.Intereses),
                Mora = a.Sum(b => b.Mora),
                a.First().Plazo,
                a.First().Tasa,
                a.First().NumTotalCuota,
                a.First().Fecha
            }).ToList();

            rptPrestamo.DataSource = src2; 
            rptPrestamo.DataBind();
            List<PrestamosA> tt = GetPrestamos(adapterP2).Where(x=>x.EstadoOpera==1).ToList();
            decimal mtp = tt.Sum(x => x.Cuota) ?? 0;
            decimal motp = tt.Sum(x => x.Mora) ?? 0;
            decimal utp = tt.Sum(x => x.Intereses) ?? 0;
            decimal cpt = tt.Sum(x => x.Capital) ?? 0;
            decimal gtp = 0;
            int ftp = tt.GroupBy(count => count.ID).Select(group => group.LastOrDefault()).ToList().Count;
            txtMTP.Text = mtp.ToString("N");
            txtMoTP.Text = motp.ToString("N");
            txtUTP.Text = utp.ToString("N");
            txtCPT.Text = cpt.ToString("N");
            txtGTP.Text = gtp.ToString("N");
            txtPT.Text = ftp.ToString();

        }

        public void FixMora(string idPrestamo)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_AMORTIZACION";
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                ArrayList tipoDatos = new ArrayList();
                parametros.Add("@ID_PRESTAMO", idPrestamo);
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

        public string SumMontoTotal(int id) {
            string idCliente = ddlCliente.SelectedValue.ToString();
            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);
            int tipoFecha = Convert.ToInt32(ddlEdoFactura.SelectedValue.ToString());
            SqlDataAdapter adapterP = GetAdapterPrestamos(idCliente, fechaD, fechaH, tipoFecha);
            List<PrestamosA> tt = GetPrestamos(adapterP).Where(x => x.EstadoOpera == 1 && x.ID == id).ToList();
            decimal monto = tt.Sum(x => x.Capital) ?? 0;
            return monto.ToString("N");

        }

    }
}