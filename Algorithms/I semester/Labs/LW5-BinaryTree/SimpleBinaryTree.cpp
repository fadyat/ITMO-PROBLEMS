#include <iostream>

using namespace std;

void file() {
    freopen("bstsimple.in", "r", stdin);
    freopen("bstsimple.out", "w", stdout);
}

template<typename T>
struct knot {
    T value;
    knot *left;
    knot *right;
};

template<typename T>
void print(knot<T> *&tree) {                   //                1
    if (tree != nullptr) {                     //       -5               7
        cout << tree->value << " ";            //   -7      -1        5     9
        print(tree->left);                     // -9 -6   -3  0     4  6  8  10
        print(tree->right);
    }
}

template<typename T>
knot<T> *add(knot<T> *&tree, T x) {
    if (tree == nullptr) {                     // empty tree or no vertex
        tree = new knot<T>;
        tree->left = nullptr;
        tree->right = nullptr;
        tree->value = x;
    }
    else {
        if (x > tree->value) {
            return add(tree->right, x);        // go right
        }
        else {
            return add(tree->left, x);         // go left
        }
    }
    return tree;
}

template<typename T>
knot<T> *del(knot<T> *&tree, T x) {
    if (tree == nullptr) {
        return tree;
    }
    if (x < tree->value) {
        tree->left = del(tree->left, x);
    }
    else if (x > tree->value) {
        tree->right = del(tree->right, x);
    }
    else if (tree->left != nullptr && tree->right != nullptr) {
        tree->value = minimum(tree->right)->value;
        tree->right = del(tree->right, tree->value);
    }
    else {
        if (tree->left != nullptr) {
            tree = tree->left;
        }
        else if (tree->right != nullptr) {
            tree = tree->right;
        }
        else {
            tree = nullptr;
        }
    }
    return tree;
}

template<typename T>
knot<T> *exist(knot<T> *&tree, T x) {
    if (tree == nullptr || tree->value == x) {
        return tree;
    }
    else {
        if (x > tree->value) {
            return exist(tree->right, x); // go right
        }
        else {
            return exist(tree->left, x); // go left
        }
    }
}

template<typename T>
string existP(knot<T> *&tree, T x) {
    knot<int> *tmp = exist(tree, x);
    if (tmp != nullptr) {
        return "true";
    }
    else {
        return "false";
    }
}

template<typename T>
knot<T> *next(knot<T> *&tree, T x) {
    knot<T> *good = nullptr; // last knot that have value more than x | min(a[i]) > x
    knot<T> *tree1 = tree;
    while (tree1 != nullptr) {
        if (tree1->value > x) {
            good = tree1;
            tree1 = tree1->left;
        }
        else {
            tree1 = tree1->right;
        }
    }
    return good;
}

template<typename T>
void nextP(knot<T> *&tree, T x) {
    knot<T> *tmp = next(tree, x);
    if (tmp != nullptr) {
        cout << tmp->value << '\n';
    }
    else {
        cout << "none\n";
    }
}

template<typename T>
knot<T> *prev(knot<T> *&tree, T x) {
    knot<T> *good = nullptr;    // last knot that have value lower than x | max(a[i]) < x
    knot<T> *tree1 = tree;
    while (tree1 != nullptr) {
        if (tree1->value < x) {
            good = tree1;
            tree1 = tree1->right;
        }
        else {
            tree1 = tree1->left;
        }
    }
    return good;
}

template<typename T>
void prevP(knot<T> *&tree, T x) {
    knot<T> *tmp = prev(tree, x);
    if (tmp != nullptr) {
        cout << tmp->value << '\n';
    }
    else {
        cout << "none\n";
    }
}

template<typename T>
knot<T> *minimum(knot<T> *&tree) {
    if (tree->left == nullptr) {
        return tree;
    }
    return minimum(tree->left);
}

template<typename T>
knot<T> *maximum(knot<T> *&tree) {
    if (tree->right == nullptr) {
        return tree;
    }
    return maximum(tree->right);
}

int main() {
    ios_base::sync_with_stdio(false);
    cin.tie(nullptr);
    cout.tie(nullptr);
    file();
    knot<int> *my_tree;
    my_tree = nullptr;
    string s;
    while (cin >> s) {
        int x;
        cin >> x;
        if (s[0] == 'i') { // insert
            if (existP(my_tree, x) == "false") {
                add(my_tree, x);
            }
        }
        else if (s[0] == 'd') { // delete
            my_tree = del(my_tree, x);
        }
        else if (s[0] == 'e') { // exist
            cout << existP(my_tree, x) << '\n';
        }
        else if (s[0] == 'n') { // min(x[i]) > x
            nextP(my_tree, x);
        }
        else if (s[0] == 'p') { // max(x[i]) < x
            prevP(my_tree, x);
        }
//        print(my_tree);
//        cout << '\n';
    }
}
