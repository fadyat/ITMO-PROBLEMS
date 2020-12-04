#include <iostream>
#include <fstream>
#include <vector>
#include <map>

using namespace std;
// wrong

vector<char> bad = {'!', '.', ',', '?', '!',
 ';', ':', '"','(', ')', '«', '»'};

bool is_it_good(char x) {
    for(int i = 0; i < (int)bad.size(); i++) {
        if(x == bad[i]) {
            return false;
        }
    }
    return true;
}

string check(string &s) {
    string new_s;
    if(s == "—") { // not hyphen, it's dash
        return "-";
    }
    for(int i = 0; i < (int)s.size(); i++) {
        if(is_it_good(s[i])) {
            if(s[i] >= 'À' && s[i] <= 'ß') {
                s[i] = s[i] - 'A' + 'a';
            }
            else if(s[i] >= 'A' && s[i] <= 'Z') {
                s[i] = s[i] - 'A' + 'a';
            }
            new_s.push_back(s[i]);
        }
    }
    return new_s;
}

int levenshtein(string s1, string s2, int ins, int del, int rep) {
    vector<vector<int>> D((int)s1.size(), vector<int>((int)s2.size()));
    D[0][0] = 0;
    for(int i = 1; i < (int)s2.size(); i++){
            D[0][i] = D[0][i - 1] + ins;
    }
    for(int i = 1; i < (int)s1.size(); i++) {
        D[i][0] = D[i - 1][0] + del;
        for(int j = 1; j < (int)s2.size(); j++) {
            if(s1[i] != s2[j]) {
                D[i][j] = min(D[i - 1][j] + del,
                              min(D[i][j - 1] + ins,
                                  D[i - 1][j - 1] + rep));
            }
            else {
                D[i][j] = D[i - 1][j - 1];
            }
        }
    }
    return D[(int)s1.size() - 1][(int)s2.size() - 1];
}

int main() {
    setlocale(LC_ALL, "Russian");
    ifstream in("input.txt");
    in.is_open();
    string s;
    map<string, pair<int, string>> all;
    vector<string> my_ans;
    while(in >> s) {
        string new_s = check(s);
        if(new_s != "-") {
            all[new_s].first++;
            all[new_s].second = new_s;
            my_ans.push_back(new_s);
        }
    }

    int forms = 0;
    for(auto it = all.begin(); it != all.end(); it++) {
            forms += (*it).second.first;
    }
    cout << "Total wordforms: " << forms << '\n';
    cout << "Different wordforms: " << (int)all.size() << '\n' << '\n';
    in.close();

    ifstream inn("dictionary.txt");
    inn.is_open();
    map<string, int> dict;
    bool now_string = true;
    string word;
    while(inn >> s) {
        if(now_string) {
            word = s;
            now_string = false;
        }
        else {
            dict[word] = stoi(s);
            now_string = true;
        }
    }

    vector<pair<string, string>> not_here;

    int dif_forms = 0;
    for(auto it = all.begin(); it != all.end(); it++) {
        if(dict.count((*it).first)) {
            dif_forms++;
        }
        else {
            not_here.push_back({(*it).first, ""});
        }
    }
    cout << "Different wordforms in dictionary: " << dif_forms << '\n';

    cout << "Not in dictionary(potentional mistakes): " << (int)not_here.size() << '\n' << '\n';
    inn.close();
    /*                              Levenshtein distance
        Time:
            O(not_here * dictionary * (not_here[i].size * dictionary[i].size)) <->
            O(n^4) -> n ~ 100
            it's if length of the string is equal to numbers of this strings
        Memory:
            O(n^4) ~ so much

    */
    int yet = 0;
    for(int i = 0; i < not_here.size(); i++) {
        int ans = 1000;
        for(auto j = dict.begin(); j != dict.end(); j++) {
            // here I count Levenshtein distance for my wrong wordform
            int tmp = levenshtein(not_here[i].first, (*j).first, 1, 1, 1);
            if(tmp < ans) {
                not_here[i].second = (*j).first;
                ans = tmp;
            }
        }
        cout << not_here[i].first << " - ";
        if(ans <= 2) {
            cout << not_here[i].second << " - " << ans << '\n';
        }
        else {
            yet++;
            not_here[i].second = "badd";
            cout << "not find - >2" << '\n';
        }
        /* update string in all */
        all[not_here[i].first].second = not_here[i].second;
    }

    dif_forms = 0;
    int forms1 = 0;
    for(auto it = all.begin(); it != all.end(); it++) {
        if((*it).second.second != "badd") {
            dif_forms++;
            forms += (*it).second.first;
        }
    }
    cout << "\nDifferent wordforms: " << dif_forms << '\n';
    cout << "Not in dictionary(potentional mistakes): " << forms1 << '\n';

    ofstream out("answer.txt");
    out.is_open();
    for(int i = 0; i < my_ans.size(); i++) {
        out << all[my_ans[i]].second << " ";
    }
    out.close();
}
