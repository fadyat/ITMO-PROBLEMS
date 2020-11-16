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
    freopen("stack.in","r", stdin);
    freopen("stack.out","w", stdout);
}

// void ...

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(0);
    cout.tie(0);
    file();
    int n;
    cin >> n;
    int ans[1000000 + 1];
    int sz = 0, j = 0;
    for(int i = 0; i < n; i++) {
        char x;
        cin >> x;
        if(x == '+'){
            int b;
            cin >> b;
            ans[j] = b;
            j++;
            sz++;
        }
        else {
            cout << ans[j - 1] << endl;
            ans[j] = 0;
            j--;
            sz--;
        }
    }
}