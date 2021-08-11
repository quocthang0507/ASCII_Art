using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace ArtLib
{
	public static class BitmapExtension
	{
		private static readonly string[] AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };
		private const string BLACK = "@";
		private const string CHARCOAL = "#";
		private const string DARKGRAY = "8";
		private const string MEDIUMGRAY = "&";
		private const string MEDIUM = "o";
		private const string GRAY = ":";
		private const string SLATEGRAY = "*";
		private const string LIGHTGRAY = ".";
		private const string WHITE = " ";

		public static Bitmap ResizedImage(this Bitmap originalBitmap, int asciiWidth = 0, int asciiHeight = 0)
		{
			if (asciiWidth > 0 && asciiHeight == 0)
			{
				asciiHeight = (int)Math.Ceiling((double)originalBitmap.Height * asciiWidth / originalBitmap.Width);
			}
			else if (asciiHeight > 0 && asciiWidth == 0)
			{
				asciiWidth = (int)Math.Ceiling((double)originalBitmap.Width * asciiHeight / originalBitmap.Height);
			}
			else if (asciiHeight == 0 && asciiWidth == 0)
			{
				asciiHeight = originalBitmap.Height;
				asciiWidth = originalBitmap.Width;
			}
			Bitmap result = new(asciiWidth, asciiHeight);
			Graphics graphics = Graphics.FromImage((Image)result);
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(originalBitmap, 0, 0, asciiWidth, asciiHeight);
			graphics.Dispose();
			return result;
		}

		/// <summary>
		/// https://www.c-sharpcorner.com/article/generating-ascii-art-from-an-image-using-C-Sharp/
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static string ToAscii(this Bitmap image, int charset = 1)
		{
			bool toggle = false;
			StringBuilder stringBuilder = new();
			for (int h = 0; h < image.Height; h++)
			{
				for (int w = 0; w < image.Width; w++)
				{
					Color pixelColor = image.GetPixel(w, h);
					int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					Color grayColor = Color.FromArgb(red, green, blue);

					if (!toggle)
					{
						string @char = string.Empty;
						if (charset == 1)
						{
							@char = AsciiChars[(grayColor.R * 10) / 255];
						}
						else if (charset == 2)
						{
							@char = GetGrayShade(grayColor.R);
						}
						stringBuilder.Append(@char);
					}
				}
				if (!toggle)
				{
					stringBuilder.AppendLine();
					toggle = true;
				}
				else
				{
					toggle = false;
				}
			}
			return stringBuilder.ToString();
		}

		public static void ToAsciiFile(this Bitmap image, int charset, string filename)
		{
			string ascii = ToAscii(image, charset);
			Console.WriteLine("Đã tạo xong nội dung ASCII từ hình ảnh.");
			try
			{
				File.WriteAllText(filename, ascii);
				Console.WriteLine("Đã ghi xong nội dung ASCII vào tập tin {0}", filename);
			}
			catch (Exception)
			{
				Console.WriteLine("Lỗi: Không thể chuyển hình ảnh thành tập tin văn bản được. Có thể từ tên tập tin đầu ra không hợp lệ. Hãy thử lại với tên khác.");
			}
		}

		private static string GetGrayShade(int red)
		{
			return red switch
			{
				>= 230 => WHITE,
				>= 200 => LIGHTGRAY,
				>= 180 => SLATEGRAY,
				>= 160 => GRAY,
				>= 130 => MEDIUM,
				>= 100 => MEDIUMGRAY,
				>= 70 => DARKGRAY,
				>= 50 => CHARCOAL,
				_ => BLACK,
			};
		}
	}
}
