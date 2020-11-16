//var25
//lab11
#include <stdio.h>
#include "rhombus.h"

int main() {
    struct rhombus figure;
    scanf("%f%f", &figure.x1, &figure.y1);
    scanf("%f%f", &figure.x2, &figure.y2);
    scanf("%f%f", &figure.x3, &figure.y3);
    scanf("%f%f", &figure.x4, &figure.y4);

    printf("%f\n", S(figure));
    printf("%f", P(figure));
}