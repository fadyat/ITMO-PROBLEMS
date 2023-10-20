#include <bits/stdc++.h>
typedef long long ll;
#define all(v) v.begin(), v.end()
#define rall(v) v.rbegin(), v.rend()
using namespace std;
int main()
{
  freopen("sortland.in", "r", stdin); 
  freopen("sortland.out", "w", stdout); 
  ios_base::sync_with_stdio(0); 
  cin.tie(0); cout.tie(0);
  int n;
  cin >> n;
  vector< pair<double, int> > vec(n);
  for(int i = 0; i < n; i++){
    cin >> vec[i].first;
    vec[i].second = i + 1;
  }

  for(int i = 0; i < n; i++){
    for(int j = 0; j < n - i - 1; j++){
      if(vec[j].first > vec[j+1].first)
        swap(vec[j], vec[j+1]);
    }
  }

  //SORRY =)
  cout << vec[0].second << " " << vec[n / 2].second << " " << vec[n - 1].second;
}