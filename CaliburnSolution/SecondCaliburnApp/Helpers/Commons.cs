using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondCaliburnApp.Helpers
{
    public class Commons
    {
        public static string STRCONNSTRING =
            "Data Source=localhost;" +
            "Port=3306;" +
            "Database=test2db;" +
            "Uid=root;" +
            "password=mysql_p@ssw0rd";

        public static string SELECTPEOPLEQUERY =
            "SELECT id, FirstName, LastName " +
            " FROM peopletbl " +
            "ORDER BY id ";
    }
}
