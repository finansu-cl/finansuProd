<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DireccionPrestamo.ascx.cs" Inherits="sifaco.Controls.DireccionPrestamo" %>
<script src="../Scripts/lostFocus.js" type="text/javascript"></script>

        <asp:UpdatePanel ID="updPanelCiudad" runat="server">
            <ContentTemplate>

                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Región*</label>
                  <div class="col-sm-10" >
                   <asp:DropDownList runat="server" ID="ddlRegionEmp" ClientIDMode="Static" CssClass="form-control" 
                          AutoPostBack="true" onselectedindexchanged="ddlRegion_SelectedIndexChanged" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                          </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Ciudad*</label>
                  <div class="col-sm-10" >
                                    <asp:DropDownList runat="server" ID="ddlCiudadEmp" ClientIDMode="Static" CssClass="form-control" 
                                          AutoPostBack="true" onselectedindexchanged="ddlCiudad_SelectedIndexChanged" AppendDataBoundItems="true">
                                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                                        </asp:DropDownList>                                  
                   
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Comuna*</label>
                  <div class="col-sm-10" >
                   <asp:DropDownList runat="server" ID="ddlComunaEmp" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                          </asp:DropDownList>
                  </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlRegionEmp" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlCiudadEmp" EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Calle / Num / Dpto*</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtDireccion" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
