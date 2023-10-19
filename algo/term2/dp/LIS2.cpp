#include <iostream>
#include <vector>

using namespace std;

int n, m;
vector<vector<int>> dp;

long long check (int i, int j) {
    if (i >= 0 && j >= 0 && i < n && j < m) {
        if (dp[i][j] == -1) {
            dp[i][j] = check(i + 1, j - 2) + check(i - 1, j - 2) + check(i - 2, j + 1) + check(i - 2, j - 1);
        }
    }
    else {
        return 0;
    }
    return dp[i][j];
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("knight2.in", "r", stdin);
    freopen("knight2.out", "w", stdout);
    cin >> n >> m;
    dp.resize(n, vector<int> (m, -1));
    dp[0][0] = 1;
    cout << check(n - 1, m - 1);
}