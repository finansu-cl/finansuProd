<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.PrestamosVencer.Default" %>
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
      <%--<h1>
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
                        <h3 class="box-title">Cuotas a vencer</h3>
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
                            <th align="center">Cuota</th>
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
                                <tr <%# (Convert.ToInt32(Eval("VenceEn"))>0)?"style='background:orange;color:white;'":""%>>
                                      <td><%# Eval("NumCuota").ToString().Replace(",00","").Replace(",",".").Replace(".10",".1").Replace(".20",".2").Replace(".30",".3").Replace(".40",".4")%></td>
                                      <td><%# Eval("Nombre")%></td>
                                      <td><%# Eval("Plazo")%></td>
                                      <td><%# Eval("Tasa")%></td>
                                      <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                                      <td><%#  "$" + decimal.Parse(Eval("Cuota").ToString()).ToString("N")%></td>
                                      <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Fecha"))%></td>
                                      <td><%# Eval("VenceEn")%></td>
                                      <td><%# string.Format("{0:dd-MM-yyyy}",Eval("Vencimiento"))%></td>
                                      <td>SIN VENCER</td>
                                      <td><a id="vp<%#Eval("ID").ToString()%>" href="../CobranzaPrestamos/?cid=<%#Eval("IdCliente").ToString()%>&pid=<%#Eval("ID").ToString()%>">Ver Prestamo</a></td>
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
