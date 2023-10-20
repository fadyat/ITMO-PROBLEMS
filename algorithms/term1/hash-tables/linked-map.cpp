#include <iostream>
#include <list>
#include <vector>
#include <algorithm>

using namespace std;

static const int size_ = 1e3 + 121;
static const int p = 31;
vector<list<pair<string, vector<string>>>> linkedMap;
string before = "none";

int hash_ (string &key) {
    int ans = 0;
    for(char c : key) {
        int x = (int) abs(c - 'a' + 1);
        ans = (ans * p + x) % size_;
    }
    return ans;
}

void update_next (string &upNext, string &nVal) {
    int x1 = hash_(upNext);
    for (auto it = linkedMap[x1].begin(); it != linkedMap[x1].end(); it++) {
        if((*it).first == upNext) {
            (*it).second[2] = nVal;
            break;
        }
    }
}

void update_prev (string &upPrev, string &nVal) {
    int x1 = hash_(upPrev);
    for (auto it = linkedMap[x1].begin(); it != linkedMap[x1].end(); it++) {
        if((*it).first == upPrev) {
            (*it).second[0] = nVal;
            break;
        }
    }
}

void put (string &key, string &value) {
    int x = hash_(key);
    if (linkedMap[x].empty()) {
        linkedMap[x].push_back({key, {before, value, "none"}});
        if (before != "none") {
            update_next(before, key);
        }
        before = key;
    }
    else {
        bool have = false;
        for (auto it = linkedMap[x].begin(); it != linkedMap[x].end(); it++) {
            if ((*it).first == key) {
                have = true;
                (*it).second[1] = value;
                break;
            }
        }
        if(!have) {
            linkedMap[x].push_back({key, {before, value, "none"}});
            update_next(before, key);
            before = key;
        }
    }
}

string get(string &key, int i) {
    int x = hash_(key);
    if(!linkedMap[x].empty()) {
        for (auto it = linkedMap[x].begin(); it != linkedMap[x].end(); it++) {
            if((*it).first == key) {
                if (i == 1) {
                    return (*it).second[i];
                }
                string s1 = (*it).second[i];
                int x1 = hash_(s1);
                for (auto it1 = linkedMap[x1].begin(); it1 != linkedMap[x1].end(); it1++) {
                    if((*it1).first == s1) {
                        return (*it1).second[1];
                    }
                }
            }
        }
    }
    return "none";
}

void delete_(string &key) {
    int x = hash_(key);
    if (!linkedMap[x].empty()) {
        for (auto it = linkedMap[x].begin(); it != linkedMap[x].end(); it++) {
            if ((*it).first == key) {
                if ((*it).second[0] != "none") {
                    update_next((*it).second[0], (*it).second[2]);
                }
                if ((*it).second[2] != "none") {
                    update_prev((*it).second[2], (*it).second[0]);
                }
                if (before == (*it).first) {
                    before = (*it).second[0];
                }
                linkedMap[x].erase(it);
                break;
            }
        }
    }
}

int main() {
    freopen("linkedmap.in", "r", stdin);
    freopen("linkedmap.out", "w", stdout);
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    string cmd;
    linkedMap.resize(size_);
    // string -> prev(key), value, next(key)
    while (cin >> cmd) {
        string key;
        cin >> key;
        if (cmd[0] == 'p' && cmd.back() == 't') {
            string value;
            cin >> value;
            put(key, value);
        }
        else if (cmd[0] == 'd') {
            delete_(key);
        }
        else if (cmd[0] == 'g') {
            cout << get(key, 1) << '\n';
        }
        else if (cmd[0] == 'p') {
            cout << get(key, 0) << '\n';
        }
        else {
            cout << get(key, 2) << '\n';
        }
    }
}
