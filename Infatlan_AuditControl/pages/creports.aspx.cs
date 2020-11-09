using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class creports : System.Web.UI.Page
    {
        db vConexion = new db();
        DataTable vDatosResponsables = new DataTable();
        String vInforme = string.Empty;
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Session["AUTH"] != null){
                    vInforme = Request.QueryString["i"];
                    BtnCrearInforme.Visible = vInforme != null ? false : true;
                    BtnModificarInforme.Visible = vInforme != null ? true : false;
                    LbIdInforme.Text = Request.QueryString["i"];
                    TxFechaRespuesta.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                    Session["DATARESPONSABLES"] = null;
                    if (vInforme != null){
                        cargarDatos(vInforme);
                    }
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 4:
                        case 5:
                            Response.Redirect("/default.aspx");
                            break;
                    }
                }
                getUsuariosResponsables();
                getTiposEnvio();
            }
        }

        private void cargarDatos(String vInforme) {
            String vQuery = "[ACSP_ObtenerUsuariosInforme] 3," + vInforme;
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);
            vDatos.Columns.Add("usuarioResponsable");
            for (int i = 0; i < vDatos.Rows.Count; i++){
                vDatos.Rows[i]["usuarioResponsable"] = vDatos.Rows[i]["idUsuario"].ToString();
            }
            if (vDatos.Rows.Count > 0){
                Session["DATARESPONSABLES"] = vDatos;
                GVResponsables.DataSource = vDatos;
                GVResponsables.DataBind();
            }

            vQuery = "[ACSP_ObtenerInformes] 2," + vInforme;
            vDatos = vConexion.obtenerDataTable(vQuery);

            TxNombreInforme.Text = vDatos.Rows[0]["nombre"].ToString();
            DateTime vFecha = Convert.ToDateTime(vDatos.Rows[0]["fechaRespuesta"].ToString());
            TxFechaRespuesta.Text = vFecha.ToString("yyyy-MM-dd");
            TxDescripcionInforme.Text = vDatos.Rows[0]["descripcion"].ToString();
            CBEstadoCerrado.Checked = vDatos.Rows[0]["tipoEstado"].ToString() == "2" ? true : false;
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void getUsuariosResponsables(){
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

                DDLUserResponsable.Items.Add(new ListItem { Value="0", Text="Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows){
                    DDLUserResponsable.Items.Add(new ListItem { Value = item["usuario"].ToString(), Text = item["nombre"].ToString() + " " + item["apellido"].ToString() +  " - " + item["empresa"].ToString() });
                }
                DDLUserResponsable.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        public void getTiposEnvio(){
            try{
                String vQuery = "[ACSP_TiposEnvio]";
                DataTable vDatosFinal = vConexion.obtenerDataTable(vQuery);

                DDLTipoResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows){
                    DDLTipoResponsable.Items.Add(new ListItem { Value = item["tipoEnvio"].ToString(), Text = item["tipoEnvio"].ToString() + "-" + item["nombre"].ToString() });
                }
                DDLTipoResponsable.DataBind();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnCrearInforme_Click(object sender, EventArgs e){
            try{
                if (DDLUserResponsable.SelectedIndex.Equals(0))
                    throw new Exception("Por favor seleccione un responsable.");
                if (TxNombreInforme.Text.Equals(""))
                    throw new Exception("Por favor escriba el nombre del informe");
                if (Convert.ToDateTime(TxFechaRespuesta.Text) <= DateTime.Now)
                    throw new Exception("Por favor seleccione una fecha mayor a hoy");
                if (Session["DATARESPONSABLES"] is null)
                    throw new Exception("Por favor ingrese un reponsable para el informe");
                else
                    vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];

                String vEstado = CBEstadoCerrado.Checked ? "2" : "1";
                String vQuery = "[ACSP_Informes] 1,0," + vEstado + ",'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "'" + TxNombreInforme.Text.Replace("'", "") + "'," +
                    "'" + TxDescripcionInforme.Text.Replace("'","") + "'," +
                    "'',0," +
                    "'" + TxFechaRespuesta.Text + "'";

                int? vIdInforme = vConexion.ejecutarSQLGetValue(vQuery);
                if (vIdInforme != null){
                    if (FUInforme.HasFile){
                        String vNombreDeposito = String.Empty;
                        HttpPostedFile bufferDeposito1T = FUInforme.PostedFile;
                        byte[] vFileDeposito1 = null;
                        if (bufferDeposito1T != null){
                            vNombreDeposito = FUInforme.FileName;
                            Stream vStream = bufferDeposito1T.InputStream;
                            BinaryReader vReader = new BinaryReader(vStream);
                            vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                        }
                        String vDeposito = Convert.ToBase64String(vFileDeposito1);
                        vQuery = "[ACSP_Informes] 4," + vIdInforme + ",0,'','','','','','','" + vDeposito + "'";
                        vConexion.ejecutarSql(vQuery);
                    }

                    foreach (DataRow item in vDatosResponsables.Rows){
                        vQuery = "[ACSP_Informes] 3," + vIdInforme + ",0,'','','','" + item["usuarioResponsable"].ToString() + "'," + item["envio"].ToString().Split('-')[0].ToString();
                        vConexion.ejecutarSql(vQuery);
                    }

                    LimpiarInformes();
                    Response.Redirect("/pages/ereports.aspx?id=" + vIdInforme);
                }else
                    throw new Exception("Error al ingresar el informe por favor consulte con sistemas");
            }catch (Exception Ex) { 
                LbMensajeCrearInforme.Text = Ex.Message; 
            }
        }

        void LimpiarInformes(){
            TxNombreInforme.Text = String.Empty;
            TxDescripcionInforme.Text = String.Empty;
            DDLUserResponsable.SelectedIndex = -1;
            DDLTipoResponsable.SelectedIndex = -1;
            GVResponsables.DataSource = null;
            GVResponsables.DataBind();
            Session["DATARESPONSABLES"] = null;
            TxFechaRespuesta.Text = String.Empty;
        }

        protected void BtnAgregarResponsable_Click(object sender, EventArgs e){
            try{
                if (DDLUserResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione un usuario!");
                if (DDLTipoResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor selccione un tipo de envio");

                if (Session["DATARESPONSABLES"] == null){
                    vDatosResponsables = new DataTable();
                    vDatosResponsables.Columns.Add("usuarioResponsable");
                    vDatosResponsables.Columns.Add("envio");
                }else{
                    vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];
                }

                Boolean vFlagInsert = false;
                for (int i = 0; i < vDatosResponsables.Rows.Count; i++){
                    if (vDatosResponsables.Rows[i]["usuarioResponsable"].ToString().Equals(DDLUserResponsable.SelectedValue))
                        vFlagInsert = true;
                }

                if (!vFlagInsert)
                    if (LbIdInforme.Text != "")
                        vDatosResponsables.Rows.Add(DDLUserResponsable.SelectedValue, "",DDLTipoResponsable.SelectedValue, DDLTipoResponsable.SelectedItem.Text, DDLUserResponsable.SelectedValue);
                    else
                        vDatosResponsables.Rows.Add(DDLUserResponsable.SelectedValue, DDLTipoResponsable.SelectedItem.Text);
                else
                    throw new Exception("Este usuario ya ha sido agregado");

                GVResponsables.DataSource = vDatosResponsables;
                GVResponsables.DataBind();
                Session["DATARESPONSABLES"] = vDatosResponsables;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVResponsables_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                if (e.CommandName == "DeleteRow"){
                    string vIdResponsable = e.CommandArgument.ToString();
                    if (Session["DATARESPONSABLES"] != null){
                        vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];

                        DataRow[] result = vDatosResponsables.Select("usuarioResponsable = '" + vIdResponsable + "'");
                        foreach (DataRow row in result){
                            if (row["usuarioResponsable"].ToString().Contains(vIdResponsable))
                                vDatosResponsables.Rows.Remove(row);
                        }
                    }
                }

                GVResponsables.DataSource = vDatosResponsables;
                GVResponsables.DataBind();
                Session["DATARESPONSABLES"] = vDatosResponsables;
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnModificarInforme_Click(object sender, EventArgs e){
            try{
                if (DDLUserResponsable.SelectedIndex.Equals(0))
                    throw new Exception("Por favor seleccione un responsable.");
                if (TxNombreInforme.Text.Equals(""))
                    throw new Exception("Por favor escriba el nombre del informe");
                if (Convert.ToDateTime(TxFechaRespuesta.Text) <= DateTime.Now)
                    throw new Exception("Por favor seleccione una fecha mayor a hoy");
                if (Session["DATARESPONSABLES"] is null)
                    throw new Exception("Por favor ingrese un reponsable para el informe");
                else
                    vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];

                String vEstado = CBEstadoCerrado.Checked ? "2" : "1";
                String vQuery = "[ACSP_Informes] 8" +
                    "," + LbIdInforme.Text + 
                    "," + vEstado + 
                    ",0" +
                    ",'" + TxNombreInforme.Text.Replace("'", "") + "'" +
                    ",'" + TxDescripcionInforme.Text.Replace("'","") + "'" +
                    ",'',0" +
                    ",'" + TxFechaRespuesta.Text + "'";

                int vIdInforme = vConexion.ejecutarSql(vQuery);
                if (vIdInforme > 0){
                    vQuery = "[ACSP_Informes] 9," + LbIdInforme.Text;
                    vConexion.ejecutarSql(vQuery);
                    foreach (DataRow item in vDatosResponsables.Rows){
                        vQuery = "[ACSP_Informes] 3," + LbIdInforme.Text + ",0,'','','','" + item["usuarioResponsable"].ToString() + "'," + item["envio"].ToString().Split('-')[0].ToString();
                        vConexion.ejecutarSql(vQuery);
                    }

                    LimpiarInformes();
                    Response.Redirect("/pages/ereports.aspx?id=" + vIdInforme);
                }else
                    throw new Exception("Error al modificar el informe por favor consulte con sistemas");
            }catch (Exception Ex) { 
                LbMensajeCrearInforme.Text = Ex.Message; 
            }
        }
    }
}