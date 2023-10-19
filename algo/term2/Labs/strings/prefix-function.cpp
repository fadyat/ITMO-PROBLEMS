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
    freopen("prefix.in", "r", stdin);
    freopen("prefix.out", "w", stdout);
    string s;
    cin >> s;
    vector<int> pref = build_pref(s);
    for (int i : pref) {
        cout << i << " ";
    }
}