using System;
using System.Collections.Generic;
using System.Linq;

namespace gpc
{
    public class GPParser
    {
        public List<String> Sanitize(List<String> lines, int pass = 0)
        {
            List<String> validLines = (from x in lines
                                       where 
                                            x.Length > 0 &&
                                            !x.Trim().StartsWith("//")
                                       select x).ToList();

            List<String> Cmd = new List<String>();
            bool isString = false;
            bool isArray = false;

            int iParenthesis = 0;
            String sCmd = String.Empty;
            foreach (String validLine in validLines)
            {
                int i = 0;
                bool bDoubleSpace = false;
                bool bAdd = false;
                bool bHasParenthesis = false;
                bool bNewBlockAtEnd = false;
                Char prevChar = '\0';
                foreach (Char c in validLine)
                {
                    sCmd += c;
                    if (c == '"')
                        if(prevChar != '\\')
                            isString = !isString;

                    if (c == ' ' && !isString)
                        if (!bDoubleSpace)
                            bDoubleSpace = true;
                        else
                            sCmd = sCmd.Substring(0, sCmd.Length - 1);

                    if (c != ' ')
                        if (bDoubleSpace)
                            bDoubleSpace = false;

                    if (sCmd.ToLowerInvariant().Trim().Contains("["))
                        isArray = true;

                    if ((c == ';' && !isString) || i == validLine.Length - 1)
                        bAdd = true;

                    if (c == ';' && !isString && isArray)
                        isArray = false;

                    if (i == validLine.Length - 1 && isArray)
                        bAdd = false;

                    if (c == '(')
                        iParenthesis++;
                    if (c == ')')
                        iParenthesis--;

                    if (iParenthesis > 0)
                    {
                        bHasParenthesis = true;
                        if (sCmd.ToLowerInvariant().Trim().StartsWith("if"))
                            bNewBlockAtEnd = true;
                        if (sCmd.ToLowerInvariant().Trim().StartsWith("while"))
                            bNewBlockAtEnd = true;
                    }
                    if (bHasParenthesis)
                    {
                        if (iParenthesis <= 0)
                        {
                            bHasParenthesis = false;
                            if (bNewBlockAtEnd)
                            {
                                bAdd = true;
                            }
                        }
                    }

                    if (sCmd.ToLowerInvariant().Trim().StartsWith("loop"))
                        bAdd = true;

                    if (sCmd.ToLowerInvariant().Trim().StartsWith("else"))
                        bAdd = true;

                    if (bAdd)
                    {
                        if (iParenthesis <= 0)
                        {
                            Cmd.Add(sCmd.Trim());
                            bAdd = false;
                            sCmd = String.Empty;
                            bNewBlockAtEnd = false;
                        }
                    }
                    
                    i++;
                    prevChar = c;
                }                
            }

            List<String> rv = Cmd.Where(x => !x.StartsWith("//")).ToList();

            if (pass == 0)
            {
                for (int i = 0; i < rv.Count; i++)
                    if (rv[i].ToLowerInvariant().Trim().StartsWith("else") && rv[i+1].ToLowerInvariant().Trim().StartsWith("if"))
                        rv[i] = "end";
                rv = Sanitize(rv, 1);
            }

            return rv;
        }

    }
}
