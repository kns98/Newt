using System;
using Grimoire;

internal class ExprParser : TableDrivenLL1Parser
{
    public const int EOS = 11;
    public const int ERROR = 12;
    public const int expr = 0;
    public const int term = 1;
    public const int factor = 2;
    public const int lparen = 5;
    public const int rparen = 6;
    public const int @int = 7;
    public const int add = 8;
    public const int mul = 9;

    private static readonly string[] _Symbols =
    {
        "expr", "term", "factor", "expr`", "term`", "lparen", "rparen", "int", "add", "mul", "whitespace", "#EOS",
        "#ERROR"
    };

    private static readonly (int Left, int[] Right)[][] _ParseTable =
    {
        new (int Left, int[] Right)[]
        {
            (0, new[] { 1, 3 }), (-1, null), (0, new[] { 1, 3 }), (-1, null), (-1, null), (-1, null), (-1, null)
        },
        new (int Left, int[] Right)[]
        {
            (1, new[] { 2, 4 }), (-1, null), (1, new[] { 2, 4 }), (-1, null), (-1, null), (-1, null), (-1, null)
        },
        new (int Left, int[] Right)[]
        {
            (2, new[] { 5, 0, 6 }), (-1, null), (2, new[] { 7 }), (-1, null), (-1, null), (-1, null), (-1, null)
        },
        new (int Left, int[] Right)[]
        {
            (-1, null), (3, new int[] { }), (-1, null), (3, new[] { 8, 1, 3 }), (-1, null), (-1, null),
            (3, new int[] { })
        },
        new (int Left, int[] Right)[]
        {
            (-1, null), (4, new int[] { }), (-1, null), (4, new int[] { }), (4, new[] { 9, 2, 4 }), (-1, null),
            (4, new int[] { })
        }
    };

    private static readonly int[] _SubstitutionsAndHiddenTerminals = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -2, 11, 12, -1 };

    private static readonly (int SymbolId, bool IsNonTerminal, int NonTerminalCount) _StartingConfiguration =
        (0, true, 5);

    private static readonly string[] _BlockEnds =
    {
        null, null, null, null, null, null, null, null, null, null, null, null, null
    };

    private static readonly Type[] _Types =
    {
        null, null, null, null, null, null, null, typeof(int), null, null, null, null, null
    };

    private static readonly int[] _CollapsedNonTerminals =
    {
        -1, -1, -1, -3, -3, -1, -1, -1, -1, -1, -1, -1, -1
    };

    private static readonly (int Accept, ((char First, char Last)[] Ranges, int Destination)[] Transitions, int[]
        PossibleAccepts)[] _LexTable =
        {
            (-1, new ((char First, char Last)[] Ranges, int Destination)[]
            {
                (new (char First, char Last)[]
                {
                    ((char)43, (char)43), ((char)45, (char)45)
                }, 1),
                (new (char First, char Last)[]
                {
                    ((char)42, (char)42), ((char)47, (char)47)
                }, 2),
                (new (char First, char Last)[]
                {
                    ((char)40, (char)40)
                }, 3),
                (new (char First, char Last)[]
                {
                    ((char)41, (char)41)
                }, 4),
                (new (char First, char Last)[]
                {
                    ((char)48, (char)57)
                }, 5),
                (new (char First, char Last)[]
                {
                    ((char)9, (char)10), ((char)13, (char)13), ((char)32, (char)32)
                }, 6)
            }, new[] { 8, 9, 5, 6, 7, 10 }),
            (8, new ((char First, char Last)[] Ranges, int Destination)[]
            {
            }, new[] { 8 }),
            (9, new ((char First, char Last)[] Ranges, int Destination)[]
            {
            }, new[] { 9 }),
            (5, new ((char First, char Last)[] Ranges, int Destination)[]
            {
            }, new[] { 5 }),
            (6, new ((char First, char Last)[] Ranges, int Destination)[]
            {
            }, new[] { 6 }),
            (7, new ((char First, char Last)[] Ranges, int Destination)[]
            {
                (new (char First, char Last)[]
                {
                    ((char)48, (char)57)
                }, 5)
            }, new[] { 7 }),
            (10, new ((char First, char Last)[] Ranges, int Destination)[]
            {
                (new (char First, char Last)[]
                {
                    ((char)9, (char)10), ((char)13, (char)13), ((char)32, (char)32)
                }, 6)
            }, new[] { 10 })
        };

    public ExprParser(ParseContext parseContext = null) : base(_ParseTable, _StartingConfiguration, _LexTable, _Symbols,
        _SubstitutionsAndHiddenTerminals, _BlockEnds, _CollapsedNonTerminals, _Types, parseContext)
    {
    }
}