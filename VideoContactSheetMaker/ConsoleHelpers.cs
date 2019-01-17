using System;
using System.Collections.Generic;
using System.Text;

namespace VideoContactSheetMaker {
	static class ConsoleHelpers {
		public static void WriteException(Exception e) {
			const string exceptionTitle = "EXCEPTION";
			Console.WriteLine(" ");
			Console.WriteLine(exceptionTitle);
			Console.WriteLine(new string('#', exceptionTitle.Length));
			Console.WriteLine(e.Message);
			Console.WriteLine();
			Console.WriteLine(e.StackTrace);

		}
	}
}
