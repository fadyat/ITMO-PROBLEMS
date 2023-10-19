#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

vector<int> a;

void siftUp(int v){
    while(v) {
        if(a[v] > a[(v - 1) / 2]) {
            swap(a[v], a[(v - 1) / 2]);
            v = (v - 1) / 2;
        }
        else
            break;
    }
}

void siftDown(int v, int n) {
    while(2 * v + 1 < n) {
        int l = 2 * v + 1;
        int r = 2 * v + 2;
        int j = l;
        if(r < n && a[r] > a[l]) {
            j = r;
        }
        if(a[v] > a[j]) {
            break;
        }
        swap(a[v], a[j]);
        v = j;
    }
}

void build(int my_sz) {
    for(int i = my_sz / 2; i >= 0; i--){
        siftDown(i, my_sz);
    }
}

void sort_(){
    build(a.size());
    int sz = a.size();
    int my_sz = sz;
    for(int i = 0; i < sz - 1; i++){
        swap(a[0], a[sz - i - 1]);
        my_sz--;
        siftDown(0, my_sz);
    }
}

void push(int x){
    a.push_back(x);
    siftUp((int)a.size() - 1);
}
int main()
{
    freopen("sort.in", "r", stdin);
    freopen("sort.out", "w", stdout);
    int n;
    cin >> n;
    for(int i = 0; i < n; i++){
        int x;
        cin >> x;
        push(x);
    }
    sort_();
    for(int i = 0; i < n; i++){
        cout << a[i] << " ";
    }
}