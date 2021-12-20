<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosBanco.ascx.cs" Inherits="sifaco.Controls.DatosBanco" %>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Banco*</label>
                  <div class="col-sm-10" >
                   <asp:DropDownList runat="server" ID="ddlBancos" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                    </asp:DropDownList>
                  </div>
                </div>
                    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Destinatario*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtDestinatario" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
                    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Rut Destinatario*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtRutDest" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Número de Cuenta*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtNumCta" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
                    <asp:TextBox runat="server" ID="txtIdCliente" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>
