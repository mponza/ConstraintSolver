using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA
{
    class Statements
    {
        public List<Variable> vars;
        public Statements() { this.vars = new List<Variable>(); }
        public string GetIdeFromValue(String valueToFind) {
            foreach (Variable var in vars)
                foreach (string value in var.values)
                    if (value == valueToFind) return var.ide;
            return null;
        }
    }
}
