<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosPrendaVehiculo.ascx.cs" Inherits="sifaco.Controls.DatosPrendaVehiculo" %>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Fecha de Escritura*</label>
    <div class="col-sm-10" >
        <asp:TextBox runat="server" ID="txtFecha" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Notaria*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtNotaria" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Nombre*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtNombre" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Rut*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtRut" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>

<asp:TextBox runat="server" ID="txtIdCliente" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
                         
