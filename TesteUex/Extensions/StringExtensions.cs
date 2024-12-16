using System.Globalization;
using System.Text;

namespace TesteUex.Extensions
{
    public static class StringExtensions
    {
        public static string GetNumbers(this String value)
        {
            var tmp = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                for (var i = 0; i < value.Length; i++)
                    if (Char.IsDigit(value[i]))
                        tmp += value[i];
            }

            return tmp;
        }

        public static string ToFormat(this String value, string mask)
        {
            string res = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                int p = 0;

                for (int i = 0; (i < mask.Length && p < value.Length); i++)
                {
                    if (mask[i] == '#')
                        res += value[p++];
                    else
                        res += mask[i];
                }
            }

            return res;
        }

        public static string RemoverAcentos(this string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);

                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(s[k]);
            }

            return sb.ToString();
        }
    }
}
