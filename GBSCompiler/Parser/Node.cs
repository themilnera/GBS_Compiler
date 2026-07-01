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
		public Program(string kind, Node[] body)
		{
			Body = body;
		}
	}

	internal class Assignment : Node
	{
		public Node Value { get; set; }
		public Assignment(string kind, Node value){
			Value = value;
		}
	}

	internal class Operation : Node 
	{
		public Node[] Elements { get; set; }
		public string[] Ops { get; set; }
		public Operation(string kind, string[] ops, Node[] elements)
		{
			Ops = ops;
			Elements = elements;
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
}
