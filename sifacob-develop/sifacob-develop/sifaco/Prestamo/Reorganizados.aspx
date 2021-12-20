<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reorganizados.aspx.cs" Inherits="sifaco.Prestamos.Reorganizados" EnableViewStateMac="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        var table = $('#example2').DataTable({
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
        Buscador
        <small>Prestamos</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
        <div class="box box-primary">
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
                    <div class="box-header with-border">
                        <h3 class="box-title">Prestamos Reorganizados</h3>
                    </div>
                    <br />
                    <div class="box-body no-padding">
                        <asp:PlaceHolder ID="plhDeleteQuestion" runat="server" Visible="false">
                            <div class="alert alert-warning alert-dismissible">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <h4><i class="icon fa fa-warning"></i> Alerta!</h4>
                                    ¿Estas seguro de eliminar el prestamo del cliente?
                                    <asp:Button runat="server" ID="btnEliminar" Text="Aceptar" CssClass="btn btn-block btn-warning" onclick="btnEliminar_Click" />
                                    <asp:Literal ID="ltHidden" runat="server" Visible="false"></asp:Literal>
                                    <asp:Literal ID="ltHidden2" runat="server" Visible="false"></asp:Literal>
                            </div>
                        </asp:PlaceHolder>

                        <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Nombre(RUT)</th>
                                    <th>Monto</th>
                                    <th>Tasa</th>
                                    <th>Plazo</th>
                                    <th>Cuota</th>
                                    <th>Fecha Prestamo</th>
                                    <th>Editar                                         <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                                                                                           { %>
/ Eliminar<%} %></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptSimulaciones"  onitemcommand="rptSimulaciones_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("ID")%></td>
                                            <td><%# Eval("Nombre") + " (" + Eval("Rut") + ")"%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                                            <td><%# Eval("Tasa")+"%"%></td>
                                            <td><%# Eval("Plazo") + "Días"%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("Cuota").ToString()).ToString("N")%></td>
                                            <td><%#   String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Eval("Fecha")))%></td>
                                            <td>
                                                <asp:Button ID="btnEdit" runat="server" CommandName="edit"  CssClass="btn btn-info" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")+";"+DataBinder.Eval(Container.DataItem, "IdCliente") %>' Text="Editar" />
                                                                                        <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                                                                                            { %>

                                                <asp:Button ID="btnDelete" runat="server" CommandName="delete"  CssClass="btn btn-danger" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")+";"+DataBinder.Eval(Container.DataItem, "IdCliente") %>' Text="X" />
                                                <%} %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
