<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.ToPrint.Default" EnableViewStateMac="false" %>
<%@ Register TagPrefix="uc" TagName="datosSim" Src="~/Controls/Simulacion.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%--    <link href="~/Styles/bootstrap.min.css" rel="stylesheet" type="text/css" media="print" />
    <link rel="stylesheet" href="~/Styles/AdminLTE.min.css" media="print" />
--%>
<script src="../Scripts/jquery.PrintArea.js_4.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("body").removeAttr("class");
        $("body").attr("class", "skin-blue sidebar-mini sidebar-collapse");

        $('#imgDiv').attr("style", "display:block");
        window.resizeTo(screen.width, screen.height);
        var container = $('.print').attr('rel');
        $('#' + container).printArea();
        return false;
    });


   /* function printData() {
        var divToPrint = document.getElementById("Form1");
        newWin = window.open("");
        newWin.document.write(divToPrint.outerHTML);
        newWin.print();
        //newWin.close();
    }

    //$('button').on('click',function(){
    $(document).ready(function () {
        printData();
    })*/

</script>
<style type="text/css" media="print">
#content {
	padding: 20px;
	margin-bottom: 16px;
	background: #fff;
}
.print {
	text-decoration: none;
	text-transform: uppercase;
	padding: 5px 15px;
	border: solid 1px #056fc4;
	/*color: #fff;
	background: url(/images/button.png) repeat-x 0 0;*/
}

@media print{
        #imgDiv
        {
            display:block;
        }
 }
 
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
        <div class="box box-primary" id="idToPrint">
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
                    <div id="content">
                        <div class="row">
                            <table id="example3" class="table table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th style="text-align:center">Cliente</th>
                                        <th style="text-align:center">RUT</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td align="center" style="width:60%"><h3 class="box-title"><asp:Literal runat="server" ID="ltrCliente"></asp:Literal></h3></td>
                                        <td align="center" style="width:40%"><h3 class="box-title"><asp:Literal runat="server" ID="ltrRutCliente"></asp:Literal></h3></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="box-header with-border">
                            <h3 class="box-title">Datos Factura</h3>
                        </div>
                        <br />
                        <div class="box-body table-responsive no-padding">
                            <table class="table table-hover">
                                <tr>
                                    <th align="center" style="width:5%;">Factura</th>
                                    <th style="width:15%;">Deudor</th>
                                    <th align="center">Tipo</th>
                                    <th align="center">Plazo</th>
                                    <th align="center">Monto</th>
                                    <th align="center">Anticipo</th>
                                    <th align="center">Pendiente</th>
                                    <th align="center">Utilidad</th>
                                    <th align="center">Saldo</th>
                                </tr>
                                <asp:Repeater runat="server" ID="rptFacturas">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center"><%# Eval("NumFactura")%></td>
                                            <td><%# Eval("Deudor")%></td>
                                            <td><%# Eval("Tipo").ToString().Substring(0,1)%></td>
                                            <td><%# Eval("Plazo")+"Días"%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("Monto").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoAnticipo").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoPendiente").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("Utilidad").ToString()).ToString("N")%></td>
                                            <td><%#  "$" + decimal.Parse(Eval("MontoGirable").ToString()).ToString("N")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                        <br />
                        <div class="box-header with-border">
                            <h3 class="box-title">Datos Simulación</h3>
                        </div>
                        <table>
                            <tr>
                                <td style="width: 90% !important;padding-left:10%;">
                                    <div id="txtDiv" class="col-md-6" style="width: 100% !important;">
                                        <div class="form-group">
                                            <label for="inputEmail3" class="col-sm-2 control-label">Estado de la simulacion</label>
                                            <div class="col-sm-10">
                                                <asp:DropDownList runat="server" ID="ddlEdoSim" ClientIDMode="Static" CssClass="form-control" Enabled="false" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="-1">-- Seleccione --</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-2 control-label">Fecha de la Simulacion</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" ID="txtFecSimulacion" ClientIDMode="Static" CssClass="form-control" Enabled="false"  ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-2 control-label">Tasa</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" ID="txtTasa" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-2 control-label">Anticipo</label>
                                            <div class="col-sm-10" >
                                                <asp:TextBox runat="server" ID="txtAnticipo" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="95"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-2 control-label">Saldo Pendiente</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" ID="txtSalPendiente" ClientIDMode="Static"  Enabled="false" Text="5" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputPassword3" class="col-sm-2 control-label">Gastos de Operacion</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" ID="txtGasOperacion" ClientIDMode="Static" Enabled="false" CssClass="form-control" Text="0"></asp:TextBox>
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
                                        <a href="#" class="print" rel="content" style="display:none">Print</a>
                                    </div>
                                </td>
                                <td id="imgDiv" style="width: 10% !important;padding-right:20%">
                                    <div class="col-md-6" style="width: 100% !important;">
                                        <img src="../Styles/img/logo_fnnsu.png" width="200" height="143" alt="FINANSU"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

