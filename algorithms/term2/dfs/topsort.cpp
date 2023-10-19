#include <iostream>
#include <vector>

using namespace std;

vector<vector<int>> g;
vector<int> color;
bool cycle = false;
vector<int> path;

void dfs (int v) {
    color[v] = 1;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (color[*i] == 0) {
            dfs(*i);
        }
        else if (color[*i] == 1) {
            cycle = true;
        }
    }
    color[v] = 2;
    path.push_back(v);
}

int main() {
    freopen("topsort.in", "r", stdin);
    freopen("topsort.out", "w", stdout);
    int n, m;
    cin >> n >> m;
    g.resize(n);
    color.resize(n, 0);
    for (int i = 0; i <  m; i++) {
        int x, y;
        cin >> x >> y;
        --x, --y;
        g[x].push_back(y);
    }

    for (int i = 0; i < n; i++) {
        if (color[i] == 0) {
            dfs(i);
        }
    }

    if (cycle) {
        cout << "-1";
        return 0;
    }

    for (int i = (int) path.size() - 1; i >= 0; i--) {
        cout << path[i] + 1 << " ";
    }
    return 0;
}