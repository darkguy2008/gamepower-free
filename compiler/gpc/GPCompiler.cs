using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace gpc
{
    public enum EKeyword
    {
        Program = 1,
        Global,
        Begin,
        End,
        Process,
        If,
        Else,
        Break,
        Frame,
        Loop,
        Repeat,
        Until,
        While,
        From,
        For,
        Local,
        Private,
        Const,
        Return,
    }

    public enum EKeywordState 
    {
        Begin = 1,
        Initialize,
        Condition,
        Continue,
        Break,
        Return,
        End
    }

    public class GPCmd
    {
        public bool IsKeyword = false;
        public EKeyword Keyword;
        public GPCmd Parent;
        public String Arguments;
        public String Name;
        public String Raw;
        public int Id;
        public List<GPCmd> Commands = new List<GPCmd>();
        public Dictionary<String, String> Variables = new Dictionary<String, String>();
        public Dictionary<String, String> InternalVariables = new Dictionary<String, String>();
    }

    public class JSLine
    {
        public int idControl;
        public EKeyword Keyword;
        public EKeywordState State;
        public String InnerCode;
        public String Output;

        public override string ToString()
        {
            return idControl + " | " + Keyword.ToString() + ", " + State.ToString() + " | " + Output + " (" + InnerCode + ")";
            //return base.ToString();
        }
    }

    public class GPCompiler
    {
        public String GPProcessVarName = "_gpp";
        public String GPEngineVarName = "_gp";
        public String ProgramProcess = String.Empty;
        public List<String> ProcessList = new List<String>();

        public String[] Keywords = new String[] {
            "if",
            "else",
            "program",
            "process",
            "global",
            "begin",
            "end",
            "loop",
            "break",
            "frame",
            "repeat",
            "until",
            "while",
            "from",
            "for",
            "local",
            "private",
            "const",
            "return"
        };
        public List<KeyValuePair<String, String>> LocalVars = new List<KeyValuePair<String, String>>();
        public List<KeyValuePair<String, String>> ConstVars = new List<KeyValuePair<String, String>>();
        public List<KeyValuePair<String, String>> GlobalVars = new List<KeyValuePair<String, String>>();
        public List<KeyValuePair<String, String>> GlobalLocalVars = new List<KeyValuePair<String, String>>();
        
        public enum EResourceType
        {
            FPG = 1,
            FNT = 2,
            Song = 3,
            Audio = 4
        }
        public Dictionary<String, EResourceType> Resources = new Dictionary<String, EResourceType>();

        public List<int> usedId = new List<int>();

        public GPCmd Compile(List<String> lines)
        {
            GPParser p = new GPParser();
            List<String> Cmd = p.Sanitize(lines);
            GPCmd Root = new GPCmd()
            {
                IsKeyword = false,
            };

            GPCmd curCmd = Root;
            GPCmd oldCmd = null;
            bool IsGlobals = false;
            bool IsLocals = false;
            bool IsConstants = false;
            bool IsAddingVariables = false;
            int iControl = 1;
            int iControlLast = 0;
            usedId = new List<int>();
            foreach (String sCommand in Cmd)
            {
                String raw = sCommand;
                String cmd = sCommand.ToLowerInvariant().Trim();
                if (cmd.EndsWith(";")) {
                    cmd = cmd.Substring(0, cmd.Length - 1);
                    raw = raw.Substring(0, raw.Length - 1); 
                }

                String sKeyword = cmd;
                if (sKeyword.Contains(' '))
                    sKeyword = sKeyword.Substring(0, sKeyword.IndexOf(' '));
                if (sKeyword.Contains('('))
                    sKeyword = sKeyword.Substring(0, sKeyword.IndexOf('('));

                GPCmd d2cmd = new GPCmd();
                String sArguments = String.Empty;
                String sName = raw.Substring(sKeyword.Length).Trim();

                if (raw.Contains('(') && raw.Contains(')'))
                {
                    sArguments = raw.Substring(raw.IndexOf('(') + 1);
                    sArguments = sArguments.Substring(0, sArguments.LastIndexOf(')'));
                    sName = sName.Substring(0, sName.IndexOf('('));
                }
                if (Keywords.Contains(sKeyword))
                {
                    switch (sKeyword)
                    {
                        case "program":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Program, Name = sName });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControl = 1;
                            break;
                        case "process":
                            // If curCmd == null - extra end without opening if/loop/etc.
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Process, Name = sName, Arguments = sArguments });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControl = 1;
                            break;
                        case "loop":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Loop, Id = iControl });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControlLast = iControl;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;

                        case "repeat":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Repeat, Id = iControl });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControlLast = iControl;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;
                        case "until":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Until, Id = iControlLast, Arguments = sArguments });
                            curCmd = curCmd.Parent;
                            //iControl--;
                            break;

                        case "while":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.While, Id = iControlLast, Arguments = sArguments });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControlLast = iControl;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;


                        case "from":
                            sArguments = raw.Substring(sKeyword.Length).Trim();
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.From, Id = iControl, Arguments = sArguments });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControlLast = iControl;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;

                        case "for":
                            sArguments = raw.Substring(sKeyword.Length).Trim();
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.For, Id = iControl, Arguments = sArguments });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControlLast = iControl;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;

                        case "if":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.If, Id = iControl, Arguments = sArguments });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            iControl++; while (usedId.Contains(iControl)) { iControl++; }
                            usedId.Add(iControl);
                            break;
                        case "else":
                            iControl-=2;
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd.Parent, IsKeyword = true, Keyword = EKeyword.Else, Id = iControl });
                            d2cmd = curCmd.Commands.Last();
                            oldCmd = curCmd;
                            curCmd = d2cmd;
                            //iControl++;
                            break;
                        case "end":
                            //iControl--;
                            curCmd = curCmd.Parent;
                            break;

                        case "global":
                            IsGlobals = true;
                            IsConstants = false;
                            IsLocals = false;
                            IsAddingVariables = true;
                            break;
                        case "const":
                            IsGlobals = false;
                            IsConstants = true;
                            IsLocals = false;
                            IsAddingVariables = true;
                            break;
                        case "local":
                            IsLocals = true;
                            IsGlobals = false;
                            IsConstants = false;
                            IsAddingVariables = true;
                            break;
                        case "private":
                            IsLocals = false;
                            IsGlobals = false;
                            IsConstants = false;
                            IsAddingVariables = true;
                            break;
                        case "begin":
                            IsLocals = false;
                            IsGlobals = false;
                            IsConstants = false;
                            IsAddingVariables = false;
                            break;

                        case "return":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Return, Id = 0, Arguments = sArguments });
                            break;
                        case "break":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Break, Id = iControlLast });
                            break;
                        case "frame":
                            curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = true, Keyword = EKeyword.Frame, Arguments = sArguments });
                            break;
                    }
                }
                else
                {
                    if (!IsAddingVariables)
                        curCmd.Commands.Add(new GPCmd() { Parent = curCmd, IsKeyword = false, Raw = raw.Trim() });
                    else
                    {
                        Char sc = ',';
                        List<KeyValuePair<String, String>> Vars = new List<KeyValuePair<String, String>>();

                        if (cmd.Contains("[")) {
                            sc = ';';
                            if(!cmd.Contains(";"))
                                cmd += ";";
                        }
                        else
                            if (!cmd.Contains(','))
                                cmd += ",";

                        foreach (String v in cmd.Split(new char[] { sc }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            String varName = v;
                            String varValue = "null";
                            if (v.IndexOf('=') > 0)
                            {
                                varName = v.Substring(0, v.IndexOf('=')).Trim();
                                varValue = v.Substring(v.IndexOf('=') + 1).Trim();
                                if (varName.Contains("["))
                                {
                                    varValue = varName.Substring(varName.IndexOf("[")) + " = " + varValue;
                                    varName = varName.Substring(0, varName.IndexOf("["));
                                }
                            }
                            if (IsGlobals)
                                GlobalVars.Add(new KeyValuePair<String, String>(varName, varValue));
                            else if(IsConstants)
                                ConstVars.Add(new KeyValuePair<String, String>(varName, varValue));
                            else if (IsLocals)
                                LocalVars.Add(new KeyValuePair<String, String>(varName, varValue));
                            else
                            {
                                curCmd.Variables.Add(varName, varValue);
                                if (curCmd.Keyword == EKeyword.Program)
                                    GlobalLocalVars.Add(new KeyValuePair<String, String>(varName, varValue));
                            }
                        }

                    }
                }
            }

            return Root;
        }

        public List<String> Convert(List<String> lines)
        {
            List<String> rvPre = new List<String>();
            List<String> rvTmp = new List<String>();
            List<String> rvPost = new List<String>();
            List<String> rv = new List<String>();
            GPCmd RootCmd = Compile(lines);
            Resources.Add("default.fnt", EResourceType.FNT);

            ProcessList = RootCmd.Commands.Select(x => x.Name.ToLowerInvariant().Trim()).ToList();

            // TODO: Process function calling this way!
            //     _gpp._code[11] = function() { var x = bind(menuCursor, menuCursor); x(150, 260, 504, 3, false); };

            foreach (GPCmd d2process in RootCmd.Commands)
            {
                d2process.InternalVariables.Add("file", "(typeof file === 'undefined') ? 0 : file");
                d2process.InternalVariables.Add("graph", "(typeof graph === 'undefined') ? 0 : graph");
                d2process.InternalVariables.Add("x", "(typeof x === 'undefined') ? 0 : x");
                d2process.InternalVariables.Add("y", "(typeof y === 'undefined') ? 0 : y");
                d2process.InternalVariables.Add("z", "(typeof z === 'undefined') ? 0 : z");
                d2process.InternalVariables.Add("id", GPEngineVarName + ".NewPID()");
                d2process.InternalVariables.Add("size", "(typeof size === 'undefined') ? 100 : size");
                d2process.InternalVariables.Add("angle", "(typeof angle === 'undefined') ? 0 : angle");
                d2process.InternalVariables.Add("father", "(typeof " + GPEngineVarName + " === 'undefined') ? 0 : " + GPEngineVarName + ".parentProcess");
                d2process.InternalVariables.Add("priority", "(typeof priority === 'undefined') ? 0 : priority");
                d2process.InternalVariables.Add("resolution", "(typeof resolution === 'undefined') ? 1 : resolution");
                d2process.InternalVariables.Add("son", "null");
                if (d2process.Keyword == EKeyword.Process)
                    d2process.InternalVariables.Add("type", "\"" + d2process.Name + "\"");
                d2process.InternalVariables.Add("ctype", "(typeof ctype === 'undefined') ? c_screen : ctype");

                if (d2process.Keyword != EKeyword.Program)
                    foreach (KeyValuePair<String, String> var in GlobalLocalVars)
                        d2process.Variables.Add(var.Key, var.Value);

                List<JSLine> PreJSLines = PreProcess(d2process.Commands);
                List<String> JSLines = ToJavascript(d2process, PreJSLines);

                List<String> PreOutput = new List<String>();
                List<String> PostOutput = new List<String>();

                PreOutput.Add("var " + d2process.Name + " = function(" + d2process.Arguments + ") {");
                PreOutput.Add("    var " + GPProcessVarName + " = new GPProcess();");

                if (d2process.Keyword == EKeyword.Program)
                    foreach(KeyValuePair<String, String> var in GlobalVars)
                        PreOutput.Add(ParseVariable("    " + GPEngineVarName + ".aGlobals.", var));

                foreach (KeyValuePair<String, String> var in d2process.InternalVariables)
                    PreOutput.Add(ParseVariable("    " + GPProcessVarName + ".", var));

                foreach (KeyValuePair<String, String> var in d2process.Variables)
                    PreOutput.Add("    " + GPProcessVarName + "." + var.Key + " = (typeof " + var.Key + " === 'undefined') ? " + var.Value + " : " + var.Key + ";");

                foreach (KeyValuePair<String, String> var in LocalVars)
                    PreOutput.Add("    " + GPProcessVarName + "." + var.Key + " = (typeof " + var.Key + " === 'undefined') ? " + var.Value + " : " + var.Key + ";");

                if (d2process.Keyword != EKeyword.Program)
                {
                    PreOutput.Add("    if (!(typeof " + GPEngineVarName + " === 'undefined')) {");
                    PreOutput.Add("        " + GPEngineVarName + ".parentProcess.son = " + GPProcessVarName + ";");
                    PreOutput.Add("        " + GPProcessVarName + "._fatherId = " + GPProcessVarName + ".father.id;");
                    PreOutput.Add("    }");
                }

                PostOutput.Add("    return " + GPEngineVarName + ".AddProcess(" + GPProcessVarName + ");");
                PostOutput.Add("}");
                PostOutput.Add("");

                rvTmp.AddRange(PreOutput);
                rvTmp.AddRange(JSLines);
                rvTmp.AddRange(PostOutput);
            }

            foreach (KeyValuePair<String, EResourceType> kvRes in Resources)
            {
                String type = kvRes.Value.ToString().ToLowerInvariant();
                String s = GPEngineVarName + ".res." + type + "['" + kvRes.Key + "'] = { status: 0, data: []";
                if (kvRes.Value == EResourceType.FNT)
                    s = s + ", _format: '" + kvRes.Value.ToString().ToLowerInvariant().Trim() + "'";
                s = s + " };";
                rvPre.Add(s);
            }
            rvPre.Add("");
            foreach (KeyValuePair<String, String> var in ConstVars)
                rvPre.Add(ParseVariable("", var));
            rvPre.Add("");

            rv.AddRange(rvPre);
            rv.AddRange(rvTmp);
            rv.AddRange(rvPost);

            ProgramProcess = RootCmd.Commands.Where(x => x.Keyword == EKeyword.Program).Single().Name;
            return rv;
        }

        public String ParseVariable(String prefix, KeyValuePair<String, String> kv)
        {
            if (kv.Value.StartsWith("["))
            {
                String val = kv.Value.Substring(kv.Value.IndexOf("=") + 1);

                if (val.ToLowerInvariant().Contains(" dup"))
                {
                    int times = int.Parse(val.Substring(0, val.ToLowerInvariant().IndexOf(" dup")));
                    String pattern = val.Substring(val.IndexOf('(') + 1);
                    pattern = pattern.Substring(0, pattern.IndexOf(')'));
                    val = String.Empty;
                    for (int i = 1; i <= times; i++)
                        val += pattern + ",";
                    val = val.Substring(0, val.Length - 1);
                }

                val = "[ " + val.Trim() + " ];";

                // prefix + kv.Key + kv.Value.Substring(0, kv.Value.IndexOf("=") + 1) + val;
                return prefix + kv.Key + " = " + val;
            }
            else
            {
                return prefix + kv.Key + " = " + kv.Value + ";";
            }
        }

        public List<JSLine> PreProcess(List<GPCmd> d2commands)
        {
            List<JSLine> rv = new List<JSLine>();

            foreach (GPCmd cmd in d2commands)
            {
                if (cmd.IsKeyword)
                {
                    String condition = String.Empty;
                    switch (cmd.Keyword)
                    {
                        case EKeyword.Loop:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Loop, State = EKeywordState.Begin });
                            rv.AddRange(PreProcess(cmd.Commands));
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Loop, State = EKeywordState.End });
                            break;
                        case EKeyword.Repeat:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Repeat, State = EKeywordState.Begin });
                            rv.AddRange(PreProcess(cmd.Commands));
                            break;
                        case EKeyword.Until:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Repeat, State = EKeywordState.End, InnerCode = cmd.Arguments });
                            break;
                        case EKeyword.While:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.While, State = EKeywordState.Begin });
                            rv.AddRange(PreProcess(cmd.Commands));
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.While, State = EKeywordState.End, InnerCode = cmd.Arguments });
                            break;
                        case EKeyword.From:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.From, State = EKeywordState.Initialize, InnerCode = cmd.Arguments });
                            rv.AddRange(PreProcess(cmd.Commands));
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.From, State = EKeywordState.Begin, InnerCode = cmd.Arguments });
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.From, State = EKeywordState.End });
                            break;
                        case EKeyword.For:
                            if (cmd.Arguments.StartsWith("("))
                                cmd.Arguments = cmd.Arguments.Substring(1);
                            if (cmd.Arguments.EndsWith(")"))
                                cmd.Arguments = cmd.Arguments.Substring(0, cmd.Arguments.Length - 1);
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.For, State = EKeywordState.Initialize, InnerCode = cmd.Arguments });
                            rv.AddRange(PreProcess(cmd.Commands));
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.For, State = EKeywordState.Begin, InnerCode = cmd.Arguments });
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.For, State = EKeywordState.End });
                            break;
                        case EKeyword.If:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.If, State = EKeywordState.Begin, InnerCode = cmd.Arguments });
                            rv.AddRange(PreProcess(cmd.Commands));
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.If, State = EKeywordState.End });
                            break;
                        case EKeyword.Else:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.If, State = EKeywordState.Condition });
                            rv.AddRange(PreProcess(cmd.Commands));
                            break;
                        case EKeyword.Return:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Return, InnerCode = cmd.Arguments });
                            break;
                        case EKeyword.Break:
                            rv.Add(new JSLine() { idControl = cmd.Id, Keyword = EKeyword.Break });
                            break;
                        case EKeyword.Frame:
                            rv.Add(new JSLine() { Output = GPProcessVarName + ".Frame(" + (cmd.Arguments.Length > 0 ? cmd.Arguments : "") + ");" });
                            break;
                        default:
                            rv.Add(new JSLine() { Output = cmd.Keyword.ToString() + cmd.Raw });
                            break;
                    }
                }
                else
                    if(cmd.Raw.Trim().Length > 0)
                        rv.Add(new JSLine() { Output = cmd.Raw });
            }

            return rv;
        }

        public List<String> ToJavascript(GPCmd cmd, List<JSLine> preJSLines)
        {
            List<String> rv = new List<String>();
            List<JSLine> wjs = new List<JSLine>();

            // Handle LOOPs
            preJSLines.Where(x => x.Keyword == EKeyword.Loop && x.State == EKeywordState.Begin).ToList().ForEach(x => x.Output = "/* LOOP */");
            preJSLines.Where(x => x.Keyword == EKeyword.Loop && x.State == EKeywordState.End)
                .ToList()
                .ForEach(x =>
                    x.Output = GPProcessVarName + "._instLocal = " + 
                    preJSLines.IndexOf(
                        preJSLines.Where(y => 
                            y.idControl == x.idControl && 
                            y.Keyword == x.Keyword && 
                            y.State == EKeywordState.Begin
                        ).Single()
                    )
                    + "; /* LOOP END */"
                );

            // Handle REPEATs
            preJSLines.Where(x => x.Keyword == EKeyword.Repeat && x.State == EKeywordState.Begin).ToList().ForEach(x => x.Output = "/* REPEAT */");
            preJSLines.Where(x => x.Keyword == EKeyword.Repeat && x.State == EKeywordState.End)
                .ToList()
                .ForEach(x =>
                    x.Output = x.Output = "if ( !(" + ParseCondition(x.InnerCode) + ") ) { " + GPProcessVarName + "._instLocal = " + 
                    preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == x.Keyword &&
                            y.State == EKeywordState.Begin
                        ).Single()
                    )
                    + " }; /* UNTIL */"
                );

            // Handle WHILEs
            preJSLines.Where(x => x.Keyword == EKeyword.While && x.State == EKeywordState.Begin).ToList().ForEach(x => x.Output = "/* WHILE */");
            preJSLines.Where(x => x.Keyword == EKeyword.While && x.State == EKeywordState.End)
                .ToList()
                .ForEach(x =>
                    x.Output = x.Output = "if ( (" + ParseCondition(x.InnerCode) + ") ) { " + GPProcessVarName + "._instLocal = " +
                    preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == x.Keyword &&
                            y.State == EKeywordState.Begin
                        ).Single()
                    )
                    + " }; /* WHILE END */"
                );

            // Handle FROMs
            preJSLines.Where(x => x.Keyword == EKeyword.From && x.State == EKeywordState.End)
                .ToList()
                .ForEach(x =>
                    x.Output = GPProcessVarName + "._instLocal = " +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == x.Keyword &&
                            y.State == EKeywordState.Initialize
                        ).Single()
                    ) + 1
                    ) + "; /* FROM END */"
                );
            preJSLines.Where(x => x.Keyword == EKeyword.From && x.State == EKeywordState.Initialize)
                .ToList()
                .ForEach(x => 
                    x.Output = x.InnerCode.Substring(0, x.InnerCode.ToUpperInvariant().IndexOf(" TO"))
                );
            preJSLines.Where(x => x.Keyword == EKeyword.From && x.State == EKeywordState.Begin)
                .ToList()
                .ForEach(x =>
                    x.Output = ParseFrom(x.InnerCode) +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == EKeyword.From &&
                            y.State == EKeywordState.End
                        ).Single()
                    ) + 1
                    ) + " }"
                );

            // Handle FORs
            preJSLines.Where(x => x.Keyword == EKeyword.For && x.State == EKeywordState.End)
                .ToList()
                .ForEach(x =>
                    x.Output = GPProcessVarName + "._instLocal = " +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == x.Keyword &&
                            y.State == EKeywordState.Initialize
                        ).Single()
                    ) + 1
                    ) + "; /* FOR END */"
                );
            preJSLines.Where(x => x.Keyword == EKeyword.For && x.State == EKeywordState.Initialize)
                .ToList()
                .ForEach(x =>
                    x.Output = x.InnerCode.Substring(0, x.InnerCode.ToUpperInvariant().IndexOf(";"))
                );
            preJSLines.Where(x => x.Keyword == EKeyword.For && x.State == EKeywordState.Begin)
                .ToList()
                .ForEach(x =>
                    x.Output = ParseFor(x.InnerCode) +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == EKeyword.For &&
                            y.State == EKeywordState.End
                        ).Single()
                    ) + 1
                    ) + " }"
                );

            // Handle RETURNs
            preJSLines.Where(x => x.Keyword == EKeyword.Return).ToList().ForEach(x =>
                x.Output = GPProcessVarName + "._instLocal = -1; return " + x.InnerCode + ";"
            );

            // Handle BREAKs
            preJSLines.Where(x => x.Keyword == EKeyword.Break).ToList().ForEach(x =>
                x.Output = GPProcessVarName + "._instLocal = " +
                (
                    preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            (y.Keyword == EKeyword.For ||
                             y.Keyword == EKeyword.From ||
                             y.Keyword == EKeyword.Loop ||
                             y.Keyword == EKeyword.Repeat ||
                             y.Keyword == EKeyword.While) &&
                            y.State == EKeywordState.End
                        ).Single()
                    ) + 1
                ) + ";"
            );

            // Handle IFs
            // TODO: Implement different logic, for ELSEs and such
            preJSLines.Where(x => x.Keyword == EKeyword.If && x.State == EKeywordState.End).ToList().ForEach(x => x.Output = "/* ENDIF */");
            preJSLines.Where(x => x.Keyword == EKeyword.If && x.State == EKeywordState.Condition).ToList().ForEach(x => x.Output = "/* IF -> ELSE */");
            preJSLines.Where(x => x.Keyword == EKeyword.If && x.State == EKeywordState.Condition)
                .ToList()
                .ForEach(x =>
                    x.Output = "/* ELSE */ " + GPProcessVarName + "._instLocal = " +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y =>
                            y.idControl == x.idControl &&
                            y.Keyword == EKeyword.If &&
                            y.State == EKeywordState.End
                        ).First()
                    ) + 1
                    ) + ""
                );
            preJSLines.Where(x => x.Keyword == EKeyword.If && x.State == EKeywordState.Begin)
                .ToList()
                .ForEach(x =>
                    x.Output = "if ( !(" + ParseCondition(x.InnerCode) + ") ) { " + GPProcessVarName + "._instLocal = " +
                    (preJSLines.IndexOf(
                        preJSLines.Where(y => 
                            y.idControl == x.idControl &&
                            y.Keyword == EKeyword.If &&
                            (y.State == EKeywordState.Condition || y.State == EKeywordState.End)
                        ).First()
                    ) + 1
                    ) + " }"
                );

            ////////////////////////////////////////////////////////////
            // CONVERT
            ////////////////////////////////////////////////////////////

            if(GlobalVars.Count > 0)
                preJSLines.ForEach(x => x.Output = FindVariables(x.Output, GPEngineVarName + ".aGlobals.", GlobalVars.Select(y => y.Key).OrderBy(y => y).ToList()));
            if (cmd.Variables.Count > 0)
                preJSLines.ForEach(x => x.Output = FindVariables(x.Output, GPProcessVarName + ".", cmd.Variables.Keys.OrderBy(y => y).ToList()));
            if (cmd.InternalVariables.Count > 0)
                preJSLines.ForEach(x => x.Output = FindVariables(x.Output, GPProcessVarName + ".", cmd.InternalVariables.Keys.OrderBy(y => y).ToList()));
            if (LocalVars.Count > 0)
                preJSLines.ForEach(x => x.Output = FindVariables(x.Output, GPProcessVarName + ".", LocalVars.Select(y => y.Key).OrderBy(y => y).ToList()));

            if (!String.IsNullOrEmpty(cmd.Arguments))
                foreach (String v in cmd.Arguments.Split(','))
                {
                    String arg = v.Trim();
                    if (GlobalVars.Where(x => x.Key == arg).Count() == 0)
                        if (GlobalLocalVars.Where(x => x.Key == arg).Count() == 0)
                            if (cmd.Variables.Where(x => x.Key == arg).Count() == 0)
                                if (cmd.InternalVariables.Where(x => x.Key == arg).Count() == 0)
                                    cmd.InternalVariables.Add(arg, "(typeof " + arg + " === 'undefined') ? null : " + arg);
                }


            foreach (JSLine l in preJSLines)
                if (l.Keyword == EKeyword.For)
                    if (l.State == EKeywordState.Initialize)
                        if (!l.Output.ToLowerInvariant().Trim().StartsWith("_gpp"))
                            if (!l.Output.ToLowerInvariant().Trim().StartsWith("var"))
                                cmd.InternalVariables.Add(l.Output.Trim().Substring(0, l.Output.IndexOf('=')).Trim(), l.Output.Substring(l.Output.IndexOf("=") + 1));

            if (cmd.InternalVariables.Count > 0)
                preJSLines.ForEach(x => x.Output = FindVariables(x.Output, GPProcessVarName + ".", cmd.InternalVariables.Keys.OrderBy(y => y).ToList()));
            
            preJSLines.ForEach(x => x.Output = Sanitize(x.Output));
            preJSLines.ForEach(x => x.Output = FindCode(x.Output, SanitizeFunctions));
            preJSLines.ForEach(x => x.Output = FindResources(x.Output));

            int i = 0;
            foreach (String o in preJSLines.Select(x => x.Output).ToList())
            {
                string finalLine = o ?? "";
                if (!finalLine.EndsWith(";") && !finalLine.EndsWith("/"))
                    finalLine += ";";

                foreach (String p in ProcessList)
                    if (finalLine.ToLowerInvariant().Trim().Contains(p + "(") || finalLine.ToLowerInvariant().Trim().Contains(p + " ("))
                    {
                        finalLine = GPEngineVarName + ".SetParent(_gpp); " + finalLine;
                        break;
                    }

                foreach (String fn in new String[] {
                    "let_me_alone"
                })
                    if (finalLine.ToLowerInvariant().Trim().Contains(fn + "(") || finalLine.ToLowerInvariant().Trim().Contains(fn + " ("))
                    {
                        finalLine = GPEngineVarName + ".SetCaller(_gpp); " + finalLine;
                        break;
                    }

                rv.Add("    " + GPProcessVarName + "._code[" + i + "] = function() { " + finalLine + " };");
                i++;
            }

            return rv;
        }

        // TODO: Refactor, maybe.
        public String Sanitize(String line)
        {
            String rv = line;
            if (line.ToLowerInvariant().Trim().Contains("load_fpg(\"")) { rv = line.Replace("\\", "/"); }
            if (line.ToLowerInvariant().Trim().Contains("load_fnt(\"")) { rv = line.Replace("\\", "/"); }
            if (line.ToLowerInvariant().Trim().Contains("load_song(\"")) { rv = line.Replace("\\", "/"); }
            if (line.ToLowerInvariant().Trim().Contains("load_wav(\"")) { rv = line.Replace("\\", "/"); }
            if (line.ToLowerInvariant().Trim().Contains("load_pcm(\"")) { rv = line.Replace("\\", "/"); }
            return rv;
        }

        public String SanitizeFunctions(String line)
        {
            String rv = line;

            string strRegex = @"(?<![\b.])collision[\( ]*\s*(TYPE)\s*(\w*)\s*[\)](?!\.)";
            Regex rx = new Regex(strRegex, RegexOptions.IgnoreCase);
            string strReplace = @"collision(_gpp, ""$2"")";
            rv = rx.Replace(rv, strReplace);

            rv = rv.Replace("advance(", "advance(_gpp,", StringComparison.InvariantCultureIgnoreCase);
            rv = rv.Replace("xadvance(", "xadvance(_gpp,", StringComparison.InvariantCultureIgnoreCase);
            rv = rv.Replace("offset ", "", StringComparison.InvariantCultureIgnoreCase);
            
            // TODO: TEST THIS CASE
            strRegex = @"(&)(\w)";
            strReplace = @"$2";
            rv = rx.Replace(rv, strReplace);

            rv = rv.Replace(" mod ", " % ", StringComparison.InvariantCultureIgnoreCase);

            // Replace last period with a comma and put the variable name between quotes
            // Ex: write_int(0,0,0,0,_gp.aGlobals.blah) -> write_int(0,0,0,0,_gp.aGlobals,"blah")
            // TODO: Stop using StartsWith, what if there's a variable assignment on the left-hand side?
            if (rv.ToLowerInvariant().StartsWith("write_int"))
            {
                rv = rv.Replace("&", "", StringComparison.InvariantCultureIgnoreCase);

                strRegex = @"(.)([^.]*)\)$";
                rx = new Regex(strRegex, RegexOptions.IgnoreCase);
                strReplace = @",""$2"")";
                rv = rx.Replace(rv, strReplace);
            }

            // Replace scroll.Whatever by scroll[0].Whatever since scroll in JS is an array
            strRegex = @"(?<![\b._\w])scroll\.\b(?!\.)";
            rx = new Regex(strRegex, RegexOptions.IgnoreCase);
            strReplace = @"scroll[0].";
            rv = rx.Replace(rv, strReplace);

            return rv;
        }

        // Add resources to global resources array
        public String FindResources(String line)
        {
            String rv = line;
            String filename;

            if (rv.ToLowerInvariant().Contains("load_fpg"))
            {
                filename = rv.Substring(rv.IndexOf("\"") + 1);
                filename = filename.Substring(0, filename.LastIndexOf("\""));
                Resources.Add(filename, EResourceType.FPG);
                return rv;
            }

            if (rv.ToLowerInvariant().Contains("load_fnt"))
            {
                filename = rv.Substring(rv.IndexOf("\"") + 1);
                filename = filename.Substring(0, filename.LastIndexOf("\""));
                Resources.Add(filename, EResourceType.FNT);
                return rv;
            }

            if (rv.ToLowerInvariant().Contains("load_song"))
            {
                filename = rv.Substring(rv.IndexOf("\"") + 1);
                filename = filename.Substring(0, filename.LastIndexOf("\""));
                Resources.Add(filename, EResourceType.Song);
                return rv;
            }

            if (rv.ToLowerInvariant().Contains("load_wav") || rv.ToLowerInvariant().Contains("load_pcm"))
            {
                filename = rv.Substring(rv.IndexOf("\"") + 1);
                filename = filename.Substring(0, filename.LastIndexOf("\""));
                Resources.Add(filename, EResourceType.Audio);
                return rv;
            }

            return rv;
        }

        public String ParseCondition(String cnd)
        {
            String rv = cnd;
            rv = rv.Replace(" or ", " || ", StringComparison.InvariantCultureIgnoreCase);
            rv = rv.Replace(" and ", " && ", StringComparison.InvariantCultureIgnoreCase);
            rv = rv.Replace("not ", "! ", StringComparison.InvariantCultureIgnoreCase);
            rv = SanitizeFunctions(rv);
            return rv;
        }

        public String ParseFrom(String cnd)
        {
            String rv = cnd;
            bool defaultStep = true;
            int step = 1;
            if (cnd.ToLowerInvariant().Contains(" step ")) {
                defaultStep = false;
                step = int.Parse(cnd.Substring(cnd.ToLowerInvariant().IndexOf(" step ") + " step ".Length));
            }

            String tmp = cnd.Substring(cnd.IndexOf('=') + 1);
            tmp = tmp.Substring(0, tmp.IndexOf(' '));
            int left = int.Parse(tmp);

            tmp = cnd.Substring(cnd.ToLowerInvariant().IndexOf(" to ") + " to ".Length);
            if (tmp.IndexOf(' ') > 0)
                tmp = tmp.Substring(0, tmp.IndexOf(' '));
            int right = int.Parse(tmp);

            String variable = cnd.Substring(0, cnd.IndexOf('='));

            // TODO: Check limits
            if (left > right)
            {
                if (defaultStep)
                    step = -1;
                else
                {
                    if (step > -1)
                        throw new Exception("Step > -1");
                }
                //rv = "if ( " + variable + " >= " + right + " ) { " + variable + "=" + variable + "-" + step + " } else { " + GPProcessVarName + "._instLocal = ";
                rv = variable + " = " + variable + " - " + step + "; if ( " + variable + " < " + right + " ) { " + GPProcessVarName + "._instLocal = ";
            }
            else
            {
                if (defaultStep)
                    step = 1;
                else
                {
                    if (step < 1)
                        throw new Exception("Step < 1");
                }
                //rv = "if ( " + variable + " <= " + right + " ) { " + variable + "=" + variable + "+" + step + " } else { " + GPProcessVarName + "._instLocal = ";
                rv = variable + " = " + variable + " + " + step + "; if ( " + variable + " > " + right + " ) { " + GPProcessVarName + "._instLocal = ";
            }

            return rv;
        }

        public String ParseFor(String cnd)
        {
            return cnd.Split(';')[2] + "; if ( !( " + cnd.Split(';')[1] + " ) ) { " + GPProcessVarName + "._instLocal = ";
        }

        public String FindVariables(String line, String prefix, List<String> Variables)
        {
            String rv = String.Empty;            
            String buf = String.Empty;
            Char prevChar = '\0';
            bool isString = false;
            //string strRegex = @"(?<![\b._\w\(])" + var + @"\b(?!\.)";
            String sRegex = @"(?<![\b._\w]){0}[.]??\b(?!\.)";

            foreach (Char c in line)
            {
                buf += c;

                if (c == '"')
                    if (prevChar != '\\')
                    {
                        isString = !isString;
                        if (isString)
                        {
                            foreach (String var in Variables)
                            {
                                String strRegex = String.Format(sRegex, var);
                                Regex myRegex = new Regex(strRegex, RegexOptions.None);
                                string strReplace = prefix + "$&";
                                buf = myRegex.Replace(buf, strReplace);
                            }
                            rv += buf;
                            buf = String.Empty;
                        }
                        else
                        {
                            rv += buf;
                            buf = String.Empty;
                        }
                    }

                prevChar = c;
            }

            foreach (String var in Variables)
            {
                String strRegex = String.Format(sRegex, var);
                Regex myRegex = new Regex(strRegex, RegexOptions.None);
                string strReplace = prefix + "$&";
                buf = myRegex.Replace(buf, strReplace);
            }
            rv += buf;
            return rv;
        }


        public String FindCode(String line, Func<String, String> fn)
        {
            String rv = String.Empty;
            String buf = String.Empty;
            Char prevChar = '\0';
            bool isString = false;

            foreach (Char c in line)
            {
                buf += c;

                if (c == '"')
                    if (prevChar != '\\')
                    {
                        isString = !isString;
                        if (isString)
                        {
                            buf = fn(buf);
                            rv += buf;
                            buf = String.Empty;
                        }
                        else
                        {
                            rv += buf;
                            buf = String.Empty;
                        }
                    }

                prevChar = c;
            }

            buf = fn(buf);
            rv += buf;
            return rv;
        }
    }

}
