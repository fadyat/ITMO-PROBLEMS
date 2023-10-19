#include <iostream>
#include <vector>

using namespace std;

vector<vector<int>> g;
vector<int> color;

vector<int> parents;
int cycle_start, cycle_end = -1;

void dfs (int v) {
    color[v] = 1;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (color[*i] == 0) {
            parents[*i] = v;
            dfs(*i);
        }
        else if (color[*i] == 1) {
            cycle_start = *i;
            cycle_end = v;
        }
    }
    color[v] = 2;
}

int main() {
    freopen("cycle.in", "r", stdin);
    freopen("cycle.out", "w", stdout);
    int n, m;
    cin >> n >> m;

    g.resize(n);
    color.resize(n, 0);
    parents.resize(n, -1);

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

    if (cycle_end == -1) {
        cout << "NO\n";
        return 0;
    }
    else {
        cout << "YES\n";
        vector<int> ans;
        for (int i = cycle_end; i != cycle_start; i = parents[i]) {
            ans.push_back(i);
        }
        ans.push_back(cycle_start);
        for (int i = (int) ans.size() - 1; i >= 0; --i) {
            cout << ans[i] + 1 << " ";
        }
    }
    return 0;
}