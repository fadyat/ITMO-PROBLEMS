#include <iostream>
#include "pugixml.hpp"
#include <cassert>
#include <windows.h>
#include <vector>
#include <map>
#include <cmath>
#include <algorithm>

using namespace std;

class other_info {
public:
    vector<string> streets;

    vector<string> others;

    other_info(vector<string> &str, vector<string> &oth) : streets(str), others(oth) {};

    other_info() = default;

    other_info &operator=(const other_info &here) {
        if (this == &here) {
            return *this;
        }
        this->others = here.others;
        this->streets = here.streets;
        return *this;
    }
};

double distance_g(const pair<double, double> &p1, const pair<double, double> &p2) {
    double p1x = p1.first * M_PI / 180, p1y = p1.second * M_PI / 180, p2x = p2.first * M_PI / 180, p2y =
            p2.second * M_PI / 180;
    double d = 2 * asin(sqrt(pow(sin((p2x - p1x) / 2), 2) + cos(p1x) * cos(p2x) * pow(sin((p1y - p2y) / 2), 2)));
    // 6371 km ~ R(Earth)
    return d * 6371;
}

void line() {
    for (int i = 0; i < 30; ++i) {
        cout << '-';
    }
    cout << endl << endl;
}

int main() {
    SetConsoleOutputCP(CP_UTF8);
    pugi::xml_document data;
    pugi::xml_parse_result result = data.load_file("data.xml");
    assert(result);
    pugi::xml_node dataset = data.child("dataset");
    const vector<string> station_info = {"type_of_vehicle", "object_type", "routes", "coordinates", "location",
                                         "number", "name_stopping", "the_official_name"};
    map<string, map<string, map<string, map<pair<double, double>, other_info>>>> omg;
    for (pugi::xml_node station = dataset.child("transport_station"); station; station = station.next_sibling(
            "transport_station")) {
        // type
        string type = station.child_value(station_info[0].c_str());

        // object_type
        string object = station.child_value(station_info[1].c_str());

        // routes divided_routes
        string routes = station.child_value(station_info[2].c_str()), tmp;
        vector<string> divided_routes;
        for (char i : routes) {
            if (!(i == ',' || i == '.')) {
                tmp.push_back(i);
            } else {
                divided_routes.push_back(tmp);
                tmp.clear();
            }
        }
        if (!tmp.empty()) {
            divided_routes.push_back(tmp);
            tmp.clear();
        }

        // latitude and longitude divided_routes
        string coord = station.child_value(station_info[3].c_str());
        pair<double, double> coordll;
        for (char i : coord) {
            if (i != ',') {
                tmp.push_back(i);
            } else {
                coordll.first = stod(tmp);
                tmp.clear();
            }
        }
        coordll.second = stod(tmp);
        tmp.clear();

        // other info
        vector<string> divided_streets, others;
        // streets
        string streets = station.child_value(station_info[4].c_str());
        for (int i = 0; i < streets.size(); i++) {
            if (streets[i] != ',') {
                tmp.push_back(streets[i]);
            } else {
                divided_streets.push_back(tmp);
                tmp.clear();
                if (i + 1 < streets.size() && streets[i + 1] == ' ') {
                    ++i;
                }
            }
        }
        if (!tmp.empty()) {
            divided_streets.push_back(tmp);
            tmp.clear();
        }
        // other
        for (int i = 5; i < station_info.size(); i++) {
            others.emplace_back(station.child_value(station_info[i].c_str()));
        }

        other_info info(divided_streets, others);
        for (const string &route : divided_routes) {
            omg[type][object][route][coordll] = info;
        }
    }
    // #1
    for (const auto &type : omg) {
        cout << type.first << ": " << endl;
        for (const auto &object : type.second) {
            cout << object.first << endl;
            string max_route;
            int max_coord = 0;
            for (const auto &route : object.second) {
                if ((int) route.second.size() > max_coord) {
                    max_coord = (int) route.second.size();
                    max_route = route.first;
                }
            }
            cout << max_route << ": " << max_coord << endl;
        }
        cout << endl;
    }
    line();
    // #2
    for (const auto &type : omg) {
        cout << type.first << ": " << endl;
        for (const auto &object : type.second) {
            cout << object.first << endl;
            string longest_route;
            double max_path = 0;
            for (const auto &route : object.second) {
                double path = 0;
                vector<pair<double, double>> coordinates;
                for (const auto &coord : route.second) {
                    coordinates.push_back(coord.first);
                }
                int next_id, now_id = 0, cnt = (int) coordinates.size();
                vector<bool> used(cnt, false);
                used[0] = true;
                --cnt;
                while (cnt) {
                    double min_tmp_path = 1e9;
                    for (int i = 0; i < coordinates.size(); i++) {
                        if (i == now_id || used[i]) {
                            continue;
                        }
                        double path_to = distance_g(coordinates[now_id], coordinates[i]);
                        if (min_tmp_path > path_to) {
                            min_tmp_path = path_to;
                            next_id = i;
                        }
                    }
                    --cnt;
                    now_id = next_id;
                    used[now_id] = true;
                    path += min_tmp_path;
                }
                if (path > max_path) {
                    max_path = path;
                    longest_route = route.first;
                }

            }
            if (!longest_route.empty()) {
                cout << longest_route << ": " << max_path << endl;
            } else {
                cout << "None" << endl;
            }
        }
        cout << endl;
    }
    line();
    // #3
    map<string, map<string, map<string, int>>> street_stops;
    for (const auto& type : omg) {
        for (const auto& object : type.second) {
            for (const auto& route : object.second) {
                for (const auto& coord : route.second) {
                    for (const auto &street : coord.second.streets) {
                        ++street_stops[type.first][object.first][street];
                    }
                }
            }
        }
    }
    for (const auto& type : street_stops) {
        cout << type.first <<": " << endl;
        for (const auto &object : type.second) {
            cout << object.first << endl;
            int max_value = 0;
            string max_street;
            for (const auto &street : object.second) {
                if (street.second > max_value) {
                    max_value = street.second;
                    max_street = street.first;
                }
            }
            cout << max_street << ": " << max_value << endl;
        }
        cout << endl;
    }
}
