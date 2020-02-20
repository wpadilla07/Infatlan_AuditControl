<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="files.aspx.cs" Inherits="Infatlan_AuditControl.pages.files" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/assets/css/GridStyle.css" rel="stylesheet" />
    <link href="/assets/css/pager.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #ffffff; opacity: 0.7; margin: 0;">
                <span style="display: inline-block; height: 100%; vertical-align: middle;"></span>
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/assets/images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="display: inline-block; vertical-align: middle;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="/default.aspx">Home</a>
            </li>

            <li>
                <a href="#">Pages</a>
            </li>
            <li class="active">Documentos</li>
        </ul>
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>Documentos
                <small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Busca el informe 
                </small>
            </h1>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-group ">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">No.Informe</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="TxBuscarIdInforme" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable"></label>
                            <div class="col-md-8">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="BtnBuscarInforme" runat="server" style="border-radius: 4px;" class="btn btn-success" OnClick="BtnBuscarInforme_Click">
                                            <i class="ace-icon fa fa-search bigger-110"></i>
                                            Buscar Informe
                                        </asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <asp:UpdatePanel ID="UpdateForma" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GVBusqueda" runat="server"
                            CssClass="mydatagrid"
                            PagerStyle-CssClass="pgr"
                            HeaderStyle-CssClass="header"
                            RowStyle-CssClass="rows"
                            AutoGenerateColumns="false"
                            AllowPaging="true"
                            GridLines="None"
                            PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging" OnRowCommand="GVBusqueda_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UpdateForma" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="BtnDescargar" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-primary" CommandArgument='<%# Eval("idArchivo") %>' CommandName="DescargarArchivo">
                                            <i class="fa fa-download"> Descargar</i>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnDescargar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idArchivo" HeaderText="No.Archivo" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre Archivo" />
                                <asp:BoundField DataField="fechaCreacion" HeaderText="Creación" />
                                <asp:BoundField DataField="creado" HeaderText="Creado" />
                                <asp:BoundField DataField="hallazgo" HeaderText="En relación" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="hr hr-double dotted"></div>

                <div class="clearfix form-actions">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:UpdatePanel ID="UpdateBotones" runat="server">
                            <ContentTemplate>
                                Nota: ten cuidado con la modificación de información por temas de auditoria interna, cualquier cambio hecho al sistema se grabara en un log de sistema.
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
