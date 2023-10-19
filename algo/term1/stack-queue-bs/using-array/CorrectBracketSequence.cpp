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
    freopen("brackets.in","r", stdin);
    freopen("brackets.out","w", stdout);
}

// void ...

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(0);
    cout.tie(0);
    file();
    string s;
    while(cin >> s) {
        char ans[10000 + 1];
        int j = 0, sz = 0;
        for(int i = 0; i < s.size(); i++) {
            if(sz != 0 && j > 0 && (s[i] == ')' && ans[j - 1] == '(') || (s[i] == ']' && ans[j - 1] == '[')) {
                j--;
                sz--;
            }
            else {
                ans[j] = s[i];
                j++;
                sz++;
            }
        }
        if(sz == 0) {
            cout << "YES" << endl;
        }
        else {
            cout << "NO" << endl;
        }
    }
}