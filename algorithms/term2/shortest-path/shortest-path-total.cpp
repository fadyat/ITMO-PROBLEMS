#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int INF = 1e9 + 100;

int main() {
    freopen("pathsg.in", "r", stdin);
    freopen("pathsg.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    int n, m;
    cin >> n >> m;
    vector<vector<int>> d(n, vector<int> (n, INF));
    while (m--) {
        int x, y, w;
        cin >> x >> y >> w;
        x--, y--;
        d[x][y] = w;
    }
    for (int i = 0; i < n; i++) {
        d[i][i] = 0;
    }
    for (int k = 0; k < n; k++) {
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                d[i][j] = min(d[i][j], d[i][k] + d[k][j]);
            }
        }
    }
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            cout << ((d[i][j] == INF) ? (0) : (d[i][j])) << " ";
        }
        cout << endl;
    }
}
