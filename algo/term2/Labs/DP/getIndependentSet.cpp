#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
vector<vector<int>> g;
vector<int> dp;
 
int getIndependentSet(int v) {
    if (dp[v] != 0) {
        return dp[v];
    }
    int childrenSum = 0;
    int grandchildrenSum = 0;
    for (int child : g[v]) {
        childrenSum += getIndependentSet(child);
    }
    for (int children : g[v]) {
        for (int grandchildren : g[children]) {
            grandchildrenSum += getIndependentSet(grandchildren);
        }
    }
    dp[v] = max(1 + grandchildrenSum, childrenSum);
    return dp[v];
}
 
int main() {
    int n;
    cin >> n;
    g.resize(n + 1);
    dp.resize(n + 1, 0);
    for (int i = 1; i <= n; i++) {
        int parent;
        cin >> parent;
        g[parent].push_back(i);
    }
    cout << getIndependentSet(g[0][0]) << endl;
}
