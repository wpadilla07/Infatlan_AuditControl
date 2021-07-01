<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infatlan_AuditControl._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/assets/css/GridStyle.css" rel="stylesheet" />
    <link href="/assets/css/pager.css" rel="stylesheet" />
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

        <%--<div class="alert alert-block alert-success">
            <button type="button" class="close" data-dismiss="alert">
                <i class="ace-icon fa fa-times"></i>
            </button>

            <i class="ace-icon fa fa-check green"></i>

            Bienvenido a
            <strong class="green">AuditControl
                <small>(v1.0)</small>
            </strong>de Auditoria Interna - versión Beta - En desarrollo 
        </div>--%>

        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-blue">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tasks"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Literal ID="LitTotalInformes" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Total Informes</div>
                </div>
            </div>

            <div class="infobox infobox-orange">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tasks"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Literal ID="LitTotalInformesPendientes" runat="server"></asp:Literal></span>
                    <div class="infobox-content">En Proceso</div>
                </div>

            </div>

            <div class="infobox infobox-green">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tasks"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Literal ID="LitTotalInformesFinalizados" runat="server"></asp:Literal>
                    </span>
                    <div class="infobox-content">Resuelto</div>
                </div>
            </div>
            <div class="space-6"></div>
        </div>
        
        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-blue">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalHallazgos" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Total Hallazgos</div>
                </div>
            </div>

            <div class="infobox infobox-green">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitTotalHallazgosFinalizados" runat="server"></asp:Literal></span>

                    <div class="infobox-content">
                        Hallazgos Finalizados
                    </div>
                </div>
            </div>
            
            <div class="infobox infobox-red">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitHallazgosVencidos" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Vencidos</div>
                </div>
            </div>
            
            <div class="space-6"></div>
        </div>

        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-orange">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitIngresados" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Ingresados</div>
                </div>
            </div>

            <div class="infobox infobox-orange">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitValidacion" runat="server"></asp:Literal></span>
                    <div class="infobox-content">Validación</div>
                </div>
            </div>

            <div class="infobox infobox-orange">
                <div class="infobox-icon">
                    <i class="ace-icon fa fa-tags"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number"><asp:Literal ID="LitEnProceso" runat="server"></asp:Literal></span>
                    <div class="infobox-content">En Proceso</div>
                </div>
            </div>
        </div>
        <div class="space-6"></div>
    </div>

    
    <div class="page-content" style="margin-top:200px;">
        <div class="page-header">
            <h1>Hallazgos
                <small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Hallazgos asignados
                </small>
            </h1>
        </div>
        <div class="row" style="margin-top:2px;">
            <div class="col-xs-12">
                <asp:GridView ID="GVHallazgos" runat="server"
                    CssClass="mydatagrid"
                    PagerStyle-CssClass="pager"
                    HeaderStyle-CssClass="header"
                    RowStyle-CssClass="rows"
                    GridLines="None"
                    AutoGenerateColumns="false" OnPageIndexChanging="GVHallazgos_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="idHallazgo" HeaderText="ID" />
                        <asp:BoundField DataField="detalle" HeaderText="Detalle" HeaderStyle-Width="600px"/>
                        <asp:BoundField DataField="accion" HeaderText="Accion" HeaderStyle-Width="500px"/>
                        <asp:BoundField DataField="informe" HeaderText="Informe" />
                        <asp:BoundField DataField="estadoHallazgo" HeaderText="Estado" />
                    </Columns>
                </asp:GridView>
                        
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
