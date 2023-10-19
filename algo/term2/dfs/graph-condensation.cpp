#include <iostream>
#include <vector>
 
using namespace std;
 
vector<vector<int>> g;
vector<vector<int>> rg;
vector<bool> used;
vector<int> sorted;
vector<int> component;
 
void dfs1 (int v) {
    used[v] = true;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (!used[*i]) {
            dfs1(*i);
        }
    }
    sorted.push_back(v);
}
 
void dfs2 (int v) {
    used[v] = true;
    component.push_back(v);
    for (auto i = rg[v].begin(); i != rg[v].end(); i++) {
        if (!used[*i]) {
            dfs2(*i);
        }
    }
}
 
int main() {
    freopen("cond.in", "r", stdin);
    freopen("cond.out", "w", stdout);
 
    int n, m;
    cin >> n >> m;
    g.resize(n);
    rg.resize(n);
    used.resize(n, false);
 
    for (int i = 0; i < m; i++) {
        int x, y;
        cin >> x >> y;
        --x, --y;
        g[x].push_back(y);
        rg[y].push_back(x);
    }
 
    for (int i = 0; i < n; i++) {
        if (!used[i]) {
            dfs1(i);
        }
    }
 
    used.assign(n, false);
    vector<int> ans(n);
    int tmp = 1;
 
    for (int i = 0; i < n; i++) {
        int u = sorted[n - i - 1];
        if (!used[u]) {
            dfs2(u);
            for (int i = 0; i < (int) component.size(); i++) {
                ans[component[i]] = tmp;
            }
            ++tmp;
            component.clear();
        }
    }
    cout << tmp - 1 << endl;
    for (int i = 0; i < n; ++i) {
        cout << ans[i] << " ";
    }
}
