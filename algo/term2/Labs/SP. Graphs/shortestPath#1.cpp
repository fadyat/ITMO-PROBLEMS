#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int main() {
    freopen("pathmgep.in", "r", stdin);
    freopen("pathmgep.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    int n, s, f;
    cin >> n >> s >> f;
    --s, --f;
    vector<vector<pair<long long, long long>>> g(n);
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            long long xx;
            cin >> xx;
            if (xx > 0) {
                g[i].push_back({j, xx});
            }
        }
    }
    long long INF = 1e14;
    vector<long long> d(n, INF);
    d[s] = 0;
    vector<bool> used(n, false);
    for (int i = 0; i < n; i++) {
        int v = -1;
        for (int j = 0; j < n; j++) {
            if (!used[j] && (v == -1 || d[j] < d[v])) {
                v = j;
            }
        }
        if (d[v] == INF) {
            break;
        }
        used[v] = true;
        for (auto j = g[v].begin(); j != g[v].end(); j++) {
            long long to = (*j).first, l = (*j).second;
            if (d[v] + l < d[to]) {
                d[to] = d[v] + l;
            }
        }
    }
    cout << ((d[f] == INF) ? (-1) : (d[f])) << endl;
}
