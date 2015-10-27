using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalPA
{
    class Relation
    {
        public List<Tuple<string, string>> relation;
        public Relation() { relation = new List<Tuple<string, string>>(); }
        public void AddPair(string x, string y) { relation.Add(new Tuple<string, string>(x, y)); }
    }
}
