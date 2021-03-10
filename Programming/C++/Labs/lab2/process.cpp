#include <iostream>
#include <vector>
#include <numeric>

using namespace std;

class Fraction {
private:
    int numerator;
    int denominator;
    void update () {
        int GCD = gcd(numerator, denominator);
        numerator /= GCD;
        denominator /= GCD;
    }
    static int to_int (string &s) {
        int ans = 0, p = 1;
        for (int i = (int) s.size() - 1; i >= 0; i--) {
            ans += (s[i] - '0') * p;
            p *= 10;
        }
        return ans;
    }
public:
    explicit Fraction (string &s) {
        string num, den;
        int u = 0;
        while (u < s.size() && s[u] != '/') {
            num.push_back(s[u]);
            u++;
        }
        ++u;
        while (u < s.size()) {
            den.push_back(s[u]);
            u++;
        }
        if (den.empty()) {
            den = "1";
        }
        numerator = to_int(num);
        denominator = to_int(den);
        update();
    }
    void look () const {
        cout << numerator << "/" << denominator << endl;
    }
};

/*
class Polynom {
private:
    vector<int> deg;
public:
    explicit Polynom (vector<int> &deg_) {
        deg = deg_;
    }
    void look () {
        for (int &i : deg) {
            cout << i << " ";
        }
        cout << endl;
    }
};
*/
int main() {/*
    vector<int> vec = {1, 2, 3};
    Polynom kek (vec);
    kek.look();
    vector<int> aa = {1, 1, 1};
    Polynom lol (aa);
    lol.look();*/
    vector<Fraction> all;
    for (int i = 0; i < 3; i++) {
        string s;
        cin >> s;
        all.emplace_back(Fraction(s));
    }
    for (int i = 0; i < (int) all.size(); i++) {
        all[i].look();
    }
}