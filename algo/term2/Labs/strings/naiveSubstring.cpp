#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int main() {
    freopen("search1.in", "r", stdin);
    freopen("search1.out", "w", stdout);
    string s1, s2;
    cin >> s1 >> s2;
    vector<int> ans;
    for (int i = 0; i < (int) (s2.size() - s1.size() + 1); i++) {
        int j = 0;
        while (j < s1.size() && s1[j] == s2[i + j]) {
            j++;
        }
        if (j == s1.size()) {
            ans.push_back(i);
        }
    }
    cout << ans.size() << endl;
    for (int i : ans) {
        cout << i + 1 << " ";
    }
}