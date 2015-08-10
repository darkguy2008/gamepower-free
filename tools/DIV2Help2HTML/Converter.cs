using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DIV2Help2HTML
{
    public static class Converter
    {
        public static void BatchConvert(String[] files, bool MSDOS)
        {
            foreach (String f in files)
            {
                FileInfo fi = new FileInfo(f);
                Convert(f, fi.DirectoryName + "\\" + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".htm", MSDOS);
            }
        }

        public static void Convert(String src, String dst, bool MSDOS)
        {
            String filename = src;
            String output = dst;

            String file = File.ReadAllText(filename, MSDOS ? Encoding.GetEncoding(437) : Encoding.Default);
            String sr = @"^\{\#9999,(.*)\}(.*(.|\n)*?)({-})";
            Regex rx = new Regex(sr, RegexOptions.Multiline);
            List<String> lPrograms = new List<String>();
            while (rx.Matches(file).Count > 0)
            {
                lPrograms.Add(rx.Matches(file)[0].Groups[2].Value);
                file = rx.Replace(file, @"###PROGRAMA###", 1);
            }
            using (StreamWriter sw = File.CreateText(output))
                sw.Write(file);

            String[] lines = File.ReadAllLines(output).ToArray();
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (String line in lines)
                {
                    String l = line;
                    l = HttpUtility.HtmlEncode(l);

                    if (l.StartsWith("# ─────"))
                        l = "<hr/>\n";

                    sw.WriteLine(l);
                }
            }

            file = File.ReadAllText(output);
            file = file.Replace("·", "###SALTO1###");
            file = file.Replace("&#183;", "###SALTO1###");
            file = file.Replace("\r\n\r\n", "###SALTO2###");
            file = file.Replace("\r\n", " ");
            file = file.Replace("###SALTO1###", "<br/>" + Environment.NewLine);
            file = file.Replace("###SALTO2###", "<br/><br/>" + Environment.NewLine);
            using (StreamWriter sw = File.CreateText(output))
                sw.Write(file);
            lines = File.ReadAllLines(output).ToArray();

            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (String line in lines)
                {
                    String l = line;
                    l = l.Trim();

                    if (l.StartsWith("#"))
                    {
                        l = l.Substring(2);
                        l = "<h1>" + l + "</h1>";
                    }

                    l = l.Replace("{/}", "<hr/>\n");

                    sr = @"\{\+([0-9])*,([0-9])*\}";
                    rx = new Regex(sr, RegexOptions.None);
                    l = rx.Replace(l, "");

                    sr = @"\{\-\}";
                    rx = new Regex(sr, RegexOptions.None);
                    l = rx.Replace(l, "");

                    sw.WriteLine(l);
                }
            }

            lines = File.ReadAllLines(output).ToArray();
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (String line in lines)
                {
                    String l = line;
                    l = l.Trim();

                    string strRegex = @"{\.([0-9]*?),(.*?)}";
                    Regex myRegex = new Regex(strRegex, RegexOptions.None);
                    string strReplace = @"<h2 id=""$1"">$2</h2>";
                    l = myRegex.Replace(l, strReplace);

                    strRegex = @"{\#([0-9]*?),(.*?)}";
                    myRegex = new Regex(strRegex, RegexOptions.None);
                    strReplace = @"<a href=""#$1"">$2</a>";
                    l = myRegex.Replace(l, strReplace);

                    strRegex = @"<a href=""#0(.*?)\""";
                    myRegex = new Regex(strRegex, RegexOptions.None);
                    strReplace = @"<a href=""#$1""";
                    l = myRegex.Replace(l, strReplace);

                    sw.WriteLine(l);
                }
            }

            file = File.ReadAllText(output);
            sr = @"\{(.*?)\}";
            rx = new Regex(sr, RegexOptions.Multiline);
            file = rx.Replace(file, @"<strong>$1</strong>");
            using (StreamWriter sw = File.CreateText(output))
                sw.Write(file);

            int i = 0;
            lines = File.ReadAllLines(output).ToArray();
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (String line in lines)
                {
                    String l = line;
                    if (l.Contains("#PROGRAMA#"))
                    {
                        l = "<strong>Programa ejemplo:</strong><br/>\n<pre>" + HttpUtility.HtmlEncode(lPrograms[i]) + "</pre>\n";
                        i++;
                    }
                    l = l.Trim();
                    sw.WriteLine(l);
                }
            }

            file = File.ReadAllText(output);
            using (StreamWriter sw = new StreamWriter(output))
            {
                sw.WriteLine(@"<html>
<head>
    <style>
    * {
	    margin: 0;
	    padding: 0;
	    font-family: Tahoma;
	    font-size: 10pt;
    }
    h1 { font-size: 16pt; }
    h2 { font-size: 12pt; }
    hr { margin: .5em 0;}
    body {
	    padding: 1em;
    }
    pre {
	    font-family: Consolas, 'Courier New';
        margin: 1em;
                border: 1px solid #000;
	    padding: .5em;
                background: #eee;
    }
    </style>
</head>
<body>");
                sw.WriteLine(file);
                sw.WriteLine("</body></html>");
            }
        }
    }
}
