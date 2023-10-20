//var25
//lab3
#include <stdio.h>
#include <math.h>
int main() {
  int n;
  int old_ns = 8, new_ns = 10;
  //1. Print number in NS == old_ns
  scanf("%o", &n);
  //2. Change NS from old_ns to new_ns
  printf("%d\n", n);
  //3. n %o, new n after « 1
  int mv = 1;
  printf("%o %o\n", n, n « mv);
  //4. n %o « 1, new n after not
  printf("%o %o\n", n « mv, ~n);
  //5. n or m
  int m; 
  scanf("%o", &m);
  printf("%o\n", m | n);
}
