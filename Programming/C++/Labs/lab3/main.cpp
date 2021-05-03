#include <iostream>
#include "pugixml.hpp"
#include <cassert>
#include <windows.h>
#include <vector>
#include <map>

using namespace std;

class other_info {
private:
    vector<string> streets;
    vector<string> others;
public:
    other_info(vector<string> &str, vector<string> &oth) : streets(str), others(oth) {};

    other_info() = default;

    other_info& operator=(const other_info &here) {
        if (this == &here) {
            return *this;
        }
        this->others = here.others;
        this->streets = here.streets;
        return *this;
    }

    friend ostream& operator<<(ostream& os, const other_info& here) {
        for (int i = 0; i < here.streets.size(); i++) {
            os << here.streets[i];
            if (i != here.streets.size() - 1) {
                os << " : ";
            }
            else {
                os << ' ';
            }
        }
        os << "/ ";
        for (int i = 0; i < here.others.size(); i++) {
            os << here.others[i];
            if (i != here.others.size() - 1) {
                os << " : ";
            }
            else {
                os << ' ';
            }
        }
        return os;
    }

};

int main() {
    SetConsoleOutputCP(CP_UTF8);
    pugi::xml_document data;
    pugi::xml_parse_result result = data.load_file("data.xml");
    assert(result);
    pugi::xml_node dataset = data.child("dataset");
    const vector<string> station_info = {"type_of_vehicle", "object_type", "routes", "coordinates", "location", "number",  "name_stopping", "the_official_name"};
    map<string, map<string, map<string, map<pair<double, double>, other_info>>>> omg;
    for (pugi::xml_node station = dataset.child("transport_station"); station; station = station.next_sibling("transport_station")) {
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
            }
            else {
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
            }
            else {
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
            }
            else {
                divided_streets.push_back(tmp);
                tmp.clear();
                if (streets[i + 1] < streets.size() && streets[i + 1] == ' ') {
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
//    for (const auto& type : omg) {
//        cout << type.first << endl << endl;
//        for (const auto& object : type.second) {
//            cout << object.first << endl << endl << endl;
//            for (const auto& route : object.second) {
//                cout << route.first << endl;
//                for (const auto& k : route.second) {
//                    cout << k.first.first << " " << k.first.second << endl;
//                    cout << k.second << endl;
//                }
//            }
//        }
//    }
}