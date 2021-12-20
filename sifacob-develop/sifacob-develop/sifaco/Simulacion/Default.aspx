<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Simulacion.Default" EnableViewStateMac="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {
        $("#txtFechaD").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaD").inputmask();
        $("#txtFechaH").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaH").inputmask();
    });

</script>
<script>
    $(document).ready(function () {
        var table = $('#example2').DataTable({
            responsive: true,
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": false,
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
        Factoring
        <small>Simulaciones</small>
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
                        <h3 class="box-title">Simulaciones</h3>
                    </div>
                    <br />
                    <div class="form-horizontal">
                        <asp:PlaceHolder ID="plhDeleteQuestion" runat="server" Visible="false">
                            <div class="alert alert-warning alert-dismissible">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <h4><i class="icon fa fa-warning"></i> Alerta!</h4>
                                    ¿Estas seguro de eliminar la simulacion del cliente?
                                    <asp:Button runat="server" ID="btnEliminar" Text="Aceptar" CssClass="btn btn-block btn-warning" onclick="btnEliminar_Click" />
                                    <asp:Literal ID="ltHidden" runat="server" Visible="false"></asp:Literal>
                                    <asp:Literal ID="ltHidden2" runat="server" Visible="false"></asp:Literal>
                            </div>
                        </asp:PlaceHolder>
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
                            <b>Buscar por Cliente</b>
                            <asp:DropDownList runat="server" ID="ddlCliente" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-xs-3" style="float:right !important">
                            <br />
                            <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                                    CssClass="btn btn-info" Text="Buscar" 
                                    onclick="btnBuscar_Click"  />
                        </div>

                        <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Nombre(RUT)</th>
                                    <th>Estatus</th>
                                    <th>Tasa</th>
                                    <th>Anticipo</th>
                                    <th>Sal. Pendiente</th>
                                    <th>Gastos Oper.</th>
                                    <th>Monto</th>
                                    <th>Utilidad</th>
                                    <th>Anticipo</th>
                                    <th>Sal. Pendiente</th>
                                    <th>Precio Cesion</th>
                                    <th>Monto Girable</th>
                                    <th>Fecha Simulación</th>
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
                                            <td style="text-align:center;"><%# (Convert.ToInt32(Eval("IdEdoSim")) == 1) ? "<span class='label label-warning'>&nbsp;</span>" : "<span class='label label-success'>&nbsp;</span>"%></td>
                                            <td><%# Eval("Tasa")+"%"%></td>
                                            <td><%# Eval("Anticipo")+"%"%></td>
                                            <td><%# Eval("SaldoPendiente") + "%"%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("GastosOper").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoTotal").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("Utilidad").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoAnticipo").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoPendiente").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("PrecioCes").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoGirable").ToString()).ToString("N")%></td>
                                            <td><%#   String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Eval("Fecha")))%></td>
                                            <td>

                                                <asp:Button ID="btnEdit" runat="server" CommandName="edit"  CssClass="btn btn-info" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")+";"+DataBinder.Eval(Container.DataItem, "IdCliente") %>' 
                                                    Text='<%# (Session["rol"] != null && Session["rol"].ToString() == "admin") ? "Editar" : (Convert.ToInt32(Eval("IdEdoSim")) == 1) ? "Editar" : "Ver"%>' />
                                                                                       
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
    </div>
</form>
</asp:Content>
