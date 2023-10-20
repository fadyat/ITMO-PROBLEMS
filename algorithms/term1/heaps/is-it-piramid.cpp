#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

bool check(vector<int> &a){
    for(int i = 0; i < a.size(); i++){
        if(2 * i + 1 < a.size()){
            if(a[i] > a[2 * i + 1]){
                //cout << a[i] << " ";
                return false;
            }
        }
        if(2 * i + 2 < a.size()){
            if(a[i] > a[2 * i + 2]){
                //cout << a[i] << " ";
                return false;
            }
        }
    }
    return true;
}

int main()
{
    freopen("isheap.in", "r", stdin);
    freopen("isheap.out", "w", stdout);
    int n;
    cin >> n;
    vector<int> a(n);
    for(int i = 0; i < n; i++){
        cin >> a[i];
    }

    if(check(a)) {
        cout << "YES" << endl;
    }
    else{
        cout << "NO";
    }
    return 0;
}