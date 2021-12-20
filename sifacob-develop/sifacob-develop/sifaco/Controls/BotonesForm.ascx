<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BotonesForm.ascx.cs" Inherits="sifaco.Controls.BotonesForm" %>
              <!-- /.box-body -->
              <div class="box-footer">
                <asp:Button runat="server" ID="btnCancel"  CssClass="btn btn-default" 
                      Text="Cancel" onclick="btnCancel_Click"/>
                <%--<button type="submit" class="btn btn-default">Cancel</button>--%>
                <asp:Button runat="server" ID="btnAceptar"  CssClass="btn btn-info pull-right" 
                      Text="Aceptar" onclick="btnAceptar_Click" />
                <%--<button type="submit" class="btn btn-info pull-right">Aceptar</button>--%>
              </div>
              <!-- /.box-footer -->
