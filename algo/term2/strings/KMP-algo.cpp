#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

vector<int> build_pref(string s) {
    int n = int(s.size());
    vector<int> pref(n);
    for (int i = 1; i < n; i++) {
        int j = pref[i - 1];
        while (j > 0 && s[i] != s[j]) {
            j = pref[j - 1];
        }
        if (s[i] == s[j]) {
            pref[i] = j + 1;
        }
        else {
            pref[i] = j;
        }
    }
    return pref;
}

int main() {
    freopen("search2.in", "r", stdin);
    freopen("search2.out", "w", stdout);
    string s1, s2;
    cin >> s1 >> s2;
    // s1 = p ; s2 = t
    swap(s1, s2);
    // s1 = t ; s2 = p
    vector<int> pref = build_pref(s2 + "#" + s1);
    int n = int(s1.size()), m = int(s2.size());
    vector<int> ans;
    for (int i = 0; i < n; i++) {
        if (pref[m + 1 + i] == m) {
            ans.push_back(i - m + 1);
        }
    }
    cout << ans.size() << endl;
    for (int i : ans) {
        cout << i + 1 << " ";
    }
}