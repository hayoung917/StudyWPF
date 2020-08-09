using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmApp.Helpers
{
    public class Commons
    {
        public static bool IsValidEmail(string email)
        {
            string[] parts = email.Split('@');
            if (parts.Length != 2) 
                return false;

            //email형식 잘라서 확인하는 방법(기초)
            return (parts[1].Split('.').Length >= 2);
        }

        public static int CalcAge(DateTime date)
        {
            int middle;
            DateTime now = DateTime.Now;
            if (now.Month <= date.Month && now.Day < date.Day)
                middle = now.Year - date.Year - 1;  //생일 안지났으면
            else
                middle = now.Year - date.Year;  //생일 지났으면

            return middle;

        }

        public static string GetChineseZodiac(DateTime date)
        {
            var value = date.Year % 12;
            switch (value)
            {
                case 0:
                    return "원숭이띠";
                case 1:
                    return "닭띠";
                case 2:
                    return "개띠";
                case 3:
                    return "돼지띠";
                case 4:
                    return "쥐띠";
                case 5:
                    return "소띠";
                case 6:
                    return "호랑이띠";
                case 7:
                    return "토끼띠";
                case 8:
                    return "용띠";
                case 9:
                    return "뱀띠";
                case 10:
                    return "말띠";
                case 11:
                    return "양띠";
                default:
                    return " ";
            }
        }
        public static string GetCalcZodiac(DateTime date)
        {
            if ((date.Month == 1 && date.Day <= 20) || (date.Month == 12 && date.Day >= 23))
                return "염소자리";
            else if ((date.Month == 2 && date.Day <= 19) || (date.Month == 1 && date.Day >= 21))
                return "물병자리";
            else if ((date.Month == 3 && date.Day <= 20) || (date.Month == 2 && date.Day >= 20))
                return "물고기자리";
            else if ((date.Month == 4 && date.Day <= 20) || (date.Month == 3 && date.Day >= 21))
                return "양자리";
            else if ((date.Month == 5 && date.Day <= 20) || (date.Month == 4 && date.Day >= 21))
                return "황소자리";
            else if ((date.Month <= 6 && date.Day <= 21) || (date.Month == 5 && date.Day >= 21))
                return "쌍둥이자리";
            else if ((date.Month <= 7 && date.Day <= 22) || (date.Month == 6 && date.Day >= 22))
                return "게자리";
            else if ((date.Month <= 8 && date.Day <= 22) || (date.Month == 7 && date.Day >= 23))
                return "사자자리";
            else if ((date.Month <= 9 && date.Day <= 22) || (date.Month == 8 && date.Day >= 23))
                return "처녀자리";
            else if ((date.Month <= 10 && date.Day <= 23) || (date.Month == 9 && date.Day >= 23))
                return "천칭자리";
            else if ((date.Month <= 11 && date.Day <= 22) || (date.Month == 10 && date.Day >= 24))
                return "전갈자리";
            else
                return "사수자리";
        }
    }
}
