using System.Globalization;

namespace KG_6
{
    internal class Utilities
    {
        private static CultureInfo dotSeparatorCI;

        static Utilities()
        {
            dotSeparatorCI = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            dotSeparatorCI.NumberFormat.CurrencyDecimalSeparator = ".";
        }

        public static float FloatParse(string str)
        {
            return float.Parse(str, NumberStyles.Any, dotSeparatorCI);
        }

        public static bool FloatTryParse(string str, out float r)
        {
            return float.TryParse(str, NumberStyles.Any, dotSeparatorCI, out r);
        }
    }
}
