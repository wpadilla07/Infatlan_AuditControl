<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="creports.aspx.cs" Inherits="Infatlan_AuditControl.pages.creports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/assets/css/GridStyle.css" rel="stylesheet" />
    <link href="/assets/css/pager.css" rel="stylesheet" />
    <link href="/assets/css/breadcrumb.css" rel="stylesheet" />
    <link href="/assets/css/fstdropdown.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <link href="/assets/css/checkboxes.css" rel="stylesheet" />
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

            <li><a href="#">Pages</a></li>
            <li class="active">Crear Informes</li>
        </ul>
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>Crear Informes
                <small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Ingresa todos los campos a continuación
                </small>
            </h1>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Responsable </label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="DDLUserResponsable" class="fstdropdown-select form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdateForma" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Tipo </label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="DDLTipoResponsable" class="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable"></label>
                            <div class="col-md-8">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="BtnAgregarResponsable" runat="server" style="border-radius: 4px;" class="btn btn-success" OnClick="BtnAgregarResponsable_Click">
                                            <i class="ace-icon fa fa-user-plus bigger-110"></i>
                                            Agregar Responsable
                                        </asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>

                        <div class="form-group ">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable"></label>
                            <div class="col-md-8">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVResponsables" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        GridLines="None"
                                        AutoGenerateColumns="false" OnRowCommand="GVResponsables_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    Acción
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button ID="BtnResponsableDelete" runat="server" Text="borrar" style="border-radius: 4px;" class="btn btn-danger mr-2" CommandArgument='<%# Eval("usuarioResponsable") %>' CommandName="DeleteRow" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="usuarioResponsable" HeaderText="Usuario" />
                                            <asp:BoundField DataField="envio" HeaderText="Envio" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="form-group ">
                            <label class="col-sm-2 control-label no-padding-right" for="TxNombreInforme">Informe </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxNombreInforme" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Nombre descriptivo del informe a crear">?</span>
                        </div>

                        <div class="form-group ">
                            <label class="col-sm-2 control-label no-padding-right" for="TxNombreInforme">Fecha Respuesta </label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="TxFechaRespuesta" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Fecha para cuando tiene que esta finalizado el informe">?</span>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label no-padding-right" for="FUInforme">Adjuntar Informe</label>
                            <div class="col-sm-4">
                                <asp:FileUpload ID="FUInforme" runat="server" class="form-control" accept="application/pdf" />
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir archivos relacionados al hallazgo">?</span>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label no-padding-right" for="CBEstadoCerrado">Estado Cerrado</label>
                            <div class="col-sm-4">
                                <label class="container">
                                    <input type="checkbox" runat="server" id="CBEstadoCerrado"><span class="checkmark"></span>
                                </label>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label no-padding-right" for="CBEstado"></label>
                            <div class="col-sm-4">
                                <asp:Label ID="LbMensajeCrearInforme" runat="server" Text="" style="color:indianred"></asp:Label>
                            </div>
                        </div>

                        <div class="page-header">
                            <h1>Descripción del Informe
                            <small>
                                <i class="ace-icon fa fa-angle-double-right"></i>
                                Por favor describa de que trata este informe
                            </small>
                            </h1>
                        </div>

                        <asp:TextBox ID="TxDescripcionInforme" runat="server" class="form-control" TextMode="MultiLine" Style="height: 120px" placeholder="Descripción (Opcional)"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="hr hr-double dotted"></div>

                <div class="clearfix form-actions">
                    <div class="col-md-offset-0 col-md-9">
                        <asp:UpdatePanel ID="UpdateBotones" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="BtnCrearInforme" runat="server" style="border-radius: 4px;" class="btn btn-info" OnClick="BtnCrearInforme_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>Crear Informe
                                </asp:LinkButton>
                                <button style="border-radius: 4px;" class="btn" type="reset">
                                    <i class="ace-icon fa fa-undo bigger-110"></i>Reset
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
    <script src="/assets/js/fstdropdown.js"></script>
</asp:Content>
