using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA
{
    class DSL
    {
        public Statements statements;
        public Relation satisfy, notSatisfy;
        public DSL() {
            statements = new Statements();
            satisfy = new Relation();
            notSatisfy = new Relation();
        }
    }
}
