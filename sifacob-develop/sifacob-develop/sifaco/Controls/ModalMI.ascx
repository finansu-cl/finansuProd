<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalMI.ascx.cs" Inherits="sifaco.Controls.ModalMI" %>
<%@ Register TagPrefix="uc" TagName="prestamoPersona" Src="~/Controls/Prestamos_Persona.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosAval" Src="~/Controls/DatosAval.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosVehiculo" Src="~/Controls/DatosGarantiaVehiculo.ascx" %>
<script type="text/javascript" language="javascript">
       $(document).ready(function () {
        $('#txtRutPer').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
        $('#txtRutEmp').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
        $('#txtRutAval').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
    });
</script>
<!-- Modal -->
<div id="myModalMI" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 80% !important;">
    <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Prenda sin desplazamiento</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                    <!-- Custom Tabs -->
                        <div class="nav-tabs-custom">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab_12" data-toggle="tab">2° Compareciente</a></li>
                                <li><a href="#tab_22" data-toggle="tab">Aval</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab_12">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:prestamoPersona runat="server" ID="PrestamoPersona" />
                                            <div class="col-md-6">
                                                <asp:LinkButton ID="lbPP1" runat="server" CssClass="btn btn-block btn-success" 
                                                    onclick="lbPP1_Click">Aceptar</asp:LinkButton> 
                                                <asp:LinkButton ID="lbPPU1" Visible="false" runat="server" CssClass="btn btn-block btn-success" 
                                                    onclick="lbPPU1_Click">Aceptar</asp:LinkButton> 
                                            </div>  
                                            <div class="col-md-6">      
                                                <asp:LinkButton ID="lbPP2" runat="server" CssClass="btn btn-block btn-default" 
                                                    onclick="lbPP2_Click">Cancelar</asp:LinkButton> 
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            <!-- /.tab-pane -->
                                <div class="tab-pane" id="tab_22">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:datosAval runat="server" ID="datosAval" />
                                            <div class="col-md-6">
                                                <asp:LinkButton ID="lbA1" runat="server" CssClass="btn btn-block btn-success" 
                                                    onclick="lbA1_Click">Aceptar</asp:LinkButton> 
                                                <asp:LinkButton ID="lbAU1" Visible="false" runat="server" CssClass="btn btn-block btn-success" 
                                                    onclick="lbAU1_Click">Aceptar</asp:LinkButton> 
                                            </div>  
                                            <div class="col-md-6">      
                                                <asp:LinkButton ID="lbA2" runat="server" CssClass="btn btn-block btn-default" 
                                                    onclick="lbA2_Click">Cancelar</asp:LinkButton> 
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            <!-- /.tab-pane -->
                            </div>
                        <!-- /.tab-content -->
                        </div>
                    <!-- nav-tabs-custom -->
                    </div>
                <!-- /.col -->
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>