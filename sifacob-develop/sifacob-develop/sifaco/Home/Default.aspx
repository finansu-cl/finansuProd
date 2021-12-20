<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sifaco.Home.Default" EnableViewStateMac="false"%>
<%@ Register TagPrefix="uc" TagName="home" Src="~/Controls/HeadHome.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="content3" ContentPlaceHolderID="contentHeaderSection" runat="server">
<%--<section class="content-header">
      <h1>
        Inicio
        <small>sifaco</small>
      </h1>
      <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>
</section>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="Form1" class="form-horizontal" runat="server" clientidmode="Static">
    <uc:home runat="server" ID="ucHome" ></uc:home>
</form>
</asp:Content>
