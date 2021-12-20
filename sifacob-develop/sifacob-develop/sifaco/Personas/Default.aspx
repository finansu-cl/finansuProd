<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Personas.Default" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="persona" Src="~/Controls/Personas.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosBanco" Src="~/Controls/DatosBanco.ascx" %>
<%@ Register TagPrefix="uc" TagName="botones" Src="~/Controls/BotonesForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="aval" Src="~/Controls/DatosAval.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function enviar() {

        var retorno = true;

        if (document.getElementById("txtRutPer").value == "") {
            alert("Debe ingresar el RUT del cliente");
            return false;
        }

        if (document.getElementById("txtNombre").value == "") {
            alert("Debe completar el NOMBRE del cliente");
            return false;
        }
        if (document.getElementById("txtNac").value == "") {
            alert("Debe completar la Nacionalidad del cliente");
            return false;
        }
        if (document.getElementById("txtProfesion").value == "") {
            alert("Debe completar la PROFESION del cliente");
            return false;
        }
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
        if (document.getElementById("txtTelefono").value == "" && document.getElementById("txtCelular").value == "") {
            alert("Debe completar al menos un TELEFONO del cliente");
            return false;
        }

        if (!esEmail(document.getElementById("txtEmail"), "E-mail", true, true)) {
            return false;
        }

        var e = document.getElementById("ddlBancos");
        if (e.options[e.selectedIndex].value < "0" || e.options[e.selectedIndex].value == "") {
            alert("Debe seleccionar el BANCO");
            return false;
        }

        if (document.getElementById("txtDestinatario").value == "") {
            alert("Debe completar el NOMBRE del destinatario");
            return false;
        }

        if (document.getElementById("txtRutDest").value == "") {
            alert("Debe completar el RUT del destinatario");
            return false;
        }

        if (document.getElementById("txtNumCta").value == "") {
            alert("Debe completar el NUMERO DE CUENTA");
            return false;
        }


        return retorno;

    }


    $(document).ready(function () {
        $('#txtRutPer').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
        $('#txtRutDest').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
        $('#txtRutAval').Rut({
            on_error: function () { alert('Rut incorrecto'); }
        });
        $("#btnAceptar").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });

    });
</script>
<script type="text/javascript">
    $(function () {
        $('#example2').DataTable({
            responsive: true,
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": false,
            "autoWidth": false
        });
    });
</script>

</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Clientes
        <small>Personas</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
<div class="row">


            <!-- /.col -->
        <div class="col-md-12">
          <div class="nav-tabs-custom">
            <ul class="nav nav-tabs">
              <li class="active" runat="server" id="tab1"><a href="#timeline" data-toggle="tab">Lista de Clientes Personas</a></li>
              <li  runat="server" id="tab2"><a href="#activity" data-toggle="tab">Cliente Persona</a></li>
            </ul>
            <div class="tab-content">
              <!-- /.tab-pane -->
              <div class="active tab-pane" runat="server" id="timeline" clientidmode="Static">
                <asp:PlaceHolder ID="plhDeleteQuestion" runat="server" Visible="false">
                    <div class="alert alert-warning alert-dismissible">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <h4><i class="icon fa fa-warning"></i> Alerta!</h4>
                            ¿Estas seguro de eliminar el cliente?
                            <asp:Button runat="server" ID="btnEliminar" Text="Aceptar" CssClass="btn btn-block btn-warning" onclick="btnEliminar_Click" />
                            <asp:Literal ID="ltHidden" runat="server" Visible="false"></asp:Literal>
                        </div>
                </asp:PlaceHolder>

                <table id="example2" class="table table-bordered table-hover">
                        <thead>
                        <tr>
                          <th>Nombre y Apellido</th>
                          <th>Simular</th>
                          <th>Prestamo</th>
                          <th>Editar</th>
                        <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                            { %>
                            <th>Eliminar</th>
                            <%} %>
                        </tr>
                        </thead>
                        <tbody>
                <asp:Repeater runat="server" ID="rptGrid" onitemcommand="rptGrid_ItemCommand">
                    <ItemTemplate>
                     <tr>
                          <td><%# Eval("Nombre") %>&nbsp;(<%# Eval("Rut") %>)</td>
                          <td><asp:LinkButton ID="lbSim" runat="server" CommandName="sim"  CssClass="btn btn-app"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                        <i class="fa fa-play"></i> Simular
                                        </asp:LinkButton></td>
                          <td><asp:LinkButton ID="lbPres" runat="server" CommandName="pres"  CssClass="btn btn-app"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                        <i class="fa fa-inbox"></i> Prestamo
                                        </asp:LinkButton></td>                  
                          <td><asp:LinkButton ID="lbEdit" runat="server" CommandName="edit"  CssClass="btn btn-app"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Rut")+";"+DataBinder.Eval(Container.DataItem, "ID") %>'>
                                            <i class="fa fa-edit"></i> Editar
                                            </asp:LinkButton></td>
                                            <% if (Session["rol"] != null && Session["rol"].ToString() == "admin")
                                                { %>

                          <td><asp:LinkButton ID="lbDelete" runat="server" ClientIDMode="Static" CommandName="delete"  CssClass="btn btn-app"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdCliente") %>'>
                                            <i class="fa fa-barcode"></i> Eliminar
                                         </asp:LinkButton></td>
                         <%} %>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>
               </table>
              </div>
              <div class="tab-pane"  runat="server" id="activity" clientidmode="Static">
                  <div class="form-horizontal" runat="server">
                    <div class="box-body">
                        <div class="box-header with-border">
                          <h3 class="box-title">Datos Cliente</h3>
                        </div>
                        <uc:persona runat="server" ID="per" />
                        <div class="box-header with-border">
                          <h3 class="box-title">Datos Banco</h3>
                        </div>
                        <uc:datosBanco runat="server" ID="DatosBanco" />
                       <div class="box-header with-border">
                          <h3 class="box-title">Datos Aval</h3>
                        </div>
                        <uc:aval runat="server" ID="DatosAval" />
                          <div class="box-footer">
                            <asp:Button runat="server" ID="btnCancel" ClientIDMode="Static" CssClass="btn btn-default" 
                                  Text="Cancel" onclick="btnCancel_Click"/>
                            <asp:Button runat="server" ID="btnAceptar" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                                  Text="Aceptar" onclick="btnAceptar_Click" />
                            <asp:Button runat="server" ID="btnModificar" Visible="false" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                                  Text="Aceptar" onclick="btnModificar_Click" />
                          </div>
                    </div>
                </div>

              </div>
            </div>
            <!-- /.tab-content -->
          </div>
          <!-- /.nav-tabs-custom -->
        </div>
        <!-- /.col -->
</div>
</form>
</asp:Content>
