using System;
using System.Text.RegularExpressions;

namespace ThirdCaliburnApp
{
    public class Commons
    {
        internal static readonly string CONNSTRING =
            "Data Source=localhost;" +
            "Port=3306;" +
            "Database=test2db;" +
            "Uid=root;" +
            "password=mysql_p@ssw0rd";

        public class EmployeesTbl
        {
            public static string SELECT_EMPLOYEES =
               "SELECT id, " +
               "          empName, " +
               "          Salary, " +
               "          deptName, " +
               "          destination " +
               " FROM employeestbl ";

            internal static string INSERT_EMPLOYEE =
                "INSERT INTO employeestbl " +
                "( " +
                "                  empName, " +
                "                  Salary, " +
                "                  deptName, " +
                "                  destination " +
                ") " +
                "       VALUES " +
                "( " +
                "                   @empName, " +
                "                   @Salary, " +
                "                   @deptName, " +
                "                   @destination " +
                ") ";
            internal static string UPDATE_EMPLOYEE =
                "UPDATE employeestbl " +
                "      SET " +
                "           empName = @empName, " +
                "           Salary = @Salary, " +
                "           deptName = @deptName, " +
                "           destination = @destination " +
                "WHERE id = @id ";

            internal static string DELETE_EMPLOYEE =
                "DELETE FROM employeestbl " +
                "WHERE id = @id ";  
        }

        internal static bool IsNumeric(string text)
        {
            // ^  -> 0부터 9까지 값이 아닌것을 판단
            Regex regx = new Regex("[^0-9.-]+");
            return !regx.IsMatch(text);
        }
    }
}