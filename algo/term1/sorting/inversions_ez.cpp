    #include <iostream>
    #include <string.h>
    #include <algorithm>
    #include <vector>
    #include <math.h>
    #include <iomanip>
    #include <stack>
    #include <set>
    typedef long long ll;
    typedef unsigned long long ull;
    #define all(v) v.begin(), v.rend()
    #define rall(v) v.rbegin(), v.rend()
    using namespace std;

    int partition_(vector<int> &vec, int l, int r){
        int m = vec[(l + r)/2];
        int i = l;
        int j = r;
        while(i <= j){
            while(vec[i] < m) i++;
            while(vec[j] > m) j--;
            if(i >= j) break;
            swap(vec[i++], vec[j--]);
        }
        return j;
    }
    void quickSort(vector<int> &a, int l, int r){
        if(l < r){
            int q = partition_(a, l, r);
            quickSort(a, l, q);
            quickSort(a, q + 1, r);
        }
    }

    void mergeSort(vector<int> &vec, int l, int r, ll &cnt) {
        if (l == r)
            return;

        int m = (l + r) / 2;
       // cout << l << " * " << r << endl;
        mergeSort(vec, l, m, cnt);
        mergeSort(vec, m + 1, r, cnt);

        vector<int> new_vec;
        for (int i = l, j = m + 1; i <= m || j <= r;) {
            if (i > m) {
                new_vec.push_back(vec[j++]);
            }
            else if (j > r) {
                new_vec.push_back(vec[i++]);
            }
            else if (vec[i] <= vec[j]) {
                new_vec.push_back(vec[i++]);
            }
            else {
                new_vec.push_back(vec[j++]);
                cnt += m - i + 1;
            }
        }
        //cout << l << " === " << r << endl;
        //cout << cnt << endl << endl;

        for (int i = 0; i < new_vec.size(); i++)
            vec[l + i] = new_vec[i];
    }

    void file_cin(){
        freopen("inversions.in", "r", stdin);
        freopen("inversions.out", "w", stdout);
    }

    int main() {
        ios_base::sync_with_stdio(0);
        cin.tie(0);
        cout.tie(0);
        file_cin();
        int n;
        cin >> n;
        vector<int> vec(n);
        int mx = 0;
        for(int i = 0; i < n; i++){
            cin >> vec[i];
        }
        ll cnt  = 0;
        mergeSort(vec, 0, (int)vec.size() - 1, cnt);
        cout << cnt << endl;
    }
    /* ans = 1 + 1 + /////// + 2
     * 10
     * 1 8 2 1 4 7 3 2 3 6
     * [1] [8] [2] [1] [4] [7] [3] [2] [3] [6]
     * [1 8] / [2] / [1 4]  /////// [&3 &7] / [2] / [3 6]
     * [1 &2 &8] / [1 4] //////// [2 3 7]
     */
