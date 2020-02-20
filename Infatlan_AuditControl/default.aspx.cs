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
                ObtenerDashboard();
            }
        }

        void ObtenerDashboard()
        {
            try
            {
                String vQuery = "[ACSP_Dashboard] 1";
                DataTable vDatos = vConexion.obtenerDataTable(vQuery);

                foreach (DataRow item in vDatos.Rows)
                {
                    LitTotalInformes.Text = item["TotalInformes"].ToString();
                    LitTotalHallazgos.Text = item["TotalHallazgos"].ToString();
                    LitTotalUsuarios.Text = item["TotalUsuarios"].ToString();
                    LitTotalComentarios.Text = item["TotalComentarios"].ToString();
                    LitTotalInformesFinalizados.Text = item["TotalInformesFinalizados"].ToString();
                    LitTotalHallazgosFinalizados.Text = item["TotalHallazgosFinalizados"].ToString();
                }
            }
            catch { }
        }
    }
}