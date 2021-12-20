<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Usuarios.Default" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="usuarios" Src="~/Controls/Usuarios.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function enviar() {

        var retorno = true;

        if (document.getElementById("txtEmail").value != "") {
            if (!esEmail(document.getElementById("txtEmail"), "E-mail", true, true)) {
                return false;
            }
        } 
        else {
            alert("Debe completar el EMAIL del usuario");
            return false;
         }

        if (document.getElementById("txtNombre").value == "") {
            alert("Debe completar el NOMBRE del usuario");
            return false;
        }
        if (document.getElementById("txtPerfil").value == "") {
            alert("Debe completar el PERFIL del usuario");
            return false;
        }
        if (document.getElementById("txtUsrlImg").value == "") {
            alert("Debe completar la IMAGEN del usuario");
            return false;
        }

        if (document.getElementById("txtClave").value == "") {
            alert("Debe completar el CLAVE del usuario");
            return false;
        }

        if (document.getElementById("txtCClave").value != document.getElementById("txtClave").value) {
            alert("Las CLAVES deben coincidir");
            return false;
        }

        return retorno;

    }
    function enviar2() {

        var retorno = true;

        if (document.getElementById("txtNombre").value == "") {
            alert("Debe completar el NOMBRE del usuario");
            return false;
        }
        if (document.getElementById("txtPerfil").value == "") {
            alert("Debe completar el PERFIL del usuario");
            return false;
        }
        if (document.getElementById("txtUsrlImg").value == "") {
            alert("Debe completar la IMAGEN del usuario");
            return false;
        }

        return retorno;

    }
    $(document).ready(function () {
        
        $("#btnAceptar").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });

        $("#btnModificar").click(function () {
            if (enviar2() == false)
                return false;
            else {
                return true;
            }
        });


    });
</script>
<script type="text/javascript">
    $(document).ready(function () {
        var table = $('#example2').DataTable({
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
      <%--<h1>
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

        <div class="col-md-12">
          <div class="nav-tabs-custom">
            <ul class="nav nav-tabs">
              <li class="active" runat="server" id="tab1"><a href="#activity" data-toggle="tab">Lista de Usuarios</a></li>
              <li  runat="server" id="tab2"><a href="#timeline" data-toggle="tab">Usuario</a></li>
            </ul>
            <div class="tab-content">
              <div class="active tab-pane" runat="server" id="activity" clientidmode="Static">
                <div class="form-horizontal">
                    <asp:PlaceHolder ID="plhDeleteQuestion" runat="server" Visible="false">
                        <div class="alert alert-warning alert-dismissible">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            <h4><i class="icon fa fa-warning"></i> Alerta!</h4>
                                ¿Estas seguro de eliminar el usuario?
                                <asp:Button runat="server" ID="btnEliminar" Text="Aceptar" CssClass="btn btn-block btn-warning" onclick="btnEliminar_Click" />
                                <asp:Literal ID="ltHidden" runat="server" Visible="false"></asp:Literal>
                          </div>
                    </asp:PlaceHolder>
                    <table id="example2" class="table table-striped table-bordered nowrap" style="width:100%">
                        <thead>
                            <tr>
                                <th>Nombre y Apellido</th>
                                <th>Editar</th>
                                <th>Eliminar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptGrid" onitemcommand="rptGrid_ItemCommand">
                                <ItemTemplate>
                                     <tr>
                                          <td><%# Eval("Nombre") %>&nbsp;(<%# Eval("Email") %>)</td>
                                          <td><asp:LinkButton ID="lbEdit" runat="server" CommandName="edit"  CssClass="btn btn-app"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                                            <i class="fa fa-edit"></i> Editar
                                                            </asp:LinkButton></td>
                                          <td><asp:LinkButton ID="lbDelete" runat="server" ClientIDMode="Static" CommandName="delete"  CssClass="btn btn-app"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'>
                                                            <i class="fa fa-barcode"></i> Eliminar
                                                         </asp:LinkButton></td>
                                        </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                   </table>
                </div>
              </div>
              <!-- /.tab-pane -->
              <div class="tab-pane" runat="server" id="timeline" clientidmode="Static">
                <div class="box-primary">
                    <div class="form-horizontal" runat="server">
                        <div class="box-body">
                            <div class="form-horizontal">
                              <h3 class="box-title">Datos Usuarios</h3>
                            </div>
                            <uc:usuarios runat="server" ID="user" />                   
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
            </div>
            <!-- /.tab-content -->
          </div>
          <!-- /.nav-tabs-custom -->
        </div>

</div>
</form>
</asp:Content>
