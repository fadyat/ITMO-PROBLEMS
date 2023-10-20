#include <iostream>
#include <vector>
#include <algorithm>
#include <queue>
 
using namespace std;
 
vector<vector<int>> g;
vector<bool> used;
vector<int> path;
 
void bfs (int v) {
    used[v] = true;
    queue<int> q;
    q.push(v);
    while (!q.empty()) {
        int vv = q.front();
        q.pop();
        for (auto i = g[vv].begin(); i != g[vv].end(); i++) {
            int to = *i;
            if (!used[to]) {
                used[to] = true;
                q.push(to);
                path[to] = path[vv] + 1;
            }
        }
    }
}
 
int main() {
    freopen("pathbge1.in", "r", stdin);
    freopen("pathbge1.out", "w", stdout);
    int n, m;
    cin >> n >> m;
 
    g.resize(n);
    path.resize(n);
    used.resize(n);
 
    while (m--) {
        int x, y;
        cin >> x >> y;
        x--, y--;
        g[x].push_back(y);
        g[y].push_back(x);
    }
 
    bfs(0);
    for (int i = 0; i < n; i++) {
        cout << path[i] << " ";
    }
}
