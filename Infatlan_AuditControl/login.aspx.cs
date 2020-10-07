using Infatlan_AuditControl.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Infatlan_AuditControl
{
    public partial class login : System.Web.UI.Page
    {
        db vConexion;
        protected void Page_Load(object sender, EventArgs e){
            vConexion = new db();
            if (!Page.IsPostBack){

            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e){
            try{
                LdapService vLdap = new LdapService();
                //Boolean vLogin = vLdap.ValidateCredentials(ConfigurationManager.AppSettings["ADHOST"], TxUsername.Text, TxPassword.Text);
                Boolean vLogin = true;

                if (vLogin){
                    //DataTable vDatosUsuarioLdap = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], TxUsername.Text);
                    //DataTable vDatosUsuarioLdap = new DataTable();
                    //vDatosUsuarioLdap.Columns.Add("givenName");
                    //vDatosUsuarioLdap.Columns.Add("sn");
                    //vDatosUsuarioLdap.Columns.Add("mail");

                    //vDatosUsuarioLdap.Rows.Add(
                    //    "dehenriquez",
                    //    "Henriquez",
                    //    "dehenriquez@bancatlan.hn"
                    //);

                    DataTable vDatos = new DataTable();
                    vDatos = vConexion.obtenerDataTable("ACSP_Login '" + TxUsername.Text + "'" );

                    foreach (DataRow item in vDatos.Rows){
                        Session["AUTHCLASS"] = vDatos;
                        Session["USUARIO"] = item["idUsuario"].ToString();  
                        Session["TIPOUSUARIO"] = item["tipoUsuario"].ToString();
                        Session["JEFEAUDITORIA"] = item["jefeAuditoria"].ToString();

                        Session["AUTH"] = true;
                        Response.Redirect("/default.aspx");
                    }
                }else{
                    Session["AUTH"] = false;
                    throw new Exception("Usuario o contraseña incorrecta.");
                }
            }catch (Exception Ex){
                LbMensaje.Text = "Usuario o contraseña incorrecta.";
                String vErrorLog = Ex.Message;
            }
        }

        protected void TxPassword_TextChanged(object sender, EventArgs e){
            try{
                BtnLogin_Click(sender, e);
            }catch (Exception ex){

            }
        }
    }
}