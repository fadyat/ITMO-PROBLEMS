#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
vector<vector<int>> g;
vector<bool> used;
vector<int> k;
 
void dfs (int v, int it) {
    used[v] = true;
    k[v] = it;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        if (!used[*i]) {
            dfs(*i, it);
        }
    }
}
 
 
int main() {
    freopen("components.in", "r", stdin);
    freopen("components.out", "w", stdout);
    int n, m;
    cin >> n >> m;
 
    g.resize(n);
    k.resize(n);
    used.resize(n);
 
    while (m--) {
        int x, y;
        cin >> x >> y;
        x--, y--;
        g[x].push_back(y);
        g[y].push_back(x);
    }
    int it = 1;
 
    for (int i = 0; i < n; i++) {
        if (!used[i]) {
            dfs(i, it);
            it++;
        }
    }
 
    cout << it - 1 << endl;
    for (int i = 0; i < n; i++) {
        cout << k[i] << " ";
    }
}
