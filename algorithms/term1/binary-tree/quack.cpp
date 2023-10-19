#include <iostream>
#include <vector>
#include <algorithm>
#include <cmath>
#include <map>
#include <queue>

using namespace std;

unsigned short calc(string &s, queue<unsigned short> &all) {
    reverse(s.begin(), s.end());
    unsigned short x = 0;
    for (unsigned short i = 0; i < (unsigned short) s.size(); i++) {
        x += (s[i] - '0') * (unsigned short) (pow(10, i));
    }
    return x;
}

string copy(string s, int j) {
    string s1;
    for (int i = j; i < (int) s.size(); i++) {
        s1.push_back(s[i]);
    }
    return s1;
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("quack.in", "r", stdin);
    freopen("quack.out", "w", stdout);
    queue<unsigned short> all;
    vector<unsigned short> _register('z' - 'a' + 1);
    vector<string> operations;
    map<string, int> labels;
    int j = 0;
    string s;
    while (cin >> s) {
        if (s[0] == ':') {
            string s1 = copy(s, 1);
            labels[s1] = j;
        }
        else {
            operations.push_back(s);
            j++;
        }
    }
    j = 0;
    while (j < operations.size()) {
        s = operations[j];
        if (s[0] >= '0' && s[0] <= '9') {
            all.push(calc(s, all));
        }
        if (s == "+") {
            unsigned short x1 = all.front();
            all.pop();
            unsigned short x2 = all.front();
            all.pop();
            all.push(x1 + x2);
        }
        else if (s == "-") {
            unsigned short x1 = all.front();
            all.pop();
            unsigned short x2 = all.front();
            all.pop();
            all.push(x1 - x2);
        }
        else if (s == "*") {
            unsigned short x1 = all.front();
            all.pop();
            unsigned short x2 = all.front();
            all.pop();
            all.push(x1 * x2);
        }
        else if (s == "/") {
            unsigned short x1 = all.front();
            all.pop();
            unsigned short x2 = all.front();
            all.pop();
            all.push((x2 == 0) ? (0) : (x1 / x2));
        }
        else if (s == "%") {
            unsigned short x1 = all.front();
            all.pop();
            unsigned short x2 = all.front();
            all.pop();
            all.push((x2 == 0) ? (0) : (x1 % x2));
        }
        else if (s[0] == '>') {
            _register[s[1] - 'a'] = all.front();
            all.pop();
        }
        else if (s[0] == '<') {
            all.push(_register[s[1] - 'a']);
        }

        else if (s.size() == 1 && s == "P") {
            cout << all.front() << endl;
            all.pop();
        }
        else if (s[0] == 'P') {
            cout << _register[s[1] - 'a'] << endl;
        }
        else if (s.size() == 1 && s == "C") {
            cout << (char) (all.front() % 256);
            all.pop();
        }
        else if (s[0] == 'C') {
            cout << (char) (_register[s[1] - 'a'] % 256);
        }
        else if (s[0] == 'J') {
            string s1 = copy(s, 1);
            j = labels[s1];
            j--;
        }
        else if (s[0] == 'Z') {
            if (_register[s[1] - 'a'] == 0) {
                string s1 = copy(s, 2);
                j = labels[s1];
                j--;
            }
        }
        else if (s[0] == 'E') {
            if (_register[s[1] - 'a'] == _register[s[2] - 'a']) {
                string s1 = copy(s, 3);
                j = labels[s1] - 1;
            }
        }
        else if (s[0] == 'G') {
            if (_register[s[1] - 'a'] > _register[s[2] - 'a']) {
                string s1 = copy(s, 3);
                j = labels[s1] - 1;
            }
        }
        else if (s == "Q") {
            return 0;
        }
        j++;
    }
}