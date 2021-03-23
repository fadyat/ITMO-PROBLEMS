#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

vector<bool> used;
vector<vector<int>> g;
vector<int> sorted;

void dfs (int v) {
    used[v] = true;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (!used[*i]) {
            dfs(*i);
        }
    }
    sorted.push_back(v);
}

int main () {
    freopen("hamiltonian.in", "r", stdin);
    freopen("hamiltonian.out", "w", stdout);
    int n, m;
    cin >> n >> m;
    used.resize(n);
    g.resize(n);
    for (int i = 0; i < m; i++) {
        int x, y;
        cin >> x >> y;
        --x, --y;
        g[x].push_back(y);
    }
    for (int i = 0; i < n; i++) {
        if (!used[i]) {
            dfs(i);
        }
    }
    bool have = true;
    for (int i = 1; i < sorted.size(); i++) {
        bool tmp = false;
        for (int j = 0; j < g[sorted[i]].size(); j++) {
            if (g[sorted[i]][j] == sorted[i - 1]) {
                tmp = have;
            }
        }
        have = min (have, tmp);
    }
    cout << ((have) ? ("YES") : ("NO"));
}
