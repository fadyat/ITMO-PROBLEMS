def build(FILE):
    line = FILE.readline()
    n, m = map(int, line.split())
    ans = []
    tmp = []
    line = FILE.readline()
    for i in line.split():
        tmp.append(float(i))
    for i in range(n):
        tmp1 = []
        for j in range(m):
            tmp1.append(tmp[i * m + j])
        ans.append(tmp1)
    return ans


def transport(mtx):
    n = len(mtx)
    m = len(mtx[0])
    ans = []
    for i in range(m):
        tmp = []
        for j in range(n):
            tmp.append(mtx[j][i])
        ans.append(tmp)
    return ans


def on_value(k, ans):
    n = len(ans)
    m = len(ans[0])
    for i in range(n):
        for j in range(m):
            ans[i][j] *= k
    return ans


def mtx_sum(mtx1, mtx2):
    n1 = len(mtx1)
    m1 = len(mtx1[0])
    n2 = len(mtx2)
    m2 = len(mtx2[0])
    if n1 != n2 or m1 != m2:
        return -1
    for i in range(n1):
        for j in range(m1):
            mtx1[i][j] += mtx2[i][j]
    return mtx1


def on_mtx(mtx1, mtx2):
    n1 = len(mtx1)
    m1 = len(mtx1[0])
    n2 = len(mtx2)
    m2 = len(mtx2[0])
    if m1 != n2:
        return -1
    ans = []
    for i in range(n1):
        tmp = []
        for j in range(m2):
            s = 0
            for k in range(m1):
                s += mtx1[i][k] * mtx2[k][j]
            tmp.append(s)
        ans.append(tmp)
    return ans


def operations(k1, k2, a, b, c, d, f):
    # ans = C·((k1·A + k2·(B)^T)^T)·D − F
    a = on_value(k1, a)
    b = transport(b)
    b = on_value(k2, b)
    new = mtx_sum(a, b)
    if new == -1:
        return -1
    new = transport(new)
    new = on_mtx(c, new)
    if new == -1:
        return -1
    new = on_mtx(new, d)
    if new == -1:
        return -1
    f = on_value(-1, f)
    new = mtx_sum(new, f)
    if new == -1:
        return -1
    return new


# main
with open("input.txt", "r") as FILE:
    line = FILE.readline()
    k1, k2 = map(float, line.split())
    a = build(FILE)
    b = build(FILE)
    c = build(FILE)
    d = build(FILE)
    f = build(FILE)

with open("output.txt", "w") as FILE:
    _ans = operations(k1, k2, a, b, c, d, f)
    if _ans == -1:
        FILE.write("0\n")
    else:
        FILE.write("1\n")
        n = len(_ans)
        m = len(_ans[0])
        FILE.write(str(n) + ' ' + str(m) + '\n')
        for i in range(n):
            for j in range(m):
                FILE.write(str(_ans[i][j]) + ' ')
            FILE.write('\n')
