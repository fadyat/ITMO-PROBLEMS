#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

class Point {
private:
    int x;
    int y;
public:
    explicit Point (int x_ = 0, int y_ = 0) {
        x = x_;
        y = y_;
    }
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
    void pointShow () const {
        cout << x << " " << y << endl;
    }
};

class Line {
private:
    vector<Point> points;
public:
    explicit Line (vector<Point> &points_) {
        points = points_;
    }
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
    void lineShow() {
        cout << "Line at " << points.size() << " points: " << endl;
        for (Point &point : points) {
            point.pointShow();
        }
        cout << '\n';
    }
};

int main() {

}
