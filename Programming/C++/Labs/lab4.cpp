#include <iostream>
#include <algorithm>
#include <vector>

using namespace std;

template<typename T>
class Rule {
private:
    T value;
public:
    explicit Rule(T value_) : value(value_) {}

    bool operator()(T number) {
        return number > value;
    }
};

template<typename T, class F>
bool all_of_(const T &begin, const T &end, F func) {
    for (T it = begin; it != end; it++) {
        if (!func(*it)) {
            return false;
        }
    }
    return true;
}

template<typename T, class F>
bool any_of_(const T &begin, const T &end, F func) {
    for (T it = begin; it != end; it++) {
        if (func(*it)) {
            return true;
        }
    }
    return false;
}

template<typename T, class F>
bool none_of_(const T &begin, const T &end, F func) {
    for (T it = begin; it != end; it++) {
        if (func(*it)) {
            return false;
        }
    }
    return true;
}

template<typename T, class F>
bool one_of_(const T &begin, const T &end, F func) {
    int cnt = 0;
    for (T it = begin; it != end; it++) {
        cnt += func(*it);
    }
    return (cnt == 1);
}

/* is_sorted works with increasing sequence */
template<typename T, class F>
bool is_sorted_(const T &begin, const T &end, F func) {
    if (begin == end) {
        return true;
    }
    for (T it = begin + 1; it != end; it++) {
        if (func(*(it - 1)) > func(*it)) {
            return false;
        }
    }
    return true;
}

template<typename T, class F>
bool is_partitioned_(const T &begin, const T &end, F func) {
    T it = begin;
    while (it != end && func(*it)) {
        it++;
    }
    while (it != end) {
        if (func(*it)) {
            return false;
        }
        it++;
    }
    return true;
}

template<typename T, typename N>
T find_not_(const T &begin, const T &end, const N &value) {
    for (T it = begin; it != end; it++) {
        if (*it != value) {
            return it;
        }
    }
    return end;
}

template<typename T, typename N>
T find_backward_(const T &begin, const T &end, const N &value) {
    for (T it = end - 1; it != begin - 1; it--) {
        if (*it == value) {
            return it;
        }
    }
    return end;
}

template<typename T, class F>
bool is_palindrome_(const T &begin, const T &end, F func) {
    T it1 = begin, it2 = end - 1;
    while (it1 < it2) {
        if (func(*it1) != func(*it2)) {
            return false;
        }
        it1++, it2--;
    }
    return true;
}

int main() {
    vector<int> a = {4, 6, -1, 1, 3, 4};
//    cout << all_of_(a.begin(), a.end(), Rule(3)) << endl;
//    cout << any_of_(a.begin(), a.end(), Rule(10)) << endl;
//    cout << none_of_(a.begin(), a.end(), Rule(12)) << endl;
//    cout << one_of_(a.begin(), a.end(), Rule(6)) << endl;
//    cout << is_sorted_(a.begin(), a.end(), Rule(4)) << endl;
//    cout << is_partitioned_(a.begin(), a.end(), Rule(6)) << endl;
//    cout << *find_not_(a.begin(), a.end(), 5) << endl;
//    cout << *find_backward_(a.begin(), a.end(), 11) << endl;
//    cout << is_palindrome_(a.begin(), a.end(), Rule(2)) << endl;
}
