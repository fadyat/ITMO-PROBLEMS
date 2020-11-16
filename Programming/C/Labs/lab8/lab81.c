//var25
//lab8
#include <stdio.h>
#include <string.h>
const int SZ = 100;
int max(int a, int b) {
    if (a > b)
        return a;
    else
        return b;
}

// 8.
int first(char s1[], char a[]){
    int pos = 0;
    for(int i = 0; i < strlen(s1); i++){
        if(s1[i] == a[0]){
            pos = i;
            break;
        }
    }
    return pos;
}

// 11.
int bunch(char s1[], char s2[]){
    int all[255] = {0};
    for(int i = 0; i < strlen(s2); i++)
        all[s2[i]]++;
    int l = 0, ml = 0;
    for(int i = 0; i < strlen(s1); i++){
        if(all[s1[i]] > 0)
            l++;
        else
            l = 0;
        ml = max(ml, l);
    }
    return ml;
}
// 12.
int not_bunch(char s1[], char s2[]){
    int all[255] = {0};
    for(int i = 0; i < strlen(s2); i++)
        all[s2[i]]++;
    int l = 0, ml = 0;
    for(int i = 0; i < strlen(s1); i++){
        if(all[s1[i]] == 0)
            l++;
        else
            l = 0;
        ml = max(ml, l);
    }
    return ml;
}

int main() {
    char s1[SZ]; printf("s1 : "); scanf("%s", s1);
    char s2[SZ]; printf("s2 : "); scanf("%s", s2);
    // 2.
    int n; printf("n: "); scanf("%d", &n);
    strncat(s1, s2, n);
    printf("s1 + s2[0...n] : %s\n", s1);
    // 5.
    char s4[SZ];
    strcpy(s4, s1);
    printf("Copy s1 : %s\n", s4);
    // 8.
    char a[1];
    printf("Print a : ");
    scanf("%s", a);
    int pos = first(s1, a);
    (s1[pos] != a[0]) ? (printf("a : not found\n")) : printf("a : [%d]\n", pos);
    // 11.
    printf("Max len s1 with s2 elem : %d\n", bunch(s1, s2));
    // 12.
    printf("Max len s1 without s2 elem : %d\n", not_bunch(s1, s2));
    return 0;
}
