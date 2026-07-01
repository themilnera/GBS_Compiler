using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBSCompiler
{
	internal class Scanner
    {
		public static Dictionary<string, Kind> Keywords = new Dictionary<string, Kind>
		{
			{ "NUM8", Kind.T_NUM8 },
			{ "NUM16", Kind.T_NUM16 },
			{ "STRING", Kind.T_STRING },
			{ "CHAR", Kind.T_CHAR },
			{ "ARRAY", Kind.T_ARRAY },
			{ "IF", Kind.IF },
			{ "ELSE", Kind.ELSE },
			{ "WHILE", Kind.WHILE },
			{ "FOR", Kind.FOR },
			{ "FUNC", Kind.FUNC },
			{ "RET", Kind.RET },
			{ "AND", Kind.AND },
			{ "OR", Kind.OR }
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
		public static Token[] Scan(string source)
        {
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
                    StringBuilder name = new StringBuilder();
					while (i < source.Length && (IsValidABChar(source[i]) || IsValidNum(source[i]))){
						name.Append(source[i]);
                        i++;
                    }
					string nm = name.ToString().ToUpper();
                    tokens.Add(new Token(Keywords.ContainsKey(nm) 
                        ? Keywords[nm] 
                        : Kind.IDENTIFIER, nm));
                }

				else if (source[i] == '\"' || source[i] == '\''){
					StringBuilder str = new StringBuilder();
					i++;
					while(i < source.Length && (source[i] != '\"' || source[i] != '\'')){
						str.Append(source[i]);
						i++;
					}
					str.Remove(str.Length - 1, 1);
					tokens.Add(new Token(Kind.STRING, str.ToString()));
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
					if (Symbols.ContainsKey(symbol.ToString())){
						tokens.Add(new Token(Symbols[symbol.ToString()], symbol.ToString()));
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
            if(Symbols.ContainsKey(c.ToString()) || c == '!'){
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