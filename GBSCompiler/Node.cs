using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBSCompiler
{
	internal class Node
	{
		public string Kind {  get; set; }
		public Node(string kind){
			this.Kind = kind;	
		}
	}
	internal class Program : Node
	{
		public Node[] Body { get; set; }
		public Program(string kind, Node[] body) : base(kind)
		{
			Body = body;
		}
	}

	internal class Assignment : Node
	{
		public Node Value { get; set; }
		public Assignment(string kind, Node value): base(kind){
			Value = value;
		}
	}


}
