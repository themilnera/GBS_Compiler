using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBSCompiler
{
    public enum Kind
    {
        IDENTIFIER,
		T_NUM8,
        T_NUM16,
        T_CHAR,
        T_STRING,
        T_ARRAY,

		NUM8,
		NUM16,
		CHAR,
		STRING,
		ARRAY,

        ASSN,//  =
        CMP,//  ==
        NE,// !=
        GT,// >
        LT,// <
        GTE,// >=
        LTE,// <=
        PLUS,// +
        MIN,//  -
        MULT,// *
        DIV,//  /
        MOD,//  %
        CAR,//  `
        QUT,//  "

        IF,
        ELSE,
        WHILE,
        FOR,

        FUNC,
        RET,

        LPAR,//  (
        RPAR,//  )
        LSQU,//  [
        RSQU,//  ]
        LBRAC,// {
        RBRAC,// }
        SEMC,//  ;
        CMT,//   //

        EOF
    }
    internal class Token
    {
        public Kind kind;
        public string value;
        public static Dictionary<string, Kind> Keywords = new Dictionary<string, Kind>
        { 
            { "NUM8", Kind.T_NUM8 },
            { "NUM16", Kind.T_NUM16 },
			{ "CHAR", Kind.T_CHAR },
            { "STRING", Kind.T_STRING },
            { "ARRAY", Kind.T_ARRAY },
            { "IF", Kind.IF },
            { "ELSE", Kind.ELSE },
            { "WHILE", Kind.WHILE },
            { "FOR", Kind.FOR },
            { "FUNC", Kind.FUNC },
            { "RET", Kind.RET }
        };
        public static Dictionary<string, Kind> Symbols = new Dictionary<string, Kind>{
            { "=", Kind.ASSN },
            { "==", Kind.CMP },
            { "!=", Kind.NE },
            { ">", Kind.GT },
            { "<", Kind.LT },
            { ">=", Kind.GTE },
            { "<=", Kind.LTE },
            { "+", Kind.PLUS },
            { "-", Kind.MIN },
            { "*", Kind.MULT },
            { "/", Kind.DIV },
            { "%", Kind.MOD },
            { "`", Kind.CAR },
            { "\"", Kind.QUT },
            { "(", Kind.LPAR },
            { ")", Kind.RPAR },
            { "[", Kind.LSQU },
            { "]", Kind.RSQU },
            { "{", Kind.LBRAC },
            { "}", Kind.RBRAC },
            { ";", Kind.SEMC },
            { "//", Kind.CMT }
        };

        public Token(Kind kind, string value)
        {
            this.kind = kind;
            this.value = value;
        }
    }
    internal class Scanner
    {
        public static Token[] Scan(string source)
        {
            source = source.ToUpper();
            List<Token> tokens = new List<Token>();
			int i = 0;
            while(i < source.Length) 
            {
				Console.Write(source[i]);
                if(IsWhiteSpace(source[i]))
                {
					i++;
                    continue;
                }

                else if(IsValidNum(source[i])) 
                {
                    StringBuilder num = new StringBuilder();
                    while (i < source.Length && IsValidNum(source[i])) 
                    {
                        num.Append(source[i]);
                        i++;
                    }
                    if(int.Parse(num.ToString()) > 255){
                        tokens.Add(new Token(Kind.NUM16, num.ToString()));
                    }
                    else
                    {
                        tokens.Add(new Token(Kind.NUM8, num.ToString()));
                    }
                }

				else if (IsValidABChar(source[i])){
                    StringBuilder str = new StringBuilder();
					while (i < source.Length && (IsValidABChar(source[i]) || IsValidNum(source[i]))){
                        str.Append(source[i]);
                        i++;
                    }
                    tokens.Add(new Token(Token.Keywords.ContainsKey(str.ToString()) 
                        ? Token.Keywords[str.ToString()] 
                        : Kind.IDENTIFIER, str.ToString()));
                }

				else if (ISValidSymbol(source[i])){
					StringBuilder symbol = new StringBuilder();
                    symbol.Append(source[i]);
					i++;
					while (i < source.Length && IsStackingSymbol(source[i]))
                    {
						symbol.Append(source[i]);
                        i++;
					}
					if (Token.Symbols.ContainsKey(symbol.ToString())){
						tokens.Add(new Token(Token.Symbols[symbol.ToString()], symbol.ToString()));
                    }
                    else{
						throw new Exception("Unrecognized symbol: "+symbol.ToString());
					}
                }
				
            }
            tokens.Add(new Token(Kind.EOF, "0"));
            return tokens.ToArray<Token>();
        }

        static bool IsValidABChar(char c){
            if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_')
            {
                return true; 
            }
            return false;
        }
        static bool IsValidNum(char c){
            if(c >= '0' && c <= '9'){
                return true;
            }
            return false;
        }
        static bool IsWhiteSpace(char c){
            if(c == ' ' || c == '\n' || c == '\r' || c == '\t')
            {
                return true;
            }
            return false;
        }
        static bool ISValidSymbol(char c){
			//Console.WriteLine(c);
            if(Token.Symbols.ContainsKey(c.ToString()) || c == '!'){
                return true;
            }
            return false;
        }
		static bool IsStackingSymbol(char c){
			if(c == '!' || c == '=' || c == '>' || c == '<'){
				return true;
			}
			return false;
		}
    }
    
}