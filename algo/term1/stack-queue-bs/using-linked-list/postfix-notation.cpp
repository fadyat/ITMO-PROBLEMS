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
    freopen("postfix.in", "r", stdin);
    freopen("postfix.out", "w", stdout);
}

template <typename T>
struct new_knot {
    T x;
    new_knot *next;
};

template <typename T>
class _stack {
private:
    new_knot<T> *head;
public:
    _stack():head(nullptr){};
    void add(T x) {
        new_knot<T> *tmp;
        tmp = new new_knot<T>;
        tmp -> x = x;
        tmp -> next = head;
        head = tmp;
    }
    T pop_back() {
        if (head != nullptr) {
            new_knot<T> *tmp = head;
            T ans =  head -> x;
            head = head -> next;
            delete tmp;
            return ans;
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
    _stack<int> my_stack;
    char x;
    while(cin >> x) {
        if(x >= '0' && x <= '9') {
            my_stack.add(x - '0');
        }
        else {
            int y1 = my_stack.pop_back();
            int y2 = my_stack.pop_back();
            if(x == '+') {
                my_stack.add(y1 + y2);
            }
            else if(x == '-') {
                my_stack.add(y2 - y1);
            }
            else {
                my_stack.add(y1 * y2);
            }
        }
    }
    cout << my_stack.top() << endl;
}