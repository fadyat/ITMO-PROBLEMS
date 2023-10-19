#include <iostream>
#include <vector>
#include <algorithm>
 
using namespace std;
 
int main() {
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);
    int n, m;
    cin >> n >> m;
    vector<vector<bool>> all (n, vector<bool> (n));
    for (int i = 0; i < m; i++) {
        int x, y;
        cin >> x >> y;
        x--, y--;
        all[x][y] = true;
    }
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            cout << all[i][j] << " ";
        }
        cout << '\n';
    }
}
