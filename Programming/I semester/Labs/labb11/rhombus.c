#include "rhombus.h"

// length of vectors 1>2, 2>3, 3>4, 4>1, 1>3, 2>4
float side(struct rhombus R, char what) {
    float x, y;
    if(what == '1') {
        x = R.x1 - R.x2;
        y = R.y1 - R.y2;
    }
    if(what == '2') {
        x = R.x2 - R.x3;
        y = R.y2 - R.y3;
    }
    if(what == '3') {
        x = R.x3 - R.x4;
        y = R.y3 - R.y4;
    }
    if(what == '4') {
        x = R.x4 - R.x1;
        y = R.y4 - R.y1;
    }
    if(what == '5') {
        x = R.x1 - R.x3;
        y = R.y1 - R.y3;
    }
    if(what == '6') {
        x = R.x2 - R.x4;
        y = R.y2 - R.y4;
    }
    return sqrtf(x * x + y * y);
}

float S(struct rhombus R) {
    float d1 = side(R, '5');
    float d2 = side(R, '6');
    return (d1 * d2) / 2;
}

float P(struct rhombus R) {
    float a = side(R, '1');
    return 4 * a;
}
