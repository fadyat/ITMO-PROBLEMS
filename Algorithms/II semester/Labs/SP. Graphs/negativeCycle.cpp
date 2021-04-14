#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
 
static const int INF = 1e9;
 
struct edge {
    int x, y, w;
    edge (int x_, int y_, int w_) : x(x_), y(y_), w(w_) {}
};
 
 
int main() {
    freopen("negcycle.in", "r", stdin);
    freopen("negcycle.out", "w", stdout);
    int n, m;
    vector<edge> g;
    cin >> n;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            int x;
            cin >> x;
            if (x != 1e9) {
                g.emplace_back(edge(i, j, x));
            }
        }
    }
    m = (int) g.size();
    vector<int> d(n);
    vector<int> p(n, -1);
    int x;
    for (int i = 0; i < n; ++i) {
        x = -1;
        for (int j = 0; j < m; ++j)
            if (d[g[j].y] > d[g[j].x] + g[j].w) {
                d[g[j].y] = max(-INF, d[g[j].x] + g[j].w);
                p[g[j].y] = g[j].x;
                x = g[j].y;
            }
    }
 
    if (x == -1) {
        cout << "NO\n";
    } else {
        for (int i = 0; i < n; ++i) {
            x = p[x];
        }
 
        vector<int> path;
        int v = x;
        while(true) {
            path.push_back(v);
            if (v == x && path.size() > 1) {
                break;
            }
            v = p[v];
        }
        reverse(path.begin(), path.end());
 
        cout << "YES\n" << path.size() << endl;
        for (int i : path) {
            cout << i + 1 << ' ';
        }
    }
}
