using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class findings : System.Web.UI.Page
    {
        db vConexion = new db();
        String vInformeQuery = String.Empty;
        String vIdHallazgo = String.Empty;
        protected void Page_Load(object sender, EventArgs e){
            vIdHallazgo = Request.QueryString["id"];
            vInformeQuery = Request.QueryString["i"];
            if (!Page.IsPostBack){
                if (vIdHallazgo != null){
                    Session["Hallazgo"] = vIdHallazgo;
                    getHallazgo(vIdHallazgo);

                    String vQuery = "[ACSP_ObtenerArchivos] 3, " + vIdHallazgo;
                    String vArchivo = vConexion.ejecutarSQLGetValueString(vQuery);
                    
                    if (vArchivo == null || vArchivo.Equals("")){
                        BtnDescargarAnexo.Enabled = false;
                        BtnDescargarAnexo.Text = "No existe archivo";
                        BtnDescargarAnexo.CssClass = "btn btn-grey";
                    }

                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 1:
                        case 2:
                            BtnModificarHallazgo.Visible = false;
                            TxHallazgoFechaResolucion.ReadOnly = true;
                            TxHallazgoAccion.ReadOnly = true;
                            break;
                        case 3:
                            TxHallazgoFechaResolucion.ReadOnly = true;
                            BtnModificarHallazgo.Visible = false;
                            TxHallazgoAccion.ReadOnly = true;
                            break;
                        case 5:
                            BtnModificarHallazgo.Visible = false;
                            if (!TxHallazgoAccion.Text.Equals("")){
                                TxHallazgoAccion.ReadOnly = true;
                                TxHallazgoFechaResolucion.ReadOnly = true;
                                UpdateForma.Update();
                            }
                            break;
                        case 4:
                            vQuery = "[ACSP_ObtenerHallazgos] 1," + vIdHallazgo;
                            DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                            DivAmpliacion.Visible = vDatos.Rows[0]["usuarioResponsable"].ToString() == Session["USUARIO"].ToString() ? true : false;
                            if (!TxHallazgoAccion.Text.Equals("")){
                                TxHallazgoAccion.ReadOnly = true;
                                TxHallazgoFechaResolucion.ReadOnly = true;
                                BtnModificarHallazgo.Visible = false;
                            }

                            if (vDatos.Rows[0]["asignado"].ToString() != "" && vDatos.Rows[0]["asignado"].ToString() == Session["USUARIO"].ToString()){
                                TxHallazgoFechaResolucion.ReadOnly = true;
                                BtnModificarHallazgo.Visible = false;
                                TxHallazgoAccion.ReadOnly = true;
                            }
                            break;
                    }
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }
        
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        
        void getHallazgo(String vIdHallazgo){
            try{
                String vQuery = "[ACSP_ObtenerHallazgos] 1," + vIdHallazgo;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                foreach (DataRow item in vDatos.Rows){
                    TxHallazgoDetalle.Text = item["detalle"].ToString();
                    TxHallazgoRiesgo.Text = item["riesgo"].ToString();
                    TxHallazgoRecomendaciones.Text = item["recomendaciones"].ToString();
                    TxHallazgoAccion.Text = item["accion"].ToString();
                    TxHallazgoComentarios.Text = item["comentarios"].ToString();
                    if (!item["fechaResolucion"].ToString().Equals(""))
                        TxHallazgoFechaResolucion.Text = Convert.ToDateTime(item["fechaResolucion"].ToString()).ToString("yyyy-MM-dd");
                    if (!item["fechaAmpliacion"].ToString().Equals("") || !item["tipoEstadoHallazgo"].ToString().Equals("2")){
                        BtnAmpliacion.Enabled = false;
                        BtnAmpliacion.CssClass = "btn btn-default";
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModificarHallazgo_Click(object sender, EventArgs e){
            try{
                if (TxHallazgoAccion.Text.Equals(""))
                    throw new Exception("Por favor ingrese un plan de acción.");
                if (TxHallazgoFechaResolucion.Text.Equals(""))
                    throw new Exception("Por favor ingrese una fecha de resolución.");
                if (Convert.ToDateTime(TxHallazgoFechaResolucion.Text) < DateTime.Now)
                    throw new Exception("Por favor ingrese una fecha de resolución mayor a hoy.");

                String vQuery = "[ACSP_Hallazgos] 2," + vIdHallazgo + ",0,'','','','','" + TxHallazgoAccion.Text.Replace("'","") + "','','','',0,0,'" + TxHallazgoFechaResolucion.Text + "','" + TxHallazgoComentarios.Text.Replace("'", "") + "'";
                if (vConexion.ejecutarSql(vQuery).Equals(1)){
                    vQuery = "[ACSP_ObtenerUsuarios] 7," + vIdHallazgo;
                    DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

                    Correo vCorreo = new Correo();
                    foreach (DataRow item in vDatosResponsables.Rows){
                        vCorreo.Usuario = vConexion.GetNombreUsuario(item["idUsuario"].ToString());
                        vCorreo.Para = item["correo"].ToString();
                        vCorreo.Copia = "";
                    }

                    SmtpService vSmtpService = new SmtpService();
                    vSmtpService.EnviarMensaje(
                        vCorreo.Para,
                        typeBody.General,
                        vCorreo.Usuario,
                        "Se ha respondido al hallazgo (No." + vIdHallazgo + ") " + @"<br \><br \>" +
                        "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                        );

                    vConexion.ActualizarEstadoHallazgo(vIdHallazgo,"2");
                    Mensaje("Modificado con Exito", WarningType.Success);
                    //LimpiarHallazgo();
                    Response.Redirect("/pages/findingsSearch.aspx?i=" + vInformeQuery);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void LimpiarHallazgo(){
            TxHallazgoDetalle.Text = String.Empty;
            TxHallazgoRiesgo.Text = String.Empty;
            TxHallazgoRecomendaciones.Text = String.Empty;
            TxHallazgoAccion.Text = String.Empty;
            TxHallazgoComentarios.Text = String.Empty;
            TxHallazgoFechaResolucion.Text = String.Empty;
        }

        protected void BtnVolver_Click(object sender, EventArgs e){
            Response.Redirect("/pages/findingsSearch.aspx?i=" + vInformeQuery );
        }

        protected void BtnDescargarAnexo_Click(object sender, EventArgs e){
            try{
                String vIdArchivo = vIdHallazgo;
                String vQuery = "[ACSP_ObtenerArchivos] 3, " + vIdArchivo;
                String vArchivo = vConexion.ejecutarSQLGetValueString(vQuery);

                if (vArchivo != ""){
                    byte[] fileData = null;

                    if (!vArchivo.Equals(""))
                        fileData = Convert.FromBase64String(vArchivo);
                    
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=ArchivoNo_" + vIdArchivo + ".xlsx");
                    Response.Flush();
                    Response.End();
                }else
                    throw new Exception("No existe archivo");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAmpliacion_Click(object sender, EventArgs e){
            try{
                LbModificacionesMensaje.Text = string.Empty;
                UpdateModificacionesMensaje.Update();
                TxMotivo.Text = string.Empty;
                TxFechaAmpliacion.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnSolicitar_Click(object sender, EventArgs e){
            try{
                if (TxMotivo.Text == string.Empty || TxMotivo.Text == "")
                    throw new Exception("Por favor ingrese el motivo de la ampliación.");                
                if (TxFechaAmpliacion.Text == string.Empty || TxFechaAmpliacion.Text == "")
                    throw new Exception("Por favor ingrese la fecha de ampliación.");
                if (Convert.ToDateTime(TxFechaAmpliacion.Text) < Convert.ToDateTime(TxHallazgoFechaResolucion.Text))
                    throw new Exception("La fecha de ampliación debe ser mayor a la de resolución.");
                if (Convert.ToDateTime(TxFechaAmpliacion.Text) < DateTime.Now)
                    throw new Exception("La fecha de ampliación debe ser mayor a la fecha actual.");


                String vArchivo = "";
                if (FUDocAmpliacion.HasFile){
                    HttpPostedFile bufferDepositoT = FUDocAmpliacion.PostedFile;
                    String vNombreDepot = String.Empty;
                    byte[] vFileDeposito = null;

                    if (bufferDepositoT != null){
                        vNombreDepot = FUDocAmpliacion.FileName;
                        Stream vStream = bufferDepositoT.InputStream;
                        BinaryReader vReader = new BinaryReader(vStream);
                        vFileDeposito = vReader.ReadBytes((int)vStream.Length);
                    }
                    if (vFileDeposito != null)
                        vArchivo = Convert.ToBase64String(vFileDeposito);
                }

                String vQuery = "[ACSP_Hallazgos] 10," + vIdHallazgo + ",0,0,'" + TxMotivo.Text + "','" + TxFechaAmpliacion.Text + "','" + vArchivo + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 1){
                    vQuery = "[ACSP_ObtenerUsuarios] 7," + vIdHallazgo;
                    DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

                    Correo vCorreo = new Correo();
                    foreach (DataRow item in vDatosResponsables.Rows){
                        vCorreo.Usuario = item["idUsuario"].ToString();
                        vCorreo.Para = item["correo"].ToString();
                        vCorreo.Copia = "";
                    }

                    SmtpService vSmtpService = new SmtpService();
                    vSmtpService.EnviarMensaje(
                        vCorreo.Para,
                        typeBody.General,
                        vCorreo.Usuario,
                        "Se ha solicitado una ampliacion para el hallazgo (No." + vIdHallazgo + ") " + @"<br \><br \>" +
                        "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                        );
                    MensajeLoad("Solicitud realizada con éxito",WarningType.Success);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                }

            }catch (Exception ex){
                LbModificacionesMensaje.Text = ex.Message;
                UpdateModificacionesMensaje.Update();
            }
        }
    }
}