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
        for (int i = (int) s.size() - 1; i >= 1; i--) {
            ans += (s[i] - '0') * p;
            p *= 10;
        }
        if (s[0] == '-') {
            ans *= -1;
        }
        else {
            ans += (s[0] - '0') * p;
        }
        return ans;
    }
public:
    Fraction() = default;
    explicit Fraction (int n, int d) : numerator(n), denominator(d) {
        update();
    }
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
        if (this->numerator * other.denominator + this->denominator * other.numerator == 0) {
            return Fraction(0, 1);
        }
        return Fraction(this->numerator * other.denominator + this->denominator * other.numerator, this->denominator * other.denominator);
    }
    Fraction  operator -  (const Fraction& other) const {
        if (this->numerator * other.denominator - this->denominator * other.numerator == 0) {
            return Fraction(0, 1);
        }
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
        if (other.denominator == -1) {
            out << "nope";
            return out;
        }
        out << other.numerator << '/' << other.denominator;
        return out;
    }
    friend istream& operator >> (istream& in, Fraction& other) {
        string s;
        in >> s;
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
        other.numerator = to_int(num);
        other.denominator = to_int(den);
        other.update();
        return in;
    }
    Fraction operator + () const {
        return *this;
    }
    Fraction operator - () const {
        return Fraction(-numerator, denominator);
    }
    bool operator == (const Fraction& other) const {
        return ((this->numerator == other.numerator) && (this->denominator == other.denominator));
    }
    bool operator != (const Fraction& other) const {
        return !(*this == other);
    }
};

class Polynom {
protected:
    int n = 0;
    map<int, Fraction> total;
public:
    explicit Polynom (map<int, Fraction> &total_) : total(total_), n(total_.size()) {}
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
    friend istream& operator >> (istream& in, Polynom& other) {
        in >> other.n;
        for (int i = 0; i < other.n; i++) {
            int power;
            Fraction xx;
            cin >> power >> xx;
            other.total[power] += xx;
        }
        return in;
    }
    Polynom operator - () {
        map<int, Fraction> tmp = this->total;
        for (const auto & i : tmp) {
            tmp[i.first] = -i.second;
        }
        return Polynom (tmp);
    }
    Polynom operator + () {
        return *this;
    }
    Fraction operator[] (const int &i) {
        if (!this->total.empty() && (this->total.find(i) != this->total.end())) {
            return this->total[i];
        }
        return Fraction(1, -1);
    }
};

int main() {
    Polynom ogo;
    cin >> ogo;
    cout << ogo;
}
