<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Simulacion.aspx.cs" Inherits="sifaco.BuscadorCobranza.Simulacion" %>
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
        
        var table = $('#example3').DataTable({
            dom: 'Bfrtip',
            responsive: true,
            "searching": false,
            "ordering": true,
            buttons: [
                    'excel','print'
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
                        <h3 class="box-title">Generador de reportes de simulación</h3>
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
                            <b>Cliente</b>
                            <asp:DropDownList runat="server" ID="ddlCliente" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3" style="float:left !important;z-index:1000;">
                            <br />
                            <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                                    CssClass="btn btn-info pull-right" Text="Buscar" 
                                    onclick="btnBuscar_Click"  />
                        </div>
                        <div class="col-xs-3">
                            <b>Gastos de Operación</b>
                            <asp:TextBox runat="server" ID="txtGO" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Comisión</b>
                            <asp:TextBox runat="server" ID="txtCO" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Iva</b>
                            <asp:TextBox runat="server" ID="txtIV" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Monto Girable</b>
                            <asp:TextBox runat="server" ID="txtMG" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Monto Cesión</b>
                            <asp:TextBox runat="server" ID="txtMC" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>

                        <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th align="center" style="width:5% !important;">N°</th>
                                    <th>Cliente</th>
                                    <th>Rut</th>
                                    <th align="center" style="width:5% !important;">Estado</th>
                                    <th align="center" style="width:5% !important;">Tasa</th>
                                    <th align="center">Anticipo</th>
                                    <th align="center">Saldo Pendiente</th>
                                    <th align="center">Gastos Oper.</th>
                                    <th align="center">Comisión</th>
                                    <th align="center">Iva</th>
                                    <th align="center">Utilidad</th>
                                    <th align="center" style="width:10% !important;">Monto Total</th>
                                    <th align="center" style="width:10% !important;">Precio Cesion</th>
                                    <th align="center" style="width:10% !important;">Monto Girable</th>
                                    <th align="center" style="width:10% !important;">Monto Anticipo</th>
                                    <th align="center" style="width:10% !important;">Monto Pendiente</th>
                                    <th align="center" style="width:5% !important;">Fecha</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptClienteFactura">
                                    <ItemTemplate>
                                        <tr>
                                                <td><%# Eval("ID")%></td>
                                                <td><%# Eval("Nombre")%></td>
                                                <td><%# Eval("Rut")%></td>
                                                <td><%# Eval("Estado")%></td>
                                                <td><%# Eval("Tasa")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Anticipo").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("SaldoPendiente").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("GastosOper").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Comision").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Iva").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Utilidad").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoTotal").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("PrecioCes").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoGirable").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoAnticipo").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("MontoPendiente").ToString()).ToString("N")%></td>
                                                <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Fecha"))%></td>
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
