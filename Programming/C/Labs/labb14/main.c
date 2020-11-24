#include <stdio.h>
#include <math.h>
#include <mm_malloc.h>

/*          Struct of BMP file
 *  Name        Length (bytes)      Description
 *  -----------------------------------------------------
 *  BitMapFileHeader
     *  Type        2           signature "BM"
     *  Size        4           sizeof(file)
     *  Reserved1   2           reserve (should be all 0)
     *  Reserved2   2           reserve (should be all 0)
     *  OffsetBits  4           image offset from the beginning of the file
     *  ---
     *  Total: 14 bytes
 * BitMapInfoHeader
     *  Size        4           size of header
     *  Width       4           ::: 18 - 21 bytes
     *  Height      4           ::: 22 - 25 bytes
     *  Planes      2           in 99% -> 1
     *  BitCount    2           number of bits on the one pixel ::: (now he is equal to 1)
     *  Compression 4           in 99% -> 0
     *  SizeImage   4           if (!compression) -> 0
     *  XpelsPerMeter 4         horizontal resolution of the dot on one meter
     *  YpelsPerMeter 4         vertical resolution of the dot on one meter
     *  ColorsUsed  4
     *  ColorsImportant 4
     *  ---
     *  Total: 40 bytes
        * :::Total to header: 54 bytes
 * ColorTable
     * ColorTable
 * BitMapArray
     * Image Size
 * */

int friends(int i, int j, unsigned char **Colors, int h, int w) {
    int lives = 0;
    for(int i1 = i - 1; i1 <= i + 1; i1++) {
        for(int j1 = j - 1; j1 <= j + 1; j1++) {
            if(i == i1 && j == j1) {
                continue;
            }
            lives += ((Colors[(i1 + h) % h][(j1 + w) % w] == '1') ? (1) : (0));
        }
    }
    return lives;
}

void next(int h, int w, unsigned char **Colors) {
    unsigned char new_area[h][w];
    for (int i = h - 1; i >= 0; i--) {
        for (int j = 0; j < w; j++) {
            int live = friends(i, j, Colors, h, w);
            if(Colors[i][j] == '1' && !(live == 2 || live == 3)) {
                new_area[i][j] = '0';
            }
            else if(Colors[i][j] == '0' && live == 3) {
                new_area[i][j] = '1';
            }
            else {
                new_area[i][j] = Colors[i][j];
            }
        }
    }

    for(int i = h - 1; i >= 0; i--) {
        for (int j = 0; j < w; j++) {
            Colors[i][j] = new_area[i][j];
        }
    }
}
struct pall{
    unsigned char R;
    unsigned char G;
    unsigned char B;
    unsigned char A;
};

void image(unsigned char **Colors, struct pall Palette[], int h, int w, char* out, unsigned char header[54], int size_Palette) {
    FILE *output;
    output = fopen(out, "w");
    for(int i = 0; i < 54; i++) {
        fprintf(output, "%c", header[i]);
    }
    for(int i = 0; i < (size_Palette - 1) / 4; i++) {
        fprintf(output, "%c%c%c%c", Palette[i].R, Palette[i].G, Palette[i].B, Palette[i].A);
    }
    for(int i = h - 1; i >= 0; i--) {
        for(int j = 0; j < w; j += 8) {
            int sum = 0;
            for(int q = 0; q <= 8; q++) {
                int tmp = (Colors[i][j + q] == '1') ? (1) : (0);
                sum += (int) tmp * (int) pow(2, 7 - q);
            }
            unsigned char xx = sum;
            fprintf(output, "%c", xx);
        }
    }
    fclose(output);
}

int main(/*int argc, char *argv[]*/) {
    
    FILE *my_picture;
    my_picture = fopen("picture.bmp", "r");

    // * Check my File
    if(my_picture == NULL) {
        printf("Not such file in directory\n");
        return 0;
    }

    /*
     * Take BitMapFileHeader + BitMapInfoHeader and add this info
     *  to header[sizeof(BitMapFileHeader + BitMapInfoHeader) = 54]
     */
    unsigned char header[54];
    fread(header, sizeof(unsigned char), sizeof(header),  my_picture);
    int width;
    width = (int) (header[21] * pow(256, 3) +
            header[20] * pow(256, 2) +
            header[19] * pow(256, 1) +
            header[18] * pow(256, 0));
    int height;
    height = (int) (header[25] * pow(256, 3) +
            header[24] * pow(256, 2) +
            header[23] * pow(256, 1) +
            header[22] * pow(256, 0));

    /*
     * I want to save my colors in my array
     * But in image we have HEX codes and we need binary code
     * In [0] - [7] I will save: ((int) (HEX)) % 2 -> ((int) (HEX)) / 2 -> ((int) (HEX)) % 2 -> ... -> 0
     */
    width *= 8;
    /*
     * My Image starts from 63 byte
     * Let's fill 54 - 63 bytes in Palette
     */
    int OffsetBits;
    OffsetBits = (int) (header[13] * pow(256, 3) +
           header[12] * pow(256, 2) +
           header[11] * pow(256, 1) +
           header[10] * pow(256, 0));
    
    struct pall Palette[(OffsetBits - 54) + 1];
    for(int i = 0; i < (sizeof(Palette) - 1) / 4; i++) {
        unsigned char bytes[4];
        fread(bytes, sizeof(unsigned char), sizeof(bytes), my_picture);
        Palette[i].R = bytes[0];
        Palette[i].G = bytes[1];
        Palette[i].B = bytes[2];
        Palette[i].A = bytes[3];
    }

    /*
     * Image from the left to the right,
     * from the bottom to top
     */
    unsigned char **Colors;
    Colors = (unsigned char **) malloc(height * sizeof(unsigned char*));
    for(int i = 0; i < height; i++) {
        Colors[i] = (unsigned char *)malloc(width * sizeof(unsigned char));
    }
    
    
    // * Filling
    for(int i = height - 1; i >= 0; i--) {
        for (int j = 0; j < width; j += 8) {
            unsigned char byte[1];
            fread(byte, sizeof(unsigned char), sizeof(byte), my_picture);
            int byte_ = byte[0];
            int move = 7;
            // * Saving colors
            while(move >= 0) {
                Colors[i][j + move] = ((byte_ % 2) ? ('1') : ('0'));
                byte_ /= 2;
                move--;
            }
        }
    }

    /*
     * New generations
     */
//    for(int i = height - 1; i >= 0; i--) {
//        for(int j = 0; j < width; j++) {
//            printf("%c", Colors[i][j]);
//        }
//        printf("\n");
//    }
    int gen = 0;
    for(int i = 0; i < gen; i++) {
        next(height, width, Colors);
    }
    image(Colors, Palette, height, width, "kek.bmp", header, (int) sizeof(Palette));
    fclose(my_picture);
}
