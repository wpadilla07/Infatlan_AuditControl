<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="ereports.aspx.cs" Inherits="Infatlan_AuditControl.pages.ereports" %>

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
        function openModalHallazgos() {$('#HallazgosModal').modal('show');}
        function closeModalHallazgos() {$('#HallazgosModal').modal('hide');}
        function openModalHallazgosCreacion() {$('#HallazgosCreacionModal').modal('show');}
        function openRevisionModal() {$('#RevisionModal').modal('show');}
        function openResponsablesModal() {$('#ResponsablesModal').modal('show');}
        function openHallazgosModificacionCreacionModal() {$('#HallazgosModificacionCreacionModal').modal('show');}
    </script>
    <link href="/assets/css/fstdropdown.css" rel="stylesheet" />
    <link href="../assets/css/select2.min.css" rel="stylesheet" />
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
            <li class="active">Buscar / Modificar Informes</li>
        </ul>
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>Buscar / Modificar Informes
				<small>
                    <i class="ace-icon fa fa-angle-double-right"></i>
                    Informes creados a la fecha
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
                            PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging" OnRowCommand="GVBusqueda_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEntrar" runat="server" Text="Entrar" style="border-radius: 4px;" class="btn btn-success" CommandArgument='<%# Eval("idInforme") %>' CommandName="EntrarInforme">
                                            <i class="fa fa-search"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UpdateForma" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="BtnDescargarInforme" runat="server" Text="Descargar" Style="border-radius: 4px;" class="btn btn-success " CommandArgument='<%# Eval("idInforme") %>' CommandName="DescargarInforme">
                                            <i class="fa fa-download"></i>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnDescargarInforme"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="idInforme" HeaderText="No." />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="fechaRes" HeaderText="Respuesta" />
                                <asp:BoundField DataField="fechaCreacion" HeaderText="Creación" />
                                <asp:BoundField DataField="usuarioCreacion" HeaderText="Creado" />
                                <asp:BoundField DataField="tipoEstado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnAsignar" runat="server" Text="Entrar" style="border-radius: 4px;" class="btn btn-info" CommandArgument='<%# Eval("idInforme") %>' CommandName="CrearHallazgos">
                                            <i class="fa fa-plus-square "></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEnviarRevision" runat="server" Text="Entrar" style="border-radius: 4px;" class="btn btn-success" CommandArgument='<%# Eval("idInforme") %>' CommandName="EnviarJefatura">
                                            <i class=" fa fa-mail-reply "> Revisión</i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEnviarResponsable" runat="server" Text="Entrar" style="border-radius: 4px; " class="btn btn-info" CommandArgument='<%# Eval("idInforme") %>' CommandName="EnviarResponsable">
                                            <i class=" fa fa-mail-reply-all "> Enviar Informe</i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UpdateReporte" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="BtnReporte" runat="server" Text="Reporte" Style="border-radius: 4px;" class="btn btn-light " CommandArgument='<%# Eval("idInforme") %>' CommandName="ReporteInforme">
                                                    <i class="fa fa-file-text"></i>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnDescargarInforme"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
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

    <!-- MODAL CREACIÓN DE HALLAZGOS -->
    <div class="modal fade" id="HallazgosCreacionModal" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 1000px; top: 350px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdateAsignarUsuarioLabel" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelHallazgosCreacion">Crear Hallazgo - Informe No.
                                    <asp:Label ID="LbNumeroInformeHallazgosCreacion" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateHallazgosCreacionMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Responsable </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLHallazgoResponsable" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group row">
                                <label class="col-sm-2 control-label no-padding-right" for="DDLHallazgoArea">Area </label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="DDLHallazgoArea" class="select2 form-control custom-select" style="width: 100%" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdateHallazgosCreacionMain2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLEmpresa">Empresa</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLEmpresa" class="form-control" style="width: 100%" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Nivel Riesgo </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLHallazgoRiesgo" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Nivel de riesgo de auditoria">?</span>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Hallazgo</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxHallazgoDetalle" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Riesgo / Incumplimiento</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxHallazgoRiesgoDetalle" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Recomendaciones</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxHallazgoRecomendacionDetalle" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Proceso / Politica</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="TxHallazgoFuente" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Otro numero documento o codigo de otro informe">?</span>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Fecha Respuesta</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="TxFechaCumplimiento" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Fecha posible de finalización del hallazgo (Auditor)">?</span>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Documento</label>
                                        <div class="col-sm-6">
                                            <asp:FileUpload ID="FUHallazgos" runat="server" class="form-control" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"/>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir archivos relacionados al hallazgo">?</span>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdateHallazgosCreacionMensaje" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbHallazgosCreacionMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateHallazgosCreacionBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:LinkButton ID="BtnHallazgosCreacionInforme" runat="server" OnClick="BtnHallazgosCreacionInforme_Click" class="btn btn-primary">Crear Hallazgo</asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnHallazgosCreacionInforme" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL PARA VER HALLAZGOS -->
    <div class="modal fade" id="HallazgosModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdateHallazgosLabel" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelUsuario">Hallazgos - Informe No.
                                    <asp:Label ID="LbNumeroInformeHallazgos" runat="server" Text=""></asp:Label>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateHallazgosMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="page-header">
                                <h1>Hallazgos creados
                                    <small>
                                        <i class="ace-icon fa fa-angle-double-right"></i>
                                        Presione entrar para ir a cualquier hallazgo
                                    </small>
                                </h1>
                            </div>
                            <div class="form-group ">
                                <div class="col-md-12">
                                    <div class="table-responsive" style="width: 100%">
                                        <asp:GridView ID="GVHallazgosView" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            GridLines="None"
                                            AutoGenerateColumns="false" OnRowCommand="GVHallazgosView_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnHallazgoEditar" runat="server" Text="Entrar" style="border-radius: 4px;" class="btn btn-warning mr-2" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="EditarHallazgo" >
                                                            <i class=" fa fa-edit "></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="BtnHallazgoEntrar" runat="server" Text="Entrar" style="border-radius: 4px;" class="btn btn-success mr-2" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="EntrarHallazgo" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="idHallazgo" HeaderText="No.Hallazgo" />
                                                <asp:BoundField DataField="fechaCreacion" HeaderText="Creación" />
                                                <asp:BoundField DataField="idArea" HeaderText="Area" />
                                                <asp:BoundField DataField="tipoRiesgo" HeaderText="Riesgo" />
                                                <asp:BoundField DataField="detalle" HeaderText="Detalle" />
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnDelete" runat="server" style="border-radius: 4px;" class="btn btn-danger mr-2" CommandArgument='<%# Eval("idHallazgo") %>' CommandName="BorrarHallazgo" >
                                                            <i class=" fa fa-trash "></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateHallazgosMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbCerrarMensajeHallazgos" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateHallazgosBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEntrarInf" Text="Modificar Informe" Style="border-radius:4px;" CssClass="btn btn-info" runat="server" OnClick="BtnEntrarInf_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE REVISION--%>
    <div class="modal fade" id="RevisionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelRevision">Revisión de Informe No.
                        <asp:Label ID="LbRevisionInforme" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <label class="control-label">¿Estas seguro de enviar este informe a tu superior?</label>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnEnviarRevision" runat="server" Text="Enviar" class="btn btn-success" OnClientClick="ShowProgress();" OnClick="BtnEnviarRevision_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE RESPONSABLES--%>
    <div class="modal fade" id="ResponsablesModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelResponsables">Enviar a responsables - Informe No.
                        <asp:Label ID="LbResponsablesInforme" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <label class="control-label">¿Estas seguro de enviar este informe a los responsables?</label>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnEnviarResponsables" runat="server" Text="Enviar" class="btn btn-success" OnClientClick="ShowProgress();" OnClick="BtnEnviarResponsables_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL MODIFICACION DE HALLAZGOS -->
    <div class="modal fade" id="HallazgosModificacionCreacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 1000px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelHallazgosModificacion">Modificar Hallazgo - Informe No.
                                    <asp:Label ID="LbModificarHallazgoLabel" runat="server" Text=""></asp:Label>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Responsable </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLModificarHallazgosResponsable" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Area </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLModificarHallazgosArea" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Nivel Riesgo </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DDLModificarHallazgosNivelRiesgo" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Nivel de riesgo de auditoria">?</span>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Hallazgo</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxModificarHallazgosHallazgo" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Riesgo / Incumplimiento</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxModificarHallazgosRiesgo" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="form-field-1">Recomendaciones</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="TxModificarHallazgosRecomendaciones" runat="server" class="form-control" TextMode="MultiLine" Style="height: 50px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Proceso / Politica</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="TxModificarHallazgosFuente" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Otro numero documento o codigo de otro informe">?</span>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Fecha Respuesta</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="TxModificarHallazgosFechaRespuesta" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Fecha posible de finalización del hallazgo (Auditor)">?</span>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Documento</label>
                                        <div class="col-sm-6">
                                            <asp:FileUpload ID="FUModificarHallazgos" runat="server" class="form-control" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" />
                                        </div>
                                        <span class="help-button" data-rel="popover" data-trigger="hover" data-placement="left" data-content="More details." title="Subir archivos relacionados al hallazgo">?</span>
                                        <asp:Label Text="" style="color:cornflowerblue" ID="TxFileName" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbModificarHallazgosMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:LinkButton ID="BtnModificarHallazgos" runat="server" class="btn btn-primary" OnClick="BtnModificarHallazgos_Click">Modificar Hallazgo</asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnModificarHallazgos" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="/assets/js/jquery.dataTables.min.js"></script>
    <script src="/assets/js/jquery.dataTables.bootstrap.min.js"></script>
    <script src="/assets/js/dataTables.select.min.js"></script>
    <script src="/assets/js/fstdropdown.js"></script>
    <link href="../assets/css/select2.min.css" rel="stylesheet" />
    <script src="../assets/js/select2.min.js"></script>
    <style>
        .select2-selection__rendered {line-height: 31px !important;}
        .select2-container .select2-selection--single {height: 35px !important;}
        .select2-selection__arrow {height: 34px !important;}
    </style>
    <script>
        $(function () {
            $(".select2").select2();
            $(".ajax").select2({
                ajax: {
                    url: "https://api.github.com/search/repositories",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            q: params.term, // search term
                            page: params.page
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        return {
                            results: data.items,
                            pagination: {
                                more: (params.page * 30) < data.total_count
                            }
                        };
                    },
                    cache: true
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1,
            });
        });
    </script>
</asp:Content>
