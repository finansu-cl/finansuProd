<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosGarantiaHipotecario.ascx.cs" Inherits="sifaco.Controls.DatosGarantiaHipotecario" %>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Deslindes*</label>
    <div class="col-sm-10" >
        <asp:TextBox runat="server" ID="txtDeslindes" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Nombre de quien compro la propiedad*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtNombreCP" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Nombre del Notario*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtNombreN" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Fojas*</label>
    <div class="col-sm-10">
        <asp:TextBox runat="server" ID="txtFojas" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Numero*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtNumero" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Ubicacion CBRS*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtUbicacionCbrs" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Año*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtAno" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Comuna*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtComuna" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Rol*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtRol" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>

<div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Fecha de Escritura*</label>
    <div class="col-sm-10">
    <asp:TextBox runat="server" ID="txtFechaEscritura" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
    </div>
</div>


<asp:TextBox runat="server" ID="txtIdCliente" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
                         
