using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infatlan_AuditControl
{
    public partial class _default : System.Web.UI.Page
    {
        db vConexion = new db();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Convert.ToBoolean(Session["AUTH"])){
                    ObtenerDashboard();
                }else {
                    Response.Redirect("/login.aspx");
                }
                //classes.SmtpService vSmtp = new SmtpService();
                //vSmtp.EnviarMensaje("dehenriquez@bancatlan.hn", "Hola esto es una prueba", "test");
            }
        }

        void ObtenerDashboard(){
            try{
                String vQuery = "[ACSP_Dashboard] 1,'" + Session["USUARIO"].ToString() + "'";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                foreach (DataRow item in vDatos.Rows){
                    LitTotalInformes.Text = item["TotalInformes"].ToString();
                    LitTotalHallazgos.Text = item["TotalHallazgos"].ToString();
                    LitTotalInformesFinalizados.Text = item["TotalInformesFinalizados"].ToString();
                    LitTotalHallazgosFinalizados.Text = item["TotalHallazgosFinalizados"].ToString();
                    LitTotalInformesPendientes.Text = item["TotalInformesPendientes"].ToString();

                    LitIngresados.Text = item["TotalIngresados"].ToString();
                    LitValidacion.Text = item["TotalValidacion"].ToString();
                    LitEnProceso.Text = item["TotalEnProceso"].ToString();
                    LitHallazgosVencidos.Text = item["TotalVencidos"].ToString();
                }

                vQuery = "[ACSP_Dashboard] 2,'" + Session["USUARIO"].ToString() + "'";
                vDatos = vConexion.obtenerDataTable(vQuery);
                if (vDatos.Rows.Count > 0){
                    Session["DASHBOARD_HALLAZGOS"] = vDatos;
                    GVHallazgos.DataSource = vDatos;
                    GVHallazgos.DataBind();
                }


            }catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        protected void GVHallazgos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                GVHallazgos.PageIndex = e.NewPageIndex;
                GVHallazgos.DataSource = (DataTable)Session["DASHBOARD_HALLAZGOS"];
                GVHallazgos.DataBind();
            }catch (Exception Ex) { 
                throw new Exception( Ex.Message); 
            }
        }
    }
}