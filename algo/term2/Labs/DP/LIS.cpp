#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    int n;
    cin >> n;
    vector<int> all(n);
    for(int i = 0; i < n; i++) {
        cin >> all[i];
    }
    vector<int> dp(n, 1);
    for(int i = 1; i < n; i++) {
        for(int j = i - 1; j >= 0; j--) {
            if(all[j] < all[i]) {
                dp[i] = max(dp[j] + 1, dp[i]);
            }
        }
    }
    int ans = 0, value = -1e9, id_mx = 0;
    for(int i = 0; i < dp.size(); i++) {
        if(dp[i] > ans) {
            ans = dp[i];
            value = all[i];
            id_mx = i;
        }
    }
    cout << ans << endl;
    vector<int> pos;
    pos.push_back(id_mx);
    ans--;
    for(int i = id_mx - 1; i >= 0; i--) {
        if(dp[i] == ans && all[i] < value) {
            pos.push_back(i);
            ans--;
            value = all[i];
        }
    }
    for(int i = (int) pos.size() - 1; i >= 0; i--) {
        cout << all[pos[i]] << " ";
    }
}