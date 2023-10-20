import math


def where(ex, ey, sx, sy, px, py):
    s = (ex - sx) * (py - sy) - (ey - sy) * (px - sx)
    if s >= 0:
        return 1
    elif s < 0:
        return -1


def len_(px, py, pz):
    return math.sqrt(px * px + py * py + pz * pz)


def degree(px, py, pz, qx, qy, qz):
    sum_ = px * qx + py * qy + pz * qz
    return math.degrees(math.acos(sum_ / (len_(px, py, pz) * len_(qx, qy, qz))))


with open("input.txt", "r") as file:
    line = file.readline()
    vx, vy = map(float, line.split())
    line = file.readline()
    ax, ay = map(float, line.split())
    line = file.readline()
    mx, my = map(float, line.split())
    line = file.readline()
    wx, wy = map(float, line.split())

zx = wx - vx
zy = wy - vy
alp = 180 - degree(ax, ay, 0, zx, zy, 0)
if alp >= 90:
    alp -= 90
elif alp <= 90:
    alp -= 90

with open("output.txt", "w") as file:
    if alp > 60 or alp < -60:
        file.write("0\n")
    else:
        check = where(ax, ay, 0, 0, wx, wy)
        file.write(str(check) + "\n")
        file.write(str(alp) + "\n")
        y = degree(mx, my, 1, 0, 0, 1)
        if mx >= 0:
            y *= -1
        file.write(str(y) + "\n")
        file.write("kek")   
