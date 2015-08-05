using System;
using System.Collections;

namespace LibDIV2
{
    public static class Utils
    {
        public static String SanitizeString(String str)
        {
            String rv = String.Empty;
            foreach (Char c in str)
                if (c < 32)
                    break;
                else
                    rv += c;
            rv = rv.Replace("?", "_");
            return rv;
        }
    }
}
