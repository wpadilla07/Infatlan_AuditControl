using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class upload : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack){
                if (Session["AUTH"] != null){
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 4:
                        case 5:
                            Response.Redirect("/default.aspx");
                            break;
                    }
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        
        protected void BtnCrearInforme_Click(object sender, EventArgs e){
            try{
                String vDireccionCarga = @"C:\Carga\";
                if (FUInforme.HasFile){
                    String vNombreArchivo = FUInforme.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    vDireccionCarga += vNombreArchivo;
                    FUInforme.SaveAs(vDireccionCarga);

                    int? vIdInforme = null;
                    int vSuccess = 0, vError = 0;
                    if (File.Exists(vDireccionCarga)){
                        Cargar vCargarDatos = new Cargar();
                        vIdInforme = vCargarDatos.cargarArchivo(vDireccionCarga, ref vSuccess, ref vError, Convert.ToString(Session["usuario"]));
                    }

                    if (vIdInforme != null){
                        if (FUAdjunto.HasFile){
                            String vArchivo = "";
                            HttpPostedFile bufferDepositoT = FUAdjunto.PostedFile;
                            String vNombreDepot = String.Empty;
                            byte[] vFileDeposito = null;

                            if (bufferDepositoT != null){
                                vNombreDepot = FUAdjunto.FileName;
                                Stream vStream = bufferDepositoT.InputStream;
                                BinaryReader vReader = new BinaryReader(vStream);
                                vFileDeposito = vReader.ReadBytes((int)vStream.Length);
                            }
                            if (vFileDeposito != null)
                                vArchivo = Convert.ToBase64String(vFileDeposito);

                            String vQuery = "[ACSP_Informes] 4," + vIdInforme + ",0,'','','','','','','" + vArchivo + "'";
                            int vAdjunto = vConexion.ejecutarSql(vQuery);
                        }
                        LabelMensaje.Text = "Archivo cargado con exito, favor revise logs" + "<br>";
                        Response.Redirect("/pages/ereports.aspx?id=" + vIdInforme);
                    }
                }else{
                    LabelMensaje.Text = "Fallo la carga del archivo, contacte a sistemas.";
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}