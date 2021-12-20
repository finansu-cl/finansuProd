using Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco.Controls
{
    public partial class HeadHome : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaD.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaH.Text = DateTime.Now.ToShortDateString();
                ltrHead.Text = ChartJavasScriptCapital().ToString();
                ltrHead.Text += ChartJavasScriptUtilidad().ToString();

                SqlDataAdapter adapterFM = GetAdapterCantFacturasMora();
                SqlDataAdapter adapterFVV = GetAdapterCantFacturasVencerVencidas();
                SqlDataAdapter adapterCM = GetAdapterCobroFacturasMes();
                SqlDataAdapter adapterOM = GetAdapterCantOperacionesMes();
                ltrFacM.Text = GetEstadisticasTotalesFM(adapterFM).LastOrDefault().CantFacturasMora.ToString();
                ltrFacVV.Text = GetEstadisticasTotalesFVV(adapterFVV).LastOrDefault().CantFacturasVencerVencidas.ToString();
                ltrCobM.Text = "$"+decimal.Parse(GetEstadisticasTotalesCM(adapterCM).LastOrDefault().FacturasCobradasMes.ToString()).ToString("N");
                ltrOpeM.Text = GetEstadisticasTotalesOM(adapterOM).LastOrDefault().CantOperacionesMes.ToString();

            }

        }

        public string[] SetLabelsChart()
        {
            SqlDataAdapter adapter = GetAdapterEstadisticas();
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            string[] ArrayLabelCyU = new string[5];
            ArrayLabelCyU[0] = "";
            ArrayLabelCyU[1] = "0";
            ArrayLabelCyU[2] = "1000";
            ArrayLabelCyU[3] = "0";
            ArrayLabelCyU[4] = "1000";
            int num = 1;
            int num2 = 1;
            int num3 = 1;
            foreach (var estadisticas in estats)
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num == 1)
                    chart.Append(" [ '" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                if (estats.Count != num && num != 1)
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                if (estats.Count == num)
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "'],");

                num++;
            }

            foreach (var estadisticas in estats.OrderBy(x => x.Capital))
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num2 == 1)
                {
                    ArrayLabelCyU[1] = estadisticas.Capital.ToString();//menor
                }
                if (estats.Count == num2)
                {
                    ArrayLabelCyU[2] = estadisticas.Capital.ToString();//mayor
                }
                num2++;
            }

            foreach (var estadisticas in estats.OrderBy(x => x.Utilidad))
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num3 == 1)
                {
                    ArrayLabelCyU[3] = estadisticas.Utilidad.ToString();//menor
                }
                if (estats.Count == num3)
                {
                    ArrayLabelCyU[4] = estadisticas.Utilidad.ToString();//mayor
                }
                num3++;
            }

            ArrayLabelCyU[0] = chart.ToString();

            return ArrayLabelCyU;
        }

        public string SetDataChart()
        {
            SqlDataAdapter adapter = GetAdapterEstadisticas();
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            int num = 1;
            foreach (var estadisticas in estats)
            {
                //[65, 59, 80, 81, 56, 55, 40]
                if (num == 1)
                    chart.Append(" [" + estadisticas.Capital.ToString().Replace(",00", "") + ",");
                if (estats.Count != num && num != 1)
                    chart.Append(estadisticas.Capital.ToString().Replace(",00", "") + ",");
                if (estats.Count == num)
                    chart.Append(estadisticas.Capital.ToString().Replace(",00", "") + "]");

                num++;
            }

            return chart.ToString();
        }

        public string SetDataUtilidadChart()
        {
            SqlDataAdapter adapter = GetAdapterEstadisticas();
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            int num = 1;
            foreach (var estadisticas in estats)
            {
                //[65, 59, 80, 81, 56, 55, 40]
                if (num == 1)
                    chart.Append(" [" + estadisticas.Utilidad.ToString().Replace(",", ".") + ",");
                if (estats.Count != num && num != 1)
                    chart.Append(estadisticas.Utilidad.ToString().Replace(",", ".") + ",");
                if (estats.Count == num)
                    chart.Append(estadisticas.Utilidad.ToString().Replace(",", ".") + "]");

                num++;
            }

            return chart.ToString();
        }

        public StringBuilder ChartJavasScriptCapital()
        {
            string[] arrayLables = SetLabelsChart();

            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + arrayLables[0].ToString());
            chart.Append("        datasets: [");
            chart.Append("    {");
            chart.Append("        label: 'Electronics',");
            chart.Append("        fillColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        strokeColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        pointColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        pointStrokeColor: '#c1c7d1',");
            chart.Append("        pointHighlightFill: '#fff',");
            chart.Append("        pointHighlightStroke: 'rgba(220,220,220,1)',");
            chart.Append("        data: " + SetDataChart());
            chart.Append("    }");
            chart.Append("  ]");
            chart.Append("    };");
            chart.Append("   var areaChartOptions = {");
            chart.Append("   showScale: true,");
            chart.Append("   scaleShowGridLines: false,");
            chart.Append("   scaleGridLineColor: 'rgba(0,0,0,.05)',");
            chart.Append("   scaleGridLineWidth: 1,");
            chart.Append("   scaleShowHorizontalLines: true,");
            chart.Append("   scaleShowVerticalLines: true,");
            chart.Append("   bezierCurve: true,");
            chart.Append("   bezierCurveTension: 0.3,");
            chart.Append("   pointDot: true,");
            chart.Append("   pointDotRadius: 4,");
            chart.Append("   pointDotStrokeWidth: 1,");
            chart.Append("   pointHitDetectionRadius: 20,");
            chart.Append("   datasetStroke: true,");
            chart.Append("   datasetStrokeWidth: 2,");
            chart.Append("   datasetFill: true,");
            chart.Append("   legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',");
            chart.Append("   maintainAspectRatio: true,");
            chart.Append("   responsive: true");
            chart.Append("   };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");
            return chart;
        }

        public StringBuilder ChartJavasScriptUtilidad()
        {
            string[] arrayLables = SetLabelsChart();

            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart2').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + arrayLables[0].ToString());
            chart.Append("        datasets: [");
            chart.Append("    {");
            chart.Append("        label: 'Electronics',");
            chart.Append("        fillColor: 'rgba(60,141,188,0.9)',");
            chart.Append("        strokeColor: 'rgba(60,141,188,0.8)',");
            chart.Append("        pointColor: '#3b8bba',");
            chart.Append("        pointStrokeColor: 'rgba(60,141,188,1)',");
            chart.Append("        pointHighlightFill: '#fff',");
            chart.Append("        pointHighlightStroke: 'rgba(60,141,188,1)',");
            chart.Append("        data: " + SetDataUtilidadChart());
            chart.Append("    }");
            chart.Append("  ]");
            chart.Append("    };");
            chart.Append("   var areaChartOptions = {");
            chart.Append("   showScale: true,");
            chart.Append("   scaleShowGridLines: false,");
            chart.Append("   scaleGridLineColor: 'rgba(0,0,0,.05)',");
            chart.Append("   scaleGridLineWidth: 1,");
            chart.Append("   scaleShowHorizontalLines: true,");
            chart.Append("   scaleShowVerticalLines: true,");
            chart.Append("   bezierCurve: true,");
            chart.Append("   bezierCurveTension: 0.3,");
            chart.Append("   pointDot: true,");
            chart.Append("   pointDotRadius: 4,");
            chart.Append("   pointDotStrokeWidth: 1,");
            chart.Append("   pointHitDetectionRadius: 20,");
            chart.Append("   datasetStroke: true,");
            chart.Append("   datasetStrokeWidth: 2,");
            chart.Append("   datasetFill: true,");
            chart.Append("   legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',");
            chart.Append("   maintainAspectRatio: true,");
            chart.Append("   responsive: true");
            chart.Append("   };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");

            return chart;
        }

        public SqlDataAdapter GetAdapterEstadisticas()
        {
            try
            {
                DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
                DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CAPITAL_DIA";
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

        public SqlDataAdapter GetAdapterEstadisticasFiltro(DateTime fechaD, DateTime fechaH)
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CAPITAL_DIA";
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            DateTime fechaD = Convert.ToDateTime("1753-01-01 00:00:00"); ;
            DateTime fechaH = Convert.ToDateTime("9999-12-31 23:59:59.997");
            if (txtFechaD.Text != "")
                fechaD = Convert.ToDateTime(txtFechaD.Text);
            if (txtFechaH.Text != "")
                fechaH = Convert.ToDateTime(txtFechaH.Text);

            if (txtFechaD.Text == "" && txtFechaH.Text == "")
                Response.Redirect(Request.Url.ToString());
            else
            {
                ltrHead.Text = ChartJavasScriptCapitalFiltro(fechaD, fechaH).ToString();
                ltrHead.Text += ChartJavasScriptUtilidadFiltro(fechaD, fechaH).ToString();
            }
        }

        public List<Estadisticas> GetEstadisticas(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Estadisticas> re = null;
            re = (from item in dt.AsEnumerable()
                  select new Estadisticas()
                  {
                      Capital = item.Field<decimal?>("CAPITAL"),
                      Utilidad = item.Field<decimal?>("UTILIDAD"),
                      Fecha = item.Field<string>("FECHA")

                  }).ToList();
            return re;
        }

        public string[] SetLabelsChartFiltro(DateTime fechaD, DateTime fechaH)
        {
            SqlDataAdapter adapter = GetAdapterEstadisticasFiltro(fechaD, fechaH);
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            string[] ArrayLabelCyU = new string[5];
            ArrayLabelCyU[0] = "";
            ArrayLabelCyU[1] = "0";
            ArrayLabelCyU[2] = "1000";
            ArrayLabelCyU[3] = "0";
            ArrayLabelCyU[4] = "1000";
            int num = 1;
            int num2 = 1;
            int num3 = 1;
            foreach (var estadisticas in estats)
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num == 1)
                {
                    chart.Append(" [ '" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                }
                if (estats.Count != num && num != 1)
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                if (estats.Count == num)
                {
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "'],");
                }

                num++;
            }

            foreach (var estadisticas in estats.OrderBy(x => x.Capital))
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num2 == 1)
                {
                    ArrayLabelCyU[1] = estadisticas.Capital.ToString();//menor
                }
                if (estats.Count == num2)
                {
                    ArrayLabelCyU[2] = estadisticas.Capital.ToString();//mayor
                }
                num2++;
            }

            foreach (var estadisticas in estats.OrderBy(x => x.Utilidad))
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num3 == 1)
                {
                    ArrayLabelCyU[3] = estadisticas.Utilidad.ToString();//menor
                }
                if (estats.Count == num3)
                {
                    ArrayLabelCyU[4] = estadisticas.Utilidad.ToString();//mayor
                }
                num3++;
            }

            ArrayLabelCyU[0] = chart.ToString();
            return ArrayLabelCyU;
        }

        public string SetDataChartFiltro(DateTime fechaD, DateTime fechaH)
        {
            SqlDataAdapter adapter = GetAdapterEstadisticasFiltro(fechaD, fechaH);
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            int num = 1;
            foreach (var estadisticas in estats)
            {
                //[65, 59, 80, 81, 56, 55, 40]
                if (num == 1)
                    chart.Append(" [" + estadisticas.Capital.ToString().Replace(",00", "") + ",");
                if (estats.Count != num && num != 1)
                    chart.Append(estadisticas.Capital.ToString().Replace(",00", "") + ",");
                if (estats.Count == num)
                    chart.Append(estadisticas.Capital.ToString().Replace(",00", "") + "]");

                num++;
            }

            return chart.ToString();
        }

        public string SetDataUtilidadChartFiltro(DateTime fechaD, DateTime fechaH)
        {
            SqlDataAdapter adapter = GetAdapterEstadisticasFiltro(fechaD, fechaH);
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            int num = 1;
            foreach (var estadisticas in estats)
            {
                //[65, 59, 80, 81, 56, 55, 40]
                if (num == 1)
                    chart.Append(" [" + estadisticas.Utilidad.ToString().Replace(",", ".") + ",");
                if (estats.Count != num && num != 1)
                    chart.Append(estadisticas.Utilidad.ToString().Replace(",", ".") + ",");
                if (estats.Count == num)
                    chart.Append(estadisticas.Utilidad.ToString().Replace(",", ".") + "]");

                num++;
            }

            return chart.ToString();
        }

        public StringBuilder ChartJavasScriptCapitalFiltro(DateTime fechaD, DateTime fechaH)
        {
            string[] arrayLables = SetLabelsChartFiltro(fechaD, fechaH);

            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + arrayLables[0].ToString());
            chart.Append("        datasets: [");
            chart.Append("    {");
            chart.Append("        label: 'Electronics',");
            chart.Append("        fillColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        strokeColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        pointColor: 'rgba(210, 214, 222, 1)',");
            chart.Append("        pointStrokeColor: '#c1c7d1',");
            chart.Append("        pointHighlightFill: '#fff',");
            chart.Append("        pointHighlightStroke: 'rgba(220,220,220,1)',");
            chart.Append("        data: " + SetDataChartFiltro(fechaD, fechaH));
            chart.Append("    }");
            chart.Append("  ]");
            chart.Append("    };");
            chart.Append("   var areaChartOptions = {");
            chart.Append("   showScale: true,");
            chart.Append("   scaleShowGridLines: false,");
            chart.Append("   scaleGridLineColor: 'rgba(0,0,0,.05)',");
            chart.Append("   scaleGridLineWidth: 1,");
            chart.Append("   scaleShowHorizontalLines: true,");
            chart.Append("   scaleShowVerticalLines: true,");
            chart.Append("   bezierCurve: true,");
            chart.Append("   bezierCurveTension: 0.3,");
            chart.Append("   pointDot: true,");
            chart.Append("   pointDotRadius: 4,");
            chart.Append("   pointDotStrokeWidth: 1,");
            chart.Append("   pointHitDetectionRadius: 20,");
            chart.Append("   datasetStroke: true,");
            chart.Append("   datasetStrokeWidth: 2,");
            chart.Append("   datasetFill: true,");
            chart.Append("   legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',");
            chart.Append("   maintainAspectRatio: true,");
            chart.Append("   responsive: true,");
            chart.Append("   tooltipTemplate: ' <%if (label){%><%=label %>: <%}%><%= value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, \",\") %>',");
            chart.Append("   tooltips:");
            chart.Append("   {");
            chart.Append("   callbacks:");
            chart.Append("   {");
            chart.Append("      enabled: true,");
            chart.Append("      mode: 'single',");
            chart.Append("       label: ");
            chart.Append("        function(tooltipItem, data) {");
            chart.Append("          var value = data.datasets[0].data[tooltipItem.index];");
            chart.Append("          if (parseInt(value) >= 1000)");
            chart.Append("          {");
            chart.Append("              return '$' + value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');");
            chart.Append("          }");
            chart.Append("          else");
            chart.Append("          {");
            chart.Append("              return '$' + value;");
            chart.Append("          }");
            chart.Append("        }");
            chart.Append("       }");
            chart.Append("   },");
            chart.Append("   scaleLabel:");
            chart.Append("        function(label){return  ' $' + label.value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');}");
            chart.Append("   };");
            //chart.Append("    var areaChartOptions = {");
            //chart.Append("        responsive: true,");
            //chart.Append("    animation: false,scaleOverride: true, scaleStartValue: "+ arrayLables[1].ToString().Replace(",00", "") + ", scaleStepWidth: "+ arrayLables[2].ToString().Replace(",00", "") + ",");
            //chart.Append("        scaleLabel:");
            //chart.Append("        function(label){return  ' $' + label.value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');}");
            //chart.Append("    };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");
            return chart;
        }

        public StringBuilder ChartJavasScriptUtilidadFiltro(DateTime fechaD, DateTime fechaH)
        {
            string[] arrayLables = SetLabelsChartFiltro(fechaD, fechaH);

            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart2').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + arrayLables[0].ToString());
            chart.Append("        datasets: [");
            chart.Append("    {");
            chart.Append("        label: 'Electronics',");
            chart.Append("        fillColor: 'rgba(60,141,188,0.9)',");
            chart.Append("        strokeColor: 'rgba(60,141,188,0.8)',");
            chart.Append("        pointColor: '#3b8bba',");
            chart.Append("        pointStrokeColor: 'rgba(60,141,188,1)',");
            chart.Append("        pointHighlightFill: '#fff',");
            chart.Append("        pointHighlightStroke: 'rgba(60,141,188,1)',");
            chart.Append("        data: " + SetDataUtilidadChartFiltro(fechaD, fechaH));
            chart.Append("    }");
            chart.Append("  ]");
            chart.Append("    };");
            chart.Append("   var areaChartOptions = {");
            chart.Append("   showScale: true,");
            chart.Append("   scaleShowGridLines: false,");
            chart.Append("   scaleGridLineColor: 'rgba(0,0,0,.05)',");
            chart.Append("   scaleGridLineWidth: 1,");
            chart.Append("   scaleShowHorizontalLines: true,");
            chart.Append("   scaleShowVerticalLines: true,");
            chart.Append("   bezierCurve: true,");
            chart.Append("   bezierCurveTension: 0.3,");
            chart.Append("   pointDot: true,");
            chart.Append("   pointDotRadius: 4,");
            chart.Append("   pointDotStrokeWidth: 1,");
            chart.Append("   pointHitDetectionRadius: 20,");
            chart.Append("   datasetStroke: true,");
            chart.Append("   datasetStrokeWidth: 2,");
            chart.Append("   datasetFill: true,");
            chart.Append("   legendTemplate: '<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',");
            chart.Append("   maintainAspectRatio: true,");
            chart.Append("   responsive: true");
            chart.Append("   };");
            //chart.Append("    var areaChartOptions = {");
            //chart.Append("        responsive: true,");
            //chart.Append("    animation: false,scaleOverride: true, scaleStartValue: "+ arrayLables[3].ToString().Replace(",00", "") + ", scaleStepWidth: "+ arrayLables[4].ToString().Replace(",00", "") + ",");
            //chart.Append("        scaleLabel:");
            //chart.Append("        function(label){return  ' $' + label.value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');}");
            //chart.Append("    };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");

            return chart;
        }

        public SqlDataAdapter GetAdapterCantFacturasMora()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CANTIDAD_FACTURAS_MORA";
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

        public SqlDataAdapter GetAdapterCantFacturasVencerVencidas()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CANTIDAD_FACTURAS_VENCER_VENCIDAS";
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

        public SqlDataAdapter GetAdapterCobroFacturasMes()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_TOTAL_COBRADO_FACTURAS_MES";
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

        public SqlDataAdapter GetAdapterCantOperacionesMes()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CANTIDAD_CLIENTES_NUEVOS";
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

        public List<EstadisticasTotales> GetEstadisticasTotalesFM(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadisticasTotales> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadisticasTotales()
                  {
                      CantFacturasMora = item.Field<int?>("CANTIDAD_FACTURAS_MORA")
                  }).ToList();
            return re;
        }

        public List<EstadisticasTotales> GetEstadisticasTotalesFVV(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadisticasTotales> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadisticasTotales()
                  {
                      CantFacturasVencerVencidas = item.Field<int?>("CANTIDAD_FACTURAS_VENCER_VENCIDAS")
                  }).ToList();
            return re;
        }

        List<EstadisticasTotales> GetEstadisticasTotalesCM(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadisticasTotales> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadisticasTotales()
                  {
                      FacturasCobradasMes = item.Field<decimal?>("TOTAL_COBRADO_FACTURAS_MES")
                  }).ToList();
            return re;
        }

        public List<EstadisticasTotales> GetEstadisticasTotalesOM(SqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<EstadisticasTotales> re = null;
            re = (from item in dt.AsEnumerable()
                  select new EstadisticasTotales()
                  {
                      CantOperacionesMes = item.Field<int?>("CANTIDAD_CLIENTES_NUEVOS")
                  }).ToList();
            return re;
        }

    }
}