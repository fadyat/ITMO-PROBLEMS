#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

string LCS(string s1, string s2, int n, int m) {
    vector<vector<bool>> left(n + 1, vector<bool> (m + 1, false));
    vector<vector<int>> lcs(2, vector<int> (m + 1, 0));
    for (int i = 1; i < n + 1; i++) {
        lcs[1][0] = 0;
        for (int j = 1; j < m + 1; j++) {
            lcs[0][j] = lcs[1][j];
            if (s1[i - 1] == s2[j - 1]) {
                lcs[1][j] = lcs[0][j - 1] + 1;
            }
            else {
                if (lcs[1][j - 1] > lcs[0][j]) {
                    left[i][j] = true;
                    lcs[1][j] = lcs[1][j - 1];
                }
                else {
                    lcs[1][j] = lcs[0][j];
                }
            }
        }
    }
    string answer;
    while (n > 0 && m > 0) {
        if (s1[n - 1] == s2[m - 1]) {
            answer.push_back(s1[n - 1]);
            n--;
            m--;
        }
        else if (left[n][m]) {
            m--;
        }
        else {
            n--;
        }
    }
    reverse(answer.begin(), answer.end());
    return answer;
}

int main() {
    string s1, s2;
    cin >> s1 >> s2;
    cout << LCS(s1, s2, s1.size(), s2.size()) << endl;
}
