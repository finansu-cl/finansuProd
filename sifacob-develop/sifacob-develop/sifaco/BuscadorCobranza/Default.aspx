<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.BuscadorCobranza.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/Chart.js"></script>
<asp:Literal runat="server" ID="ltrHead">
<style type="text/css">
.visto
{
    display:none;
    }
</style>
</asp:Literal>
<link href="../Scripts/datatables/extensions/buttons.dataTables.min.css" rel="stylesheet" />
<script src="../Scripts/datatables/extensions/dataTables.buttons.min.js"></script>
<script src="../Scripts/datatables/extensions/JsZip/jszip.min.js"></script>
<script src="../Scripts/datatables/extensions/buttons.html5.min.js"></script>
<script src="../Scripts/datatables/extensions/buttons.print.min.js"></script>

<script type="text/javascript">
    $(function () {
        $("#txtFechaD").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaD").inputmask();
        $("#txtFechaH").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaH").inputmask();
    });

    $(document).ready(function () {
        $("#printBC").click(function () {
            setTimeout(function () { window.print(); }, 1000);
        });

        var buttonCommon = {
            exportOptions: {
                format: {
                    body: function (data, row, column, node) {
                        // Strip $ from salary column to make it numeric
                        return column === 5 || column === 6 || column === 7 || column === 8 ?
                            data.replace(/[$.]/g, '') :
                            data;
                    }
                }
            }
        };

        var table = $('#example3').DataTable({
           dom: 'Bfrtip',
            responsive: true,
            "searching": false,
            "ordering": true,
            buttons: [
                $.extend(true, {}, buttonCommon, {
                    extend: 'excelHtml5'
                }),
                'print'
                ]
        });

        new $.fn.dataTable.FixedHeader(table);

    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Generar
        <small>Reportes</small>
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
            <div class="box-header with-border">
                        <h3 class="box-title">Generador de reportes de factoring</h3>
                    </div>
                    <br />
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
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
                        <div class="col-xs-3" style="z-index:1000;">
                            <b>Buscar fecha en</b>
                            <asp:DropDownList runat="server" ID="ddlEdoFactura" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                                    <asp:ListItem Value="2">Vencidas</asp:ListItem>
                                    <asp:ListItem Value="3">Pagadas</asp:ListItem>
                                    <asp:ListItem Value="6">Pagadas parcial</asp:ListItem>
                                    <asp:ListItem Value="7">Pagadas sin interéses</asp:ListItem>
                                    <asp:ListItem Value="4">No pagadas</asp:ListItem>
                                    <asp:ListItem Value="8">En Juicio</asp:ListItem>
                                    <asp:ListItem Value="9">Incobrable</asp:ListItem>
                                    <asp:ListItem Value="20">Emisión</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3" style="z-index:1000;">
                            <b>Cliente</b>
                            <asp:DropDownList runat="server" ID="ddlCliente" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3" style="z-index:1000;">
                            <b>Deudor</b>
                            <asp:TextBox runat="server" ID="txtDeudor" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-3" style="float:left !important;z-index:1000;">
                            <br />
                            <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                                    CssClass="btn btn-info pull-right" Text="Buscar" 
                                    onclick="btnBuscar_Click"  />
                        </div>
                        <div class="col-xs-3">
                            <b>Monto Total</b>
                            <asp:TextBox runat="server" ID="txtMT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Utilidad Total</b>
                            <asp:TextBox runat="server" ID="txtUT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Mora Total</b>
                            <asp:TextBox runat="server" ID="txtMoT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Cesión Total</b>
                            <asp:TextBox runat="server" ID="txtGT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b id="bTagF">Total de Facturas</b>
                            <asp:TextBox runat="server" ID="txtFT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>

                        <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th align="center" style="width:5% !important;">N°</th>
                                    <th>Cliente</th>
                                    <th>Deudor</th>
                                    <th align="center" style="width:5% !important;">Plazo</th>
                                    <th align="center" style="width:5% !important;">Tasa</th>
                                    <th align="center">Monto</th>
                                    <th align="center">Utilidad</th>
                                    <th align="center">Cesión</th>
                                    <th align="center">Mora</th>
                                    <th align="center" style="width:10% !important;">Emisión</th>
                                    <th align="center" style="width:10% !important;">Operación</th>
                                    <th align="center" style="width:10% !important;">Vencimiento</th>
                                    <th align="center" style="width:10% !important;">Cobranza&nbsp;&nbsp;</th>
                                    <th align="center" style="width:5% !important;">Atraso</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptClienteFactura">
                                    <ItemTemplate>
                                        <tr>
                                                <td><%# Eval("NumFactura")%></td>
                                                <td><%# Eval("Nombre")%></td>
                                                <td><%# Eval("Deudor")%></td>
                                                <td><%# Eval("Plazo")%></td>
                                                <td><%# Eval("Tasa")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N").Replace(",00","")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Utilidad").ToString()).ToString("N").Replace(",00","")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoGirable").ToString()).ToString("N").Replace(",00","")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoMora").ToString()).ToString("N").Replace(",00","")%></td>
                                                <td><%# (Eval("Emision")==null)?"N/A":string.Format("{0:dd-MM-yyyy}",Eval("Emision"))%></td>
                                                <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Operacion"))%></td>
                                                <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Vencimiento"))%></td>
                                                <td><%# (Eval("Pago")==null)?"N/A":string.Format("{0:dd-MM-yyyy}",Eval("Pago"))%></td>
                                                <td><%# Eval("VenceEn")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
</asp:Content>
