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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["AUTH"] != null)
                {
                    switch (Convert.ToInt32(Session["TIPOUSUARIO"]))
                    {
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
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
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
                    catch {}
                }

                DDLUserResponsable.Items.Add(new ListItem { Value="0", Text="Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows)
                {
                    DDLUserResponsable.Items.Add(new ListItem { Value = item["usuario"].ToString(), Text = item["nombre"].ToString() + " " + item["apellido"].ToString() +  " - " + item["correo"].ToString() });
                }

                DDLUserResponsable.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        public void getTiposEnvio()
        {
            try
            {
                String vQuery = "[ACSP_TiposEnvio]";
                DataTable vDatosFinal = vConexion.obtenerDataTable(vQuery);


                DDLTipoResponsable.Items.Add(new ListItem { Value = "0", Text = "Seleccione un usuario" });
                foreach (DataRow item in vDatosFinal.Rows)
                {
                    DDLTipoResponsable.Items.Add(new ListItem { Value = item["tipoEnvio"].ToString(), Text = item["tipoEnvio"].ToString() + "-" + item["nombre"].ToString() });
                }

                DDLTipoResponsable.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCrearInforme_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLUserResponsable.SelectedIndex.Equals(0))
                    throw new Exception("Por favor seleccione un responsable.");

                if (TxNombreInforme.Text.Equals(""))
                    throw new Exception("Por favor escriba el nombre del informe");

                //if (TxDescripcionInforme.Text.Equals(""))
                //    throw new Exception("Por favor escriba una descripción del informe");


                if (Session["DATARESPONSABLES"] is null)
                {
                    throw new Exception("Por favor ingrese un reponsable para el informe");
                }
                else
                {
                    vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];
                }
                
                String vQuery = "[ACSP_Informes] 1,0,1,'" + Convert.ToString(Session["USUARIO"]) + "'," +
                    "'" + TxNombreInforme.Text.Replace("'", "") + "'," +
                    "'" + TxDescripcionInforme.Text.Replace("'","") + "'," +
                    "'',0," +
                    "'" + TxFechaRespuesta.Text + "'";

                int? vIdInforme = vConexion.ejecutarSQLGetValue(vQuery);
                if (vIdInforme != null)
                {
                    try
                    {
                        String vNombreDeposito = String.Empty;
                        HttpPostedFile bufferDeposito1T = FUInforme.PostedFile;
                        byte[] vFileDeposito1 = null;
                        if (bufferDeposito1T != null)
                        {
                            vNombreDeposito = FUInforme.FileName;
                            Stream vStream = bufferDeposito1T.InputStream;
                            BinaryReader vReader = new BinaryReader(vStream);
                            vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                        }
                        String vDeposito = Convert.ToBase64String(vFileDeposito1);
                        vQuery = "[ACSP_Informes] 4," + vIdInforme + ",0,'','','','','','','" + vDeposito + "'";
                        vConexion.ejecutarSql(vQuery);
                    }
                    catch { }

                    foreach (DataRow item in vDatosResponsables.Rows)
                    {
                        vQuery = "[ACSP_Informes] 3," + vIdInforme + ",0,'','','','" + item["usuarioResponsable"].ToString() + "'," + item["envio"].ToString().Split('-')[0].ToString();
                        if (vConexion.ejecutarSql(vQuery).Equals(1))
                        {
                            Mensaje("Ingresado con Exito, proceda a ingresar los hallazgos", WarningType.Success);
                            LimpiarInformes();
                            Response.Redirect("/pages/ereports.aspx?id=" + vIdInforme);
                        }
                    }
                }
                else
                    throw new Exception("Error al ingresar el informe por favor consulte con sistemas");

            }
            catch (Exception Ex) { LbMensajeCrearInforme.Text += Ex.Message; }
        }

        void LimpiarInformes()
        {
            TxNombreInforme.Text = String.Empty;
            TxDescripcionInforme.Text = String.Empty;
            DDLUserResponsable.SelectedIndex = -1;
            DDLTipoResponsable.SelectedIndex = -1;
            GVResponsables.DataSource = null;
            GVResponsables.DataBind();
            Session["DATARESPONSABLES"] = null;
            TxFechaRespuesta.Text = String.Empty;

        }

        protected void BtnAgregarResponsable_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLUserResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione un usuario!");
                if (DDLTipoResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor selccione un tipo de envio");


                if (Session["DATARESPONSABLES"] == null)
                {
                    vDatosResponsables = new DataTable();
                    vDatosResponsables.Columns.Add("usuarioResponsable");
                    vDatosResponsables.Columns.Add("envio");
                }
                else
                {
                    vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];
                }


                Boolean vFlagInsert = false;
                for (int i = 0; i < vDatosResponsables.Rows.Count; i++)
                {
                    if (vDatosResponsables.Rows[i]["usuarioResponsable"].ToString().Equals(DDLUserResponsable.SelectedValue))
                        vFlagInsert = true;
                }

                if (!vFlagInsert)
                    vDatosResponsables.Rows.Add(DDLUserResponsable.SelectedValue, DDLTipoResponsable.SelectedItem.Text);
                else
                    throw new Exception("Este usuario ya ha sido agregado");

                GVResponsables.DataSource = vDatosResponsables;
                GVResponsables.DataBind();
                Session["DATARESPONSABLES"] = vDatosResponsables;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVResponsables_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdResponsable = e.CommandArgument.ToString();
                    if (Session["DATARESPONSABLES"] != null)
                    {
                        vDatosResponsables = (DataTable)Session["DATARESPONSABLES"];

                        DataRow[] result = vDatosResponsables.Select("usuarioResponsable = '" + vIdResponsable + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["usuarioResponsable"].ToString().Contains(vIdResponsable))
                                vDatosResponsables.Rows.Remove(row);
                        }
                    }
                }
                GVResponsables.DataSource = vDatosResponsables;
                GVResponsables.DataBind();
                Session["DATARESPONSABLES"] = vDatosResponsables;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}