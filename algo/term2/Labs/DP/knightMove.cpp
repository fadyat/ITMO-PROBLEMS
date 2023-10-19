#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

static const int INF = 1e9 + 100;

int main() {
    int n;
    cin >> n;
    vector<int> a(n);
    for (int i = 0; i < n; i++) {
        cin >> a[i];
    }
    vector<int> d(n + 1, INF);
    d[0] = -INF;
    vector<int> pos(n + 1, -1);
    vector<int> prev(n + 1, -1);
    for (int i = 0; i < n; i++) {
        int l = 0, r = n;
        while (r - l > 1) {
            int m = (r + l) / 2;
            if (d[m] >= a[i]) {
                r = m;
            }
            else {
                l = m;
            }
        }
        d[r] = a[i];
        pos[r] = i;
        prev[i] = pos[r - 1];
    }
    for (int i = n; i >= 0; --i) {
        if (d[i] != INF) {
            vector<int> answer;
            int p = pos[i];
            while (p != -1) {
                answer.push_back(a[p]);
                p = prev[p];
            }
            reverse(answer.begin(), answer.end());
            cout << answer.size() << endl;
            for (int q : answer) {
                cout << q <<  " ";
            }
            return 0;
        }
    }
}