#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
int main() {
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);
    int n, m;
    cin >> n >> m;
    vector<vector<int>> all (n, vector<int> (n));
    while (m--) {
        int x, y;
        cin >> x >> y;
        x--, y--;
        all[x][y]++;
    }
 
 
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            if (all[i][j] + all[j][i] >= 2) {
                cout << "YES";
                return 0;
            }
        }
    }
    cout << "NO";
    return 0;
}
