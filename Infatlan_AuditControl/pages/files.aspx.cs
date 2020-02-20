using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infatlan_AuditControl.classes;

namespace Infatlan_AuditControl.pages
{
    public partial class files : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }

            
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void BtnBuscarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxBuscarIdInforme.Text.Equals(""))
                    throw new Exception("Por favor busque un numero de informe");

                String vQuery = "[ACSP_ObtenerArchivos] 1," + TxBuscarIdInforme.Text;
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0)
                {
                    GVBusqueda.DataSource = vDatos;
                    GVBusqueda.DataBind();
                }
                else
                {
                    GVBusqueda.DataSource = null;
                    GVBusqueda.DataBind();
                    throw new Exception("No se encontrarón archivos para el informe No." + TxBuscarIdInforme.Text);
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                String vIdArchivo = e.CommandArgument.ToString();
                if (e.CommandName == "DescargarArchivo")
                {
                    String vQuery = "[ACSP_ObtenerArchivos] 2, " + vIdArchivo;
                    String vArchivo = vConexion.ejecutarSQLGetValueString(vQuery);

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
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
               
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}