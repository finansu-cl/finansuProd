<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosGarantiaVehiculo.ascx.cs" Inherits="sifaco.Controls.DatosGarantiaVehiculo" %>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Tipo*</label>
    <div class="col-sm-10" >
        <asp:TextBox runat="server" ID="txtTipo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Marca*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtMarca" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Modelo*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtModelo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Año*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtAno" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Numero de Motor*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtMotor" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Numero de Chasis*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtChasis" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Color*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtColor" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Patente*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtPatente" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Numero de inscripcion en el R.V.M*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtRVM" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>


<asp:TextBox runat="server" ID="txtIdCliente" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
                         
