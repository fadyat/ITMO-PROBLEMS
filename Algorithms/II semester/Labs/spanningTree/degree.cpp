#include <iostream>
#include <vector>
#include <set>

using namespace std;

int main() {
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);

    int n, m;
    cin >> n >> m;
    vector<set<int>> g(n);

    while (m--) {
        int x, y;
        cin >> x >> y;
        x--, --y;
        g[x].insert(y);
        g[y].insert(x);
    }

    for (int i = 0; i < n; i++) {
        cout << g[i].size() << " ";
    }
}