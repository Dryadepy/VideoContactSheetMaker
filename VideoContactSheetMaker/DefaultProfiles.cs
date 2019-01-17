using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SixLabors.ImageSharp.PixelFormats;

namespace VideoContactSheetMaker
{
 public   interface IProfile
    {
		int FontHeaderColumnWidth { get; set; }
		CustomFonts Font { get; set; }

		int ThumbnailWidth { get; set; }
		int ThumbnailHeight { get; set; }
		int Rows { get; set; }
		int Columns { get; set; }
		Rgba32 BackgroundColor { get; set; }
		Rgba32 HeaderTextColor { get; set; }
		Rgba32 OverlayTextColor { get; set; }
		bool HasHeader { get; set; }
		bool ShowTimeStamp { get; set; }
		bool ShowAdvertisement { get; set; }
    }

	[Serializable]
	public class SerializableProfile : IProfile {
		[XmlElement("FontHeaderColumnWidth")]
		public int FontHeaderColumnWidth { get; set; }
		[XmlElement("Font")]
		public CustomFonts Font { get; set; }
		[XmlElement("ThumbnailWidth")]
		public int ThumbnailWidth { get; set; }
		[XmlElement("ThumbnailHeight")]
		public int ThumbnailHeight { get; set; }
		[XmlElement("Rows")]
		public int Rows { get; set; }
		[XmlElement("Columns")]
		public int Columns { get; set; }
		[XmlElement("BackgroundColor")]
		public Rgba32 BackgroundColor { get; set; }
		[XmlElement("HeaderTextColor")]
		public Rgba32 HeaderTextColor { get; set; }
		[XmlElement("OverlayTextColor")]
		public Rgba32 OverlayTextColor { get; set; }
		[XmlElement("HasHeader")]
		public bool HasHeader { get; set; }
		[XmlElement("ShowTimeStamp")]
		public bool ShowTimeStamp { get; set; }
		[XmlElement("ShowAdvertisement")]
		public bool ShowAdvertisement { get; set; }
    }

	/// <summary>
	/// KM / MPC-HC Player - like profile
	/// </summary>
	[Serializable]
public	class DefaultProfile1 : IProfile
    {
        public int FontHeaderColumnWidth {get; set;} = 0;
        public CustomFonts Font {get; set;} =  CustomFonts.Arial;
        public int ThumbnailWidth {get; set;} = 250;
        public int ThumbnailHeight {get; set;} = 140;
        public int Rows {get; set;} = 10;
        public int Columns {get; set;} = 4;
        public Rgba32 BackgroundColor {get; set;} = new Rgba32(241, 241, 241);
        public Rgba32 HeaderTextColor {get; set;} = Rgba32.Black;
        public Rgba32 OverlayTextColor {get; set;} = Rgba32.White;
        public bool HasHeader {get; set;} = true;
        public bool ShowTimeStamp {get; set;} = true;
        public bool ShowAdvertisement {get; set;} = true;
    }
    /// <summary>
    /// Big thumbnails
    /// </summary>
    class DefaultProfile2 : IProfile
    {
        public int FontHeaderColumnWidth { get; set; } = 0;
        public CustomFonts Font { get; set; } = CustomFonts.MonoFonto;
        public int ThumbnailWidth { get; set; } = 950;
        public int ThumbnailHeight { get; set; } = 540;
        public int Rows { get; set; } = 12;
        public int Columns {get; set;} = 2;
        public Rgba32 BackgroundColor {get; set;} = Rgba32.Black;
        public Rgba32 HeaderTextColor {get; set;} = Rgba32.White;
        public Rgba32 OverlayTextColor {get; set;} = Rgba32.White;
		public bool HasHeader {get; set;} = false;
        public bool ShowTimeStamp {get; set;} = true;
        public bool ShowAdvertisement {get; set;} = false;
    }
    /// <summary>
    /// Video Thumbnail Maker - like profile
    /// </summary>
    class DefaultProfile3 : IProfile
    {
        public int FontHeaderColumnWidth {get; set;} = 15;
        public CustomFonts Font {get; set;} = CustomFonts.Roboto;
        public int ThumbnailWidth {get; set;} = 370;
        public int ThumbnailHeight {get; set;} = 210;
        public int Rows {get; set;} = 4;
        public int Columns {get; set;} = 5;
        public Rgba32 BackgroundColor {get; set;} = new Rgba32(36, 36, 36);
        public Rgba32 HeaderTextColor {get; set;} = Rgba32.White;
        public Rgba32 OverlayTextColor {get; set;} = Rgba32.White;
		public bool HasHeader {get; set;} = true;
        public bool ShowTimeStamp {get; set;} = true;
        public bool ShowAdvertisement {get; set;} = true;
    }
}
