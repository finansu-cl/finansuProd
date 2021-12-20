<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosFactura.ascx.cs" Inherits="sifaco.Controls.DatosFactura" %>
<div class="col-xs-3">
<b>Deudor</b>
                  <asp:TextBox runat="server" ID="txtDeudor" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Rut del Deudor</b>
                  <asp:TextBox runat="server" ID="txtRutD" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Numero de Factura</b>
                  <asp:TextBox runat="server" ID="txtNumFac" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Tipo de Factura</b>
                 <asp:DropDownList runat="server" ID="ddlTipoFac" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                 </asp:DropDownList>

</div>
<div class="col-xs-3">
<b>Monto de la Factura</b>
                   <asp:TextBox runat="server" ID="txtMonto" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Plazo de la Factura</b>
                   <asp:TextBox runat="server" ID="txtPlazoFac" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Monto Anticipo</b>
                   <asp:TextBox runat="server" ID="txtAnticipoFac" ClientIDMode="Static" Enabled="false" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Saldo Pendiente</b>
                   <asp:TextBox runat="server" ID="txtSalPendienteFac" ClientIDMode="Static" Enabled="false" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Utilidad</b>
                   <asp:TextBox runat="server" ID="txtUtilidadFac" ClientIDMode="Static" Enabled="false" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Saldo</b>
                   <asp:TextBox runat="server" ID="txtGirableFac" ClientIDMode="Static" Enabled="false" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Dirección Deudor</b>
                   <asp:TextBox runat="server" ID="txtDireccionFac" ClientIDMode="Static" Enabled="true" CssClass="form-control"></asp:TextBox>
</div>
<div class="col-xs-3">
<b>Comuna Deudor</b>
                   <asp:DropDownList runat="server" ID="ddlComuna" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                          </asp:DropDownList>
</div>
<div class="col-xs-3">
<b>Fecha de Emisión</b>
                   <asp:TextBox runat="server" ID="txtFechaEmision" ClientIDMode="Static" Enabled="true" CssClass="form-control"></asp:TextBox>
</div>
 <asp:TextBox runat="server" ID="txtIdFactura" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>