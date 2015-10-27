using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA
{
    public enum Kind { VARIABLE, ASSIGN, VALUE, COMMA, LEFT_PARENT, RIGHT_PARENT, LEFT_BRACE, RIGHT_BRACE, EXCLAMATION, EOF }
    class Token
    {
        public readonly string value;
        public readonly Kind kind;
        public Token(Kind kind) : this(null, kind) { }
        public Token(string value, Kind kind) { this.kind = kind; this.value = value; }
    }
}
