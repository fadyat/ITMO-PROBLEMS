#include <iostream>
#include <algorithm>

using namespace std;

class Ring {
public:
    char *buffer;
    int buffer_size;
    int begin;
    int end;
    int reserved;
public:
    explicit Ring(int buffer_size_) : begin(0), end(0), buffer_size(0), buffer(), reserved(0) {
        resize(buffer_size_);
    }

    void resize(int new_buffer_size) {
        char *new_buffer = new char[new_buffer_size];
        for (int i = 0; i < new_buffer_size; i++) {
            new_buffer[i] = '*';
        }
        int tmp_begin = begin, tmp_end = end, new_id = 0;
        if (reserved) {
            while (new_id < min(new_buffer_size, buffer_size) && tmp_begin != tmp_end) {
                new_buffer[new_id] = buffer[tmp_begin];
                tmp_begin = ++tmp_begin % buffer_size;
                new_id++;
            }
            new_buffer[new_id] = buffer[tmp_begin];
            begin = 0;
            if (new_id == min(new_buffer_size, buffer_size)) {
                end = new_id - 1;
            } else { // have a free cell
                end = new_id;
            }
        }
        delete[] buffer;
        buffer = new_buffer;
        buffer_size = new_buffer_size;
    }

    void push_back(char x) {
        if (!reserved) {
            buffer[begin] = x;
            reserved++;
        } else {
            end = ++end % buffer_size;
            int next = end;
            if (next == begin) {
                begin = ++begin % buffer_size;
            } else {
                reserved++;
            }
            buffer[next] = x;
        }
    }

    void push_front(char x) {
        if (!reserved) {
            buffer[begin] = x;
            reserved++;
        } else {
            begin = (--begin + buffer_size) % buffer_size;
            int prev = begin;
            if (prev == end) {
                end = (--end + buffer_size) % buffer_size;
            } else {
                reserved++;
            }
            buffer[prev] = x;

        }
    }

    void pop_back() {
        if (reserved) {
            reserved--;
            buffer[end] = '*';
            end = (--end + buffer_size) % buffer_size;
        }
    }

    void pop_forward() {
        if (reserved) {
            reserved--;
            buffer[begin] = '*';
            begin = ++begin % buffer_size;
        }
    }

    void look() const {
        for (int i = 0; i < buffer_size; i++) {
            cout << buffer[i] << " ";
        }
        cout << endl;
    }

    char operator[](int i) const {
        if (i < buffer_size) {
            return buffer[i];
        }
        return '*';
    }
};


int main() {
    
}
