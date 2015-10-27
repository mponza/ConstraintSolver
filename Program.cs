using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace FinalPA
{
    using TokenStream = System.Collections.Generic.IEnumerator<Token>;
    class Program
    {
        static void Main(string[] args)
        {
            string str = "a = {a1, a2, a3} b = {b1, b2, b3} c = {c1, c2, c3} d = {d1, d2, d3} e = {e1, e2, e3} f = {f1, f2, f3} g = {g1, g2, g3} h = {h1, h2, h3} i = {i1, i2, i3} !{ (a1, b1), (a1, c1), (a1, d1), (a1, g1), (a2, b2), (a2, c2), (a2, d2), (a2, g2), (a3, b3), (a3, c3), (a3, d3), (a3, g3), (b1, a1), (b1, c1), (b1, e1), (b1, h1), (b2, a2), (b2, c2), (b2, e2), (b2, h2), (b3, a3), (b3, c3), (b3, e3), (b3, h3), (c1, a1), (c1, b1), (c1, f1), (c1, i1), (c2, a2), (c2, b2), (c2, f2), (c2, i2), (c3, a3), (c3, b3), (c3, f3), (c3, i3), (d1, a1), (d1, g1), (d1, e1), (d1, f1), (d2, a2), (d2, g2), (d2, e2), (d2, f2), (d3, a3), (d3, g3), (d3, e3), (d3, f3), (e1, d1), (e1, f1), (e1, b1), (e1, h1), (e2, d2), (e2, f2), (e2, b2), (e2, h2), (e3, d3), (e3, f3), (e3, b3), (e3, h3), (f1, c1), (f1, e1), (f1, i1), (f1, d1), (f2, c2), (f2, e2), (f2, i2), (f2, d2), (f3, c3), (f3, e3), (f3, i3), (f3, d3), (g1, d1), (g1, a1), (g1, h1), (g1, i1), (g2, d2), (g2, a2), (g2, h2), (g2, i2), (g3, d3), (g3, a3), (g3, h3), (g3, i3), (h1, g1), (h1, i1), (h1, e1), (h1, b1), (h2, g2), (h2, i2), (h2, e2), (h2, b2), (h3, g3), (h3, i3), (h3, e3), (h3, b3), (i1, g1), (i1, h1), (i1, c1), (i1, f1), (i2, g2), (i2, h2), (i2, c2), (i2, f2), (i3, g3), (i3, h3), (i3, c3), (i3, f3) }";
            //string str = "x = { 00, 01, 02 } y = { 10, 11, 12 } z = { 20, 21, 22 } {} !{ (00, 10), (00, 20), (01, 11), (01, 21), (02, 12), (02, 22), (10, 00), (10, 20), (11, 01), (11, 21), (12, 02), (12, 22), (20, 00), (20, 10), (21, 01), (21, 11), (22, 02), (22, 12)}";
            //string str = "x = { x1, x2, x3 }" + "\n" + "y = { y1, y2, y3 }" + "\n" + "z = { z1, z2, z3 }" + "\n" + "{ (y1, z1), (y2, z2), (y3, z3) }"
            //+ "\n" + "!{ (x1, y2), (x1, y3), (x2, y1), (x2, y3), (x3, y1), (x3, y2) }";
            Parser p = new Parser(new Scanner(new StringReader(str)).Scan());
            DSL dsl = p.Parse();
            ConstraintSolver cs = new ConstraintSolver(dsl);
            /*foreach (KeyValuePair<string, string> kvp in cs.FirstSolution().assignments)
            {
                Console.WriteLine(kvp.Key + " " + kvp.Value);
            }*/

            int i = 0;
            ConstraintSolverEnumerator ce = new ConstraintSolverEnumerator(dsl);
            ConstraintDemonstrator cd = new ConstraintDemonstrator(dsl);
            StreamWriter sw = new StreamWriter("./prova.txt");
            while (cd.MoveNext()) {
                foreach (KeyValuePair<string, string> kvp in cd.Current.assignments)
                {
                    Console.Write("(" + kvp.Key + " = " + kvp.Value + "), "); 
                    sw.Write("(" + kvp.Key + " = " + kvp.Value + "), ");
                }
                Console.Write("{ ");
                sw.Write("{ ");
                foreach (Tuple<string, string> t in cd.Current.satisfy.relation)
                {
                    Console.Write("(" + t.Item1 + " = " + t.Item2 + "), ");
                    sw.Write("(" + t.Item1 + " = " + t.Item2 + "), ");
                }
                Console.WriteLine("}");
                sw.WriteLine("}");
                Console.Write("!{ ");
                sw.Write("!{ ");
                foreach (Tuple<string, string> t in cd.Current.notSatisfy.relation)
                {
                    Console.Write("(" + t.Item1 + " = " + t.Item2 + "), ");
                    sw.Write("(" + t.Item1 + " = " + t.Item2 + "), ");
                }
                Console.WriteLine("}");
                sw.WriteLine("}");
                i++;
                Console.WriteLine();
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            Console.WriteLine("\n" + i + "\n");
            
            /*ConstraintDemonstrator cd = new ConstraintDemonstrator(dsl);
            i = 0;
            foreach (Demonstration dm in cd) {
                foreach (KeyValuePair<string, string> kvp in dm.assignments)
                {
                    Console.WriteLine(kvp.Key + " " + kvp.Value);
                }
                Console.Write("{ ");
                foreach (Tuple<string, string> pair in dm.satisfy.relation) {
                    Console.Write(("(" + pair.Item1 + ", " + pair.Item2 + ") "));
                }
                Console.WriteLine("}");
                Console.Write("!{ ");
                foreach (Tuple<string, string> pair in dm.notSatisfy.relation)
                {
                    Console.Write(("(" + pair.Item1 + ", " + pair.Item2 + ") "));
                }
                Console.WriteLine("}");
                i++;
            }


            /*foreach (SortedList<string, string> sl in cs.solve()) {
                foreach (KeyValuePair<string, string> kvp in sl) {
                    Console.WriteLine(kvp.Key + " " + kvp.Value);
                }
            }*/
            /*foreach (Variable v in dsl.statements.vars) {
                Console.WriteLine(v.ide);
                foreach (string s in v.values) {
                    Console.Write(s + " ");
                }
                Console.WriteLine("\n");
            }
            Console.Write("Satisfy: ");
            foreach (Tuple<string, string> t in dsl.satisfy.relation) {
                Console.Write("(" + t.Item1 + ", " + t.Item2 + ") ");
            }
            Console.WriteLine("\n");
            Console.Write("NotSatisfy: ");
            foreach (Tuple<string, string> t in dsl.notSatisfy.relation)
            {
                Console.Write("(" + t.Item1 + ", " + t.Item2 + ") ");
            }*/
            Console.WriteLine("\n");
            /*Variable v = new Variable("ciao");
            List<Variable> vars = new List<Variable>();
            Relation satisfy = new Relation();
            Relation notSatisfy = new Relation();

            v.ide = "ciao";
            v.AddValue("x1");
            v.AddValue("x2");
            //string str = "x = { 00, 01, 02 } y = { 10, 11, 12 } z = { 20, 21, 22 } {} !{ (00, 10), (00, 20), (01, 11), (01, 21), (02, 12), (02, 22), (10, 00), (10, 20), (11, 01), (11, 21), (12, 02), (12, 22), (20, 00), (20, 10), (21, 01), (21, 12), (22, 02), (22, 12)}";
            string str = "x = { x1, x2, x3 }" + "\n" + "y = { y1, y2, y3 }" + "\n" + "z = { z1, z2, z3 }" + "\n" + "{ (y1, z1), (y2, z2), (y3, z3) }"
            + "\n" + "!{ (x1, y2), (x1, y3), (x2, y1), (x2, y3), (x3, y1), (x3, y2) }";
            Parser p = new Parser(vars, satisfy, notSatisfy, new Scanner(new StringReader(str)).Scan());
            p.Parse();*/

           /*
            foreach (KeyValuePair<string, Edges> node in satisfy.graph) {  // stampare tutto
                foreach (KeyValuePair<string, List<Tuple<string, Edges>>> edgeX in node.Value.arcs)
                    if (edgeX.Value != null)
                        foreach (Tuple<string, Edges> edgeY in edgeX.Value)
                            Console.WriteLine(edgeX.Key + " - " + edgeY.Item1);
            }
            foreach (KeyValuePair<string, Edges> node in notSatisfy.graph)
            {  // stampare tutto
                foreach (KeyValuePair<string, List<Tuple<string, Edges>>> edgeX in node.Value.arcs)
                    if (edgeX.Value != null)
                        foreach (Tuple<string, Edges> edgeY in edgeX.Value)
                            Console.WriteLine(edgeX.Key + " - " + edgeY.Item1);
            }*/

            //ConstraintSolver cs = new ConstraintSolver(vars, satisfy, notSatisfy);
            /*Scanner sc = new Scanner(new StringReader(str));
            TokenStream ts = sc.Scan();
            ts.MoveNext();
            while (ts.Current.kind != Kind.EOF) {
                Console.WriteLine(ts.Current.kind == Kind.VALUE || ts.Current.kind == Kind.VARIABLE ? ts.Current.value : "simbolo");
                ts.MoveNext();
            }*/
            /*
            List<Variable> vars = new List<Variable>();
            Relation satisfy = new Relation();
            Relation notSatisfy = new Relation();
            
            List<Token> toks = new List<Token>();
            toks.Add(new Token("x", Kind.VARIABLE));
            toks.Add(new Token(Kind.ASSIGN));
            toks.Add(new Token(Kind.LEFT_BRACE));
            toks.Add(new Token("x1", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.RIGHT_BRACE));

            toks.Add(new Token(Kind.LEFT_BRACE));


            toks.Add(new Token(Kind.LEFT_PARENT));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x1", Kind.VALUE));
            toks.Add(new Token(Kind.RIGHT_PARENT));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token(Kind.LEFT_PARENT));
            toks.Add(new Token("x3", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.RIGHT_PARENT));

            toks.Add(new Token(Kind.RIGHT_BRACE));

            toks.Add(new Token(Kind.EXCLAMATION));
            toks.Add(new Token(Kind.LEFT_BRACE));
            toks.Add(new Token(Kind.LEFT_PARENT));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x1", Kind.VALUE));
            toks.Add(new Token(Kind.RIGHT_PARENT));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token(Kind.LEFT_PARENT));
            toks.Add(new Token("x3", Kind.VALUE));
            toks.Add(new Token(Kind.COMMA));
            toks.Add(new Token("x2", Kind.VALUE));
            toks.Add(new Token(Kind.RIGHT_PARENT));
            toks.Add(new Token(Kind.RIGHT_BRACE));
            toks.Add(new Token(Kind.EOF));

            
            Parser p = new Parser(vars, satisfy, notSatisfy, toks.GetEnumerator());
            p.Parse();
            */
            /*foreach (Variable x in vars)
            {
                Console.WriteLine(x.ide);
                foreach (string s in x.NextValue()) {
                    Console.WriteLine(s);
                }
            }
            foreach (Tuple<string, string> s in satisfy.NextPair())
            {
                Console.WriteLine("(" + s.Item1 + ", " + s.Item2 + ")");
            }
            foreach (Tuple<string, string> s in notSatisfy.NextPair())
            {
                Console.WriteLine("(" + s.Item1 + ", " + s.Item2 + ")");
            }
            */
            /*
            Relation r = new Relation();
            r.AddPair("(x1, y1)");
            r.AddPair("(x0, y0)");
            foreach (String s in r.NextPair()) {
                Console.WriteLine(s);
            }
            
            foreach (String s in v.NextValue())
                Console.WriteLine(s);
            */
            Console.WriteLine("Hello World!");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
