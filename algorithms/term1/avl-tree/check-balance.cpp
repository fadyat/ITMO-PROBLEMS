#include <iostream>
#include <vector>

using namespace std;

int main() {
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("balance.in", "r", stdin);
    freopen("balance.out", "w", stdout);
    int n;
    cin >> n;
    vector<int> k(n);
    vector<int> l(n);
    vector<int> r(n);
    for (int i = 0; i < n; i++) {
        cin >> k[i] >> l[i] >> r[i];
        l[i]--;
        r[i]--;
    }
    vector<int> h(n, 1);
    vector<int> balance(n, 0);
    /*
     * for left subtree -
     * for right subtree +
     */
    for (int i = n - 1; i >= 0; i--) {
        //* no children
        if (l[i] == -1 && r[i] == -1) {
            balance[i] = 0;
        }
        //* go right
        else if (l[i] == -1) {
            balance[i] = h[r[i]];
            h[i] = h[r[i]] + 1;
        }
        //* go left
        else if (r[i] == -1) {
            balance[i] = -h[l[i]];
            h[i] = h[l[i]] + 1;
        }
        //* have left and right
        else {
            balance[i] = h[r[i]] - h[l[i]];
            h[i] = max(h[r[i]] + 1, h[l[i]] + 1);
        }
    }
    for (int i = 0; i < n; i++) {
        cout << balance[i] << '\n';
    }
}