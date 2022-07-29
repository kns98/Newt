namespace Grimoire
{
    internal interface ISymbolResolver
    {
        string GetSymbolById(int symbolId);
        int GetSymbolId(string symbol);
    }
}