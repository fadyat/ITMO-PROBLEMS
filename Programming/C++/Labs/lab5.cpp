#include <iostream>
#include <algorithm>
#include <cassert>

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
            buffer[finish] = 0; // not necessary, made for clarity
            finish = (--finish + buffer_size) % buffer_size;
        }
    }

    void pop_forward() {
        if (reserved) {
            reserved--;
            buffer[start] = 0; // not necessary, made for clarity
            start = ++start % buffer_size;
        }
    }

    T &operator[](int i) const {
        assert(i >= 0 && i < buffer_size);
        return buffer[i];
    }

    ~Ring() {
        delete[] buffer;
    }

    class Custom_iterator : public iterator<random_access_iterator_tag, T> {
    private:
        T *ptr;
    public:
        explicit Custom_iterator(T *ptr_) : ptr(ptr_) {}

        Custom_iterator(const Custom_iterator &other) : ptr(other.ptr) {}

        Custom_iterator& operator=(const Custom_iterator &other) {
            if (this == &other) {
                return *this;
            }
            ptr = other.ptr;
            return *this;
        }

        bool operator<(Custom_iterator &other) {
            return ptr < other.ptr;
        }

        bool operator>=(Custom_iterator &other) {
            return !(ptr < other.ptr);
        }

        bool operator>(Custom_iterator &other) {
            return ptr > other.ptr;
        }

        bool operator<=(Custom_iterator &other) {
            return !(ptr > other.ptr);
        }

        bool operator==(const Custom_iterator &other) {
            return ptr == other.ptr;
        }

        bool operator!=(const Custom_iterator &other) {
            return ptr != other.ptr;
        }

        ptrdiff_t operator-(Custom_iterator &other) {
            return ptr - other.ptr;
        }

        Custom_iterator& operator++() {
            ++this->ptr;
            return *this;
        }

        Custom_iterator operator++(int) {
            Custom_iterator tmp = *this;
            ++(*this->ptr);
            return tmp;
        }

        Custom_iterator& operator--() {
            --this->ptr;
            return *this;
        }

        Custom_iterator operator--(int) {
            Custom_iterator tmp = *this;
            --(*this->ptr);
            return tmp;
        }

        friend Custom_iterator operator+(Custom_iterator &other, int n) {
            return Custom_iterator(other.ptr + n);
        }

        friend Custom_iterator operator+(int n, Custom_iterator &other) {
            return Custom_iterator(other.ptr + n);
        }

        friend Custom_iterator operator-(Custom_iterator &other, int n) {
            return Custom_iterator(other.ptr - n);
        }

        friend Custom_iterator operator-(int n, Custom_iterator &other) {
            return Custom_iterator(other.ptr - n);
        }

        Custom_iterator& operator+=(int n) {
            this->ptr += n;
            return *this;
        }

        Custom_iterator& operator-=(int n) {
            this->ptr -= n;
            return *this;
        }

        T& operator*() {
            return *ptr;
        }
    };

    Custom_iterator begin() {
        return Custom_iterator(buffer);
    }

    Custom_iterator end() {
        return Custom_iterator(buffer + buffer_size);
    }

};

int main() {
    Ring<int> ogo(12);
    ogo.push_back(3);ogo.push_back(1);ogo.push_back(-111);
    ogo.push_back(0);ogo.push_back(0);ogo.push_back(0);ogo.push_back(0);
    ogo.push_front(31);ogo.push_front(31);ogo.push_front(31);ogo.push_front(31);ogo.push_front(31);
    for (auto &i : ogo) {
        i += 11;
        cout << i << " ";
    }
    cout << endl;
    cout << *find(ogo.begin(), ogo.end(), 11) << endl;
    sort(ogo.begin(), ogo.end());
    cout << binary_search(ogo.begin(), ogo.end(), 11) << endl;
    for (auto i : ogo) {
        cout << i << " ";
    }
}
