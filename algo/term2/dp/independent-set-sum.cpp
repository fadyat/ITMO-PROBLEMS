#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

vector<vector<pair<int, int>>> g;
vector<int> dp;

int getIndependentSet(pair<int, int> v) {
    if (dp[v.first] != 0) {
        return dp[v.first];
    }
    int childrenSum = 0;
    int grandchildrenSum = 0;
    for (auto child : g[v.first]) {
        childrenSum += getIndependentSet(child);
    }
    for (auto children : g[v.first]) {
        for (auto grandchildren : g[children.first]) {
            grandchildrenSum += getIndependentSet(grandchildren);
        }
    }
    dp[v.first] = max(v.second + grandchildrenSum, childrenSum);
    return dp[v.first];
}

int main() {
    freopen("selectw.in", "r", stdin);
    freopen("selectw.out", "w", stdout);
    int n;
    cin >> n;
    g.resize(n + 1);
    dp.resize(n + 1, 0);
    for (int i = 1; i <= n; i++) {
        int parent;
        int weight;
        cin >> parent >> weight;
        g[parent].push_back({i, weight});
    }
    cout << getIndependentSet(g[0][0]) << endl;
}
