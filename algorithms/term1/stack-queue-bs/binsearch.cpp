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
    freopen("binsearch.in", "r", stdin);
    freopen("binsearch.out", "w", stdout);
}

// void ...

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(0);
    cout.tie(0);
    file();
    int n;
    cin >> n;
    vector<int> a(n);
    for (int i = 0; i < n; i++) {
        cin >> a[i];
    }
    a.push_back(1e9 + 1);
    int t;
    cin >> t;
    while (t--) {
        int x;
        cin >> x;
        int l = -1, r = (int)a.size() + 1;
        while (r - l > 1) {
            int m = (r + l) / 2;
            if (a[m] >= x) {
                r = m;
            } else {
                l = m;
            }
        }

        if (a[r] != x) {
            cout << "-1 -1" << endl;
            continue;
        }
        cout << r + 1 << " ";
        x++;
        l = -1, r = (int)a.size() + 1;
        while (r - l > 1) {
            int m = (r + l) / 2;
            if (a[m] >= x) {
                r = m;
            } else {
                l = m;
            }
        }
        cout << l + 1 << endl;
    }
}