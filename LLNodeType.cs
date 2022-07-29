namespace Grimoire
{
    internal enum LLNodeType
    {
        Initial = 0,
        NonTerminal = 1,
        EndNonTerminal = 2,
        Terminal = 3,
        Error = 4,
        EndDocument = 5
    }
}