#include <iostream>
#include <vector>
#include <algorithm>
#include <cmath>

using namespace std;

class Point {
private:
    double x;
    double y;
public:
    explicit Point (double x_ = 0, double y_ = 0) : x(x_), y(y_) {}
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
    [[nodiscard]] double getX () const {
        return x;
    }
    [[nodiscard]] double getY () const {
        return y;
    }
};

void build (vector<Point> &tmp, int n) {
    for (int i = 0; i < n; i++) {
        int x, y;
        cin >> x >> y;
        tmp.emplace_back(Point(x, y));
    }
    cout << endl;
}

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
    void show () const {
        cout << "\nHave " << points.size() << " points:\n";
        for (const auto &point : points) {
            point.show();
        }
        cout << '\n';
    }
};

class LockedLine : public Line {
private:
    double P = -1;
    double countP() {
        int n = (int) points.size();
        double p = 0;
        for (int i = 0; i < n; i++) {
            int j = (i + 1) % n;
            p += dist(i, j);
        }
        return p;
    }
protected:
    double dist (int i, int j) {
        double x1 = points[i].getX(), y1 = points[i].getY();
        double x2 = points[j].getX(), y2 = points[j].getY();
        return sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
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
    void itsP () const {
        cout << "P: " << P << endl;
    }

    virtual void itsAll () const {
        show();
        itsP();
    }
};

class Polygon : public LockedLine {
private:
    double S = -1;
    double countS () {
        double s = 0;
        int n = (int) points.size();
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
        int n = (int) points.size();
        if (n < 3) {
            return false;
        }
        bool sign1, sign;
        for (int i = 0; i < n; i++) {
            int j = (n + i - 1) % n, k = (n + i + 1) % n;
            double x1 = points[j].getX(), y1 = points[j].getY();
            double x2 = points[i].getX(), y2 = points[i].getY();
            double x3 = points[k].getX(), y3 = points[k].getY();
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
        int n = (int) points.size();
        if (n < 3) {
            return true;
        }
        double x1 = points[0].getX(), y1 = points[0].getY();
        double x2 = points[1].getX(), y2 = points[1].getY();
        for (int i = 2; i < n; i++) {
            double x = points[i].getX(), y = points[i].getY();
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
    void itsS () const {
        cout << "S: " << S << endl;
    }
    void itsAll () const override {
        LockedLine::itsAll();
        itsS();
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

class Trapeze : public Polygon {
private:
    bool itsTrapeze () {
        int n = (int) points.size();
        if (n != 4) {
            return false;
        }
        vector<double> k(n);
        for (int i = 0; i < n; i++) {
            double x1 = points[i].getX(), y1 = points[i].getY();
            double x2 = points[(i + 1) % n].getX(), y2 = points[(i + 1) % n].getY();
            k[i] = (y2 - y1) / (x2 - x1);
        }
        if (!((k[0] == k[2] && k[1] != k[3]) || (k[0] != k[2] && k[1] == k[3]))) {
            return false;
        }
        return true;
    }
public:
    explicit Trapeze (vector<Point> &points_) : Polygon(points_) {
        if (!itsTrapeze()) {
            cout << "Incorrect data: not 4 vertices / not trapeze" << endl;
        }
    }
    Trapeze () = default;
};

class RegularPolygon : public Polygon {
private:
    bool itsRegularSide () {
        int n = (int) points.size();
        double last, now;
        last = dist(0, 1);
        for (int i = 1; i < n; i++) {
            int j = (i + 1) % n;
            now = dist(i, j);
            if (last != now) {
                return false;
            }
            else {
                last = now;
            }
        }
        return true;
    }
    static double deg (double a, double b, double c) {
        return acos((pow(a, 2) + pow(b, 2) - pow(c, 2)) / (2 * a * b)) * 180 / M_PI;
    }
    bool itsRegularDeg() {
        int n = (int) points.size();
        double deg1 = deg(dist(0, 1), dist(1, 2), dist(0, 2));
        for (int i = 1; i < n; i++) {
            int j = (i + 1) % n;
            int k = (i + 2) % n;
            double deg2 = deg(dist(i, j), dist(j, k), dist(i, k));
            if (deg1 != deg2) {
                return false;
            }
        }
        return true;
    }
public:
    explicit RegularPolygon (vector<Point> &points_) : Polygon(points_) {
        if (!itsRegularSide()) {
            cout << "Incorrect data: different sides" << endl;
        }
        if (!itsRegularDeg()) {
            cout << "Incorrect data: different angles" << endl;
        }
    }
    RegularPolygon () = default;
};

int main() {
    while (true) {
        cout << '\n';
        for (int i = 0; i < 20; i++) {
            cout << "#";
        }
        cout << "\nChoose:\n"
                "1. Point\n"
                "2. Line\n"
                "3. Locked line\n"
                "4. Polygon\n"
                "5. Triangle\n"
                "6. Trapeze\n"
                "7. Regular polygon\n"
                "0. Out\n\n";
        int press;
        cin >> press;
        int n;
        vector<Point> tmp;
        int x, y;
        if (press == 1) {
            cin >> x >> y;
            Point my(x, y);
            my.show();
        }
        else if (press >= 2 && press <= 7) {
            cout << "Number of points:\n";
            cin >> n;
            build(tmp, n);
        }

        if (press == 2) {
            Line my (tmp);
            my.show();
        }
        else if (press == 3) {
            LockedLine my (tmp);
            my.itsAll();
        }
        else if (press == 4) {
            Polygon my (tmp);
            my.itsAll();
        }
        else if (press == 5) {
            Triangle my (tmp);
            my.itsAll();
        }
        else if (press == 6) {
            Trapeze my (tmp);
            my.itsAll();
        }
        else if (press == 7) {
            RegularPolygon my (tmp);
            my.itsAll();
        }
        else if (press != 1) {
            break;
        }
    }
}
