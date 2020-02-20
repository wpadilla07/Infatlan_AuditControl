<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infatlan_AuditControl._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">Home</a>
            </li>
            <li class="active">Dashboard</li>
        </ul>
        <!-- /.breadcrumb -->

        <div class="nav-search" id="nav-search">
            <form class="form-search">
                <span class="input-icon"></span>
            </form>
        </div>
        <!-- /.nav-search -->
    </div>

    <div class="page-content">
        <div class="ace-settings-container" id="ace-settings-container">
            <div class="btn btn-app btn-xs btn-success ace-settings-btn" id="ace-settings-btn">
                <i class="ace-icon fa fa-bug bigger-130"></i>
            </div>
        </div>

        <div class="page-header">
            <h1>Dashboard
				<small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    graficas &amp; estadisticas
                </small>
            </h1>
        </div>

        <div class="alert alert-block alert-success">
            <button type="button" class="close" data-dismiss="alert">
                <i class="ace-icon fa fa-times"></i>
            </button>

            <i class="ace-icon fa fa-check green"></i>

            Bienvenido a
            <strong class="green">AuditControl
                <small>(v1.0)</small>
            </strong>de Auditoria Interna - versión Beta - En desarrollo 
        </div>

        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-green">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tasks"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Literal ID="LitTotalInformes" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Informes</div>
                </div>

            </div>

            <div class="infobox infobox-blue">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalHallazgos" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Hallazgos</div>
                </div>


            </div>

            <div class="infobox infobox-pink">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-users"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalUsuarios" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Usuarios</div>
                </div>
               
            </div>

            <div class="infobox infobox-red" id="DivComentarios" runat="server" visible="false">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-comments"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalComentarios" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Comentarios</div>
                </div>
            </div>

            <div class="infobox infobox-orange2">
                <div class="infobox-chart">
                    <span class="sparkline" data-values="196,128,202,177,154,94,100,170,224"></span>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalInformesFinalizados" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Informes Finalizados</div>
                </div>


            </div>

            <div class="infobox infobox-blue2">
                <div class="infobox-progress">
                    <div class="easy-pie-chart percentage" data-percent="10" data-size="46">
                        <span class="percent">0</span>
                    </div>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalHallazgosFinalizados" runat="server"></asp:Literal></span>

                    <div class="infobox-content">
                        Hallazgos Finalizados
                    </div>
                </div>
            </div>

            <div class="space-6"></div>


        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
