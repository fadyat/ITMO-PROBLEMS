import matplotlib.pyplot as plt
import numpy


class FoucaultPendulum:
    _lst_x = numpy.array([])
    _lst_y = numpy.array([])
    _g = 9.832

    def __init__(self, x, y, vx=0.0, vy=0.0, w=0.04, l=100, dt=0.01):
        self._x, self._y = x, y
        self._vx, self._vy = vx, vy
        self._w, self._l, self._dt = w, l, dt

    def update(self):
        self._vx += (2 * self._vy * self._w + self._w ** 2 * self._x
                     - self._g * self._x / self._l) * self._dt
        self._vy += (-2 * self._vx * self._w + self._w ** 2 * self._y
                     - self._g * self._y / self._l) * self._dt
        self._x += self._vx * self._dt
        self._y += self._vy * self._dt
        self._lst_x = numpy.append(self._lst_x, self._x)
        self._lst_y = numpy.append(self._lst_y, self._y)

    def draw(self, rep=10000):
        for _ in range(rep):
            self.update()

        plt.scatter(self._lst_x, self._lst_y, s=0.15, c='black')
        plt.show()


pendulum = FoucaultPendulum(x=1, y=1, vx=10)
pendulum.draw()

