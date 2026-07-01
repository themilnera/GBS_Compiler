using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	internal class Assignment : Node
	{
		public Node Value { get; set; }
		public Assignment(Node value){
			Value = value;
		}
	}

	internal class Operation : Node 
	{
		public Node[] Elements { get; set; }
		public string[] Ops { get; set; }
		public Operation(string[] ops, Node[] elements)
		{
			Ops = ops;
			Elements = elements;
		}
	}
	internal class Condition : Node 
	{
		public Node[] Elements { get; set; }
		public string[] Ops { get; set; }
		public Condition(Node[] elements, string[] ops)
		{ 
			Elements = elements;
			Ops = ops;
		}
	}

	internal class If : Node
	{
		public Node[] Conditions { get; set; }
		public Node[] Body { get; set; }
		public If(Node[] conditions, Node[] body)
		{
			Body = body;
			Conditions = conditions;
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
		public Node[] Body { get; set; }
		public Node[] Conditions { get; set; }
		public While(Node[] conditions, Node[] body){
			Body = body;
			Conditions = conditions;
		}
	}

	internal class For : Node 
	{
		public Node[] Body { get; set; }
		public Node Assignment { get; set; }
		public Node Condition { get; set; }
		public Node Operation { get; set; }
	}

	internal class Function : Node
	{
		public Node[] Body { get; set; }
		public Node[] Arguments { get; set; }
	}

}
