<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Autorizacion.Default" EnableViewStateMac="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('#example2').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": false,
            "info": false,
            "autoWidth": false
        });
    });

    function MiClick(id1,id2) {
        $(id1).click(function () {
            $(id2).toggle();
        });
    }

</script>

</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Autorizacion
        <small>Operaciones</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
<div class="row">
    <div class="box box-primary">
        <div class="box-header with-border">
                        <h3 class="box-title">Operaciones pendientes por autorizar</h3>
                    </div>
                    <br />
        <div class="form-horizontal">
        <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                <thead>
                <tr>
                  <th>Usuario que opera</th>
                  <th>Tipo de operación</th>
                  <th>Origen de la operación</th>
                  <th>Fecha de Modificación</th>
                  <th>Estado</th>
                  <th>Autoriza</th>
                  <th>Rechaza</th>
                </tr>
                </thead>
                <tbody>
        <asp:Repeater runat="server" ID="rptGrid" onitemcommand="rptGrid_ItemCommand">
            <ItemTemplate>
             <tr>
                  <td><%# Eval("UsuarioMod")%></td>
                  <td><%# Eval("TipoOperacion")%></td>
                  <td><%# DatosAlterados((Eval("Origen") == null) ? "_DEL_NA" : Eval("Origen").ToString(), Container.ItemIndex + 1)%></td>
                  <td><%# Eval("FechaMod")%></td>
                  <td><%# (Eval("Autorizado").ToString() == "1") ? "AUTORIZADO" : (Eval("Autorizado").ToString() == "2") ? "NO AUTORIZADO" : (Eval("Autorizado").ToString() == "3") ? "VENCIDO" : "PENDIENTE"%></td>
                  <td><asp:LinkButton ID="lbEdit" runat="server" CommandName="edit"  CssClass="btn btn-app"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                    <i class="fa fa-edit"></i> Autorizar
                                    </asp:LinkButton></td>
                  <td><asp:LinkButton ID="lbDelete" runat="server" ClientIDMode="Static" CommandName="delete"  CssClass="btn btn-app"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                    <i class="fa fa-barcode"></i> Rechazar
                                 </asp:LinkButton></td>
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
