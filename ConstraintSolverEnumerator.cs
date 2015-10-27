using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace FinalPA
{
    class ConstraintSolverEnumerator : ConstraintSolver, IEnumerator<Solution>
    {
        protected IEnumerator<Solution> solver;
        public ConstraintSolverEnumerator() : base() { }
        public ConstraintSolverEnumerator(DSL dsl) : base(dsl) { solver = RecSolve(new Solution()).GetEnumerator(); }
        private IEnumerable<Solution> RecSolve(Solution solution)
        {
            if (solution.assignments.Count == dsl.statements.vars.Count) { yield return solution; }
            Variable var = UnassignedVar(solution); // var può ora essere null visto che questo comando può essere eseguito anche se l'if sopra risulta true (causa yield)
            if (var != null)
                foreach (string val in var.values)
                {
                    Solution newSolution = new Solution(solution);
                    newSolution.assignments.Add(var.ide, val);
                    if (!Propagate(newSolution)) break;
                    if (Consinstent(newSolution))
                        foreach (Solution result in RecSolve(newSolution))
                            yield return result;
                }
            yield break;
        }
        object IEnumerator.Current { get { return Current; } }
        public Solution Current { get { return solver.Current; } }
        public virtual void Dispose() { solver.Dispose(); }
        public virtual bool MoveNext() { return solver.MoveNext(); }
        public virtual void Reset() { solver.Reset(); }
    }
}
