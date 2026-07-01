
using GBSCompiler;
using System.IO;

string path = "C:\\Users\\adamm\\source\\repos\\GBSCompiler\\GBSCompiler\\test.gbs";
string source = File.ReadAllText(path);

Token[] scan = Scanner.Scan(source);
foreach  (Token token in scan){
	Console.Write("{ kind: "+token.kind+", ");
	Console.Write("value: "+token.value + " }\n");
}