using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Newtonsoft.Json;

namespace Infatlan_AuditControl.pages
{
    public partial class findings : System.Web.UI.Page
    {
        db vConexion = new db();
        String vInformeQuery = String.Empty;
        String vIdHallazgo = String.Empty;
        protected void Page_Load(object sender, EventArgs e){
            //String vToken = Request.QueryString["id"];
            //if (vToken != null){
            //    try{
            //        String vQuery = "[RSP_Documentacion] 12,'" + vToken + "'";
            //        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            //        if (vDatos.Rows.Count > 0){
            //            tokenClass vTokenClass = new tokenClass();
            //            CryptoToken.CryptoToken vTokenCrypto = new CryptoToken.CryptoToken();
            //            vTokenClass = JsonConvert.DeserializeObject<tokenClass>(vTokenCrypto.Decrypt(vToken, ConfigurationManager.AppSettings["TOKEN_DOC"].ToString()));
            //            Session["DOCUMENTOS_ARCHIVO_ID"] = Request.QueryString["d"];


            //            Session["AUTHCLASS"] = vDatos;
            //            Session["USUARIO"] = vDatos.Rows[0]["idEmpleado"].ToString();
            //            Session["AUTH"] = true;
            //            vQuery = "[RSP_Documentacion] 13,'" + vToken + "'";
            //            vConexion.ejecutarSql(vQuery);
            //            Response.Redirect("archivo.aspx", false);
            //        }else 
            //            throw new Exception();

            //    }catch(Exception ex){
            //        Session["AUTH"] = false;
            //        Response.Redirect("/login.aspx");
            //    }
            //}

                                 



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
                            BtnAmpliacion.Visible = vDatos.Rows[0]["usuarioResponsable"].ToString() == Session["USUARIO"].ToString() ? true : false;
                            
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

                            TxHallazgoFechaResolucion.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
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
                String vConsulta = "[ACSP_Ampliaciones] 6," + vIdHallazgo;
                DataTable vData = vConexion.obtenerDataTable(vConsulta);

                foreach (DataRow item in vDatos.Rows){

                    String vAmpliacion = item["idAmpliacion"].ToString();
                    String vAmpliacion2 = item["tipoEstadoHallazgo"].ToString();

                    TxHallazgoDetalle.Text = item["detalle"].ToString();
                    TxHallazgoRiesgo.Text = item["riesgo"].ToString();
                    TxHallazgoRecomendaciones.Text = item["recomendaciones"].ToString();
                    TxHallazgoAccion.Text = item["accion"].ToString();
                    TxHallazgoComentarios.Text = item["comentarios"].ToString();
                    if (!item["fechaResolucion"].ToString().Equals(""))
                        TxHallazgoFechaResolucion.Text = Convert.ToDateTime(item["fechaResolucion"].ToString()).ToString("yyyy-MM-dd");
                    if (item["idAmpliacion"].ToString() != "" || !item["tipoEstadoHallazgo"].ToString().Equals("2")){
                        BtnAmpliacion.Enabled = false;
                        BtnAmpliacion.CssClass = "btn btn-default";
                    }

                    if (vData.Rows.Count > 0 && Convert.ToInt32(vData.Rows[0][0].ToString()) > 2){
                        BtnAmpliacion.Enabled = false;
                        BtnAmpliacion.CssClass = "btn btn-default";
                    }
                }

                vQuery = "[ACSP_Logs] 4," + vInformeQuery + "," + vIdHallazgo;
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    String vComentarios = "";
                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        vComentarios += vDatos.Rows[i]["Accion"].ToString() + " " + vDatos.Rows[i]["valorActual"].ToString() + "\r\n";
                    }
                    TxHallazgoComentarios.Text = vComentarios;
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnModificarHallazgo_Click(object sender, EventArgs e){
            try{
                if (TxHallazgoAccion.Text.Equals(""))
                    throw new Exception("Por favor ingrese un plan de acción.");
                if (TxHallazgoFechaResolucion.Text.Equals(""))
                    throw new Exception("Por favor ingrese una fecha de resolución.");
                if (Convert.ToDateTime(TxHallazgoFechaResolucion.Text) < DateTime.Now)
                    throw new Exception("Por favor ingrese una fecha de resolución mayor a hoy.");

                String vConsul = "[ACSP_Logs] 1," + vInformeQuery + "," + vIdHallazgo + ",'accion','" + TxHallazgoAccion.Text.Replace("'", "") + "','" + Session["USUARIO"].ToString() + "'";
                vConexion.ejecutarSql(vConsul);

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
                TxMotivo.Text = string.Empty;
                TxFechaAmpliacion.Attributes["min"] = TxHallazgoFechaResolucion.Text.ToString();

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

                String vQuery = "", vArchivo = "", vIdArchivo = "NULL";
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

                    vQuery = "[ACSP_Archivos] 1,'" + FUDocAmpliacion.FileName + "','" + vArchivo + "',2";
                    int vRes = vConexion.ejecutarSQLGetValue(vQuery);
                    vIdArchivo = vRes.ToString();
                    if (vRes > 0){
                        vQuery = "[ACSP_Archivos] 2,0,0," + vIdHallazgo + "," + vIdArchivo;
                        vConexion.ejecutarSql(vQuery);
                    }
                }
                vQuery = "[ACSP_Ampliaciones] 1," + vIdHallazgo + "," + vIdArchivo + ",'" + TxMotivo.Text + "','" + TxFechaAmpliacion.Text + "','" + Session["USUARIO"].ToString() + "'";
                int vId = vConexion.ejecutarSQLGetValue(vQuery);

                if (vId > 0){
                    String vConsul = "[ACSP_Logs] 6," + vInformeQuery + "," + vIdHallazgo + ",'comentarioAmpliacion','" + TxMotivo.Text + "','" + Session["USUARIO"].ToString() + "'";
                    vConexion.ejecutarSql(vConsul);

                    vQuery = "[ACSP_Hallazgos] 10," + vIdHallazgo + "," + vId;
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

                        BtnAmpliacion.Enabled = false;
                        BtnAmpliacion.CssClass = "btn btn-default";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    }
                }
            }catch (Exception ex){
                MensajeLoad(ex.Message, WarningType.Danger);
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){

        }

        protected void BtnHistorico_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_Logs] 4," + vInformeQuery + "," + vIdHallazgo;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    vDatos.Columns.Add("nombre");
                    for (int i = 0; i < vDatos.Rows.Count; i++){
                        vDatos.Rows[i]["nombre"] = vConexion.GetNombreUsuario(vDatos.Rows[i]["usuarioCreacion"].ToString());
                    }

                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();

                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalComments();", true);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}