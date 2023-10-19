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

    void file_cin(){
        freopen("antiqs.in", "r", stdin);
        freopen("antiqs.out", "w", stdout);
    }

    int main() {
        ios_base::sync_with_stdio(0);
        cin.tie(0);
        cout.tie(0);
        file_cin();
        int n;
        cin >> n;
        vector<int> vec;
        for(int i = 0; i < n; i++){
            vec.push_back(i + 1);
            swap(vec[(vec.size() - 1) / 2], vec.back());
        }
        for(int i = 0; i < n; i++){
            cout << vec[i] << " ";
        }
    }
