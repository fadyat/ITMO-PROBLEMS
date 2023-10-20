#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int likeDFS(vector<int> &v, vector<int> &l, vector<int> &r, int start) {
    int h = 0;
    if (l[start] != -1) {
        int left = likeDFS(v, l, r, l[start]);
        h = max(h, left);
    }
    if (r[start] != -1) {
        int right = likeDFS(v, l, r, r[start]);
        h = max(h, right);
    }
    return h + 1;
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("height.in", "r", stdin);
    freopen("height.out", "w", stdout);
    int n;
    cin >> n;
    if (n == 0) {
        cout << 0 << endl;
        return 0;
    }
    vector<int> v(n);
    vector<int> l(n);
    vector<int> r(n);
    for (int i = 0; i < n; i++) {
        cin >> v[i] >> l[i] >> r[i];
        l[i]--, r[i]--;
    }
    cout << likeDFS(v, l, r, 0) << endl;
}