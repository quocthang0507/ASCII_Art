import sys
import os
import imghdr
import math
import PIL

from PIL import Image

AsciiChars = ["#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " "]
BLACK = "@"
CHARCOAL = "#"
DARKGRAY = "8"
MEDIUMGRAY = "&"
MEDIUM = "o"
GRAY = ":"
SLATEGRAY = "*"
LIGHTGRAY = "."
WHITE = " "


def is_valid_image(filename: str):
    if os.path.exists(filename):
        try:
            ext = imghdr.what(filename)
            if ext == None:
                raise Exception()
            elif ext == 'jpeg' or ext == 'bmp' or ext == 'png' or ext == 'webp' or ext == 'gif':
                return True
        except:
            print('Lỗi: Đường dẫn đến tập tin không phải là hình ảnh.')
            return False
    else:
        print('Lỗi: Không tìm thấy tập tin với đường dẫn này.')
        return False


def int_try_parse(string: str):
    try:
        return int(string)
    except:
        return None


def resize_image(originalBitmap: Image, asciiWidth: int = 0, asciiHeight: int = 0):
    width, height = originalBitmap.size
    if asciiWidth > 0 and asciiHeight == 0:
        asciiHeight = math.ceil(float(height) * asciiWidth / width)
    elif asciiWidth > 0 and asciiWidth == 0:
        asciiWidth = math.ceil(float(width) * asciiHeight / height)
    elif asciiWidth == 0 and asciiHeight == 0:
        asciiWidth, asciiHeight = width, height
    resized = originalBitmap.resize(
        (asciiWidth, asciiHeight), PIL.Image.NEAREST)
    return resized


def image_to_ascii(image: Image, charset: int = 1):
    toggle = False
    image = image.convert('RGB')
    result = []  # line by line
    line = []
    width, height = image.size
    for h in range(height):
        for w in range(width):
            pixel_color = image.getpixel((w, h))
            red = int(sum(pixel_color) / 3)
            green = int(sum(pixel_color) / 3)
            blue = int(sum(pixel_color) / 3)
            gray_color = (red, green, blue)

            if not toggle:
                char = ''
                if charset == 1:
                    char = AsciiChars[int((gray_color[0] * 10) / 255)]
                elif charset == 2:
                    char = get_gray_shade(gray_color[0])
                line.append(char)
        if not toggle:
            result.append(line)
            line = []
            toggle = True
        else:
            toggle = False
    return result


def get_gray_shade(red: int):
    if red >= 230:
        return WHITE
    elif red >= 200:
        return LIGHTGRAY
    elif red >= 180:
        return SLATEGRAY
    elif red >= 160:
        return GRAY
    elif red >= 130:
        return MEDIUM
    elif red >= 100:
        return MEDIUMGRAY
    elif red >= 70:
        return DARKGRAY
    elif red >= 50:
        return CHARCOAL
    else:
        return BLACK


def image_to_ascii_file(image: Image, charset: int, filename: str):
    ascii = image_to_ascii(image, charset)
    print('Đã tạo xong nội dung ASCII từ hình ảnh.')
    try:
        with open(filename, 'w') as file:
            for line in ascii:
                file.write(''.join(line))
                file.write('\n')
        print('Đã ghi xong nội dung ASCII vào tập tin {}'.format(filename))
    except:
        print('Lỗi: Không thể chuyển hình ảnh thành tập tin văn bản được. Có thể từ tên tập tin đầu ra không hợp lệ. Hãy thử lại với tên khác.')


def get_agrument_by_name(arguments: list, name: str):
    index = -1
    for i in range(len(arguments)):
        if arguments[i].lower() == name.lower():
            index = i
            break
    return None if index == -1 or index + 1 >= len(arguments) else arguments[index + 1]


if __name__ == "__main__":
    arguments = sys.argv
    if len(arguments) == 1 or arguments[1].lower() == '-h':
        print("Vui lòng thêm các đối số vào sau chương trình này.")
        print(
            "\npython program.py -i \"đường dẫn đến tập tin hình ảnh\" -o \"đường dẫn đến tập tin văn bản\" [-c mã_bộ_ký_tự] [-w chiều_rộng] [-h chiều_cao]\n")
        print("Trong đó:")
        print("-i tên hoặc đường dẫn đến tập tin hình ảnh bắt buộc.")
        print("-o tên hoặc đường dẫn đến tập tin văn bản đầu ra bắt buộc.")
        print("[-c mã_bộ_ký_tự] là 1 hoặc 2. Mặc định là 1.")
        print("[-w chiều_rộng] là chiều rộng của ASCII đầu ra. Mặc định là chiều rộng của hình, đơn vị pixel.")
        print("[-h chiều_cao] là chiều cao của ASCII đầu ra. Mặc định là chiều cao của hình, đơn vị pixel. ")
        print("Nếu một trong chiều cao và chiều rộng không được chỉ định hoặc bằng 0 thì kích thước sẽ thay đổi theo tỷ lệ mới này.")
        exit()

    print('CHƯƠNG TRÌNH VẼ TRANH ASCII TỪ HÌNH ẢNH')

    inputArg = get_agrument_by_name(arguments, "-i")
    outputArg = get_agrument_by_name(arguments, "-o")
    charsetArg = get_agrument_by_name(arguments, "-c")
    widthArg = get_agrument_by_name(arguments, "-w")
    heightArg = get_agrument_by_name(arguments, "-h")

    if inputArg == None or outputArg == None:
        print('Lỗi: Không được bỏ trống đối số -i và -o.')
    else:
        is_valid = is_valid_image(inputArg)
        if is_valid:
            width = int_try_parse(widthArg)
            height = int_try_parse(heightArg)
            charset = int_try_parse(charsetArg)
            if width == None:
                width = 0
            if height == None:
                height = 0
            if charset == None:
                charset = 1
            image = Image.open(inputArg)
            image = resize_image(image, width, height)
            image_to_ascii_file(image, charset, outputArg)
