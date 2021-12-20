<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosPrestamo.ascx.cs" Inherits="sifaco.Controls.DatosPrestamo" %>
<script type='text/javascript'>
$(document).ready(function () {
    $(function () {
        $('#txtFecPres').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
        $('#txtFecPres').inputmask(); 
    });
});
</script>



<div class="col-xs-3">
<b>Monto del Prestamo</b>
                   <asp:TextBox runat="server" ID="txtMonto" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Plazo del Prestamo</b>
                   <asp:TextBox runat="server" ID="txtPlazo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Número de Cuotas</b>
                   <asp:TextBox runat="server" ID="txtNumCuotas" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-xs-3">
<b>Tasa del Prestamo</b>
                   <asp:TextBox runat="server" ID="txtTasa" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Meses de Gracia</b>
      <asp:TextBox runat="server" ID="txtMGracia" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-xs-3">
<b>Cuota</b>
                   <asp:TextBox runat="server" ID="txtCuotaEspecial" ClientIDMode="Static" CssClass="form-control oculto" Enabled="false">CUOTA ESPECIAL</asp:TextBox>
                   <asp:TextBox runat="server" ID="txtCuota" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>

<div class="col-xs-3">
<b>Fecha del Prestamo</b>
                   <asp:TextBox runat="server" ID="txtFecPres" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>

 <asp:TextBox runat="server" ID="txtIdPrestamo" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
