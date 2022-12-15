import dataclasses
import time
import typing

import numpy as np


@dataclasses.dataclass
class SimplexMethodInput:
    extremum_type: str
    obj_func: str
    constraints: typing.List[str]


def to_canonical(inp: SimplexMethodInput):
    """
    Converts the input to canonical form:
    - non-negativity constraints for all variables
    - all remaining constraints are expressed as equalities
    - the right hand side vector b is non-negative

    :return: Inplace conversion.
    """

    c = np.array(inp.obj_func.split(), dtype=float)
    if inp.extremum_type == "min":
        c *= -1

    a = np.array([constraint.split()[:-2] for constraint in inp.constraints], dtype=float)
    b = np.array([constraint.split()[-1] for constraint in inp.constraints], dtype=float)
    s = np.array([constraint.split()[-2] for constraint in inp.constraints], dtype=str)

    for i, sign in enumerate(s):
        if sign == ">=":
            a[i] *= -1
            b[i] *= -1
        elif sign in ["<=", "="]:
            continue

        raise ValueError(f"Unknown sign {sign}")

    a = np.hstack((a, np.eye(a.shape[0])))
    c = np.hstack((c, np.zeros(a.shape[0])))

    for i, value in enumerate(b):
        if value < 0:
            a[i] *= -1
            b[i] *= -1

    return c, a, b


class SimplexMethod:

    def __init__(self, inp: SimplexMethodInput):
        self.input = input
        self.c, self.a, self.b = to_canonical(inp)

    def solve(self):
        table = create_simplex_table(self.c, self.a, self.b)
        print(table)
        while not is_optimal(table):
            table = step(table)
            time.sleep(1)
            print(table)

        res = table[-1, -1]
        if inp.extremum_type == "min":
            res *= -1

        return res


def step(table):
    """
    Performs one step of the simplex method.

    :param table: The simplex table.
    :return: The next simplex table.
    """
    pivot = find_pivot(table)
    print(f"Pivot: {pivot}")
    return pivotize(table, pivot)


def find_pivot(table):
    """
    Finds the pivot element.

    :param table: The simplex table.
    :return: The pivot element.
    """

    pivot_col = np.argmin(np.where(table[-1, :-1] < 0, table[-1, :-1], np.inf))
    pivot_row = np.argmin(table[:-1, -1] / table[:-1, pivot_col])
    return pivot_row, pivot_col


def pivotize(table, pivot):
    """
    Pivotizes the table.

    :param table: The simplex table.
    :param pivot: The pivot element.
    :return: The pivotized table.
    """

    pivot_row, pivot_col = pivot
    pivot_value = table[pivot_row, pivot_col]
    table[pivot_row] /= pivot_value
    for i, row in enumerate(table):
        if i == pivot_row:
            continue
        table[i] -= row[pivot_col] * table[pivot_row]
    return table


def is_optimal(table):
    """
    Checks if the table is optimal.

    :param table: The simplex table.
    :return: True if the table is optimal, False otherwise.
    """

    return np.all(table[-1, :-1] >= 0)


def create_simplex_table(c, a, b):
    """
    Creates a simplex table.

    :param c: The objective function coefficients.
    :param a: The coefficients of the constraints.
    :param b: The right hand side vector.
    :return: The simplex table.
    """

    upd_a = np.hstack((a, np.zeros((a.shape[0], 1))))
    table = np.hstack((upd_a, b.reshape(-1, 1)))
    upd_c = np.hstack((-c, np.array([1, 0])))
    table = np.vstack((table, upd_c))
    return table


def scan(
    filename: str,
) -> SimplexMethodInput:
    """
    Reads a file and returns the contents as a string.

    File format:
    1. Extremum type (min or max)
    2. Objective function parameters separated by spaces, e.g. 1 2 3 for f(x) = x1 + 2x2 + 3x3
    3. Constraints and signs separated by spaces, e.g. 1 2 3 <= 4 for 1x1 + 2x2 + 3x3 <= 4

    :param filename: The name of the file to read.
    :return: Parsed input.
    """

    with open(filename, "r") as file:
        f = file.read()

    extremum, obj_func, *constraints = f.splitlines()
    return SimplexMethodInput(
        extremum_type=extremum,
        obj_func=obj_func,
        constraints=constraints,
    )


if __name__ == "__main__":
    inp = scan("test.txt")
    method = SimplexMethod(inp)
    print(method.solve())
