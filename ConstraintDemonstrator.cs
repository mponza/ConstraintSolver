using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FinalPA
{
    class Demonstration : Solution {
        public Relation satisfy;
        public Relation notSatisfy;
        public Demonstration() { this.satisfy = new Relation(); this.notSatisfy = new Relation(); }
        public Demonstration(Solution solution) : base(solution) { }
    }
    class ConstraintDemonstrator : ConstraintSolverEnumerator, IEnumerator<Demonstration>
    {
        protected IEnumerator<Demonstration> demonstrator;
        public ConstraintDemonstrator() : base() { }
        public ConstraintDemonstrator(DSL dsl) : base(dsl) { demonstrator = Demonstrate().GetEnumerator(); }
        // Ritorna una relazione contenente coppie (xi, yi) tali per cui in solution.assignments
        // esiste una variabile il cui valore assegnato è xi
        private Relation FulfilledRelation(Solution solution, Relation rel) {
            Relation fulRel = new Relation();
            foreach (KeyValuePair<string, string> assignment in solution.assignments)
                foreach (Tuple<string, string> pair in rel.relation)
                    if (assignment.Value == pair.Item1)
                        fulRel.AddPair(pair.Item1, pair.Item2);
            return fulRel;
        }
        private IEnumerable<Demonstration> Demonstrate() {
            while (solver.MoveNext()) {
                Demonstration demonstration = new Demonstration(solver.Current);
                demonstration.satisfy = FulfilledRelation(demonstration, dsl.satisfy);
                demonstration.notSatisfy = FulfilledRelation(demonstration, dsl.notSatisfy);
                yield return demonstration;
            }
        }
        public new Demonstration Current { get { return demonstrator.Current; } } // Non si può fare l'overloading per il tipo di ritorno, costretto ad usare new, oppure si può forse fare qualcosa usando l'explicit
        public override void Dispose() { base.Dispose();  demonstrator.Dispose(); }
        public override bool MoveNext() { return demonstrator.MoveNext(); }
        public override void Reset() { base.Reset();  demonstrator.Reset(); }
    }
}
