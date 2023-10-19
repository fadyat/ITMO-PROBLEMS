// This code was created with the support of Sergo.

#include <iostream>
#include <vector>
#include <set>
#include <algorithm>
#include <cmath>
#include <iomanip>
#include <queue>

typedef long long ll;
typedef double ld;
typedef unsigned long long ull;

using namespace std;

void file() {
    freopen("postfix.in", "r", stdin);
    freopen("postfix.out", "w", stdout);
}

// void ...

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(0);
    cout.tie(0);
    file();
    char x;
    int ans[1000000 + 1], j = 0;
    while(cin >> x) {
        if(x >= '0' && x <= '9') {
            ans[j++] = x - '0';
        }
        else if(x != ' '){
            j--;
            int y = ans[j--];
            int q = ans[j];
            if(x == '+') {
                ans[j] = y + q;
                j++;
                ans[j] = 0;
            }
            else if(x == '*') {
                ans[j] = y * q;
                j++;
                ans[j] = 0;
            }
            else if(x == '-') {
                ans[j] = q - y;
                j++;
                ans[j] = 0;
            }
        }
    }
    cout << ans[0] << endl;
}