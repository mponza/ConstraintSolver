using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA {
    class Variable
    {
        public string ide; // Identificatore della variabile
        public List<string> values; // Insieme di possibili valori della variabile
        public Variable(string ide) { this.ide = ide; values = new List<String>(); }
    }
}
