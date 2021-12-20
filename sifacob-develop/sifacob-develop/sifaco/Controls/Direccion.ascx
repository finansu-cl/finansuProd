<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Direccion.ascx.cs" Inherits="sifaco.Controls.Direccion" %>
<script src="../Scripts/lostFocus.js" type="text/javascript"></script>
        <asp:UpdatePanel ID="updPanelCiudad" runat="server">
            <ContentTemplate>

                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Región*</label>
                  <div class="col-sm-10" >
                   <asp:DropDownList runat="server" ID="ddlRegion" ClientIDMode="Static" CssClass="form-control" 
                          AutoPostBack="true" onselectedindexchanged="ddlRegion_SelectedIndexChanged" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                          </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Ciudad*</label>
                  <div class="col-sm-10" >
                                    <asp:DropDownList runat="server" ID="ddlCiudad" ClientIDMode="Static" CssClass="form-control" 
                                          AutoPostBack="true" onselectedindexchanged="ddlCiudad_SelectedIndexChanged" AppendDataBoundItems="true">
                                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                                        </asp:DropDownList>                                  
                   
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Comuna*</label>
                  <div class="col-sm-10" >
                   <asp:DropDownList runat="server" ID="ddlComuna" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                          </asp:DropDownList>
                  </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlCiudad" EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Calle / Num / Dpto*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtDireccion" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
