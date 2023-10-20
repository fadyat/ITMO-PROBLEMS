#include <iostream>
#include <algorithm>
#include <vector>
#include <cmath>
using namespace std;

int partition(vector<int> &vec, int l, int r){
    int m = vec[(l + r) / 2];
    int i = l;
    int j = r;
    while(i <= j){
        while(vec[i] < m){
            i++;
        }
        while(vec[j] > m){
            j--;
        }
        if(i >= j) break;
        swap(vec[i], vec[j]);
        i++;
        j--;
    }
    return j;
}

int find_k(vector<int> &vec, int l, int r, int k){
    if(r == l)
        return vec[l];

    int m = partition(vec, l, r);
    if(m - l >= k)
        return find_k(vec, l, m, k);
    else
        return find_k(vec, m + 1, r, k - (m - l + 1));
}

void file_cin(){
    freopen("kth.in", "r", stdin);
    freopen("kth.out", "w", stdout);
}

int main() {
    file_cin();
    int n, k;
    cin >> n >> k;
    vector<int> vec(n);
    int A, B, C, a, b;
    cin >> A >> B >> C >> a >> b;
    vec[0] = a;
    vec[1] = b;
    for(int i = 2; i < n; i++)
        vec[i] = A * vec[i - 2] + B * vec[i - 1] + C;

    cout << find_k(vec, 0, n - 1, k - 1);
}
