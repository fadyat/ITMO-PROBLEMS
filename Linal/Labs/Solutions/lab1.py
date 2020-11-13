# with open("input.txt", "r") as file:
def readMatrix(file):
    line = file.readline()
    an, am = map(int, line.split())
    line = file.readline()
    matrix = []
    for i in range(an):
        matrix.append([0] * am)
    p = [float(i) for i in line.split()]
    for i in range(an):
        for j in range(am):
            matrix[i][j] = p[i * am + j]
    return matrix


def transMatrix(matrix):
    n = len(matrix)
    m = len(matrix[0])
    t = []
    for i in range(m):
        t.append([0] * n)
    for i in range(n):
        for j in range(m):
            t[j][i] = matrix[i][j]
    return t


def sumMatrix(m1, m2):
    n1 = len(m1)
    n2 = len(m2)
    ml1 = len(m1[0])
    print(ml1)
    ml2 = len(m2[0])
    if n1 != n2 or ml1 != ml2:
        return False
    n = n1
    m = ml2
    s = []
    for i in range(n):
        s.append([0] * m)
    # s = [[0] * m for i in range(n)]
    for i in range(n):
        for j in range(m):
            s[i][j] = m1[i][j] + m2[i][j]
    return s


def valMatrixCompostion(a, matrix):
    n = len(matrix)
    m = len(matrix[0])
    s = []
    # s = [[0] * m for i in range(n)]
    for i in range(n):
        s.append([0] * m)
    for i in range(n):
        for j in range(m):
            s[i][j] = matrix[i][j] * a
    return s


def matrixCompostion(matrix1, matrix2):
    n1 = len(matrix1)
    m1 = len(matrix1[0])
    n2 = len(matrix2)
    m2 = len(matrix2[0])
    if m1 != n2:
        return -1
    t = []
    # t = [[0] * m2 for i in range(n1)]
    for i in range(n1):
        t.append([0] * m2)
    for i in range(n1):
        for j in range(m2):
            s = 0
            for q in range(m1):
                s += matrix1[i][q] * matrix2[q][j]
            t[i][j] = s
    return t


def func(z, x, a, b, c, d, f):
    a = valMatrixCompostion(z, a)
    b = transMatrix(b)
    b = valMatrixCompostion(x, b)
    s = transMatrix(sumMatrix(a, b))
    if s == -1:
        return -1
    s = matrixCompostion(c, s)
    if s == -1:
        return -1
    s = matrixCompostion(s, d)
    if s == -1:
        return -1
    f = valMatrixCompostion(-1, f)
    s = sumMatrix(s, f)
    return s


with open("input.txt", "r") as file:
    line = file.readline()
    z, x = map(float, line.split())
    a = readMatrix(file)
    b = readMatrix(file)
    c = readMatrix(file)
    d = readMatrix(file)
    f = readMatrix(file)
# print(matrixCompostion([[1, 2], [1,3]],[[1,7], [5,5]]))
with open("output.txt","w") as file:
    q = func(z, x, a, b, c, d, f)
    if q == -1:
        file.write("0")
        file.write("\n")
    else:
        file.write("1")
        file.write("\n")
        file.write(str(len(q)) +" "+ str(len(q[0])))
        file.write("\n")
        n = len(q)
        m = len(q[0])
        for i in range(n):
            for j in range(m):
                file.write(str(float(q[i][j])) + " ")