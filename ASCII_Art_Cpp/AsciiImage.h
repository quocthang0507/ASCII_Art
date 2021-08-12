#pragma once

#define MAX_SIZE 1000

/// <summary>
/// https://github.com/puru1796/Image-to-ASCII-Art
/// </summary>
class AsciiImage
{
public:
	int width;
	int height;
	char ASCII[MAX_SIZE][MAX_SIZE];
	int i = 0;
	int j = 0;
	double maxGrayScale = 0.3 * (255.0 / 256.0) + 0.6 * (255.0 / 256.0) + 0.11 * (255.0 / 256.0);

	AsciiImage(char* filename);
	void PrintImage(void);
	void ToFile(const char* filename);
private:
	void SetArray(double);
	int GetPadding(int);
};

