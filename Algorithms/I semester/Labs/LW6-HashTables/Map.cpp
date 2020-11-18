#include <iostream>
#include <vector>
#include <algorithm>
#include <list>
 
using namespace std;
 
typedef long long ll;
 
static const ll size_ = 1e3 + 121;
static const ll p = 31;
 
ll hash_(string &s) {
    ll ans = 0;
    for (char c : s) {
        ll x = (int) abs(c - 'a' + 1);
        ans = (ans * p + x) % size_;
    }
    return ans;
}
 
void put_(string &x, string &y, vector<list<pair<string, string>>> &all, ll &where) {
    if (all[where].empty()) {
        all[where].push_back(make_pair(x, y));
    }
    else {
        bool was_find = false;
        for (auto it = all[where].begin(); it != all[where].end(); it++) {
            if ((*it).first == x) {
                was_find = true;
                (*it).second = y;
                break;
            }
        }
        if (!was_find) {
            all[where].push_back({x, y});
        }
    }
}
 
string get_(string &x, ll &where, vector<list<pair<string, string>>> &all) {
    if (!all[where].empty()) {
        for (auto it = all[where].begin(); it != all[where].end(); it++) {
            if ((*it).first == x) {
                return (*it).second;
            }
        }
    }
    return "none";
}
 
void delete_(string &x, ll &where, vector<list<pair<string, string>>> &all) {
    if (!all[where].empty()) {
        for (auto it = all[where].begin(); it != all[where].end(); it++) {
            if ((*it).first == x) {
                all[where].erase(it);
                break;
            }
        }
    }
}
 
int main() {
    freopen("map.in", "r", stdin);
    freopen("map.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    string s;
    vector<list<pair<string, string>>> all(size_);
    while (cin >> s) {
        string key;
        cin >> key;
        ll where = hash_(key);
        if (s[0] == 'p') { // put
            string value;
            cin >> value;
            put_(key, value, all, where);
        }
        else if (s[0] == 'g') { // get
            cout << get_(key, where, all) << '\n';
        }
        else if (s[0] == 'd') { // delete
            delete_(key, where, all);
        }
    }
}
