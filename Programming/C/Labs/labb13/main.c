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

struct pair {
    int first;
    int second;
};

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

struct pair check (const byte* arr, char* value, int n) {
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
                struct pair fr = {i - 10, tmpSize};
                return fr;
            }
            i += tmpSize;
        }
        free(frame);
        free(name);
    }
    struct pair none = {0, 0};
    return none;
}

byte* edit (byte* header, byte* arr, char* tag, char* value, int n) {
    struct pair exist = check(arr, tag, n);
    if (exist.second) {
        int newFramesSize = n - exist.second + (int) strlen(value);
        byte* newArr = (byte*) malloc (newFramesSize);

        // updateHeader().size
        byte binary[32] = {0};
        int t = 31, tmp = newFramesSize;
        while (tmp) {
            binary[t] = tmp % 2;
            tmp = tmp>>1;
            t--;
        }
        t = 31;
        for (int i = 7; i >= 4; i++) {
            int hex = 0, p = 1;
            for (int j = 0; j < 7; j++) {
                hex += binary[t] * p;
                p *= 2;
                t--;
            }
            header[i] = hex;
        }

        for (int i = 0; i < exist.first; i++) {
            // rewrite tags before
            newArr[i] = arr[i];
        }
        // update frameHeader
        for (int i = exist.first; i < 4 + exist.first; i++) {
            // rewrite name
            newArr[i] = arr[i];
        }
        int newLen = (int) strlen(value);
        for (int i = exist.first + 7; i >= exist.first + 4; i--) {
            // new len of frame
            newArr[i] = newLen % 256;
            newLen /= 256;
        }
        for (int i = exist.first + 8; i < exist.first + 10; i++) {
            // rewrite flags
            newArr[i] = arr[i];
        }
        for (int i = exist.first + 10; i < exist.first + 10 + strlen(value); i++) {
            // rewrite value
            newArr[i] = value[i - exist.first - 10];
        }

        // rewrite other tags
        // ...
        // ...

        return newArr;
    }
    else {
        return arr;
    }
}

int main() {
    FILE *song = fopen("kek.mp3", "r");
    if (song == NULL) {
        printf("%s", "No such file!");
        return 0;
    }
    byte* headerID3 = (byte*) malloc(10);
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
    fclose(song);
}
