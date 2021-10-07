#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/stat.h>

void list (char *archiveName) {
    FILE* archive = fopen(archiveName, "r");
    char s[20], *end;
    fscanf(archive, "%s", s);
    int j = (int) strtol(s, &end, 10);
    for (int i = 0; i < j; i++) {
        char tmp[100];
        fscanf(archive, "%s", tmp);
        printf("\n%d. %s", i + 1, tmp);
        fscanf(archive, "%s", tmp);
    }
    printf("\n\n");
}

void extract (char *archiveName) {
    mkdir("all", 0777);
    FILE* archive = fopen(archiveName, "r");
    char s[20], *end;
    fscanf(archive, "%s", s);
    int j = (int) strtol(s, &end, 10);
    char name[j][100];
    long long size[j];
    for (int i = 0; i < j; i++) {
        char tmp[100], tmp1[100], *e;
        fscanf(archive, "%s", tmp);
        strcpy(name[i], tmp);
        fscanf(archive, "%s", tmp1);
        size[i] = strtol(tmp1, &e, 10);
    }
    getc(archive); // read '\n'
    for (int i = 0; i < j; i++) {
        char str[100] = "all//";
        strcat(str, name[i]);
        FILE *new = fopen(str, "w");
        int tmp;
        while (size[i]--) {
            tmp = getc(archive);
            putc(tmp, new);
        }
        fclose(new);
    }
    fclose(archive);
}

void create (char *archiveName, int argc, char* argv[]) {
    FILE *archive = fopen(archiveName, "w");
    // write count of my files
    fprintf(archive, "%d ", argc - 4);
    // create header
    for (int i = 4; i < argc; i++) {
        long long rate;
        FILE *tmp = fopen(argv[i], "rb");
        fseek(tmp, SEEK_SET, SEEK_END);
        rate = ftell(tmp);
        fseek(tmp, SEEK_CUR, SEEK_SET);
        fprintf(archive, "%s %llu", argv[i], rate);
        fprintf(archive, (i + 1 != argc) ? (" ") : ("\n"));
        fclose(tmp);
    }
    // rewrite info
    for (int i = 4; i < argc; i++) {
        FILE *tmp = fopen(argv[i], "rb");
        int new;
        while ((new = getc(tmp)) != EOF) {
            putc(new, archive);
        }
        fclose(tmp);
    }
    fclose(archive);
}

int main(int argc, char *argv[]) {
    /* --file name --extract
       --file name --list
       --file name --create file1 file2 ... fileN */
    char *archiveName;
    for (int i = 1; i < argc; i++) {
        if (!strcmp("--file", argv[i])) {
            archiveName = argv[++i];
        }
        else if (!strcmp("--list", argv[i])) {
            list(archiveName);
        }
        else if (!strcmp("--extract", argv[i])) {
            extract(archiveName);
        }
        else if (!strcmp("--create", argv[i])) {
            create(archiveName, argc, argv);
        }
    }
}
