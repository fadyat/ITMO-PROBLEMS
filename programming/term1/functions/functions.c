//var25
//lab10
#include <stdio.h>
#include <string.h>

typedef long long ll;
const int SZ = 100;

ll gcd(ll a, ll b){
    while(a != 0 && b != 0){
        if(a > b)
            a %= b;
        else
            b %= a;
    }
    return a + b;
}

ll lcm(ll a, ll b){
    return (a * b) / gcd(a, b);
}

//char all[6] = {' ', '(', '{', '[', '\'', '"'};
char rec(char s[], int i, char s1[], int j){
    if(i >= strlen(s)){
        printf("%s", s1);
        return 0;
    }
    else{
        // '.'
        if(i == strlen(s) - 2 && s[i] == '.'){
            s1[j] = s[i];
            if(s[i + 1] == ' '){
                printf("%s\n", s1);
                return 0;
            }
        }
        // ' '
        else if(s[i] == ' '){
            if (i + 2 < strlen(s)){
                if(s[i + 1] == ' ' && s[i + 2] == ' '){
                    rec(s, i + 1, s1, j);
                }
                else{
                    s1[j] = s[i];
                    rec(s, i + 1, s1, j + 1);
                }
            }
            else{
                s1[j] = s[i];
                rec(s, i + 1, s1, j + 1);
            }
        }
        // pair el
        else if(s[i] == '(' || s[i] == '[' || s[i] == '{' || s[i] == '\'' || s[i] == '"'){
            if (i + 1 < strlen(s)){
                if (s[i + 1] != ' '){
                    s1[j] = s[i];
                    rec(s, i + 1, s1, j + 1);
                }
                else{
                    s1[j] = s[i];
                    int q = 1;
                    while(s[q + i] == ' ') q++;
                    rec(s, i + q, s1, j + 1);
                }
            }
            else{
                s1[j] = s[i];
                rec(s, i + 1, s1, j + 1);
            }
        }
        //all symbols
        else{
            s1[j] = s[i];
            rec(s, i + 1, s1, j + 1);
        }
    }
    return 0;
}
int main(){
    // 1.
    ll x, y;
    printf("x and y : ");
    scanf("%lld%lld", &x, &y);
    getchar();
    printf("gcd : %lld\nlcm : %lld\n", gcd(x, y), lcm(x, y));
    //5.
    char s[SZ];
    printf("\nString : ");
    scanf("%[^\n]s", s);
    int i = 0, j = 0;
    char s1[SZ] = "";
    rec(s, i, s1, j);
    return 0;
}
