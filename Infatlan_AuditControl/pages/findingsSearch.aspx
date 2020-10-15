<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="findingsSearch.aspx.cs" Inherits="Infatlan_AuditControl.pages.findingsSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/assets/css/GridStyle.css" rel="stylesheet" />
    <link href="/assets/css/pager.css" rel="stylesheet" />
    <link href="/assets/css/fstdropdown.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>

    <script type="text/javascript">
        function openModalHallazgos() {
            $('#ModificacionesModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openModificacionesEstadoModal() {
            $('#ModificacionesEstadoModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openAutorizacionEstadoModal() {
            $('#AutorizacionModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openFinalizarHallazgoModal() {
            $('#FinalizarHallazgoModal').modal('show');
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

            <li><a href="#">Pages</a></li>
            <li class="active">Buscar / Modificar Hallazgos</li>
        </ul>
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>Buscar / Modificar Hallazgos
				<small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Hallazgos creados a la fecha por area
                </small>
            </h1>
        </div>
        <div class="row">
            <div class="col-xs-12">

                <!-- PAGE CONTENT BEGINS -->
                <div class="form-group ">
                    <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">No.Informe</label>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="DDLBuscarInforme" runat="server" class="fstdropdown-select form-control"></asp:DropDownList>
                        <%--<asp:TextBox ID="TxBuscarIdInforme" runat="server" class=" form-control"></asp:TextBox>--%>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-group" runat="server" visible="false">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserRevision">Nombre </label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="TxBuscarNombre" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable"></label>
                            <div class="col-md-8">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="BtnBuscarHallazgo" runat="server" Style="border-radius: 4px;" class="btn btn-success" OnClick="BtnBuscarHallazgo_Click">
                                            <i class="ace-icon fa fa-search bigger-110"></i>
                                            Buscar Hallazgos
                                        </asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <hr />
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
                            PageSize="50" OnPageIndexChanging="GVBusqueda_PageIndexChanging" OnRowCommand="GVBusqueda_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" ShowHeader="true">
                                    <HeaderTemplate>Auth</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnAutorizarInforme" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-info2" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="AutorizarEstadoHallazgo">
                                            <i class="fa fa-check"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnModificar" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-yellow" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="ModificarEstadoHallazgo">
                                            <i class=" fa fa-cogs "></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnFinalizarHallazgo" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-yellow" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="FinalizarHallazgo">
                                            <i class=" fa fa-check "> Cerrar Hallazgo</i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idHallazgo" HeaderText="No.Hallazgo" />
                                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                <asp:BoundField DataField="idArea" HeaderText="Area" />
                                <asp:BoundField DataField="tipoRiesgo" HeaderText="Riesgo" />
                                <asp:BoundField DataField="detalle" HeaderText="Detalle" />
                                <asp:BoundField DataField="tipoEstadoHallazgo" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEntrar" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-success" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="EntrarHallazgo">
                                            <i class="fa fa-pencil"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnAsignar" runat="server" Text="Entrar" Style="border-radius: 4px;" class="btn btn-info" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="AsignarUsuario">
                                            <i class="fa fa-user-plus "></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
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

    <!-- MODAL MODIFICACIONES INFORME -->
    <div class="modal fade" id="ModificacionesModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 800px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdateModificacionesLabel" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelModificaciones">Modificar - Hallazgo No.
                                    <asp:Label ID="LbNumeroHallazgoModificaciones" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificacionesMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="page-header">
                                <h1>Modificar Hallazgo
                            <small>
                                <i class="ace-icon fa fa-angle-double-right"></i>
                                Asignacion de Usuario a Hallazgo</small>
                                </h1>
                            </div>
                            <div class="col-xs-12">
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Usuario </label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="DDLUsuariosAsignacionHallazgo" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateModificacionesMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div class="form-group row">
                                <asp:Label ID="LbModificacionesMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateModificacionesBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnModificarHallazgo" runat="server" Text="Asignar" Style="border-radius: 4px;" class="btn btn-primary" OnClick="BtnModificarHallazgo_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL MODIFICACIONES HALLAZGO -->
    <div class="modal fade" id="ModificacionesEstadoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelModificacionesEstado">Modificar - Hallazgo No.
                                <asp:Label ID="LbNumeroHallazgoModificacionesEstado" runat="server" Text=""></asp:Label>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="page-header">
                                <h1>Modificar Hallazgo
                                <small>
                                    <i class="ace-icon fa fa-angle-double-right"></i>Estado del informe (Ten cuidado seleccionando los estados) 
                                </small>
                                </h1>
                            </div>

                            <div runat="server" id="DivCierre" visible="false">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLModificarHallazgoEstado">
                                            Comentario
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:Label Text="" runat="server" ID="TxComentarioCierre" CssClass="control-label"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="DivFile" visible="false">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLModificarHallazgoEstado">
                                            Descargar
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:LinkButton Text="" ID="LBArchivo" runat="server" OnClick="LBArchivo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12">
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label no-padding-right" for="DDLModificarHallazgoEstado">
                                        Estado Hallazgo
                                    </label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="DDLModificarHallazgoEstado" class="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DDLModificarHallazgoEstado_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div runat="server" visible="false" id="DivComentario">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLModificarHallazgoEstado">
                                            Comentario
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:TextBox runat="server" ID="TxComentarioAuditor" CssClass="form-control" TextMode="MultiLine" Rows="3"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbHallazgoEstadoMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnModificarEstadoHallazgo" runat="server" Text="Cambiar Estado" Style="border-radius: 4px;" class="btn btn-primary" OnClick="BtnModificarEstadoHallazgo_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="LBArchivo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL MODIFICACIONES HALLAZGO FINALIZACION-->
    <div class="modal fade" id="FinalizarHallazgoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 800px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelFinalizarHallazgo">Modificar - Hallazgo No.
                                    <asp:Label ID="LbFinalizarHallazgo" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="page-header">
                                <h1>Finalizar Hallazgo
                                    <small>
                                        <i class="ace-icon fa fa-angle-double-right"></i>Estas finalizando el hallazgo 
                                    </small>
                                </h1>
                            </div>
                            <div class="col-xs-12">
                                <asp:UpdatePanel ID="UpdateComentario" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group row">
                                            <label class="col-sm-2 control-label no-padding-right" for="DDLModificarHallazgoEstado">
                                                Comentario
                                            </label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="TxFinalizarHallazgoComentario" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="form-group row">
                                    <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Documento</label>
                                    <div class="col-sm-6">
                                        <asp:FileUpload ID="FUHallazgos" runat="server" class="form-control" />

                                    </div>
                                    <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir archivos relacionados a la finalización del hallazgo">?</span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div class="form-group row">
                                <asp:Label ID="LbMensajeCierreHallazgo" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnFinalizarHallazgo" runat="server" Text="Finalizar Hallazgo" Style="border-radius: 4px;" class="btn btn-success" OnClick="BtnFinalizarHallazgo_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnFinalizarHallazgo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    
    <%--MODAL DE AUTORIZACION--%>
    <div class="modal fade" id="AutorizacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelAutorizacion">Revisión de Hallazgo No.
                        <asp:Label ID="LbAutorizacionHallazgo" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <label class="control-label">¿Estas seguro de autorizar este cambio de estado?</label>
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Label Text="Estado solicitado:" runat="server" CssClass="col-lg-4"/>
                                        <b><asp:Label ID="LbEstadoTemporal" runat="server" Text="" CssClass="col-lg-8"></asp:Label></b>
                                    </div>
                                </div>

                                <div id="DivComentarioAuditor" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-12">
                                            <asp:Label Text="Comentario Auditor:" runat="server" CssClass="col-lg-4"/>
                                            <b><asp:Label Text="" ID="TxComentario" runat="server" CssClass="col-lg-8"/></b>
                                        </div>
                                    </div>
                                </div>
                                <div id="DivComentarioResponsable" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-12">
                                            <asp:Label Text="Comentario Responsable:" runat="server" CssClass="col-lg-4"/>
                                            <b><asp:Label Text="" ID="TxComentarioResp" runat="server" CssClass="col-lg-8"/></b>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>
                            <%--<button type="button" class="btn btn-danger" Style="border-radius: 4px;" data-dismiss="modal">Rechazar</button>--%>
                            <asp:Button ID="BtnRechazarAutorizacion" runat="server" Text="Rechazar" Style="border-radius: 4px;" class="btn btn-danger" OnClientClick="ShowProgress();" OnClick="BtnRechazarAutorizacion_Click" />
                            <asp:Button ID="BtnEnviarAutorizacion" runat="server" Text="Aprobar" Style="border-radius: 4px;" class="btn btn-success" OnClientClick="ShowProgress();" OnClick="BtnEnviarAutorizacion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="/assets/js/fstdropdown.js"></script>
</asp:Content>
