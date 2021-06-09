import matplotlib.pyplot as plt
import math
import numpy


# calculations
def next_vy(v_, a_):
    return v_ + a_ * t


def next_a(u_, y_):
    return (u_ * e) / (m * y_ * ln)


def next_y(y_, v_, a_):
    return y_ + v_ * t + a_ * t * t / 2


# constants
e = -1.6021766208 * (10 ** -19)
m = 9.1093837015 * (10 ** -31)

# variables
R = 0.27
r = 0.13
vx = 5 * (10 ** 5)
L = 0.35

# new
tmax = L / vx
ln = math.log(R / r)
y, vy, a = 0, 0, 0
ul, ur = 0, 1000
p = 9
t = 10 ** -p

# code
while ur - ul > (10 ** -6):
    um = (ur + ul) / 2
    y = (R + r) / 2
    vy = 0
    a = next_a(um, y)
    departedOY = False
    for i in range(1, int(tmax * (10 ** p))):
        y = next_y(y, vy, a)
        vy = next_vy(vy, a)
        a = next_a(um, y)
        if y < r or y > R:
            departedOY = True
            break

    if departedOY:
        ur = um
    else:
        ul = um

print('{:25s}'.format("Required voltage:"), "%.7f" % ur)
v = (math.sqrt(vx ** 2 + vy ** 2))
print('{:25s}'.format("Speed:"), "%.7f" % v)
print('{:25s}'.format("Time:"), "%.7f" % tmax)

y = (R + r) / 2
vy = 0
a = next_a(ur, y)
totalY = [y]
totalV = [vy]
totalA = [a]
totalT = [0]
totalX = numpy.linspace(0, L, int(tmax * (10 ** p)))

for i in range(1, int(tmax * (10 ** p))):
    y = next_y(y, vy, a)
    vy = next_vy(vy, a)
    a = next_a(ur, y)
    totalY.append(y)
    totalV.append(vy)
    totalA.append(a)
    totalT.append(i * (10 ** -p))

# visualisation with matplotlib
figure = plt.figure(figsize=(15, 15))
plt.subplots_adjust(wspace=0.5, hspace=0.5)

axis1 = figure.add_subplot(2, 2, 1)
axis1.set_title("y(x)")
axis1.set_xlabel("x, m", color='blue')
axis1.set_ylabel("y, m", color='blue')
axis1.plot(totalX, totalY)

axis2 = figure.add_subplot(2, 2, 2)
axis2.set_title("v(t)")
axis2.set_xlabel("t, s", color='blue')
axis2.set_ylabel("v, m/s", color='blue')
axis2.plot(totalT, totalV)

axis3 = figure.add_subplot(2, 2, 3)
axis3.set_title("a(t)")
axis3.set_xlabel("t, s", color='blue')
axis3.set_ylabel("a, m/s^2", color='blue')
axis3.plot(totalT, totalA)

axis4 = figure.add_subplot(2, 2, 4)
axis4.set_title("y(t)")
axis4.set_xlabel("t, s", color='blue')
axis4.set_ylabel("y, m", color='blue')
axis4.plot(totalT, totalY)

plt.show()
