#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("check.in", "r", stdin);
    freopen("check.out", "w", stdout);
    int n; cin >> n;
    vector<int> v(n);
    vector<int> l(n);
    vector<int> r(n);
    if(n == 0) {
        cout << "YES";
        return 0;
    }
    for(int i = 0; i < n; i++) {
        cin >> v[i] >> l[i] >> r[i];
        l[i]--, r[i]--;
    }

    vector<int> minimum(n, -1e9 -100);
    vector<int> maximum(n, 1e9 + 100);
    for(int i = 0; i < n; i++) {
        if(l[i] >= 0) {
            maximum[l[i]] = min(maximum[i], v[i]);
            minimum[l[i]] = minimum[i];
        }
        if(r[i] >= 0) {
            minimum[r[i]] = max(minimum[i], v[i]);
            maximum[r[i]] = maximum[i];
        }
        if(v[i] >= maximum[i] || v[i] <= minimum[i]) {
            cout << "NO";
            return 0;
        }
    }
    cout << "YES";
}