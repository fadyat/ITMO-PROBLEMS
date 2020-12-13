#include <iostream>
#include <vector>
#include <algorithm>
#include <fstream>
#include <queue>
#include <set>

using namespace std;

int edge;
vector<set<int>> g;

int edges () {
    edge = 0;
    for (int i = 0; i < (int) g.size(); i++) {
        edge += (int) g[i].size();
    }
    return (edge / 2);
}

void insulators() {
    cout << '\n';
    vector<int> all;
    for (int i = 0; i < (int) g.size(); i++) {
        if (g[i].empty()) {
            all.push_back(i);
        }
    }
    cout << "-  " << (int) all.size() << "\t| ";
    for (int i = 0; i < (int)all.size(); i++) {
        cout << all[i] << " ";
    }
    cout << '\n';
}

void degree_max () {
    int max_degree = 0;
    for (int i = 0; i < (int) g.size(); i++) {
        max_degree = max(max_degree, (int) g[i].size());
    }
    cout << max_degree << '\n';
    for (int i = 0; i < (int) g.size(); i++) {
        if ((int)g[i].size() == max_degree) {
            cout << "-  " << i << "\t| ";
            for (auto j = g[i].begin(); j != g[i].end(); j++) {
                cout << *j << " ";
            }
            cout << '\n';
        }
    }
}
vector<bool> used;
vector<int> d;
vector<int> p;

int total;

void bfs (int s) {
    used[s] = true;
    queue<int> q;
    q.push(s);
    p[s] = -1;
    while (!q.empty()) {
        int v = q.front();
        q.pop();
        for (auto i = g[v].begin(); i != g[v].end(); i++) {
            int to = *i;
            if (!used[to]) {
                used[to] = true;
                total++;
                q.push(to);
                d[to] = d[v] + 1;
                p[to] = v;
            }
        }
    }
}



int diameter () {
    // Counting components
    int n = (int) g.size();
    used.assign(n, false);
    d.assign(n, 0);
    p.assign(n, 0);
    vector<pair<int, int>> components;
    for (int i = 0; i < n; ++i) {
        if (!used[i]) {
            total = 1;
            bfs(i);
            components.push_back({i, total});
        }
    }
    int start = 0, score = 0;
    for (int i = 0; i < (int) components.size(); i++) {
        if (components[i].second > score) {
            start = components[i].first;
            score = components[i].second;
        }
    }
    used.assign(n, false);
    d.assign(n, 0);
    p.assign(n, 0);
    vector<int> longest;
    bfs(start);
    for (int i = 0; i < n; i++) {
        if (used[i]) {
            longest.push_back(i);
        }
    }
    int h_max = 0;
    for (int i = 0; i < (int)longest.size(); i++) {
        total = 1;
        used.assign(n, false);
        d.assign(n, 0);
        p.assign(n, 0);
        bfs(longest[i]);
        for (int j = 0; j < n; j++) {
            h_max = max(h_max, d[j]);
        }
    }
    return h_max;
}

void path (int v, int u) {
    int n = (int) g.size();
    total = 1;
    used.assign(n, false);
    d.assign(n, 0);
    p.assign(n, 0);
    bfs(v);
    if (!used[u]) {
        cout << "No path: [" << v << " : " << u << "]" << '\n';
    }
    else {
        vector<int> path;
        for (int w = u; w != -1; w = p[w]) {
            path.push_back(w);
        }
        reverse(path.begin(), path.end());
        cout << "Path: [" << v << " : " << u << "]" << '\n';
        cout << "-  " << (int) path.size() - 1 << "\t| ";
        for (int i = 0; i < (int) path.size(); i++) {
            cout << path[i] << " ";
        }
        cout << '\n';
    }
}

void delete_ (int v) {
    vector<int> elem;
    for (auto i = g[v].begin(); i != g[v].end(); i++) {
        elem.push_back(*i);
    }
    g[v].clear();
    for (int i = 0; i < (int)elem.size(); i++) {
        g[elem[i]].erase(v);
        for (int j = 0; j < (int)elem.size(); j++) {
            if (g[elem[j]].find(elem[i]) == g[elem[j]].end() && elem[i] != elem[j]) {
                g[elem[j]].insert(elem[i]);
            }
        }
    }
}

void erase_() {
    /*
        Delete all vertexes % 17
        + [224, 932, 478, 459, 13, 26, 862]
    */
    for (int v = 0; v < 1000; v += 17) {
        delete_(v);
    }
    vector<int> need_erase = {224, 932, 478, 459, 13, 26, 862};
    for (int i = 0; i < (int) need_erase.size(); i++) {
        delete_(need_erase[i]);
    }
}

void print() {
    for (int i = 0; i < (int) g.size(); i++) {
        cout << i << " | ";
        for (auto j = g[i].begin(); j != g[i].end(); j++) {
            cout << *j << " ";
        }
        cout << '\n';
    }
}

int main() {
    setlocale(LC_ALL, "Russian");
    ifstream in("graphedges240.txt");
    in.is_open();
    int x, y;
    g.resize(1000);
    while (in >> x >> y) {
        g[x].insert(y);
        g[y].insert(x);
    }

    cout << "1. Edges in graph: " << edges() << '\n';

    cout << '\n' << "2. Insulators: ";
    insulators();

    cout << '\n' << "3. Max degree of vertex is: ";
    degree_max();

    cout << '\n' << "4. Diameter of the biggest component: " << diameter() << '\n';

    int A = 455, B = 521;
    cout << '\n' << "5. ";
    path(A, B);

    cout << '\n' << "6. ";
    int C = 311, D = 638;
    path(C, D);

    cout << '\n' << "7. ";
    int E = 406, F = 681;
    path(E, F);

    erase_();

    cout << '\n' << "8. Edges in graph(2): " << edges() << '\n';

    cout << '\n' << "9. Insulators(2): ";
    insulators();

    cout << '\n' << "10. Max degree of vertex is(2): ";
    degree_max();

    cout << '\n' << "11. Diameter of the biggest component(2): " << diameter() << '\n';

    cout << '\n' << "12. ";
    path(A, B);

    cout << '\n' << "13. ";
    path(C, D);

    cout << '\n' << "14. ";
    path(E, F);
}
