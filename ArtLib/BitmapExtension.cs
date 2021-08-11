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

		public static string ToAscii(this Bitmap image)
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
						int index = (grayColor.R * 10) / 255;
						stringBuilder.Append(AsciiChars[index]);
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

		public static void ToAsciiFile(this Bitmap image, string filename)
		{
			string ascii = ToAscii(image);
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
	}
}
