
using GBSCompiler;
using System.IO;

//void DisplayTree(GBSCompiler.Program ast)
//{
//	foreach (Node n in ast.Body)
//	{
//		if (n is Assignment a)
//		{
//			Console.Write("Assignment: ");
//			Console.Write(a.Name + " " + a.Type + " " + a.Value);
//		}
//		if (n is If i)
//		{
//			Console.Write("IF: ");
//			Console.Write(i.Condition + " ");
//			foreach (Node n2 in i.Body)
//			{
//				Console.Write(n2);
//			}
//		}
//		Console.Write("\n");
//	}
//}

string path = "C:\\Users\\adamm\\source\\repos\\GBSCompiler\\GBSCompiler\\test.gbs";
string source = File.ReadAllText(path);

Token[] scan = Scanner.Scan(source);
//foreach  (Token token in scan){
//	Console.Write("{ kind: "+token.kind+", ");
//	Console.Write("value: "+token.value + " }\n");
//}

Parser parser = new Parser(scan);
GBSCompiler.Program ast = parser.ParseTokens();
Generator generator = new Generator();
Generator.Write("D:\\assembly\\GameBoy\\Test\\init.asm", generator.GenerateCode(ast));
