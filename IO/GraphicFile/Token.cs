using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile
{
    public class Token
    {
        public TokenType type;
        public string text;

        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override string ToString()
        {
            string TokenName = type.ToString();
            return "<" + type + ", '" + text + "'>";
        }

    }
}
