namespace Grimoire
{
    internal abstract class EbnfBinaryExpression : EbnfExpression
    {
        public EbnfExpression Left { get; set; }
        public EbnfExpression Right { get; set; }
    }
}