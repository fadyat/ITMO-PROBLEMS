#include <math.h>

#ifndef UNTITLED_RHOMBUS_H
#define UNTITLED_RHOMBUS_H

struct rhombus {
    float x1, y1;
    float x2, y2;
    float x3, y3;
    float x4, y4;
};

float side(struct rhombus, char what);
float S(struct rhombus);
float P(struct rhombus);

#endif //UNTITLED_RHOMBUS_H
