using ArtLib;
using System;
using System.Drawing;
using System.Text;

namespace ASCII_Art_CSharp
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;
			string[] arguments = Environment.GetCommandLineArgs();
			if (arguments.Length == 1 || arguments[1].Equals("-H", StringComparison.OrdinalIgnoreCase))
			{
				Console.WriteLine("Vui lòng thêm các đối số vào sau chương trình này.");
				Console.WriteLine("\nAsciiArt -i \"đường dẫn đến tập tin hình ảnh\" -o \"đường dẫn đến tập tin văn bản\" [-c mã_bộ_ký_tự] [-w chiều_rộng] [-h chiều_cao]\n");
				Console.WriteLine("Trong đó:");
				Console.WriteLine("-i tên hoặc đường dẫn đến tập tin hình ảnh bắt buộc.");
				Console.WriteLine("-o tên hoặc đường dẫn đến tập tin văn bản đầu ra bắt buộc.");
				Console.WriteLine("[-c mã_bộ_ký_tự] là 1 hoặc 2. Mặc định là 1.");
				Console.WriteLine("[-w chiều_rộng] là chiều rộng của ASCII đầu ra. Mặc định là chiều rộng của hình, đơn vị pixel.");
				Console.WriteLine("[-h chiều_cao] là chiều cao của ASCII đầu ra. Mặc định là chiều cao của hình, đơn vị pixel. ");
				Console.WriteLine("Nếu một trong chiều cao và chiều rộng không được chỉ định hoặc bằng 0 thì kích thước sẽ thay đổi theo tỷ lệ mới này.");
				return 1;
			}
			Console.WriteLine("CHƯƠNG TRÌNH VẼ TRANH ASCII TỪ HÌNH ẢNH");

			string inputArg = GetAgrumentByName(arguments, "-i");
			string outputArg = GetAgrumentByName(arguments, "-o");
			string charsetArg = GetAgrumentByName(arguments, "-c");
			string widthArg = GetAgrumentByName(arguments, "-w");
			string heightArg = GetAgrumentByName(arguments, "-h");

			if (inputArg == string.Empty || outputArg == string.Empty)
			{
				Console.WriteLine("Lỗi: Không được bỏ trống đối số -i và -o.");
			}
			else
			{
				bool isValid = ImageValidation.IsValidImage(inputArg);
				if (isValid)
				{
					int width, height, charset;
					if (widthArg == string.Empty || !int.TryParse(widthArg, out width))
					{
						width = 0;
					}
					if (heightArg == string.Empty || !int.TryParse(heightArg, out height))
					{
						height = 0;
					}
					if (charsetArg == string.Empty || !int.TryParse(charsetArg, out charset))
					{
						charset = 1;
					}
					Bitmap image = new(inputArg, true);
					image = image.ResizedImage(width, height);
					image.ToAsciiFile(charset, outputArg);
				}
			}
			return 0;
		}

		static string GetAgrumentByName(string[] args, string name)
		{
			int index = Array.FindIndex(args, a => a.Equals(name, StringComparison.OrdinalIgnoreCase));
			return index == -1 || index + 1 >= args.Length ? string.Empty : args[index + 1];
		}
	}
}
