#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

struct node {
    int value;
    int left;
    int right;
    int parent;
};

int n;
int top = -1;

vector<node> tree;
vector<int> balance;
vector<int> height;

// balance
void dfs_balance(int root) {
    int hl = 0, hr = 0;
    if (tree[root].left != -1) {
        dfs_balance(tree[root].left);
        height[root] = max(height[root], height[tree[root].left] + 1);
        hl = height[tree[root].left];
    }
    if (tree[root].right != -1) {
        dfs_balance(tree[root].right);
        height[root] = max(height[root], height[tree[root].right] + 1);
        hr = height[tree[root].right];
    }
    balance[root] = hr - hl;
}

// rotations
void LL(int knot) {
    int par = tree[knot].parent;
    int hl, hrl;
    hl = ((tree[knot].left != -1) ? (height[tree[knot].left]) : (0));
    hrl = ((tree[tree[knot].right].left != -1) ? (height[tree[tree[knot].right].left]) : (0));
    height[knot] = max(hl, hrl) + 1;
    balance[knot] = hrl - hl;

    int hrr, my_h;
    hrr = ((tree[tree[knot].right].right != -1) ? (height[tree[tree[knot].right].right]) : (0));
    my_h = height[knot];
    height[tree[knot].right] = max(hrr, my_h) + 1;
    balance[tree[knot].right] = hrr - my_h;

    char check = 'n';
    if (par != -1) {
        if (tree[tree[knot].parent].right == knot) {
            check = 'r';
        }
        else if (tree[tree[knot].parent].left == knot) {
            check = 'l';
        }
    }

    int tmp = tree[knot].right;
    if (tree[tree[knot].right].left != -1) {
        tree[tree[tree[knot].right].left].parent = knot;
    }
    tree[knot].right = tree[tree[knot].right].left;
    tree[tmp].left = knot;

    tree[knot].parent = tmp;
    tree[tmp].parent = par;

    if (check == 'l') {
        tree[par].left = tmp;
    }
    else if (check == 'r') {
        tree[par].right = tmp;
    }
    top = ((top == knot) ? (tmp) : (top));
}
void RR(int knot) {
    int par = tree[knot].parent;
    int hr, hlr;
    hr = ((tree[knot].right != -1) ? (height[tree[knot].right]) : (0));
    hlr = ((tree[tree[knot].left].right != -1) ? (height[tree[tree[knot].left].right]) : (0));
    height[knot] = max(hr, hlr) + 1;
    balance[knot] = hr - hlr;

    int hll, my_h;
    hll = ((tree[tree[knot].left].left != -1) ? (height[tree[tree[knot].left].left]) : (0));
    my_h = height[knot];
    height[tree[knot].left] = max(hll, my_h) + 1;
    balance[tree[knot].left] = my_h - hll;

    char check = 'n';
    if (par != -1) {
        if (tree[tree[knot].parent].right == knot) {
            check = 'r';
        }
        else if (tree[tree[knot].parent].left == knot) {
            check = 'l';
        }
    }

    int tmp = tree[knot].left;
    if (tree[tree[knot].left].right != -1) {
        tree[tree[tree[knot].left].right].parent = knot;
    }
    tree[knot].left = tree[tree[knot].left].right;
    tree[tmp].right = knot;

    tree[knot].parent = tmp;
    tree[tmp].parent = par;

    if (check == 'l') {
        tree[par].left = tmp;
    }
    else if (check == 'r') {
        tree[par].right = tmp;
    }
    top = ((top == knot) ? (tmp) : (top));
}
void LR(int knot) {
    LL(tree[knot].left);
    RR(knot);
}
void RL(int knot) {
    RR(tree[knot].right);
    LL(knot);
}
void rotate(int knot) {
    if (balance[knot] == 2 && balance[tree[knot].right] == -1) {
        RL(knot);
    }
    else if (balance[knot] == 2 && balance[tree[knot].right] != -1) {
        LL(knot);
    }

    else if (balance[knot] == -2 && balance[tree[knot].left] == 1) {
        LR(knot);
    }
    else if (balance[knot] == -2 && balance[tree[knot].left] != 1) {
        RR(knot);
    }
}

// push and erase
int new_place (int xx) {
    int tmp = top;
    while (true) {
        if (tree[tmp].left != -1 && xx < tree[tmp].value) {
            tmp = tree[tmp].left;
        }
        else if (tree[tmp].right != -1 && xx > tree[tmp].value) {
            tmp = tree[tmp].right;
        }
        else {
            break;
        }
    }
    return tmp;
}
void push (int xx) {
    int prev = -1;
    node new_knot{xx, -1, -1, prev};
    height.push_back(1);
    balance.push_back(0);
    if (tree.empty()) {
        tree.push_back(new_knot);
        top = 0;
        return;
    }
    prev = new_place (xx);
    new_knot.parent = prev;
    tree.push_back(new_knot);
    if (tree[prev].value > xx) {
        tree[prev].left = (int) tree.size() - 1;
    }
    else if (tree[prev].value < xx) {
        tree[prev].right = (int) tree.size() - 1;
    }
    else if (tree[prev].value == xx) {
        return;
    }

    while (prev != -1) {
        int hl = 0, hr = 0;
        if (tree[prev].left != -1) {
            hl = height[tree[prev].left];
        }
        if (tree[prev].right != -1) {
            hr = height[tree[prev].right];
        }
        height[prev] = max(hr, hl) + 1;
        balance[prev] = hr - hl;
        rotate(prev);
        prev = tree[prev].parent;
    }
}
void erase (int xx) {
    if (top == -1) {
        tree.clear();
        return;
    }
    else if (tree[top].left == -1 && tree[top].right == -1 && tree[top].value == xx) {
        tree.clear();
        top = -1;
        return;
    }
    int prev = new_place(xx);
    if (tree[prev].value != xx) {
        return;
    }
    int next;
    if (tree[prev].left == -1) {
        if (tree[prev].right == -1) {
            next = tree[prev].parent;
        }
        else {
            next = prev;
            prev = tree[prev].right;
            swap(tree[prev].value, tree[next].value);
        }
        tree[prev].parent = -1;
        if (tree[next].left == prev) {
            tree[next].left = -1;
        }
        else if (tree[next].right == prev) {
            tree[next].right = -1;
        }
    }
    else {
        int tmp = tree[prev].left;
        while (tree[tmp].right != -1) {
            tmp = tree[tmp].right;
        }
        swap(tree[prev].value, tree[tmp].value);
        if (tree[tmp].left != -1) {
            swap(tree[tree[tmp].left].value, tree[tmp].value);
            tmp = tree[tmp].left;
        }
        next = tree[tmp].parent;
        tree[tmp].parent = -1;
        if (tree[next].right == tmp) {
            tree[next].right = -1;
        }
        else if (tree[next].left == tmp) {
            tree[next].left = -1;
        }
    }
    while (next != -1) {
        int hl = 0, hr = 0;
        if (tree[next].left != -1) {
            hl = height[tree[next].left];
        }
        if (tree[next].right != -1) {
            hr = height[tree[next].right];
        }
        height[next] = max(hr, hl) + 1;
        balance[next] = hr - hl;
        rotate(next);
        next = tree[next].parent;
    }
}

int main() {
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("avlset.in", "r", stdin);
    freopen("avlset.out", "w", stdout);
    cin >> n;
    while (n--) {
        char o;
        int xx;
        cin >> o >> xx;
        if (o == 'A') {
            push(xx);
            if (tree.empty()) {
                cout << 0 << endl;
            }
            else {
                cout << balance[top] << endl;
            }
        }
        else if (o == 'D') {
            erase(xx);
            if (tree.empty()) {
                cout << 0 << endl;
            }
            else {
                cout << balance[top] << endl;
            }
        }
        else if (o == 'C') {
            if (tree.empty()) {
                cout << 'N' << endl;
            }
            else {
                if (tree[new_place(xx)].value == xx) {
                    cout << 'Y' << endl;
                }
                else {
                    cout << 'N' << endl;
                }
            }
        }
    }
}
