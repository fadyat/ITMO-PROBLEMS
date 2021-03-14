#include <iostream>
#include <vector>
#include <algorithm>
#include <cmath>

using namespace std;

class Point {
private:
    int x;
    int y;
public:
    explicit Point (int x_ = 0, int y_ = 0) : x(x_), y(y_) {}
    Point (const Point &other) {
        x = other.x;
        y = other.y;
    }
    Point& operator = (const Point &other) {
        if (this == &other) {
            return *this;
        }
        x = other.x;
        y = other.y;
        return *this;
    }
    void show () const {
        cout << x << " " << y << endl;
    }
    [[nodiscard]] int getX () const {
        return x;
    }
    [[nodiscard]] int getY () const {
        return y;
    }
};

class Line {
protected:
    vector<Point> points;
public:
    explicit Line (const vector<Point> &points_) : points(points_) {}
    Line () = default;
    Line (const Line &other) {
        this->points = other.points;
    }
    Line& operator = (const Line &other) {
        if (this == &other) {
            return *this;
        }
        this->points = other.points;
        return *this;
    }
    void show ()  {
        cout << "Have " << points.size() << " points: " << endl;
        for (Point &point : points) {
            point.show();
        }
        cout << '\n';
    }
};

class LockedLine : public Line {
private:
    double P = -1;
    double countP() {
        int n = points.size();
        double s = 0;
        for (int i = 0; i < n; i++) {
            int j = (i + 1) % n;
            double x1 = points[i].getX(), y1 = points[i].getY();
            double x2 = points[j].getX(), y2 = points[j].getY();
            s += sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
        return s;
    }
public:
    explicit LockedLine (vector<Point> &points_) : Line(points_), P(countP()) {}
    LockedLine () = default;
    LockedLine (const LockedLine &other) : Line(other) {
        this->P = other.P;
    }
    LockedLine& operator = (const LockedLine &other) {
        if (this == &other) {
            return *this;
        }
        Line::operator = (other);
        this->P = other.P;
        return *this;
    }
    [[nodiscard]] double itsP () const {
        return P;
    }
};

class Polygon : public LockedLine {
private:
    double S = -1;
    double countS () {
        double s = 0;
        int n = points.size();
        for (int i = 0; i < n; i++) {
            int j = (i + 1) % n;
            double x1 = points[i].getX(), y1 = points[i].getY();
            double x2 = points[j].getX(), y2 = points[j].getY();
            s += x1 * y2;
            s -= x2 * y1;
        }
        return abs(s / 2);
    }
    bool itsConvex () {
        int n = points.size();
        if (n < 3) {
            return false;
        }
        bool sign1, sign;
        for (int i = 0; i < n; i++) {
            int j = (n + i - 1) % n, k = (n + i + 1) % n;
            int x1 = points[j].getX(), y1 = points[j].getY();
            int x2 = points[i].getX(), y2 = points[i].getY();
            int x3 = points[k].getX(), y3 = points[k].getY();
            Point ab (x2 - x1, y2 - y1);
            Point bc (x3 - x2, y3 - y2);
            double cnt = ab.getX() * bc.getY() - ab.getY() * bc.getX();
            if (cnt == 0) {
                continue;
            }
            sign = (cnt > 0);
            if (i == 0) {
                sign1 = (cnt >= 0);
            }
            if (sign != sign1) {
                return false;
            }
        }
        return true;
    }
    bool itsOneLine () {
        int n = points.size();
        if (n < 3) {
            return true;
        }
        int x1 = points[0].getX(), y1 = points[0].getY();
        int x2 = points[1].getX(), y2 = points[1].getY();
        for (int i = 2; i < n; i++) {
            int x = points[i].getX(), y = points[i].getY();
            if ((y1 - y2) * x + (x2 - x1) * y + x1 * y2 - x2 * y1 != 0) {
                return false;
            }
        }
        return true;
    }
public:
    explicit Polygon (vector<Point> &points_) : LockedLine(points_) {
        if (!itsConvex()) {
            cout << "Incorrect data: polygon isn't convex" << endl;
        }
        else if (itsOneLine()) {
            cout << "Incorrect data: all points on the same line" << endl;
        }
        else {
            S = countS();
        }
    }
    Polygon () = default;
    Polygon (const Polygon &other) : LockedLine(other) {
        this->S = other.S;
    }
    Polygon& operator = (const Polygon &other) {
        if (this == &other) {
            return *this;
        }
        LockedLine::operator=(other);
        this->S = other.S;
        return *this;
    }
    [[nodiscard]] double itsS () const {
        return S;
    }
};

class Triangle : public Polygon {
private:
    bool itsTriangle() {
        if (points.size() != 3) {
            return false;
        }
        return true;
    }
public:
    explicit Triangle (vector<Point> &points_) : Polygon(points_) {
        if (!itsTriangle()) {
            cout << "Incorrect data: not 3 vertices" << endl;
        }
    }
    Triangle () = default;
};

int main() {
    /*
    vector<Point> all;
    all.emplace_back(0, 0);
    all.emplace_back(2, 0);
    all.emplace_back(2, 2);
    all.emplace_back(0, 2);
    LockedLine line2(all);
    cout << line2.itsP() << endl;
    line2.show(); */
    vector<Point> all1;
    all1.emplace_back(0, 0);
    all1.emplace_back(1, 1);
    all1.emplace_back(3, 3);
    all1.emplace_back(5, 3);
    /*
    LockedLine line1 (all1);
    cout << line1.itsP() << endl;
    line1.show();
    LockedLine line3 = line1;
    cout << line3.itsP() << endl;
    line3.show();
    line3 = line1;
    cout << line3.itsP() << endl;
    line3.show();
    line3 = line2;
    cout << line3.itsP() << endl;
    line3.show();*/
    Triangle x (all1);
//    cout << x.itsS() << endl;
//    cout << x.itsP() << endl;
//    x.show();
    Triangle tr = x;
//    cout << tr.itsS() << endl;
//    cout << tr.itsP() << endl;
    tr.show();
    reverse(all1.begin(), all1.end());
    Triangle x1 (all1);
//    cout << x1.itsS() << endl;
//    cout << x1.itsP() << endl;
    x.show();
    tr = x1;
//    cout << tr.itsS() << endl;
//    cout << tr.itsP() << endl;
    tr.show();
    /*    Polygon yy;
    cout << yy.itsS() << endl;
    cout << yy.itsP() << endl;
    yy.show();*/
}
