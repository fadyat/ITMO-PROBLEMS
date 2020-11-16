//var25
//lab6
#include <stdio.h>
#include <stdlib.h>
int main() {
    int s = 4;
    // 1. WORK
    char* a[s];
    a[0] = 'W';
    a[1] = 'O';
    a[2] = 'R';
    a[3] = 'K';
    for(int i = 0; i < s; i++)
        printf("a[%d]: %c \n", i, a[i]);
    printf("\n");

    // 2. dynamic
    int *b;
    b = malloc(s * sizeof(char));
    *b = 'W';
    *(b + 1) = 'O';
    *(b + 2) = 'R';
    *(b + 3) = 'K';
    for(int i = 0; i < s; i++)
        printf("b[%d]: %c \n", i, *(b + i));
    printf("\n");
    free(b);
    return 0;
}
