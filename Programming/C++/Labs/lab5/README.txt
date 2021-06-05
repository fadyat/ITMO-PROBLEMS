lab5.cpp
- Version containing a regular ring buffer without max_capacity.
- Can be bugs with resize (m): m < prev_capacity.
- Iterators take out "garbage".

lab5-max_capacity.cpp
- Increases the size to max_capacity, if necessary, by analogy with std::vector.
- The circular buffer is provided with a random access iterator.
- Minimum buffer size = 2.

(*)Let's talk about the functions:
    resize(int ...)
    - overwrites items in the correct order
    - saves prev_capacity if possible
    - updates the max_capacity

    make_lr_normal()
    - overwrites elements in the correct order (made to work normally with iterators)

    resize_before_max(bool ...)
    - updates capacity to min(capacity * 2, max_capacity)
    - overwrites items in the correct order
    - used only if((capacity == reserved) && (capacity < max_capacity))
    - takes bool to determine which type of insert is made, if push_back() -> false, if push_front() -> true; thereby shifts all elements as needed.

    push_back(T)
    - insert an element to the right of finish

    push_front(T)
    - insert item to the left of start

    pop_back(), pop_front() - delete the last and first, respectively.
