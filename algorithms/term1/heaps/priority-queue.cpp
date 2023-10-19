#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

vector<pair<int, int> > a;
vector<int> pos;

void siftUp(int v){
    while(v) {
        if(a[v].first < a[(v - 1) / 2].first) {
            swap(pos[   a[v].second    ], pos[    a[(v - 1) / 2].second   ]);
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
        if(r < n && a[r].first < a[l].first) {
            j = r;
        }
        if(a[v].first < a[j].first) {
            break;
        }
        swap(pos[   a[v].second   ], pos[   a[j].second   ]);
        swap(a[v], a[j]);
        v = j;
    }
}

void push(int x, int operation){
    a.push_back(make_pair(x, operation));
    pos[operation] = (int)a.size() - 1;
    siftUp((int)a.size() - 1);
}

void extract() {
    if(a.empty())
        cout << "*" << endl;
    else{
        cout << a[0].first << endl;
        swap(a[0], a[(int)a.size() - 1]);
        pos[a[0].second] = 0;
        a.pop_back();
        siftDown(0, (int)a.size());
    }
}

void key(int x, int y){
    int i = pos[x];
    a[i].first = y;
    siftUp(i);
}

int main() {
    freopen("priorityqueue.in", "r", stdin);
    freopen("priorityqueue.out", "w", stdout);
    string s;
    int operation = 0;
    pos.resize(1e6 + 100);
    while (cin >> s) {
        if (s == "push") {
            int x;
            cin >> x;
            push(x, operation);
        } else if (s == "extract-min") {
            extract();
        } else {
            int x, y;
            cin >> x >> y;
            x--;
            key(x, y);
        }
        operation++;
    }
}