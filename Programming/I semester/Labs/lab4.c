//var25
//lab4
#include <stdio.h>
int main() {
    int n;
// 1. -9 <= n <= 0
    scanf("%d", &n);
    //used ternary operator
    (n >= -9 && n <= 0) ? (printf("YES = ")) : (printf("NO = "));
    //used simple printf yes = 1 / no = 0
    printf("%d\n", n >= -9 && n <= 0);
// 2. 25bit
    int m;
    scanf("%d", &m);
    int bit = 25;
    int all_bits[33] ={0}, i = 0;
    // with >> 25
    printf("%d", (m >> bit) % 2);
    // with while
    while(m){
        all_bits[i] = m % 2;
        m /= 2;
        i++;
    }
    printf("\n%d ", all_bits[bit]);
}
