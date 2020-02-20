using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Infatlan_AuditControl.classes
{
    public class CalendarConnection
    {
        static db vConexion = new db();
        public static List<CalendarEvent> getEvents(DateTime start, DateTime end)
        {

            String vQuery = "[ACSP_ObtenerInformes] 1";
            DataTable vDatos = vConexion.obtenerDataTable(vQuery);

            List<CalendarEvent> events = new List<CalendarEvent>();

            foreach (DataRow item in vDatos.Rows)
            {
                events.Add(new CalendarEvent()
                {
                    id = Convert.ToInt32(item["idInforme"].ToString()),
                    title = Convert.ToString(item["nombre"].ToString()),
                    description = Convert.ToString(item["descripcion"].ToString()),
                    start = Convert.ToDateTime(item["fechaCreacion"].ToString()),
                    end = Convert.ToDateTime((item["fechaCreacion"].ToString().Equals("") ? item["fechaCreacion"].ToString() : item["fechaCreacion"].ToString())),
                    allDay = false,
                    status = Convert.ToInt32(item["EstadoNumero"].ToString())
                });
            }

            return events;
        }

        public static void updateEvent(int id, String title, String description)
        {

        }

        public static void updateEventTime(int id, DateTime start, DateTime end, bool allDay)
        {

        }

        public static void deleteEvent(int id)
        {

        }

        public static int addEvent(CalendarEvent cevent)
        {
            int key = 0;
            return key;
        }
    }
}