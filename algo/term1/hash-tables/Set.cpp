#include <iostream>
#include <vector>
#include <algorithm>
#include <list>
 
using namespace std;
 
vector<list<int>> all;
static const int size_ = 1e4 + 121;
 
int hash_(int &x) {
    return abs(x % size_);
}
 
string exist_(int &x) {
    int i = hash_(x);
    bool have = false;
    if (!all[i].empty()) {
        for (auto it = all[i].begin(); it != all[i].end(); it++) {
            if (*it == x) {
                have = true;
                break;
            }
        }
    }
    return ((have) ? ("true") : ("false"));
}
 
void delete_(int &x) {
    int i = hash_(x);
    if (!all[i].empty()) {
        for (auto it = all[i].begin(); it != all[i].end(); it++) {
            if (*it == x) {
                all[i].erase(it);
                break;
            }
        }
    }
}
 
int main() {
    freopen("set.in", "r", stdin);
    freopen("set.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    string s;
    all.resize(size_);
    while (cin >> s) {
        int x;
        cin >> x;
        if (s[0] == 'i') {
            if (exist_(x) == "false") {
                int i = hash_(x);
                all[i].push_back(x);
            }
        }
        else if (s[0] == 'e') {
            cout << exist_(x) << '\n';
        }
        else if (s[0] == 'd') {
            delete_(x);
        }
    }
}
