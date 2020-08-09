using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoMonitoringWpf_Test.Helpers
{
    class Commons
    {
        internal static readonly string CONNSTRING =
           "Data Source=localhost;" +
           "Port=3306;" +
           "Database=iot_sensordata;" +
           "Uid=root;" +
           "password=mysql_p@ssw0rd";

        public class Sensordatatbl
        {
            public static string STRQUERY =
                "INSERT INTO sensordatatbl " +
                " (Date, Value) " +
                " VALUES " +
                " (@Date, @Value) ";
        }
    }
}
