//var25
//lab7
#include <stdio.h>
#include <math.h>
// 1. Enumeration type
enum music{
    classic,    //0
    pop,        //1
    rock,       //2
    rap,        //...
    electronic,
    disco
};
// 2. Struct
struct pair{
    int x;
    int y;
};
struct square{
    struct pair ulc;    // upper left corner
    struct pair llc;    // lower left corner
    struct pair urc;    // upper right corner
    struct pair lrc;    // lower right corner
    float side;
    float P;
};
//3. Union
struct x_bit{
    unsigned int ready : 1; // 0 - 1
    unsigned int toner : 1; // 0 - 1
    unsigned int drum : 1; // 0 - 1
    unsigned int paper : 1; // 0 - 1
};
union printer{
    struct x_bit x_b;
    int x;
};
int main() {
    //------------------------------------
    int s = rock;
    printf("Number of the rock is: %d", s);
    printf("\n");

    //------------------------------------

    struct square my_sqr;
    printf("Enter ulc coordinates(x, y): ");
    scanf("%d%d", &my_sqr.ulc.x, &my_sqr.ulc.y);

    printf("Enter llc coordinates(x, y): ");
    scanf("%d%d", &my_sqr.llc.x, &my_sqr.llc.y);

    printf("Enter urc coordinates(x, y): ");
    scanf("%d%d", &my_sqr.urc.x, &my_sqr.urc.y);

    printf("Enter lrc coordinates(x, y): ");
    scanf("%d%d", &my_sqr.lrc.x, &my_sqr.lrc.y);

    // Count length side of my square : sqrt((x2 - x1)^2 + (y2 - y1)^2)
    // Assumed that the data entered is correct for the all sides of the square
    my_sqr.side = (float)sqrt(pow((my_sqr.ulc.x - my_sqr.llc.x), 2) + pow((my_sqr.ulc.y - my_sqr.llc.y), 2));
    my_sqr.P = 4 * my_sqr.side;
    // I limited my numbers after the dot to 2
    printf("%.2f\n", my_sqr.P);

    //------------------------------------

    union printer m;
    scanf("%x", &m.x);
    printf("ready : %d \n", m.x_b.ready);
    printf("toner : %d \n", m.x_b.toner);
    printf("drum : %d \n", m.x_b.drum);
    printf("paper : %d \n", m.x_b.paper);
}
