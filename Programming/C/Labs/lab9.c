//var25
//lab9
#include <stdio.h>
#include <string.h>
const int SZ = 100;
int main(){
    // 1.
    char s[SZ];
    printf("s : ");
    scanf("%s", s);
    int l = 0, n = 0;
    for(int i = 0; i < strlen(s); i++){
        if(s[i] >= '0' && s[i] <= '9')
            n++;
        else if((s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z'))
            l++;
    }
    printf("letters : %d\nnumbers : %d\n\n", l, n);
    // 5.
    float a, p;
    int m;
    printf("Amount : ");
    scanf("%f", &a);
    printf("Months : ");
    scanf("%d", &m);
    printf("Percent : ");
    scanf("%f", &p);
    for(int i = 1; i <= m; i++){
        a += a * (p / 100) / 12;
        if(i / 12 != 0 && i % 12 == 0)
            printf("%dY : %.2f\n", i / 12, a);
        else if(i / 12 != 0)
            printf("%dY %dM : %.2f\n", i / 12, i % 12, a);
        else
            printf("%dM : %.2f\n", i % 12, a);
    }
    printf("\nTotal : %.2f", a);
    return 0;
}
