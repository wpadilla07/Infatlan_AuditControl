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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["AUTH"] != null)
                {
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"]))
                    {
                        case 3:
                        case 4:
                        case 5:
                            Response.Redirect("/default.aspx");
                            break;
                    }
                }


                ObtenerUsuarios();
                GetUsuariosJefaturas();
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
        void ObtenerUsuarios()
        {
            try
            {
                String vQuery = "[ACSP_ObtenerUsuarios] 3";
                DataTable vDatosUsuarios = vConexion.obtenerDataTable(vQuery);

                GVBusqueda.DataSource = vDatosUsuarios;
                GVBusqueda.DataBind();
                Session["DATOSUSUARIOS"] = vDatosUsuarios;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void GetUsuariosJefaturas()
        {
            try
            { 
                String vQuery = "[ACSP_ObtenerUsuarios] 6,2";
                DataTable vDatosDB = vConexion.obtenerDataTable(vQuery);

                DDLUsuarioJefatura.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatosDB.Rows)
                {
                    DDLUsuarioJefatura.Items.Add(new ListItem { Value = item["idUsuario"].ToString(), Text = vConexion.GetNombreUsuario(item["idUsuario"].ToString()) });
                }

                DDLUsuarioJefatura.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBuscarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxUsuario.Text == String.Empty)
                    throw new Exception("Ingrese un usuario para proceder con la busqueda.");

                LdapService vLdap = new LdapService();
                DataTable vDatos = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], TxUsuario.Text);

                if (vDatos.Rows.Count > 0)
                {
                    TxCorreo.Text = vDatos.Rows[0]["mail"].ToString();
                    TxNombres.Text = vDatos.Rows[0]["givenName"].ToString();
                    TxApellidos.Text = vDatos.Rows[0]["sn"].ToString();
                }
                else
                {
                    TxCorreo.Text = String.Empty;
                    TxNombres.Text = string.Empty;
                    TxApellidos.Text = String.Empty;
                    throw new Exception("No existe el usuario buscado");
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLCargo.SelectedIndex.Equals(0))
                    throw new Exception("Por favor seleccione un cargo para crear el usuario");

                if (TxUsuario.Text.Equals(""))
                    throw new Exception("Por favor escriba un usuario y presione el boton buscar");

                String vJefeAuditoria = String.Empty;
                if (!DDLUsuarioJefatura.SelectedValue.Equals("0"))
                    vJefeAuditoria = DDLUsuarioJefatura.SelectedValue;


                String vQuery = "[ACSP_Usuarios] 1, '" + TxUsuario.Text + "'," + DDLCargo.SelectedValue + ",'" + TxCorreo.Text + "',0,'" + vJefeAuditoria + "'";
                try
                {
                    if (vConexion.ejecutarSql(vQuery).Equals(1))
                    {
                        ObtenerUsuarios();
                        LimpiarForma();
                        Mensaje("Usuario creado con Exito!", WarningType.Success);
                    }
                }
                catch { throw new Exception("El usuario ya esta creado en el sistema"); }
               
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarForma()
        {
            TxApellidos.Text = String.Empty;
            TxCorreo.Text = String.Empty;
            TxNombres.Text = String.Empty;
            TxUsuario.Text = String.Empty;
            DDLCargo.SelectedIndex = -1;

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

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string vIdUsuario = e.CommandArgument.ToString();
                if (e.CommandName == "ModificarUsuario")
                {
                    LbUsuarioModificar.Text = vIdUsuario;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "UsuariosModal();", true);
                }
                
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModificarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLCargoModificar.SelectedIndex.Equals(0))
                    throw new Exception("Selecciona un cargo para modificar");

                if (DDLEstado.SelectedIndex.Equals(0))
                    throw new Exception("Selecciona un estado para modificar");

                String vQuery = "[ACSP_Usuarios] 2, '" + LbUsuarioModificar.Text + "'," + DDLCargoModificar.SelectedValue + ",''," + DDLEstado.SelectedValue;
                if (vConexion.ejecutarSql(vQuery).Equals(1))
                {

                    DDLCargoModificar.SelectedIndex = -1;
                    DDLEstado.SelectedIndex = -1;
                    UpdateUsuariosMain.Update();

                    ObtenerUsuarios();
                    CerrarModal("UsuariosModal");
                    Mensaje("Usuario modificado con Exito!", WarningType.Success);
                }
            }
            catch (Exception Ex) { LbErrorUsuario.Text = Ex.Message; UpdateUsuarioMensaje.Update(); }
        }

        protected void DDLCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLCargo.SelectedValue.Equals("3"))            
                    DIVUsuarioJefatura.Visible = true;
                else
                    DIVUsuarioJefatura.Visible = false;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}