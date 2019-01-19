using System;
using System.IO;
using System.Net;

namespace VideoContactSheetMaker {

	enum InputFileType {
		File,
		Directory,
		Url
	}

	class InputFile {
		public InputFile(string input, InputFileType type) {

			File = input;
			Type = type;

			var uri = new Uri(input);
			Filename = Path.GetFileName(uri.LocalPath);
			FilenameWithoutExtension = Path.GetFileNameWithoutExtension(uri.LocalPath);

			if (Type == InputFileType.Url) {
				try {
					//Try getting file size
					var req = (HttpWebRequest)WebRequest.Create(input);
					req.Method = "HEAD";
					using (var resp = (HttpWebResponse)req.GetResponse()) {
						FileSize = resp.ContentLength;
					}

				}
				catch {
					FileSize = 0;
				}
			}
			else if (Type == InputFileType.File) {
				FileSize = new FileInfo(input).Length;
			}


		}

		public readonly string FilenameWithoutExtension;
		public readonly string Filename;
		public readonly string File;
		public readonly InputFileType Type;
		public readonly long FileSize;
	}
}
