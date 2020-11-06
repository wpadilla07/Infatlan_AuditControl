using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl
{
    public partial class main : System.Web.UI.MasterPage
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Convert.ToBoolean(Session["AUTH"]))
                Response.Redirect("/login.aspx");
            
            if (!Page.IsPostBack){
                String vError = "";
                try{
                    DataTable vDatos = (DataTable)Session["AUTHCLASS"];
                    LbUsuarioNombre.Text = Session["USUARIO"].ToString();

                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 1:
                            LbPerfil.Text = "Administrador";
                            break;
                        case 2:
                            LbPerfil.Text = "Auditor Jefatura";
                            break;
                        case 3:
                            LbPerfil.Text = "Auditor";
                            break;
                        case 4:
                            LbPerfil.Text = "Responsable";
                            LI1.Visible = false;
                            LICrearInforme.Visible = false;
                            break;
                        case 5:
                            LbPerfil.Text = "Consultas";
                            LI1.Visible = false;
                            LIInformes.Visible = false;
                            break;
                    }

                    if (Convert.ToInt32(Session["AUTH"]).Equals(1)){

                    }
                    //OWNER
                    else if (Convert.ToInt32(Session["AUTH"]).Equals(2)){

                    }
                    //CONSULTAS
                    else if (Convert.ToInt32(Session["AUTH"]).Equals(3)){

                    }
                    //REPORTES
                    else if (Convert.ToInt32(Session["AUTH"]).Equals(4)){

                    }

                    cargarInformes();
                    cargarHallazgos();
                }catch (Exception Ex){
                    vError = Ex.Message;
                }
            }

            void cargarInformes(){
                try{
                    String vQuery = "[ACSP_MensajesMasterPage] 1";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    String vLiteralFinal = String.Empty;
                    foreach (DataRow item in vDatos.Rows){
                        vLiteralFinal += 
                            "<li>" + Environment.NewLine +
                            "   <a href=\"/pages/ereports.aspx?id=" + item["idInforme"].ToString() + "\" class=\"clearfix\">" + Environment.NewLine +
                            "       <img src=\"/assets/images/avatars/avatar2.png\" class=\"msg-photo\" alt=\"Alex's Avatar\" /> " + Environment.NewLine +
                            "       <span class=\"msg-body\"> " + Environment.NewLine +
                            "           <span class=\"msg-title\"> " + Environment.NewLine +
                            "               <span class=\"blue\">Informe No." + item["idInforme"].ToString() +  " </span>" + Environment.NewLine +
                            "               " + item["nombre"].ToString() + Environment.NewLine +
                            "           </span>" + Environment.NewLine +
                            "           <span class=\"msg-time\">" + Environment.NewLine +
                            "               <i class=\"ace-icon fa fa-clock-o\"></i>" + Environment.NewLine +
                            "               <span>" + item["fechaCreacion"].ToString() + "</span>" + Environment.NewLine +
                            "           </span> " + Environment.NewLine +
                            "       </span>" + Environment.NewLine +
                            "   </a>" + Environment.NewLine +
                            "</li>";
                    }

                    //LitInformes.Text = vLiteralFinal;
                    LitInformes.Text = "";

                }catch { }
            }

            void cargarHallazgos(){
                try{
                    String vQuery = "[ACSP_MensajesMasterPage] 2";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    String vLiteralFinal = String.Empty;
                    foreach (DataRow item in vDatos.Rows){
                        vLiteralFinal +=
                            "<li>" + Environment.NewLine +
                            "   <a href=\"/pages/findings.aspx?id=" + item["idHallazgo"].ToString() + "\" class=\"clearfix\">" + Environment.NewLine +
                            "       <img src=\"/assets/images/avatars/avatar2.png\" class=\"msg-photo\" alt=\"Alex's Avatar\" /> " + Environment.NewLine +
                            "       <span class=\"msg-body\"> " + Environment.NewLine +
                            "           <span class=\"msg-title\"> " + Environment.NewLine +
                            "               <span class=\"blue\">Informe No." + item["idHallazgo"].ToString() + " </span>" + Environment.NewLine +
                            "               " + item["Detalle"].ToString().Substring(0, (item["Detalle"].ToString().Length > 50 ? 50 : item["Detalle"].ToString().Length)) + Environment.NewLine +
                            "           </span>" + Environment.NewLine +
                            "           <span class=\"msg-time\">" + Environment.NewLine +
                            "               <i class=\"ace-icon fa fa-clock-o\"></i>" + Environment.NewLine +
                            "               <span>" + item["fechaCreacion"].ToString() + "</span>" + Environment.NewLine +
                            "           </span> " + Environment.NewLine +
                            "       </span>" + Environment.NewLine +
                            "   </a>" + Environment.NewLine +
                            "</li>";
                    }

                    //LitHallazgos.Text = vLiteralFinal;
                    LitHallazgos.Text = "";

                }catch { }
            }
        }
    }
}
