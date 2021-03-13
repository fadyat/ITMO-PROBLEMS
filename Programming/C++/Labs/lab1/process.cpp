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
    static double distance (Point &p1, Point &p2) {
        return sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
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
    double P = 0;
    static double countP(vector<Point> &points_) {
        double s = 0;
        for (int i = 0; i < points_.size(); i++) {
            s += points_[i].distance(points_[i], points_[(i + 1) % points_.size()]);
        }
        return s;
    }
public:
    explicit LockedLine (vector<Point> &points_) : Line(points_), P(countP(points_)) {}
    LockedLine () = default;
    LockedLine (const LockedLine &other) : Line(other) {
        this->P = other.P;
    }
    LockedLine& operator = (const LockedLine &other) {
        Line::operator = (other);
        this->P = other.P;
        return *this;
    }
    void itsP () const {
        cout << "P: \n" << P << endl;
    }
};

int main() {/*
    vector<Point> all;
    all.emplace_back(0, 0);
    all.emplace_back(2, 0);
    all.emplace_back(2, 2);
    all.emplace_back(0, 2);
    LockedLine line2(all);
    line2.itsP();
    line2.show();
    vector<Point> all1;
    all1.emplace_back(0, 0);
    all1.emplace_back(3, 0);
    all1.emplace_back(3, 3);
    all1.emplace_back(0, 3);
    LockedLine line1 (all1);
    line1.itsP();
    line1.show();
    LockedLine line3 = line1;
    line3.itsP();
    line3.show();
    line3 = line1;
    line3.itsP();
    line3.show();
    line3 = line2;
    line3.itsP();
    line3.show();*/
}
