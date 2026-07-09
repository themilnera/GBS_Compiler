using System.IO;

namespace GBSCompiler
{
	internal class Generator
	{
		List<string> lines = new List<string>();
		private List<string> nodeAsm = new List<string>();
		private List<string> initialized = new List<string>();
		private List<Assignment> initializedStrings = new List<Assignment>();
		string asm = "";
		string inlineAsm = "";
		string location = "";

		private string[] inserts = ["PMI","VBHI","INITI","BI","SCODI"];
		private string[] nodeInserts = ["NVAR", "NMAIN"];
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
			nodeAsm.Reverse();
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
			if (assignment.Type == "T_INT8" || assignment.Type == "T_STRING")
			{
				kw = "db";
			}
			else if (assignment.Type == "T_INT16")
			{
				kw = "dw";
			}

			if (assignment.Value is Int8 num && num.Value > 255)
			{
				throw new Exception("Cannot assign value " + num.Value.ToString() + " to int8 " + assignment.Name);
			}
			if(location == ""){ //outside of a function
				initialized.Add(assignment.Name);
				asm = ";NVAR";
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
					initializedStrings.Add(assignment);
					asm += "\""+stri.Value+"\", 0";
					if(stri.Padding != 0){
						asm += $"\n\tds {stri.Padding}, 0";
					}
				}
				nodeAsm.Add(asm+"\n");
			}
		}
		private void parseOperation(Operation opr){

		}

		private void parseReassignment(Assignment assignment){
			if(location == ""){
				throw new Exception("Cannot reassign variable outside of function: "+assignment.Value);
			}
			else{
				if(assignment.Type == "T_STRING" && assignment.Value is String str){
					foreach(Assignment old_as in initializedStrings){
						if(assignment.Name == old_as.Name)
						{
							if(assignment.Value is String new_str && old_as.Value is String old_str){
								if(new_str.Length > old_str.Length){
									throw new Exception("String lengths are fixed, cannot reassign string: "+assignment.Name);	
								}
								else{
									string s_asm = location;
									location = "";
									
									int count = 0;
									foreach(Assignment ct in initializedStrings){
										if(ct.Name.StartsWith(assignment.Name)) count++;
									}
									int zeroes = old_str.Length - new_str.Length;
									if(zeroes != 0){
										new_str.Padding = zeroes;
									}
									string newName = old_as.Name+"__X"+count;
									parseInitialization(new Assignment(new_str, newName, "T_STRING"));;
									
									location = s_asm;
									s_asm += $"ld de, {newName}\n";
									s_asm += $"ld hl, {assignment.Name}\n";
									s_asm += $"ld bc, {old_str.Length.ToString()}\n";
									s_asm += $"call MemCopy\n";
									nodeAsm.Add(s_asm);
								}
							}
							break;
						}
					}
				}
				else if(assignment.Type == "T_INT8")
				{
					if (assignment.Value is Int8 eight)
					{
						asm = location;
						asm += $"ld hl, {assignment.Name}\n";
						asm += $"ld a, {eight.Value}\n";
						asm += $"ld [hl], a\n";
						nodeAsm.Add(asm);
						asm = "";
					}
				}
				else if(assignment.Type == "T_INT16"){
					if (assignment.Value is Int16 sixteen)
					{
						asm = location;
						asm += $"ld hl, {assignment.Name}\n";
						asm += $"ld a, LOW({sixteen.Value})\n";
						asm += $"ld [hli], a\n";
						asm += $"ld a, HIGH({sixteen.Value})\n";
						asm += $"ld [hl], a\n";
						nodeAsm.Add(asm);
						asm = "";
					}
				}
				else if (assignment.Value is Operation opr)
				{
					parseNode(opr);
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
				if ((assignment.Type == "T_INT8" || assignment.Type == "T_INT16" || assignment.Type == "T_STRING") && !initialized.Contains(assignment.Name))
				{
					parseInitialization(assignment);
				}
				else
				{
					parseReassignment(assignment);
				}
			}
			else if (node is Function func){
				if ((func.Name == "MAIN" || func.Name == "VBLANK") && func.Arguments.Length != 0)
				{
					throw new Exception("Top level function "+func.Name+" must not have arguments.");
				}
				if (func.Name == "MAIN"){
					location = ";NMAIN";
					
					foreach(Node b in func.Body){
						parseNode(b);
					}
				}
				else if (func.Name == "VBLANK"){
					location = ";NVBLANK";
					asm = location;
				}
				else{
					location = ";NPMAIN";
					asm = location;
					asm += func.Name + ":\n";
					foreach(Node a in func.Arguments){
						parseNode(a);
					}
					foreach(Node b in func.Body){
						parseNode(b);
					}
				}
			}
			else if (node is Operation opr){
				parseOperation(opr);
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
