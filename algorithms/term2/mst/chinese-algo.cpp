#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

struct Edge {
    int to;
    int w;
    Edge (int to, int w) {
        this->to = to;
        this->w = w;
    }
};

void dfs (int v, vector<vector<Edge>> &edges, vector<bool> &used) {
    used[v] = true;
    for (Edge &i : edges[v]) {
        if (!used[i.to]) {
            dfs(i.to, edges, used);
        }
    }
}

bool Reach (int v, vector<vector<Edge>> &edges, int n) {
    vector<bool> used(n + 1, false);
    dfs(v, edges, used);
    for (int i = 1; i <= n; i++) {
        if (!used[i]) {
            return false;
        }
    }
    return true;
}

struct Components {
    vector<int> components;
    int total;
    Components (int size, int n) {
        components.resize(size + 1);
        total = n;
    }
};

void dfs1 (int v, vector<vector<Edge>> &edges, vector<bool> &used, vector<int> &topSort) {
    used[v] = true;
    for (Edge &i : edges[v]) {
        if (!used[i.to]) {
            dfs1(i.to, edges, used, topSort);
        }
    }
    topSort.push_back(v);
}

void dfs2 (int v, vector<vector<Edge>> &reversed, vector<int> &components, int total) {
    components[v] = total;
    for (Edge &i : reversed[v]) {
        if (components[i.to] == 0) {
            dfs2(i.to, reversed, components, total);
        }
    }
}

Components Condensation (vector<vector<Edge>> &edges, int n) {
    vector<bool> used(n + 1, false);
    Components cp (n + 1, 0);
    vector<int> topSort;
    for (int i = 1; i <= n; i++) {
        if (!used[i]) {
            dfs1(i, edges, used, topSort);
        }
    }
    reverse(topSort.begin(), topSort.end());
    vector<vector<Edge>> reversed (n + 1);
    for (int i = 1; i <= n; i++) {
        for (Edge &edge : edges[i]) {
            reversed[edge.to].push_back(Edge(i, edge.w));
        }
    }
    for (int &i : topSort) {
        if (cp.components[i] == 0) {
            dfs2(i, reversed, cp.components, ++cp.total);
        }
    }
    return cp;
}

long long Chinese (vector<vector<Edge>> &edges, int n, int v) {
    if (!Reach(v, edges, n)) {
        return -1;
    }
    vector<int> minEdges (n + 1, 1e9 + 100);
    for (int i = 1; i <= n; i++) {
        for (Edge &edge : edges[i]) {
            minEdges[edge.to] = min (minEdges[edge.to], edge.w);
        }
    }
    long long res = 0;
    for (int i = 1; i <= n; i++) {
        if (i != v) {
            res += minEdges[i];
        }
    }
    vector<vector<Edge>> zeroEdges(n + 1);
    for (int i = 1; i <= n; i++) {
        for (Edge &edge : edges[i]) {
            if (edge.w == minEdges[edge.to]) {
                zeroEdges[i].push_back(Edge(edge.to, 0));
            }
        }
    }
    if (Reach(v, zeroEdges, n)) {
        return res;
    }
    Components cp = Condensation(zeroEdges, n);
    vector<vector<Edge>> newEdges (cp.total + 1);
    for (int i = 1; i <= n; i++) {
        for (Edge &edge : edges[i]) {
            if (cp.components[i] != cp.components[edge.to]) {
                newEdges[cp.components[i]].push_back(Edge(cp.components[edge.to], edge.w - minEdges[edge.to]));
            }
        }
    }
    res += Chinese(newEdges, cp.total, cp.components[v]);
    return res;
}

int main() {
    freopen("chinese.in", "r", stdin);
    freopen("chinese.out", "w", stdout);
    ios_base::sync_with_stdio();
    cin.tie(nullptr);
    cout.tie(nullptr);
    int n, m;
    cin >> n >> m;
    vector<vector<Edge>> g (n + 1);
    for (int i = 0; i < m; i++) {
        int x, y, w;
        cin >> x >> y >> w;
        g[x].push_back(Edge(y, w));
    }
    long long ans = Chinese(g, n, 1);
    if (ans == -1) {
        cout << "NO" << endl;
    }
    else {
        cout << "YES" << endl;
        cout << ans << endl;
    }
}