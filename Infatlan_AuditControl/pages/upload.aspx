<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="Infatlan_AuditControl.pages.upload" %>

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
            <li class="active">Subir Informes</li>
        </ul>
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>Subir Informes
                <small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Si no tienes el formato de subida del archivo por favor descargalo
                </small>
            </h1>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <asp:UpdatePanel ID="UpdateForma" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label no-padding-right" for="FUInforme">Informe</label>
                            <div class="col-sm-4">
                                <asp:FileUpload ID="FUInforme" runat="server" class="form-control" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" />
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir informe con hallazgos">?</span>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label no-padding-right" for="FUInforme">Adjunto</label>
                            <div class="col-sm-4">
                                <asp:FileUpload ID="FUAdjunto" runat="server" class="form-control" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" />
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir archivo de adjunto del informe">?</span>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="form-group row">
                    <label class="col-sm-2 control-label no-padding-right" for="FUInforme"></label>
                    <div class="col-sm-10">
                        <asp:Label ID="LabelMensaje" runat="server" Text="" style="color: indianred;"></asp:Label>
                    </div>
                </div>
                <div class="hr hr-double dotted"></div>
                <div class="clearfix form-actions">
                    <div class="col-md-offset-0 col-md-9">
                        <asp:UpdatePanel ID="UpdateBotones" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="BtnCrearInforme" runat="server" style="border-radius: 4px;" class="btn btn-info" OnClick="BtnCrearInforme_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Crear Informe
                                </asp:LinkButton>
                                <button style="border-radius: 4px;" class="btn" type="reset">
                                    <i class="ace-icon fa fa-undo bigger-110"></i>
                                    Reset
                                </button>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BtnCrearInforme" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
