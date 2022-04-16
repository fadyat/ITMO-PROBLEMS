import math
import numpy as np


def brents_method(f, l, r, eps, s=1):
    gr = (math.sqrt(5) - 1) / 2
    m = w = v = l + gr * (r - l)
    fm = fw = fv = s * f(m)
    d = e = 0
    u = float('+inf')
    i = 0
    while r - l > eps:
        g, e = e, d
        if len({m, w, v}) == len({fm, fw, fv}) == 3:
            p = ((m - w) ** 2) * (fm - fv) - ((m - v) ** 2) * (fm - fw)
            q = 2 * ((m - w) * (fm - fv) - (m - v) * (fm - fw))
            u = m - p / q

        if l + eps <= u <= r - eps and 2 * abs(u - m) < g:
            d = abs(u - m)
        else:
            if 2 * m < (r + l):
                d = r - m
                u = m + gr * d
            else:
                d = m - l
                u = m - gr * d

        if abs(u - m) < eps:
            return u

        fu = s * f(u)

        if fu <= fm:
            if u >= m:
                l = m
            else:
                r = m
            v, w, m = w, m, u
            fv, fw, fm = fw, fm, fu

        else:
            if u >= m:
                r = u
            else:
                l = u

            if fu <= fw or w == m:
                v, w = w, u
                fv, fw = fw, fu
            elif fu <= fv or v == m or v == w:
                v = u
                fv = fu
    return (l + r) * .5


def dichotomous_search(f, l, r, eps, s=1):
    delta = eps / 3
    while r - l > eps:
        m = (l + r) * .5
        x1 = m - delta
        x2 = m + delta
        if s * f(x1) > s * f(x2):
            l = x1
        else:
            r = x2
    return (l + r) * .5


def fibonacci_search(f, l, r, eps, s=1):
    fib = np.array([1, 1])
    while fib[-1] <= (r - l) / eps:
        fib = np.append(fib, fib[-1] + fib[-2])

    d = lambda k: (r - l) * (fib[n - k] / fib[n - k + 1])
    n = len(fib) - 1
    x1, x2 = r - d(1), l + d(1)
    f1, f2 = s * f(x1), s * f(x2)
    for k in range(1, n):
        if f1 > f2:
            l, x1, f1 = x1, x2, f2
            x2 = l + d(k)
            f2 = s * f(x2)
        else:
            r, x2, f2 = x2, x1, f1
            x1 = r - d(k)
            f1 = s * f(x1)

    return (l + r) * .5


def golden_section_search(f, l, r, eps, s=1):
    d = lambda: gr * (r - l)
    gr = (math.sqrt(5) - 1) * .5
    x1, x2 = r - d(), l + d()
    f1, f2 = s * f(x1), s * f(x2)
    i = 0
    while r - l > eps:
        if f1 > f2:
            l, x1, f1 = x1, x2, f2
            x2 = l + d()
            f2 = s * f(x2)
        else:
            r, x2, f2 = x2, x1, f1
            x1 = r - d()
            f1 = s * f(x1)
    return (l + r) * .5


def successive_parabolic_interpolation(f, l, r, eps, s=1):
    m = (l + r) * .5
    f1, f2, f3 = s * f(l), s * f(m), s * f(r)
    while r - l > eps:
        p = ((m - l) ** 2) * (f2 - f3) - ((m - r) ** 2) * (f2 - f1)
        q = 2 * ((m - l) * (f2 - f3) - (m - r) * (f2 - f1))
        u = m - p / q
        fu = s * f(u)
        if m > u:
            if f2 < fu:
                l, f1 = u, fu
            else:
                r, f3 = m, f2
                m, f2 = u, fu
        else:
            if f2 > fu:
                l, f1 = m, f2
                m, f2 = u, fu
            else:
                r, f3 = u, fu

    return (l + r) / 2


if __name__ == '__main__':
    def f(x):
        return math.sin(0.5 * math.log(x) * x) + 1


    print(f'bm: {brents_method(f, 4, 7, 1e-3)}')
    print(f'ds: {dichotomous_search(f, 4, 7, 1e-3)}')
    print(f'fs: {fibonacci_search(f, 4, 7, 1e-3)}')
    print(f'gss: {golden_section_search(f, 4, 7, 1e-3)}')
    print(f'spi: {successive_parabolic_interpolation(f, 4, 7, 1e-3)}')
