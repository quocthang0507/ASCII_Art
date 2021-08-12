#include <iostream>
#include <fstream>

using namespace std;

#include "AsciiImage.h"

AsciiImage::AsciiImage(char* filename)
{
	ifstream fin;
	fin.open(filename, ios::binary);
	unsigned char* info;
	info = new unsigned char[54];
	fin.read((char*)info, 54);
	width = *(int*)&info[18];
	height = *(int*)&info[22];

	int padding = GetPadding(width);
	unsigned char* pixel = new unsigned char[padding];
	char temp;
	while (i < height)
	{
		fin.read((char*)pixel, padding);
		for (j = 0; j < width * 3; j += 3) {
			temp = pixel[j];
			pixel[j] = pixel[j + 2];
			pixel[j + 2] = temp;
			double grayScale = 0.3 * ((int)pixel[j] / 256.0) + 0.6 * ((int)pixel[j + 1] / 256.0) + 0.11 * ((int)pixel[j + 2] / 256.0);
			SetArray(grayScale);
		}
		i++;
	}

	fin.close();
	cout << "Da khoi tao thanh cong!\n";
}

void AsciiImage::PrintImage(void)
{
	for (int i = height - 1; i >= 0; i--) {
		for (int j = 0; j < width * 3; j += 3) {
			if (ASCII[i][j] == '[') {
				ASCII[i][j] = 'z';
			}
			cout << ASCII[i][j];
		}
		cout << endl;
	}
}

void AsciiImage::ToFile(const char* filename)
{
	ofstream fout;
	fout.open(filename);
	for (int i = height - 1; i >= 0; i--) {
		for (int j = 0; j < width * 3; j += 3) {
			if (ASCII[i][j] == '[') {
				ASCII[i][j] = 'z';
			}
			fout << ASCII[i][j];
		}
		fout << endl;
	}
	fout.close();
	cout << "Xuat thanh cong!\n";
}

int AsciiImage::GetPadding(int width)
{
	return (((width * 3 + 3) / 4) * 4);
}

void AsciiImage::SetArray(double gray_scale)
{
	if (gray_scale > 0.6) {
		ASCII[i][j] = (char)(65 + (gray_scale - 0.6) * (26 / (maxGrayScale - 0.6)));
	}
	else if (gray_scale >= 0.3) {
		ASCII[i][j] = (char)(97 + (gray_scale - 0.3) * (26 / 0.3));
	}
	else {
		ASCII[i][j] = (char)(32 + (gray_scale * (16 / 0.3)));
	}
}

