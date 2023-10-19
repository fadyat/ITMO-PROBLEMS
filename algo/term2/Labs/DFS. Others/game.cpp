#include <iostream>
#include <vector>
#include <algorithm>

vector<int> h;
vector<vector<int>> g;

void dfs (int v) {
    h[v] = 2;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (!h[*i]) {
            dfs(*i);
        }
    }
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (h[*i] == 2) {
            h[v] = 1;
        }
    }
}

using namespace std;

int main () {
    freopen("game.in", "r", stdin);
    freopen("game.out", "w", stdout);
    int n, m, s;
    cin >> n >> m >> s;
    --s;
    h.resize(n, 0);
    g.resize(n);
    for (int i = 0; i < m; i++) {
        int x, y;
        cin >> x >> y;
        --x, --y;
        g[x].push_back(y);
    }
    dfs(s);
    cout << ((h[s] == 1) ? ("First") : ("Second")) << " player wins\n";
}
