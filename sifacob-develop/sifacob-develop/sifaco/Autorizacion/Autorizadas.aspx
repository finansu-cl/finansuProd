<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Autorizadas.aspx.cs" Inherits="sifaco.Autorizacion.Autorizadas" EnableViewStateMac="false"%>

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
        <small>Operaciones Autorizadas</small>
      </h1>--%>
</section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
<div class="row">
    <div class="box box-primary">
                <div class="box-header with-border">
                        <h3 class="box-title">Operaciones autorizadas</h3>
                    </div>
                    <br />

        <div class="form-horizontal">
        <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                <thead>
                <tr>
                  <th>Usuario que opera</th>
                  <th>Tipo de operación</th>
                  <th>Origen de la operación</th>
                  <th>Fecha de modificación</th>
                  <th>Estado</th>
                  <th>Usuario que autoriza</th>
                  <th>Fecha de autorizacion</th>
                </tr>
                </thead>
                <tbody>
        <asp:Repeater runat="server" ID="rptGrid">
            <ItemTemplate>
             <tr>
                  <td><%# Eval("UsuarioMod")%></td>
                  <td><%# Eval("TipoOperacion")%></td>
                  <td><%# DatosAlterados((Eval("Origen") == null) ? "_DEL_NA" : Eval("Origen").ToString(), Container.ItemIndex + 1)%></td>
                  <td><%# Eval("FechaMod")%></td>
                  <td><%# (Eval("Autorizado").ToString() == "1") ? "AUTORIZADO" : (Eval("Autorizado").ToString() == "2") ? "NO AUTORIZADO" : (Eval("Autorizado").ToString() == "3") ? "VENCIDO" : "PENDIENTE"%></td>
                  <td><%# Eval("UsuarioAut")%></td>
                  <td><%# Eval("FechaAut")%></td>
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