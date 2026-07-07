using System.IO;

namespace GBSCompiler
{
	internal class Generator
	{
		List<string> lines = new List<string>();
		private List<string> nodeAsm = new List<string>();
		string inlineAsm = "";

		private string[] inserts = ["PMI","VBHI","INITI","BI","SCODI"];
		private string[] nodeInserts = ["NVAR"];
		private void emit(string line) {
			lines.Add(line + "\n");
		}

		private void parseInlineInserts(){
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
		private void parseNodeInserts(){
			foreach(string item in nodeAsm){ 
				foreach (string insert in nodeInserts)
				{
					if(item.Contains(";"+insert)){
						string i = item.Remove(0, insert.Length +1);
						lines.Insert(lines.IndexOf(";" + insert + "\n")+1, i);
					}
				}
			}
		}
		
		public string GenerateCode(Node node){
			parseNode(node);
			parseNodeInserts();
			parseInlineInserts();
			return string.Concat(lines);
		}

		private void parseInitialization(Assignment assignment){
			string kw = "";
			string value = "";
			if (assignment.Type == "T_INT8" || assignment.Type == "T_STRING")
			{
				kw = "db";
			}
			else if (assignment.Type == "T_INT16")
			{
				kw = "dw";
			}

			if (kw != "")
			{
				if (assignment.Value is Int8 int8 && int8.Value > 255)
				{
					throw new Exception("Cannot assign value " + int8.Value.ToString() + " to int8 " + assignment.Name);
				}
				else
				{
					string asm = ";NVAR";
					asm += assignment.Name + ": " + kw + " ";
					if (assignment.Value is Int8 eight)
					{
						asm += eight.Value.ToString();
					}
					if (assignment.Value is Int16 sixteen)
					{
						asm += sixteen.Value.ToString();
					}
					if (assignment.Value is String stri){
						asm += "\""+stri.Value+"\", 0";
					}
					nodeAsm.Add(asm+"\n");
				}
			}
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
			else if (node is Assignment assignment){
				if (assignment.Type == "T_INT8" || assignment.Type == "T_INT16" || assignment.Type == "T_STRING"){
					parseInitialization(assignment);
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
