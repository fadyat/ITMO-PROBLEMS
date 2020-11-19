#include <stdio.h>
#include <math.h>

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
     * ColorTable 4
 * BitMapArray
     * Image Size
 * */

int main(int argc, char *argv[]) {

    FILE *my_picture;
    my_picture = fopen("testt.bmp", "rb");

    while(my_picture == NULL) {
        printf("Not such file in directory\n");
        return 0;
    }
    unsigned char Header[54];
    /*
     * Take BitMapFileHeader + BitMapInfoHeader and add this info
     *  to Header[sizeof(BitMapFileHeader + BitMapInfoHeader) = 54]
     */
    fread(Header, sizeof(unsigned char), sizeof(Header),  my_picture);
    int width;
    width = (int) (Header[21] * pow(256, 3) +
            Header[20] * pow(256, 2) +
            Header[19] * pow(256, 1) +
            Header[18] * pow(256, 0));
    int height;
    height = (int) (Header[25] * pow(256, 3) +
            Header[24] * pow(256, 2) +
            Header[23] * pow(256, 1) +
            Header[22] * pow(256, 0));
    printf("%d %d\n", height, width);
    unsigned char Colors[height][width];
    /*
     * Image from the left to the right,
     * from the bottom to top
     */
    for(int i = height - 1; i >= 0; i--) {
        for (int j = 0; j < width; j++) {
            unsigned char RGB[3];
            fread(RGB, sizeof(unsigned char), sizeof(RGB), my_picture);
            // check
            Colors[i][j] = ((RGB[0] == 255) ? (0) : (1));
        }
    }
//    for(int i = height - 1; i >= 0; i--) {
//        for (int j = 0; j < width; j++) {
//            printf("%d", Colors[i][j]);
//        }
//        printf("\n");
//    }
}