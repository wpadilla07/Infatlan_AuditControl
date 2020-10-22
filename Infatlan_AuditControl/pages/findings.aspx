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
    <script type="text/javascript">
        function openModal() { $('#ModificacionesModal').modal('show'); }
        function closeModal() { $('#ModificacionesModal').modal('hide'); }
        function openModalComments() { $('#ComentariosModal').modal('show'); }
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
                <asp:UpdatePanel ID="UpdateForma" runat="server" UpdateMode="Conditional">
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
                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Fecha Resolución </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoFechaResolucion" runat="server" class="form-control" TextMode="Date" ></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Fecha en la cual se comprometen a tener resuelto el hallazgo">?</span>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Opciones</label>
                            <div class="col-sm-8">
                                <asp:LinkButton ID="BtnDescargarAnexo" runat="server" class="btn btn-info" OnClick="BtnDescargarAnexo_Click" >
                                        <i class="ace-icon fa fa-download bigger-110"></i>Anexo
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="BtnHistorico" runat="server" class="btn btn-warning" OnClick="BtnHistorico_Click" >
                                        <i class="ace-icon fa fa-comments bigger-110"></i>Comentarios
                                </asp:LinkButton>

                                <asp:LinkButton ID="BtnAmpliacion" runat="server" Visible="false" class="btn btn-success" OnClick="BtnAmpliacion_Click" >
                                        <i class="ace-icon fa fa-calendar-plus-o bigger-110"></i>Ampliación
                                </asp:LinkButton>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Descargar archivo del hallazgo">?</span>
                        </div>
                        
                        <div class="form-group" runat="server" visible="true" >
                            <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Comentarios </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="TxHallazgoComentarios" runat="server" class="form-control" Rows="3" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                            <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Comentarios finales del hallazgos">?</span>
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

    <%--MODAL AMPLIACION--%>
    <div class="modal fade" id="ModalAmpliacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabel">
                        <asp:Label CssClass=" text-white" ID="Label1" runat="server" Text="Solicitar Ampliación"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel123123" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL MODIFICACIONES INFORME -->
    <div class="modal fade" id="ModificacionesModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdateModificacionesLabel" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelModificaciones">Solicitar Ampliación</h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificacionesMain" runat="server">
                        <ContentTemplate>
                            <div class="page-header">
                                <h1>Modificar Hallazgo
                                    <small>
                                        <i class="ace-icon fa fa-angle-double-right"></i>Ampliacion de fecha de resolución
                                    </small>
                                </h1>
                            </div>
                            <div class="col-xs-12">
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="DDLUserResponsable">Motivo</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="TxMotivo" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>                                        
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="DDLUserResponsable">Nueva Fecha</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="TxFechaAmpliacion" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="DDLUserResponsable">Documento</label>
                                    <div class="col-sm-10">
                                        <asp:FileUpload runat="server" CssClass="form-control" ID="FUDocAmpliacion" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateModificacionesMensaje" runat="server">
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
                            <asp:Button ID="BtnSolicitar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnSolicitar_Click"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnSolicitar"/>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    
    <%--MODAL COMENTARIOS--%>
    <div class="modal fade" id="ComentariosModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label CssClass=" text-white" ID="Label2" runat="server" Text="Histórico de comentarios"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="50" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="usuarioCreacion" HeaderText="Usuario" />
                                                    <asp:BoundField DataField="valorActual" HeaderText="Comentario" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Movimiento" />
                                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                                </Columns>
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
