#include <iostream>
#include <vector>
#include <set>
#include <algorithm>
#include <cmath>
#include <iomanip>
#include <queue>

typedef long long ll;
typedef double ld;
typedef unsigned long long ull;
using namespace std;

void file() {
    freopen("queue.in", "r", stdin);
    freopen("queue.out", "w", stdout);
}

template <typename T>
struct new_knot {
    T x;
    new_knot *next;
};

template <typename T>
class _queue{
private:
    new_knot<T> *head, *tail;
public:
    _queue():head(nullptr), tail(nullptr){};

    void add(int x) {
        new_knot<T> *tmp;
        tmp = new new_knot<T>;
        tmp -> x = x;
        tmp -> next = nullptr;          // [] -> [] -> [tmp] -> null
        if(head != nullptr) {
            tail -> next = tmp;         // tail have address o tmp
            tail = tmp;                 // move tail
        }
        else {
            head = tail = tmp;          // queue.empty()
        }
    }

    void pop_forward() {
        if(head != nullptr) {
            new_knot<T> *out = head;    // get head
            cout << out -> x << endl;   // print head element
            head = head -> next;        // move head
            delete out;
        }
    }
    T top() {
        if(head != nullptr)
            return head -> x;
    }
};

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    file();
    _queue<int> my_queue;
    int n; cin >> n;
    while(n--) {
        char xx; cin >> xx;
        if(xx == '+') {
            int x; cin >> x;
            my_queue.add(x);
        }
        else {
            my_queue.pop_forward();
        }
    }
}