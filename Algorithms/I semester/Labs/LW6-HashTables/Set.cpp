#include <iostream>
#include <vector>
#include <algorithm>
#include <list>

using namespace std;

static const int size_ = 1e4 + 121;

int hash_(int &x) {
    return abs(x % size_);
}

string exist_(int &x, int &where, vector<list<int>> &all) {
    bool have = false;
    if ((int) all[where].size() != 0) {
        for (auto it = all[where].begin(); it != all[where].end(); it++) {
            if (*it == x) {
                have = true;
                break;
            }
        }
    }
    return ((have) ? ("true") : ("false"));
}

void delete_(int &x, int &where, vector<list<int>> &all) {
    if ((int) all[where].size() != 0) {
        for (auto it = all[where].begin(); it != all[where].end(); it++) {
            if (*it == x) {
                all[where].erase(it);
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
    vector<list<int>> all(size_);
    while (cin >> s) {
        int x;
        cin >> x;
        int where = hash_(x);
        if (s[0] == 'i') {
            if (exist_(x, where, all) == "false") {
                all[where].push_back(x);
            }
        }
        else if (s[0] == 'e') {
            cout << exist_(x, where, all) << '\n';
        }
        else if (s[0] == 'd') {
            delete_(x, where, all);
        }
    }
}