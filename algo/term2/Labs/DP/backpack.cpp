#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("knapsack.in", "r", stdin);
    freopen("knapsack.out", "w", stdout);
    int S, n;
    cin >> S >> n;
    vector<vector<bool>> dp;
    vector<int> w;
    w.resize(n);
    for (int i = 0; i < n; i++) {
        cin >> w[i];
    }
    dp.resize(n + 1, vector<bool> (S + 1));
    for (int i = 0; i < n + 1; i++) {
        dp[i][0] = true;
    }
    for (int i = 1; i <= n; i++) {
        for (int j = 1; j <= S; j++) {
            if (j >= w[i - 1]) {
                dp[i][j] = max(dp[i - 1][j], dp[i - 1][j - w[i - 1]]);
            }
            else {
                dp[i][j] = dp[i - 1][j];
            }
        }
    }
    for (int i = S; i >= 0; i--) {
        if (dp[n][i]) {
            cout << i << endl;
            return 0;
        }
    }
}