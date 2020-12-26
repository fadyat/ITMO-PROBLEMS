#include <stdio.h>
#include <mm_malloc.h>
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

void show(const byte *arr, int n) {
    printf("\n");
    int i = 0;
    while (i < n) {
        byte *frame = (byte *) malloc(10);
        for (int j = 0; j < 10; j++) {
            frame[j] = arr[i + j];
        }
        i += 10;
        int p = 1, tmpSize = 0;
        for (int j = 7; j > 3; j--) {
            tmpSize += frame[j] * p;
            p *= 256;
        }
        if (tmpSize && i + tmpSize < n) {
            for (int j = 0; j < 4; j++) {
                printf("%c", frame[j]);
            }
            printf(": ");
            for (int j = i; j < tmpSize + i; j++) {
                printf("%c", arr[j]);
            }
            printf("\n");
            i += tmpSize;
        } else {
            free(frame);
            printf("\n");
            return;
        }
        free(frame);
    }
}

void showValue(const byte *arr, char *value, int n) {
    printf("\n");
    int i = 0;
    while (i < n) {
        byte *frame = (byte *) malloc(10);
        for (int j = 0; j < 10; j++) {
            frame[j] = arr[i + j];
        }
        i += 10;
        int p = 1, tmpSize = 0;
        for (int j = 7; j > 3; j--) {
            tmpSize += frame[j] * p;
            p *= 256;
        }
        char *name = (char *) malloc(5);
        for (int j = 0; j < 5; j++) {
            name[j] = frame[j];
        }
        if (tmpSize && i + tmpSize < n) {
            if (!strcmp(name, value)) {
                for (int j = 0; j < 4; j++) {
                    printf("%c", frame[j]);
                }
                printf(": ");
                for (int j = i; j < tmpSize + i; j++) {
                    printf("%c", arr[j]);
                }
                printf("\n\n");
                return;
            }
            i += tmpSize;
        } else {
            free(frame);
            free(name);
            return;
        }
        free(frame);
        free(name);
    }
    printf("Tag with name: < %s > doesn't exist\n", value);
}

struct pair check(const byte *arr, char *value, int n) {
    int i = 0;
    while (i < n) {
        byte *frame = (byte *) malloc(10);
        for (int j = 0; j < 10; j++) {
            frame[j] = arr[i + j];
        }
        i += 10;
        int p = 1, tmpSize = 0;
        for (int j = 7; j > 3; j--) {
            tmpSize += frame[j] * p;
            p *= 256;
        }
        char *name = (char *) malloc(5);
        for (int j = 0; j < 5; j++) {
            name[j] = frame[j];
        }
        if (tmpSize != 0 && i + tmpSize < n) {
            if (!strcmp(name, value)) {
                struct pair fr = {i - 10, tmpSize};
                return fr;
            }
            i += tmpSize;
        } else {
            free(frame);
            free(name);
            break;
        }
        free(frame);
        free(name);
    }
    struct pair none = {0, 0};
    return none;
}

int updateSize(const byte *headerID3) {
    byte totalSize[28] = {0};
    for (int i = 9; i >= 6; i--) {
        byte tmp = headerID3[i];
        int t = 6;
        while (tmp) {
            totalSize[(i - 6) * 7 + t] = tmp % 2;
            tmp /= 2;
            t--;
        }
    }

    int framesSize = 0, p = 1;
    for (int i = 27; i >= 0; i--) {
        framesSize += totalSize[i] * p;
        p *= 2;
    }
    return framesSize;
}

byte *edit(byte *header, byte *arr, char *tag, char *value, int n) {
    struct pair exist = check(arr, tag, n);
    if (exist.second) {
        int newFramesSize = n - exist.second + (int) strlen(value);
        byte *newArr = (byte *) malloc(newFramesSize);

        // updateHeader().size
        byte binary[32] = {0};
        int t = 31, tmp = newFramesSize;
        while (tmp) {
            binary[t] = tmp % 2;
            tmp /= 2;
            t--;
        }
        t = 31;
        for (int i = 9; i >= 6; i--) {
            int hex = 0, p = 1;
            for (int j = 0; j < 7; j++) {
                hex += binary[t] * p;
                p *= 2;
                t--;
            }
            header[i] = hex;
        }


        // rewrite tags before
        for (int i = 0; i < exist.first; i++) {
            newArr[i] = arr[i];
        }
        // rewrite name
        for (int i = exist.first; i < 4 + exist.first; i++) {
            newArr[i] = arr[i];
        }
        // update size of frame
        int newLen = (int) strlen(value);
        for (int i = exist.first + 7; i >= exist.first + 4; i--) {
            newArr[i] = newLen % 256;
            newLen /= 256;
        }
        // rewrite flags
        for (int i = exist.first + 8; i < exist.first + 10; i++) {
            newArr[i] = arr[i];
        }
        // rewrite value
        t = 0;
        for (int i = exist.first + 10; i < strlen(value) + exist.first + 10; i++) {
            newArr[i] = value[t];
            t++;
        }
        // rewrite tags after
        t = 0;
        for (int i = exist.first + 10 + (int) strlen(value); i < newFramesSize; i++) {
            newArr[i] = arr[exist.first + exist.second + 10 + t];
            t++;
        }
        return newArr;
    } else {
        return arr;
    }
}

int main(int argc, char* argv[]) {
    char* name = (char*) malloc(100);
    for (int i = 0; i < strlen(argv[1]) - 11; i++) {
        name[i] = argv[1][i + 11];
    }
    FILE *song = fopen(name, "r");
    if (song == NULL) {
        printf("%s", "No such file!");
        return 0;
    }

    byte *headerID3 = (byte *) malloc(10);
    fread(headerID3, 1, 10, song);

    int framesSize = updateSize(headerID3);
    byte *frames = (byte *) malloc(framesSize);
    fread(frames, 1, framesSize, song);
    for (int i = 2; i < argc; i++) {
        if (argv[i][2] == 'g') {
            char* tag = (char*) malloc(100);
            for (int j = 0; j < strlen(argv[i]) - 6; j++) {
                tag[j] = argv[i][j + 6];
            }
            showValue(frames, tag, framesSize);
            free(tag);
        }
        else if (argv[i][2] == 's' && strlen(argv[i]) == 6) {
            show (frames, framesSize);
        }
        else if (argv[i][2] == 's') {
            char* tag = (char*) malloc(100);
            for (int j = 0; j < strlen(argv[i]) - 6; j++) {
                tag[j] = argv[i][j + 6];
            }
            i++;
            char* value = (char*) malloc(100);
            for (int j = 0; j < strlen(argv[i]) - 8; j++) {
                value[j] = argv[i][j + 8];
            }
            frames = edit(headerID3, frames, tag, value, framesSize);
            framesSize = updateSize(headerID3);
            free(tag);
            free(value);
        }
    }
    FILE *output = fopen("out.mp3", "w");
    for (int i = 0; i < 10; i++) {
        fprintf(output, "%c", headerID3[i]);
    }
    for (int i = 0; i < framesSize; i++) {
        fprintf(output, "%c", frames[i]);
    }
    int tmp;
    while ((tmp = getc(song)) != EOF) {
        putc(tmp, output);
    }
    fclose(song);
    free(name);
    free(frames);
}
