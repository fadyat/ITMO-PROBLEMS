#include <iostream>
#include <vector>
#include <bitset>
#include <iomanip>

using namespace std;

vector<short> xx;
vector<string> operation;
short a, c;

void push() {
    xx.push_back(a);
    xx.push_back(c);
    xx.push_back(a + c);
    xx.push_back(a + c + c);
    xx.push_back(c - a);
    xx.push_back(65536 - xx[3]);
    xx.push_back(-xx[0]);
    xx.push_back(-xx[1]);
    xx.push_back(-xx[2]);
    xx.push_back(-xx[3]);
    xx.push_back(-xx[4]);
    xx.push_back(-xx[5]);

    operation.emplace_back("A");
    operation.emplace_back("C");
    operation.emplace_back("A + C");
    operation.emplace_back("A + C + C");
    operation.emplace_back("C - A");
    operation.emplace_back("65536 - X4");
    operation.emplace_back("-X1");
    operation.emplace_back("-X2");
    operation.emplace_back("-X3");
    operation.emplace_back("-X4");
    operation.emplace_back("-X5");
    operation.emplace_back("-X6");
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    a = 1818, c = 16924;
    push();

    /*                                  WARNING
     *
     * In such arithmetic operations, the result may not be the expected result.
     * Because an overflow of type <=> -2^15 <= short < 2^15 occurs.
     * If the result is greater than the short-range -> overflow
     * If you have an overflow, the computer will count the NUM number % 2^15 +/- 2^16
     * +/- depends on the type of overflow you have
     *
     * */

    for(int i = 0; i < xx.size(); i++) {
        cout << endl << setw(12) << operation[i]
        << " \t\tx" << i + 1 << "\t "
        << xx[i] << "\t "
        << bitset<sizeof(short) * CHAR_BIT>(xx[i]);
    }
    cout << endl;
    for(int i = 1; i <= 12; i++) {
        for(int j = i + 1; j <= 12; j++) {
            if((i == 1 && j == 2) || (i == 2 && j == 3) || (i == 7 && j == 8)
            || (i == 8 && j == 9) || (i == 2 && j == 7) || (i == 1 && j == 8)) {
                cout << endl << "\tB" << i << " " << "+ B" << j << " "
                << bitset<sizeof(short) * CHAR_BIT>(xx[i - 1] + xx[j - 1])<< " "
                << short(xx[i - 1] + xx[j - 1]);
            }
        }
    }
    cout << endl;
}
