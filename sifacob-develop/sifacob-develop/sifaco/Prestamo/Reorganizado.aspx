<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reorganizado.aspx.cs" Inherits="sifaco.Prestamo.Reorganizado" EnableViewStateMac="false"%>
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

        if (optVal == 3 || optVal == 4 || optVal == 11) {
            if (optVal == 3) {
                $("#datePick" + idFac).show();
                $("#btnGuar" + idFac).show(); //btnGuar

                $("#pagoParcial" + idFac).hide();
                $("#btnGuarP" + idFac).hide(); //btnGuar

                $("#pagoParcialMora" + idFac).hide();
                $("#btnGuarPMora" + idFac).hide(); //btnGuar

            } else if (optVal == 4) {
                $("#datePick" + idFac).show();
                $("#pagoParcial" + idFac).show();
                $("#btnGuarP" + idFac).show(); //btnGuar

                $("#pagoParcialMora" + idFac).hide();
                $("#btnGuarPMora" + idFac).hide(); //btnGuar

                $("#btnGuar" + idFac).hide(); //btnGuar

            } else {
                $("#pagoParcialMora" + idFac).show();
                $("#btnGuarPMora" + idFac).show(); //btnGuar

                $("#datePick" + idFac).hide();
                $("#btnGuar" + idFac).hide(); //btnGuar

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
        
    }

    function SaveValuePay(idFac, optVal, optValPay, edo) {

        if (optValPay == "" && edo == "3") {
            alert("El campo fecha no debe estar vacío");
            return false;
        }
        else if (optValPay == "" && (edo == "4" || edo == "11")) {
            alert("El monto no debe estar vacío");
            return false;
        }
        else {

            var url = "?aid=" + idFac + "&val=" + optVal + "&val2=" + optValPay.replace(/\./g,'') + "&edo=" + edo + "&oldedoval=" + currentSelectedValue;

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

        if (idEstado == "" && (edo == "4" || edo == "11")) {
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
                        <h3 class="box-title">Prestamo Reorganizado</h3>
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
                        <div class="col-xs-6">
                        <%--<br />
                            <asp:Button runat="server" ID="btnModificarFac" ClientIDMode="Static"
                                CssClass="btn btn-info pull-right" Text="Guardar Prestamo" 
                                onclick="btnModificarFac_Click" />--%>
                        </div>
                        <div class="col-xs-6">
                        <br />
                        <asp:Button runat="server" ID="btnPrestamoReorganizado" ClientIDMode="Static"
                                CssClass="btn btn-success" Text="Ver Prestamo Reorganizado" 
                                onclick="btnVerPrestamoReorganizado_Click" />
                        <%--<asp:Button runat="server" ID="btnPrestamoEspecial" ClientIDMode="Static"
                                CssClass="btn btn-warning  pull-right" Text="Ir a Prestamo Especial>>>" 
                                onclick="btnPrestamoEspecial_Click" />--%>
                        </div>
                    </div>
           <br />
           <div class="box-body table-responsive no-padding">
              <table class="table table-hover">
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
                    <tr style="background:#cccccc;color:black;">
                      <td align="center"><%# Eval("NumCuota")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoInicial").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Cuota").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Intereses").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Capital").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoFinal").ToString()).ToString("N")%></td>
                      <td style='color:black;'>
                          <select id='ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='width:163px;' disabled="disabled">
                            <option value='10'selected='selected' style='color:black;'>REORGANIZADO</option>
                          </select>
                      </td>
                      <td>
                          <%#(Eval("Vencimiento") == null) ? "NA" : Convert.ToDateTime(Eval("Vencimiento")).ToShortDateString()%>
                      </td>
                      <td><%#  (Eval("IdEdoPres").ToString()!="8")? "$" + decimal.Parse(Eval("Mora").ToString()).ToString("N"): "$" + decimal.Parse(Eval("MontoRestanteMora").ToString()).ToString("N")%></td>
                      <td>
                          <%# (Eval("FechaPago")==null)?"NA":Convert.ToDateTime(Eval("FechaPago")).ToShortDateString()%>
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
            </div>
        </div>
</div>
</form>

</asp:Content>
