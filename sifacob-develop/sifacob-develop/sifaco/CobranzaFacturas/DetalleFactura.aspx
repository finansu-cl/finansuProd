<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleFactura.aspx.cs" Inherits="sifaco.CobranzaFacturas.DetalleFactura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/validaciones.js" type="text/javascript"></script>
    <script type="text/javascript">

        function SaveValue(idFac, optVal, edo) {

            if (optVal == 3 || optVal == 4) {
                if (optVal == 3) {
                    $("#datePick" + idFac).show();
                    $("#btnGuar" + idFac).show(); //btnGuar
                } else {
                    $("#pagoParcial" + idFac).show();
                    $("#datePick3" + idFac).show();
                    $("#btnGuarP" + idFac).show(); //btnGuar
                }
            }
            else {
                var url = "DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&edo=" + edo;

                var success = function (data) {
                    var html = [];
                    data = $.parseJSON(data);
                    $.each(data, function (index, d) {
                        location.reload();
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

        function stripDots(n) {
            var z = n.replace(/\./g, "");
            return z;
        }
        function SaveDateNoti(idFac, optVal, optValPay, edo) {
            if (optValPay == "" && optVal == "3") {
                alert("El campo fecha no debe estar vacío");
                return false;
            }

        }

        function SaveValuePay(idFac, optVal, optValPay, edo, optValPay2) {

            if (optValPay == "" && optVal == "3") {
                alert("El campo fecha no debe estar vacío");
                return false;
            }
            else if ((optValPay == "" || optValPay2 == "") && optVal == "4") {
                alert("El campo MONTO y el campo FECHA no deben estar vacío");
                return false;
            }
            else if (parseFloat(stripDots($("#td" + idFac).html()).replace(/\,/g, ".")) < parseFloat(stripDots(optValPay).replace(/\,/g, "."))) {
                //stripDots(precioCes.val()).replace(/\,/g, ".")
                //alert(stripDots($("#td" + idFac).html()).replace(/\,/g, "."));
                var r = confirm("Advertencia: Usted esta ingresando un monto mayor al monto restante. ¿Desea Continuar?");
                if (r == true) {
                    var url = "DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + optValPay2;

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
                } else {
                    return false;
                }
            }
            else {
                if (optVal == "4")
                    var url = "DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + optValPay2;
                else
                    var url = "DetalleFactura.aspx?fid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + edo;

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
            var url = "DetalleFactura.aspx?fid=" + idFac + "&flg=" + flg + "&obser=" + $.trim(obser);

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

        $(function () {
            $("#txtfechad").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
            $("#txtfechad").inputmask();
            $("#txtfechah").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
            $("#txtfechah").inputmask();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
    <section class="content-header">
<%--      <h1>
        Cobranza
        <small>Seguimiento de Facturas por Cliente</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
        <div class="panel box box-primary">
            <div class="box-body">
                <div class="box-header with-border">
                    <h3 class="box-title">Seguimiento de facturas por cliente</h3>
                </div>
                <br />
                <div id="Div1" class="form-horizontal" runat="server">
                    <div class="box-body">
                        <div class="box-body no-padding">

                            <div class="col-xs-5">
                                <b>Fecha Desde</b>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtFechaD" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-5">
                                <b>Fecha Hasta</b>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtFechaH" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-2" style="float: left !important">
                                <br />
                                <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static"
                                    CssClass="btn btn-info pull-right" Text="Buscar"
                                    OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="box-body table-responsive no-padding">
                    <table id="example3" class="table table-striped table-bordered nowrap" style="width: 100%;">
                        <thead>
                            <tr>
                                <th style="text-align: center">Cliente</th>
                                <th style="text-align: center">RUT</th>
                                <th style="text-align: center">N° Operaciones</th>
                                <th>Total Factorizado</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptClienteFactura">
                                <ItemTemplate>
                                    <tr>
                                        <td style="width: 45% !important;"><%# Eval("Nombre")%></td>
                                        <td align="center"><%# Eval("Rut")%></td>
                                        <td align="center"><%# Eval("NumOperacion")%></td>
                                        <td><%#  "$" + decimal.Parse(Eval("MontoFactoring").ToString()).ToString("N")%></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" id="f<%#Eval("ID").ToString()%>">
                                            <div id="<%#Eval("ID").ToString()%>">
                                                <div class="box-body">
                                                    <table id="example<%#Eval("ID").ToString()%>" class="table table-bordered table-hover" style="background-color: #CCC !important;">
                                                        <thead>
                                                            <tr>
                                                                <th align="center">N°</th>
                                                                <th>Deudor</th>
                                                                <th align="center">Plazo</th>
                                                                <th align="center">Monto</th>
                                                                <th align="center">Pendiente</th>
                                                                <th align="center">Mora</th>
                                                                <th align="center">Reembolso</th>
                                                                <th align="center">Operación</th>
                                                                <th align="center">Vencimiento</th>
                                                                <th align="center">Estado</th>
                                                                <th align="center">Pago Parcial/Total</th>
                                                                <th align="center">Fecha de Emisión</th>
                                                                <th align="center">Observación</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <%#FacturasPorCliente(Eval("ID").ToString())%>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <input type="button" value="Volver" onclick="javascript:window.history.back();" /></td>
                                    </tr>
                                    <script type="text/javascript">
                                        $(function () {
                                            $('#example<%#Eval("ID").ToString()%>').DataTable({
                                                "paging": true,
                                                "lengthChange": false,
                                                "searching": true,
                                                "ordering": false,
                                                "info": false,
                                                "autoWidth": false
                                            });
                                        });
                                    </script>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
