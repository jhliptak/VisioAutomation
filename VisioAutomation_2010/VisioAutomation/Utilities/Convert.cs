﻿namespace VisioAutomation.Utilities
{
    public static class Convert
    {
        private const string doublequote = "\"";
        private const string doublequote_x2 = "\"\"";

        public static string FormulaEncodeSmart(string s)
        {
            if (s == null)
            {
                throw new System.ArgumentNullException(nameof(s));
            }

            // if its empty or begins with '=' return it as is
            if (s.Length == 0 || s[0]=='=')
            {
                return s;
            }

            var result_quote_escaped = s.Replace(Convert.doublequote, Convert.doublequote_x2);
            string result = string.Format("\"{0}\"", result_quote_escaped);

            return result;
        }

        public static string FormulaDecodeSmart(string formula)
        {
            if (formula == null)
            {
                throw new System.ArgumentNullException(nameof(formula));
            }

            // Initialize the converted formula from the value passed in.
            string output_string = formula;

            // Check if this formula value is a quoted string.
            // If it is, remove extra quote characters.
            if (output_string.StartsWith(Convert.doublequote) &&
                output_string.EndsWith(Convert.doublequote))
            {

                // Remove the wrapping quote characters as well as any
                // extra quote marks in the body of the string.
                output_string = output_string.Substring(1, (output_string.Length - 2));
                output_string = output_string.Replace(Convert.doublequote_x2, Convert.doublequote);
            }

            return output_string;
        }
    }
}