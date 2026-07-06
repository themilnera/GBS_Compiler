using System.IO;

namespace GBSCompiler
{
	internal class Generator
	{
		List<string> lines = new List<string>();
		private void emit(string line) {
			lines.Add(line + "\n");
		}


		public string GenerateCode(Node node){
			return "";
		}

		public static void WriteDefaults(string path){
			File.WriteAllText(path,Default.GetDefaults());	
		}
	}
}
