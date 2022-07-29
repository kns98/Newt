using System;
using System.Collections.Generic;

namespace Grimoire
{
    internal class EbnfRefExpression : EbnfExpression, IEquatable<EbnfRefExpression>, ICloneable
    {
        public EbnfRefExpression(string symbol)
        {
            Symbol = symbol;
        }

        public EbnfRefExpression()
        {
        }

        public override bool IsTerminal => false;

        /// <summary>
        ///     Indicates the referenced symbol
        /// </summary>
        public string Symbol { get; set; }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool Equals(EbnfRefExpression rhs)
        {
            if (ReferenceEquals(rhs, this)) return true;
            if (ReferenceEquals(rhs, null)) return false;
            return Equals(Symbol, rhs.Symbol);
        }

        public override IList<IList<string>> ToDisjunctions(EbnfDocument parent, Cfg cfg)
        {
            if (string.IsNullOrEmpty(Symbol))
                throw new InvalidOperationException("The ref expression was nil.");
            var l = new List<IList<string>>();
            var ll = new List<string>();
            l.Add(ll);
            ll.Add(Symbol);
            return l;
        }

        public EbnfRefExpression Clone()
        {
            var result = new EbnfRefExpression(Symbol);
            result.SetPositionInfo(Line, Column, Position);
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EbnfRefExpression);
        }

        public override int GetHashCode()
        {
            if (null != Symbol) return Symbol.GetHashCode();
            return 0;
        }

        public static bool operator ==(EbnfRefExpression lhs, EbnfRefExpression rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(EbnfRefExpression lhs, EbnfRefExpression rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return false;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return true;
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return null != Symbol ? Symbol : "";
        }
    }
}