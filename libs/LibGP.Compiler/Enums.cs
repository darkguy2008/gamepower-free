using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGP.Compiler
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
        End,
        EndElse
    }

    public enum ECompilerFlags
    {
        max_process = 1,
        extended_conditions,
        simple_conditions,
        case_sensitive,
        ignore_errors,
        free_sintax,
        no_strfix,
        no_optimization,
        no_range_check,
        no_id_check,
        no_null_check,
        no_check
    }

    public enum EResourceType
    {
        FPG = 1,
        FNT = 2,
        Song = 3,
        Audio = 4
    }    
}
