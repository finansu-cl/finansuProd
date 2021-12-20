<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalCP.ascx.cs" Inherits="sifaco.Controls.ModalCP" %>
<%@ Register TagPrefix="uc" TagName="datosPrenda" Src="~/Controls/DatosPrendaVehiculo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosVehiculo" Src="~/Controls/DatosGarantiaVehiculo.ascx" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $(function () {
            $("#txtFecha").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
            $("#txtFecha").inputmask();
        });
        $('#txtRut').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });

    });
</script>

<!-- Modal -->
<div id="myModalCP" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 80% !important;">
    <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Cancelación de Prenda</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                    <!-- Custom Tabs -->
                        <div class="nav-tabs-custom">
                            <ul class="nav nav-tabs">
                              <li class="active"><a href="#tab_11" data-toggle="tab">Vehiculo</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab_11">
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
                                                        <asp:TemplateField HeaderText="Notaria" SortExpression="Notaria"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt110" runat="server" Text='<%# Bind("Notaria") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt1110" runat="server" Text='<%# Bind("Notaria") %>'></asp:Label> 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt120" runat="server" Text='<%# Bind("NOMBRE_INSCRITO") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt1120" runat="server" Text='<%# Bind("NOMBRE_INSCRITO") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rut" SortExpression="Rut">
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt130" runat="server" Text='<%# Bind("Rut_INSCRITO") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt1130" runat="server" Text='<%# Bind("Rut_INSCRITO") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha Escritura" SortExpression="Fecha">
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txt140" runat="server" Text='<%# Bind("Fecha_ESCRITURA") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txt1140" runat="server" Text='<%# Bind("Fecha_ESCRITURA") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField>
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
                                            <uc:datosPrenda runat="server" ID="Prenda" />
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