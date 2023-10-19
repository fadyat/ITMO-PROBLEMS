#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
int main() {
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);
    int n;
    cin >> n;
    vector<vector<int>> all (n, vector<int> (n));
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            cin >> all[i][j];
        }
    }
 
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            if (all[i][j] != all[j][i]) {
                cout << "NO";
                return 0;
            }
        }
    }
    for (int i = 0; i < n; i++) {
        if (all[i][i]) {
            cout << "NO";
            return 0;
        }
    }
    cout << "YES";
    return 0;
}
