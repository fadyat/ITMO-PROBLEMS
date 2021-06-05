#include <iostream>
#include <algorithm>
#include <cassert>

using namespace std;

template<typename T>
class Ring {
public:
    T *buffer;
    int capacity;
    int start;
    int finish;
    int reserved;
    int max_capacity;

public:
    explicit Ring(int max_capacity_) : start(0), finish(0), capacity(2), buffer(), reserved(0),
                                       max_capacity(max(max_capacity_, 2)) {
        buffer = new T[capacity];
    }

    void resize(int new_capacity) {
        assert(new_capacity > 0);
        T *new_buffer = new T[new_capacity];
        int l = start, r = finish, id = 0;
        while (id < min(new_capacity, capacity) && l != r) {
            new_buffer[id] = buffer[l];
            l = ++l % capacity;
            id++;
        }
        if (id != min(new_capacity, capacity) && reserved) {
            new_buffer[id] = buffer[l];
            id++;
        }
        start = 0;
        finish = (((id - 1) < 0) ? (0) : (id - 1));
        reserved = id;
        delete[] buffer;
        buffer = new_buffer;
        capacity = min(new_capacity, capacity);
        max_capacity = new_capacity;
    }

    void make_lr_normal() {
        T *new_buffer = new T[capacity];
        int l = start, r = finish, id = 0;
        while (l != r) {
            new_buffer[id] = buffer[l];
            l = ++l % capacity;
            id++;
        }
        if (id != capacity && reserved) {
            new_buffer[id] = buffer[l];
            id++;
        }
        start = 0;
        finish = (((id - 1) < 0) ? (0) : (id - 1));
        delete[] buffer;
        buffer = new_buffer;
    }

    void resize_before_max(bool type_of_push) {
        int new_capacity = min(capacity * 2, max_capacity);
        T *new_buffer = new T[new_capacity];
        int l = start, r = finish, id = type_of_push;
        while (l != r) {
            new_buffer[id] = buffer[l];
            l = ++l % capacity;
            id = ++id % new_capacity;
        }
        new_buffer[id++] = buffer[l];
        start = type_of_push;
        finish = id - 1;
        delete[] buffer;
        buffer = new_buffer;
        capacity = new_capacity;
        reserved++;
    }

    void push_back(T x) {
        if (!reserved) {
            buffer[start] = x;
            reserved++;
        } else {
            int next = (finish + 1) % capacity;
            if (next == start) {
                if (capacity < max_capacity) {
                    resize_before_max(false);
                    next = ++finish;
                } else {
                    finish = next;
                    start = ++start % capacity;
                }
            } else {
                finish = next;
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
            int prev = (start - 1 + capacity) % capacity;
            if (prev == finish) {
                if (capacity < max_capacity) {
                    resize_before_max(true);
                    prev = --start;
                } else {
                    start = prev;
                    finish = (--finish + capacity) % capacity;
                }
            } else {
                start = prev;
                reserved++;
            }
            buffer[prev] = x;
        }
    }

    void pop_back() {
        if (reserved) {
            reserved--;
            if (reserved || finish != 0) {
                finish = (--finish + capacity) % capacity;
            }
        }
    }

    void pop_forward() {
        if (reserved) {
            reserved--;
            if (reserved || start != 0) {
                start = ++start % capacity;
            }
        }
    }

    T &operator[](int i) const {
        assert(i >= 0 && i < capacity);
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

        Custom_iterator &operator=(const Custom_iterator &other) {
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

        Custom_iterator &operator++() {
            ++this->ptr;
            return *this;
        }

        Custom_iterator operator++(int) {
            Custom_iterator tmp = *this;
            ++(*this->ptr);
            return tmp;
        }

        Custom_iterator &operator--() {
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

        Custom_iterator &operator+=(int n) {
            this->ptr += n;
            return *this;
        }

        Custom_iterator &operator-=(int n) {
            this->ptr -= n;
            return *this;
        }

        T &operator*() {
            return *ptr;
        }
    };

    Custom_iterator begin() {
        return Custom_iterator(buffer);
    }

    Custom_iterator end() {
        return Custom_iterator(buffer + reserved);
    }

};

int main() {
    Ring<int> ogo(12);
    ogo.push_back(1);
    ogo.resize(14);
    ogo.push_back(2);
    ogo.push_back(3);
    ogo.push_front(4);
    ogo.push_back(12);
    ogo.push_front(13);
//    ogo.pop_back();
//    ogo.pop_forward();
//    ogo.resize(4);
    ogo.make_lr_normal();
//    sort(ogo.begin(), ogo.end());
    for (auto i : ogo) {
        cout << i << " ";
    }
    cout << endl;
}
