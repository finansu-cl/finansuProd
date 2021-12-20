using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data;
using System.Net;

namespace Clases
{
    public class Common
    {

        public SqlDataAdapter GetConnectionToDb(string connetionString, string spName, Dictionary<string, string> parametros, ArrayList tipoDato)
        {
            SqlConnection cnn;
            SqlDataAdapter reader = null;
            cnn = new SqlConnection(connetionString);
            try
            {
                using (SqlCommand cmd = new SqlCommand(spName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int i = 0;
                    foreach (KeyValuePair<string, string> kv in parametros)
                    {
                        //Key = Nombre Parametro; Value = Valor del Parametro
                        switch (tipoDato[i].ToString())
                        {
                            case "varchar":
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                            case "char":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Char).Value = kv.Value;
                                break;
                            case "int":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Int).Value = kv.Value;
                                break;
                            case "smallint":
                                cmd.Parameters.Add(kv.Key, SqlDbType.SmallInt).Value = Convert.ToInt16(kv.Value);
                                break;
                            case "numeric":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            case "datetime":
                                cmd.Parameters.Add(kv.Key, SqlDbType.DateTime).Value = kv.Value;
                                break;
                            case "float":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Float).Value = kv.Value;
                                break;
                            case "decimal":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            default:
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                        }
                        i++;
                    }

                    cnn.Open();
                    reader = new SqlDataAdapter(cmd);
                    cmd.Dispose();
                    cnn.Close();
                    return reader;
                }
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw new Exception(m);
                //return reader;
                //MessageBox.Show("Can not open connection ! ");
            }
        }

        public void GetConnectionToDbInsert(string connetionString, string spName, Dictionary<string, string> parametros, ArrayList tipoDato)
        {
            SqlConnection cnn;
            SqlDataAdapter reader = null;
            cnn = new SqlConnection(connetionString);
            try
            {
                using (SqlCommand cmd = new SqlCommand(spName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int i = 0;
                    foreach (KeyValuePair<string, string> kv in parametros)
                    {
                        //Key = Nombre Parametro; Value = Valor del Parametro
                        switch (tipoDato[i].ToString())
                        {
                            case "varchar":
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                            case "char":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Char).Value = kv.Value;
                                break;
                            case "int":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Int).Value = kv.Value;
                                break;
                            case "smallint":
                                cmd.Parameters.Add(kv.Key, SqlDbType.SmallInt).Value = Convert.ToInt16(kv.Value);
                                break;
                            case "numeric":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            case "datetime":
                                cmd.Parameters.Add(kv.Key, SqlDbType.DateTime).Value = kv.Value;
                                break;
                            case "float":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Float).Value = kv.Value;
                                break;
                            case "decimal":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            case "decimal18":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                /*SqlParameter param = new SqlParameter(kv.Key, SqlDbType.Decimal,18);
                                param.SourceColumn = kv.Key.Substring(1);
                                param.Precision = 18;
                                param.Scale = 2;
                                cmd.Parameters.Add(param).Value = Convert.ToDecimal(kv.Value);*/
                                break;
                            default:
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                        }
                        i++;
                    }

                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                //return reader;
                //MessageBox.Show("Can not open connection ! ");
            }
        }

        public void GetConnectionToDbNonQuery(string connetionString, string spName, Dictionary<string, string> parametros, ArrayList tipoDato)
        {
            SqlConnection cnn;
            cnn = new SqlConnection(connetionString);
            try
            {
                using (SqlCommand cmd = new SqlCommand(spName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int i = 0;
                    foreach (KeyValuePair<string, string> kv in parametros)
                    {
                        //Key = Nombre Parametro; Value = Valor del Parametro
                        switch (tipoDato[i].ToString())
                        {
                            case "varchar":
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                            case "char":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Char).Value = kv.Value;
                                break;
                            case "int":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Int).Value = kv.Value;
                                break;
                            case "smallint":
                                cmd.Parameters.Add(kv.Key, SqlDbType.SmallInt).Value = Convert.ToInt16(kv.Value);
                                break;
                            case "numeric":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            case "datetime":
                                cmd.Parameters.Add(kv.Key, SqlDbType.DateTime).Value = kv.Value;
                                break;
                            case "float":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Float).Value = kv.Value;
                                break;
                            case "decimal":
                                cmd.Parameters.Add(kv.Key, SqlDbType.Decimal).Value = kv.Value;
                                break;
                            default:
                                cmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;
                                break;
                        }
                        i++;
                    }

                    cnn.Open();
                    cmd.CommandTimeout = 200;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }
        }

        public void LogFile(string logMessage, string fileName, Exception ex)
        {
            try
            {
                StreamWriter w = File.AppendText(fileName);
                Log(logMessage, w, ex);
            }
            catch (Exception exc)
            {
                string m = exc.Message;
            }
        }

        public static void Log(string logMessage, TextWriter w, Exception exc)
        {
            var st = new StackTrace(exc, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            w.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                w.Write("Inner Exception Type: ");
                w.WriteLine(exc.InnerException.GetType().ToString());
                w.Write("Inner Exception: ");
                w.WriteLine(exc.InnerException.Message);
                w.Write("Inner Source: ");
                w.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    w.WriteLine("Inner Stack Trace: ");
                    w.WriteLine(exc.InnerException.StackTrace);
                }
            }
            w.Write("Exception Type: ");
            w.WriteLine(exc.GetType().ToString());
            w.WriteLine("Exception: " + exc.Message);
            w.WriteLine("Source: " + exc.Source);
            w.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                w.WriteLine(exc.StackTrace);
                w.WriteLine();
            }
            w.Write("Line Number: ");
            w.WriteLine(line.ToString());

            w.Close();
        }

        public string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }

        public string DecimalToWords(decimal number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + DecimalToWords(Math.Abs(number));

            string words = "";

            int intPortion = (int)number;
            decimal fraction = (number - intPortion) * 100;
            int decPortion = (int)fraction;

            words = NumberToWords(intPortion);
            if (decPortion > 0)
            {
                words += " and ";
                words += NumberToWords(decPortion);
            }
            return words;
        }

        public string NumberToWords(int number)
        {
            try
            {
                if (number == 0)
                    return "cero";

                if (number < 0)
                    return "menos " + NumberToWords(Math.Abs(number));

                string words = "";

                if ((number / 1000000) > 0)
                {
                    words += (NumberToWords(number / 1000000) == "uno") ? "un millón " : NumberToWords(number / 1000000) + " millones ";
                    number %= 1000000;
                }

                if ((number / 1000) > 0)
                {
                    words += NumberToWords(number / 1000) + " mil ";
                    number %= 1000;
                }

                if ((number / 100) > 0 && (number / 100) != 5 && (number / 100) != 7 && (number / 100) != 9)
                {
                    words += (NumberToWords(number / 100) == "uno") ? "ciento " : NumberToWords(number / 100) + "cientos ";
                    number %= 100;
                }

                if ((number / 100) > 0 && (number / 100) == 5)
                {
                    words += " quinientos ";
                    number %= 100;
                }

                if ((number / 100) > 0 && (number / 100) == 7)
                {
                    words += " setecientos ";
                    number %= 100;
                }

                if ((number / 100) > 0 && (number / 100) == 9)
                {
                    words += " novecientos ";
                    number %= 100;
                }

                if (number > 0)
                {
                    if (words != "")
                        words += "";

                    var unitsMap = new[] { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve" };
                    var tensMap = new[] { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        if (number > 20 && number <= 29)
                        {
                            words += " veinti";
                            if ((number % 10) > 0)
                                words += "" + unitsMap[number % 10];
                        }
                        else
                        {
                            words += tensMap[number / 10];
                            if ((number % 10) > 0)
                                words += " y " + unitsMap[number % 10];
                        }
                    }
                }

                return words;
            }
            catch(Exception ex)
                {
                return "";
                }
        }

        public ArrayList NumOrdinales() 
        {
            ArrayList numO = new ArrayList();
            numO.Add("Primera");
            numO.Add("Segunda");
            numO.Add("Tercera");
            numO.Add("Cuarta");
            numO.Add("Quinta");
            numO.Add("Sexta");
            numO.Add("Septima");
            numO.Add("Octava");
            numO.Add("Novena");
            numO.Add("Decima");
            numO.Add("Decima primera");
            numO.Add("Decima segunda");
            numO.Add("Decima tercera");
            numO.Add("Decima cuarta");
            numO.Add("Decima quinta");
            numO.Add("Decima sexta");
            numO.Add("Decima septima");
            numO.Add("Decima octava");
            numO.Add("Decima novena");
            numO.Add("Vigesima");
            numO.Add("Vigesima primera");
            numO.Add("Vigesima segunda");
            numO.Add("Vigesima tercera");
            numO.Add("Vigesima cuarta");
            numO.Add("Vigesima quinta");
            numO.Add("Vigesima sexta");
            numO.Add("Vigesima septima");
            numO.Add("Vigesima octava");
            numO.Add("Vigesima novena");
            numO.Add("Trigesima");
            numO.Add("Trigesima primera");
            numO.Add("Trigesima segunda");
            numO.Add("Trigesima tercera");
            numO.Add("Trigesima cuarta");
            numO.Add("Trigesima quinta");
            numO.Add("Trigesima sexta");
            numO.Add("Trigesima septima");
            numO.Add("Trigesima octava");
            numO.Add("Trigesima novena");
            numO.Add("Cuadragesima");
            numO.Add("Cuadragesima primera");
            numO.Add("Cuadragesima segunda");
            numO.Add("Cuadragesima tercera");
            numO.Add("Cuadragesima cuarta");
            numO.Add("Cuadragesima quinta");
            numO.Add("Cuadragesima sexta");
            numO.Add("Cuadragesima septima");
            numO.Add("Cuadragesima octava");
            numO.Add("Cuadragesima novena");
            numO.Add("Quincuagesima");
            numO.Add("Quincuagesima primera");
            numO.Add("Quincuagesima segunda");
            numO.Add("Quincuagesima tercera");
            numO.Add("Quincuagesima cuarta");
            numO.Add("Quincuagesima quinta");
            numO.Add("Quincuagesima sexta");
            numO.Add("Quincuagesima septima");
            numO.Add("Quincuagesima octava");
            numO.Add("Quincuagesima novena");
            numO.Add("Sexagesima");
            numO.Add("Sexagesima primera");
            numO.Add("Sexagesima segunda");
            numO.Add("Sexagesima tercera");
            numO.Add("Sexagesima cuarta");
            numO.Add("Sexagesima quinta");
            numO.Add("Sexagesima sexta");
            numO.Add("Sexagesima septima");
            numO.Add("Sexagesima octava");
            numO.Add("Sexagesima novena");
            numO.Add("Septuagesima");
            numO.Add("Septuagesima primera");
            numO.Add("Septuagesima segunda");
            numO.Add("Septuagesima tercera");
            numO.Add("Septuagesima cuarta");
            numO.Add("Septuagesima quinta");
            numO.Add("Septuagesima sexta");
            numO.Add("Septuagesima septima");
            numO.Add("Septuagesima octava");
            numO.Add("Septuagesima novena");
            numO.Add("Octogesima");
            numO.Add("Octogesima primera");
            numO.Add("Octogesima segunda");
            numO.Add("Octogesima tercera");
            numO.Add("Octogesima cuarta");
            numO.Add("Octogesima quinta");
            numO.Add("Octogesima sexta");
            numO.Add("Octogesima septima");
            numO.Add("Octogesima octava");
            numO.Add("Octogesima novena");
            numO.Add("Nonagesima");
            numO.Add("Nonagesima primera");
            numO.Add("Nonagesima segunda");
            numO.Add("Nonagesima tercera");
            numO.Add("Nonagesima cuarta");
            numO.Add("Nonagesima quinta");
            numO.Add("Nonagesima sexta");
            numO.Add("Nonagesima septima");
            numO.Add("Nonagesima octava");
            numO.Add("Nonagesima novena");
            numO.Add("Centesima");
            numO.Add("Centesima primera");
            numO.Add("Centesima segunda");
            numO.Add("Centesima tercera");
            numO.Add("Centesima cuarta");
            numO.Add("Centesima quinta");
            numO.Add("Centesima sexta");
            numO.Add("Centesima septima");
            numO.Add("Centesima octava");
            numO.Add("Centesima novena");
            numO.Add("Centesima Decima");
            numO.Add("Centesima Decima primera");
            numO.Add("Centesima Decima segunda");
            numO.Add("Centesima Decima tercera");
            numO.Add("Centesima Decima cuarta");
            numO.Add("Centesima Decima quinta");
            numO.Add("Centesima Decima sexta");
            numO.Add("Centesima Decima septima");
            numO.Add("Centesima Decima octava");
            numO.Add("Centesima Decima novena");
            numO.Add("Centesima Vigesima");
            numO.Add("Centesima Vigesima primera");
            numO.Add("Centesima Vigesima segunda");
            numO.Add("Centesima Vigesima tercera");
            numO.Add("Centesima Vigesima cuarta");
            numO.Add("Centesima Vigesima quinta");
            numO.Add("Centesima Vigesima sexta");
            numO.Add("Centesima Vigesima septima");
            numO.Add("Centesima Vigesima octava");
            numO.Add("Centesima Vigesima novena");
            numO.Add("Centesima Trigesima");
            numO.Add("Centesima Trigesima primera");
            numO.Add("Centesima Trigesima segunda");
            numO.Add("Centesima Trigesima tercera");
            numO.Add("Centesima Trigesima cuarta");
            numO.Add("Centesima Trigesima quinta");
            numO.Add("Centesima Trigesima sexta");
            numO.Add("Centesima Trigesima septima");
            numO.Add("Centesima Trigesima octava");
            numO.Add("Centesima Trigesima novena");
            numO.Add("Centesima Cuadragesima");
            numO.Add("Centesima Cuadragesima primera");
            numO.Add("Centesima Cuadragesima segunda");
            numO.Add("Centesima Cuadragesima tercera");
            numO.Add("Centesima Cuadragesima cuarta");
            numO.Add("Centesima Cuadragesima quinta");
            numO.Add("Centesima Cuadragesima sexta");
            numO.Add("Centesima Cuadragesima septima");
            numO.Add("Centesima Cuadragesima octava");
            numO.Add("Centesima Cuadragesima novena");
            numO.Add("Centesima Quincuagesima");
            numO.Add("Centesima Quincuagesima primera");
            numO.Add("Centesima Quincuagesima segunda");
            numO.Add("Centesima Quincuagesima tercera");
            numO.Add("Centesima Quincuagesima cuarta");
            numO.Add("Centesima Quincuagesima quinta");
            numO.Add("Centesima Quincuagesima sexta");
            numO.Add("Centesima Quincuagesima septima");
            numO.Add("Centesima Quincuagesima octava");
            numO.Add("Centesima Quincuagesima novena");
            numO.Add("Centesima Sexagesima");
            numO.Add("Centesima Sexagesima primera");
            numO.Add("Centesima Sexagesima segunda");
            numO.Add("Centesima Sexagesima tercera");
            numO.Add("Centesima Sexagesima cuarta");
            numO.Add("Centesima Sexagesima quinta");
            numO.Add("Centesima Sexagesima sexta");
            numO.Add("Centesima Sexagesima septima");
            numO.Add("Centesima Sexagesima octava");
            numO.Add("Centesima Sexagesima novena");
            numO.Add("Centesima Septuagesima");
            numO.Add("Centesima Septuagesima primera");
            numO.Add("Centesima Septuagesima segunda");
            numO.Add("Centesima Septuagesima tercera");
            numO.Add("Centesima Septuagesima cuarta");
            numO.Add("Centesima Septuagesima quinta");
            numO.Add("Centesima Septuagesima sexta");
            numO.Add("Centesima Septuagesima septima");
            numO.Add("Centesima Septuagesima octava");
            numO.Add("Centesima Septuagesima novena");
            numO.Add("Centesima Octogesima");
            numO.Add("Centesima Octogesima primera");
            numO.Add("Centesima Octogesima segunda");
            numO.Add("Centesima Octogesima tercera");
            numO.Add("Centesima Octogesima cuarta");
            numO.Add("Centesima Octogesima quinta");
            numO.Add("Centesima Octogesima sexta");
            numO.Add("Centesima Octogesima septima");
            numO.Add("Centesima Octogesima octava");
            numO.Add("Centesima Octogesima novena");
            numO.Add("Centesima Nonagesima");
            numO.Add("Centesima Nonagesima primera");
            numO.Add("Centesima Nonagesima segunda");
            numO.Add("Centesima Nonagesima tercera");
            numO.Add("Centesima Nonagesima cuarta");
            numO.Add("Centesima Nonagesima quinta");
            numO.Add("Centesima Nonagesima sexta");
            numO.Add("Centesima Nonagesima septima");
            numO.Add("Centesima Nonagesima octava");
            numO.Add("Centesima Nonagesima novena");

            return numO;
        }

        public string CampoValor(string clave, string valor)
        {
            string retorno = valor;
            switch (clave)
            {
                case "ID_EDO_FACTURA":
                    switch (valor)
                    {
                        case "1":
                            retorno = "SIN VENCER";
                            break;
                        case "2":
                            retorno = "VENCIDA";
                            break;
                        case "3":
                            retorno = "PAGADA";
                            break;
                        case "4":
                            retorno = "PAGO PARCIAL";
                            break;
                        case "5":
                            retorno = "PAGADO SIN INTERESES";
                            break;
                        case "6":
                            retorno = "EN JUICIO";
                            break;
                        case "7":
                            retorno = "INCOBRABLE";
                            break;
                        case "11":
                            retorno = "PAGO PARCIAL MORA";
                            break;
                    }
                    break;
                case "ID_EDO_SIM":
                    switch (valor)
                    {
                        case "1":
                            retorno = "CREADA";
                            break;
                        case "2":
                            retorno = "APROBADA";
                            break;
                    }
                    break;
                case "ID_TIPO_FACTURA":
                    switch (valor)
                    {
                        case "1":
                            retorno = "F";
                            break;
                        case "2":
                            retorno = "E";
                            break;
                    }
                    break;
                case "FLG_NOTIFICACION":
                    switch (valor)
                    {
                        case "1":
                            retorno = "SI";
                            break;
                        case "2":
                            retorno = "NO";
                            break;
                    }
                    break;
                case "ID_EDO_PRES":
                    switch (valor)
                    {
                        case "1":
                            retorno = "SIN VENCER";
                            break;
                        case "2":
                            retorno = "VENCIDA";
                            break;
                        case "3":
                            retorno = "PAGADA";
                            break;
                        case "4":
                            retorno = "PAGO PARCIAL";
                            break;
                        case "5":
                            retorno = "PAGADO SIN INTERESES";
                            break;
                        case "6":
                            retorno = "EN JUICIO";
                            break;
                        case "7":
                            retorno = "INCOBRABLE";
                            break;
                    }
                    break;
            }

            return retorno;
        }

        public string RetornoCampo(string clave)
        {
            string retorno = clave;
            switch (clave)
            {
                case "ID_EDO_FACTURA":
                    retorno = "ESTADO FACTURA";
                    break;
                case "NAME_USER":
                    retorno = "USUARIO";
                    break;
                case "ID_EDO_SIM":
                    retorno = "ESTADO SIMULACION";
                    break;
                case "ID_TIPO_FACTURA":
                    retorno = "TIPO FACTURA";
                    break;
                //case "NUM_FACTURA":
                //    retorno = "Numero Factura";
                //    break;
                //case "PLAZO":
                //    retorno = "Plazo";
                //    break;
                //case "FECHA_MOD":
                //    retorno = "Fecha Modificacion";
                //    break;
                //case "FECHA_CREA":
                //    retorno = "Fecha Creacion";
                //    break;
                //case "MONTO_INTERES":
                //    retorno = "Intereses";
                //    break;
                //case "MONTO_TOTAL":
                //    retorno = "Monto";
                //    break;
                //case "MONTO_GIRABLE":
                //    retorno = "Monto Girable";
                //    break;
                //case "MONTO_ANTICIPO":
                //    retorno = "Monto Anticipo";
                //    break;
                //case "MONTO_PENDIENTE":
                //    retorno = "Monto Pendiente";
                //    break;
                //case "MONTO_RESTANTE":
                //    retorno = "Monto Restante";
                //    break;
                //case "DEUDOR":
                //    retorno = "Deudor";
                //    break;
                //case "DEUDOR":
                //    retorno = "Deudor";
                //    break;
                //case "DEUDOR":
                //    retorno = "Deudor";
                //    break;
                //case "DEUDOR":
                //    retorno = "Deudor";
                //    break;
                //case "DEUDOR":
                //    retorno = "Deudor";
                //    break;
            }

            return retorno.Replace('_',' ');
        }

        public string ValuesAltered(string allValues, string idObj) 
        {
            string retorno = "";
            try
            {
                if (allValues.IndexOf("ELIMINA") < 0 && allValues.IndexOf("MODIFICA") >= 0)
                {
                    string[] datos = allValues.Split('|');
                    string origen = datos[0].ToString();
                    string id = origen.Replace(' ', '_').Replace(':', '_').Replace('.', '_');
                    string[] datosViejos = datos[1].Split('>');
                    string[] datosNuevos = datos[2].Split('>');
                    string viejos = datosViejos[0].ToString();
                    string[] claveValorViejos = datosViejos[1].Split(';');
                    string nuevos = datosNuevos[0].ToString();
                    string[] claveValorNuevos = datosNuevos[1].Split(';');
                    StringBuilder table = new StringBuilder();
                    table.Append("<div class='box'><div class='box-header'><h3 class='box-title'><a id='vc_" + idObj + "_" + id + "'>");
                    table.Append(origen.Substring(2));
                    table.Append("</a></h3></div><div class='box-body no-padding' style='display:none;' id='dc_" + idObj + "_" + id + "'><table class='table table-condensed'><tr><th style='width: 40px'>Campos</th>");
                    table.Append("<th>Valor</th><th>Valor Nuevo</th></tr>");

                    for (int x = 0; x < claveValorViejos.Length; x = x + 2)
                    {
                        table.Append("<tr><td>");
                        table.Append(RetornoCampo(claveValorViejos[x]));
                        table.Append("</td>");
                        table.Append("<td>");
                        table.Append(CampoValor(claveValorViejos[x], claveValorViejos[x + 1]));
                        table.Append("</td>");
                        table.Append("<td>");
                        table.Append(CampoValor(claveValorViejos[x], claveValorNuevos[x + 1]));
                        table.Append("</td></tr>");
                    }
                    table.Append("</table></div></div>");
                    //table.Append("<script type='text/javascript'>$(document).ready(function () { $('#vc_" + idObj + "_" + id + "').click(function () {$('#dc_" + idObj + "_" + id + "').toggle();});});</script>");
                    table.Append("<script type='text/javascript'>MiClick('#vc_" + idObj + "_" + id + "','#dc_" + idObj + "_" + id + "');</script>");
                    //
                    //


                    retorno = table.ToString();
                }
                else
                {
                    retorno = allValues.Substring(2);
                }
                return retorno;
            }
            catch
            {
                return retorno;
            }
        }

    }

    public class Direccion
    {
        public string Direcciones { get; set; }
    }

    public class Documentos
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Template { get; set; }
        public string Funcionalidad { get; set; }
        public string Url { get; set; }
    }

    public class Region 
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    public class Ciudad
    {
        public int ID { get; set; }
        public int IDRegion { get; set; }
        public string Nombre { get; set; }
    }

    public class Comuna
    {
        public int ID { get; set; }
        public int IDCiudad { get; set; }
        public string Nombre { get; set; }
    }



    public class Clientes
    {
        public int ID { get; set; }
        public int? IdCliente { get; set; }
        public int? IdPersona { get; set; }
        public int? IdEmpresa { get; set; }
        public decimal? MontoFactoring { get; set; }
        public decimal? MontoMora { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public int NumOperacion { get; set; }
    }

    public class Persona
    {
        public int ID { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string EdoCivil { get; set; }
        public string Sexo { get; set; }
        public string Nacionalidad { get; set; }
        public string Profesion { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
    }

    public class Compareciente
    {
        public int ID { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string EdoCivil { get; set; }
        public string Sexo { get; set; }
        public string Nacionalidad { get; set; }
        public string Profesion { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Empresa { get; set; }
        public string RutEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
    }

    public class Bancos
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    public class DatosBanco
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public int IdBanco { get; set; }
        public string Banco { get; set; }
        public string NumCuenta { get; set; }
        public string Destinatario { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; }
    }

    public class Avales
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }
    }


    public class Empresa
    {
        public int ID { get; set; }
        public int IdTipoEmpresa { get; set; }
        public string Tipo { get; set; }
        public string Rut { get; set; }
        public string RazonSocial { get; set; }
        public string Giro { get; set; }
        public string NombreNotaria { get; set; }
        public string Direccion { get; set; }
        public string FechaEscritura { get; set; }
        public string Telefono { get; set; }
    }

    public class TipoEmpresa
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
    }

    public class TipoFactura
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
    }

    public class Facturas
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public int idTipoFac { get; set; }
        public string Tipo { get; set; }
        public string NumFactura { get; set; }
        public decimal Monto { get; set; }
        public int Plazo { get; set; }
        public decimal Utilidad { get; set; }
        public decimal MontoGirable { get; set; }
        public decimal MontoAnticipo { get; set; }
        public decimal MontoPendiente { get; set; }
        public decimal MontoRestante { get; set; }
        public decimal MontoParcial { get; set; }
        public string Deudor { get; set; }
        public string RutDeudor { get; set; }
        public string DireccionDeudor { get; set; }
        public string ComunaDeudor { get; set; }
        public int IdEdoFactura { get; set; }
        public decimal MontoMora { get; set; }
        public decimal MontoMoraParcial { get; set; }
        public decimal MontoReembolso { get; set; }
        public string EstadoFactura { get; set; }
        public string Notificacion { get; set; }
        public DateTime? Vencimiento { get; set; }
        public DateTime? FechaPagoMoraParcial { get; set; }
        public DateTime? Operacion { get; set; }
        public DateTime? Pago { get; set; }
        public DateTime? FechaPagoParcial { get; set; }
        public DateTime? Emision { get; set; }
        public int VenceEn { get; set; }
        public decimal Tasa { get; set; }
        public string Observacion { get; set; }
        public string Devuelto { get; set; }
        public string MoraPagada { get; set; }

    }

    public class Simulaciones 
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public int ID { get; set; }
        public int IdEdoSim { get; set; }
        public string Estado { get; set; }
        public int IdCliente { get; set; }
        public decimal? Tasa { get; set; }
        public decimal? NuevaTasa { get; set; }
        public decimal? Anticipo { get; set; }
        public decimal? SaldoPendiente { get; set; }
        public int? Plazo { get; set; }
        public decimal? GastosOper { get; set; }
        public decimal? Utilidad { get; set; }
        public decimal? MontoTotal { get; set; }
        public decimal? PrecioCes { get; set; }
        public decimal? MontoGirable { get; set; }
        public decimal? MontoAnticipo { get; set; }
        public decimal? MontoPendiente { get; set; }
        public decimal? Comision { get; set; }
        public decimal? Iva { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? NuevaFecha { get; set; }
        public DateTime? FechaPrimeraOp { get; set; }
    }

    public class PrestamosA
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public int? ID { get; set; }
        public int? IdPrestamo { get; set; }
        public int? IdEdoPres { get; set; }
        public int? IdCliente { get; set; }
        public decimal? Tasa { get; set; }
        public decimal? NumCuota { get; set; }
        public int? NumTotalCuota { get; set; }
        public int? MesGracia { get; set; }
        public int? Plazo { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? SaldoFinal { get; set; }
        public decimal? Capital { get; set; }
        public decimal? Intereses { get; set; }
        public decimal? PagoParcialMora { get; set; }
        public decimal? MontoRestanteMora { get; set; }
        public decimal? MontoRestanteCapital { get; set; }
        public decimal? PagoParcialIntereses { get; set; }
        public decimal? MontoRestanteIntereses { get; set; }
        public decimal? Monto { get; set; }
        public decimal? Cuota { get; set; }
        public decimal? Mora { get; set; }
        public decimal? Parcial { get; set; }
        public decimal? CapitalParcial { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? Vencimiento { get; set; }
        public int? NumeroCuotas { get; set; }
        public int VenceEn { get; set; }
        public string Observacion { get; set; }
        public int? EstadoOpera { get; set; }
        public int? IdPrestamoReorganizado { get; set; }


    }

    public class MoraTotal
    {
        public decimal? Mora { get; set; }
    }

    public class EstadoSimulacion
    {
        public int ID { get; set; }
        public string Estado { get; set; }
    }

    public class EstadoFactura
    {
        public int ID { get; set; }
        public string Estado { get; set; }
    }

    public class Usuario
    {
        public int ID { get; set; }
        public string Clave { get; set; }
        public string ClaveConfirm { get; set; }
        public string Perfil { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Activo { get; set; }
        public string PerfilImg { get; set; }
        public string FechaCrea { get; set; }
        public string Error { get; set; }
    }

    public class Estadisticas
    {
        public decimal? Capital { get; set; }
        public decimal? Utilidad { get; set; }
        public string Fecha { get; set; }
    }

    public class Notaria
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    public class DetalleNotaria
    {
        public int ID { get; set; }
        public int IdNotaria { get; set; }
        public string Descripcion { get; set; }
        public double Valor { get; set; }
    }

    public class Correlativos
    {
        public int Correlativo { get; set; }
    }

    public class Operacion
    {
        public int ID { get; set; }
        public int IdOrigen { get; set; }
        public int? IdDestino { get; set; }
        public string Origen { get; set; }
        public string TipoOperacion { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? FechaMod { get; set; }
        public int Autorizado { get; set; }
        public string UsuarioAut { get; set; }
        public DateTime? FechaAut { get; set; }
        public int Correlativo { get; set; }
    }

    public class PrendasV
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public string RutInscrito { get; set; }
        public string NombreInscrito { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Ano { get; set; }
        public string Motor { get; set; }
        public string Chasis { get; set; }
        public string Color { get; set; }
        public string Patente { get; set; }
        public string Rvm { get; set; }
        public string Notaria { get; set; }
        public DateTime? FechaEscritura { get; set; }
    }

    public class PrendasH
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public string Deslindes { get; set; }
        public string NombreCompro { get; set; }
        public string NombreNotario { get; set; }
        public string Fojas { get; set; }
        public string Numero { get; set; }
        public string UbicacionCbrs { get; set; }
        public string Ano { get; set; }
        public string Comuna { get; set; }
        public string Rol { get; set; }
        public DateTime? FechaEscritura { get; set; }
    }

    public class PagosParciales
    {
        public int ID { get; set; }
        public int IdPrestamo { get; set; }
        public int IdAmortizacion { get; set; }
        public decimal? MontoParcial { get; set; }
        public DateTime? FechaPagoParcial { get; set; }
    }

    public class EstadisticasTotales
    {
        public int? CantFacturasMora { get; set; }
        public int? CantFacturasVencerVencidas { get; set; }
        public decimal? FacturasCobradasMes { get; set; }
        public int? CantOperacionesMes { get; set; }
    }

}
