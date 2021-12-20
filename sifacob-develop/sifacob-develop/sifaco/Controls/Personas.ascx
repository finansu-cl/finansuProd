<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Personas.ascx.cs" Inherits="sifaco.Controls.Personas" %>
<%@ Register TagPrefix="uc" TagName="direccion" Src="~/Controls/Direccion.ascx" %>
<asp:ScriptManager ID="ScriptManager" runat="server" />
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label">Rut*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtRutPer" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Nombre*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNombre" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Sexo*</label>
                  <div class="col-sm-10" >
                      <asp:DropDownList runat="server" ID="ddlSexo" ClientIDMode="Static" CssClass="form-control">
                        <asp:ListItem Value="M">Hombre</asp:ListItem>
                        <asp:ListItem Value="F">Mujer</asp:ListItem>
                      </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Estado Civil*</label>
                  <div class="col-sm-10" >
                      <asp:DropDownList runat="server" ID="ddlEdoCivil" ClientIDMode="Static" CssClass="form-control">
                        <asp:ListItem Value="SO">Soltero</asp:ListItem>
                        <asp:ListItem Value="CA">Casado</asp:ListItem>
                        <asp:ListItem Value="VI">Viudo</asp:ListItem>
                        <asp:ListItem Value="DI">Divorciado</asp:ListItem>
                        <asp:ListItem Value="CC">Conviviente Civil</asp:ListItem>
                      </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Nacionalidad*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNac" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Profesión*</label>
        <div class="col-sm-10">
            <asp:TextBox runat="server" ID="txtProfesion" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    
    <uc:direccion runat="server" ID="dir" />

    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Teléfono*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtTelefono" ClientIDMode="Static" CssClass="form-control" MaxLength="9"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Celular</label>
        <div class="col-sm-10">
             <asp:TextBox runat="server" ID="txtCelular" ClientIDMode="Static" CssClass="form-control" MaxLength="9"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Email*</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtEmail" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
                  <asp:TextBox runat="server" ID="txtIdPersonas" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
