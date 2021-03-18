#include <iostream>
#include <numeric>
#include <map>

using namespace std;

class Fraction {
private:
    int numerator = 0;
    int denominator = 1;
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
    explicit Fraction (int n, int d) : numerator(n), denominator(d) {
        update();
    }
    explicit Fraction() = default;
    Fraction& operator = (const Fraction& other) {
        if (this == &other) {
            return *this;
        }
        this->numerator = other.numerator;
        this->denominator = other.denominator;
        return *this;
    }
    Fraction operator + (const Fraction& other) const {
        return Fraction(this->numerator * other.denominator + this->denominator * other.numerator, this->denominator * other.denominator);
    }
    Fraction operator - (const Fraction& other) const {
        return Fraction(this->numerator * other.denominator - this->denominator * other.numerator, this->denominator * other.denominator);
    }
    Fraction operator * (const Fraction& other) const {
        if (this->numerator * other.numerator == 0) {
            return Fraction(0, 1);
        }
        return Fraction(this->numerator * other.numerator, this->denominator * other.denominator);
    }
    Fraction operator / (const Fraction& other) const {
        if (this->numerator * other.denominator == 0) {
            return Fraction(0, 1);
        }
        return Fraction(this->numerator * other.denominator, this->denominator * other.numerator);
    }
    Fraction& operator += (const Fraction& other) {
        *this = *this + other;
        return *this;
    }
    Fraction& operator -= (const Fraction& other) {
        *this = *this - other;
        return *this;
    }
    Fraction& operator *= (const Fraction& other) {
        *this = *this * other;
        return *this;
    }
    Fraction& operator /= (const Fraction& other) {
        *this = *this / other;
        return *this;
    }
    friend ostream& operator << (ostream& out, const Fraction& other) {
        out << other.numerator << '/' << other.denominator;
        return out;
    }
    Fraction (const Fraction &other) {
        this->numerator = other.numerator;
        this->denominator = other.denominator;
    }
};

class Polynom {
private:
    map<int, Fraction> total;
public:
    explicit Polynom (map<int, Fraction> &total_) : total(total_) {}
    Polynom() = default;
};

int main() {/*
    Fraction x (3, 3);
    Fraction y = x;
    y += Fraction(1, 5);
    Fraction z;
    z = y;
    z -= Fraction(5, 13);
    cout << x << " " << y << " " << z << endl;*/
    /* map<int, Fraction> total;
    for (int i = 0; i < 2; ++i) {
        int n;
        string s;
        cin >> n >> s;
        Fraction tmp (s);
        total[n] += tmp;
    }
    Polynom pol1(total);*/ /*
    for (auto i = total.begin(); i != total.end(); i++) {
        cout << i->first << " " << i->second << endl;
    } */
}
