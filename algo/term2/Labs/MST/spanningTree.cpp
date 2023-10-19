#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

vector<int> p;

int get (int v) {
    return ((v == p[v]) ? (v) : (p[v] = get(p[v])));
}

void unite (int l, int r) {
    l = get(l);
    r = get(r);
    if (rand() & 1) {
        swap(l, r);
    }
    if (l != r) {
        p[l] = r;
    }
}
int main() {
    freopen("spantree3.in", "r", stdin);
    freopen("spantree3.out", "w", stdout);

    int n, m;
    cin >> n >> m;
    vector<pair<int, pair<int, int>>> g;

    for (int i = 0; i < m; ++i) {
        int b, e, w;
        cin >> b >> e >> w;
        b--, e--;
        g.push_back({w, {b, e}});
    }
    sort(g.begin(), g.end());

    long long total = 0;
    p.resize(n);
    for (int i = 0; i < n; i++) {
        p[i] = i;
    }

    for (int i = 0; i < m; ++i) {
        int l = g[i].second.first, r = g[i].second.second, u = g[i].first;
        if (get(l) != get(r)) {
            total += u;
            unite(l, r);
        }
    }

    cout << total << endl;
}