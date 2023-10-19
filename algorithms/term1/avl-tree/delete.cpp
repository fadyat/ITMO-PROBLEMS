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
int position = 0;

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

void count_balance() {
    if (tree.empty()) {
        return;
    }
    height.resize((int) tree.size(), 1);
    balance.resize((int) tree.size(), 0);
    dfs_balance(top);
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

// rebuild
vector<node> new_tree;

void dfs(int t) {
    int v = position;
    new_tree[v].value = tree[t].value;
    if (tree[t].left != -1) {
        ++position;
        new_tree[v].left = position;
        new_tree[position].parent = v;
        dfs(tree[t].left);
    }
    else {
        new_tree[v].left = -1;
    }

    if (tree[t].right != -1) {
        ++position;
        new_tree[v].right = position;
        new_tree[position].parent = v;
        dfs(tree[t].right);
    }
    else {
        new_tree[v].right = -1;
    }
}

void clear () {
    vector<node> tmp;
    for (int i = 0; i < (int) tree.size(); i++) {
        if (!(tree[i].parent == -1 && i != top)) {
            tmp.push_back(tree[i]);
        }
    }
    tree = tmp;
}

void rebuild() {
    if(!tree.empty()) {
        new_tree.resize((int) tree.size());
        for (auto & i : new_tree) {
            i.parent = -1;
        }
    }
    else {
        return;
    }
    position = 0;
    dfs(top);
    top = 0;
    tree = new_tree;
    clear();
    new_tree.clear();
    top = 0;
    count_balance();
}

// push and erase_
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

void erase_ (int xx) {
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
    freopen("deletion.in", "r", stdin);
    freopen("deletion.out", "w", stdout);
    cin >> n;
    if (n) {
        tree.resize(n);
        tree[0].parent = -1;
        top = 0;
        for (int i = 0; i < (int) tree.size(); i++) {
            cin >> tree[i].value >> tree[i].left >> tree[i].right;
            --tree[i].left;
            --tree[i].right;
            if (tree[i].left != -1) {
                tree[tree[i].left].parent = i;
            }
            if (tree[i].right != -1) {
                tree[tree[i].right].parent = i;
            }
        }
        count_balance();
    }
    int del = 1;
    for (int i = 0; i < del; i++) {
        int xx; cin >> xx;
        erase_(xx);
    }
    rebuild();
    cout << (int) tree.size() << endl;
    for (auto &i : tree) {
        cout << i.value << " " << i.left + 1 << " " << i.right + 1 << " " << endl;
    }
}
