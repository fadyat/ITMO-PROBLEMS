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
    Fraction (const Fraction &other) = default;
    Fraction& operator =  (const Fraction& other) {
        if (this == &other) {
            return *this;
        }
        this->numerator = other.numerator;
        this->denominator = other.denominator;
        return *this;
    }
    Fraction  operator +  (const Fraction& other) const {
        return Fraction(this->numerator * other.denominator + this->denominator * other.numerator, this->denominator * other.denominator);
    }
    Fraction  operator -  (const Fraction& other) const {
        return Fraction(this->numerator * other.denominator - this->denominator * other.numerator, this->denominator * other.denominator);
    }
    Fraction  operator *  (const Fraction& other) const {
        if (this->numerator * other.numerator == 0) {
            return Fraction(0, 1);
        }
        return Fraction(this->numerator * other.numerator, this->denominator * other.denominator);
    }
    Fraction  operator /  (const Fraction& other) const {
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
};

class Polynom {
protected:
    map<int, Fraction> total;
public:
    explicit Polynom (map<int, Fraction> &total_) : total(total_) {}
    Polynom() = default;
    Polynom (const Polynom &other) = default;
    Polynom& operator =  (const Polynom &other) {
        if (this == &other) {
            return *this;
        }
        this->total = other.total;
        return *this;
    }
    Polynom  operator +  (const Polynom &other) {
        map<int, Fraction> tmp;
        for (const auto &i : this->total) {
            tmp[i.first] += i.second;
        }
        for (const auto &i : other.total) {
            tmp[i.first] += i.second;
        }
        return Polynom(tmp);
    }
    Polynom  operator -  (const Polynom &other) {
        map<int, Fraction> tmp;
        for (const auto &i : this->total) {
            tmp[i.first] += i.second;
        }
        for (const auto &i : other.total) {
            tmp[i.first] -= i.second;
        }
        return Polynom(tmp);
    }
    Polynom  operator *  (const Polynom &other) {
        map<int, Fraction> tmp;
        for (const auto &i : this->total) {
            for (const auto &j : other.total) {
                tmp[i.first + j.first] += i.second * j.second;
            }
        }
        return Polynom(tmp);
    }
    Polynom  operator /  (const Fraction& other) {
        map<int, Fraction> tmp;
        for (const auto &i : this->total) {
            tmp[i.first] = i.second / other;
        }
        return Polynom(tmp);
    }
    Polynom& operator += (const Polynom& other) {
        *this = *this + other;
        return *this;
    }
    Polynom& operator -= (const Polynom& other) {
        *this = *this - other;
        return *this;
    }
    Polynom& operator *= (const Polynom& other) {
        *this = *this * other;
        return *this;
    }
    Polynom& operator /= (const Fraction& other) {
        *this = *this / other;
        return *this;
    }
    friend ostream& operator << (ostream& out, const Polynom& other) {
        for (const auto & i : other.total) {
            out << i.first << " " << i.second << endl;
        }
        return out;
    }
};

int main() {
    /*
    Fraction x (3, 4);
    Fraction y (1, 2);
    cout << x + y << endl;
    cout << x << endl;
    */
    /*
    map<int, Fraction> total;
    map<int, Fraction> total1;
    for (int i = 0; i < 2; ++i) {
        int n;
        string s;
        cin >> n >> s;
        Fraction tmp (s);
        total[n] += tmp;
    }
    for (int i = 0; i < 1; ++i) {
        int n;
        string s;
        cin >> n >> s;
        Fraction tmp (s);
        total1[n] += tmp;
    }

    Polynom pol1(total);
    Polynom pol2 (total1);

    cout << pol1 << endl;
    cout << pol2 << endl;
    cout << pol1 / Fraction(1, 3) << endl;
    string s = "2";
    cout << pol1 / Fraction(s) << endl;
    cout << pol1 << endl; */
}
