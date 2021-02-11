#include <iostream>
#include <vector>
#include <algorithm>
#include <queue>

using namespace std;

vector<vector<int>> g;
vector<bool> used;
vector<int> path;
vector<int> parents;

void bfs (int v) {
    used[v] = true;
    queue<int> q;
    parents[v] = -1;
    q.push(v);
    while (!q.empty()) {
        int vv = q.front();
        q.pop();
        for (auto i = g[vv].begin(); i != g[vv].end(); i++) {
            int to = *i;
            if (!used[to]) {
                used[to] = true;
                q.push(to);
                path[to] = path[vv] + 1;
                parents[to] = vv;
            }
        }
    }
}

int main() {
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);
    int n, m;
    cin >> n >> m;

    vector<string> field(n);
    for (int i = 0; i < n; i++) {
        cin >> field[i];
    }

    g.resize(n * m);
    path.resize(n * m);
    used.resize(n * m);
    parents.resize(n * m);
    // analise our field
    int start = 0, end = 0;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < m; j++) {
            if (field[i][j] == 'S') {
                start = i * m + j;
            }
            if (field[i][j] == 'T') {
                end = i * m + j;
            }
            if (field[i][j] != '#') {
                // number of vertex: i * m  + j
                int v = i * m + j;
                if (i > 0 && field[i - 1][j] != '#') /* have a U cell */{
                    g[v].push_back((i - 1) * m + j);
                }
                if (i < n - 1 && field[i + 1][j] != '#') /* have a D cell */ {
                    g[v].push_back((i + 1) * m + j);
                }
                if (j > 0 && field[i][j - 1] != '#') /* have a L cell */ {
                    g[v].push_back(i * m + j - 1);
                }
                if (j < m - 1 && field[i][j + 1] != '#') /* have a R cell */ {
                    g[v].push_back(i * m + j + 1);
                }
            }
        }
    }
    bfs(start);
    if (path[end] == 0) {
        cout << "-1";
        return 0;
    }
    vector<int> LocalPath;
    for (int w = end; w != -1; w = parents[w]) {
        LocalPath.push_back(w);
    }
    reverse(LocalPath.begin(), LocalPath.end());
    string ans;
    if (m != 1) {
        for (int i = 1; i < (int) LocalPath.size(); i++) {
            if (LocalPath[i] - LocalPath[i - 1] == 1) {
                ans.push_back('R');
            } else if (LocalPath[i] - LocalPath[i - 1] == -1) {
                ans.push_back('L');
            } else if (LocalPath[i] > LocalPath[i - 1]) {
                ans.push_back('D');
            } else if (LocalPath[i] < LocalPath[i - 1]) {
                ans.push_back('U');
            }
        }
    }
    else {
        for (int i = 1; i < (int) LocalPath.size(); i++) {
            if (LocalPath[i] > LocalPath[i - 1]) {
                ans.push_back('D');
            } else if (LocalPath[i] < LocalPath[i - 1]) {
                ans.push_back('U');
            }
        }
    }
    cout << ans.size() << endl << ans << endl;
}