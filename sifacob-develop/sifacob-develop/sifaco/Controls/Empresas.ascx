<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Empresas.ascx.cs" Inherits="sifaco.Controls.Empresas" %>
<%@ Register TagPrefix="uc" TagName="direccion" Src="~/Controls/Direccion.ascx" %>
<asp:ScriptManager ID="ScriptManager" runat="server" />
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label">Rut*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtRutEm" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Razón Social*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtRazon" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Tipo de Empresa*</label>
        <div class="col-sm-10">
                 <asp:DropDownList runat="server" ID="ddlTipoEmp" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                 </asp:DropDownList>
        </div>
    </div>
                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Giro Comercial*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtGiro" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <uc:direccion runat="server" ID="dir" />
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Teléfono*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtTlf" ClientIDMode="Static"  CssClass="form-control" MaxLength="9"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Nombre de la Notaria*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNomNotaria" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
    <label for="inputPassword3" class="col-sm-2 control-label">Fecha Escritura*</label>
    <div class="col-sm-10">
    <div class="input-group">
        <div class="input-group-addon">
        <i class="fa fa-calendar"></i>
        </div>
       <asp:TextBox runat="server" ID="txtFecha" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
    </div>
    </div>
    <!-- /.input group -->
    </div>
    <asp:TextBox runat="server" ID="txtIdEmpresa" Visible="false" Text="0"></asp:TextBox>
