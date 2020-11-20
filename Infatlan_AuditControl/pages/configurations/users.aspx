<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="Infatlan_AuditControl.pages.configurations.users" %>

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


        // Change hash for page-reload
        $('.nav-tabs a').on('shown.bs.tab', function (e) {
            window.location.hash = e.target.hash;
        })
    </script>
    <script type="text/javascript">
        function UsuariosModal() {
            $('#UsuariosModal').modal('show');
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
            <li>
                <a href="#">Configurations</a>
            </li>
            <li class="active">Usuarios</li>
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
        <div class="col-xs-12">


            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Selección de Cargo</h4>
                            <p class="card-description">
                                Por favor seleccione un cargo para el usuario
                            </p>
                            <hr />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Usuario</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLCargo" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLCargo_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                        <asp:ListItem Value="1">Administrador</asp:ListItem>
                                                        <asp:ListItem Value="2">Auditor Jefatura</asp:ListItem>
                                                        <asp:ListItem Value="3">Auditor</asp:ListItem>
                                                        <asp:ListItem Value="4">Responsables</asp:ListItem>
                                                        <asp:ListItem Value="5">Consultas</asp:ListItem>
                                                        <asp:ListItem Value="6">Junta Directiva</asp:ListItem>
                                                        <asp:ListItem Value="7">Reporte</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6" runat="server" visible="false" id="DivEmpresas">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Empresa</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLEmpresa" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6" runat="server" visible="false" id="DIVUsuarioJefatura">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Supervisor</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLUsuarioJefatura" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Información del nuevo usuario</h4>
                            <p class="card-description">
                                Por favor ingrese todos los siguientes campos
                            </p>
                            <hr />
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Usuario</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxUsuario" placeholder="Ej. admin" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="BtnBuscarUsuario" Style="width: 100%" class="btn btn-dark mr-2" runat="server" Text="Buscar" OnClick="BtnBuscarUsuario_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Correo</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCorreo" placeholder="Ej. admin@banctlan.hn" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Password</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxPassword" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Confirmar</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxPasswordConfirmacion" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Nombres</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxNombres" placeholder="Ej. Juan Jose" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 control-label no-padding-right">Apellidos</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxApellidos" placeholder="Ej. Perez Perez" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="clearfix form-actions">
                <div class="col-md-offset-0 col-md-9">
                    <asp:UpdatePanel ID="UpdateBotones" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="BtnCrearUsuario" runat="server" class="btn btn-info" OnClick="BtnCrearUsuario_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Crear Usuario
                            </asp:LinkButton>
                            <button class="btn" type="reset">
                                <i class="ace-icon fa fa-undo bigger-110"></i>
                                Reset
                            </button>
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
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="BtnModificar" runat="server" Text="Modificar" class="btn btn-success" CommandArgument='<%# Eval("idUsuario") %>' CommandName="ModificarUsuario">
                                            <i class="fa fa-edit"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="idUsuario" HeaderText="Usuario" />
                                    <asp:BoundField DataField="correo" HeaderText="Correo" />
                                    <asp:BoundField DataField="tipoUsuario" HeaderText="Tipo Usuario" />
                                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="hr hr-double dotted"></div>

                    <div class="clearfix form-actions">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    Nota: ten cuidado con la modificación de información por temas de auditoria interna, cualquier cambio hecho al sistema se grabara en un log de sistema.
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL PARA VER HALLAZGOS -->
    <div class="modal fade" id="UsuariosModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 640px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdateHallazgosLabel" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelUsuario">Usuario 
                                    <asp:Label ID="LbUsuarioModificar" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateUsuariosMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Cargo </label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLCargoModificar" class="form-control" runat="server">
                                                <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                <asp:ListItem Value="1">Administrador</asp:ListItem>
                                                <asp:ListItem Value="2">Auditor Jefatura</asp:ListItem>
                                                <asp:ListItem Value="3">Auditor</asp:ListItem>
                                                <asp:ListItem Value="4">Responsables</asp:ListItem>
                                                <asp:ListItem Value="5">Consultas</asp:ListItem>
                                                <asp:ListItem Value="6">Reporte</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 control-label no-padding-right" for="DDLUserResponsable">Estado </label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLEstado" class="form-control" runat="server">
                                                <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                <asp:ListItem Value="True">Activado</asp:ListItem>
                                                <asp:ListItem Value="False">Desactivado</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateUsuarioMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbErrorUsuario" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnModificarUsuario" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnModificarUsuario_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
