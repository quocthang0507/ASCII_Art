using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ArtLib
{
	/// <summary>
	/// Validate byte[] of array is belonged to an image
	/// </summary>
	public static class ImageValidation
	{
		public static bool IsValidImage(string filePath)
		{
			if (File.Exists(filePath))
			{
				try
				{
					Image image = new Bitmap(filePath);
					ImageFormat ext = GetImageFormat(image);
					if (ext == null)
						throw new Exception();
					return true;
				}
				catch (Exception)
				{
					Console.WriteLine("Lỗi: Đường dẫn đến tập tin không phải là hình ảnh.");
					return false;
				}
			}
			Console.WriteLine("Lỗi: Không tìm thấy tập tin với đường dẫn này.");
			return false;
		}

		public static bool IsValidImage(this byte[] image)
		{
			ImageFormat format = GetImageFormat(image);
			return format == ImageFormat.Jpeg || format == ImageFormat.Png;
		}

		/// <summary>
		/// http://www.mikekunz.com/image_file_header.html  
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(byte[] bytes)
		{
			byte[] bmp = Encoding.ASCII.GetBytes("BM");
			byte[] gif = Encoding.ASCII.GetBytes("GIF");
			byte[] png = new byte[] { 137, 80, 78, 71 };
			byte[] tiff = new byte[] { 73, 73, 42 };
			byte[] tiff2 = new byte[] { 77, 77, 42 };
			byte[] jpeg = new byte[] { 255, 216, 255, 224 };
			byte[] jpeg2 = new byte[] { 255, 216, 255, 225 };

			if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
				return ImageFormat.Bmp;
			else if (gif.SequenceEqual(bytes.Take(gif.Length)))
				return ImageFormat.Gif;
			else if (png.SequenceEqual(bytes.Take(png.Length)))
				return ImageFormat.Png;
			else if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
				return ImageFormat.Tiff;
			else if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
				return ImageFormat.Tiff;
			else if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
				return ImageFormat.Jpeg;
			else if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
				return ImageFormat.Jpeg;

			return null;
		}

		public static ImageFormat GetImageFormat(Image image)
		{
			if (ImageFormat.Bmp.Equals(image.RawFormat))
				return ImageFormat.Bmp;
			else if (ImageFormat.Gif.Equals(image.RawFormat))
				return ImageFormat.Gif;
			else if (ImageFormat.Png.Equals(image.RawFormat))
				return ImageFormat.Png;
			else if (ImageFormat.Tiff.Equals(image.RawFormat))
				return ImageFormat.Tiff;
			else if (ImageFormat.Jpeg.Equals(image.RawFormat))
				return ImageFormat.Jpeg;

			return null;
		}
	}
}
