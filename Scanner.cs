using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalPA
{
    using TokenStream = System.Collections.Generic.IEnumerator<Token>;
    class Scanner
    {
        private TextReader reader;
        public Scanner(TextReader reader) { this.reader = reader; }
        public TokenStream Scan()
        {
            bool isValue = false;
            List<char> specialChars = new List<char> { '{', '}', '(', ')', ',', '!', '=' };
            string str = "";
            while (reader.Peek() != -1) {
                if (Char.IsWhiteSpace((char) reader.Peek())) reader.Read();
                else {
                    str = str + (char) reader.Read();
                    switch (str) {
                        case "{": isValue = true; yield return new Token(Kind.LEFT_BRACE); break;
                        case "}": isValue = false; yield return new Token(Kind.RIGHT_BRACE); break;
                        case "(": yield return new Token(Kind.LEFT_PARENT); break;
                        case ")": yield return new Token(Kind.RIGHT_PARENT); break;
                        case ",": yield return new Token(Kind.COMMA); break;
                        case "!": yield return new Token(Kind.EXCLAMATION); break;
                        case "=": yield return new Token(Kind.ASSIGN); break;
                        default:
                            while (!Char.IsWhiteSpace((char)reader.Peek()) && !specialChars.Contains((char)reader.Peek()) && reader.Peek() != -1)
                                str = str + (char) reader.Read();
                            yield return new Token(str, isValue ? Kind.VALUE : Kind.VARIABLE);
                            break;
                    }
                    str = "";
                }
            }
            yield return new Token(Kind.EOF);
        }
    }
}
