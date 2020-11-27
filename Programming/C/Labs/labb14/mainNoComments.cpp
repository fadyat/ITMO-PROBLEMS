#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>

int friends(int i, int j, unsigned char **Colors, int h, int w) {
    int lives = 0;
    for (int i1 = i - 1; i1 <= i + 1; i1++) {
        for (int j1 = j - 1; j1 <= j + 1; j1++) {
            if (i == i1 && j == j1) {
                continue;
            }
            lives += ((Colors[(i1 + h) % h][(j1 + w) % w] == '1') ? (1) : (0));
        }
    }
    return lives;
}

void next(int h, int w, int realWidth, unsigned char **Colors) {
    unsigned char new_area[h][w];
    for (int i = h - 1; i >= 0; i--) {
        for (int j = 0; j < realWidth; j++) {
            int live = friends(i, j, Colors, h, realWidth);
            if (Colors[i][j] == '1' && !(live == 2 || live == 3)) {
                new_area[i][j] = '0';
            }
            else if (Colors[i][j] == '0' && live == 3) {
                new_area[i][j] = '1';
            }
            else {
                new_area[i][j] = Colors[i][j];
            }
        }
    }

    for (int i = h - 1; i >= 0; i--) {
        for (int j = 0; j < w; j++) {
            Colors[i][j] = new_area[i][j];
        }
    }
}

struct pall {
    unsigned char R;
    unsigned char G;
    unsigned char B;
    unsigned char A;
};

void image(unsigned char **Colors, struct pall Palette[],
           int h, int w, char *out, unsigned char header[54], int PalSz) {
    FILE *output;
    output = fopen(out, "w");
    for (int i = 0; i < 54; i++) {
        fprintf(output, "%c", header[i]);
    }
    for (int i = 0; i < PalSz; i++) {
        fprintf(output, "%c%c%c%c", Palette[i].R, Palette[i].G, Palette[i].B, Palette[i].A);
    }
    for (int i = h - 1; i >= 0; i--) {
        for (int j = 0; j < w; j += 8) {
            int sum = 0;
            for (int q = 0; q < 8; q++) {
                int tmp = (Colors[i][j + q] == '1') ? (1) : (0);
                sum += tmp * (int) pow(2, 7 - q);
            }
            unsigned char xx = sum;
            fprintf(output, "%c", xx);
        }
    }
    fclose(output);
}

int main(int argc, char *argv[]) {
    FILE *my_picture;
    char *directory;
    int gen = 0, freq = 1;
    for (int i = 1; i < argc; i += 2) {
        if (!strcmp(argv[i], "--input")) {
            my_picture = fopen(argv[i + 1], "r");
        }
        else if (!strcmp(argv[i], "--max_iter")) {
            char *end;
            gen = (int) strtol(argv[i + 1], &end, 10);
        }
        else if (!strcmp(argv[i], "--dump_freq")) {
            char *end;
            freq = (int) strtol(argv[i + 1], &end, 10);
        }
        else if (!strcmp(argv[i], "--output")) {
            directory = argv[i + 1];
            mkdir(directory, 0777);
        }
    }
    if (my_picture == NULL) {
        printf("Not such file in directory\n");
        return 0;
    }
    unsigned char header[54];
    fread(header, 1, 54, my_picture);
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
    int OffsetBits;
    OffsetBits = (int) (header[13] * pow(256, 3) +
                        header[12] * pow(256, 2) +
                        header[11] * pow(256, 1) +
                        header[10] * pow(256, 0));

    struct pall Palette[(OffsetBits - 54) / 4];
    for (int i = 0; i < (OffsetBits - 54) / 4; i++) {
        unsigned char bytes[4];
        fread(bytes, 1, 4, my_picture);
        Palette[i].R = bytes[0];
        Palette[i].G = bytes[1];
        Palette[i].B = bytes[2];
        Palette[i].A = bytes[3];
    }
    unsigned char **Colors;
    Colors = (unsigned char **) malloc(height * sizeof(unsigned char *));
    for (int i = 0; i < height; i++) {
        Colors[i] = (unsigned char *) malloc(width * sizeof(unsigned char *));
    }
    int realWidth = width;
    width += 32 - width % 32;
    for (int i = height - 1; i >= 0; i--) {
        for (int j = 0; j < width; j += 8) {
            unsigned char byte[1];
            fread(byte, 1, 1, my_picture);
            int byte_ = byte[0];
            int step = 7;
            while (step >= 0) {
                Colors[i][j + step] = ((byte_ % 2) ? ('1') : ('0'));
                byte_ /= 2;
                step--;
            }
        }
    }
    for (int i = 1; i <= gen; i++) {
        next(height, width, realWidth, Colors);
        if (i % freq == 0) {
            char out[100];
            strcpy(out, directory);
            char num[17];
            strcat(out, "//kek");
            sprintf(num, "%d", i);
            strcat(out, num);
            strcat(out, ".bmp");
            image(Colors, Palette, height, width, out, header, (OffsetBits - 54) / 4);
        }
    }
    fclose(my_picture);
}
