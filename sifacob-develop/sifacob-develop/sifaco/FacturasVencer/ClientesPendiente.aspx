<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientesPendiente.aspx.cs" Inherits="sifaco.FacturasVencer.ClientesPendiente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#example3').DataTable({
            responsive: true,
            "paging": false,
            "lengthChange": false,
            "searching": true,
            "ordering": false,
            "info": false,
            "autoWidth": false
        });
        $("#txtFechaD").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaD").inputmask();
        $("#txtFechaH").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaH").inputmask();

        var idH1 = getUrlVars()["idH"];
        //var name2 = getUrlVars()["name2"];
        //var idH1 = $.url().param('idH');
        //alert(idH1);
        //$("#vc_" + idH1).focus();
        shoRow2(idH1);
    });

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

    function MiClick(id1, id2) {
        $(id1).click(function () {
            $(id2).toggle();
        });
    }
    function stripDots(n) {
        var z = n.replace(/\./g, "");
        return z;
    }
    function SaveValue(idFac, optVal, edo) {

        if (optVal != -1){
            var url = "../CobranzaFacturas/DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&edo=" + edo;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    var idH = $('#hdValToggle').val();
                    //location.href = "../FacturasVencer/ClientesPendiente.aspx?idH=" + idH;
                    /*alert(d.Sold);
                    alert(d.Month);*/
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
                    /*alert(errorThrown);
                    alert(textStatus);
                    alert(jqXHR);*/
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
        else if (optValPay == "" && edo == "4") {
            alert("El monto no debe estar vacío");
            return false;
        }
        else if (parseFloat(stripDots($("#td" + idFac).html()).replace(/\,/g, ".")) < parseFloat(stripDots(optValPay).replace(/\,/g, "."))) {
            var r = confirm("Advertencia: Usted esta ingresando un monto mayor al monto restante. ¿Desea Continuar?");
            if (r == true) {
                var url = "../CobranzaFacturas/DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + edo;

                var success = function (data) {
                    var html = [];
                    data = $.parseJSON(data);
                    $.each(data, function (index, d) {
                        var idH = $('#hdValToggle').val();
                        //location.href = "../FacturasVencer/ClientesPendiente.aspx?idH=" + idH;
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
            } else {
                return false;
            }
        }
        else {
            var url = "../CobranzaFacturas/DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + edo;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    var idH = $('#hdValToggle').val();
                    //location.href = "../FacturasVencer/ClientesPendiente.aspx?idH=" + idH;
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
        var url = "../CobranzaFacturas/DetalleFactura.aspx?fid=" + idFac + "&flg=" + flg + "&obser=" + $.trim(obser);

        var success = function (data) {
            var html = [];
            data = $.parseJSON(data);
            $.each(data, function (index, d) {
                var idH = $('#hdValToggle').val();
                //location.href = "../FacturasVencer/ClientesPendiente.aspx?idH=" + idH;
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

    function SaveValueDev(idFac, flg, dev) {
        var url = "../CobranzaFacturas/DetalleFactura.aspx?fid=" + idFac + "&flg=" + flg + "&obser=" + dev;

        var success = function (data) {
            var html = [];
            data = $.parseJSON(data);
            $.each(data, function (index, d) {
                var idH = $('#hdValToggle').val();
                //location.href = "../FacturasVencer/ClientesPendiente.aspx?idH=" + idH;
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


    function SaveValueHdd(idFac, valDdl,idHd) {
        var myInput = document.getElementById(idHd);
        if(valDdl == 0)
            myInput.value = valDdl+";"+idFac+"-";
        else
            myInput.value += valDdl + ";" + idFac + "-";
    }


    function shoRow(id) {
        
            $('#vc_' + id).click(function () {
                $('#dc_' + id).toggle();
                $('#hdValToggle').val(id);
            });
       
    }
    function shoRow2(id) {
        $(document).ready(function () {
            $('#dc_' + id).toggle();
            $('#hdValToggle').val(id);

            var nav = $('#vc_' + id);
            if (nav.length) {
                $(function () {
                    $(document).scrollTop($('#vc_' + id).offset().top);
                });
            }
        });
    }
</script>
    <style type="text/css">
        .dataTables_filter {
            text-align: right;
            margin-top: 0rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Cobranza
        <small>Facturas vencidas o próximas a vencer</small>
      </h1>--%>

</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">

    <div class="panel box box-primary">
        <div class="box-body">
            <div class="box-header with-border">
                <h3 class="box-title">Montos no financiados</h3>
            </div>
            <br />
            <div class="box-body no-padding">
                <div class="col-xs-3">
                    <b>Fecha Desde</b>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <asp:TextBox runat="server" ID="txtFechaD" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-3">
                    <b>Fecha Hasta</b>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <asp:TextBox runat="server" ID="txtFechaH" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                
                <div class="col-xs-3">
                    <b>Buscar por</b>
                    <asp:DropDownList runat="server" ID="ddlBusquedaPor" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Value="1">Cliente</asp:ListItem>
                            <asp:ListItem Value="2">Deudor</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-xs-3" style="float:left !important">
                    <br />
                    <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                            CssClass="btn btn-info" Text="Buscar" 
                            onclick="btnBuscar_Click"  />
                </div>
                <div class="col-xs-3">
                    <b>Monto Total</b>
                    <asp:TextBox runat="server" ID="txtMTP" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                </div>

                <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                    <thead>
                        <tr>
                            <th style="text-align:center"><asp:Literal runat="server" ID="ltrAfec" Text="Cliente"></asp:Literal></th>
                            <th style="text-align:center;width:10%;">Rut</th>
                            <th style="text-align:center;width:20%;">Monto Pendiente por <asp:Literal runat="server" ID="ltrAfec3" Text="Cliente"></asp:Literal></th>
                            <th style="text-align:center;width:10%;">Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptClienteFactura">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Nombre")%></td>
                                    <td style="text-align:center;"><%# Eval("Rut")%></td>
                                    <td style="text-align:center;"><%#GetTotalSaldoPendienteByClDe(Eval("IdCliente").ToString(), Eval("RutDeudor").ToString())%></td>
                                    <td>                                              
                                        <a id="vc_<%#Container.ItemIndex + 1%>">Ver Facturas</a>
                                       	<script type="text/javascript">
                                       	        shoRow(<%#Container.ItemIndex + 1%>);
                                        </script>
					                    <input type="hidden" id="hdValToggle" value="0" />
                                    </td>
                                </tr>
                                <tr id="dc_<%#Container.ItemIndex + 1%>" style="display:none">
                                <td align="center" style="display:none"><%# Eval("Nombre")%></td>
                                <td align="center" style="display:none"><%# Eval("Rut")%></td>
                                <td align="center" style="display:none"><%#GetTotalSaldoPendienteByClDe(Eval("IdCliente").ToString(), Eval("RutDeudor").ToString())%></td>
                                <td align="center" style="display:none">Pendiente</td>
                                    <td colspan="4">
                                            <table id="example4" class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th align="center">N°</th>
                                                    <th align="center">Cliente/Deudor</th>
                                                    <th align="center">Monto</th>
                                                    <th align="center">Monto Pendiente</th>
                                                    <th align="center">Operación</th>
                                                    <th align="center">Vencimiento</th>
                                                    <th align="center">Devuelto</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%#FactorasXAfect(Eval("IdCliente").ToString(), Eval("RutDeudor").ToString(), Container.ItemIndex + 1)%>
                                            </tbody>
                                        </table>
                                    </td>                                        
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

</form>
</asp:Content>
