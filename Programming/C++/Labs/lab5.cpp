#include <iostream>
#include <algorithm>

using namespace std;

template<typename T>
class Ring {
private:
    T *buffer;
    int buffer_size;
    int start;
    int finish;
    int reserved;
public:
    explicit Ring(int buffer_size_ = 1) : start(0), finish(0), buffer_size(0), buffer(), reserved(0) {
        resize(buffer_size_);
    }

    void resize(int new_buffer_size) {
        T *new_buffer = new T[new_buffer_size];
        int tmp_begin = start, tmp_end = finish, new_id = 0;
        if (reserved) {
            while (new_id < min(new_buffer_size, buffer_size) && tmp_begin != tmp_end) {
                new_buffer[new_id] = buffer[tmp_begin];
                tmp_begin = ++tmp_begin % buffer_size;
                new_id++;
            }
            new_buffer[new_id] = buffer[tmp_begin];
            start = 0;
            if (new_id == min(new_buffer_size, buffer_size)) { // all cells are occupied
                finish = new_id - 1;
            } else { // have a free cell
                finish = new_id;
            }
        }
        delete[] buffer;
        buffer = new_buffer;
        buffer_size = new_buffer_size;
    }

    void push_back(T x) {
        if (!reserved) {
            buffer[start] = x;
            reserved++;
        } else {
            finish = ++finish % buffer_size;
            int next = finish;
            if (next == start) {
                start = ++start % buffer_size;
            } else {
                reserved++;
            }
            buffer[next] = x;
        }
    }

    void push_front(T x) {
        if (!reserved) {
            buffer[start] = x;
            reserved++;
        } else {
            start = (--start + buffer_size) % buffer_size;
            int prev = start;
            if (prev == finish) {
                finish = (--finish + buffer_size) % buffer_size;
            } else {
                reserved++;
            }
            buffer[prev] = x;
        }
    }

    void pop_back() {
        if (reserved) {
            reserved--;
            buffer[finish] = 0;
            finish = (--finish + buffer_size) % buffer_size;
        }
    }

    void pop_forward() {
        if (reserved) {
            reserved--;
            buffer[start] = 0;
            start = ++start % buffer_size;
        }
    }

    T &operator[](int i) const {
        if (i >= 0 && i < buffer_size) {
            return buffer[i];
        }
        return buffer[buffer_size - 1];
    }

    ~Ring() {
        delete[] buffer;
    }

    class Iterator : public iterator<random_access_iterator_tag, T> {
    private:
        T *link;
    public:
        explicit Iterator(T *link_) : link(link_) {}

        Iterator(const Iterator &other) : link(other.link) {}

        Iterator& operator=(const Iterator &other) {
            if (this == &other) {
                return *this;
            }
            link = other.link;
            return *this;
        }

        bool operator<(Iterator &other) {
            return link < other.link;
        }

        bool operator>=(Iterator &other) {
            return !(link < other.link);
        }

        bool operator>(Iterator &other) {
            return link > other.link;
        }

        bool operator<=(Iterator &other) {
            return !(link > other.link);
        }

        bool operator==(const Iterator &other) {
            return link == other.link;
        }

        bool operator!=(const Iterator &other) {
            return link != other.link;
        }

        ptrdiff_t operator-(Iterator &other) {
            return link - other.link;
        }

        T& operator++() {
            return *(++link);
        }

        T operator++(int) {
            Iterator tmp(link++);
            return tmp.link;
        }

        T& operator--() {
            return *(--link);
        }

        T operator--(int) {
            Iterator tmp(link--);
            return tmp.link;
        }

        friend Iterator operator+(Iterator &other, int n) {
            return Iterator(other.link + n);
        }

        friend Iterator operator+(int n, Iterator &other) {
            return Iterator(other.link + n);
        }

        friend Iterator operator-(Iterator &other, int n) {
            return Iterator(other.link - n);
        }

        friend Iterator operator-(int n, Iterator &other) {
            return Iterator(other.link - n);
        }

        T& operator*() {
            return *link;
        }

        T& operator+=(int n) {
            link += n;
            return *link;
        }

        T& operator-=(int n) {
            link -= n;
            return *link;
        }
    };

    Iterator begin() {
        return Iterator(buffer);
    }

    Iterator end() {
        return Iterator(buffer + buffer_size);
    }

};

int main() {
    Ring<int> ogo(3);
    ogo.push_back(1);ogo.push_back(2);ogo.push_back(3);
    cout << *find(ogo.begin(), ogo.end(), 1) << endl;
    cout << binary_search(ogo.begin(), ogo.end(), 1) << endl;
    sort_heap(ogo.begin(), ogo.end());
    for (auto i : ogo) {
        cout << i << " ";
    }
}
