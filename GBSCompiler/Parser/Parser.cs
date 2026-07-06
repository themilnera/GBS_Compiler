
namespace GBSCompiler
{
    internal class Parser
    {
		public Token[] Tokens {  get; set; }
		int i = 0;
		public List<Assignment> Assigned = new List<Assignment>();
		public Parser(Token[] tokens){
			Tokens = tokens;
		}
		private Token peek(){
			return Tokens[i];
		}
		private Token peek(int offset){
			return Tokens[i+offset];
		}
		private Token consume(Kind kind){
			Token token = Tokens[i];
			if(token.kind != kind){
				throw new Exception("Expected token: "+kind+" \nGot kind: "+token.kind);
			}
			i++;
			return token;
		}
		private Token consume(){
			Token token = Tokens[i];
			i++;
			return token;
		}
		private Node branch()
		{
			Token token = peek();
			//if line starts with type kw or identifier, it should be an assignment
			if(token.kind == Kind.T_STRING || token.kind == Kind.T_CHAR || token.kind == Kind.T_ARRAY || token.kind == Kind.T_INT8 || token.kind == Kind.T_INT16 || token.kind == Kind.IDENTIFIER){
				return parseAssignment();
			}
			else if(token.kind == Kind.IF)
			{
				return parseIf();
			}
			//else if(token.kind == Kind.ELSE)
			//{

			//}
			//else if(token.kind == Kind.WHILE)
			//{

			//}
			//else if(token.kind == Kind.FOR)
			//{

			//}
			else if (token.kind == Kind.FUNC)
			{
				return parseFunc();
			}
			else if (token.kind == Kind.CALL)
			{
				return parseCall();
			}
			else if (token.kind == Kind.INLINEASM){
				return parseInlineASM();
			}
			else
			{
				throw new Exception("Unrecognized token: " + token.kind);
			}
		}

		private Node parseInlineASM(){
			return new InlineASM(consume().value);
		}

		private Node parseCall(){
			consume(Kind.CALL);
			string name = consume(Kind.IDENTIFIER).value;
			consume(Kind.LPAR);
			List<Node> args = new List<Node>();
			while(peek().kind != Kind.RPAR){
				args.Add(parseExpression());
				if(peek().kind == Kind.COM){
					consume(Kind.COM);
				}
			}
			consume(Kind.RPAR);
			consume(Kind.SEMC);
			return new Call(name, args.ToArray());
		}
		private Node parseFunc()
		{
			consume(Kind.FUNC);
			string name = consume(Kind.IDENTIFIER).value;
			consume(Kind.LPAR);
			List<Node> args = new List<Node>();
			
			while (peek().kind != Kind.RPAR){
				Node assn = parseAssignment();
				args.Add(assn);
				if(peek().kind == Kind.COM){ 
					consume(Kind.COM);
				}
			}
			consume(Kind.RPAR);
			consume(Kind.LBRAC);
			List<Node> body = new List<Node>();
			while(peek().kind != Kind.RET){
				body.Add(branch());
			}
			consume(Kind.RET);
			Node @return = parseExpression();
			consume(Kind.SEMC);
			consume(Kind.RBRAC);
			return new Function(name, args.ToArray(), body.ToArray(), @return);
		}

		private Node parseIf(){
			consume(Kind.IF);
			consume(Kind.LPAR);
			Node condition = parseCondition();
			consume(Kind.RPAR);
			consume(Kind.LBRAC);
			List<Node> body = new List<Node>();
			while(peek().kind != Kind.RBRAC)
			{
				body.Add(branch());
			}
			consume(Kind.RBRAC);
			return new If(condition, body.ToArray());
		}
		
		private Node parseAssignment(){
			Token token = peek();

			//reassignment, is identifier initialized?
			if(token.kind == Kind.IDENTIFIER){
				bool initialized = false;
				string name = token.value;
				string type = "";

				foreach(Assignment a in Assigned){
					if(a.Name == name){
						initialized = true;
						type = a.Type;
					}
				}
				if(!initialized){
					throw new Exception("Assignment: "+token.value+" occurred prior to initialization.");
				}
				consume();
				consume(Kind.ASSN);
				Node value = parseExpression();
				Assignment assn = new Assignment(value, name, type);
				consume(Kind.SEMC);
				return assn;
			}

			//initialization
			else if(token.kind == Kind.T_STRING || token.kind == Kind.T_CHAR || token.kind == Kind.T_INT8 || token.kind == Kind.T_INT16 || token.kind == Kind.T_BOOL || token.kind == Kind.T_ARRAY)
			{
				string type = token.kind.ToString();
				consume();
				string name = consume(Kind.IDENTIFIER).value;
				consume(Kind.ASSN);
				Node value = parseExpression();
				Assignment assn = new Assignment(value, name, type);
				Assigned.Add(assn);
				consume(Kind.SEMC);
				return assn;
			}
			else
			{
				throw new Exception("Unexpected token: "+token.kind);
			}
		}
		
		private Node parseExpression(){
			Token token = peek();
			if(token.kind == Kind.STRING){
				return parseStringExpression();
			}
			else if(token.kind == Kind.INT8 || token.kind == Kind.INT16){
				return parseMathExpression();
			}
			else if(token.kind == Kind.IDENTIFIER){
				string type = "unknown";
				foreach (Assignment a in Assigned)
				{
					if (a.Name == token.value)
					{
						type = a.Type;
					}
				}
				if(type == "T_STRING"){
					return parseStringExpression();
				}
				else if(type == "T_INT8" || type == "T_INT16"){
					return parseMathExpression();
				}
				else{
					throw new Exception("In expression, identifier: "+token.value+" has not been intialized.");
				}
			}
			else
			{
				throw new Exception("Unexpected token: "+token.kind);	
			}

		}
		private Node parseMathExpression()
		{
			Node left = parseValue();
			Token token = peek();
			while (token.kind == Kind.PLUS || token.kind == Kind.MIN || token.kind == Kind.MULT || token.kind == Kind.DIV || token.kind == Kind.MOD)
			{
				string op = consume().value;
				Node right = parseValue();
				if(right is Int8 || right is Int16){
					left = new Operation(op, left, right);
					token = peek();
				}
				else if(right is String)
				{
					throw new Exception("Cannot implicitly convert from INT8/INT16 to STRING.");
				}
				else
				{
					throw new Exception("Cannot implicitly convert types.");
				}
			}
			return left;
		}
		private Node parseStringExpression()
		{
			Node left = parseValue();
			Token token = peek();
			while (token.kind == Kind.PLUS)
			{
				string op = consume().value;
				Node right = parseValue();
				if(right is String)
				{
					left = new Operation(op, left, right);
					token = peek();
				}
				else{
					throw new Exception("Cannot implicitly convert types.");
				}
			}
			return left;
		}
		

		private Node parseCondition()
		{
			Node left = parseValue();
			Token token = peek();
			bool condition = true;
			while(condition)
			{
				string op = consume().value;
				Node right = parseValue();
				left = new Condition(op, left, right);
				
				token = peek();
				if(token.kind == Kind.AND || token.kind == Kind.OR) {
					continue;
				}
				condition = false;
			}

			return left;
		}

		private Node parseValue(){
			Token token = peek();
			if(token.kind == Kind.IDENTIFIER){
				consume();
				return new Identifier(token.value);
			}
			if(token.kind == Kind.STRING){
				consume();
				return new String(token.value);
			}
			if(token.kind == Kind.INT8){
				consume();
				return new Int8(int.Parse(token.value));
			}
			if (token.kind == Kind.INT16)
			{
				consume();
				return new Int16(int.Parse(token.value));
			}
			throw new Exception("Unexpected token: "+token.kind);
		}

		public Program ParseTokens(){
			List<Node> body = new List<Node>();
			while(peek().kind != Kind.EOF){
				body.Add(branch());
			}
			return new Program(body.ToArray());
		}
    }
}
