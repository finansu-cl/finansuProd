<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalMH.ascx.cs" Inherits="sifaco.Controls.ModalMH" %>
<%@ Register TagPrefix="uc" TagName="prestamoPersona" Src="~/Controls/Prestamos_Persona.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosPropiedad" Src="~/Controls/DatosGarantiaHipotecario.ascx" %>
<script type="text/javascript" language="javascript">
    function enviar() {
        var retorno = true;

        var e = document.getElementById("ddlRegion");
        if (e.options[e.selectedIndex].value < "0" || e.options[e.selectedIndex].value == "") {
            alert("Debe seleccionar la REGION");
            return false;
        }
        var e = document.getElementById("ddlCiudad");
        if (e.options[e.selectedIndex].value < "0" || e.options[e.selectedIndex].value == "") {
            alert("Debe seleccionar la CIUDAD");
            return false;
        }
        var e = document.getElementById("ddlComuna");
        if (e.options[e.selectedIndex].value < "0" || e.options[e.selectedIndex].value == "") {
            alert("Debe seleccionar la COMUNA");
            return false;
        }
        if (document.getElementById("txtDireccion").value == "") {
            alert("Debe completar la DIRECCION del cliente");
            return false;
        }
  
        return retorno;
    }

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
        $(function () {
            $("#txtH9").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
            $("#txtH9").inputmask();
            $("#txtFechaEscritura").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
            $("#txtFechaEscritura").inputmask();
        });

        $("#lbPP1").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });
    });
</script>
<!-- Modal -->
<div id="myModalMH" class="modal fade" role="dialog">
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
                                <li class="active"><a href="#tab_h1" data-toggle="tab">2° Compareciente</a></li>
                                <li><a href="#tab_h2" data-toggle="tab">Propiedades</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab_h1">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:prestamoPersona runat="server" ID="PrestamoPersona" />
                                            <div class="col-md-6">
                                                <asp:LinkButton ID="lbPP1" runat="server" ClientIDMode="Static" CssClass="btn btn-block btn-success" 
                                                    onclick="lbPP1_Click">Aceptar</asp:LinkButton> 
                                                <asp:LinkButton ID="lbPPU1" Visible="false" ClientIDMode="Static" runat="server" CssClass="btn btn-block btn-success" 
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
                                <div class="tab-pane" id="tab_h2">
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
                                                        <asp:TemplateField HeaderText="Deslindes" SortExpression="Deslindes"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH1" runat="server" Text='<%# Bind("Deslindes") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH11" runat="server" Text='<%# Bind("Deslindes") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Nombre de quien compro la propiedad" SortExpression="Nombre_Compro_Propiedad"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH2" runat="server" Text='<%# Bind("Nombre_Compro_Propiedad") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH12" runat="server" Text='<%# Bind("Nombre_Compro_Propiedad") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Nombre del Notario" SortExpression="Nombre_Notario"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH3" runat="server" Text='<%# Bind("Nombre_Notario") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH13" runat="server" Text='<%# Bind("Nombre_Notario") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Fojas" SortExpression="Fojas"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH4" runat="server" Text='<%# Bind("Fojas") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH14" runat="server" Text='<%# Bind("Fojas") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Numero" SortExpression="Numero"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH5" runat="server" Text='<%# Bind("Numero") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH15" runat="server" Text='<%# Bind("Numero") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Ubicacion Cbrs" SortExpression="Ubicacion_Cbrs"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH6" runat="server" Text='<%# Bind("Ubicacion_Cbrs") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH16" runat="server" Text='<%# Bind("Ubicacion_Cbrs") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Ano" SortExpression="Ano"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH7" runat="server" Text='<%# Bind("Ano") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH17" runat="server" Text='<%# Bind("Ano") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Rol" SortExpression="Rol"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH8" runat="server" Text='<%# Bind("Rol") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH18" runat="server" Text='<%# Bind("Rol") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Comuna" SortExpression="Comuna"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH0" runat="server" Text='<%# Bind("Comuna") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH10" runat="server" Text='<%# Bind("Comuna") %>'></asp:Label> 
                                                            </ItemTemplate> 
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Fecha de la escritura" SortExpression="Fecha_Escritura"> 
                                                            <EditItemTemplate> 
                                                                <asp:TextBox ID="txtH9" ClientIDMode="Static" runat="server" Text='<%# Bind("Fecha_Escritura") %>'></asp:TextBox> 
                                                            </EditItemTemplate> 
                                                            <ItemTemplate> 
                                                                <asp:Label ID="txtH19" runat="server" Text='<%# Bind("Fecha_Escritura") %>'></asp:Label> 
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
                                            <uc:datosPropiedad runat="server" ID="DatosHip" />
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