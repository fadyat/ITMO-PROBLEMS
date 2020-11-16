//lab5
//var25
#include <stdio.h>
int main() {
    //1. array
    // -
    int m1[9] = {99, 88, 77, 66, 55, 44, 33, 22, 11};
    for(int i = 0; i < 9; i++){
        printf("%d ", m1[i]);
    }
    printf("\n\n");
    
    // -auto
    int m2[9], p = 99;
    for(int i = 0; i < 9; i++){
        m2[i] = p;
        p -= 11;
    }
    for(int i = 0; i < 9; i++){
        printf("%d ", m2[i]);
    }
    printf("\n\n");
    
    //----------------------------
    //2. mtrx1 * mtrx2
    int n[2][2] = {{1, 2},{3, 4}};
    int m[2][2] = {{1, 0},{0, 1}};
    int t[2][2];
    for(int i = 0; i < 2; i++){
        for(int j = 0; j < 2; j++){
            t[i][j] = 0;
            for(int q = 0; q < 2; q++)
                t[i][j] += n[i][q] * m[q][j];
        }
    }
    
    for(int i = 0; i < 2; i++){
        for(int j = 0; j < 2; j++)
            printf("%d ", t[i][j]);
        printf("\n");
    }
    return 0;
}
