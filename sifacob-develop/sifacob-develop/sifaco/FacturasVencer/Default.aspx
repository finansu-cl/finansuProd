<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.FacturasVencer.Default" %>
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
        <small>Facturas vencidas o próximas a vencer</small>
      </h1>--%>

</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
    <div class="panel box box-primary">
        <div class="box-body">
            <div class="box-header with-border">
                <h3 class="box-title">Facturas a vencer y vencidas</h3>
            </div>
            <br />
            <div class="box-body no-padding">
                <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                    <thead>
                        <tr>
                            <th align="center">N°</th>
                            <th>Cliente</th>
                            <th align="center">Plazo</th>
                            <th align="center">Tasa</th>
                            <th align="center">Monto</th>
                            <th align="center">Operación</th>
                            <th align="center">Días antes del vencimiento</th>
                            <th align="center">Vencimiento</th>
                            <th align="center">Estado</th>
                            <th style="text-align:center">Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptClienteFactura">
                            <ItemTemplate>
                                <tr <%# (Convert.ToInt32(Eval("VenceEn"))>0 && Convert.ToInt32(Eval("VenceEn"))<11)?"style='background:#e08e0b;color:white;'":(Convert.ToInt32(Eval("VenceEn"))<=0)?EditarEdoNotiFactura(Eval("ID").ToString(), "2", Eval("IdEdoFactura").ToString())+"style='background:red;color:white;'":""%>>
                                      <td><%# Eval("NumFactura")%></td>
                                      <td><%# Eval("Nombre")%></td>
                                      <td><%# Eval("Plazo")%></td>
                                      <td><%# Eval("Tasa")%></td>
                                      <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                                      <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Operacion"))%></td>
                                      <td><%# Eval("VenceEn")%></td>
                                      <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Vencimiento"))%></td>
                                      <td><%# Eval("EstadoFactura")%></td>
                                      <td><a id="vf<%#Eval("ID").ToString()%>" href="../CobranzaFacturas/DetalleFactura.aspx?cid=<%#Eval("IdCliente").ToString()%>">Ver Facturas</a></td>
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
