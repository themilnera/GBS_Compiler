using System.IO;

namespace GBSCompiler
{
	internal class Generator
	{
		List<string> lines = new List<string>();
		string inlineAsm = "";
		private string[] inserts = ["PMI","VBHI","INITI","BI","SCODI"];
		private void emit(string line) {
			lines.Add(line + "\n");
		}

		private void parseInline(){
			foreach(string insert in inserts){
				if(inlineAsm.Contains(insert+":") && inlineAsm.Contains("END_"+insert+":")){
					int s_i = inlineAsm.IndexOf(insert + ":");
					int s_end_i = inlineAsm.IndexOf("END_" + insert + ":");
					int d_i = lines.IndexOf(insert + ":\n");
					lines.RemoveAt(d_i);
					string block = inlineAsm.Substring(s_i, s_end_i - s_i);
					lines.Insert(d_i, block);
				}
			}
		}
		public string GenerateCode(Node node){
			parseNode(node);
			parseInline();
			return string.Concat(lines);
		}

		private void parseNode(Node node){
			if (node is Program program)
			{
				lines.InsertRange(0, Default.GetDefaults());

				foreach (Node n in program.Body)
				{
					parseNode(n);
				}
			}
			else if (node is InlineASM inline)
			{
				inlineAsm += inline.Body + "\n";
			}
		}

		public static void Write(string path, string data){
			File.WriteAllText(path, data);	
		}
	}
}
