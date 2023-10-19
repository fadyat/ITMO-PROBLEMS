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
    void mergeSort(vector<pair<string, string>> &vec, int l, int r) {
        if (l == r)
            return;

        int m = (l + r) / 2;
        mergeSort(vec, l, m);
        mergeSort(vec, m + 1, r);

        vector<pair<string, string>> new_vec;
        for (int i = l, j = m + 1; i <= m || j <= r;) {
            if (i > m) {
                new_vec.push_back({vec[j].first, vec[j].second});
                j++;
            }
            else if (j > r) {
                new_vec.push_back({vec[i].first, vec[i].second});
                i++;
            }
            else if (vec[i].first <= vec[j].first) {
                new_vec.push_back({vec[i].first, vec[i].second});
                i++;
            }
            else {
                new_vec.push_back({vec[j].first, vec[j].second});
                j++;
            }
        }


        for (int i = 0; i < new_vec.size(); i++) {
            vec[l + i].first = new_vec[i].first;
            vec[l + i].second = new_vec[i].second;
        }
    }
    void file_cin(){
        freopen("race.in", "r", stdin);
        freopen("race.out", "w", stdout);
    }
    int main() {
        ios_base::sync_with_stdio(0);
        cin.tie(0);
        cout.tie(0);
        file_cin();
        int n;
        cin >> n;
        vector<pair<string, string> > vec(n);
        for (int i = 0; i < n; i++) {
            cin >> vec[i].first >> vec[i].second;
        }
        mergeSort(vec, 0, n - 1);

        string s = vec[0].first;
        for(int i = 0; i < n; i++){
            if(vec[i].first != s || i == 0){
                s = vec[i].first;
                cout << "=== " << vec[i].first << " ===" << '\n' << vec[i].second << '\n';
            }
            else{
                cout << vec[i].second << '\n';
            }
        }

    }
