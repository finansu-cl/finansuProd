using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sifaco
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CultureInfo cInfo = new CultureInfo("es-CL");
            //cInfo.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            //cInfo.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = cInfo;
            //Thread.CurrentThread.CurrentUICulture = cInfo;

            if (Session["activo"] != null && Session["imgPerfil"] != null && Session["rol"] != null)
            {
                if (Session["activo"].ToString() != "" && Session["imgPerfil"].ToString() != "" && Session["rol"].ToString() != "")
                {
                    HttpCookie myCookie = Request.Cookies["activo"];
                    if (myCookie != null)
                    {
                        ltrName.Text = Session["login"].ToString();
                        ltrName2.Text = Session["login"].ToString();
                        imgUser.ImageUrl = Session["imgPerfil"].ToString();
                        imgUser2.ImageUrl = Session["imgPerfil"].ToString();
                        imgUser.ToolTip = Session["login"].ToString();
                        imgUser2.ToolTip = Session["login"].ToString();
                        imgUser3.ImageUrl = Session["imgPerfil"].ToString();
                        imgUser3.ToolTip = Session["login"].ToString();
                        ltrName3.Text = Session["login"].ToString();

                        if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                        {
                            Usuarios.Visible = true;
                            Autorizado.Visible = true;
                        }

                        if (Session["rol"] != null && Session["rol"].ToString() == "analista")
                        {
                            perEmp.Visible = false;
                            facSim.Visible = false;
                            Prestamo.Visible = false;
                            Contratos.Visible = false;
                            Liquidaciones.Visible = false;
                        }


                        if (Session["menu"] != null && Session["menu"].ToString() != "")
                        {
                            switch (Session["menu"].ToString()) 
                            {
                                case "1":
                                    per.Attributes.Add("class", "active");
                                    perEmp.Attributes.Add("class", "active treeview");
                                    break;
                                case "2":
                                    emp.Attributes.Add("class", "active");
                                    perEmp.Attributes.Add("class", "active treeview");
                                    break;
                                case "3":
                                    //fac.Attributes.Add("class", "active");
                                    facSim.Attributes.Add("class", "active treeview");
                                    break;
                                case "4":
                                    sim.Attributes.Add("class", "active");
                                    facSim.Attributes.Add("class", "active treeview");
                                    break;
                                case "5":
                                    Cobranza.Attributes.Add("class", "active");
                                    fcob.Attributes.Add("class", "active treeview");
                                    break;
                                case "6":
                                    Cobranza.Attributes.Add("class", "active");
                                    fcob.Attributes.Add("class", "active treeview");
                                    break;
                                case "7":
                                    Cobranza.Attributes.Add("class", "active");
                                    fven.Attributes.Add("class", "active treeview");
                                    break;
                                case "8":
                                    Cobranza.Attributes.Add("class", "active");
                                    bcob.Attributes.Add("class", "active treeview");
                                    break;
                                case "9":
                                    Cobranza.Attributes.Add("class", "active");
                                    fcm.Attributes.Add("class", "active treeview");
                                    break;
                                case "10":
                                    Prestamo.Attributes.Add("class", "active");
                                    pres.Attributes.Add("class", "active treeview");
                                    break;
                                case "11":
                                    Cobranza.Attributes.Add("class", "active");
                                    pcm.Attributes.Add("class", "active treeview");
                                    break;
                                case "12":
                                    Cobranza.Attributes.Add("class", "active");
                                    pcv.Attributes.Add("class", "active treeview");
                                    break;
                                case "13":
                                    Usuarios.Attributes.Add("class", "active");
                                    fuser.Attributes.Add("class", "active treeview");
                                    break;
                                case "14":
                                    Autorizado.Attributes.Add("class", "active");
                                    fpen.Attributes.Add("class", "active treeview");
                                    break;
                                case "15":
                                    Autorizado.Attributes.Add("class", "active");
                                    faut.Attributes.Add("class", "active treeview");
                                    break;
                                case "16":
                                    Autorizado.Attributes.Add("class", "active");
                                    frec.Attributes.Add("class", "active treeview");
                                    break;
                                case "17":
                                    Autorizado.Attributes.Add("class", "active");
                                    fvenc.Attributes.Add("class", "active treeview");
                                    break;
                                case "18":
                                    Cobranza.Attributes.Add("class", "active");
                                    bcobp.Attributes.Add("class", "active treeview");
                                    break;
                                case "19":
                                    Cobranza.Attributes.Add("class", "active");
                                    fsp.Attributes.Add("class", "active treeview");
                                    break;
                                case "20":
                                    Contratos.Attributes.Add("class", "active");
                                    ccar.Attributes.Add("class", "active treeview");
                                    break;
                                case "21":
                                    Liquidaciones.Attributes.Add("class", "active");
                                    lli.Attributes.Add("class", "active treeview");
                                    break;
                                case "22":
                                    Cobranza.Attributes.Add("class", "active");
                                    pcvv.Attributes.Add("class", "active treeview");
                                    break;
                                case "23":
                                    Cobranza.Attributes.Add("class", "active");
                                    bcobs.Attributes.Add("class", "active treeview");
                                    break;
                                case "24":
                                    Prestamo.Attributes.Add("class", "active");
                                    presR.Attributes.Add("class", "active treeview");
                                    break;
                            }
                        }
                    }
                    else 
                    {
                        Response.Redirect("../");
                    }
                }
                else 
                {
                    Response.Redirect("../");
                }
            }
            else 
            {
                Response.Redirect("../");
            }
        }

    }
}
