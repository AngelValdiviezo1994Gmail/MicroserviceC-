using Common.Domain.Interfaces;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Infrastructure.Helpers
{
    public class UtilsHelper : IUtilsHelper
    {
        public string[] GuessName(String FullName)
        {
            var slices = FullName.Split(" ");

            var pn = "";
            var sn = "";
            var pa = "";
            var sa = "";


            if (slices.Count() == 3)
            {
                pn = slices[0];
                pa = slices[1];
                sa = slices[2];
            }
            else if (slices.Count() > 3)
            {

                int secondLastNameIndex = slices.Count() - 1;
                int firstLastNameIndex = slices.Count() - 2;

                string middleName = "";

                for (int i = 1; i < slices.Count() - 2; i++)
                {
                    middleName += slices[i] + " ";
                }

                pn = slices[0];
                sn = middleName;
                pa = slices[firstLastNameIndex];
                sa = slices[secondLastNameIndex];
            }
            else
            {
                return new string[] { };
            }

            return new string[] { pn, sn, pa, sa };
        }

        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetEnumMemberAttrValue<T>(T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if (attr != null)
            {
                return attr.Value;
            }

            return null;
        }

    }
}
