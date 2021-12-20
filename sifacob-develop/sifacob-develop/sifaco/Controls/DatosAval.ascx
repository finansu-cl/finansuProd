<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosAval.ascx.cs" Inherits="sifaco.Controls.DatosAval" %>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Nombre</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNombreAval" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label">Rut</label>

                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtRutAval" ClientIDMode="Static" CssClass="form-control" MaxLength="11"></asp:TextBox>
                  </div>
                </div>
                                  <asp:TextBox runat="server" ID="txtIdCliente" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
