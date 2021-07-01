using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages.configurations
{
    public partial class users : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e){
            if (!Page.IsPostBack){
                if (Session["AUTH"] != null){
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"])){
                        case 3:
                        case 4:
                        case 5:
                            Response.Redirect("/default.aspx");
                            break;
                        case 6:
                            Response.Redirect("/default.aspx");
                            break;
                    }
                }
                ObtenerUsuarios();
                GetUsuariosJefaturas();
                getEmpresas();
            }
        }

        public void Mensaje(string vMensaje, WarningType type){
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        public void CerrarModal(String vModal){
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        
        void ObtenerUsuarios(){
            try{
                String vQuery = "[ACSP_ObtenerUsuarios] 3";
                DataTable vDatosUsuarios = vConexion.obtenerDataTable(vQuery);

                GVBusqueda.DataSource = vDatosUsuarios;
                GVBusqueda.DataBind();
                Session["DATOSUSUARIOS"] = vDatosUsuarios;

                vQuery = "[ACSP_Usuarios] 3,0";
                vDatosUsuarios = vConexion.obtenerDataTable(vQuery);

                DDLCargoModificar.Items.Add(new ListItem { Value = "0", Text = "Seleccione un cargo" });
                foreach (DataRow item in vDatosUsuarios.Rows){
                    DDLCargoModificar.Items.Add(new ListItem { Value = item["tipoUsuario"].ToString(), Text = item["descripcion"].ToString() });
                }
                DDLCargoModificar.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void GetUsuariosJefaturas(){
            try{ 
                String vQuery = "[ACSP_ObtenerUsuarios] 6,2";
                DataTable vDatosDB = vConexion.obtenerDataTable(vQuery);

                DDLUsuarioJefatura.Items.Clear();
                DDLJefes.Items.Clear();
                DDLUsuarioJefatura.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                DDLJefes.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatosDB.Rows){
                    DDLUsuarioJefatura.Items.Add(new ListItem { Value = item["idUsuario"].ToString(), Text = vConexion.GetNombreUsuario(item["idUsuario"].ToString()) });
                    DDLJefes.Items.Add(new ListItem { Value = item["idUsuario"].ToString(), Text = vConexion.GetNombreUsuario(item["idUsuario"].ToString()) });
                }
                DDLUsuarioJefatura.DataBind();
                DDLJefes.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void getEmpresas(){
            try{
                String vQuery = "[ACSP_ObtenerUsuarios] 9";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DDLEmpresa.Items.Clear();
                DDLEmpresa.Items.Add(new ListItem { Value = "0", Text = "Seleccione una empresa" });
                foreach (DataRow item in vDatos.Rows){
                    DDLEmpresa.Items.Add(new ListItem { Value = item["idEmpresa"].ToString(), Text = item["nombre"].ToString() });
                }
                DDLEmpresa.DataBind();
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnBuscarUsuario_Click(object sender, EventArgs e){
            try{
                if (TxUsuario.Text == String.Empty)
                    throw new Exception("Ingrese un usuario para proceder con la busqueda.");

                LdapService vLdap = new LdapService();
                DataTable vDatos = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], TxUsuario.Text);


                MostrarOcultar(true);
                if (vDatos.Rows.Count > 0){
                    TxCorreo.Text = vDatos.Rows[0]["mail"].ToString();
                    TxNombres.Text = vDatos.Rows[0]["givenName"].ToString();
                    TxApellidos.Text = vDatos.Rows[0]["sn"].ToString();
                    TxPuesto.ReadOnly = false;
                    TxCorreo2.ReadOnly = false;
                }else if(DDLCargo.SelectedValue == "6") {
                    TxCorreo.Text = String.Empty;
                    TxNombres.Text = string.Empty;
                    TxApellidos.Text = String.Empty;
                    TxPuesto.Text = String.Empty;
                    TxCorreo2.Text = String.Empty;

                    MostrarOcultar(false);
                }else
                    throw new Exception("El usuario no existe en el dominio ADBANCAT");

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCrearUsuario_Click(object sender, EventArgs e){
            try{
                if (DDLCargo.SelectedIndex.Equals(0))
                    throw new Exception("Por favor seleccione un cargo para crear el usuario");
                if (DDLCargo.SelectedValue == "4"){
                    if (DDLEmpresa.SelectedValue == "0"){
                        throw new Exception("Por favor seleccione la empresa.");
                    }
                }
                if (TxUsuario.Text.Equals(""))
                    throw new Exception("Por favor escriba un usuario y presione el boton buscar");
                if (TxPuesto.Text.Equals(""))
                    throw new Exception("Por favor ingrese el puesto del usuario.");


                String vJefeAuditoria = String.Empty;
                if (!DDLUsuarioJefatura.SelectedValue.Equals("0"))
                    vJefeAuditoria = DDLUsuarioJefatura.SelectedValue;

                String vEmpresa = DDLCargo.SelectedValue == "4" ? DDLEmpresa.SelectedValue : "0" ;
                String vQuery = "[ACSP_Usuarios] 1" + 
                    ",'" + TxUsuario.Text + "'" + 
                    "," + DDLCargo.SelectedValue + 
                    ",'" + TxCorreo.Text + "'" + 
                    ",0,'" + vJefeAuditoria + "'" + 
                    "," + vEmpresa + "" +
                    ",'" + TxPuesto.Text + "'" +
                    ",'" + TxNombres.Text + "'" +
                    ",'" + TxApellidos.Text + "'" +
                    ",'" + TxCorreo2.Text + "'";

                try{
                    if (vConexion.ejecutarSql(vQuery).Equals(1)){
                        GetUsuariosJefaturas();
                        ObtenerUsuarios();
                        LimpiarForma();
                        Mensaje("Usuario creado con Exito!", WarningType.Success);
                    }
                }
                catch { throw new Exception("El usuario ya esta creado en el sistema"); }
               
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarForma(){
            TxCorreo.ReadOnly = true;
            TxNombres.ReadOnly = true;
            TxApellidos.ReadOnly = true;
            TxPuesto.Text = String.Empty;
            TxCorreo2.Text = String.Empty;

            TxApellidos.Text = String.Empty;
            TxCorreo.Text = String.Empty;
            TxNombres.Text = String.Empty;
            TxUsuario.Text = String.Empty;
            DDLCargo.SelectedIndex = -1;
        }

        void MostrarOcultar(Boolean vValue) {
            TxNombres.ReadOnly = vValue;
            TxApellidos.ReadOnly = vValue;
            TxCorreo.ReadOnly = vValue;
            TxPuesto.ReadOnly = vValue;
            TxCorreo2.ReadOnly = vValue;
        }

        void LimpiarModal() {
            LbErrorUsuario.Text = string.Empty;
            TxModPuesto.Text = string.Empty;
            DivJefe.Visible = false;
            DDLEstado.SelectedIndex = -1;
            DDLCargoModificar.SelectedIndex = -1;
            DDLJefes.SelectedIndex = -1;
            UpdateUsuarioMensaje.Update();
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATOSUSUARIOS"];
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e){
            try{
                LimpiarModal();
                string vIdUsuario = e.CommandArgument.ToString();
                if (e.CommandName == "ModificarUsuario"){
                    String vQuery = "[ACSP_ObtenerUsuarios] 5,'" + vIdUsuario + "'";
                    DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                    LbUsuarioModificar.Text = vIdUsuario;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "UsuariosModal();", true);

                    DDLCargoModificar.SelectedValue = vDatos.Rows[0]["tipoUsuario"].ToString();
                    DDLEstado.SelectedValue = Convert.ToBoolean(vDatos.Rows[0]["Estado"]).ToString();
                    TxModPuesto.Text = vDatos.Rows[0]["puesto"].ToString();

                    if (vDatos.Rows[0]["tipoUsuario"].ToString() == "3") {
                        DivJefe.Visible =  true;
                        DDLJefes.SelectedValue = vDatos.Rows[0]["jefeAuditoria"].ToString() != "" ? vDatos.Rows[0]["jefeAuditoria"].ToString() : "0";
                    }else { 
                        DivJefe.Visible = false;
                        DDLJefes.SelectedValue = "0";
                    }
                    UpdateUsuariosMain.Update();
                }
                
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void BtnModificarUsuario_Click(object sender, EventArgs e){
            try{
                if (DDLCargoModificar.SelectedIndex.Equals(0))
                    throw new Exception("Selecciona un cargo para modificar");

                if (DDLEstado.SelectedIndex.Equals(0))
                    throw new Exception("Selecciona un estado para modificar");

                String vJefe = DDLCargoModificar.SelectedValue == "3" ? DDLJefes.SelectedValue : "";
                String vQuery = "[ACSP_Usuarios] 2" +
                    ",'" + LbUsuarioModificar.Text + "'" +
                    "," + DDLCargoModificar.SelectedValue + "" +
                    ",''" +
                    "," + DDLEstado.SelectedValue + "" +
                    ",'" + vJefe + "'" +
                    ",0" +
                    ",'" + TxModPuesto.Text + "'";

                if (vConexion.ejecutarSql(vQuery).Equals(1)){
                    DDLCargoModificar.SelectedIndex = -1;
                    DDLEstado.SelectedIndex = -1;
                    UpdateUsuariosMain.Update();

                    ObtenerUsuarios();
                    CerrarModal("UsuariosModal");
                    Mensaje("Usuario modificado con Exito!", WarningType.Success);
                }
            }catch (Exception Ex) { 
                LbErrorUsuario.Text = Ex.Message; 
                UpdateUsuarioMensaje.Update(); 
            }
        }

        protected void DDLCargo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                MostrarOcultar(true);
                TxPuesto.Text = String.Empty;
                TxCorreo2.Text = String.Empty;
                TxApellidos.Text = String.Empty;
                TxCorreo.Text = String.Empty;
                TxNombres.Text = String.Empty;
                TxUsuario.Text = String.Empty;

                DIVUsuarioJefatura.Visible = DDLCargo.SelectedValue == "3" ? true : false;
                DivEmpresas.Visible = DDLCargo.SelectedValue == "4" ? true : false;
                TxUsuario.ReadOnly = DDLCargo.SelectedValue != "0" ? false : true;
                DivCorreo2.Visible = DDLCargo.SelectedValue == "6" ? true : false;
                
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }

        protected void DDLCargoModificar_SelectedIndexChanged(object sender, EventArgs e){
            try{
                DivJefe.Visible = DDLCargoModificar.SelectedValue == "3" ? true : false;
            }catch (Exception Ex) { 
                Mensaje(Ex.Message, WarningType.Danger); 
            }
        }
    }
}