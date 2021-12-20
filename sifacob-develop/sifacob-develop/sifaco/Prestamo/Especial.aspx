<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Especial.aspx.cs" Inherits="sifaco.Prestamo.Especial" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="datosPrestamos" Src="~/Controls/DatosPrestamo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datosSim" Src="~/Controls/Simulacion.ascx" %>
<%@ Register TagPrefix="uc" TagName="toPrint" Src="~/Controls/DocumentsPresToPrint.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
.oculto{display:none;}
</style>
<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script src="../Scripts/jquery.PrintArea.js_4.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function enviar() {

        var retorno = true;

        if (document.getElementById("txtMonto").value == "") {
            alert("Debe ingresar el MONTO del Prestamo");
            return false;
        }


        if (document.getElementById("txtPlazo").value == "" || document.getElementById("txtPlazoFac").value == "0") {
            alert("Debe ingresar el PLAZO del Prestamo");
            return false;
        }

        if (document.getElementById("txtNumCuotas").value == "" || document.getElementById("txtNumCuotas").value == "0") {
            alert("Debe ingresar el NUMERO DE CUOTAS del Prestamo");
            return false;
        }


        if (parseFloat(document.getElementById("txtTasa").value) <= 0) {
            alert("La TASA de la simulacion debe ser mayor a cero (0)");
            return false;
        }

        return retorno;

    }

    function calc() {
        var tasa = $("#txtTasa");
        var plazo = $("#txtPlazo");
        var numCuotas = $("#txtNumCuotas");
        var monto = $("#txtMonto");
        var cuota = $("#txtCuota");
        var plazoCeil = numCuotas.val(); //Math.ceil(plazo.val() / 30);
        if (tasa.val() == "")
            tasa.val(0);

        var tasa12 = parseFloat(tasa.val().replace(/\,/g, ".") / 100);

        var a = (parseFloat(stripDots(monto.val()).replace(/\,/g, ".")) * tasa12) * Math.pow((1 + tasa12), plazoCeil);
        var b = Math.pow((1 + tasa12), plazoCeil) - 1;
        cuota.val(parseFloat(a / b).toFixed(2).replace(/\./g, ","));

        formatomiles("txtMonto");
        formatomiles("txtCuota");
    }

    function stripDots(n) {
        var z = n.replace(/\./g, "");
        return z;
    }

    $(document).ready(function () {
        calc();

        $("#btnGuardarFac").click(function () {
            if (enviar() == false)
                return false;
            else {
                return true;
            }
        });

        $("#txtTasa").blur(function () {
            calc();
        });

        $("#txtPlazo").blur(function () {
            calc();
        });
        $("#txtNumCuotas").blur(function () {
            calc();
        });
        $("#txtMonto").blur(function () {
            calc();
        });

    });

    function SaveValue(idFac, optVal, edo) {
        if (optVal == 3) {
            $("#datePick" + idFac).show();
            $("#btnGuar" + idFac).show();
        } else {
            var url = "?aid=" + idFac + "&val=" + optVal + "&edo=" + edo;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                    /*alert(d.Sold);
                    alert(d.Month);*/
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    /*alert(errorThrown);
                    alert(textStatus);
                    alert(jqXHR);*/
                    location.reload();
                }
            });
        }
    }

    function SaveValuePay(idFac, optVal, optValPay, edo) {

        if (optValPay == "" && edo == "3") {
            alert("El campo fecha no debe estar vacío");
            return false;
        }
        else {
            var url = "?aid=" + idFac + "&val=" + optVal + "&val2=" + optValPay + "&edo=" + edo;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    }

    function SaveValueObser(idFac, flg, obser) {
        var url = "?aid=" + idFac + "&flg=" + flg + "&obser=" + $.trim(obser);

        var success = function (data) {
            var html = [];
            data = $.parseJSON(data);
            $.each(data, function (index, d) {
                location.reload();
            });
        };

        $.ajax({
            type: 'GET',
            url: url,
            data: { todo: "jsonp" },
            dataType: "jsonp",
            crossDomain: true,
            cache: false,
            success: success,
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

    function SaveValueCuota(idPres, valCuota, idAmor, numC, tasa) {

        /*alert("idPres " + idPres);
        alert("valCuota " + valCuota);
        alert("idAmor " + idAmor);
        alert("tasa " + tasa);*/
        tasa = $('#txtTasa').val();
        if (valCuota == "") {
            alert("El valor de la cuota no debe estar vacío");
            return false;
        }
        else {
            var url = "Especial.aspx?prid=" + idPres + "&valc=" + valCuota + "&amid=" + idAmor + "&num=" + numC + "&tasa=" + tasa;

            var success = function (data) {
                var html = [];
                data = $.parseJSON(data);
                $.each(data, function (index, d) {
                    location.reload();
                });
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: { todo: "jsonp" },
                dataType: "jsonp",
                crossDomain: true,
                cache: false,
                success: success,
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#txtCuotaEspecial').show();
        $('#txtCuota').hide();
    });
    //txtCuotaEspecial

    $(function () {
        $('#example2').DataTable({
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
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderSection" runat="server">
<section class="content-header">
<%--      <h1>
        Prestamo Especial
        <small>Tabla Amortización</small>
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
            <div id="Div1" class="form-horizontal" runat="server">
                <div class="box-body">
                   <div class="box-header with-border">
                        <h3 class="box-title">Prestamo Especial</h3>
                    </div>
                    <br />
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
                      <h3 class="box-title">Datos Prestamo</h3>
                    </div>
                    <div class="row">
                        <uc:datosPrestamos runat="server" ID="pres" />
                        <div class="col-xs-6">
                        <br />
                            <asp:Button runat="server" ID="btnModificarFac" ClientIDMode="Static"
                                CssClass="btn btn-info pull-right" Text="Guardar Prestamo" 
                                onclick="btnModificarFac_Click" />
                        </div>
                        <div class="col-xs-6">
                        <br />
                        <asp:Button runat="server" ID="btnPrestamoEspecial" ClientIDMode="Static"
                                CssClass="btn btn-warning  pull-right" Text="Ir a Prestamos>>>" 
                                onclick="btnPrestamoEspecial_Click" />
                        </div>
                    </div>

                    </div>
           <br />
           <div class="box-body table-responsive no-padding">
              <table class="table table-hover">
                <tr>
                  <th align="center" style="width:5%;">N°</th>
                  <th style="width:15%;">Saldo Inicial</th>
                  <th align="center">Cuotas</th>
                  <th align="center">Intereses</th>
                  <th align="center">Capital</th>
                  <th align="center">Saldo Final</th>
                  <th align="center">Estado</th>
                  <th align="center">Vencimiento</th>
                  <th align="center">Mora</th>
                  <th align="center">Fecha Pago</th>
                  <th align="center">Observación</th>
                </tr>
                <asp:Repeater runat="server" ID="rptPrestamos" >
                <ItemTemplate>
                    <tr <%# (Eval("IdEdoPres").ToString()=="2")?"style='background:red;color:white;'":(Eval("IdEdoPres").ToString()=="3")?"style='background:green;color:white;'":(Eval("IdEdoPres").ToString()=="4")?"style='background:orange;color:black;'":(Eval("IdEdoPres").ToString()=="5")?"style='background:violet;color:white;'":(Eval("IdEdoPres").ToString()=="6")?"style='background:blue;color:white;'":(Eval("IdEdoPres").ToString()=="7")?"style='background:gray;color:black;'":"style='background:white;color:black;'"%>>
                      <td align="center"><%# Eval("NumCuota")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoInicial").ToString()).ToString("N")%></td>
                      <td><span id="cuot_<%# DataBinder.Eval(Container.DataItem, "ID") %>">$<%#decimal.Parse(Eval("Cuota").ToString()).ToString("N")%></span><span id="txtInput_<%# DataBinder.Eval(Container.DataItem, "ID") %>" style="display:none;"><input style="color:Black;" onkeypress='return soloNum(event, this.id)' onblur='formatomiles(this.id);' type='text' id='cuota_<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='<%#decimal.Parse(Eval("Cuota").ToString()).ToString("N")%>'/><br>
                        <input type='button' id='btnGuarC_<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Re-Calcular' style="color:Black;"/>
                      </span>
                      </td>
                      <td><%#  "$" + decimal.Parse(Eval("Intereses").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("Capital").ToString()).ToString("N")%></td>
                      <td><%#  "$" + decimal.Parse(Eval("SaldoFinal").ToString()).ToString("N")%></td>
                      <td style='color:black;'>
                          <select id='ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='width:110px;'>
                            <option value='1' <%# (Eval("IdEdoPres").ToString()=="1")?"selected='selected'":"" %> style='color:black;'>SIN VENCER</option>
                            <option value='2' <%# (Eval("IdEdoPres").ToString()=="2")?"selected='selected'":"" %> style='background:red;color:white;'>VENCIDA</option>
                            <option value='3' <%# (Eval("IdEdoPres").ToString()=="3")?"selected='selected'":"" %> style='background:green;color:white;'>PAGADA</option>
                            <option value='4' <%# (Eval("IdEdoPres").ToString()=="4")?"selected='selected'":"" %> style='background:orange;color:white;'>PAGO PARCIAL</option>
                            <option value='5' <%# (Eval("IdEdoPres").ToString()=="5")?"selected='selected'":"" %> style='background:violet;color:white;'>PAGADO SIN INTERESES DE MORA</option>
                            <option value='6' <%# (Eval("IdEdoPres").ToString()=="6")?"selected='selected'":"" %> style='background:blue;color:white;'>EN JUICIO</option>
                            <option value='7' <%# (Eval("IdEdoPres").ToString()=="7")?"selected='selected'":"" %> style='background:gray;color:black;'>INCOBRABLE</option>
                          </select>
                          <input type='text' id='datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>' style='display:none;'/>
                          <br>
                          <input type='button' id='btnGuar<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar' style='display:none;'/>
                            <script type='text/javascript'>
                                $(document).ready(function () {
                                    $(function () {
                                        $('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' });
                                        $('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').inputmask();
                                    });
                                    $('#cuot_<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        $('#txtInput_<%# DataBinder.Eval(Container.DataItem, "ID") %>').show();
                                        $('#cuot_<%# DataBinder.Eval(Container.DataItem, "ID") %>').hide();
                                    });
                                    $('#btnGuar<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValuePay(<%# DataBinder.Eval(Container.DataItem, "ID") %>, 3, $('#datePick<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true);
                                    });
                                    $('#btnGuarC_<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValueCuota(<%#DataBinder.Eval(Container.DataItem, "IdPrestamo")%>, $('#cuota_<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(),<%# DataBinder.Eval(Container.DataItem, "ID") %>,<%# DataBinder.Eval(Container.DataItem, "NumCuota") %>, $('#txtTasa').val());
                                    });
                                    $('#btnSaveObser<%# DataBinder.Eval(Container.DataItem, "ID") %>').click(function () {
                                        SaveValueObser(<%# DataBinder.Eval(Container.DataItem, "ID") %>, 'T', $('#txtObser<%# DataBinder.Eval(Container.DataItem, "ID") %>').val());
                                    });
                                    $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').change(function () {
                                        SaveValue(<%# DataBinder.Eval(Container.DataItem, "ID") %>, $('#ddl<%# DataBinder.Eval(Container.DataItem, "ID") %>').val(), true);
                                    });

                                });
                            </script>
                      </td>
                      <td>
                          <%#(Eval("Vencimiento") == null) ? "NA" : Convert.ToDateTime(Eval("Vencimiento")).ToShortDateString()%>
                      </td>
                      <td><%#  "$" + decimal.Parse(Eval("Mora").ToString()).ToString("N")%></td>
                      <td>
                          <%# (Eval("FechaPago")==null)?"NA":Convert.ToDateTime(Eval("FechaPago")).ToShortDateString()%>
                      </td>
                      <td style="color:Black;">
                      <textarea id='txtObser<%# DataBinder.Eval(Container.DataItem, "ID") %>' rows="5" cols="20"><%# (Eval("Observacion") == null)?"":Eval("Observacion").ToString().Trim()%></textarea>
                      <input type='button' id='btnSaveObser<%# DataBinder.Eval(Container.DataItem, "ID") %>' value='Guardar'/>
                      </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
              </table>
            </div>
            <br />
                    <div class="col-md-6" style="width: 35% !important;">
                    <uc:toPrint runat="server" ID="print" />
                    <asp:Button runat="server" ID="btnCrearDocs" ClientIDMode="Static" CssClass="btn btn-info pull-right" 
                              Text="Generar Contratos" onclick="btnCrearDocs_Click" Enabled="true" />
                    </div>

                      <div class="box-footer">
                      <iframe runat="server" id="ifr1" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr2" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr3" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr4" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr5" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr6" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr7" frameborder="0" style="height:10px;"></iframe>
                      <iframe runat="server" id="ifr8" frameborder="0" style="height:10px;"></iframe>
                      </div>
                </div>
            </div>
        </div>
</div>
</form>

</asp:Content>
