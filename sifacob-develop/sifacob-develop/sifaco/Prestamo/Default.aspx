<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Prestamo.Default" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="datosPrestamos" Src="~/Controls/DatosPrestamo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosSim" Src="~/Controls/Simulacion.ascx" %>
<%@ Register TagPrefix="uc" TagName="toPrint" Src="~/Controls/DocumentsPresToPrint.ascx" %>
<%@ Register TagPrefix="uc" TagName="modalSD" Src="~/Controls/ModalSD.ascx" %>
<%@ Register TagPrefix="uc" TagName="modalCP" Src="~/Controls/ModalCP.ascx" %>
<%@ Register TagPrefix="uc" TagName="modalMI" Src="~/Controls/ModalMI.ascx" %>
<%@ Register TagPrefix="uc" TagName="modalMH" Src="~/Controls/ModalMH.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
.oculto{display:none;}
</style>
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script src="../Scripts/jquery.PrintArea.js_4.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function enviar() {

        var retorno = true;

        if (document.getElementById("txtMonto").value == "") {
            alert("Debe ingresar el MONTO del Prestamo");
            return false;
        }


        if (document.getElementById("txtPlazo").value == "" || document.getElementById("txtPlazoFac").value == "0") {
            alert("Debe ingresar el PLAZO del Prestamo");
            return false;
        }

        if (document.getElementById("txtNumCuotas").value == "" || document.getElementById("txtNumCuotas").value == "0") {
            alert("Debe ingresar el NUMERO DE CUOTAS del Prestamo");
            return false;
        }


        if (parseFloat(document.getElementById("txtTasa").value) <= 0) {
            alert("La TASA de la simulacion debe ser mayor a cero (0)");
            return false;
        }

        return retorno;
    }

    function calc() {
        var tasa = $("#txtTasa");
        var plazo = $("#txtPlazo");
        var numCuotas = $("#txtNumCuotas");
        var monto = $("#txtMonto");
        var cuota = $("#txtCuota");
        var plazoCeil = numCuotas.val(); //Math.ceil(plazo.val() / 30);
        if (tasa.val() == "")
            tasa.val(0);

        var tasa12 = parseFloat(tasa.val().replace(/\,/g, ".") / 100);

        var a = (parseFloat(stripDots(monto.val()).replace(/\,/g, ".")) * tasa12) * Math.pow((1 + tasa12), plazoCeil);
        var b = Math.pow((1 + tasa12), plazoCeil) - 1;
        cuota.val(parseFloat(a / b).toFixed(2).replace(/\./g, ","));

        formatomiles("txtMonto");
        formatomiles("txtCuota");
    }

    function stripDots(n) {
        var z = n.replace(/\./g, "");
        return z;
     }

    $(document).ready(function () {

       calc();

	  $('#Form1').on('keyup keypress', function(e) {
      var keyCode = e.keyCode || e.which;
      if (keyCode === 13) { 
        e.preventDefault();
        return false;
      }
    });
        var currentSelectedValue; //Variable para capturar el valor del dropdown de una cuota antes de cambiar su selección.

        $("#btnGuardarFac").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });

        $("#txtTasa").blur(function () {
            calc();
        });

        $("#txtPlazo").blur(function () {
            calc();
        });
        $("#txtNumCuotas").blur(function () {
            calc();
        });
        $("#txtMonto").blur(function () {
            calc();
        });

    });

    function SaveValue(idFac, optVal, edo, currentSelectedValue) {

        if (optVal == 3 || optVal == 4 || optVal == 11 || optVal == 14) {
            if (optVal == 3) {
                $("#datePick" + idFac).show();
                $("#btnGuar" + idFac).show(); //btnGuar

                $("#pagoParcial" + idFac).hide();
                $("#btnGuarP" + idFac).hide(); //btnGuar
                $("#btnGuarP2" + idFac).hide(); //btnGuar
                $("#pagoParcialMora" + idFac).hide();
                $("#btnGuarPMora" + idFac).hide(); //btnGuar

            } else if (optVal == 4) {
                $("#datePick" + idFac).show();
                $("#pagoParcial" + idFac).show();
                $("#btnGuarP" + idFac).show(); //btnGuar

                $("#pagoParcialMora" + idFac).hide();
                $("#btnGuarPMora" + idFac).hide(); //btnGuar
                $("#btnGuarP2" + idFac).hide(); //btnGuar
                $("#btnGuar" + idFac).hide(); //btnGuar

            } else if (optVal == 14) {
                $("#datePick" + idFac).show();
                $("#pagoParcial" + idFac).show();
                $("#btnGuarP2" + idFac).show(); //btnGuar

                $("#pagoParcialMora" + idFac).hide();
                $("#btnGuarPMora" + idFac).hide(); //btnGuar
                $("#btnGuarP" + idFac).hide(); //btnGuar
                $("#btnGuar" + idFac).hide(); //btnGuar

            } else {
                $("#pagoParcialMora" + idFac).show();
                $("#btnGuarPMora" + idFac).show(); //btnGuar

                $("#datePick" + idFac).show();
                $("#btnGuar" + idFac).hide(); //btnGuar
                $("#btnGuarP2" + idFac).hide(); //btnGuar
                $("#pagoParcial" + idFac).hide();
                $("#btnGuarP" + idFac).hide(); //btnGuar
            }
        } else {

            var url = "?aid=" + idFac + "&val=" + optVal + "&edo=" + edo + "&oldedoval=" + currentSelectedValue;
           
             
            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                    console.log('data', data);
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    // location.reload();
                    console.log('jqXHR', jqXHR);
                    console.log('textStatus', textStatus);
                    console.log('errorThrown', errorThrown);
                }
            });

            
        }
        
    }

    function SaveValuePay(idFac, optVal, optValPay, edo) {

        if (optValPay == "" && (edo == "3" || optVal == "33")) {
            alert("El campo fecha no debe estar vacío");
            return false;
        }
        else if (optValPay == "" && (edo == "4" || edo == "11")) {
            alert("El monto no debe estar vacío");
            return false;
        }
        else {
            var fecha = $("#datePick" + idFac).val();
            var url = "?aid=" + idFac + "&val=" + optVal + "&val2=" + optValPay.replace(/\./g,'') + "&edo=" + edo + "&oldedoval=" + currentSelectedValue + "&fecha="+fecha;

            console.log("aid: ", idFac);
            console.log("val: ", optVal);
            console.log("val2: ", optValPay.replace(/\./g, ''));
            console.log("edo: ", edo);
            console.log("oldedoval: ", currentSelectedValue);
            console.log("fecha: ", fecha);

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    }

    function SaveValueObser(idFac, flg, obser) {
        var url = "?aid=" + idFac + "&flg=" + flg + "&obser=" + $.trim(obser);

        var success = function (data) {
            var html = [];
            data = $.parseJSON(data);
            $.each(data, function (index, d) {
                location.reload();
            });
        };

        $.ajax({
            type: 'GET',
            url: url,
            data: { todo: "jsonp" },
            dataType: "jsonp",
            crossDomain: true,
            cache: false,
            success: success,
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function SaveValueParcial(idFac, idEstado, Pago, edo, fecha) {

        if (idEstado == "" && (edo == "4" || edo == "11" || edo == "14")) {
            alert("El monto no debe estar vacío");
            return false;
        }
        else {

            var url = "?aid=" + idFac + "&val=" + idEstado + "&val2=" + Pago.replace(/\./g,'') + "&edo=" + edo + "&oldedoval=" + currentSelectedValue + "&fecha="+fecha;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    }

    $(document).ready(function () {

        $("input").on("click", function () {
            $('input[type=checkbox]').each(function () {
                if ($(this).is(':checked')) {

                    switch ($(this).attr("title")) {
                        case "PRENDA_DESPLAZAMIENTO":
                            $("#" + $(this).attr("title")).show();
                            $("#myModalSD").modal({ backdrop: "static" });
                            break;
                        case "CANCELACION_PRENDA":
                            $("#" + $(this).attr("title")).show();
                            $("#myModalCP").modal({ backdrop: "static" });
                            break;
                        case "MUTUO_HIPOTECARIO":
                        $("#" + $(this).attr("title")).show();
                        $("#myModalMH").modal({ backdrop: "static" });
                        break;
                        case "MANDATO_IRREVOCABLE":
                            $("#" + $(this).attr("title")).show();
                            $("#myModalMI").modal({ backdrop: "static" });
                        break;
                        /*default:
                        $("#myModalSD").modal("hide")*/ 
                    }

                    // $("#" + $(this).attr("title")).show();

                }
                else {

                    switch ($(this).attr("title")) {
                        case "PRENDA_DESPLAZAMIENTO":
                            $("#myModalSD").modal("hide");
                            $("#" + $(this).attr("title")).hide();
                            break;
                        case "CANCELACION_PRENDA":
                            $("#myModalCP").modal("hide");
                            $("#" + $(this).attr("title")).hide();
                            break;
                        case "MUTUO_HIPOTECARIO":
                            $("#myModalMH").modal("hide")
                            $("#" + $(this).attr("title")).hide();
                        break;
                        case "MANDATO_IRREVOCABLE":
                            $("#myModalMI").modal("hide")
                            $("#" + $(this).attr("title")).hide();
                        break;
                        /*default:
                        $("#myModalSD").modal("hide")*/ 
                    }
                    //$("#" + $(this).attr("title")).hide();
                    //.modal("hide")
                }
            });
        });
    });
   

</script>
<script type="text/javascript">
    $(function () {
        $('#example2').DataTable({
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
        Préstamo
        <small>Tabla Amortización</small>
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
                        <h3 class="box-title">Prestamo</h3>
                    </div>
                    <br />
                    <div class="row">
                        <table id="example3" class="table table-bordered table-hover">
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
                      <h3 class="box-title">Datos Préstamo</h3>
                    </div>
                    <div class="row">
                        <uc:datosPrestamos runat="server" ID="pres" />
                        <% if (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Convert.ToInt32(Session["NumCuotasEval"]) == 0))
                            { %>
                        <div class="col-xs-6">

                        <br />
                            <asp:Button runat="server" ID="btnModificarFac" ClientIDMode="Static"
                                CssClass="btn btn-info pull-right" Text="Guardar Prestamo" 
                                onclick="btnModificarFac_Click" />
                        </div>
                        <div class="col-xs-6">
                        <br />
                        <asp:Button runat="server" ID="btnPrestamoReorganizado" ClientIDMode="Static"
                                CssClass="btn btn-success" Text="Reorganizar" 
                                onclick="btnPrestamoReorganizado_Click" />
                        <asp:Button runat="server" ID="btnPrestamoEspecial" ClientIDMode="Static"
                                CssClass="btn btn-warning  pull-right" Text="Ir a Prestamo Especial>>>" 
                                onclick="btnPrestamoEspecial_Click" />
                        </div>
                        <%} %>
                    </div>
           <br />
           <div class="box-body table-responsive no-padding">
              <table class="table table-striped table-bordered nowrap">
                <tr>
                  <th align="center" style="width:5%;">N°</th>
                  <th style="width:15%;">Saldo Inicial</th>
                  <th align="center">Cuotas</th>
                  <th align="center">Intereses</th>
                  <th align="center">Capital</th>
                  <th align="center">Saldo Final</th>
                  <th align="center">Estado</th>
                  <th align="center">Vencimiento</th>
                  <th align="center">Mora</th>
                  <th align="center">Fecha Pago</th>
                  <th align="center">Observación</th>
                </tr>
                <asp:Repeater runat="server" ID="rptPrestamos" >
                <ItemTemplate>
                    <tr <%# (Eval("IdEdoPres").ToString()=="2")?"style='background:red;color:white;'":(Eval("IdEdoPres").ToString()=="3")?"style='background:green;color:white;'":(Eval("IdEdoPres").ToString()=="4")?"style='background:orange;color:black;'":(Eval("IdEdoPres").ToString()=="5")?"style='background:violet;color:white;'":(Eval("IdEdoPres").ToString()=="6")?"style='background:blue;color:white;'":(Eval("IdEdoPres").ToString()=="7")?"style='background:gray;color:black;'":(Eval("IdEdoPres").ToString()=="8")?"style='background:#caef00;color:black;'" : (Eval("IdEdoPres").ToString()=="9")?"style='background:#9cb9f9;color:black;'":(Eval("IdEdoPres").ToString()=="11")?"style='background:navajowhite;color:black;'":(Eval("IdEdoPres").ToString()=="12")?"style='background:coral;color:black;'":"style='background:white;color:black;'"%>>
                      <td align="center"><%# Eval("NumCuota").ToString().Replace(",00","").Replace(",",".").Replace(".10",".1").Replace(".20",".2").Replace(".30",".3").Replace(".40",".4")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoInicial").ToString()).ToString("N")%><%# (Eval("NumCuota").ToString().Contains(",10"))?"<div style='position: relative;top: 8rem;'><img src='../Styles/img/notaFinansu.png' style='position: absolute;' /></div>":""%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Cuota").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Intereses").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Capital").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoFinal").ToString()).ToString("N")%></td>
                      <td style='color:black;'>
                          <select id='ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='width:163px;'>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() == "1"))? 
                                      "<option value='1' selected='selected' style='color:black;'>SIN VENCER</option>":""%>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "4"
                                        && Eval("IdEdoPres").ToString() != "5" && Eval("IdEdoPres").ToString() != "6" && Eval("IdEdoPres").ToString() != "7"
                                        && Eval("IdEdoPres").ToString() != "8" && Eval("IdEdoPres").ToString() != "9" && Eval("IdEdoPres").ToString() != "11"
                                        && Eval("IdEdoPres").ToString() != "12"))? (Eval("IdEdoPres").ToString()=="2")?
                                        "<option value='2' selected='selected' style='background:red;color:white;'>VENCIDA</option>":"<option value='2' style='background:red;color:white;'>VENCIDA</option>" : ""%>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "6" && Eval("IdEdoPres").ToString() != "7"))?
                                      (Eval("IdEdoPres").ToString()=="3")?
                                      "<option value='3' selected='selected' style='background:green;color:white;'>PAGADA</option>":"<option value='3' style='background:green;color:white;'>PAGADA</option>":""%>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                        && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString() == "4") ? 
                                      "<option value='4' selected='selected' style='background:orange;color:white;'>PAGO PARCIAL</option>" : "<option value='4' style='background:orange;color:white;'>PAGO PARCIAL</option>" : ""%>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                          && Eval("IdEdoPres").ToString() != "7"))?(Eval("IdEdoPres").ToString()=="5")?
                                      "<option value='5' selected='selected' style='background:violet;color:white;'>PAGADO SIN INTERESES DE MORA</option>" : "<option value='5' style='background:violet;color:white;'>PAGADO SIN INTERESES DE MORA</option>" : "" %>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString() == "6") ?
                                      "<option value='6' selected='selected' style='background:blue;color:white;'>EN JUICIO</option>" : "<option value='6' style='background:blue;color:white;'>EN JUICIO</option>" : "" %>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6")) ? (Eval("IdEdoPres").ToString()=="7") ? 
                                      "<option value='7' selected='selected' style='background:gray;color:black;'>INCOBRABLE</option>" : "<option value='7' style='background:gray;color:black;'>INCOBRABLE</option>" : "" %>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                            && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString()=="8") ? 
                                 "<option value='11' selected='selected' style='background:#caef00;color:black;'>PAGO PARCIAL MORA</option>" : "<option value='11' style='background:#caef00;color:black;'>PAGO PARCIAL MORA</option>" : "" %>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                            && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString()=="9") ? 
                                            "<option value='12' selected='selected' style='background:#9cb9f9;color:black;'>PAGO SÓLO INTERESES</option>" : "<option value='12'  style='background:#9cb9f9;color:black;'>PAGO SÓLO INTERESES</option>" : "" %>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                            && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString()=="11") ? 
                                            "<option value='13' selected='selected' style='background:navajowhite;color:black;'>PAGO SÓLO CAPITAL</option>" : "<option value='13' style='background:navajowhite;color:black;'>PAGO SÓLO CAPITAL</option>": ""%>
                              <%# (Session["rol"] != null && Session["rol"].ToString() == "admin" || (Eval("IdEdoPres").ToString() != "3" && Eval("IdEdoPres").ToString() != "6"
                                            && Eval("IdEdoPres").ToString() != "7")) ? (Eval("IdEdoPres").ToString()=="12") ? 
                                            "<option value='14' selected='selected' style='background:coral;color:black;'>PAGO CAPITAL PARCIAL</option>" : "<option value='14' style='background:coral;color:black;'>PAGO CAPITAL PARCIAL</option>" : ""%>
                          </select>
                          <span style="font-weight:bold;" data-toggle="modal" data-target="#exampleModal<%# DataBinder.Eval(Container.DataItem, "ID") %>" title="<%#  (Eval("IdEdoPres").ToString() == "8") ? "Pago parcial de mora" : "Pago parcial" %>">
                              <%#  (Eval("IdEdoPres").ToString() == "4" || decimal.Parse(Eval("Parcial").ToString())>0) ?  "$" + decimal.Parse(Eval("Parcial").ToString()).ToString("N") : (Eval("IdEdoPres").ToString() == "8") ?  "$" + decimal.Parse(Eval("PagoParcialMora").ToString()).ToString("N") :  "" %>
                          </span>
                          <span data-toggle="modal" data-target="#exampleModal<%# DataBinder.Eval(Container.DataItem, "ID") %>" title="<%#  (Eval("IdEdoPres").ToString() == "8") ? "Pago parcial de mora" : "Pago parcial" %>"><%#  (Eval("IdEdoPres").ToString() == "4" || decimal.Parse(Eval("Parcial").ToString())>0) ? " - VER DETALLE" : "" %></span>
                          <br />
                          <span style="font-weight:bold;" data-toggle="modal" data-target="#exampleModal2<%# DataBinder.Eval(Container.DataItem, "ID") %>" title="<%#  (Eval("IdEdoPres").ToString() == "8") ? "Pago parcial de mora" : "Pago parcial" %>">
                              <%#  (Eval("IdEdoPres").ToString() == "12" || decimal.Parse(Eval("CapitalParcial").ToString())>0) ?  "$" + decimal.Parse(Eval("CapitalParcial").ToString()).ToString("N") : (Eval("IdEdoPres").ToString() == "8") ?  "$" + decimal.Parse(Eval("PagoParcialMora").ToString()).ToString("N") :  "" %>
                          </span>
                          <span data-toggle="modal" data-target="#exampleModal2<%# DataBinder.Eval(Container.DataItem, "ID") %>" title="<%#  (Eval("IdEdoPres").ToString() == "8") ? "Pago parcial de mora" : "Pago capital parcial" %>"><%#  (Eval("IdEdoPres").ToString() == "12" || decimal.Parse(Eval("CapitalParcial").ToString())>0) ? " - VER DETALLE CAPITAL" : "" %></span>
                          <br />
                          <span id='mas<%# DataBinder.Eval(Container.DataItem, "ID") %>'><%#  (Eval("IdEdoPres").ToString() == "3") ?  "" : "Agregar Pago Parcial"%></span>
                          <br />
                          <span id='mas2<%# DataBinder.Eval(Container.DataItem, "ID") %>'><%#  (Eval("IdEdoPres").ToString() == "3") ?  "" : "Agregar Pago Capital Parcial"%></span>
                          <input type='text' id='datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;'/>
                          <input onkeypress='return soloNum(event, this.id)' onblur='formatomiles(this.id);' type='text' id='pagoParcial<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;'/>
                          <br>
                          <input type='button' id='btnGuarP<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;'/>
                          <input type='button' id='btnGuarP2<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;'/>
                          <input onkeypress='return soloNum(event, this.id)' onblur='formatomiles(this.id);' type='text' id='pagoParcialMora<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;'/>
                          <br>
                          <input type='button' id='btnGuarPMora<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;'/>
                          <br>
                          <input type='button' id='btnGuar<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;'/>
                            <script type='text/javascript'>
                                $(document).ready(function () {
                                    $(function () {
                                        $('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
                                        $('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask(); 
                                        $('#datePickV<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
                                        $('#datePickV<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask(); 
                                        $('#datePick12<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
                                        $('#datePick12<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask(); 
                                    });
                                    $('#btnGuarP<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValueParcial(<%# Eval("ID").ToString() %>,4,$('#pagoParcial<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(),true,$('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').val());
                                    });
                                    $('#btnGuarP2<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValueParcial(<%# Eval("ID").ToString() %>,14,$('#pagoParcial<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(),true,$('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').val());
                                    });
                                    $('#btnGuarPMora<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {

                                        if(Number($('#pagoParcialMora<%# DataBinder.Eval(Container.DataItem, "ID") %>').val().replace(/\./g, "").replace(",", ".")) == 0)
                                            alert("Debe ingresar un monto a pagar.");
                                        else if (parseFloat($('#pagoParcialMora<%# DataBinder.Eval(Container.DataItem, "ID") %>').val().replace(/\./g, "").replace(",", ".")) > parseFloat(<%# DataBinder.Eval(Container.DataItem, "Mora") %>))
                                            alert("El monto no puede ser mayor a la mora adeudada para esta cuota.");
                                        else
                                            SaveValuePay(<%# Eval("ID").ToString() %>, 11, $('#pagoParcialMora<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true);

                                    });
                                    $('#btnGuar<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValuePay(<%# DataBinder.Eval(Container.DataItem, "ID") %>,3,$('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(),true);
                                    });
                                    $('#btnGuarV<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValuePay(<%# DataBinder.Eval(Container.DataItem, "ID") %>, 33, $('#datePickV<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true);
                                    });
                                    $('#btnGuar12<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValuePay(<%# DataBinder.Eval(Container.DataItem, "ID") %>, 34, $('#datePick12<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true);
                                    });

                                    $('#btnSaveObser<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValueObser(<%# DataBinder.Eval(Container.DataItem, "ID") %>,'T',$('#txtObser<%# DataBinder.Eval(Container.DataItem, "ID") %>').val());
                                    });
                                    $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').focus(function (e) {
                                        currentSelectedValue = this.value;
                                    });
                                    $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').change(function (e) {

                                        console.log('change', e);
                                        console.log('currentSelectedValue', currentSelectedValue);
                                        console.log('ddl ID', <%# DataBinder.Eval(Container.DataItem, "ID") %>);

                                if ($('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').val() == "12" && Number(<%# DataBinder.Eval(Container.DataItem, "Intereses") %>) == 0) {
                                            alert("No se puede seleccionar este estado porque no hay intereses pendientes por pago en esta cuota.");
                                            $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(currentSelectedValue);
                                        }
                                        else {
                                            SaveValue(<%# DataBinder.Eval(Container.DataItem, "ID") %>, $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true, currentSelectedValue);
                                        }
                                    });
                                    $('#mas<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                         currentSelectedValue = 4;
                                            SaveValue(<%# DataBinder.Eval(Container.DataItem, "ID") %>,4, true, 4);
                                    });

                                    $('#mas2<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                         currentSelectedValue = 14;
                                            SaveValue(<%# DataBinder.Eval(Container.DataItem, "ID") %>,14, true, 14);
                                    });

                                    $('#masV<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        currentSelectedValue = 33;
                                        $('#datePickV<%# DataBinder.Eval(Container.DataItem, "ID") %>').show();
                                        $('#btnGuarV<%# DataBinder.Eval(Container.DataItem, "ID") %>').show();
                                    });

                                    $('#mas12<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        currentSelectedValue = 34;
                                        $('#datePick12<%# DataBinder.Eval(Container.DataItem, "ID") %>').show();
                                        $('#btnGuar12<%# DataBinder.Eval(Container.DataItem, "ID") %>').show();
                                    });



                                });
                            </script>

                          <!-- Modal -->
                            <div class="modal fade" id='exampleModal<%# DataBinder.Eval(Container.DataItem, "ID") %>' tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                              <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                  <div class="modal-header">
                                    <h5 class="modal-title" id="exampleModalLabel">Pagos Parciales</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      <span aria-hidden="true">&times;</span>
                                    </button>
                                  </div>
                                  <div class="modal-body">
                                    <table id="example4" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th align="center">Monto Pago Parcial</th>
                                                <th align="center">Fecha de Pago Parcial</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                              <%#GetPagosParciales(Eval("ID").ToString())%>
                                        </tbody>
                                    </table>
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                                  </div>
                                </div>
                              </div>
                            </div>
                          <!-- Modal -->
                            <div class="modal fade" id='exampleModal2<%# DataBinder.Eval(Container.DataItem, "ID") %>' tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                              <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                  <div class="modal-header">
                                    <h5 class="modal-title" id="exampleModalLabel">Pagos Capitales Parciales</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      <span aria-hidden="true">&times;</span>
                                    </button>
                                  </div>
                                  <div class="modal-body">
                                    <table id="example4" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th align="center">Monto Pago Parcial</th>
                                                <th align="center">Fecha de Pago Parcial</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                              <%#GetPagosCapitalesParciales(Eval("ID").ToString())%>
                                        </tbody>
                                    </table>
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                                  </div>
                                </div>
                              </div>
                            </div>
                      </td>
                      <td>
                          <span id="masV<%# DataBinder.Eval(Container.DataItem, "ID") %>"><%#(Eval("Vencimiento") == null) ? "NA" : Convert.ToDateTime(Eval("Vencimiento")).ToShortDateString()%></span>
                          <input type='text' id='datePickV<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;color:black;'/>
                          <input type='button' id='btnGuarV<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar Vencimiento' style='display:none;color:black;'/>
                      </td>
                      <td><%#  (Eval("IdEdoPres").ToString()!="8")? "$" + decimal.Parse(Eval("Mora").ToString()).ToString("N"): "$" + decimal.Parse(Eval("MontoRestanteMora").ToString()).ToString("N")%></td>
                      <td>
                          <span id="mas12<%# DataBinder.Eval(Container.DataItem, "ID") %>"><%#(Eval("FechaPago") == null) ? "NA" : Convert.ToDateTime(Eval("FechaPago")).ToShortDateString()%></span>
                          <input type='text' id='datePick12<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;color:black;'/>
                          <input type='button' id='btnGuar12<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;color:black;'/>

                      </td>
                      <td style="color:Black;">
                      <textarea id='txtObser<%# DataBinder.Eval(Container.DataItem, "ID") %>' rows="5" cols="20"><%# (Eval("Observacion") == null)?"":Eval("Observacion").ToString().Trim()%></textarea>
                      <input type='button' id='btnSaveObser<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar'/>
                      </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
              </table>
            </div>
            <br />
                    <div class="col-md-6" style="width: 35% !important;">
                    <%--<uc:toPrint runat="server" ID="print" />--%>
                    <asp:Repeater runat="server" ID="rptCrearDocs" >

                        <ItemTemplate>
                            <div class="form-group">
                              <label for="inputPassword3" class="col-sm-2 control-label" style="width: 85% !important;"><%# Eval("Nombre")%></label>
                              <div class="col-sm-10" style="width: 15% !important;">
                                <input type="checkbox" runat="server" id="chkDoc" title='<%# Eval("Template")%>' />
                                <asp:HiddenField runat="server" ID="hdItem"  Value='<%# Eval("Template")%>' />
                              </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button runat="server" ID="btnCrearDocs" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                              Text="Generar Contratos" onclick="btnCrearDocs_Click" Enabled="true" />
                    </div>
                    <asp:ScriptManager ID="ScriptManager" runat="server" />
                    <div class="col-md-12" id="PRENDA_DESPLAZAMIENTO" style="display:none">
                        <uc:modalSD runat="server" ID="modalSinDespl" />
                    </div>
                    <div class="col-md-12" id="CANCELACION_PRENDA" style="display:none">
                        <uc:modalCP runat="server" ID="ModalCPren" />
                    </div>

                    <div class="col-md-12" id="MUTUO_HIPOTECARIO" style="display:none">
                        <uc:modalMH runat="server" ID="ModalMh" />
                    </div>
                    <div class="col-md-12" id="MANDATO_IRREVOCABLE" style="display:none">
                        <uc:modalMI runat="server" ID="ModalMIrre" />
                    </div>

                      <div class="box-footer">
                      <iframe runat="server" id="ifr1" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr2" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr3" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr4" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr5" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr6" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr7" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr8" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr9" frameborder="0" style="height:10px;"></iframe>
                      </div>
                </div>
            </div>
        </div>
</div>
</form>

</asp:Content>
