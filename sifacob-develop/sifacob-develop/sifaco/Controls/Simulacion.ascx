<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Simulacion.ascx.cs" Inherits="sifaco.Controls.Simulacion" %>

                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label">Estado de la simulacion</label>

                  <div class="col-sm-10">
                        <asp:DropDownList runat="server" ID="ddlEdoSim" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                          <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                 </asp:DropDownList>
                  </div>
                </div>
                <div class="form-group">
                    <label for="inputPassword3" class="col-sm-2 control-label">Fecha de la Simulacion</label>
                    <div class="col-sm-10">
                          <asp:TextBox runat="server" ID="txtFecSimulacion" ClientIDMode="Static" CssClass="form-control" Enabled="true"  ></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Tasa</label>
                  <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtTasa" ClientIDMode="Static" CssClass="form-control" Text="3"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtNvaTasa" ClientIDMode="Static" CssClass="form-control" Text="3" Visible="false"></asp:TextBox>
                    <asp:CheckBox runat="server" ID="chkCambio" ClientIDMode="Static" 
                          CssClass="form-control" AutoPostBack="true" 
                          oncheckedchanged="chkCambio_CheckedChanged" Text="Aplicar Nueva Tasa" />
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label">Anticipo</label>
                  <div class="col-sm-10" >
                    <asp:TextBox runat="server" ID="txtAnticipo" ClientIDMode="Static" CssClass="form-control" Text="95"></asp:TextBox>
                  </div>
                </div>
                <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Saldo Pendiente</label>
        <div class="col-sm-10">
            <asp:TextBox runat="server" ID="txtSalPendiente" ClientIDMode="Static" Enabled="false" Text="5" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    
<%--    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Plazo</label>
        <div class="col-sm-10">
                    <asp:TextBox runat="server" ID="txtPlazo" ClientIDMode="Static" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
--%>    
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Gastos de Operación</label>
        <div class="col-sm-10">
             <asp:TextBox runat="server" ID="txtGasOperacion" ClientIDMode="Static" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Comisión</label>
        <div class="col-sm-10">
             <asp:TextBox runat="server" ID="txtComision" ClientIDMode="Static" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">IVA</label>
        <div class="col-sm-10">
             <asp:TextBox runat="server" ID="txtIva" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Monto Total</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtMonTotal" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Utilidad</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtMonInteres" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>    
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Monto Anticipo</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtMonAnticipo" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Monto Pendiente</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtMonPendiente" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Precio Cesion</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtPreCesion" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label for="inputPassword3" class="col-sm-2 control-label">Monto Girable</label>
        <div class="col-sm-10">
              <asp:TextBox runat="server" ID="txtMonGirable" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
    </div>
                  <asp:TextBox runat="server" ID="txtIdSimulacion" Visible="false" ClientIDMode="Static" Text="0"></asp:TextBox>

