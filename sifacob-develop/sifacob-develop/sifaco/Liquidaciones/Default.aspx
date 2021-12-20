<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Liquidaciones.Default" EnableViewStateMac="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
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
                        <h3 class="box-title">Clientes liquidados</h3>
                    </div>
                    <br />
        <div class="box-body no-padding">
        <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                <thead>
                <tr>
                  <th>Cliente</th>
                  <th>Rut</th>
                </tr>
                </thead>
                <tbody>
        <asp:Repeater runat="server" ID="rptGrid">
            <ItemTemplate>
             <tr>
                  <td><%# Eval("Nombre")%></td>
                  <td><%# Eval("Rut")%></td>
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
