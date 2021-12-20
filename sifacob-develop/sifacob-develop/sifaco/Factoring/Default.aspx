<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Factoring.Default" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="datosFactura" Src="~/Controls/DatosFactura.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosSim" Src="~/Controls/Simulacion.ascx" %>
<%@ Register TagPrefix="uc" TagName="toPrint" Src="~/Controls/DocumentsToPrint.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script src="../Scripts/jquery.PrintArea.js_4.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#txtFecSimulacion").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFecSimulacion").inputmask();
        $("#txtFechaEmision").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaEmision").inputmask();

    });
</script>

<script type="text/javascript" language="javascript">
    function enviar() {

        var retorno = true;

        if (document.getElementById("txtDeudor").value == "") {
            alert("Debe completar el Deudor de la factura");
            return false;
        }

        if (document.getElementById("txtRutD").value == "") {
            alert("Debe ingresar el RUT del deudor");
            return false;
        }

        if (document.getElementById("txtNumFac").value == "") {
            alert("Debe completar el NUMERO de la factura");
            return false;s
        }
        var e = document.getElementById("ddlTipoFac");
        if (e.options[e.selectedIndex].value < "0" || e.options[e.selectedIndex].value == "") {
            alert("Debe seleccionar el TIPO de factura");
            return false;
        }

        if (document.getElementById("txtMonto").value == "") {
            alert("Debe ingresar el MONTO de la factura");
            return false;
        }


        if (document.getElementById("txtPlazoFac").value == "" || document.getElementById("txtPlazoFac").value == "0") {
            alert("Debe ingresar el PLAZO de la factura");
            return false;
        }

        if (parseFloat(document.getElementById("txtAnticipo").value) <= 0) {
            alert("El ANTICIPO de la simulacion debe ser mayor a cero (0)");
            return false;
        }

        if (parseFloat(document.getElementById("txtTasa").value) <= 0) {
            alert("La TASA de la simulacion debe ser mayor a cero (0)");
            return false;
        }

        if (document.getElementById("txtFechaEmision").value == "") {
            alert("Debe ingresar la FECHA DE EMISION de la factura");
            return false;
        }


        return retorno;

    }

    function calc() {
        var tasa = $("#txtTasa");
        var anticipo = $("#txtAnticipo");
        var saldoPen = $("#txtSalPendiente");
        var plazo = $("#txtPlazo");
        var gastosOper = $("#txtGasOperacion");
        var montoTot = $("#txtMonTotal");
        var utilidad = $("#txtMonInteres");
        var montoAntici = $("#txtMonAnticipo");
        var montoPen = $("#txtMonPendiente");
        var precioCes = $("#txtPreCesion");
        var montoGir = $("#txtMonGirable");
        var comision = $("#txtComision");
        var iva = $("#txtIva");

        if (tasa.val() == "")
            tasa.val(0);
        if (anticipo.val() == "")
            anticipo.val(0);
        if (gastosOper.val() == "")
            gastosOper.val(0);

        
        saldoPen.val(100 - anticipo.val().replace(/\,/g, "."));
        montoAntici.val(parseFloat((parseFloat(anticipo.val()) * parseFloat(stripDots(montoTot.val()).replace(/\,/g, "."))) / 100).toFixed(2).replace(/\./g, ","));
        montoPen.val(parseFloat(parseFloat(stripDots(montoTot.val()).replace(/\,/g, ".")) - parseFloat(stripDots(montoAntici.val()).replace(/\,/g, "."))).toFixed(2).replace(/\./g, ","));
        precioCes.val(parseFloat(parseFloat(stripDots(montoAntici.val()).replace(/\,/g, ".")) - parseFloat(stripDots(utilidad.val()).replace(/\,/g, "."))).toFixed(2).replace(/\./g, ","));
        if (comision.val() != "0" && comision.val() != "0,00") {
            iva.val(Math.round(parseFloat((19 * parseFloat(stripDots(comision.val()).replace(/\,/g, "."))) / 100).toFixed(0).replace(/\./g, ",")));
        } else {
        iva.val("0,00");
         } 
        montoGir.val(parseFloat(parseFloat(stripDots(precioCes.val()).replace(/\,/g, ".")) - parseFloat(stripDots(gastosOper.val()).replace(/\,/g, ".")) - parseFloat(stripDots(comision.val()).replace(/\,/g, ".")) - parseFloat(stripDots(iva.val()).replace(/\,/g, "."))).toFixed(2).replace(/\./g, ","));
        formatomiles("txtMonTotal");
        formatomiles("txtMonInteres");
        formatomiles("txtMonAnticipo");
        formatomiles("txtMonPendiente");
        formatomiles("txtPreCesion");
        formatomiles("txtMonGirable");
    }

    function stripDots(n) {
        var z = n.replace(/\./g, "");
        return z;
     }

     function valSim() {
         var retorno = true;
         if (parseFloat(document.getElementById("txtTasa").value) <= 0) {
             alert("La TASA de la simulacion debe ser mayor a cero (0)");
             return false;
         }
         if (parseFloat(document.getElementById("txtAnticipo").value) <= 0) {
             alert("El ANTICIPO de la simulacion debe ser mayor a cero (0)");
             return false;
         }
         if (parseFloat(document.getElementById("txtPlazo").value) <= 0) {
             alert("El PLAZO de la simulacion debe ser mayor a cero (0)");
             return false;
         }
         if (parseFloat(document.getElementById("txtMonTotal").value) <= 0) {
             alert("Debe crear facturas para calcular el monto total de la simulacion");
             return false;
         }

         return retorno;
      }

      $(document).ready(function () {
          $('#txtRutD').Rut({
              on_error: function () { alert('Rut incorrecto'); }
          });

        $("#btnGuardarFac").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });

        $("#btnGuardarSim").click(function () {
            if (valSim() == false)
                return false;
            else {
                return true;
            }
        });

        $("#txtTasa").blur(function () {
            calc();
        });
        $("#txtAnticipo").blur(function () {
            calc();
        });
        $("#txtPlazo").blur(function () {
            calc();
        });
        $("#txtGasOperacion").blur(function () {
            calc();
        });
        $("#txtComision").blur(function () {
            calc();
        });
        $("#txtIva").blur(function () {
            calc();
        });
        $("#txtMonto").blur(function () {
            calcFac();
        });
        $("#txtPlazoFac").blur(function () {
            calcFac();
        });

    });
    
    function calcFac() {
        var tasa = $("#txtTasa");
        var anticipo = $("#txtAnticipo");
        var saldoPen = $("#txtSalPendiente");
        var plazo = $("#txtPlazoFac");
        var montoTot = $("#txtMonto");
        var utilidad = $("#txtUtilidadFac");
        var montoAntici = $("#txtAnticipoFac");
        var montoPen = $("#txtSalPendienteFac");
        var montoGir = $("#txtGirableFac");

        if (tasa.val() == "")
            tasa.val(0);
        if (anticipo.val() == "")
            anticipo.val(0);
        if (plazo.val() == "")
            plazo.val(0);

        saldoPen.val(100 - anticipo.val().replace(/\,/g, "."));
        montoAntici.val(Math.round(parseFloat((parseFloat(anticipo.val().replace(/\,/g, ".")) * parseFloat(stripDots(montoTot.val()).replace(/\,/g, "."))) / 100)));
        montoPen.val(Math.round(parseFloat(parseFloat(stripDots(montoTot.val()).replace(/\,/g, ".")) - parseFloat(stripDots(montoAntici.val()).replace(/\,/g, ".")))));
        utilidad.val(Math.round(parseFloat(((((parseFloat(tasa.val().replace(/\,/g, ".")) / 100) / 30) * parseFloat(plazo.val())) * parseFloat(stripDots(montoAntici.val()).replace(/\,/g, "."))))));
        montoGir.val(Math.round(parseFloat(parseFloat(stripDots(montoAntici.val()).replace(/\,/g, ".")) - parseFloat(stripDots(utilidad.val()).replace(/\,/g, ".")))));
        formatomiles("txtMonto");
        formatomiles("txtUtilidadFac");
        formatomiles("txtAnticipoFac");
        formatomiles("txtSalPendienteFac");
        formatomiles("txtGirableFac");
    }

</script>
<script type="text/javascript">
    $(function () {
        $('#example2').DataTable({
            responsive: true,
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": false,
            "autoWidth": false
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Factoring
        <small>Simulación</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
<div>
        <div class="box box-primary">
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
                    <div class="box-header with-border">
                        <h3 class="box-title">Carga y simulación de factoring</h3>
                    </div>
                    <br />
                    <div class="row">
                        <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th style="text-align:center">Cliente</th>
                                    <th style="text-align:center">RUT</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="center" style="width:60%"><h3 class="box-title"><asp:Literal runat="server" ID="ltrCliente"></asp:Literal></h3></td>
                                    <td align="center" style="width:40%"><h3 class="box-title"><asp:Literal runat="server" ID="ltrRutCliente"></asp:Literal></h3></td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                    <div class="box-header with-border">
                      <h3 class="box-title">Datos Factura</h3>
                    </div>
                    <div class="row">
                        <uc:datosFactura runat="server" ID="fac" />
                        <div class="col-xs-3">
                        <br />
                             <% if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Convert.ToInt32(Session["EstadoSimulacion"]) == 1))
                                 { %>
                            <asp:Button runat="server" ID="btnGuardarFac" ClientIDMode="Static" CssClass="btn btn-info pull-right" Text="Guardar Factura" 
                                onclick="btnGuardarFac_Click" />
                            <asp:Button runat="server" ID="btnModificarFac" ClientIDMode="Static" Visible="false"
                                CssClass="btn btn-info pull-right" Text="Guardar Factura" 
                                onclick="btnModificarFac_Click" />
                            <%}%>
                        </div>
                    </div>
           <br />
           <div class="box-body table-responsive no-padding">
         <asp:PlaceHolder ID="plhDeleteQuestion" runat="server" Visible="false">
            <div class="alert alert-warning alert-dismissible">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                <h4><i class="icon fa fa-warning"></i> Alerta!</h4>
                    ¿Estas seguro de eliminar la factura?
                    <asp:Button runat="server" ID="btnEliminar" Text="Aceptar" CssClass="btn btn-block btn-warning" onclick="btnEliminar_Click" />
                    <asp:Literal ID="ltHidden" runat="server" Visible="false"></asp:Literal>
              </div>
        </asp:PlaceHolder>

              <table class="table table-hover">
                <tr>
                  <th align="center" style="width:5%;">Factura</th>
                  <th style="width:15%;">Deudor</th>
                  <th align="center">Tipo</th>
                  <th align="center">Plazo</th>
                  <th align="center">Monto</th>
                  <th align="center">Anticipo</th>
                  <th align="center">Pendiente</th>
                  <th align="center">Utilidad</th>
                  <th align="center">Saldo</th>
                  <th align="center">Emisión</th>
                  <th align="center">Editar                                         <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                                                                                        { %>
/ Eliminar<%} %></th>
                  <th align="center">Carta</th>
                </tr>
                <asp:Repeater runat="server" ID="rptFacturas"  onitemcommand="rptFacturas_ItemCommand">
                <ItemTemplate>
                    <tr>
                      <td align="center"><%# Eval("NumFactura")%></td>
                      <td><%# Eval("Deudor")%></td>
                      <td><%# Eval("Tipo").ToString().Substring(0,1)%></td>
                      <td><%# Eval("Plazo")+"Días"%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("MontoAnticipo").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("MontoPendiente").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Utilidad").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("MontoGirable").ToString()).ToString("N")%></td>
                      <td><%# string.Format("{0:dd/MM/yyyy}", Eval("Emision"))%></td>
                      <td>
                          <asp:Button ID="btnEdit" runat="server" CommandName="edit"  CssClass="btn btn-info" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                              Text='<%# (Session["rol"] != null && Session["rol"].ToString() == "admin") ? "Editar" : (Convert.ToInt32(Session["EstadoSimulacion"]) == 1) ? "Ver" : "Editar"%>' />
                                                                  <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                                                                      { %>

                          <asp:Button ID="btnDelete" runat="server" CommandName="delete"  CssClass="btn btn-danger" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="X" />
                          <%} %>
                      </td>
                      <td>
                           <asp:CheckBox runat="server" ID="chkCarta" />
                           <asp:HiddenField runat="server" ID="hdNumFac"  Value='<%# Eval("NumFactura")%>' />
                           <asp:HiddenField runat="server" ID="hdDirDeud"  Value='<%# Eval("DireccionDeudor")%>' />
                           <asp:HiddenField runat="server" ID="hdComDeud"  Value='<%# Eval("ComunaDeudor")%>' />
                      </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
              </table>
                    <asp:Button runat="server" ID="btnCrearCarta" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                     Text="Generar Cartas Notificadas" onclick="btnCrearCarta_Click" Enabled="false" />
            </div>
            <br />

                    <div class="box-header with-border">
                      <h3 class="box-title">Datos Simulación</h3>
                    </div>
                    <div class="col-md-6" style="width: 65% !important;">
                    <uc:datosSim runat="server" ID="Sim" />
                         <% if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Convert.ToInt32(Session["EstadoSimulacion"]) == 1))
                             { %>
                    <asp:Button runat="server" ID="btnGuardarSim" ClientIDMode="Static" CssClass="btn btn-info pull-right" onclick="btnGuardarSim_Click"
                    Text="Guardar Simulacion" /> 
                    <asp:Button runat="server" ID="btnLastSim" ClientIDMode="Static" CssClass="btn btn-warning pull-right" 
                    Text="Ver Ultima Simulacion" onclick="btnLastSim_Click" />
                        <%} %>
                    <a href="#" target="_blank" id="aPrint" runat="server">Imprimir</a>
                    </div>
                    <div class="col-md-6" style="width: 35% !important;">

                    <asp:Repeater runat="server" ID="rptCrearDocs" >
                        <ItemTemplate>
                            <div class="form-group">
                              <label for="inputPassword3" class="col-sm-2 control-label" style="width: 85% !important;"><%# Eval("Nombre")%></label>
                              <div class="col-sm-10" style="    width: 15% !important;">
                                <asp:CheckBox runat="server" ID="chkDoc" />
                                <asp:HiddenField runat="server" ID="hdItem"  Value='<%# Eval("Template")%>' />
                              </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button runat="server" ID="btnCrearDocs" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                              Text="Generar Contratos" onclick="btnCrearDocs_Click" Enabled="false" />
                    </div>                      
                        <iframe runat="server" id="ifr1" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr2" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr3" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr4" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr5" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr6" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr7" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr8" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr9" frameborder="0" style="height:10px;"></iframe>

                      <asp:Literal runat="server" ID="footIfr">
                      
                      </asp:Literal>
                </div>
            </div>
        </div>
</div>
</form>

</asp:Content>
