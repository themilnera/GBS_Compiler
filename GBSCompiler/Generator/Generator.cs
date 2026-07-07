using System.IO;

namespace GBSCompiler
{
	internal class Generator
	{
		List<string> lines = new List<string>();
		string inlineAsm = "";
		private void emit(string line) {
			lines.Add(line + "\n");
		}

		private void parseInline(){
			if (inlineAsm.Contains("PMI:") && inlineAsm.Contains("END_PMI:"))
			{
				int s_i = inlineAsm.IndexOf("PMI:");
				int s_end_i = inlineAsm.IndexOf("END_PMI:");
				int d_i = lines.IndexOf("PMI:\n");
				lines.RemoveAt(d_i);
				string block = inlineAsm.Substring(s_i, s_end_i - s_i);
				lines.Insert(d_i, block);
			}
			if (inlineAsm.Contains("VBHI:") && inlineAsm.Contains("END_VBHI:"))
			{
				int s_i = inlineAsm.IndexOf("VBHI:");
				int s_end_i = inlineAsm.IndexOf("END_VBHI:");
				int d_i = lines.IndexOf("VBHI:\n");
				lines.RemoveAt(d_i);
				string block = inlineAsm.Substring(s_i, s_end_i - s_i);
				lines.Insert(d_i, block);
			}
			if (inlineAsm.Contains("INITI:") && inlineAsm.Contains("END_INITI:"))
			{
				int s_i = inlineAsm.IndexOf("INITI:");
				int s_end_i = inlineAsm.IndexOf("END_INITI:");
				int d_i = lines.IndexOf("INITI:\n");
				lines.RemoveAt(d_i);
				string block = inlineAsm.Substring(s_i, s_end_i - s_i);
				lines.Insert(d_i, block);
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
