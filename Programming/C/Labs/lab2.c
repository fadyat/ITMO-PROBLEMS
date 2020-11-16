//2var
//2lab
#include <stdio.h>
#include <math.h>
int main() {
  float z1, z2, x;
  // x - in radians
  scanf("%f", &x);
  // 2sin^2(3pi-2x)cos^2(5pi+2x) => 2sin^2(2x)cos^2(2x)
  // 0.25(1-sin(2,5pi-8x)) => 0.25(1-cos(8x))
  z1 = 2 * pow(sin(2*x), 2) * pow(cos(2*x), 2);
  z2 = 0.25 * (1 - cos(8*x));
  printf("%f\n%f\n", z1, z2);
} 
