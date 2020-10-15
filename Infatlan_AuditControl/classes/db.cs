using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infatlan_AuditControl.classes
{
    public enum WarningType
    {
        Success,
        Info,
        Warning,
        Danger
    }
    public class db
    {
        SqlConnection vConexion;
        public db()
        {
            vConexion = new SqlConnection(ConfigurationManager.AppSettings["SQLServer"]);
        }

        public DataTable obtenerDataTable(String vQuery)
        {
            DataTable vDatos = new DataTable();
            try
            {
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
            }
            catch
            {
                throw;
            }
            return vDatos;
        }

        public int ejecutarSql(String vQuery)
        {
            int vResultado = 0;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = vSqlCommand.ExecuteNonQuery();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public int ejecutarSQLGetValue(String vQuery)
        {
            int vResultado = 0;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (Int32)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String ejecutarSQLGetValueString(String vQuery){
            String vResultado = String.Empty;
            try{
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (String)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public Boolean ejecutarSQLGetValueBoolean(String vQuery)
        {
            Boolean vResultado = false;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (Boolean)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String GetNombreInforme(String vId)
        {
            String vResultado = String.Empty;
            try
            {
                String vQuery = "[ACSP_Generales] 1," + vId; 
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (String)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String GetNombreUsuario(String vUsuario){
            String vResultado = String.Empty;
            try{
                LdapService vLdap = new LdapService();
                DataTable vDatos = vLdap.GetDatosUsuario(ConfigurationManager.AppSettings["ADHOST"], vUsuario);
                foreach (DataRow item in vDatos.Rows){
                    vResultado = item["givenName"].ToString() + " " + item["sn"].ToString();
                }
                //DataTable vDatosJefatura = obtenerDataTable("ACSP_Login '" + vUsuario + "'");
                //vResultado = vDatosJefatura.Rows[0]["nombre"].ToString();
            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String ValidarEstadoHallazgo(String vHallazgo){
            String vResultado = String.Empty;
            try{
                String vQuery = "[ACSP_ObtenerTipos] 5," + vHallazgo;
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (String)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String ActualizarEstadoHallazgo(String vHallazgo, String vPaso){
            String vResultado = String.Empty;
            try{
                String vQuery = "[ACSP_Hallazgos] 8," + vHallazgo + "," + vPaso;
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = Convert.ToString((int)vSqlCommand.ExecuteScalar());
                vConexion.Close();
            }catch (Exception Ex){
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String ValidarAutorizacionHallazgo(String vHallazgo)
        {
            String vResultado = String.Empty;
            try
            {
                String vQuery = "[ACSP_ObtenerHallazgos] 8," + vHallazgo;
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = Convert.ToString((int)vSqlCommand.ExecuteScalar());
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }
    }
}