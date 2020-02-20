using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class findings : System.Web.UI.Page
    {
        db vConexion = new db();
        String vInformeQuery = String.Empty;
        String vIdHallazgo = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            vIdHallazgo = Request.QueryString["id"];
            vInformeQuery = Request.QueryString["i"];
            if (!Page.IsPostBack)
            {
                if (vIdHallazgo != null)
                {
                    Session["Hallazgo"] = vIdHallazgo;
                    getHallazgo(vIdHallazgo);

                    String vQuery = "[ACSP_ObtenerArchivos] 3, " + vIdHallazgo;
                    String vArchivo = vConexion.ejecutarSQLGetValueString(vQuery);
                    
                    if (vArchivo == null)
                    {
                        BtnDescargarAnexo.Enabled = false;
                        BtnDescargarAnexo.Text = "No existe archivo";
                        BtnDescargarAnexo.CssClass = "btn btn-grey";
                    }

                    switch (Convert.ToInt32(Session["TIPOUSUARIO"]))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 5:
                            BtnModificarHallazgo.Visible = false;
                            break;
                        case 4:
                            if(!TxHallazgoAccion.Text.Equals(""))
                                BtnModificarHallazgo.Visible = false;
                            break;
                    }
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        void getHallazgo(String vIdHallazgo)
        {
            try
            {

                String vQuery = "[ACSP_ObtenerHallazgos] 1," + vIdHallazgo;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                foreach (DataRow item in vDatos.Rows)
                {
                    TxHallazgoDetalle.Text = item["detalle"].ToString();
                    TxHallazgoRiesgo.Text = item["riesgo"].ToString();
                    TxHallazgoRecomendaciones.Text = item["recomendaciones"].ToString();
                    TxHallazgoAccion.Text = item["accion"].ToString();
                    TxHallazgoComentarios.Text = item["comentarios"].ToString();
                    TxHallazgoFechaResolucion.Text = Convert.ToDateTime(item["fechaResolucion"].ToString()).ToString("yyyy-MM-dd");
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModificarHallazgo_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "[ACSP_Hallazgos] 2," + vIdHallazgo + ",0,'','','','','" + TxHallazgoAccion.Text.Replace("'","") + "','','','',0,0,'" + TxHallazgoFechaResolucion.Text + "','" + TxHallazgoComentarios.Text.Replace("'", "") + "'";
                if (vConexion.ejecutarSql(vQuery).Equals(1))
                {

                    vQuery = "[ACSP_ObtenerUsuarios] 7," + vIdHallazgo;
                    DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

                    Correo vCorreo = new Correo();

                    foreach (DataRow item in vDatosResponsables.Rows)
                    {
                        vCorreo.Usuario = item["idUsuario"].ToString();
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



                    Mensaje("Modificado con Exito", WarningType.Success);
                    //LimpiarHallazgo();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        void LimpiarHallazgo()
        {
            TxHallazgoDetalle.Text = String.Empty;
            TxHallazgoRiesgo.Text = String.Empty;
            TxHallazgoRecomendaciones.Text = String.Empty;
            TxHallazgoAccion.Text = String.Empty;
            TxHallazgoComentarios.Text = String.Empty;
            TxHallazgoFechaResolucion.Text = String.Empty;
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/pages/findingsSearch.aspx?i=" + vInformeQuery );
        }

        protected void BtnDescargarAnexo_Click(object sender, EventArgs e)
        {
            try
            {
                String vIdArchivo = vIdHallazgo;

                String vQuery = "[ACSP_ObtenerArchivos] 3, " + vIdArchivo;
                String vArchivo = vConexion.ejecutarSQLGetValueString(vQuery);

                if (vArchivo != "")
                {

                    byte[] fileData = null;

                    if (!vArchivo.Equals(""))
                    {
                        fileData = Convert.FromBase64String(vArchivo);
                    }

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=ArchivoNo_" + vIdArchivo + ".xlsx");
                    Response.Flush();
                    Response.End();
                }
                else
                    throw new Exception("No existe archivo");

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}