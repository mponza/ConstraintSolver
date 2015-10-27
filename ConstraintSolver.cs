using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FinalPA
{
    class Solution {
        public SortedList<string, string> assignments;
        public Solution() { assignments = new SortedList<string, string>(); }
        public Solution(Solution solution) { this.assignments = new SortedList<string,string>(solution.assignments); }
    }
    class ConstraintSolver {
        protected DSL dsl;
        public ConstraintSolver() : this(null) { }
        public ConstraintSolver(DSL dsl) { this.dsl = dsl; }
        // Ritorna una variabile a cui non è ancora stato effettuato un assegnamento
        protected Variable UnassignedVar(Solution solution) { 
            foreach (Variable var in dsl.statements.vars)
                if (!solution.assignments.ContainsKey(var.ide)) return var;
            return null;
        }
        // Propaga le conseguenze degli assegnamenti effettuati modificando assignments.
        // Ritorna false se, durante la propagazione, si cerca d'assegnare un valore xi ad una variabile
        // a cui è già stato assegnato un valore yi (con xi != yi). Ritorna true altrimenti.
        protected bool Propagate(Solution solution) {
            List<KeyValuePair<string, string>> assignmentsList = solution.assignments.ToList();
            if (dsl.satisfy.relation.Count == 0) return true;
            for (int i = 0; i < assignmentsList.Count; i++)
                foreach (Tuple<string, string> pair in dsl.satisfy.relation)
                    if (assignmentsList[i].Value == pair.Item1) {
                        string ide = dsl.statements.GetIdeFromValue(pair.Item2);
                        if (solution.assignments.ContainsKey(ide)) {
                            if (solution.assignments[ide] != pair.Item2) return false;
                        } else { // ide non è contenuto in assignments
                            solution.assignments.Add(ide, pair.Item2);
                            assignmentsList.Add(new KeyValuePair<string,string>(ide, pair.Item2));
                        }
                    }
            return true;
        }
        // Ritorna true se l'assegnamento rispetta la relazione notSatisfy, false altrimenti.
        protected bool Consinstent(Solution solution)
        {
            if (dsl.notSatisfy.relation.Count == 0) return true;
            foreach (Tuple<string, string> pair in dsl.notSatisfy.relation) {
                string ideX = dsl.statements.GetIdeFromValue(pair.Item1);
                string ideY = dsl.statements.GetIdeFromValue(pair.Item2);
                if (solution.assignments.ContainsKey(ideX) && solution.assignments[ideX] == pair.Item1 && solution.assignments.ContainsKey(ideY) && solution.assignments[ideY] == pair.Item2)
                    return false;
            }
            return true;
        }
        private Solution RecSolveFirstSolution(Solution solution) {
            // Ad ogni variabile è stato assegnato un valore
            if (solution.assignments.Count == dsl.statements.vars.Count) { return solution; }
            Variable var = UnassignedVar(solution);
            foreach (string val in var.values)
            {
                Solution newSolution = new Solution(solution);
                newSolution.assignments.Add(var.ide, val);
                if (!Propagate(newSolution)) break;
                if (Consinstent(newSolution))
                {
                    newSolution = RecSolveFirstSolution(newSolution);
                    if (newSolution != null) return newSolution;
                }
            }
            return null;
        }
        // Ritorna la prima soluzione trovata dalla funzione recSolve
        public Solution FirstSolution() {
            if (dsl == null) throw new InvalidOperationException("DSL is null");
            return RecSolveFirstSolution(new Solution());
        }
    }
}