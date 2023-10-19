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
    freopen("garland.in", "r", stdin);
    freopen("garland.out", "w", stdout);
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(0);
    cout.tie(0);
    file();
    int n;
    double A;
    cin >> n >> A;
    /* let's find h[1] with bin.search
     * now we know all h[1...n]
     * h[i + 1] = 2 * h[i] - h[i - 1] + 2
     * all h[i] > 0 & exist (!) h[j] == 0
     */
    double l = 0;
    double r = A;
    double eps = 1e-7;
    double ans = -1;
    while (r - l > eps) {
        double m = (r + l) / 2;
        double now = m, before = A;
        bool cool = now != 0;
        int zero = 0;
        for (int i = 2; i < n; i++) {
            double next = 2 * now - before + 2;
            if (next == 0) {
                zero++;
            }
            if (next < 0) {
                cool = false;
            }
            before = now;
            now = next;
        }
        if (cool && zero <= 1) {
            r = m;
            ans = now;
        } else {
            l = m;
        }
    }
    cout << fixed << setprecision(2) << ans << endl;
}