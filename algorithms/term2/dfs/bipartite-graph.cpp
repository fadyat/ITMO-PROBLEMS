#include <iostream>
#include <vector>

using namespace std;

vector<vector<int>> g;
vector<int> color;

void dfs (int v, int cl) {
    color[v] = cl;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (color[*i] == 0) {
            dfs(*i, cl % 2 + 1);
        }
        else if (color[*i] == cl) {
            cout << "NO\n";
            exit(0);
        }
    }
}

int main() {
    freopen("bipartite.in", "r", stdin);
    freopen("bipartite.out", "w", stdout);
    int n, m;
    cin >> n >> m;
    g.resize(n);
    color.resize(n, 0);

    for (int i = 0; i <  m; i++) {
        int x, y;
        cin >> x >> y;
        --x, --y;
        g[x].push_back(y);
        g[y].push_back(x);
    }

    for (int i = 0; i < n; i++) {
        if (color[i] == 0) {
            dfs(i, 1);
        }
    }
    cout << "YES\n";
}