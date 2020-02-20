using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Infatlan_AuditControl.classes
{
    public class Cargar
    {
        public int? cargarArchivo(String DireccionCarga, ref int vSuccess, ref int vError, String vUsuario)
        {
            int? vResultado = null;
            try
            {
                FileStream stream = File.Open(DireccionCarga, FileMode.Open, FileAccess.Read);

                IExcelDataReader excelReader;
                if (DireccionCarga.Contains("xlsx"))
                    //2007
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    //97-2003
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet vDatosExcel = excelReader.AsDataSet();
                excelReader.Close();

                DataSet vDatosVerificacion = vDatosExcel.Copy();
                for (int i = 0; i < vDatosVerificacion.Tables[0].Rows.Count; i++)
                {
                    if (verificarRow(vDatosVerificacion.Tables[0].Rows[i]))
                        vDatosExcel.Tables[0].Rows[i].Delete();
                }
                vDatosExcel.Tables[0].AcceptChanges();

                vResultado = procesarArchivo(vDatosExcel, ref vSuccess, ref vError, vUsuario);

            }
            catch (Exception)
            {
                throw;
            }
            return vResultado;
        }

        public int? procesarArchivo(DataSet vArchivo, ref int vSuccess, ref int vError, String vUsuario)
        {
            int? vInforme = null;
            try
            {
                db vConexion = new db();
                log vLog = new log();
                if (vArchivo.Tables.Count > 0)
                {
                    
                    if (vArchivo.Tables[0].Rows.Count > 0)
                    {
                        DataTable vDatosInforme = vArchivo.Tables[0];
                        foreach (DataRow item in vDatosInforme.Rows)
                        {
                            if (!item["NombreInforme"].ToString().Equals(""))
                            {
                                String vQuery = "[ACSP_Informes] 1,0,1,'" + vUsuario + "'," +
                                "'" + item["NombreInforme"].ToString() + "'," +
                                "'" + item["DescripcionInforme"].ToString() + "'," +
                                "'',0," +
                                "'" + DateTime.Parse(item["FechaRespuesta"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                vInforme = vConexion.ejecutarSQLGetValue(vQuery);
                                if (vInforme != null)
                                {
                                    vQuery = "[ACSP_Informes] 3," + vInforme + ",0,'','','','" + item["Responsable"].ToString() + "',1";
                                    vConexion.ejecutarSql(vQuery);
                                    if (!item["Copia"].ToString().Equals(""))
                                    {
                                        vQuery = "[ACSP_Informes] 3," + vInforme + ",0,'','','','" + item["Copia"].ToString() + "',2";
                                        vConexion.ejecutarSql(vQuery);
                                    }
                                }
                            }
                        }
                    }
                    if (vArchivo.Tables[1].Rows.Count > 0)
                    {
                        DataTable vDatosHallazgos = vArchivo.Tables[1];
                        foreach (DataRow item in vDatosHallazgos.Rows)
                        {
                            if (!item["responsable"].ToString().Equals(""))
                            {
                                String vQuery = "[ACSP_Hallazgos] 1,0,0," +
                                    "'" + vUsuario + "'," +
                                    "'" + item["Hallazgo"].ToString() + "'," +
                                    "'" + item["RiesgoIncumplimiento"].ToString() + "'," +
                                    "'" + item["Recomendaciones"].ToString() + "'," +
                                    "''," +
                                    "'" + DateTime.Parse(item["FechaRespuesta"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "1," +
                                    "'" + item["ProcesoPolitica"].ToString() + "'," +
                                    "" + item["Area"].ToString() + "," +
                                    "" + item["NivelRiesgo"].ToString() + ",'',''," +
                                    "'" + item["Responsable"].ToString() + "'";
                                int? vIdHallazgo = vConexion.ejecutarSQLGetValue(vQuery);
                                if (vIdHallazgo != null)
                                {
                                    vQuery = "[ACSP_Hallazgos] 3," + vIdHallazgo + "," + vInforme + "";
                                    vConexion.ejecutarSql(vQuery);
                                }
                            }
                        }
                    }
                }
                else
                    throw new Exception("No contiene ninguna hoja de excel.");
            }
            catch (Exception Ex)
            {
                String vErrorMessage = Ex.Message;
                throw;
            }
            return vInforme;
        }
        private bool verificarRow(DataRow dr)
        {
            int contador = 0;
            foreach (var value in dr.ItemArray)
            {
                if (value.ToString() != "")
                {
                    contador++;
                }
            }
            if (contador > 0)
                return false;
            else
                return true;
        }
    }
}