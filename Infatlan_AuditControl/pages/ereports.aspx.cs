using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class ereports : System.Web.UI.Page
    {
        db vConexion = new db();
        String vIdInforme = string.Empty;
        protected void Page_Load(object sender, EventArgs e){
            String vEx = Request.QueryString["ex"];
            vIdInforme = Request.QueryString["id"];
            if (!Page.IsPostBack){
                getInformes();
                getArea();
                getRiesgo();
                getEmpresas();

                if (vEx != null){
                    if (vEx.Equals("1"))
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Hallazgo creado con exito" + "')", true);
                    if (vEx.Equals("2"))
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Pop", "window.alert('" + "Hallazgo modificado con exito!" + "')", true);
                }
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void MensajeLoad(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", " document.addEventListener(\"DOMContentLoaded\", function (event) { infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "'); });", true);
        }

        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }

        void getInformes(){
            try{
                String vQuery = "[ACSP_ObtenerInformes] 5,0,'" + Convert.ToString(Session["USUARIO"]) + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                vDatos.Columns.Add("fechaRes");
                for (int i = 0; i < vDatos.Rows.Count; i++){
                    String vFecha = Convert.ToDateTime(vDatos.Rows[i]["fechaRespuesta"]).ToString("dd-MM-yyyy");
                    vDatos.Rows[i]["fechaRes"] = vFecha;
                }

                GVBusqueda.DataSource = vDatos;
                Session["BUSQUEDAINFORMES"] = vDatos;
                mostrarOcultar();
            }catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void mostrarOcultar() {
            switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                case 2:
                    GVBusqueda.Columns[10].Visible = false;
                    break;
                case 3:
                    GVBusqueda.Columns[11].Visible = false;
                    break;
                case 1:
                case 4:
                case 5:
                    GVBusqueda.Columns[9].Visible = false;
                    GVBusqueda.Columns[10].Visible = false;
                    GVBusqueda.Columns[11].Visible = false;
                    break;
                case 6:
                    GVBusqueda.Columns[9].Visible = false;
                    GVBusqueda.Columns[10].Visible = false;
                    GVBusqueda.Columns[11].Visible = false;
                    break;
            }
            GVBusqueda.DataBind();

            foreach (GridViewRow row in GVBusqueda.Rows){
                String vQuery = "[ACSP_ObtenerInformes] 6," + row.Cells[2].Text + ",'" + Session["USUARIO"].ToString() + "'";
                DataTable vDatosEnvio = vConexion.obtenerDataTable(vQuery);

                foreach (DataRow item in vDatosEnvio.Rows){
                    if (!row.Cells[7].Text.Equals(Convert.ToString(Session["USUARIO"]))){
                        LinkButton vBtnJefatura = row.Cells[10].FindControl("BtnEnviarRevision") as LinkButton;
                        vBtnJefatura.Enabled = false;
                        vBtnJefatura.CssClass = "btn btn-grey";
                    }

                    DataTable vDatosJefatura = vConexion.obtenerDataTable("ACSP_Login '" + row.Cells[7].Text + "'");
                    if (!vDatosJefatura.Rows[0]["jefeAuditoria"].Equals(Convert.ToString(Session["USUARIO"]))){
                        LinkButton vBtnResponsables = row.Cells[11].FindControl("BtnEnviarResponsable") as LinkButton;
                        vBtnResponsables.Enabled = false;
                        vBtnResponsables.CssClass = "btn btn-grey";
                    }

                    if (Convert.ToBoolean(item["envioJefatura"].ToString())){
                        LinkButton vBtnJefatura = row.Cells[10].FindControl("BtnEnviarRevision") as LinkButton;
                        vBtnJefatura.Enabled = false;
                        vBtnJefatura.CssClass = "btn btn-grey";
                    }

                    if (Convert.ToBoolean(item["envioResponsables"].ToString())){
                        LinkButton vBtnResponsables = row.Cells[11].FindControl("BtnEnviarResponsable") as LinkButton;
                        vBtnResponsables.Enabled = false;
                        vBtnResponsables.CssClass = "btn btn-grey";

                        LinkButton vBtnRevision = row.Cells[10].FindControl("BtnEnviarRevision") as LinkButton;
                        vBtnRevision.Enabled = false;
                        vBtnRevision.CssClass = "btn btn-grey";

                        LinkButton vBtnAgregar = row.Cells[9].FindControl("BtnAsignar") as LinkButton;
                        vBtnAgregar.Enabled = false;
                        vBtnAgregar.CssClass = "btn btn-grey";
                    }
                    // nueva validacion
                    if (row.Cells[9].Text.Equals("Resuelto")){
                        LinkButton vBtnAsignar = row.Cells[9].FindControl("BtnAsignar") as LinkButton;
                        vBtnAsignar.Enabled = false;
                        vBtnAsignar.CssClass = "btn btn-grey";
                    }

                    if (!Convert.ToBoolean(item["envioJefatura"].ToString()) && row.Cells[7].Text == Convert.ToString(Session["USUARIO"]) && !Convert.ToBoolean(item["envioResponsables"].ToString())){
                        LinkButton vBtnJefatura = row.Cells[11].FindControl("BtnEnviarResponsable") as LinkButton;
                        vBtnJefatura.Enabled = true;
                        vBtnJefatura.CssClass = "btn btn-info";
                    }
                }

                vQuery = "[ACSP_ObtenerInformes] 4, " + row.Cells[2].Text;
                DataTable vData = vConexion.obtenerDataTable(vQuery);

                String vArchivo = String.Empty;

                try{
                    vArchivo = vData.Rows[0]["archivo"].ToString();
                }catch {
                    vArchivo = null;
                }   
                        
                if (vArchivo == null || vArchivo.Equals("")){
                    LinkButton vBtnDescarga = row.Cells[2].FindControl("BtnDescargarInforme") as LinkButton;
                    vBtnDescarga.Enabled = false;
                    vBtnDescarga.CssClass = "btn btn-grey";
                }
            }
        }

        void buscarInforme(String vIdInforme, Boolean vRevision){
            try{
                DataTable vDatos = (DataTable)Session["BUSQUEDAINFORMES"];
                TxBuscarIdInforme.Text = String.Empty;
                TxBuscarNombre.Text = String.Empty;

                if (vRevision){
                    GVBusqueda.DataSource = vDatos;
                    mostrarOcultar();
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        void getArea(){
            try{
                String vQuery = "[ACSP_ObtenerTipos] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLHallazgoArea.Items.Add(new ListItem { Value="0", Text="Seleccione una area" });
                DDLModificarHallazgosArea.Items.Add(new ListItem { Value = "0", Text = "Seleccione una area" });
                foreach (DataRow item in vDatos.Rows){
                    DDLHallazgoArea.Items.Add(new ListItem { Value = item["idArea"].ToString(), Text = item["idArea"].ToString() + "-" + item["nombre"].ToString() });
                    DDLModificarHallazgosArea.Items.Add(new ListItem { Value = item["idArea"].ToString(), Text = item["idArea"].ToString() + "-" + item["nombre"].ToString() });
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void getRiesgo(){
            try{
                String vQuery = "[ACSP_ObtenerTipos] 2";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLHallazgoRiesgo.Items.Add(new ListItem { Value = "0", Text = "Seleccione un riesgo" });
                DDLModificarHallazgosNivelRiesgo.Items.Add(new ListItem { Value = "0", Text = "Seleccione un riesgo" });
                foreach (DataRow item in vDatos.Rows){
                    DDLHallazgoRiesgo.Items.Add(new ListItem { Value = item["tipoRiesgo"].ToString(), Text = item["tipoRiesgo"].ToString() + "-" + item["nombre"].ToString() });
                    DDLModificarHallazgosNivelRiesgo.Items.Add(new ListItem { Value = item["tipoRiesgo"].ToString(), Text = item["tipoRiesgo"].ToString() + "-" + item["nombre"].ToString() });
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        void getEmpresas(){
            try{
                String vQuery = "[ACSP_Empresas] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLEmpresa.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatos.Rows){
                    DDLEmpresa.Items.Add(new ListItem { Value = item["idEmpresa"].ToString(), Text = item["idEmpresa"].ToString() + "-" + item["nombre"].ToString() });
                }
                DDLEmpresa.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void getResponsables(String vIdInforme){
            try{
                LdapService vLdap = new LdapService();
                String vQuery = "[ACSP_ObtenerUsuariosInforme] 1," + vIdInforme;
                DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

                DDLHallazgoResponsable.Items.Clear();
                DDLModificarHallazgosResponsable.Items.Clear();
                DDLHallazgoResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione un estado" });
                DDLModificarHallazgosResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione un estado" });
                foreach (DataRow item in vDatosResponsables.Rows){
                    DDLHallazgoResponsable.Items.Add(new ListItem { Value = item["idUsuario"].ToString(), Text = vConexion.GetNombreUsuario(item["idUsuario"].ToString()) });
                    DDLModificarHallazgosResponsable.Items.Add(new ListItem { Value = item["idUsuario"].ToString(), Text = vConexion.GetNombreUsuario(item["idUsuario"].ToString()) });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                string vIdInforme = e.CommandArgument.ToString();
                if (e.CommandName == "EntrarInforme"){
                    LbNumeroInformeHallazgos.Text = vIdInforme;

                    String vQuery = "[ACSP_ObtenerHallazgos] 2," + LbNumeroInformeHallazgos.Text + ",'" + Convert.ToString(Session["USUARIO"]) + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    DataTable vDatosConvertidos = new DataTable();
                    vDatosConvertidos.Columns.Add("idHallazgo");
                    vDatosConvertidos.Columns.Add("fechaCreacion");
                    vDatosConvertidos.Columns.Add("idArea");
                    vDatosConvertidos.Columns.Add("tipoRiesgo");
                    vDatosConvertidos.Columns.Add("detalle");
                    vDatosConvertidos.Columns.Add("usuarioResponsable");

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
                            item["detalle"].ToString().Substring(0,(item["detalle"].ToString().Length > 20 ? 20 : item["detalle"].ToString().Length)) + "...",
                            vConexion.GetNombreUsuario(item["usuarioResponsable"].ToString())
                            );


                    }
                    GVHallazgosView.DataSource = vDatosConvertidos;

                    vQuery = "[ACSP_ObtenerInformes] 6, " + vIdInforme + ",'" + Session["USUARIO"].ToString() + "'";
                    DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 1:
                        case 2:
                            //GVHallazgosView.Columns[0].Visible = true;
                            break;
                        case 3:
                            if (Convert.ToBoolean(vDatosHallazgo.Rows[0][0].ToString()))
                                GVHallazgosView.Columns[0].Visible = false;
                            else
                                GVHallazgosView.Columns[0].Visible = true;
                            break;
                        case 4:
                            GVHallazgosView.Columns[0].Visible = false;
                            break;

                        case 5:
                            if (Convert.ToBoolean(vDatosHallazgo.Rows[0][0].ToString()))
                                GVHallazgosView.Columns[0].Visible = false;
                            else
                                GVHallazgosView.Columns[0].Visible = true;
                            break;
                    }

                    Boolean vEnviado = Convert.ToBoolean(vDatosHallazgo.Rows[0]["envioResponsables"].ToString());
                    GVHallazgosView.Columns[0].Visible = vEnviado ? false : true;
                    GVHallazgosView.Columns[8].Visible = vEnviado ? false : true;
                    BtnEntrarInf.Visible = vEnviado ? false : true;
                    GVHallazgosView.DataBind();
                    UpdateHallazgosMain.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHallazgos();", true);
                }    
                
                if (e.CommandName == "CrearHallazgos"){
                    LimpiarHallazgos();
                    LbNumeroInformeHallazgosCreacion.Text = vIdInforme;
                    getResponsables(vIdInforme);
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    String vFecha = GVBusqueda.Rows[RowIndex].Cells[5].Text;
                    TxFechaCumplimiento.Text = DateTime.Parse(vFecha).ToString("yyyy-MM-dd");
                    TxFechaCumplimiento.ReadOnly = true;
                    UpdateHallazgosCreacionMain.Update();
                    //UpdateHallazgosCreacionMain2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHallazgosCreacion();", true);
                }
                

                if (e.CommandName == "EnviarJefatura"){
                    LbRevisionInforme.Text = vIdInforme;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openRevisionModal();", true);
                }

                if (e.CommandName == "EnviarResponsable"){
                    LbResponsablesInforme.Text = vIdInforme;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openResponsablesModal();", true);
                }

                if (e.CommandName == "DescargarInforme"){
                    try{
                        String vQuery = "[ACSP_ObtenerInformes] 4, "+ vIdInforme;
                        DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                        String vArchivo = vDatos.Rows[0]["archivo"].ToString();
                        byte[] fileData = null;

                        if (!vArchivo.Equals(""))
                            fileData = Convert.FromBase64String(vArchivo);

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //Response.AppendHeader("Content-Type", GetExtension(vDatos.Rows[0]["nombreArchivo"].ToString()));
                        byte[] bytFile = fileData;
                        Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                        Response.AddHeader("Content-disposition", "attachment;filename=" + vDatos.Rows[0]["nombreArchivo"].ToString());
                        Response.Flush();
                        Response.End();
                    }
                    catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
                }

                if (e.CommandName == "ReporteInforme")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('/pages/resume.aspx?id=" + vIdInforme + "','_blank')", true);
                
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
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
                case ".rar":
                    return "application/rar";
                case ".7-zip":
                    return "application/7-zip";
                default:
                    return "application/octet-stream";
            }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["BUSQUEDAINFORMES"];
                mostrarOcultar();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBuscarInforme_Click(object sender, EventArgs e){
            try{
                String vBusqueda = String.Empty;
                if (TxBuscarIdInforme.Text.Equals(""))
                    vBusqueda = TxBuscarNombre.Text;
                else
                    vBusqueda = TxBuscarIdInforme.Text;

                DataTable vDatos = (DataTable)Session["BUSQUEDAINFORMES"];
                TxBuscarIdInforme.Text = String.Empty;
                TxBuscarNombre.Text = String.Empty;

                if (vBusqueda.Equals("")){
                    GVBusqueda.DataSource = vDatos;
                    mostrarOcultar();
                }else{
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombre").ToUpper().Contains(vBusqueda.ToUpper()));

                    Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                    if (isNumeric){
                        if (filtered.Count() == 0){
                            filtered = vDatos.AsEnumerable().Where(r =>
                                Convert.ToInt32(r["idInforme"]) == Convert.ToInt32(vBusqueda));
                        }
                    }

                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idInforme");
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("fechaRespuesta");
                    vDatosFiltrados.Columns.Add("fechaRes");
                    vDatosFiltrados.Columns.Add("fechaCreacion");
                    vDatosFiltrados.Columns.Add("fechaEmision");
                    vDatosFiltrados.Columns.Add("usuarioCreacion");
                    vDatosFiltrados.Columns.Add("tipoEstado");

                    foreach (DataRow item in filtered){
                        vDatosFiltrados.Rows.Add(
                            item["idInforme"].ToString(),
                            item["nombre"].ToString(),
                            item["fechaRespuesta"].ToString(),
                            item["fechaRes"].ToString(),
                            item["fechaCreacion"].ToString(),
                            item["fechaEmision"].ToString(),
                            item["usuarioCreacion"].ToString(),
                            item["tipoEstado"].ToString()
                            );
                    }

                    GVBusqueda.DataSource = vDatosFiltrados;
                    mostrarOcultar();
                    Session["DATAEMPLEADOS"] = vDatosFiltrados;
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnHallazgosCreacionInforme_Click(object sender, EventArgs e){
            try{
                if (DDLHallazgoResponsable.SelectedIndex.Equals(0))
                    throw new Exception("Seleccione un responsable");
                if (DDLHallazgoArea.SelectedIndex.Equals(0))
                    throw new Exception("Seleccione una area");
                if (DDLEmpresa.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione una empresa.");
                if (DDLHallazgoRiesgo.SelectedIndex.Equals(0))
                    throw new Exception("Seleccione un riesgo");
                if (TxHallazgoDetalle.Text.Equals(""))
                    throw new Exception("Detalle debe de ir lleno");
                if (TxHallazgoRiesgoDetalle.Text.Equals(""))
                    throw new Exception("Riesgo debe de ir lleno");
                if (TxHallazgoRecomendacionDetalle.Text.Equals(""))
                    throw new Exception("Recomendaciones debe ir lleno");
                if (TxFechaCumplimiento.Text.Equals(""))
                    throw new Exception("Ingrese una fecha de cumplimiento");

                String vQuery = "[ACSP_Hallazgos] 1,0,0," +
                    "'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "'" + TxHallazgoDetalle.Text.Replace("'", "") + "'," +
                    "'" + TxHallazgoRiesgoDetalle.Text.Replace("'", "") + "'," +
                    "'" + TxHallazgoRecomendacionDetalle.Text.Replace("'", "") + "'," +
                    "''," +
                    "'" + TxFechaCumplimiento.Text + "'," +
                    "1," +
                    "'" + TxHallazgoFuente.Text.Replace("'", "") + "'" +
                    "," + DDLHallazgoArea.SelectedValue +
                    "," + DDLHallazgoRiesgo.SelectedValue + ",'',''," +
                    "'" + DDLHallazgoResponsable.SelectedValue + "'" + 
                    "," + DDLEmpresa.SelectedValue;

                int? vIdHallazgo = vConexion.ejecutarSQLGetValue(vQuery);
                if (vIdHallazgo != null){
                    vQuery = "[ACSP_Hallazgos] 3," + vIdHallazgo + "," + LbNumeroInformeHallazgosCreacion.Text + "";
                    if (vConexion.ejecutarSql(vQuery).Equals(1)){
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
                            vQuery = "[ACSP_Archivos] 1,'" + vNombreDeposito + "','" + vDeposito + "',0";
                            int? vIdArchivo = vConexion.ejecutarSQLGetValue(vQuery);
                            if (vIdArchivo != null){
                                vQuery = "[ACSP_Archivos] 2,'',''," + vIdHallazgo + "," + vIdArchivo ;
                                int vCreado = vConexion.ejecutarSql(vQuery);
                            }
                        }
                    } 

                    Response.Redirect("/pages/ereports.aspx?ex=1&id=" + LbNumeroInformeHallazgosCreacion.Text);
                }else
                    throw new Exception("Error al ingresar el hallazgo, contacte a sistemas.");
                
                CerrarModal("HallazgosCreacionModal");
            }catch (Exception Ex) { 
                CerrarModal("HallazgosCreacionModal");
                MensajeLoad(Ex.Message, WarningType.Danger); 
            }
        }

        void LimpiarHallazgos(){
            DDLHallazgoArea.SelectedIndex = -1;
            DDLHallazgoRiesgo.SelectedIndex = -1;
            DDLEmpresa.SelectedIndex = -1;
            TxHallazgoDetalle.Text = String.Empty;
            TxHallazgoRiesgoDetalle.Text = String.Empty;
            TxHallazgoRecomendacionDetalle.Text = String.Empty;
            TxHallazgoFuente.Text = String.Empty;
            TxFechaCumplimiento.Text = String.Empty;
        }

        protected void GVHallazgosView_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                String vIdHallazgo = e.CommandArgument.ToString();
                if (e.CommandName == "EntrarHallazgo"){
                    Response.Redirect("/pages/findings.aspx?id=" + vIdHallazgo + "&i=" + LbNumeroInformeHallazgos.Text);
                }

                if (e.CommandName == "EditarHallazgo"){
                    LbModificarHallazgoLabel.Text = vIdHallazgo;
                    String vQuery = "[ACSP_ObtenerHallazgos] 3," + vIdHallazgo;
                    int? vInforme = vConexion.ejecutarSQLGetValue(vQuery);
                    if(vInforme != null)
                        getResponsables(vInforme.ToString());
                    
                    vQuery = "[ACSP_ObtenerHallazgos] 1," + vIdHallazgo;
                    DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow item in vDatosHallazgo.Rows){
                        DDLModificarHallazgosResponsable.SelectedIndex = GetDDLIndex(DDLModificarHallazgosResponsable, item["usuarioResponsable"].ToString());
                        DDLModificarHallazgosArea.SelectedIndex = GetDDLIndex(DDLModificarHallazgosArea, item["idArea"].ToString());
                        DDLModificarHallazgosNivelRiesgo.SelectedIndex = GetDDLIndex(DDLModificarHallazgosNivelRiesgo, item["tipoRiesgo"].ToString());
                        TxModificarHallazgosHallazgo.Text = item["detalle"].ToString();
                        TxModificarHallazgosRiesgo.Text = item["riesgo"].ToString();
                        TxModificarHallazgosRecomendaciones.Text = item["recomendaciones"].ToString();
                        TxModificarHallazgosFuente.Text = item["fuente"].ToString();
                        TxModificarHallazgosFechaRespuesta.Text = Convert.ToDateTime(item["fechaPrevistaResolucion"].ToString()).ToString("yyyy-MM-dd") ;
                        TxFileName.Text = item["archivo"].ToString();
                    }
                    UpdatePanel7.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHallazgosModificacionCreacionModal();", true);
                }

                if (e.CommandName == "BorrarHallazgo"){
                    String vQuery = "[ACSP_ObtenerHallazgos] 14," + vIdHallazgo;
                    int vResultado = vConexion.ejecutarSql(vQuery);

                    if (vResultado > 0){
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalHallazgos();", true);
                        Mensaje("Hallazgo eliminado con éxito.", WarningType.Success);
                    }
                }
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        int GetDDLIndex(DropDownList vDDL, String vValue){
            int vResultado = 0;
            try{
                for (int i = 0; i < vDDL.Items.Count; i++){
                    if (vDDL.Items[i].Value.Equals(vValue))
                        vResultado = i;
                }
            }
            catch { }
            return vResultado;
        }

        protected void BtnEnviarRevision_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_ObtenerUsuarios] 4,'" + Convert.ToString(Session["USUARIO"]) + "'";
                DataTable vDatosResponsables = vConexion.obtenerDataTable(vQuery);

                Correo vCorreo = new Correo();

                foreach (DataRow item in vDatosResponsables.Rows){
                    vCorreo.Usuario = vConexion.GetNombreUsuario(item["idUsuario"].ToString());
                    vCorreo.Para = item["correo"].ToString();
                    vCorreo.Copia = "";
                }

                SmtpService vSmtpService = new SmtpService();
                Boolean vEnvio = vSmtpService.EnviarMensaje(
                    vCorreo.Para,
                    typeBody.General,
                    vCorreo.Usuario,
                    "Se ha ingresado un nuevo informe para su revisión y envio. (No." + LbRevisionInforme.Text + ") " + vConexion.GetNombreInforme(LbRevisionInforme.Text).ToUpper() + @"<br \><br \>" +
                    "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                    vCorreo.Copia
                    );

                if (vEnvio){
                    vQuery = "[ACSP_Informes] 5," + LbRevisionInforme.Text + ",1";
                    vConexion.ejecutarSql(vQuery);
                }

                if (vIdInforme != null)
                    buscarInforme(vIdInforme, true);
                else
                    getInformes();

                CerrarModal("RevisionModal");
                Mensaje("Mensaje enviado con exito", WarningType.Success);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); CerrarModal("RevisionModal"); }
        }

        protected void BtnEnviarResponsables_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_Informes] 6," + LbResponsablesInforme.Text + ",1";
                DataTable vData = vConexion.obtenerDataTable(vQuery);
                String vArchivo = vData.Rows[0]["archivo"].ToString() != "" ? vData.Rows[0]["archivo"].ToString() : "";
                String vNombreArchivo = vData.Rows[0]["nombreArchivo"].ToString() != "" ? vData.Rows[0]["nombreArchivo"].ToString() : "";


                vQuery = "[ACSP_ObtenerUsuariosInforme] 3," + LbResponsablesInforme.Text;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                String vVai = vDatos.Rows.Count > 0 ? vDatos.Rows[0]["vai"].ToString() : "";

                vDatos.Columns.Add("nombre");
                for (int i = 0; i < vDatos.Rows.Count; i++){
                    vDatos.Rows[i]["nombre"] = vConexion.GetNombreUsuario(vDatos.Rows[i]["idUsuario"].ToString());
                }

                for (int i = 0; i < vDatos.Rows.Count; i++){
                    String vMensaje = "Buen día, por instrucciones recibidas por el Lic. Ariel Pavón García, se ha emitido el Informe <b>" + vConexion.GetNombreInforme(LbResponsablesInforme.Text).ToUpper() + "</b> con código <b>" + vVai + "</b>. " + @"<br \><br \>" + "Enviado por: " + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"]));
                    EnviarCorreo(i, vMensaje, vDatos, vArchivo, vNombreArchivo);
                }

                //JUNTA DIRECTIVA
                vQuery = "[ACSP_ObtenerUsuariosInforme] 6";
                vDatos = vConexion.obtenerDataTable(vQuery);
                SmtpService vSmtpService = new SmtpService();
                for (int i = 0; i < vDatos.Rows.Count; i++){
                    vSmtpService.EnviarMensaje(
                            vDatos.Rows[i]["correo"].ToString(),
                            typeBody.General,
                            vDatos.Rows[i]["nombre"].ToString(),
                            "Buen día, por instrucciones recibidas por el Lic. Ariel Pavón García, se ha emitido el Informe <b>" + vConexion.GetNombreInforme(LbResponsablesInforme.Text).ToUpper() + "</b> con código <b>" + vVai + "</b>.",
                            "",
                            vArchivo,
                            vNombreArchivo
                            );

                    if (vDatos.Rows[i]["correo2"].ToString() != ""){
                        vSmtpService.EnviarMensaje(
                            vDatos.Rows[i]["correo2"].ToString(),
                            typeBody.General,
                            vDatos.Rows[i]["nombre"].ToString(),
                            "Buen día, por instrucciones recibidas por el Lic. Ariel Pavón García, se ha emitido el Informe <b>" + vConexion.GetNombreInforme(LbResponsablesInforme.Text).ToUpper() + "</b> con código <b>" + vVai + "</b>.",
                            "",
                            vArchivo,
                            vNombreArchivo
                            );
                    }
                }

                if (vIdInforme != null)
                    buscarInforme(vIdInforme, true);
                else
                    getInformes();

                CerrarModal("ResponsablesModal");
                getInformes();
                Mensaje("Mensaje enviado con exito", WarningType.Success);
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
                CerrarModal("ResponsablesModal"); 
            }
        }

        private Boolean EnviarCorreo(int i, String vMensaje, DataTable vDatos, String vArchivo, String vNombreArchivo) {
            Boolean vResult = false;
            try{
                SmtpService vSmtpService = new SmtpService();
                vSmtpService.EnviarMensaje(
                        vDatos.Rows[i]["correo"].ToString(),
                        typeBody.General,
                        vDatos.Rows[i]["nombre"].ToString(),
                        vMensaje,
                        "",
                        vArchivo,
                        vNombreArchivo
                        );
                vResult = true;
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
            return vResult;
        }

        protected void BtnModificarHallazgos_Click(object sender, EventArgs e){
            try{
                String vQuery = "[ACSP_Hallazgos] 7," + LbModificarHallazgoLabel.Text + ",0," +
                    "'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "'" + TxModificarHallazgosHallazgo.Text.Replace("'", "") + "'," +
                    "'" + TxModificarHallazgosRiesgo.Text.Replace("'", "") + "'," +
                    "'" + TxModificarHallazgosRecomendaciones.Text.Replace("'", "") + "'," +
                    "''," +
                    "'" + TxModificarHallazgosFechaRespuesta.Text + "'," +
                    "1," +
                    "'" + TxModificarHallazgosFuente.Text.Replace("'", "") + "'," +
                    "" + DDLModificarHallazgosArea.SelectedValue + "," +
                    "" + DDLModificarHallazgosNivelRiesgo.SelectedValue + ",'',''," +
                    "'" + DDLModificarHallazgosResponsable.SelectedValue + "'";

                if (vConexion.ejecutarSql(vQuery).Equals(1)){
                    String vArchivoNotificacion = String.Empty;
                    try{
                        if (FUModificarHallazgos.HasFile){
                            String vNombreDeposito = String.Empty;
                            HttpPostedFile bufferDeposito1T = FUModificarHallazgos.PostedFile;
                            byte[] vFileDeposito1 = null;
                            if (bufferDeposito1T != null){
                                vNombreDeposito = FUModificarHallazgos.FileName;
                                Stream vStream = bufferDeposito1T.InputStream;
                                BinaryReader vReader = new BinaryReader(vStream);
                                vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                            }

                            String vDeposito = Convert.ToBase64String(vFileDeposito1);
                            vQuery = "[ACSP_Archivos] 1,'" + vNombreDeposito + "','" + vDeposito + "',0";
                            int? vIdArchivo = vConexion.ejecutarSQLGetValue(vQuery);
                            if (vIdArchivo != null){
                                vQuery = "[ACSP_Archivos] 2,'',''," + LbModificarHallazgoLabel.Text + "," + vIdArchivo;
                                int vCreado = vConexion.ejecutarSql(vQuery);
                                if (vCreado.Equals(1))
                                    vArchivoNotificacion = "archivo almacenado";
                                else
                                    vArchivoNotificacion = "no se pudo guardar el archivo";
                            }
                        }
                    }
                    catch { }

                    Response.Redirect("/pages/ereports.aspx?ex=2&id=" + LbNumeroInformeHallazgos.Text);
                    CerrarModal("HallazgosModificacionCreacionModal");
                    Mensaje("Mensaje enviado con exito", WarningType.Success);
                }else
                    throw new Exception("Error al modificar el hallazgo");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); CerrarModal("HallazgosModificacionCreacionModal"); }
        }

        protected void BtnEntrarInf_Click(object sender, EventArgs e){
            try{
                if (LbNumeroInformeHallazgos.Text == "")
                    BtnHallazgosCreacionInforme_Click(sender, e);
                else
                    Response.Redirect("creports.aspx?i=" + LbNumeroInformeHallazgos.Text);
            }catch (Exception ex){
                Mensaje(ex.Message, WarningType.Danger);
            }
        }
    }
}