using HandlebarsDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Seq.App.DingTalk
{
    public static class HandlebarsHelpers
    {
        public static void Register()
        {
            Handlebars.RegisterHelper("pretty", PrettyPrintHelper);
            Handlebars.RegisterHelper("if_eq", IfEqHelper);
            Handlebars.RegisterHelper("substring", SubstringHelper);
            Handlebars.RegisterHelper("formatDate", FormatDateHelper);
        }

        static void FormatDateHelper(TextWriter output, dynamic context, object[] arguments)
        {
            var value = arguments.FirstOrDefault();
            if (value == null)
            {
                output.WriteSafeString("null");
                return;
            }
            var data = value as System.DateTime?;
            if (data == null || !data.HasValue)
            {
                output.WriteSafeString(value);
                return;
            }
            var format = arguments.Skip(1).FirstOrDefault()?.ToString();
            if (format != null)
            {
                var cultureName = arguments.Skip(2).FirstOrDefault()?.ToString();
                var culture = string.IsNullOrWhiteSpace(cultureName) ? System.Globalization.CultureInfo.CurrentCulture : new System.Globalization.CultureInfo(cultureName);
                output.WriteSafeString(data.Value.ToString(format, culture));
            }
        }

        static void PrettyPrintHelper(TextWriter output, object context, object[] arguments)
        {
            var value = arguments.FirstOrDefault();
            if (value == null)
                output.WriteSafeString("null");
            else if (value is IEnumerable<object> || value is IEnumerable<KeyValuePair<string, object>>)
                output.Write(JsonConvert.SerializeObject(FromDynamic(value)));
            else
            {
                var str = value.ToString();
                if (string.IsNullOrWhiteSpace(str))
                {
                    output.WriteSafeString("&nbsp;");
                }
                else
                {
                    output.Write(str);
                }
            }
        }

        static void IfEqHelper(TextWriter output, HelperOptions options, dynamic context, object[] arguments)
        {
            if (arguments?.Length != 2)
            {
                options.Inverse(output, context);
                return;
            }

            var lhs = (arguments[0]?.ToString() ?? "").Trim();
            var rhs = (arguments[1]?.ToString() ?? "").Trim();

            if (lhs.Equals(rhs, StringComparison.Ordinal))
            {
                options.Template(output, context);
            }
            else
            {
                options.Inverse(output, context);
            }
        }

        static object FromDynamic(object o)
        {
            if (o is IEnumerable<KeyValuePair<string, object>> dictionary)
            {
                return dictionary.ToDictionary(kvp => kvp.Key, kvp => FromDynamic(kvp.Value));
            }

            if (o is IEnumerable<object> enumerable)
            {
                return enumerable.Select(FromDynamic).ToArray();
            }

            return o;
        }

        static void SubstringHelper(TextWriter output, object context, object[] arguments)
        {
            //{{ substring value 0 30 }}
            var value = arguments.FirstOrDefault();

            if (value == null)
                return;

            if (arguments.Length < 2)
            {
                // No start or length arguments provided
                output.Write(value);
            }
            else if (arguments.Length < 3)
            {
                // just a start position provided
                int.TryParse(arguments[1].ToString(), out var start);
                if (start > value.ToString().Length)
                {
                    // start of substring after end of string.
                    return;
                }
                output.Write(value.ToString().Substring(start));
            }
            else
            {
                // Start & length provided.
                int.TryParse(arguments[1].ToString(), out var start);
                int.TryParse(arguments[2].ToString(), out var end);

                if (start > value.ToString().Length)
                {
                    // start of substring after end of string.
                    return;
                }
                // ensure the length is still in the string to avoid ArgumentOutOfRangeException
                if (end > value.ToString().Length - start)
                {
                    end = value.ToString().Length - start;
                }

                output.Write(value.ToString().Substring(start, end));
            }
        }
    }
}
