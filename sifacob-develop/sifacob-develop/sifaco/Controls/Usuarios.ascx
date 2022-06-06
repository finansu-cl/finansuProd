<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.ascx.cs" Inherits="sifaco.Controls.Usuarios" %>
                <div class="form-group">
                    <label for="inputPassword3" class="col-sm-2 control-label">Email*</label>
                    <div class="col-sm-10">
                          <asp:TextBox runat="server" ID="txtEmail" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Nombre*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNombre" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label">Perfil*</label>

                  <div class="col-sm-10">
                      <asp:DropDownList runat="server" ID="ddlPerfil" ClientIDMode="Static" CssClass="form-control">
                        <asp:ListItem Value="admin">Admin</asp:ListItem>
                        <asp:ListItem Value="user">User</asp:ListItem>
                        <asp:ListItem Value="analista">Analista</asp:ListItem>
                        <asp:ListItem Value="guest">Invitado</asp:ListItem>
                      </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Url Imagen*</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtUsrlImg" Enabled="false" Text="~/Styles/img/img-default.png" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>

                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Clave*</label>
        <div class="col-sm-10">
            <asp:TextBox runat="server" ID="txtClave" ClientIDMode="Static" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
    </div>
    
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Confirmar Clave*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtCClave" ClientIDMode="Static" CssClass="form-control"  TextMode="Password"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Activo</label>
        <div class="col-sm-10">
             <asp:CheckBox ID="chkActi" runat="server" />
        </div>
    </div>
                  <asp:TextBox runat="server" ID="txtIdUsuarios" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
