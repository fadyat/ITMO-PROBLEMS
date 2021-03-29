#include <iostream>
#include <vector>
#include <algorithm>
#include <cmath>
#include <iomanip>

using namespace std;

class Point {
private:
    int x;
    int y;
public:
    Point (int x_, int y_) {
        x = x_;
        y = y_;
    }
    double dist (Point p) const {
        return (sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y)));
    }
};

int main() {
    freopen("spantree.in", "r", stdin);
    freopen("spantree.out", "w", stdout);
    int n;
    cin >> n;
    vector<Point> points;
    for (int i = 0; i < n; i++) {
        int x, y;
        cin >> x >> y;
        points.emplace_back(Point(x, y));
    }

    vector<double> freeD(n, 0);
    vector<bool> used(n, false);
    used[0] = true;
    for (int i = 1; i < n; i++) {
        freeD[i] = points[0].dist(points[i]);
    }
    double ans = 0;
    while (true) {
        double minpath = 1e9;
        int next = 1e9;
        for (int i = 1; i < n; i++) {
            if (!used[i] && freeD[i] < minpath) {
                minpath = freeD[i];
                next = i;
            }
        }
        if (next == 1e9) {
            break;
        }
        ans += freeD[next];
        used[next] = true;
        for (int i = 1; i < n; i++) {
            freeD[i] = min (freeD[i], points[next].dist(points[i]));
        }
    }
    cout << fixed << setprecision(15) << ans << endl;
}
