<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="findings.aspx.cs" Inherits="Infatlan_AuditControl.pages.findings" %>
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
            <li class="active">Hallazgos</li>
        </ul>
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>Resolución Hallazgos
                <small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Revión de Hallazgo
                </small>
            </h1>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <asp:UpdatePanel ID="UpdateForma" runat="server">
                    <ContentTemplate>


                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Detalle </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoDetalle" runat="server" class="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Riesgo </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoRiesgo" runat="server" class="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Recomendaciones </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoRecomendaciones" runat="server" class="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>                        
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Plan de Acción </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoAccion" runat="server" class="form-control" TextMode="MultiLine" ></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Ingresar acción a ejecutar para resolver el hallazgo">?</span>
                        </div>
                        <div class="form-group" runat="server" visible="true" >
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Comentarios </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoComentarios" runat="server" class="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Comentarios finales del hallazgos">?</span>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Fecha Resolución </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoFechaResolucion" runat="server" class="form-control" TextMode="Date" ></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Fecha en la cual se comprometen a tener resuelto el hallazgo">?</span>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Anexo </label>
                            <div class="col-sm-8">
                                <asp:LinkButton ID="BtnDescargarAnexo" runat="server" class="btn btn-info" OnClick="BtnDescargarAnexo_Click" >
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Descargar
                                </asp:LinkButton>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Descargar archivo del hallazgo">?</span>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnDescargarAnexo" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="hr hr-double dotted"></div>

                <div class="clearfix form-actions">
                    <div class="col-md-offset-0 col-md-9">
                        <asp:UpdatePanel ID="UpdateBotones" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="BtnVolver" runat="server" class="btn btn-info" OnClick="BtnVolver_Click" >
                                        <i class="ace-icon fa fa-arrow-left bigger-110"></i>
                                        Volver
                                </asp:LinkButton>
                                <asp:LinkButton ID="BtnModificarHallazgo" runat="server" class="btn btn-success" OnClick="BtnModificarHallazgo_Click" >
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Responder Hallazgo
                                </asp:LinkButton>
                                Recuerde que la información es para unico uso de Banco Atlantida.
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
