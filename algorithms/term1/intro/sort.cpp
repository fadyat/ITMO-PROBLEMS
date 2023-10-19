#include <bits/stdc++.h>
typedef long long ll;
#define all(v) v.begin(), v.end()
#define rall(v) v.rbegin(), v.rend()
using namespace std;
int main()
{
  freopen("smallsort.in", "r", stdin); 
  freopen("smallsort.out", "w", stdout); 
  ios_base::sync_with_stdio(0); 
  cin.tie(0); cout.tie(0);
  int n;
  cin >> n;
  vector<int> vec(n);
  for(int i = 0; i < n; i++){
    cin >> vec[i];
  }

  for(int i = 0; i < n; i++){
    for(int j = 0; j < n - i - 1; j++){
      if(vec[j] > vec[j+1])
        swap(vec[j], vec[j+1]);
    }
  }

  //SORRY =)
  for(int i = 0; i < n; i++){
    cout << vec[i] << " ";
  }
}