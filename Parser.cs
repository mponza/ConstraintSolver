using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA
{
    // Dsl      ::= stats sat notSat
    // Stats    ::= var "= {" vals "}" stats | ε
    // Vals     ::= val seqVals | ε
    // SeqVals  ::= "," val seqVals | ε
    // Sat      ::= "{" cons "}" | ε
    // NotSat   ::= "!{" cons "}" | ε
    // Cons     ::=  pair seqCons | ε
    // SeqCons  ::= "," pair seqCons | ε
    // Pair     ::= "(" val "," val ")"
    // Var      ::= identifier
    // Val      ::= string
    using TokenStream = System.Collections.Generic.IEnumerator<Token>;
    class Parser
    {
        private DSL myDsl;
        private TokenStream ts;
        public Parser(TokenStream ts) { this.myDsl = new DSL(); this.ts = ts; }
        // Dsl ::= stats sat notSat
        void Dsl(TokenStream ts)
        {
            Stats(ts); Sat(); NotSat();
        }
        // Stats ::= var "= {" vals "}" stats |
        void Stats(TokenStream ts)
        {
            if (ts.Current.kind != Kind.VARIABLE) return;
            Var(); Expect(Kind.ASSIGN); Expect(Kind.LEFT_BRACE); Vals(); Expect(Kind.RIGHT_BRACE); Stats(ts);
        }
        // Vals ::= val seqVals | ε
        void Vals()
        {
            Val();
            SeqVals();
            return;
        }
        // SeqVals ::= "," val seqVals | ε
        void SeqVals()
        {
            if (ts.Current.kind != Kind.COMMA) return;
            ts.MoveNext();
            Val();
            SeqVals();
        }
        // Sat ::= "{" cons "}" | ε
        void Sat()
        {
            if (ts.Current.kind != Kind.LEFT_BRACE) return;
            ts.MoveNext();
            Cons(myDsl.satisfy);
            Expect(Kind.RIGHT_BRACE);
        }
        // NotSat ::= "!{" cons "}" | ε
        void NotSat()
        {
            if (ts.Current.kind != Kind.EXCLAMATION) return;
            ts.MoveNext();
            Expect(Kind.LEFT_BRACE);
            Cons(myDsl.notSatisfy);
            Expect(Kind.RIGHT_BRACE);
        }
        // Cons ::=  pair seqCons | ε
        void Cons(Relation r)
        {
            Pair(r); SeqCons(r);
        }
        // SeqCons ::= "," pair seqCons | ε
        void SeqCons(Relation r)
        {
            if (ts.Current.kind != Kind.COMMA) return;
            ts.MoveNext();
            Pair(r);
            SeqCons(r);
        }
        // Pair ::= "(" val "," val ")"
        void Pair(Relation r)
        {
            if (ts.Current.kind != Kind.LEFT_PARENT) return;
            Expect(Kind.LEFT_PARENT);
            string x = ts.Current.value;
            Expect(Kind.VALUE);
            Expect(Kind.COMMA);
            string y = ts.Current.value;
            Expect(Kind.VALUE);
            Expect(Kind.RIGHT_PARENT);
            r.AddPair(x, y);
        }
        // Var ::= identifier
        void Var()
        {
            if (ts.Current.kind != Kind.VARIABLE) throw new ApplicationException("Expected " + Kind.VARIABLE + " instead of " + ts.Current.kind);
            myDsl.statements.vars.Add(new Variable(ts.Current.value));
            ts.MoveNext();
        }
        // Val ::= string
        void Val()
        {
            if (ts.Current.kind != Kind.VALUE) return;
            myDsl.statements.vars[myDsl.statements.vars.Count - 1].values.Add(ts.Current.value);
            ts.MoveNext();
        }
        void Expect(Kind kind)
        {
            if (ts.Current.kind != kind) throw new ApplicationException("Expected " + kind + " instead of " + ts.Current.kind);
            ts.MoveNext();
        }
        public DSL Parse()
        {
            ts.MoveNext();
            Dsl(ts);
            if (ts.Current.kind != Kind.EOF) throw new ApplicationException("Expected end of file instead of " + ts.Current.kind);
            return myDsl;
        }
    }
}
