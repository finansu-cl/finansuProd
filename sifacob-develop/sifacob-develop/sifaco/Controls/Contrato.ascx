<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contrato.ascx.cs" Inherits="sifaco.Controls.Contrato" %>

<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Nombre*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtNombre" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Funcionalidad*</label>
    <div class="col-sm-10">
        <asp:DropDownList runat="server" ID="ddlFunc" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
            <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
            <asp:ListItem Value="001">Factoring</asp:ListItem>
            <asp:ListItem Value="002">Prestamo</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Contrato*</label>
    <div class="col-sm-10">
        <asp:FileUpload runat="server" ID="fuContrato" />
    </div>
</div>

<asp:TextBox runat="server" ID="txtIdContrato" Visible="false" Text="0"></asp:TextBox>