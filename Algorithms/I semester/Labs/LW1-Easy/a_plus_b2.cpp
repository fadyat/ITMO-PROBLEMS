#include <bits/stdc++.h>
typedef long long ll;
#define all(v) v.begin(), v.end()
#define rall(v) v.rbegin(), v.rend()
using namespace std;
int main()
{
  freopen("aplusbb.in", "r", stdin); 
  freopen("aplusbb.out", "w", stdout); 
  ios_base::sync_with_stdio(0); 
  cin.tie(0); cout.tie(0);
  ll a, b;
  cin >> a >> b;
  cout << a + b * b;
}