using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBSCompiler
{
	internal class Token
	{
		public Kind kind;
		public string value;
		

		public Token(Kind kind, string value)
		{
			this.kind = kind;
			this.value = value;
		}
	}
}
