using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace LibGP.Utils
{
    public static class Extensions
    {
        public static bool Compare(this byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }

        // http://stackoverflow.com/questions/7049827/linq-singleordefault-how-to-setup-a-default-for-a-custom-class
        public static T SingleOr<T>(this IEnumerable<T> list, T defaultValue) where T : class
        {
            return list.SingleOrDefault() ?? defaultValue;
        }

        // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        public static String Replace(this String str, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        // http://stackoverflow.com/questions/283456/byte-array-pattern-search @YujiSoftware
        public static IEnumerable<int> IndexOf(this byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }
    }

    public static class Xml
    {
        public static T Deserialize<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(toDeserialize);
            return (T)xmlSerializer.Deserialize(textReader);
        }

        public static string Serialize<T>(this T toSerialize)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringWriter tw = new StringWriter();
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlWriter xw = XmlWriter.Create(tw, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true
            });
            xs.Serialize(xw, toSerialize, emptyNs);
            xw.Close();
            return tw.ToString();
        }
    }

    public class CommandLine
    {
        private Dictionary<String, String> _cmd = new Dictionary<String, String>();

        public CommandLine(string[] args)
        {
            String lastKey = String.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                String arg = args[i];
                if (arg.StartsWith("\"")) { arg = arg.Substring(1); }
                if (arg.EndsWith("\"")) { arg = arg.Substring(0, arg.Length - 1); }
                if (arg.StartsWith("-"))
                {
                    lastKey = arg.Substring(1);
                    _cmd.Add(lastKey, null);
                }
                else
                {
                    _cmd[lastKey] = arg;
                }
            }
        }

        public bool ContainsArg(String key)
        {
            return _cmd.ContainsKey(key);
        }
        public int Length { get { return _cmd.Count; } }

        public String this[String key]
        {
            get {
                return _cmd[key];
            }
            set {
                _cmd[key] = value;
            }
        }

        public String this[int index]
        {
            get
            {
                return _cmd.ToList()[index].Value;
            }
        }
    }

    // http://www.codeproject.com/Tips/279969/Recursively-Copy-folder-contents-to-another-in-C
    public static class FolderManager
    {
        public static Exception LastError { get; private set; }
        public static void ClearLastError()
        {
            LastError = null;
        }

        public static bool CopyFolderContents(string sourcePath, string destinationPath)
        {
            string slashEndingSourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
            string slashEndingDestinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";
            try
            {
                if (Directory.Exists(slashEndingSourcePath))
                {
                    if (!Directory.Exists(slashEndingDestinationPath))
                        Directory.CreateDirectory(slashEndingDestinationPath);

                    foreach (string files in Directory.GetFiles(slashEndingSourcePath))
                        new FileInfo(files).CopyTo(slashEndingDestinationPath + new FileInfo(files).Name, true);

                    foreach (string drs in Directory.GetDirectories(slashEndingSourcePath))
                        if (!CopyFolderContents(drs, slashEndingDestinationPath + new DirectoryInfo(drs).Name))
                            return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }


    }

    // http://omegacoder.com/?p=359
    public static class INI
    {
        public static Dictionary<String, Dictionary<String, String>> Load(String filename)
        {
            string pattern = @"
                                ^                           # Beginning of the line
                                ((?:\[)                     # Section Start
                                 (?<Section>[^\]]*)         # Actual Section text into Section Group
                                 (?:\])                     # Section End then EOL/EOB
                                 (?:[\r\n]{0,}|\Z))         # Match but don't capture the CRLF or EOB
                                 (                          # Begin capture groups (Key Value Pairs)
                                   (?!\[)                    # Stop capture groups if a [ is found; new section
                                   (?<Key>[^=]*?)            # Any text before the =, matched few as possible
                                   (?:=)                     # Get the = now
                                   (?<Value>[^\r\n]*)        # Get everything that is not an Line Changes
                                   (?:[\r\n]{0,4})           # MBDC \r\n
                                  )+                        # End Capture groups";
            Dictionary<string, Dictionary<string, string>> rv = (from Match m in Regex.Matches(File.ReadAllText(filename), pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
            select new
            {
                Section = m.Groups["Section"].Value,
                kvps = (from cpKey in m.Groups["Key"].Captures.Cast<Capture>().Select((a, i) => new { a.Value, i })
                        join cpValue in m.Groups["Value"].Captures.Cast<Capture>().Select((b, i) => new { b.Value, i }) on cpKey.i equals cpValue.i
                        select new KeyValuePair<string, string>(cpKey.Value, cpValue.Value)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            }).ToDictionary(itm => itm.Section, itm => itm.kvps);
            return rv;
        }
    }

    public static class Actions
    {
        public static void Retry(Action action, TimeSpan sleepPeriod, int retryCount = 3)
        {
            while (true)
            {
                try
                {
                    action();
                    break; // success!
                }
                catch
                {
                    if (--retryCount == 0) throw;
                    else Thread.Sleep(sleepPeriod);
                }
            }
        }
    }
}
