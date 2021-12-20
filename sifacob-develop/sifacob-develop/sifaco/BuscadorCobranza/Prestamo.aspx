<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prestamo.aspx.cs" Inherits="sifaco.BuscadorCobranza.Prestamo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/Chart.js"></script>
<asp:Literal runat="server" ID="ltrHead">
<style type="text/css">
.visto
{
    display:none;
    }
</style>
</asp:Literal>
<script type="text/javascript">
    $(function () {
        $("#txtFechaD").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaD").inputmask();
        $("#txtFechaH").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaH").inputmask();
    });

    $(document).ready(function () {
        $("#printBC").click(function () {
            setTimeout(function () { window.print(); }, 1000);
        });    
       
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Generar
        <small>Reportes</small>
      </h1>--%>
      <%--<ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>--%>
</section>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
    <div>
        <div class="box box-primary">
            <div class="box-header with-border">
                        <h3 class="box-title">Generador de reportes de prestamos</h3>
                    </div>
                    <br />
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
                    <div class="box-body no-padding">
                       
                        <div class="col-xs-3">
                            <b>Fecha Desde</b>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="fa fa-calendar"></i>
                                </div>
                                <asp:TextBox runat="server" ID="txtFechaD" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-3">
                            <b>Fecha Hasta</b>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="fa fa-calendar"></i>
                                </div>
                                <asp:TextBox runat="server" ID="txtFechaH" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-3">
                            <b>Buscar fecha en</b>
                            <asp:DropDownList runat="server" ID="ddlEdoFactura" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                                    <asp:ListItem Value="2">Vencidas</asp:ListItem>
                                    <asp:ListItem Value="3">Pagadas</asp:ListItem>
                                    <asp:ListItem Value="6">Pagadas parcial</asp:ListItem>
                                    <asp:ListItem Value="7">Pagadas sin interéses</asp:ListItem>
                                    <asp:ListItem Value="4">No pagadas</asp:ListItem>
                                    <asp:ListItem Value="8">En Juicio</asp:ListItem>
                                    <asp:ListItem Value="9">Incobrable</asp:ListItem>
                                    <asp:ListItem Value="10">Reorganizados</asp:ListItem>
                                    <asp:ListItem Value="11">Pago Solo Capital</asp:ListItem>
                                    <asp:ListItem Value="12">Pago Solo Intereses</asp:ListItem>
                                    <asp:ListItem Value="13">Pago Parcial Mora</asp:ListItem>
                                    <asp:ListItem Value="14">Pago Capital Parcial</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3">
                            <b>Cliente</b>
                            <asp:DropDownList runat="server" ID="ddlCliente" ClientIDMode="Static" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3" style="float:left !important">
                            <br />
                            <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                                    CssClass="btn btn-info pull-right" Text="Buscar" 
                                    onclick="btnBuscar_Click"  />
                        </div>
                        <div class="col-xs-3">
                            <b>Monto Total</b>
                            <asp:TextBox runat="server" ID="txtMTP" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Utilidad Total</b>
                            <asp:TextBox runat="server" ID="txtUTP" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Capital Total</b>
                            <asp:TextBox runat="server" ID="txtCPT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Mora Total</b>
                            <asp:TextBox runat="server" ID="txtMoTP" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b>Girable Total</b>
                            <asp:TextBox runat="server" ID="txtGTP" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-xs-3">
                            <b id="bTagP">Total de Prestamos</b>
                            <asp:TextBox runat="server" ID="txtPT" ClientIDMode="Static" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>

                        <table id="example3" class="table table-striped table-bordered nowrap" style="width:100%">
                            <thead>
                                <tr>
                                    <th align="center" style="width:5% !important;">N° Prestamo</th>
                                    <th>Cliente</th>
                                    <th align="center">Monto</th>
                                    <th align="center">Utilidad</th>
                                    <th align="center">Mora</th>
                                    <th align="center" style="width:5% !important;">Plazo</th>
                                    <th align="center" style="width:5% !important;">Tasa</th>
                                    <th align="center" style="width:5% !important;">N° Cuota</th>
                                    <th align="center" style="width:10% !important;">Operación</th>
                                    <th align="center" style="width:10% !important;">Detalle</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptPrestamo">
                                    <ItemTemplate>
                                        <tr>
                                                <td><%# Eval("ID")%></td>
                                                <td><%# Eval("Nombre")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Intereses").ToString()).ToString("N")%></td>
                                                <td><%#  "$" + decimal.Parse(Eval("Mora").ToString()).ToString("N")%></td>
                                                <td><%# Eval("Plazo")%></td>
                                                <td><%# Eval("Tasa")%></td>
                                                <td><%# Eval("NumTotalCuota")%></td>
                                                <td><%# string.Format("{0:dd-MM-yyyy}", Eval("Fecha"))%></td>
                                                <td>                                              
                                                    <a id="vc<%#Eval("ID").ToString()%>">Ver Cuotas</a>
                                                    <script type="text/javascript">
                                                        $(document).ready(function () {
                                                            $('#vc<%#Eval("ID").ToString()%>').click(function () {
                                                                $('#dc<%#Eval("ID").ToString()%>').toggle();
                                                            });
                                                        });
                                                    </script>
                                                </td>
                                        </tr>
                                        <tr id="dc<%#Eval("ID").ToString()%>" style="display:none">
                                            <td colspan="10">
                                                 <table id="example4" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th align="center" style="width:5% !important;">N° Cuota</th>
                                                            <th align="center">Cuota</th>
                                                            <th align="center">Utilidad</th>
                                                            <th align="center">Capital</th>
                                                            <th align="center">Mora</th>
                                                            <th align="center" style="width:10% !important;">Operación</th>
                                                            <th align="center" style="width:10% !important;">Vencimiento</th>
                                                            <th align="center" style="width:10% !important;">Cobranza/Pago</th>
                                                            <th align="center" style="width:5% !important;">Atraso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <%#CuotasXPrestamos(Eval("ID").ToString())%>
                                                    </tbody>
                                                </table>
                                            </td>                                        
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </tbody>
                        </table>
                        <a href="#" id="printBC" class="sidebar-toggle" data-toggle="offcanvas" role="button">
                            Imprimir
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
</asp:Content>
