<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.CobranzaFacturas.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        var table = $('#example3').DataTable({
            responsive: true,
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": false,
            "autoWidth": false
        });

        new $.fn.dataTable.FixedHeader(table);
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Cobranza
        <small>Seguimiento de Facturas por Cliente</small>
      </h1>--%>

</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
    <div class="panel box box-primary">
        <div class="box-body">
            <div class="box-header with-border">
                <h3 class="box-title">Facturas por clientes</h3>
            </div>
            <br />
            <div class="box-body no-padding">
                <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                    <thead>
                        <tr>
                            <th style="text-align:center">Cliente</th>
                            <th style="text-align:center">RUT</th>
                            <th style="text-align:center">N° Operaciones</th>
                            <th >Total Factorizado</th>
                            <th >Total Mora</th>
                            <th style="text-align:center">Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptClienteFactura">
                            <ItemTemplate>
                                <tr>
                                      <td style="width:40% !important;"><%# Eval("Nombre")%></td>
                                      <td align="center"><%# Eval("Rut")%></td>
                                      <td align="center"><%# Eval("NumOperacion")%></td>
                                      <td><%#  "$" + decimal.Parse(Eval("MontoFactoring").ToString()).ToString("N")%></td>
                                      <td><%#  "$" + decimal.Parse(Eval("MontoMora").ToString()).ToString("N")%></td>
                                      <td style="width:10% !important;"><a id="vf<%#Eval("ID").ToString()%>" href="DetalleFactura.aspx?cid=<%#Eval("ID").ToString()%>">Ver Facturas</a></td>
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
