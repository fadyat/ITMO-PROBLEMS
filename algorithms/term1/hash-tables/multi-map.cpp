#include <iostream>
#include <vector>

using namespace std;

vector<vector<vector<vector<string>>>> multiMap;
vector<vector<string>> Map;

static const int size_1 = 1e2 + 121;
static const int size_2 = 321;
static const int p = 31;

int hash_(string &s) {
    int ans = 0;
    for (char c : s) {
        int x = (int) abs(c - 'a' + 1);
        ans = (ans * p + x) % size_1;
    }
    return ans;
}

void put(string &key, string &value) {
    int i = hash_(key);
    for (int j = 0; j < size_2; j++) {
        if (Map[i][j] == key) {
            int k = hash_(value);
            for (string &q : multiMap[i][j][k]) {
                if (q == value) {
                    return;
                }
            }
            multiMap[i][j][k].push_back(value);
            return;
        }
        else if (Map[i][j].empty()) {
            Map[i][j] = key;
            multiMap[i][j].resize(size_1);
            int k = hash_(value);
            multiMap[i][j][k].push_back(value);
            return;
        }
    }
}

void delete_(string &key, string &value) {
    int i = hash_(key);
    for (int j = 0; j < size_2; j++) {
        if (Map[i][j] == key) {
            int k = hash_(value);
            for (int q = 0; q < multiMap[i][j][k].size(); q++) {
                if (multiMap[i][j][k][q] == value) {
                    vector<string> tmp;
                    for (string &z : multiMap[i][j][k]) {
                        if (z != value) {
                            tmp.push_back(z);
                        }
                    }
                    multiMap[i][j][k] = tmp;
                    return;
                }
            }
        }
    }
}

void deleteAll(string &key) {
    int i = hash_(key);
    for (int j = 0; j < size_2; j++) {
        if (Map[i][j] == key) {
            Map[i][j] = "";
            multiMap[i][j].clear();
            return;
        }
    }
}

vector<string> get(string &key) {
    vector<string> ans;
    int i = hash_(key);
    for (int j = 0; j < size_2; j++) {
        if (Map[i][j] == key) {
            for (int k = 0; k < size_1; k++) {
                for (string &q : multiMap[i][j][k]) {
                    ans.push_back(q);
                }
            }
            return ans;
        }
    }
    return ans;
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("multimap.in", "r", stdin);
    freopen("multimap.out", "w", stdout);
    multiMap.resize(size_1);
    Map.resize(size_1, vector<string>(size_2));
    for (int i = 0; i < size_1; i++) {
        multiMap[i].resize(size_2);
    }
    string s;
    while (cin >> s) {
        string key;
        cin >> key;
        if (s[0] == 'p') {
            string value;
            cin >> value;
            put(key, value);
        }
        if (s[0] == 'd' && s.back() == 'e') {
            string value;
            cin >> value;
            delete_(key, value);
        }
        if (s[0] == 'd' && s.back() == 'l') {
            deleteAll(key);
        }
        if (s[0] == 'g') {
            vector<string> ans = get(key);
            cout << ans.size() << " ";
            for (string &i : ans) {
                cout << i << " ";
            }
            cout << '\n';
        }
    }
}
