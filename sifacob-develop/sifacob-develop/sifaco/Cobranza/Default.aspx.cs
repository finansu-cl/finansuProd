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

namespace sifaco.Cobranza
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                ltrHead.Text = ChartJavasScriptCapital().ToString();
                ltrHead.Text += ChartJavasScriptUtilidad().ToString();
            }

        }

        public string SetLabelsChart()
        {
            SqlDataAdapter adapter = GetAdapterEstadisticas();
            List<Estadisticas> estats = GetEstadisticas(adapter);
            StringBuilder chart = new StringBuilder();
            int num = 1;
            foreach (var estadisticas in estats)
            {
                //["January", "February", "March", "April", "May", "June", "July"],
                if (num==1)
                    chart.Append(" [ '" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                if (estats.Count != num && num != 1)
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "',");
                if (estats.Count == num)
                    chart.Append("'" + string.Format("{0:MM-yyyy}", estadisticas.Fecha) + "'],");

                    num++;
            }

            return chart.ToString();
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

            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + SetLabelsChart());
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
            chart.Append("    var areaChartOptions = {");
            chart.Append("        responsive: true,");
            chart.Append("    animation: false,scaleOverride: true, scaleStartValue: 10000000, scaleStepWidth: 60000000, scaleSteps: 20,");
            chart.Append("        scaleLabel:");
            chart.Append("        function(label){return  ' $' + label.value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');}");
            chart.Append("    };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");
            return chart;
        }
        
        public StringBuilder ChartJavasScriptUtilidad()
        {
            StringBuilder chart = new StringBuilder();
            chart.Append("<script  type='text/javascript'>");
            chart.Append("$(function () {");
            chart.Append("    var areaChartCanvas = $('#areaChart2').get(0).getContext('2d');");
            chart.Append("    var areaChart = new Chart(areaChartCanvas);");
            chart.Append("    var areaChartData = {");
            chart.Append("        labels: " + SetLabelsChart());
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
            chart.Append("    var areaChartOptions = {");
            chart.Append("        responsive: true,");
            chart.Append("    animation: false,scaleOverride: true, scaleStartValue: 1000000, scaleStepWidth: 3000000, scaleSteps: 20,");
            chart.Append("        scaleLabel:");
            chart.Append("        function(label){return  ' $' + label.value.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ',');}");
            chart.Append("    };");
            chart.Append("    areaChart.Line(areaChartData, areaChartOptions);");
            chart.Append("});");
            chart.Append("</script>");
           
            return chart;
        }

        public SqlDataAdapter GetAdapterEstadisticas()
        {
            try
            {
                Common cls = new Common();
                string conn = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
                string spName = "usr_fnns.SP_SEL_CAPITAL_DIA";
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

    }
}