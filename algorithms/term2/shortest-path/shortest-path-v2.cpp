#include <iostream>
#include <vector>
#include <algorithm>
#include <set>

using namespace std;

long long INF = 1e14;

int main() {
    freopen("pathbgep.in", "r", stdin);
    freopen("pathbgep.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    int n, m;
    cin >> n >> m;
    vector<vector<pair<long long, long long>>> g(n);
    while (m--) {
        int x, y, w;
        cin >> x >> y >> w;
        --x, --y;
        g[x].push_back({y, w});
        g[y].push_back({x, w});
    }
    vector<long long> d(n, INF);
    d[0] = 0;
    set<pair<long long, int>> s;
    s.insert({d[0], 0});

    while (!s.empty()) {
        int v = s.begin()->second;
        s.erase(s.begin());
        for (auto j = g[v].begin(); j != g[v].end(); j++) {
            int to = (*j).first, l = (*j).second;
            if (d[v] + l < d[to]) {
                s.erase({d[to], to});
                d[to] = d[v] + l;
                s.insert({d[to], to});
            }
        }
    }

    for (int i = 0; i < n; i++) {
        cout << ((d[i] == INF) ? (0) : (d[i])) << " ";
    }
}
