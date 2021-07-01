using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Infatlan_AuditControl.pages
{
    public partial class resume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack){
                String vIdInforme = Request.QueryString["id"];

                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    String vPass = ConfigurationManager.AppSettings["ReportingPass"];
                    String vUser = ConfigurationManager.AppSettings["ReportingUser"];
                    ServerReport serverReport = ReportViewer1.ServerReport;
                    serverReport.ReportServerCredentials = new ReportServerCredentials(vUser, vPass, "adbancat.hn");

                    serverReport.ReportServerUrl =
                        new Uri("http://10.128.0.52/ReportServer");
                    serverReport.ReportPath = "/Auditoria Interna/AuditoriaInforme";


                    ReportParameter vReportParameter = new ReportParameter();
                    vReportParameter.Name = "INFORME";
                    vReportParameter.Values.Add(vIdInforme);

                    ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { vReportParameter });
                }
                else
                    Response.Redirect("/login.aspx");
            }
        }
    }
    public class ReportServerCredentials : IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;

        public ReportServerCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                // Use default identity.
                return null;
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // Use default identity.
                return new NetworkCredential(_userName, _password, _domain);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // Do not use forms credentials to authenticate.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}