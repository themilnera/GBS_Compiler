
using GBSCompiler;
string source =
"NUM8 number = 10;\n" +
"NUM8 number2 = 12;\n" +
"if(number2 > number1){\n" +
"return number + number2;\n" +
"}";

Token[] scan = Scanner.Scan(source);
foreach  (Token token in scan){
	Console.Write("{ kind: "+token.kind+", ");
	Console.Write("value: "+token.value + " }\n");
}