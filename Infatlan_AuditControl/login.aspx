<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Infatlan_AuditControl.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>Login - Auditoria</title>

    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link rel="stylesheet" href="/assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assets/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="/assets/css/fonts.googleapis.com.css" />
    <link rel="stylesheet" href="/assets/css/ace.min.css" />
    <link rel="stylesheet" href="/assets/css/ace-rtl.min.css" />
</head>
<body class="login-layout light-login">
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="main-content">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="center">
                                <h1>
                                    <img src="assets/images/logored.png" style="width: 50%" />
                                </h1>
                            </div>

                            <div class="space-6"></div>

                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header blue lighter bigger" style="text-align: center">
                                                <i class="ace-icon fa fa-lock blue"></i>
                                                Auditoria Interna
                                            </h4>

                                            <div class="space-6"></div>


                                            <fieldset>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox ID="TxUsername" class="form-control form-control-lg border-left-0" placeholder="Username"  runat="server"></asp:TextBox>
                                                        <i class="ace-icon fa fa-user"></i>
                                                    </span>
                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox ID="TxPassword" TextMode="Password" class="form-control form-control-lg border-left-0" placeholder="Password"  runat="server"></asp:TextBox>
                                                        <i class="ace-icon fa fa-lock"></i>
                                                    </span>
                                                </label>

                                                <div class="space"></div>

                                                <div class="clearfix">
                                                    <asp:LinkButton ID="BtnLogin" class="width-100 pull-right btn btn-sm btn-primary" runat="server"  OnClick="BtnLogin_Click" >
                                                        <i class="ace-icon fa fa-lock"></i>
                                                        <span class="bigger-110">Login</span>

                                                    </asp:LinkButton>
                                                </div>

                                                <div class="space-4"></div>
                                            </fieldset>


                                            <div class="social-or-login center">
                                                <span class="bigger-110">Privacidad</span>
                                            </div>

                                            <div class="space-6"></div>
                                            <div class="my-2 d-flex justify-content-center align-center" style="color:indianred;">
                                                <asp:Label ID="LbMensaje" runat="server" Text=""></asp:Label>
                                            </div>
                                            <div class="social-login center align-justify">
                                                Recuerda que esta es una aplicación de Auditoria Interna la información aqui contenida es de alta confidencialidad, cualquier uso indebido sera sancionado de acuerdo a la ley.
                                            </div>
                                        </div>
                                        <!-- /.widget-main -->

                                        <%--<div class="toolbar clearfix">
                                            <div>
                                                <a href="#" data-target="#forgot-box" class="forgot-password-link"></a>
                                            </div>

                                            <div>
                                                <a href="#" data-target="#signup-box" class="user-signup-link white">Obtener acceso
													<i class="ace-icon fa fa-arrow-right"></i>
                                                </a>
                                            </div>
                                        </div>--%>
                                    </div>
                                   
                                </div>
                               

                                <div id="signup-box" class="signup-box widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header blue lighter bigger" style="text-align: center">
                                                <i class="ace-icon fa fa-users blue"></i>
                                                Enviar registro a administrador
                                            </h4>

                                            <div class="space-6"></div>
                                            <p>Ingrese un usuario y descripción del acceso: </p>


                                            <fieldset>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <input type="text" class="form-control" placeholder="Username" />
                                                        <i class="ace-icon fa fa-user"></i>
                                                    </span>
                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <input type="text" class="form-control" placeholder="Descripción"  />
                                                        <i class="ace-icon fa fa-desktop"></i>
                                                    </span>
                                                </label>

                                                

                                                
                                                <div class="space-24"></div>

                                                <div class="clearfix">
                                              
                                                    <button type="button" class="width-100 pull-right btn btn-sm btn-primary">
                                                        <span class="bigger-110">Registrar</span>

                                                        <i class="ace-icon fa fa-arrow-right icon-on-right"></i>
                                                    </button>
                                                </div>
                                            </fieldset>

                                        </div>

                                        <div class="toolbar center">
                                            <a href="#" data-target="#login-box" class="back-to-login-link white">
                                                <i class="ace-icon fa fa-arrow-left"></i>
                                                Volver
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="assets/js/jquery-2.1.4.min.js"></script>
        <script type="text/javascript">
            if ('ontouchstart' in document.documentElement) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        </script>

        <script type="text/javascript">
            jQuery(function ($) {
                $(document).on('click', '.toolbar a[data-target]', function (e) {
                    e.preventDefault();
                    var target = $(this).data('target');
                    $('.widget-box.visible').removeClass('visible');//hide others
                    $(target).addClass('visible');//show target
                });
            });


        </script>
    </form>
</body>
</html>
