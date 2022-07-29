using System;
using System.Collections.Generic;

namespace Grimoire
{
    internal class EbnfLiteralExpression : EbnfExpression, IEquatable<EbnfLiteralExpression>, ICloneable
    {
        public EbnfLiteralExpression()
        {
        }

        public EbnfLiteralExpression(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
        public override bool IsTerminal => true;

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool Equals(EbnfLiteralExpression rhs)
        {
            if (ReferenceEquals(rhs, this)) return true;
            if (ReferenceEquals(rhs, null)) return false;
            return Equals(Value, rhs.Value);
        }

        public override IList<IList<string>> ToDisjunctions(EbnfDocument parent, Cfg cfg)
        {
            foreach (var prod in parent.Productions)
                if (Equals(prod.Value.Expression, this))
                {
                    var l = new List<IList<string>>();
                    var ll = new List<string>();
                    l.Add(ll);
                    ll.Add(prod.Key);
                    return l;
                }

            throw new InvalidOperationException("The terminal was not declared.");
        }

        public EbnfLiteralExpression Clone()
        {
            var result = new EbnfLiteralExpression(Value);
            result.SetPositionInfo(Line, Column, Position);
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EbnfLiteralExpression);
        }

        public override int GetHashCode()
        {
            if (null != Value) return Value.GetHashCode();
            return 0;
        }

        public static bool operator ==(EbnfLiteralExpression lhs, EbnfLiteralExpression rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(EbnfLiteralExpression lhs, EbnfLiteralExpression rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return false;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return true;
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Concat("\"", Value.Replace("\"", "\\\""), "\"");
        }
    }
}