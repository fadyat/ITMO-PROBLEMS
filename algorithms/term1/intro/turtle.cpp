#include <bits/stdc++.h>
typedef long long ll;
#define all(v) v.begin(), v.end()
#define rall(v) v.rbegin(), v.rend()
using namespace std;
int main()
{
  freopen("turtle.in", "r", stdin); 
  freopen("turtle.out", "w", stdout); 
  ios_base::sync_with_stdio(0); 
  cin.tie(0); cout.tie(0);
  int w, h;
  cin >> h >> w;
  vector< vector<int> > vec(h + 2, vector<int> (w + 2));
  for(int i = 1; i < h + 1; i++){
    for(int j = 1; j < w + 1; j++)
      cin >> vec[i][j];
  }

  /*for(int i = 0; i < h + 2; i++){
    for(int j = 0; j < w + 2; j++)
      cout << vec[i][j] << " ";  
    cout << endl;
  }
  cout << endl;*/

  for(int i = h; i >= 1; i--){
    for(int j = 1; j <= w; j++){
      vec[i][j] = max(vec[i][j] + vec[i + 1][j], vec[i][j] + vec[i][j - 1]);
    }
  }

  /*for(int i = 0; i < h + 2; i++){
    for(int j = 0; j < w + 2; j++)
      cout << vec[i][j] << " ";  
    cout << endl;
  }*/

  cout << vec[1][w];
}