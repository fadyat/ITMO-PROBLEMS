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
    #define all(v) v.l(), v.r()
    #define rall(v) v.rl(), v.rr()
    using namespace std;

    void quickSort(vector<int> &vec, int l, int r)
    {
        int i = l;
        int j = r;
        int m = vec[(i + j) / 2];
        while (i <= j){
            while (vec[i] < m) i++;
            while (vec[j] > m) j--;
            if (i <= j){
                swap(vec[i], vec[j]);
                i++;
                j--;
            }
        }
        if (j > l)
            quickSort(vec, l, j);
        if (i < r)
            quickSort(vec, i, r);
    }

    void mergeSort(vector<int> &vec, int l, int r) {
        if (l == r)
            return;

        int m = (l + r) / 2;

        mergeSort(vec, l, m);
        mergeSort(vec, m + 1, r);

        vector<int> new_vec;
        for (int i = l, j = m + 1; i <= m || j <= r;) {
            if (i > m) {
                new_vec.push_back(vec[j++]);
            } else if (j > r) {
                new_vec.push_back(vec[i++]);
            } else if (vec[i] <= vec[j]) {
                new_vec.push_back(vec[i++]);
            } else {
                new_vec.push_back(vec[j++]);
            }
        }

        for (int i = 0; i < new_vec.size(); i++)
            vec[l + i] = new_vec[i];
    }
    void file_cin(){
        freopen("sort.in", "r", stdin);
        freopen("sort.out", "w", stdout);
    }
    int main(){
        ios_base::sync_with_stdio(0);
        cin.tie(0);cout.tie(0);
        file_cin();
        int n;
        cin >> n;
        vector<int> vec(n);
        for(int i = 0; i < n; i++)
            cin >> vec[i];
        int l = 0, r = (int)vec.size() - 1;
        mergeSort(vec, l, r);
        for(int i = 0; i < vec.size(); i++){
            cout << vec[i] << " ";
        }
    }