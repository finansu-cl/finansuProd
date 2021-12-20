<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Error.Default" EnableViewStateMac="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="contentHeaderSection" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
  <%--    <h1>
        500 Error Page
      </h1>
      <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li><a href="#">Examples</a></li>
        <li class="active">500 error</li>
      </ol>--%>
    </section>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Main content -->
    <section class="content">

      <div class="error-page">
        <h2 class="headline text-red">500</h2>

        <div class="error-content">
          <h3><i class="fa fa-warning text-red"></i> Oops! ha ocurrido un error.</h3>

          <p>
            Si el problema persiste enviar un correo al administrador del software.
            Por ahora, puede <a href="../Home/">regresar al inicio</a> para usar otras funcionalidades.
          </p>
        </div>
      </div>
      <!-- /.error-page -->

    </section>
    <!-- /.content -->
</asp:Content>