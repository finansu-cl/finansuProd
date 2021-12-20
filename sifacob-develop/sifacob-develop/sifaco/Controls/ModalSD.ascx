<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalSD.ascx.cs" Inherits="sifaco.Controls.ModalSD" %>
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
<div id="myModalSD" class="modal fade" role="dialog">
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
                                <li class="active"><a href="#tab_1" data-toggle="tab">2° Compareciente</a></li>
                                <li><a href="#tab_2" data-toggle="tab">Vehiculo(s)</a></li>
                                <li><a href="#tab_3" data-toggle="tab">Aval</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab_1">
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
                                <div class="tab-pane" id="tab_2">
                                    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="box-body table-responsive no-padding">
                                                <asp:GridView ID="gvPerson" runat="server" AutoGenerateColumns="False" CssClass="table table-hover"
                                                onpageindexchanging="gvPerson_PageIndexChanging"  
                                                onrowcancelingedit="gvPerson_RowCancelingEdit"  
                                                onrowdatabound="gvPerson_RowDataBound" onrowdeleting="gvPerson_RowDeleting"  
                                                onrowediting="gvPerson_RowEditing" onrowupdating="gvPerson_RowUpdating"  
                                                onsorting="gvPerson_Sorting"> 
                                                    <RowStyle/> 
                                                    <Columns> 
                                                        <asp:CommandField ShowEditButton="True"  /> 
                                                        <asp:CommandField ShowDeleteButton="True" /> 
                                                        <asp:BoundField DataField="ID" ReadOnly="True" Visible="True" SortExpression="ID" /> 
                                                        <asp:TemplateField HeaderText="Tipo" SortExpression="Tipo"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt1" runat="server" Text='<%# Bind("Tipo") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt11" runat="server" Text='<%# Bind("Tipo") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Marca" SortExpression="Marca"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt2" runat="server" Text='<%# Bind("Marca") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt12" runat="server" Text='<%# Bind("Marca") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Modelo" SortExpression="Modelo"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt3" runat="server" Text='<%# Bind("Modelo") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt13" runat="server" Text='<%# Bind("Modelo") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Año" SortExpression="Ano"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt4" runat="server" Text='<%# Bind("Ano") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt14" runat="server" Text='<%# Bind("Ano") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="N° Motor" SortExpression="NumMotor"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt5" runat="server" Text='<%# Bind("Motor") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt15" runat="server" Text='<%# Bind("Motor") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="N° Chasis" SortExpression="NumChasis"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt6" runat="server" Text='<%# Bind("Chasis") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt16" runat="server" Text='<%# Bind("Chasis") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Color" SortExpression="Color"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt7" runat="server" Text='<%# Bind("Color") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt17" runat="server" Text='<%# Bind("Color") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Patente" SortExpression="Patente"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt8" runat="server" Text='<%# Bind("Patente") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt18" runat="server" Text='<%# Bind("Patente") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="N° RVM" SortExpression="NumRVM"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt9" runat="server" Text='<%# Bind("Rvm") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt19" runat="server" Text='<%# Bind("Rvm") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                    </Columns> 
                                                </asp:GridView> 
                                            </div>
                                        </ContentTemplate>
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lbtnSubmit" EventName="Click" />
                                    </Triggers>
                                    </asp:UpdatePanel>  
                                    <asp:UpdatePanel ID="InsertEmployeeUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate> 
                                            <uc:datosVehiculo runat="server" ID="DatosVeh" />
                                            <div class="col-md-6">
                                                <asp:LinkButton ID="lbtnSubmit" runat="server" CssClass="btn btn-block btn-success" onclick="lbtnSubmit_Click">Agregar</asp:LinkButton> 
                                            </div>  
                                            <div class="col-md-6">      
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn btn-block btn-default" onclick="lbtnCancel_Click">Cancelar</asp:LinkButton> 
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                         
                                </div>
                            <!-- /.tab-pane -->
                                <div class="tab-pane" id="tab_3">
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