#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

void file_cin(){
    freopen("radixsort.in", "r", stdin);
    freopen("radixsort.out", "w", stdout);
}

void cs(vector<string> &a, int t, int n, int m){
    vector<vector<string>> all1('z' - 'a' + 1);
    m--;
    for(int i = t - 1; i >= 0; i--) {
        vector<vector<string>> all2('z' - 'a' + 1);
        if(i == t - 1) {
            for (int j = 0; j < n; j++) {
                string s = a[j];
                all2[s[m] - 'a'].push_back(a[j]);
            }
        }
        else {
            for(int j = 0; j < all1.size(); j++) {
                for(int q = 0; q < all1[j].size(); q++) {
                    string s = all1[j][q];
                    all2[s[m] - 'a'].push_back(s);
                }
            }
        }
        all1 = all2;
        m--;
    }
    for(int i = 0; i < all1.size(); i++)
        for(int j = 0; j < all1[i].size(); j++)
            cout << all1[i][j] << endl;
}

void solve(){
    int n, m, t;
    cin >> n >> m >> t;
    vector<string> a(n);
    for(int i = 0; i < n; i++)
        cin >> a[i];

    cs(a, t, n, m);
}
int main() {
    solve();
    return 0;
}