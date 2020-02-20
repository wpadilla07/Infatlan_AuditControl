using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl.pages
{
    public partial class findingsSearch : System.Web.UI.Page
    {
        db vConexion = new db();
        String vInformeQuery = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            vInformeQuery = Request.QueryString["i"];
            if (!Page.IsPostBack)
            {
                getUsuariosResponsables();
                getEstados();
                if (vInformeQuery != null)
                {
                    BuscarHallazgo(vInformeQuery);
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
        void getEstados()
        {
            try
            {
                String vQuery = "[ACSP_ObtenerTipos] 4";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                DDLModificarHallazgoEstado.Items.Add(new ListItem { Value = "0", Text = "Seleccione un estado" });
                foreach (DataRow item in vDatos.Rows)
                {
                    DDLModificarHallazgoEstado.Items.Add(new ListItem { Value = item["tipoEstadoHallazgo"].ToString(), Text = item["tipoEstadoHallazgo"].ToString() + "-" + item["nombre"].ToString() });
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        public void getUsuariosResponsables()
        {
            try
            {
                LdapService vLdap = new LdapService();
                String vQuery = "[ACSP_ObtenerUsuarios] 1";
                DataTable vDatosDB = vConexion.obtenerDataTable(vQuery);

                DataTable vDatosFinal = new DataTable();
                vDatosFinal.Columns.Add("usuario");
                vDatosFinal.Columns.Add("nombre");
                vDatosFinal.Columns.Add("apellido");
                vDatosFinal.Columns.Add("correo");
                foreach (DataRow item in vDatosDB.Rows)
                {
                    try
                    {
                        DataTable vDatosAD = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], item["idUsuario"].ToString());
                        vDatosFinal.Rows.Add(
                            item["idUsuario"].ToString(),
                            vDatosAD.Rows[0]["givenName"].ToString(),
                            vDatosAD.Rows[0]["sn"].ToString(),
                            vDatosAD.Rows[0]["mail"].ToString());
                    }
                    catch { }
                }

                DDLUsuariosAsignacionHallazgo.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows)
                {
                    DDLUsuariosAsignacionHallazgo.Items.Add(new ListItem { Value = item["usuario"].ToString(), Text = item["nombre"].ToString() + " " + item["apellido"].ToString() + " - " + item["correo"].ToString() });
                }

                DDLUsuariosAsignacionHallazgo.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string vIdHallazgo = e.CommandArgument.ToString();
                if (e.CommandName == "EntrarHallazgo")
                {
                    Response.Redirect("/pages/findings.aspx?id=" + vIdHallazgo + "&i=" + TxBuscarIdInforme.Text);
                }
                if (e.CommandName == "AsignarUsuario")
                {
                    LbNumeroHallazgoModificaciones.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHallazgos();", true);
                }
                if (e.CommandName == "AutorizarEstadoHallazgo")
                {
                    LbAutorizacionHallazgo.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAutorizacionEstadoModal();", true);
                }
                if (e.CommandName == "ModificarEstadoHallazgo")
                {
                    LbNumeroHallazgoModificacionesEstado.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModificacionesEstadoModal();", true);
                }
                if (e.CommandName == "FinalizarHallazgo")
                {
                    LbFinalizarHallazgo.Text = vIdHallazgo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openFinalizarHallazgoModal();", true);
                }
                


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusqueda.PageIndex = e.NewPageIndex;
                GVBusqueda.DataSource = (DataTable)Session["DATOSHALLAZGOS"];
                GVBusqueda.DataBind();

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBuscarHallazgo_Click(object sender, EventArgs e)
        {
            BuscarHallazgo();
        }
        private void BuscarHallazgo(String vInformeQuery)
        {
            try
            {
                String vQuery = String.Empty;
                String vBusqueda = String.Empty;
                vBusqueda = vInformeQuery;

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                if (isNumeric)
                {
                    vQuery = "[ACSP_ObtenerHallazgos] 2," + vInformeQuery + ",'" + Convert.ToString(Session["USUARIO"]) + "'";
                }
                else
                {
                    throw new Exception("No puedes buscar por ese ID");
                }
                TxBuscarIdInforme.Text = vInformeQuery;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                DataTable vDatosConvertidos = new DataTable();
                vDatosConvertidos.Columns.Add("idHallazgo");
                vDatosConvertidos.Columns.Add("fechaCreacion");
                vDatosConvertidos.Columns.Add("idArea");
                vDatosConvertidos.Columns.Add("tipoRiesgo");
                vDatosConvertidos.Columns.Add("detalle");
                vDatosConvertidos.Columns.Add("tipoEstadoHallazgo");

                foreach (DataRow item in vDatos.Rows)
                {
                    String vArea = String.Empty;
                    String vRiesgo = String.Empty;
                    vQuery = "[ACSP_ObtenerTipos] 1";
                    DataTable vDatosArea = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow itemArea in vDatosArea.Rows)
                    {
                        if (itemArea["idArea"].ToString().Equals(item["idArea"].ToString()))
                            vArea = itemArea["nombre"].ToString();
                    }

                    vQuery = "[ACSP_ObtenerTipos] 2";
                    DataTable vDatosRiesgo = vConexion.obtenerDataTable(vQuery);

                    foreach (DataRow itemRiesgo in vDatosRiesgo.Rows)
                    {
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
                switch (Convert.ToInt32(Session["TIPOUSUARIO"]))
                {
                    case 1:
                    case 2:
                        GVBusqueda.Columns[10].Visible = false;
                        break;
                    case 3:
                        GVBusqueda.Columns[0].Visible = false;
                        GVBusqueda.Columns[2].Visible = false;
                        GVBusqueda.Columns[10].Visible = false;
                        break;
                    case 4:
                        GVBusqueda.Columns[0].Visible = false;
                        GVBusqueda.Columns[1].Visible = false;
                        break;
                    case 5:
                        GVBusqueda.Columns[0].Visible = false;
                        GVBusqueda.Columns[1].Visible = false;
                        GVBusqueda.Columns[2].Visible = false;
                        break;
                }
                GVBusqueda.DataBind();
                foreach (GridViewRow row in GVBusqueda.Rows)
                {
                    vQuery = "[ACSP_ObtenerHallazgos] 7, " + row.Cells[3].Text;
                    Boolean vEstadoCerrado = vConexion.ejecutarSQLGetValueBoolean(vQuery);

                    if (vEstadoCerrado)
                    {
                        LinkButton vBtnCerrarHallazgo = row.Cells[2].FindControl("BtnFinalizarHallazgo") as LinkButton;
                        vBtnCerrarHallazgo.Enabled = false;
                        vBtnCerrarHallazgo.CssClass = "btn btn-grey";

                        LinkButton vBtnAsignar = row.Cells[2].FindControl("BtnAsignar") as LinkButton;
                        vBtnAsignar.Enabled = false;
                        vBtnAsignar.CssClass = "btn btn-grey";
                    }

                }
                Session["DATOSHALLAZGOS"] = vDatosConvertidos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); GVBusqueda.DataSource = null; GVBusqueda.DataBind(); }
        }
        private void BuscarHallazgo()
        {
            try
            {
                String vQuery = String.Empty;
                String vBusqueda = String.Empty;
                if (TxBuscarIdInforme.Text.Equals(""))
                    vBusqueda = TxBuscarNombre.Text;
                else
                    vBusqueda = TxBuscarIdInforme.Text;

                Boolean isNumeric = int.TryParse(vBusqueda, out int n);
                if (isNumeric)
                {
                    vQuery = "[ACSP_ObtenerHallazgos] 2," + TxBuscarIdInforme.Text + ",'" + Convert.ToString(Session["USUARIO"]) + "'";
                }
                else
                {
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
                if (vDatos.Rows.Count > 0)
                {
                    foreach (DataRow item in vDatos.Rows)
                    {
                        String vArea = String.Empty;
                        String vRiesgo = String.Empty;
                        vQuery = "[ACSP_ObtenerTipos] 1";
                        DataTable vDatosArea = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow itemArea in vDatosArea.Rows)
                        {
                            if (itemArea["idArea"].ToString().Equals(item["idArea"].ToString()))
                                vArea = itemArea["nombre"].ToString();
                        }

                        vQuery = "[ACSP_ObtenerTipos] 2";
                        DataTable vDatosRiesgo = vConexion.obtenerDataTable(vQuery);

                        foreach (DataRow itemRiesgo in vDatosRiesgo.Rows)
                        {
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
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"]))
                    {
                        case 1:
                        case 2:
                            GVBusqueda.Columns[10].Visible = false;
                            break;
                        case 3:
                            GVBusqueda.Columns[0].Visible = false;
                            GVBusqueda.Columns[2].Visible = false;
                            GVBusqueda.Columns[10].Visible = false;
                            break;
                        case 4:
                            GVBusqueda.Columns[0].Visible = false;
                            GVBusqueda.Columns[1].Visible = false;
                            break;
                        case 5:
                            GVBusqueda.Columns[0].Visible = false;
                            GVBusqueda.Columns[1].Visible = false;
                            GVBusqueda.Columns[2].Visible = false;
                            break;
                    }
                    GVBusqueda.DataBind();

                    foreach (GridViewRow row in GVBusqueda.Rows)
                    {
                        vQuery = "[ACSP_ObtenerHallazgos] 7, " + row.Cells[3].Text;
                        Boolean vEstadoCerrado = vConexion.ejecutarSQLGetValueBoolean(vQuery);

                        if (vEstadoCerrado)
                        {
                            LinkButton vBtnCerrarHallazgo = row.Cells[2].FindControl("BtnFinalizarHallazgo") as LinkButton;
                            vBtnCerrarHallazgo.Enabled = false;
                            vBtnCerrarHallazgo.CssClass = "btn btn-grey";

                            LinkButton vBtnAsignar = row.Cells[2].FindControl("BtnAsignar") as LinkButton;
                            vBtnAsignar.Enabled = false;
                            vBtnAsignar.CssClass = "btn btn-grey";
                        }
                        
                    }

                    Session["DATOSHALLAZGOS"] = vDatosConvertidos;
                }
                else
                    throw new Exception("No existen hallazgos asignados");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); GVBusqueda.DataSource = null; GVBusqueda.DataBind(); }
        }

        protected void BtnModificarHallazgo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LbNumeroHallazgoModificaciones.Text.Equals(""))
                {
                    String vQuery = String.Empty;

                    vQuery = "[ACSP_ObtenerHallazgos] 5," + LbNumeroHallazgoModificaciones.Text + ",'" + DDLUsuariosAsignacionHallazgo.SelectedValue + "'";

                    int? vIdInforme = vConexion.ejecutarSQLGetValue(vQuery);
                    if (vIdInforme != null)
                    {
                        try
                        {
                            vQuery = "[ACSP_ObtenerUsuariosInforme] 2,'" + DDLUsuariosAsignacionHallazgo.SelectedValue + "'";
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
                                typeBody.HallazgoAsignacion,
                                vCorreo.Usuario,
                                "Se te ha asignado un hallazgo en el informe No." + vIdInforme,
                                vCorreo.Copia
                                );
                        }
                        catch { }



                        Mensaje("Hallazgo modificado y asignado con exito", WarningType.Success);
                        DDLUsuariosAsignacionHallazgo.SelectedIndex = -1;
                        CerrarModal("ModificacionesModal");
                    }
                }
                else
                {
                    BuscarHallazgo();
                }
                    

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModificarEstadoHallazgo_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "[ACSP_Hallazgos] 4," +
                    "" + LbNumeroHallazgoModificacionesEstado.Text + "," +
                    "" + DDLModificarHallazgoEstado.SelectedValue;

                if (vConexion.ejecutarSql(vQuery).Equals(1))
                {
                    try
                    {
                        vQuery = "[ACSP_ObtenerUsuarios] 4," + Convert.ToString(Session["USUARIO"]);
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
                            "Se ha hecho un cambio en el estado del hallazgo no." + LbNumeroHallazgoModificacionesEstado.Text + @", por favor reviselo para la debida autorización <br \><br \>" +
                            "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                            vCorreo.Copia
                            );
                    }
                    catch { }
                    
                    Mensaje("Hallazgo modificado con exito", WarningType.Success);
                    DDLModificarHallazgoEstado.SelectedIndex = -1;
                    CerrarModal("ModificacionesEstadoModal");
                }
                else
                    throw new Exception("Error al ingresar el hallazgo, contacte a sistemas.");

                DDLModificarHallazgoEstado.SelectedIndex = -1;
                BuscarHallazgo();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEnviarAutorizacion_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "[ACSP_Hallazgos] 5," +
                    "" + LbAutorizacionHallazgo.Text;

                if (vConexion.ejecutarSql(vQuery).Equals(1))
                {
                    try
                    {

                        vQuery = "[ACSP_ObtenerHallazgos] 1,'" + LbAutorizacionHallazgo.Text + "'";
                        DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                        String vUsuarioResponsable = String.Empty;
                        foreach (DataRow item in vDatosHallazgo.Rows)
                        {
                            vUsuarioResponsable = item["usuarioResponsable"].ToString();
                        }

                        vQuery = "[ACSP_ObtenerUsuarios] 5," + vUsuarioResponsable;
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
                            "Se ha realizado un cambio en el estado del hallazgo no." + LbAutorizacionHallazgo.Text + @", por favor revisar <br \><br \>" +
                            "Ingresado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                            vCorreo.Copia
                            );
                    }
                    catch { }

                    Mensaje("Hallazgo autorizado con exito", WarningType.Success);
                    DDLModificarHallazgoEstado.SelectedIndex = -1;
                    CerrarModal("AutorizacionModal");
                }
                else
                    throw new Exception("Error al ingresar el hallazgo, contacte a sistemas.");

                DDLModificarHallazgoEstado.SelectedIndex = -1;
                BuscarHallazgo();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnFinalizarHallazgo_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxFinalizarHallazgoComentario.Text.Equals(""))
                    throw new Exception("Por favor ingrese un comentario.");


                vConexion = new db();
                String vQuery = "[ACSP_Hallazgos] 6," + LbFinalizarHallazgo.Text + ",0,'','" + TxFinalizarHallazgoComentario.Text + "'";
                if (vConexion.ejecutarSql(vQuery).Equals(1))
                {

                    vQuery = "[ACSP_ObtenerHallazgos] 6,'" + LbFinalizarHallazgo.Text + "'";
                    DataTable vDatosHallazgo = vConexion.obtenerDataTable(vQuery);

                    String vUsuarioCreacion = String.Empty;
                    foreach (DataRow item in vDatosHallazgo.Rows)
                    {
                        vUsuarioCreacion = item["usuarioCreacion"].ToString();
                    }

                    vQuery = "[ACSP_ObtenerUsuarios] 5," + vUsuarioCreacion;
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
                        "Se ha actualizado el hallazgo No." + LbFinalizarHallazgo.Text + @" para cierre, por favor revisar <br \><br \>" +
                        "Creado por:" + vConexion.GetNombreUsuario(Convert.ToString(Session["USUARIO"])),
                        vCorreo.Copia
                        );

                    if(vInformeQuery != null)
                        BuscarHallazgo(vInformeQuery);
                    else
                        BuscarHallazgo(TxBuscarIdInforme.Text);

                    Mensaje("Hallazgo actualizado con exito", WarningType.Success);
                    CerrarModal("FinalizarHallazgoModal");
                }

            }
            catch (Exception Ex) { LbMensajeCierreHallazgo.Text = Ex.Message; UpdatePanel10.Update(); }
        }
    }
}