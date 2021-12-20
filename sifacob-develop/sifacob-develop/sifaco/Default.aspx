<%@ Page Title="Página principal" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco._Default" EnableViewStateMac="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
  <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
  <!-- Bootstrap 3.3.5 -->
  <link rel="stylesheet" href="~/Styles/bootstrap.min.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="~/Styles/AdminLTE.min.css"/>
  <!-- iCheck -->
  <link rel="stylesheet" href="~/Styles/blue.css"/>

  <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
  <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
  <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
  <!-- jQuery 2.1.4 -->
<script type="text/javascript" src="Scripts/jQuery-2.1.4.min.js"></script>
<!-- Bootstrap 3.3.5 -->
<script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
<!-- iCheck -->
<script type="text/javascript" src="Scripts/icheck.min.js"></script>
<script src="Scripts/validaciones.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });
    });

    $("#btnLogin").click(function () {
        if (enviar() == false)
            return false;
        else {
            return true;
        }
    });

    function enviar() {

        var retorno = true;

        if (document.getElementById("txtUser").value == "") {
            alert("Debe completar todos los campos");
            return false;
        }


        if (!esEmail(document.getElementById("txtUser"), "E-mail", true, true)) {
            return false;
        }


        if (document.getElementById("txtPwd").value == "") {
            alert("Debe completar todos los campos");
            return false;
        }

    }

</script>

</head>
    <body class="hold-transition login-page">
    <div class="page-container">

        <!-- bloc-0 -->
        <div align="center">
            <div class="bloc bgc-white bg-img-bkg l-bloc none bloc-fill-screen " id="bloc-0">
                <div class="container">
                    <div class="row log-izq  row-no-gutters" style="background: white;opacity: 0.9;border-radius:2%;">
                         <form id="form1" runat="server">
                            <div align="center">
                                <div class="col-sm-6">
                                    <div class="row voffset">
                                        <div class="col-sm-12">
                                            <h3 class="mg-md2 text-center ">
                                                Ingresar
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3 class="mg-md ">
                                                Email
                                            </h3>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox runat="server" ID="txtUser" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                <span class="glyphicon glyphicon-user form-control-feedback"></span>
                                                <%-- @Html.ValidationMessageFor(m => m.Rut, "", new { @class = "text-danger" })--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3 class="mg-md ">
                                                Clave
                                            </h3>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox runat="server" ID="txtPwd" CssClass="form-control" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                                                <%-- @Html.ValidationMessageFor(m => m.Password, "", new { @class = "text-danger" })--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row voffset">
                                        <div class="col-sm-12">
                                            <asp:Button runat="server" ID="btnLogin" 
                                                CssClass="btn btn-cerulean-blue btn-lg btn-block" ClientIDMode="Static" Text="Ingresar" 
                                                onclick="btnLogin_Click" />
                                            <div class="text-center">
                                                <%--<a href="@Url.Action("ForgotPassword","Account")" class=" btn btn-cerulean-blue btn-lg btn-block">¿Olvidaste tu clave?</a>--%>

                                            </div>
                                            <div class="row voffset">
                                                <div class=" col-sm-12">
                                                   <%-- <img src="Styles/img/Sigesov.svg" class="img-responsive pull-right" />--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                       
                        <div class="col-sm-6">
                            <div align="right">
<%--                                <img src="Styles/img/logoLogin.png" style="position: absolute;z-index: 5000;top: 5%;left: 30%;" />--%>
                                <img src="Styles/img/img_log.png" class="img-responsive" style="border-bottom-right-radius: 2%;border-top-right-radius: 2%;/*opacity:0.5;*/height:482px;/*position: relative;z-index: 8;top: 0;left: 0;*/" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- bloc-0 END -->
            <!-- ScrollToTop Button -->
           <%-- <a class="bloc-button btn btn-d scrollToTop" onclick="scrollToTarget('1')"><span class="fa fa-chevron-up"></span></a>--%>
            <!-- ScrollToTop Button END-->
            <!-- Footer - bloc-1 -->
            <div class="bloc bgc-charcoal none d-bloc" id="bloc-1">
                <div class="container bloc-sm">
                    <div class="row">
                        <div class="col-sm-6">
                            <h3 class="mg-md bloc-mob-center-text ">
                                ©2020 by William Sumar
                            </h3>
                        </div>
                        <div class="col-sm-3">
                            <h3 class="mg-md3  text-right">
                                Encuéntranos
                            </h3>
                        </div>
                        <div class="col-sm-1">
                            <a href="#" target="_blank">
                                <img src="Styles/img/facebok.svg" class="img-responsive" />
                            </a>
                        </div>
                        <div class="col-sm-1">
                            <a href="#" target="_blank">
                                <img src="Styles/img/ico isntagram_02.svg" class="img-responsive" />
                            </a>
                        </div>
                        <div class="col-sm-1">
                            <a href="#" target="_blank">
                                <img src="Styles/img/ico linkedin02.svg" class="img-responsive" />
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Footer - bloc-1 END -->
    </div>

    <!-- /.login-box -->
    <!-- jQuery 2.1.4 -->
    <script src="Scripts/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="Scripts/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="Scripts/icheck.min.js"></script>

</body>
</html>
