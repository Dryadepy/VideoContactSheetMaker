using System;
using System.Collections.Generic;
using System.Diagnostics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace VideoContactSheetMaker {
	class ContactSheetMaker {
		private const string AdvertisementText = "VCSM";
		private const string AdvertisementSubText = "Video Contact Sheet Maker @ github";
		readonly List<string> InputFiles = new List<string>();
		private readonly IProfile Profile;
		readonly string OutputFolder;

		private const int HeaderHeight = 90;
		private int TotalThumbnails => Profile.Rows * Profile.Columns;

		private readonly Dictionary<CustomFonts, FontFamily> TextFonts = new Dictionary<CustomFonts, FontFamily>();

		public ContactSheetMaker(IProfile profile, List<string> includes, bool recursive, string outputPath) {
			Profile = profile;
			OutputFolder = outputPath;
			InstallFonts();
			foreach (var s in includes) {
				if (System.IO.File.Exists(s))
					InputFiles.Add(s);
				else if (System.IO.Directory.Exists(s))
					InputFiles.AddRange(FileHelper.GetFilesRecursive(s, recursive));
				else
					Console.Write($"Skipped path because does not exist: '{s}'");
			}
		}

		void InstallFonts() {
			var dir = Utils.SafePathCombine(System.IO.Path.GetDirectoryName(typeof(ContactSheetMaker).Assembly.Location), "Fonts");

			var fonts = new FontCollection();
			using (var ms = System.IO.File.OpenRead(Utils.SafePathCombine(dir, "monofonto.ttf")))
				TextFonts.Add(CustomFonts.MonoFonto, fonts.Install(ms));
			using (var ms = System.IO.File.OpenRead(Utils.SafePathCombine(dir, "neoletters.ttf")))
				TextFonts.Add(CustomFonts.NeoLetters, fonts.Install(ms));
			using (var ms = System.IO.File.OpenRead(Utils.SafePathCombine(dir, "absender1.ttf")))
				TextFonts.Add(CustomFonts.Absender, fonts.Install(ms));
			using (var ms = System.IO.File.OpenRead(Utils.SafePathCombine(dir, "FUTRFW.ttf")))
				TextFonts.Add(CustomFonts.FuturistFixed, fonts.Install(ms));
			using (var ms = System.IO.File.OpenRead(Utils.SafePathCombine(dir, "RobotoMono-Bold.ttf")))
				TextFonts.Add(CustomFonts.Roboto, fonts.Install(ms));
		}

		public void CreateThumbnails() {
			var positionList = new float[TotalThumbnails];
			for (var i = 0; i < TotalThumbnails; i++) {
				positionList[i] = 1.0F / (TotalThumbnails + 1);
			}

			foreach (var f in InputFiles) {
				var st = Stopwatch.StartNew();
				Console.WriteLine($"Processing '{f}'...");
				var ffProbe = new FFProbeWrapper.FFProbeWrapper();
				var mediaInfo = ffProbe.GetMediaInfo(f);

				var thumbnails = new byte[TotalThumbnails][];
				var position = 0f;
				for (int i = 0; i < TotalThumbnails; i++) {
					var ffMpeg = new FFmpegWrapper.FFmpegWrapper();
					position += Convert.ToSingle(mediaInfo.Duration.TotalSeconds * positionList[i]);
					thumbnails[i] = ffMpeg.GetVideoThumbnail(f, position, Profile.ThumbnailWidth,
						Profile.ThumbnailHeight);
				}

				var imageWidth = Profile.Columns * Profile.ThumbnailWidth;
				var imageHeight = Profile.Rows * Profile.ThumbnailHeight + (Profile.HasHeader ? HeaderHeight : 0);
				using (var img = new Image<Rgba32>(imageWidth, imageHeight)) {
					img.Mutate(ctx => {
						ctx.Fill(Profile.BackgroundColor);


						if (Profile.HasHeader) {
							var font = TextFonts[Profile.Font].CreateFont(16, FontStyle.Regular);
							var fi = new System.IO.FileInfo(f);
							string text =
								$@"{$"{"File Name: ".PadRight(Profile.FontHeaderColumnWidth, ' ')} {System.IO.Path.GetFileName(f)}"}
{$"{"File Size: ".PadRight(Profile.FontHeaderColumnWidth, ' ')} {Utils.BytesToString(fi.Length)}"} ({fi.Length:N0} bytes)
{$"{"Resolution: ".PadRight(Profile.FontHeaderColumnWidth, ' ')} {mediaInfo.Streams[0].Width}x{mediaInfo.Streams[0].Height}"}
{$"{"Duration: ".PadRight(Profile.FontHeaderColumnWidth, ' ')} {mediaInfo.Duration.TrimMiliseconds()}"}";
							ctx.DrawText(text, font, Profile.HeaderTextColor, new Point(7, 7));
							if (Profile.ShowAdvertisement) {
								var opt = new TextGraphicsOptions(true) {
									BlendPercentage = 0.7f,
									BlenderMode = PixelBlenderMode.Over
								};
								var bigFont = TextFonts[Profile.Font].CreateFont(44, FontStyle.Bold);
								var size = TextMeasurer.Measure(AdvertisementText, new RendererOptions(bigFont));
								ctx.DrawText(opt, AdvertisementText, bigFont, Profile.HeaderTextColor,
									new PointF(imageWidth - size.Width - 3, 7));
								var subSize = TextMeasurer.Measure(AdvertisementSubText, new RendererOptions(font));
								ctx.DrawText(opt, AdvertisementSubText, font, Profile.HeaderTextColor,
									new PointF(imageWidth - subSize.Width - 3, 7 + size.Height + 3));
							}
						}




						var currentHeight = Profile.HasHeader ? HeaderHeight : 0;
						var currentWidth = 0;
						var columnCounter = 0;
						for (int i = 0; i < TotalThumbnails; i++) {
							ctx.DrawImage(Image.Load(thumbnails[i]), 1f, new Point(currentWidth, currentHeight));

							if (Profile.ShowTimeStamp) {
								var font = TextFonts[Profile.Font].CreateFont(22, FontStyle.Bold);
								var timestamp = 0f;
								for (int j = 0; j <= i; j++)
									timestamp += positionList[j];
								string text =
									$"{TimeSpan.FromSeconds(mediaInfo.Duration.TotalSeconds / 100 * (timestamp * 100f)).TrimMiliseconds()}";
								var opt = new TextGraphicsOptions(true) {
									BlendPercentage = 1f,
									BlenderMode = PixelBlenderMode.Over
								};
								var size = TextMeasurer.Measure(text, new RendererOptions(font));
								ctx.DrawText(opt, text, font, Profile.OverlayTextColor,
									new PointF(currentWidth + Profile.ThumbnailWidth - size.Width - 3,
										currentHeight + Profile.ThumbnailHeight - size.Height - 3));
							}

							currentWidth += Profile.ThumbnailWidth;
							columnCounter++;
							if (columnCounter == Profile.Columns) {
								currentHeight += Profile.ThumbnailHeight;
								columnCounter = 0;
								currentWidth = 0;
							}
						}

					});
					var outputFile = Utils.SafePathCombine(string.IsNullOrEmpty(OutputFolder) ? System.IO.Path.GetDirectoryName(f) : OutputFolder,
						System.IO.Path.GetFileNameWithoutExtension(f) + ".jpg");
					img.Save(outputFile);
					st.Stop();
					Console.WriteLine($"'{outputFile}' created in {st.Elapsed}");
				}


			}

		}
	}
}
