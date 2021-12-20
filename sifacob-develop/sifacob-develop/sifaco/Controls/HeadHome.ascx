<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeadHome.ascx.cs" Inherits="sifaco.Controls.HeadHome" %>
<script type="text/javascript">
    $(function () {
        $("#txtFechaD").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaD").inputmask();
        $("#txtFechaH").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
        $("#txtFechaH").inputmask();
    });
</script>

    <!-- Small boxes (Stat box) -->
    <div class="row">
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-red">
                <div class="inner">
                    <h3><asp:Literal runat="server" ID="ltrFacM"></asp:Literal></h3>
                    <p>Facturas con Moras</p>
                </div>
                <div class="icon">
                    <i class="ion ion-flag"></i>
                </div>
                <a href="../FacturasVencer/ClientesMora.aspx" class="small-box-footer">Más Información <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3><asp:Literal runat="server" ID="ltrFacVV"></asp:Literal></h3>
                    <p>Facturas a Vencer/Vencidas</p>
                </div>
                <div class="icon">
                    <i class="ion ion-alert"></i>
                </div>
                <a href="../FacturasVencer/" class="small-box-footer">Más Información <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->

        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua">
                <div class="inner">
                    <h3><asp:Literal runat="server" ID="ltrOpeM"></asp:Literal></h3>
                    <p>Operaciones del Mes</p>
                </div>
                <div class="icon">
                    <i class="ion ion-person-add"></i>
                </div>
                <a href="../BuscadorCobranza/Simulacion.aspx" class="small-box-footer">Más Información <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3><asp:Literal runat="server" ID="ltrCobM"></asp:Literal><sup style="font-size: 20px"></sup></h3>
                    <p>Cobranzas del Mes</p>
                </div>
                <div class="icon">
                    <i class="ion ion-stats-bars"></i>
                </div>
                <a href="../BuscadorCobranza/" class="small-box-footer">Más Información <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
    </div>

<script src="../Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/Chart.js"></script>
<asp:Literal runat="server" ID="ltrHead"></asp:Literal>
          <div class="row">
            <div class="box-body">
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

                <div class="col-xs-3" style="float:right !important; z-index:800">
                    <br />
                    <asp:Button runat="server" ID="btnBuscar" ClientIDMode="Static" 
                            CssClass="btn btn-info" Text="Buscar" 
                            onclick="btnBuscar_Click"  />
                </div>
            </div>
        </div>
          <!-- AREA CHART -->
          <div class="box box-primary">
            <div class="box-header with-border">
              <h3 class="box-title">Capital factorizado</h3>

              <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                </button>
                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
              </div>
            </div>
            <div class="box-body">
              <div class="chart">
                <canvas id="areaChart" style="height:250px"></canvas>
              </div>
            </div>
            <!-- /.box-body -->
          </div>
          <!-- /.box -->


          <!-- AREA CHART -->
          <div class="box box-primary">
            <div class="box-header with-border">
              <h3 class="box-title">Utilidad a ganar
              </h3>

              <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i>
                </button>
                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
              </div>
            </div>
            <div class="box-body">
              <div class="chart">
                <canvas id="areaChart2" style="height:250px"></canvas>
              </div>
            </div>
            <!-- /.box-body -->
          </div>
          <!-- /.box -->

