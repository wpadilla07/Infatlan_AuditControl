using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Infatlan_AuditControl.pages
{
    public partial class findingsSearch : System.Web.UI.Page
    {
        db vConexion = new db();
        String vInformeQuery = string.Empty;
        CryptoToken.CryptoToken vToken = new CryptoToken.CryptoToken();
        protected void Page_Load(object sender, EventArgs e){
            vInformeQuery = Request.QueryString["i"];
            if (!Page.IsPostBack){
                getResponsables();
                getInformes();
                if (vInformeQuery != null){
                    BuscarHallazgo(vInformeQuery);
                }
            }
        }
        
        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }

        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        
        void getEstados(String vIdHallazgo){
            try{
                String vQuery = "[ACSP_ObtenerTipos] 4," + vIdHallazgo;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLModificarHallazgoEstado.Items.Clear();
                DDLModificarHallazgoEstado.Items.Add(new ListItem { Value = "0", Text = "Seleccione un estado" });
                foreach (DataRow item in vDatos.Rows){
                    DDLModificarHallazgoEstado.Items.Add(new ListItem { Value = item["tipoEstadoHallazgo"].ToString(), Text = item["nombre"].ToString() });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void getResponsables(){
            try{
                LdapService vLdap = new LdapService();
                String vQuery = "[ACSP_ObtenerUsuarios] 8";
                DataTable vDatosDB = vConexion.obtenerDataTable(vQuery);

                DataTable vDatosFinal = new DataTable();
                vDatosFinal.Columns.Add("usuario");
                vDatosFinal.Columns.Add("nombre");
                vDatosFinal.Columns.Add("apellido");
                vDatosFinal.Columns.Add("correo");
                vDatosFinal.Columns.Add("empresa");

                for (int i = 0; i < vDatosDB.Rows.Count; i++){
                    DataTable vDatosAD = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], vDatosDB.Rows[i]["idUsuario"].ToString());
                    vDatosFinal.Rows.Add(
                        vDatosDB.Rows[i]["idUsuario"].ToString(),
                        vDatosAD.Rows[0]["givenName"].ToString(),
                        vDatosAD.Rows[0]["sn"].ToString(),
                        vDatosAD.Rows[0]["mail"].ToString());

                    vDatosFinal.Rows[i]["empresa"] = vDatosDB.Rows[i]["empresa"].ToString();
                }

                DDLUsuariosAsignacionHallazgo.Items.Add(new ListItem { Value="0", Text="Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows){
                    DDLUsuariosAsignacionHallazgo.Items.Add(new ListItem { Value = item["usuario"].ToString(), Text = item["nombre"].ToString() + " " + item["apellido"].ToString() +  " - " + item["empresa"].ToString() });
                }
                
                DDLUsuariosAsignacionHallazgo.DataBind();
                UpdateModificacionesMain.Update();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        public void getInformes(){
            try{
                DDLBuscarInforme.Items.Clear();
                String vQuery = "";
                if (Session["TIPOUSUARIO"].ToString() == "6"){
                    vQuery = "[ACSP_ObtenerInformes] 1";
                }else 
                    vQuery = "[ACSP_ObtenerInformes] 5,0,'" + Convert.ToString(Session["USUARIO"]) + "'";

                DataTable vDatosDB = vConexion.obtenerDataTable(vQuery);
                DDLBuscarInforme.Items.Add(new ListItem { Value = "0", Text = "Seleccione un informe" });
                foreach (DataRow item in vDatosDB.Rows){
                    DDLBuscarInforme.Items.Add(new ListItem { Value = item["idInforme"].ToString(), Text = item["idInforme"].ToString() + " - " + item["nombre"].ToString() });
                }

            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdHallazgo = e.CommandArgument.ToString();
                DivCierre.Visible = false;
                DivFile.Visible = false;
                DivComentario.Visible = false;
                DivComentarioResponsable.Visible = false;
                TxComentarioAuditor.Text = string.Empty;
                TxComentarioResp.Text = string.Empty;
                LbHallazgoEstadoMensaje.Text = string.Empty;
                UpdatePanel5.Update();
                UpdatePanel4.Update();

                if (e.CommandName == "EntrarHallazgo"){
                    Response.Redirect("/pages/findings.aspx?id=" + vIdHallazgo + "&i=" + DDLBuscarInforme.SelectedValue);  //TxBuscarIdInforme.Text);
                }
                
                if (e.CommandName == "AsignarUsuario"){
                    DDLUsuariosAsignacionHallazgo.SelectedValue = "0";
                    LbModificacionesMensaje.Text = "";
                    UpdateModificacionesMensaje.Update();
                    UpdateModificacionesMain.Update();
                    LbNumeroHallazgoModificaciones.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHallazgos();", true);
                }

                if (e.CommandName == "AutorizarEstadoHallazgo"){
                    LbAutorizacionHallazgo.Text = vIdHallazgo;
                    String vQuery = "[ACSP_ObtenerHallazgos] 10, " + vIdHallazgo;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    BtnRechazarAutorizacion.Visible = false;
                    if (vDatos.Rows.Count > 0){
                        DivAmpliacion.Visible = false;
                        //BtnRechazarAutorizacion.Visible = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "7" ? false : true;
                        LbEstadoTemporal.Text = vDatos.Rows[0]["nombre"].ToString();
                        TxComentario.Text = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "7" ? vDatos.Rows[0]["comentarioAuditor"].ToString() : "";
                        TxComentarioResp.Text = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "7" ? vDatos.Rows[0]["comentarios"].ToString() : "";
                        TxComentarioResp.Text = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "3" ? vDatos.Rows[0]["comentarios"].ToString() : TxComentarioResp.Text;
                        DivComentarioAuditor.Visible = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "7" ? true : false;
                        DivComentarioResponsable.Visible = TxComentarioResp.Text == "" ? false : true;
                        Session["ESTADO_AMPLIACION"] = false;
                    }else{ 
                        vQuery = "[ACSP_ObtenerHallazgos] 12, " + vIdHallazgo;
                        vDatos = vConexion.obtenerDataTable(vQuery);
                        if (vDatos.Rows.Count > 0){
                            DivAmpliacion.Visible = true;

                            LbEstadoTemporal.Text = vDatos.Rows[0]["estado"].ToString() == "2" ? "Rechazar Ampliacion" : "Aprobar Ampliación";
                            LbAmpliacion.Text = vDatos.Rows[0]["idAmpliacion"].ToString();
                            LbFechaLimite.Text = Convert.ToDateTime(vDatos.Rows[0]["fechaResolucion"]).ToString("dd-MM-yyyy");
                            LbFechaSolicitada.Text = Convert.ToDateTime(vDatos.Rows[0]["fechaAmpliacion"]).ToString("dd-MM-yyyy");
                            LbComentario.Text = vDatos.Rows[0]["comentario"].ToString();
                            DivAmpliacionDoc.Visible = vDatos.Rows[0]["nombre"].ToString() != "" ? true : false;
                            LBDocAmpliacion.Text = vDatos.Rows[0]["nombre"].ToString();

                            vQuery = "[ACSP_Logs] 8," + vIdHallazgo;
                            vDatos = vConexion.obtenerDataTable(vQuery);
                            DivComentarioRechazo.Visible = false;
                            if (vDatos.Rows.Count > 0){
                                DivComentarioRechazo.Visible = true;
                                LbComentarioAuditor.Text = vDatos.Rows[0]["valorActual"].ToString();
                            }

                            Session["ESTADO_AMPLIACION"] = true;
                        }
                    }
                    
                    UpdatePanel12.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAutorizacionEstadoModal();", true);
                }

                if (e.CommandName == "ModificarEstadoHallazgo") {
                    getEstados(vIdHallazgo);
                    String vQuery = "[ACSP_ObtenerHallazgos] 1," + vIdHallazgo;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    Session["HALLAZGO_INFO"] = vDatos;

                    String vTemporal = "";
                    if (vDatos.Rows.Count > 0) {
                        vTemporal = vDatos.Rows[0]["tipoEstadoHallazgoTemporal"].ToString();
                        if (vTemporal == "8") {
                            TxComentarioCierre.Text = vDatos.Rows[0]["comentarios"].ToString();
                            DivCierre.Visible = true;
                        }
                    }

                    vQuery = "[ACSP_ObtenerHallazgos] 11," + vIdHallazgo;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows.Count > 0 && vTemporal == "8"){
                        LBArchivo.Text = vDatos.Rows[0]["nombre"].ToString();
                        DivFile.Visible = true;
                    } 

                    DDLModificarHallazgoEstado.SelectedValue = "0";
                    UpdatePanel4.Update();
                    LbHallazgo.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModificacionesEstadoModal();", true);
                }

                if (e.CommandName == "FinalizarHallazgo"){
                    TxFinalizarHallazgoComentario.Text = string.Empty;
                    LbFinalizarHallazgo.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openFinalizarHallazgoModal();", true);
                }

                if (e.CommandName == "AmpliarFecha"){
                    LbAutorizacionHallazgo.Text = vIdHallazgo;
                    String vQuery = "[ACSP_ObtenerHallazgos] 12," + vIdHallazgo;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    if (vDatos.Rows.Count > 0){
                        LbFecha.Text = Convert.ToDateTime(vDatos.Rows[0]["fechaAmpliacion"]).ToString("dd-MM-yyyy");
                        LbFechaActual.Text = Convert.ToDateTime(vDatos.Rows[0]["fechaResolucion"]).ToString("dd-MM-yyyy");
                        LbComentarioAmpliacion.Text = vDatos.Rows[0]["comentario"].ToString();
                        DivDocumento.Visible = vDatos.Rows[0]["nombre"].ToString() != "" ? true : false;
                        LBDocumentoAmpliacion.Text = vDatos.Rows[0]["nombre"].ToString();
                        LbHallazgoAmpliacion.Text = vIdHallazgo;
                        LbAmpliacion.Text = vDatos.Rows[0]["idAmpliacion"].ToString();
                    }
                    UpdatePanel15.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAmpliacionModal();", true);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATOSHALLAZGOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBuscarHallazgo_Click(object sender, EventArgs e){
            BuscarHallazgo();
        }
        
        private void BuscarHallazgo(String vInformeQuery){
            try{
                String vQuery = String.Empty;
                String vBusqueda = String.Empty;
                vBusqueda = vInformeQuery;

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                if (isNumeric){
                    vQuery = "[ACSP_ObtenerHallazgos] 2," + vInformeQuery + ",'" + Convert.ToString(Session["USUARIO"]) + "'";
                }else{
                    throw new Exception("No puedes buscar por ese ID");
                }

                DDLBuscarInforme.SelectedValue = vInformeQuery;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DataTable vDatosConvertidos = new DataTable();
                vDatosConvertidos.Columns.Add("idHallazgo");
                vDatosConvertidos.Columns.Add("fechaCreacion");
                vDatosConvertidos.Columns.Add("idArea");
                vDatosConvertidos.Columns.Add("tipoRiesgo");
                vDatosConvertidos.Columns.Add("detalle");
                vDatosConvertidos.Columns.Add("tipoEstadoHallazgo");

                foreach (DataRow item in vDatos.Rows){
                    String vArea = String.Empty;
                    String vRiesgo = String.Empty;
                    vQuery = "[ACSP_ObtenerTipos] 1";
                    DataTable vDatosArea = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow itemArea in vDatosArea.Rows){
                        if (itemArea["idArea"].ToString().Equals(item["idArea"].ToString()))
                            vArea = itemArea["nombre"].ToString();
                    }

                    vQuery = "[ACSP_ObtenerTipos] 2";
                    DataTable vDatosRiesgo = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow itemRiesgo in vDatosRiesgo.Rows){
                        if (itemRiesgo["tipoRiesgo"].ToString().Equals(item["tipoRiesgo"].ToString()))
                            vRiesgo = itemRiesgo["nombre"].ToString();
                    }

                    vDatosConvertidos.Rows.Add(
                        item["idHallazgo"].ToString(),
                        item["fechaCreacion"].ToString(),
                        vArea,
                        vRiesgo,
                        item["detalle"].ToString().Substring(0, (item["detalle"].ToString().Length > 20 ? 20 : item["detalle"].ToString().Length)) + "...",
                        item["tipoEstadoHallazgo"].ToString()
                        );
                }
                GVBusqueda.DataSource = vDatosConvertidos;
                mostrarOcultar();
                Session["DATOSHALLAZGOS"] = vDatosConvertidos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); GVBusqueda.DataSource = null; GVBusqueda.DataBind(); }
        }
        
        private void BuscarHallazgo(){
            try{
                String vQuery = String.Empty;
                String vBusqueda = String.Empty;
                if (DDLBuscarInforme.SelectedValue.Equals(""))//TxBuscarIdInforme.Text.Equals(""))
                    vBusqueda = TxBuscarNombre.Text;
                else
                    vBusqueda = DDLBuscarInforme.SelectedValue;//TxBuscarIdInforme.Text;

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                if (isNumeric){
                    vQuery = "[ACSP_ObtenerHallazgos] 2," + DDLBuscarInforme.SelectedValue + ",'" + Convert.ToString(Session["USUARIO"]) + "'";
                }else{
                    if (TxBuscarNombre.Text.Equals(""))
                        throw new Exception("Ingrese un nombre de informe, Busqueda no puede ir vacia");
                    vQuery = "[ACSP_ObtenerHallazgos] 4,'" + TxBuscarNombre.Text.ToUpper() + "'";
                }

                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DataTable vDatosConvertidos = new DataTable();
                vDatosConvertidos.Columns.Add("idHallazgo");
                vDatosConvertidos.Columns.Add("fechaCreacion");
                vDatosConvertidos.Columns.Add("idArea");
                vDatosConvertidos.Columns.Add("tipoRiesgo");
                vDatosConvertidos.Columns.Add("detalle");
                vDatosConvertidos.Columns.Add("tipoEstadoHallazgo");
                if (vDatos.Rows.Count > 0){
                    foreach (DataRow item in vDatos.Rows){
                        String vArea = String.Empty;
                        String vRiesgo = String.Empty;
                        vQuery = "[ACSP_ObtenerTipos] 1";
                        DataTable vDatosArea = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow itemArea in vDatosArea.Rows){
                            if (itemArea["idArea"].ToString().Equals(item["idArea"].ToString()))
                                vArea = itemArea["nombre"].ToString();
                        }

                        vQuery = "[ACSP_ObtenerTipos] 2";
                        DataTable vDatosRiesgo = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow itemRiesgo in vDatosRiesgo.Rows){
                            if (itemRiesgo["tipoRiesgo"].ToString().Equals(item["tipoRiesgo"].ToString()))
                                vRiesgo = itemRiesgo["nombre"].ToString();
                        }

                        vDatosConvertidos.Rows.Add(
                            item["idHallazgo"].ToString(),
                            item["fechaCreacion"].ToString(),
                            vArea,
                            vRiesgo,
                            item["detalle"].ToString().Substring(0, (item["detalle"].ToString().Length > 20 ? 20 : item["detalle"].ToString().Length)) + "...",
                            item["tipoEstadoHallazgo"].ToString()
                            );
                    }
                    GVBusqueda.DataSource = vDatosConvertidos;
                    mostrarOcultar();
                    Session["DATOSHALLAZGOS"] = vDatosConvertidos;
                }else{
                    vQuery = "[ACSP_ObtenerInformes] 2," + DDLBuscarInforme.SelectedValue;
                    vDatos = vConexion.obtenerDataTable(vQuery);
                    if (vDatos.Rows[0]["tipoEstado"].ToString() == "2")
                        throw new Exception("El informe ya está resuelto.");
                    else
                        throw new Exception("No existen hallazgos asignados");
                }
            }catch (Exception Ex) {
                if (Ex.Message == "El informe ya está resuelto.")
                    Mensaje(Ex.Message, WarningType.Warning); 
                else
                    Mensaje(Ex.Message, WarningType.Danger); 


                GVBusqueda.DataSource = null; 
                GVBusqueda.DataBind(); 
            }
        }

        private void mostrarOcultar(){
            String vConsulta = "[ACSP_ObtenerUsuariosInforme] 4," + DDLBuscarInforme.SelectedValue + ",'" + Session["USUARIO"].ToString() + "'";
            DataTable vDatos = vConexion.obtenerDataTable(vConsulta);

            switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                case 1:
                case 2:
                    GVBusqueda.Columns[10].Visible = false;
                    GVBusqueda.Columns[2].Visible = false;
                    break;
                case 3:
                    GVBusqueda.Columns[0].Visible = false;
                    GVBusqueda.Columns[2].Visible = false;
                    GVBusqueda.Columns[10].Visible = false;
                    break;
                case 4:
                    GVBusqueda.Columns[0].Visible = false;
                    GVBusqueda.Columns[1].Visible = false;
                    GVBusqueda.Columns[11].Visible = false;
                    
                    if (vDatos.Rows.Count > 0){
                        if (vDatos.Rows[0]["tipoEnvio"].ToString() == "2"){
                            GVBusqueda.Columns[2].Visible = false;
                            GVBusqueda.Columns[10].Visible = false;
                        }else {
                            GVBusqueda.Columns[2].Visible = true;
                            GVBusqueda.Columns[10].Visible = true;
                        }
                    }else {
                        GVBusqueda.Columns[2].Visible = true;
                        GVBusqueda.Columns[10].Visible = true;
                    }
                    break;
                case 5:
                    GVBusqueda.Columns[0].Visible = false;
                    GVBusqueda.Columns[1].Visible = false;
                    GVBusqueda.Columns[11].Visible = false;

                    if (vDatos.Rows.Count > 0){
                        if (vDatos.Rows[0]["tipoEnvio"].ToString() == "2"){
                            GVBusqueda.Columns[2].Visible = false;
                            GVBusqueda.Columns[10].Visible = false;
                        }else {
                            GVBusqueda.Columns[2].Visible = true;
                            GVBusqueda.Columns[10].Visible = true;
                        }
                    }else {
                        GVBusqueda.Columns[2].Visible = true;
                        GVBusqueda.Columns[10].Visible = true;
                    }
                    break;
                case 6:
                    GVBusqueda.Columns[0].Visible = false;
                    GVBusqueda.Columns[1].Visible = false;
                    GVBusqueda.Columns[2].Visible = false;
                    GVBusqueda.Columns[10].Visible = false;
                    GVBusqueda.Columns[11].Visible = false;
                    break;
            }

            GVBusqueda.DataBind();
            foreach (GridViewRow row in GVBusqueda.Rows){
                String vQuery = "[ACSP_ObtenerHallazgos] 7, " + row.Cells[3].Text;
                DataTable vDataEstados = vConexion.obtenerDataTable(vQuery);

                if (Convert.ToBoolean(vDataEstados.Rows[0]["estadoCerrado"])){
                    LinkButton vBtnCerrarHallazgo = row.Cells[2].FindControl("BtnFinalizarHallazgo") as LinkButton;
                    vBtnCerrarHallazgo.Enabled = false;
                    vBtnCerrarHallazgo.CssClass = "btn btn-grey";

                    LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                    vBtnCogs.Enabled = false;
                    vBtnCogs.CssClass = "btn btn-grey";

                    LinkButton vBtnClock = row.Cells[2].FindControl("BtnAmpliacion") as LinkButton;
                    vBtnClock.Enabled = false;
                    vBtnClock.CssClass = "btn btn-grey";
                }

                if (vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() == "1" || vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() == "2"){
                    LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                    vBtnCogs.Enabled = false;
                    vBtnCogs.CssClass = "btn btn-grey";
                }

                if (vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() != "2" || vDataEstados.Rows[0]["idAmpliacion"].ToString() != ""){
                    LinkButton vBtnCerrarHallazgo = row.Cells[2].FindControl("BtnFinalizarHallazgo") as LinkButton;
                    vBtnCerrarHallazgo.Enabled = false;
                    vBtnCerrarHallazgo.CssClass = "btn btn-grey";
                }

                String vAutorizacion = vConexion.ValidarAutorizacionHallazgo(row.Cells[3].Text);
                if (vAutorizacion.Equals("0")){
                    LinkButton vBtnAutorizarHallazgo = row.Cells[0].FindControl("BtnAutorizarInforme") as LinkButton;
                    vBtnAutorizarHallazgo.Enabled = false;
                    vBtnAutorizarHallazgo.CssClass = "btn btn-grey";
                }

                if (vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() == "8"){
                    if (vDataEstados.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "8" && vDataEstados.Rows[0]["usuarioCreacion"].ToString() == Session["USUARIO"].ToString()){
                        LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                        vBtnCogs.Enabled = true;
                        vBtnCogs.CssClass = "btn btn-yellow";
                    }
                    if (vDataEstados.Rows[0]["jefeAuditoria"].ToString() == Session["USUARIO"].ToString()){
                        LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                        vBtnCogs.Enabled = true;
                        vBtnCogs.CssClass = "btn btn-yellow";
                    }
                }

                if (vDataEstados.Rows[0]["asignado"].ToString() != ""){
                    LinkButton vBtnAsignar = row.Cells[2].FindControl("BtnAsignar") as LinkButton;
                    vBtnAsignar.Enabled = false;
                    vBtnAsignar.CssClass = "btn btn-grey";
                }

                if (vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() != "1"){
                    LinkButton vBtnAsignar = row.Cells[2].FindControl("BtnAsignar") as LinkButton;
                    vBtnAsignar.Enabled = false;
                    vBtnAsignar.CssClass = "btn btn-grey";
                }
                
                if (vDataEstados.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() != "" && vDataEstados.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() != "8"){
                    LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                    vBtnCogs.Enabled = false;
                    vBtnCogs.CssClass = "btn btn-grey";
                }

                if (Session["TIPOUSUARIO"].ToString() == "2" && vDataEstados.Rows[0]["tipoEstadoHallazgo"].ToString() != "3"){
                    LinkButton vBtnCogs = row.Cells[2].FindControl("BtnModificar") as LinkButton;
                    vBtnCogs.Enabled = true;
                    vBtnCogs.CssClass = "btn btn-yellow";
                }                
                
                if (vDataEstados.Rows[0]["estadoAmpliacion"].ToString() != "0"){
                    LinkButton vBtnAmpliacion = row.Cells[2].FindControl("BtnAmpliacion") as LinkButton;
                    vBtnAmpliacion.Enabled = false;
                    vBtnAmpliacion.CssClass = "btn btn-grey";
                }
            }
        }

        protected void BtnModificarHallazgo_Click(object sender, EventArgs e){
            try{
                if (DDLUsuariosAsignacionHallazgo.SelectedValue == "0")
                    throw new Exception("Por favor seleccione un usuario responsable.");

                if (!LbNumeroHallazgoModificaciones.Text.Equals("")){
                    String vQuery = String.Empty;
                    vQuery = "[ACSP_ObtenerHallazgos] 5" +
                        "," + LbNumeroHallazgoModificaciones.Text + 
                        ",'" + DDLUsuariosAsignacionHallazgo.SelectedValue + "'" +
                        ",'" + Session["USUARIO"].ToString() + "'";

                    int? vIdInforme = vConexion.ejecutarSQLGetValue(vQuery);
                    if (vIdInforme != null){
                        vQuery = "[ACSP_Hallazgos] 11,0," + vIdInforme + ",'" + DDLUsuariosAsignacionHallazgo.SelectedValue + "'";
                        int vInfo = vConexion.ejecutarSql(vQuery);

                        try{
                            vQuery = "[ACSP_ObtenerUsuariosInforme] 2,'" + DDLUsuariosAsignacionHallazgo.SelectedValue + "'";
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
                                typeBody.HallazgoAsignacion,
                                vCorreo.Usuario,
                                "Se te ha asignado un hallazgo en el informe No." + vIdInforme,
                                vCorreo.Copia
                            );
                        }catch { }
                        BuscarHallazgo();
                        Mensaje("Hallazgo modificado y asignado con exito", WarningType.Success);
                        DDLUsuariosAsignacionHallazgo.SelectedIndex = -1;
                        CerrarModal("ModificacionesModal");
                    }
                }else{
                    BuscarHallazgo();
                }
            }catch (Exception Ex) {
                LbModificacionesMensaje.Text = Ex.Message;
                UpdateModificacionesMensaje.Update();
            }
        }

        protected void BtnModificarEstadoHallazgo_Click(object sender, EventArgs e){
            try{
                if (DDLModificarHallazgoEstado.SelectedValue == "7"){
                    if (TxComentarioAuditor.Text == "")
                        throw new Exception("Por favor ingrese un comentario.");

                    DataTable vDatosHallazgo = (DataTable)Session["HALLAZGO_INFO"];
                    String vTipo = vDatosHallazgo.Rows[0]["tipoEstadoHallazgoTemporal"].ToString() == "8" ? "5" : "2";
                    String vConsul = "[ACSP_Logs] " + vTipo + "," + DDLBuscarInforme.SelectedValue + "," + LbHallazgo.Text + ",'comentarioAuditor','" + TxComentarioAuditor.Text + "','" + Session["USUARIO"].ToString() + "'";
                    vConexion.ejecutarSql(vConsul);
                }

                String vQuery = "[ACSP_Hallazgos] 4" +
                    "," + LbHallazgo.Text +
                    "," + DDLModificarHallazgoEstado.SelectedValue + 
                    ",0,0,0,'" + TxComentarioAuditor.Text + "'";

                if (vConexion.ejecutarSql(vQuery).Equals(1)){
                    try{
                        if (Session["TIPOUSUARIO"].ToString() == "2")
                            vQuery = "[ACSP_ObtenerUsuarios] 4," + Convert.ToString(Session["USUARIO"]);
                        else
                            vQuery = "[ACSP_ObtenerUsuarios] 5," + Convert.ToString(Session["USUARIO"]);

                        DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);
                        Correo vCorreo = new Correo();
                        foreach (DataRow item in vDatosResponsables.Rows){
                            vCorreo.Usuario = vConexion.GetNombreUsuario(item["idUsuario"].ToString());
                            vCorreo.Para = item["correo"].ToString();
                            vCorreo.Copia = "";
                        }
                        //String vUser = vDatosResponsables.Rows[0]["idUsuario"].ToString();
                        //tokenClass vClassToken = new tokenClass(){
                        //    usuario = Convert.ToInt32(vUser)
                        //};
                        //String vTokenString = vToken.Encrypt(JsonConvert.SerializeObject(vClassToken), ConfigurationManager.AppSettings["TOKEN_DOC"].ToString());
                        ////REVISAR CONSULTA
                        //vQuery = "[ACSP_Token] 1,'" + vUser + "','" + vTokenString + "','" + Session["USUARIO"] + "'";
                        //vConexion.ejecutarSql(vQuery);

                        SmtpService vSmtpService = new SmtpService();
                        vSmtpService.EnviarMensaje(
                            vCorreo.Para,
                            typeBody.General,
                            vCorreo.Usuario,
                            "Se ha hecho un cambio en el estado del hallazgo no." + LbHallazgo.Text + @", por favor reviselo para la debida autorización <br \><br \>" +
                            "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                            vCorreo.Copia
                            );
                    }
                    catch { }

                    MensajeLoad("Hallazgo modificado con exito", WarningType.Success);
                    DDLModificarHallazgoEstado.SelectedIndex = -1;
                    CerrarModal("ModificacionesEstadoModal");
                }else
                    throw new Exception("Error al ingresar el hallazgo, contacte a sistemas.");

                DDLModificarHallazgoEstado.SelectedIndex = -1;
                BuscarHallazgo();
            }catch (Exception Ex){
                if (Ex.Message == "Por favor ingrese un comentario."){
                    LbHallazgoEstadoMensaje.Text = Ex.Message;
                    UpdatePanel5.Update();
                }else
                    MensajeLoad(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnEnviarAutorizacion_Click(object sender, EventArgs e){
            try{
                Correo vCorreo = new Correo();
                if (!Convert.ToBoolean(Session["ESTADO_AMPLIACION"])){
                    String vQuery = "[ACSP_Hallazgos] 5," +
                        "" + LbAutorizacionHallazgo.Text;

                    if (vConexion.ejecutarSql(vQuery).Equals(1) || vConexion.ejecutarSql(vQuery).Equals(2)){
                        try{
                            vQuery = "[ACSP_ObtenerHallazgos] 1,'" + LbAutorizacionHallazgo.Text + "'";
                            DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                            String vComent = LbEstadoTemporal.Text == "No aprobado" ? "El comentario del Auditor fue: <b>" + TxComentario.Text + "</b>" : "";
                            String vUsuarioResponsable = String.Empty;
                            foreach (DataRow item in vDatosHallazgo.Rows){
                                vUsuarioResponsable = item["usuarioResponsable"].ToString();
                            }

                            vQuery = "[ACSP_ObtenerUsuarios] 5," + vUsuarioResponsable;
                            DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

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
                                "Se ha realizado un cambio en el estado del hallazgo no." + LbAutorizacionHallazgo.Text + @", por favor revisar <br \>" + vComent + @"<br \>" + 
                                "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                                vCorreo.Copia
                            );
                        }catch { }

                        vQuery = "[ACSP_Informes] 7," + DDLBuscarInforme.SelectedValue;//TxBuscarIdInforme.Text;
                        vConexion.ejecutarSql(vQuery);

                        vQuery = "[ACSP_ObtenerHallazgos] 9," + LbAutorizacionHallazgo.Text;
                        int? vEstadoActual = vConexion.ejecutarSQLGetValue(vQuery);
                        if (vEstadoActual.Equals(7)){
                            vQuery = "[ACSP_Hallazgos] 9," + LbAutorizacionHallazgo.Text;
                            vConexion.ejecutarSql(vQuery);
                        }

                        if (LbEstadoTemporal.Text == "Cerrado"){
                            vQuery = "[ACSP_Logs] 7,6" +
                                "," + DDLBuscarInforme.SelectedValue + 
                                "," + LbAutorizacionHallazgo.Text +
                                ",NULL,'Cierre de hallazgo'," + Session["USUARIO"].ToString();

                            vConexion.ejecutarSql(vQuery);
                        }

                        DDLModificarHallazgoEstado.SelectedIndex = -1;
                        MensajeLoad("Hallazgo autorizado con exito", WarningType.Success);
                    }else
                        throw new Exception("Error al ingresar el hallazgo, contacte a sistemas.");
                }else{
                    String vQuery = "";

                    string vFecha = Convert.ToDateTime(LbFechaSolicitada.Text).ToString("dd-MM-yyyy");
                    if (LbEstadoTemporal.Text == "Aprobar Ampliación")
                        vQuery = "[ACSP_Ampliaciones] 5," + LbAutorizacionHallazgo.Text + ",'" + Convert.ToDateTime(LbFechaSolicitada.Text).ToString("MM-dd-yyyy") + "'";
                    else
                        vQuery = "[ACSP_Ampliaciones] 7," + LbAutorizacionHallazgo.Text;

                    if (vConexion.ejecutarSql(vQuery) == 1){
                        vQuery = "[ACSP_ObtenerUsuarios] 10," + LbAutorizacionHallazgo.Text;
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow item in vDatos.Rows){
                            vCorreo.Usuario = vConexion.GetNombreUsuario(item["idUsuario"].ToString());
                            vCorreo.Para = item["correo"].ToString();
                            vCorreo.Copia = "";
                        }

                        SmtpService vSmtpService = new SmtpService();
                        vSmtpService.EnviarMensaje(
                            vCorreo.Para,
                            typeBody.General,
                            vCorreo.Usuario,
                            "Se ha realizado un cambio en el hallazgo no." + LbAutorizacionHallazgo.Text + @", por favor revisar <br \><br \>" + 
                            "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                            vCorreo.Copia
                        );
                        MensajeLoad("Hallazgo autorizado con exito", WarningType.Success);
                    } 
                }

                BuscarHallazgo();
                CerrarModal("AutorizacionModal");
            }catch (Exception Ex) { 
                MensajeLoad(Ex.Message, WarningType.Danger); 
                CerrarModal("AutorizacionModal"); 
            }
        }

        protected void BtnFinalizarHallazgo_Click(object sender, EventArgs e){
            try{
                if (TxFinalizarHallazgoComentario.Text.Equals(""))
                    throw new Exception("Por favor ingrese un comentario.");

                String vConsul = "[ACSP_Logs] 3," + DDLBuscarInforme.SelectedValue + "," + LbFinalizarHallazgo.Text + ",'comentarios','" + TxFinalizarHallazgoComentario.Text + "','" + Session["USUARIO"].ToString() + "'";
                vConexion.ejecutarSql(vConsul);

                String vQuery = "[ACSP_Hallazgos] 6," + LbFinalizarHallazgo.Text + ",0,'','" + TxFinalizarHallazgoComentario.Text + "'";
                if (vConexion.ejecutarSql(vQuery).Equals(1)){
                    String vArchivo = "NULL";

                    if (FUHallazgos.HasFile){
                        String vNombreDeposito = String.Empty;
                        HttpPostedFile bufferDeposito1T = FUHallazgos.PostedFile;
                        byte[] vFileDeposito1 = null;
                        if (bufferDeposito1T != null){
                            vNombreDeposito = FUHallazgos.FileName;
                            Stream vStream = bufferDeposito1T.InputStream;
                            BinaryReader vReader = new BinaryReader(vStream);
                            vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                        }
                        String vDeposito = Convert.ToBase64String(vFileDeposito1);

                        vQuery = "[ACSP_Archivos] 1,'" + vNombreDeposito + "','" + vDeposito + "',1";
                        int? vInfo = vConexion.ejecutarSQLGetValue(vQuery);
                        if (vInfo != null){
                            vArchivo = Convert.ToString(vInfo);
                            vQuery = "[ACSP_Archivos] 2,'',''," + LbFinalizarHallazgo.Text + "," + vInfo;
                            vConexion.ejecutarSql(vQuery);
                        }
                    }
                    //Actualiza idArchivoCierre del hallazgo
                    vQuery = "[ACSP_Archivos] 3,'',''," + LbFinalizarHallazgo.Text + "," + vArchivo;
                    vConexion.ejecutarSql(vQuery);

                    vQuery = "[ACSP_ObtenerHallazgos] 6,'" + LbFinalizarHallazgo.Text + "'";
                    DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);
                    String vUsuarioCreacion = String.Empty;
                    foreach (DataRow item in vDatosHallazgo.Rows){
                        vUsuarioCreacion = item["usuarioCreacion"].ToString();
                    }

                    vQuery = "[ACSP_ObtenerUsuarios] 5," + vUsuarioCreacion;
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
                        "Se ha actualizado el hallazgo No." + LbFinalizarHallazgo.Text + @" para cierre, por favor revisar <br \><br \>" +
                        "Creado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                        );

                    if (vInformeQuery != null)
                        BuscarHallazgo(vInformeQuery);
                    else
                        BuscarHallazgo(DDLBuscarInforme.SelectedValue);//TxBuscarIdInforme.Text);

                    MensajeLoad("Hallazgo actualizado con exito", WarningType.Success);
                    CerrarModal("FinalizarHallazgoModal");

                    TxFinalizarHallazgoComentario.Text = "";
                    LbMensajeCierreHallazgo.Text = "";
                }

            }
            catch (Exception Ex) { LbMensajeCierreHallazgo.Text = Ex.Message; UpdatePanel10.Update(); }
        }

        protected void BtnRechazarAutorizacion_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_Hallazgos] 12,'" + LbAutorizacionHallazgo.Text + "'";
                int vInfo = vConexion.ejecutarSql(vQuery);
                
                if (vInfo == 1){
                    vQuery = "[ACSP_ObtenerHallazgos] 6,'" + LbAutorizacionHallazgo.Text + "'";
                    DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                    Correo vCorreo = new Correo();
                    foreach (DataRow item in vDatosHallazgo.Rows){
                        vCorreo.Usuario = vConexion.GetNombreUsuario(item["usuarioResponsable"].ToString());
                        vCorreo.Para = item["correo"].ToString();
                        vCorreo.Copia = "";
                    }

                    SmtpService vSmtpService = new SmtpService();
                    vSmtpService.EnviarMensaje(
                        vCorreo.Para,
                        typeBody.General,
                        vCorreo.Usuario,
                        "Se ha realizado un cambio de estado en el hallazgo No." + LbAutorizacionHallazgo.Text + @" el cual cambio a un estatus de No Aprobado, favor validarlo y comunicarse con el auditor a cargo para tener mas información.<br \><br \>" +
                        "Modificado por: " + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                        );
                }
                Mensaje("Hallazgo actualizado con exito", WarningType.Success);
                CerrarModal("AutorizacionModal");
                BuscarHallazgo();
            }catch (Exception ex){
                Mensaje(ex.Message,WarningType.Danger);
            }
        }

        protected void DDLModificarHallazgoEstado_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivComentario.Visible = DDLModificarHallazgoEstado.SelectedValue == "7" ? true : false;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void LBArchivo_Click(object sender, EventArgs e){
            try{
                String vIdArchivo = LbHallazgo.Text;
                String vQuery = "[ACSP_ObtenerHallazgos] 11, " + vIdArchivo;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                
                String vArchivo = vDatos.Rows[0]["archivo"].ToString();

                if (vArchivo != ""){
                    byte[] fileData = null;

                    if (!vArchivo.Equals(""))
                        fileData = Convert.FromBase64String(vArchivo);
                    
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", GetExtension(vDatos.Rows[0]["nombre"].ToString()));
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + LBArchivo.Text);
                    Response.End();
                }else
                    throw new Exception("No existe archivo");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        private string GetExtension(string Extension){
            switch (Extension){
                case ".doc":
                    return "application/ms-word";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".ppt":
                    return "application/mspowerpoint";
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".zip":
                    return "application/zip";
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".wav":
                    return "audio/wav";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }

        protected void LBDocumentoAmpliacion_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_ObtenerHallazgos] 13," + LbAmpliacion.Text;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                String vArchivo = vDatos.Rows[0]["archivo"].ToString();

                if (vArchivo != ""){
                    byte[] fileData = null;

                    if (!vArchivo.Equals(""))
                        fileData = Convert.FromBase64String(vArchivo);
                    
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", GetExtension(vDatos.Rows[0]["nombre"].ToString()));
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + LBDocumentoAmpliacion.Text);
                    Response.End();
                }else
                    throw new Exception("No existe archivo");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void BtnAprobarAmpliacion_Click(object sender, EventArgs e){
            try{
                Correo vCorreo = new Correo();
                String vQuery = "";
                String vMensaje = DDLAccion.SelectedValue == "0" ? "Aprobado" : "Rechazado";

                if (DDLAccion.SelectedValue == "0")
                    vQuery = "[ACSP_Ampliaciones] 3," + LbHallazgoAmpliacion.Text + "," + LbAmpliacion.Text + ",1";
                else if (DDLAccion.SelectedValue == "1"){
                    if (TxRechazarComentario.Text == "" || TxRechazarComentario.Text == string.Empty)
                        throw new Exception("Por favor ingrese un comentario.");
                    

                    vQuery = "[ACSP_Ampliaciones] 3," + LbHallazgoAmpliacion.Text + "," + LbAmpliacion.Text + ",2";

                    String vConsulta = "[ACSP_Logs] 7, 7" +
                        "," + DDLBuscarInforme.SelectedValue + 
                        "," + LbAutorizacionHallazgo.Text + 
                        ",NULL,'" + TxRechazarComentario.Text + "'" +
                        ",'" + Session["USUARIO"].ToString() + "'";
                    vConexion.ejecutarSql(vConsulta);

                }

                int vInfo = vConexion.ejecutarSql(vQuery);
                if (vInfo == 2){
                    vQuery = "[ACSP_ObtenerUsuarios] 11," + LbAutorizacionHallazgo.Text;
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                    foreach (DataRow item in vDatos.Rows){
                        vCorreo.Usuario = vConexion.GetNombreUsuario(item["idUsuario"].ToString());
                        vCorreo.Para = item["correo"].ToString();
                        vCorreo.Copia = "";
                    }

                    SmtpService vSmtpService = new SmtpService();
                    vSmtpService.EnviarMensaje(
                        vCorreo.Para,
                        typeBody.General,
                        vCorreo.Usuario,
                        "Se ha " + vMensaje + " una ampliación para el hallazgo no." + LbAutorizacionHallazgo.Text + @", por favor revisar.<br \><br \>" + 
                        "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                    );
                }

                if (vInformeQuery != null)
                    BuscarHallazgo(vInformeQuery);
                else
                    BuscarHallazgo(DDLBuscarInforme.SelectedValue);
                    
                Mensaje("Cambio realizado con éxito.", WarningType.Success);
                CerrarModal("AmpliacionModal");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void LBDocAmpliacion_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_ObtenerHallazgos] 13," + LbAmpliacion.Text;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                String vArchivo = vDatos.Rows[0]["archivo"].ToString();

                if (vArchivo != ""){
                    byte[] fileData = null;

                    if (!vArchivo.Equals(""))
                        fileData = Convert.FromBase64String(vArchivo);
                    
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Type", GetExtension(vDatos.Rows[0]["nombre"].ToString()));
                    byte[] bytFile = fileData;
                    Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                    Response.AddHeader("Content-disposition", "attachment;filename=" + LBDocAmpliacion.Text);
                    Response.End();
                }else
                    throw new Exception("No existe archivo");
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }

        protected void DDLAccion_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivComentRechazo.Visible = DDLAccion.SelectedValue == "0" ? false : true;
                UpdatePanel15.Update();
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}