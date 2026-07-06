
namespace GBSCompiler
{
	internal class Node{}
	internal class Program : Node
	{
		public Node[] Body { get; set; }
		public Program(Node[] body)
		{
			Body = body;
		}
	}
	internal class Int8 : Node
	{
		public int Value { get; set; }
		public Int8(int value) 
		{ 
			Value = value;
		}
	}

	internal class Int16 : Node
	{
		public int Value { get; set; }
		public Int16(int value)
		{
			Value = value;
		}
	}

	internal class String : Node 
	{
		public string Value { get; set; }
		public String(string value) {
			Value = value;
		}
	}
	internal class Identifier: Node
	{
		public string Name{  get; set; }
		public Identifier(string name){
			Name = name;
		}
	}

	internal class Assignment : Node
	{
		public Node Value { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public Assignment(Node value, string name, string type)
		{
			Value = value;
			Name = name;
			Type = type;
		}
	}

	internal class Operation : Node 
	{
		public string Op { get; set; }
		public Node Left { get; set; }
		public Node Right { get; set; }
		public Operation(string op, Node left, Node right)
		{
			Op = op;
			Left = left;
			Right = right;
		}
	}
	internal class Condition : Node 
	{
		public string Op { get; set; }
		public Node Left{ get; set; }
		public Node Right{ get; set; }
		public Condition(string op, Node left, Node right)
		{ 
			Op = op;
			Left = left;
			Right = right;
		}
	}

	internal class If : Node
	{
		public Node Condition { get; set; }
		public Node[] Body { get; set; }
		public If(Node condition, Node[] body)
		{
			Condition = condition;
			Body = body;
		}
	}

	internal class Else : Node 
	{
		public Node[] Body { get; set; }
		public Else(Node[] body)
		{
			Body = body;
		}
	}

	internal class While : Node
	{
		public Node[] Conditions { get; set; }
		public Node[] Body { get; set; }
		public While(Node[] conditions, Node[] body){
			Body = body;
			Conditions = conditions;
		}
	}

	internal class For : Node 
	{
		public Node Assignment { get; set; }
		public Node Condition { get; set; }
		public Node Operation { get; set; }
		public Node[] Body { get; set; }
		public For(Node assignment, Node condition, Node operation, Node[] body){
			Assignment = assignment;
			Condition = condition;
			Operation = operation;
			Body = body;
		}
	}

	internal class Function : Node
	{
		public string Name { get; set; }
		public Node[] Arguments { get; set; }
		public Node[] Body { get; set; }
		public Node Return { get; set; }
		public Function(string name, Node[] arguments, Node[] body, Node @return)
		{
			Name = name;
			Arguments = arguments;
			Body = body;
			Return = @return;
		}
	}
	internal class Call : Node
	{
		public string Name { get; set; }
		public Node[] Arguments { get; set; }
		public Call(string name, Node[] arguments)
		{
			Name = name;
			Arguments = arguments;
		}
	}
	internal class InlineASM : Node
	{
		public string Body { get; set; }
		public InlineASM(string body){
			Body = body;
		}
	}

}
