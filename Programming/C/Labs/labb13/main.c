#include <stdio.h>
#include <malloc.h>
#include <string.h>

typedef unsigned char byte;

/*
 *  Struct header of Id3:    Struct of frame-header:
 *
 *  0 1 2 3 4 5 6 7 8 9      0 1 2 3 4 5 6 7 8 9
 *  . . . . . . . . . .      . . . . . . . . . .
 *
 *  0 - 2: ID3               0 - 3: name (frame ID)
 *  3: version               4 - 7: size of frame ID
 *  4: subversion            8 - 9: flags
 *  5: flags
 *  6 - 9: size
 *
 */

void show (const byte *arr, int n);
void showValue (const byte*, char* value, int n);

int main() {
    FILE *song = fopen("kek.mp3", "r");
    if (song == NULL) {
        printf("%s", "No such file!");
        return 0;
    }
    byte headerID3[10];
    fread(headerID3,  1, 10, song);

    byte totalSize[28] = {0};
    for (int i = 9; i >= 6; i--) {
        byte tmp = headerID3[i];
        int t = 6;
        while (tmp) {
            totalSize[(i - 6) * 7 + t] = tmp % 2;
            tmp = tmp >> 1;
            t--;
        }
    }

    int framesSize = 0, p = 1;
    for (int i = 27; i >= 0; i--) {
        framesSize += totalSize[i] * p;
        p *= 2;
    }
    byte *frames = (byte*) malloc(framesSize);
    fread(frames, 1, framesSize, song);

    show(frames, framesSize);

    showValue(frames, "TPE1", framesSize);

    fclose(song);
}

void show (const byte *arr, int n) {
    printf("\n");
    int i = 0;
    while (i < n) {
        byte* frame = (byte*) malloc(9);
        for (int j = 0; j < 10; j++) {
            frame[j] = arr[i + j];
        }
        i += 10;
        int p = 1, tmpSize = 0;
        for (int j = 7; j > 3; j--) {
            tmpSize += frame[j] * p;
            p *= 256;
        }
        if (tmpSize != 0) {
            for (int j = 0; j < 4; j++) {
                printf("%c", frame[j]);
            }
            printf(": ");
            for (int j = i; j < tmpSize + i; j++) {
                printf("%c", arr[j]);
            }
            printf("\n");
            i += tmpSize;
        }
        free(frame);
    }
}

void showValue (const byte* arr, char* value, int n) {
    printf("\n");
    int i = 0;
    while (i < n) {
        byte* frame = (byte*) malloc(10);
        for (int j = 0; j < 10; j++) {
            frame[j] = arr[i + j];
        }
        i += 10;
        int p = 1, tmpSize = 0;
        for (int j = 7; j > 3; j--) {
            tmpSize += frame[j] * p;
            p *= 256;
        }
        char* name = (char*) malloc(5);
        for (int j = 0; j < 5; j++) {
            name[j] = frame[j];
        }
        if (tmpSize != 0) {
            if (!strcmp(name, value)) {
                for (int j = 0; j < 4; j++) {
                    printf("%c", frame[j]);
                }
                printf(": ");
                for (int j = i; j < tmpSize + i; j++) {
                    printf("%c", arr[j]);
                }
                printf("\n");
                return;
            }
            i += tmpSize;
        }
        free(frame);
        free(name);
    }
    printf("Tag with name: < %s > doesn't exist", value);
}
