using System.Text.RegularExpressions;
using TesteUex.Extensions;

namespace TesteUex.Helpers
{
    public static class Validators
    {
        public static bool ValidateCpf(string cpf)
        {
            int i;
            string s = cpf.GetNumbers();

            if (s == "11111111111" ||
                s == "22222222222" ||
                s == "33333333333" ||
                s == "44444444444" ||
                s == "55555555555" ||
                s == "66666666666" ||
                s == "77777777777" ||
                s == "88888888888" ||
                s == "99999999999" ||
                s == "00000000000" ||
                s.Length != 11)
            {

                return false;
            }

            string c = s.Substring(0, 9);
            string dv = s.Substring(9, 2);
            int d1 = 0;

            for (i = 0; i < 9; i++)
                d1 += int.Parse(c[i].ToString()) * (10 - i);

            if (d1 == 0)
            {
                return false;
            }

            d1 = 11 - (d1 % 11);
            if (d1 > 9) d1 = 0;

            if (int.Parse(dv[0].ToString()) != d1)
            {
                return false;
            }

            d1 *= 2;
            for (i = 0; i < 9; i++) d1 += int.Parse(c[i].ToString()) * (11 - i);

            d1 = 11 - (d1 % 11);
            if (d1 > 9) d1 = 0;

            if (int.Parse(dv[1].ToString()) != d1)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateEmail(string s)
        {
            string validEmailPattern = @"^[a-z0-9][a-z0-9\._-]+@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var r = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            return r.IsMatch(s);
        }

        public static bool ValidatePhone(string numero)
        {
            if (numero.GetNumbers().Length == 0)
                return false;

            if (numero[0] == '9')
            {
                if (numero.Length != 9)
                    return false;
            }
            else
            {
                var array = new char[] { '6', '7', '8', '9' };

                if (numero.Length != 8 || Array.IndexOf(array, numero[0]) > -1)
                    return false;
            }

            return true;
        }

        public static bool ValidateDdd(string ddd)
        {
            if (string.IsNullOrEmpty(ddd))
                return false;

            var n = Convert.ToInt32(ddd);
            return n >= 11 && n <= 99;
        }

        public static string FormatarTelefone(string t)
        {
            if (!string.IsNullOrWhiteSpace(t))
            {
                t = t.Trim().GetNumbers();

                if (t.StartsWith("0") && t.Length == 11)
                    t = t.ToFormat("####-###-####");
                else if (t.Length == 8)
                    t = t.ToFormat("####-####");
                else if (t.Length == 9)
                    t = t.ToFormat("#####-####");
                else if (t.Length == 10)
                    t = t.ToFormat("(##) ####-#####");
                else if (t.Length == 11)
                    t = t.ToFormat("(##) #####-####");
            }

            return t;
        }
    }
}
