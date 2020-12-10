#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

struct node {
    int value;
    int left;
    int right;
};

int n;
vector<node> tree;
vector<int> balance;
vector<int> height;
int top = 0;
int position = 0;

void dfs_balance(int root);

void count_balance() {
    height.assign(n, 1);
    balance.assign(n, 0);
    dfs_balance(0);
}

void dfs_balance (int root) {
    int left_height = 0, right_height = 0;
    if (tree[root].left != -1) {
        dfs_balance(tree[root].left);
        height[root] = max(height[root], height[tree[root].left] + 1);
        left_height = height[tree[root].left];
    }
    if (tree[root].right != -1) {
        dfs_balance(tree[root].right);
        height[root] = max(height[root], height[tree[root].right] + 1);
        right_height = height[tree[root].right];
    }
    balance[root] = right_height - left_height;
}


void LL (int knot) {
    int tmp = tree[tree[knot].right].left;
    top =  tree[knot].right;
    tree[tree[knot].right].left = knot;
    tree[knot].right = tmp;
}

void RR (int knot) {
    int tmp = tree[tree[knot].left].right;
    top =  tree[knot].left;
    tree[tree[knot].left].right = knot;
    tree[knot].left = tmp;
}

void LR (int knot) {
    int tmp = tree[tree[knot].left].right;
    LL(tree[knot].left);
    tree[knot].left = tmp;
    if (knot != 0) {
        tmp = tree[tree[knot].right].left;
    }
    RR(knot);
    if (knot != 0) {
        tree[knot].right = tmp;
    }
}

void RL (int knot) {
    int tmp = tree[tree[knot].right].left;
    RR(tree[knot].right);
    tree[knot].right = tmp;
    if (knot != 0) {
        tmp = tree[tree[knot].left].right;
    }
    LL(knot);
    if (knot != 0) {
        tree[knot].left = tmp;
    }
}

void rotate (int knot) {
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

vector<node> new_tree;
void dfs (int t) {
    int v = position;
    new_tree[v].value = tree[t].value;
    if (tree[t].left != -1) {
        ++position;
        new_tree[v].left = position;
        dfs(tree[t].left);
    }
    else {
        new_tree[v].left = -1;
    }

    if (tree[t].right != -1) {
        ++position;
        new_tree[v].right = position;
        dfs(tree[t].right);
    }
    else {
        new_tree[v].right = -1;
    }
}

void dfs_on_tree () {
    new_tree.resize(n);
    position = 0;
    dfs(top);
    tree = new_tree;
    top = 0;
}

int main() {
    cin.tie(nullptr);
    cout.tie(nullptr);
    freopen("rotation.in", "r", stdin);
    freopen("rotation.out", "w", stdout);
    cin >> n;
    tree.resize(n);
    for (int i = 0; i < n; i++) {
        cin >> tree[i].value >> tree[i].left >> tree[i].right;
        --tree[i].left;
        --tree[i].right;
    }
    count_balance();
    rotate(0);
    dfs_on_tree();
    cout << n << endl;
    for (int i = 0; i < n; ++i) {
        cout << tree[i].value << " " << tree[i].left + 1<< " " << tree[i].right + 1 << endl;
    }
}