import math
import matplotlib.pyplot as plt

t = 0
mE = 5.972e24
R = 6371 * 10e3
g0 = 6.67e-11 * mE / (R ** 2)

theta = 15
rocketM = 25
fuelM = 35
u = 250
alpha = 20

x = 0
y = 1
v = 0
h = 0.1
vx, vy = 0, 0

plot_m, plot_v, plot_x, plot_y, plot_vx, plot_vy = [], [], [], [], [], []

totalM = fuelM + rocketM

while y >= 0:
    g = g0 * R * R / ((R + y) * (R + y))

    if totalM - rocketM > 0:
        vx += math.sin(theta * math.pi / 180) * u * alpha * h / (totalM - alpha * h)
        vy += math.cos(theta * math.pi / 180) * u * alpha * h / (totalM - alpha * h)
        totalM -= alpha * h

    vy -= g * h

    if vx != 0:
        theta = math.atan2(vy, -vx) * 180 / math.pi - 90

    else:
        if vy >= 0:
            theta = 0
        else:
            theta = 180

    x += vx * h
    y += vy * h

    plot_m.append(totalM - rocketM)
    plot_v.append(math.sqrt(vx ** 2 + vy ** 2))
    plot_x.append(x)
    plot_y.append(y)
    plot_vx.append(vx)
    plot_vy.append(vy)
    t += 1

time = [_ for _ in range(t)]

plt.plot(plot_x, plot_y)
plt.title('x', fontsize=11)
plt.xlabel("x")
plt.ylabel("y")
plt.show()

plt.plot(time, plot_vx)
plt.title('vx', fontsize=11)
plt.xlabel("t")
plt.ylabel("vx")
plt.show()

plt.plot(time, plot_vy)
plt.title('vy', fontsize=11)
plt.xlabel("t")
plt.ylabel("vy")
plt.show()

plt.plot(time, plot_v)
plt.title('|v|', fontsize=11)
plt.xlabel("t")
plt.ylabel("v(t)")
plt.show()

plt.plot(time, plot_m)
plt.title('m(t)', fontsize=11)
plt.xlabel("t")
plt.ylabel("m(t)")
plt.show()
