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
    freopen("brackets.in","r", stdin);
    freopen("brackets.out","w", stdout);
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
    void pop_back() {
        if (head != nullptr) {
            new_knot<T> *tmp = head;
            head = head -> next;
            delete tmp;
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
    _stack<char> my_stack;
    string s;
    while(cin >> s) {
        int size = 0;
        for(int i = 0; i < s.size(); i++) {
            if(size){
                if((my_stack.top() == '(' && s[i] == ')') || (my_stack.top() == '[' && s[i] == ']')) {
                    my_stack.pop_back();
                    size--;
                }
                else {
                    my_stack.add(s[i]);
                    size++;
                }
            }
            else {
                my_stack.add(s[i]);
                size++;
            }
        }
        cout << ((!size) ? ("YES") : ("NO")) << endl;
    }
}