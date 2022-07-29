namespace Grimoire
{
    internal abstract class EbnfUnaryExpression : EbnfExpression
    {
        public EbnfExpression Expression { get; set; }
    }
}