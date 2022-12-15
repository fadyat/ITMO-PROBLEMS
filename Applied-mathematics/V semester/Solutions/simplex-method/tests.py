import numpy as np
import pytest
import scipy

from main import SimplexMethodInput


@pytest.mark.parametrize(
    "test_input, expected",
    [
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-6 -1 -4 5",
                constraints=[
                    "3 1 -1 1 = 4",
                    "5 1 1 -1 = 4",
                ],
            ),
            -4,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 -2 -1 3 -1",
                constraints=[
                    "1 1 0 2 1 = 5",
                    "1 1 1 3 2 = 9",
                    "0 1 1 2 1 = 6",
                ]
            ),
            -11,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 -2 -3 1",
                constraints=[
                    "1 -3 -1 -2 = -4",
                    "1 -1 1 0 = 0",
                ],
            ),
            -6,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 -1 -1 1 -1",
                constraints=[
                    "1 1 2 0 0 = 4",
                    "0 -2 -2 1 -1 = -6",
                    "1 -1 6 1 1 = 12",
                ],
            ),
            -10,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 4 -3 10",
                constraints=[
                    "1 1 -1 -10 = 0",
                    "1 14 10 -10 = 11"
                ],
            ),
            -4,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 -1 1 -1 2",
                constraints=[
                    "3 1 1 1 -2 = 10",
                    "6 1 2 3 -4 = 20",
                    "10 1 3 6 -7 = 30",
                ],
            ),
            10,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-1 5 1 -1",
                constraints=[
                    "1 3 3 1 <= 3",
                    "2 0 3 -1 <= 4"
                ],
            ),
            -3,
        ),
        (
            SimplexMethodInput(
                extremum_type="min",
                obj_func="-3 -2",
                constraints=[
                    "1 2 <= 7",
                    "2 1 <= 8",
                    "1 0 <= 3",
                ],
            ),
            -13
        ),
        (
            SimplexMethodInput(
                extremum_type="max",
                obj_func="40 30",
                constraints=[
                    "1 2 <= 16",
                    "1 1 <= 9",
                    "3 2 <= 24",
                ],
            ),
            330,
        ),
        (
            SimplexMethodInput(
                extremum_type="max",
                obj_func="40 50",
                constraints=[
                    "24 8 <= 600",
                    "8 8 <= 480",
                    "3 8 <= 240",
                ],
            ),
            1864.3,
        )
    ],
)
def test_scipy_result(
    test_input: SimplexMethodInput,
    expected: float,
):
    a_eq, b_eq, a_ub, b_ub = parse_input_for_scipy(test_input)
    c = np.array(test_input.obj_func.split(), dtype=float)

    is_max = test_input.extremum_type == "max"
    if is_max:
        c *= -1

    result = scipy.optimize.linprog(c, A_ub=a_ub, b_ub=b_ub, A_eq=a_eq, b_eq=b_eq, method="simplex")
    fun = result.fun * (-1 if is_max else 1)
    assert pytest.approx(fun, abs=1e-1) == expected


def parse_input_for_scipy(
    test_input: SimplexMethodInput,
):
    a_eq, b_eq = [], []
    a_ub, b_ub = [], []
    for constraint in test_input.constraints:
        *coefficients, sign, value = constraint.split()
        coefficients = np.array(coefficients, dtype=float)
        value = np.array([value], dtype=float)
        if sign == "=":
            a_eq.append(coefficients)
            b_eq.append(value)
            continue

        if sign == ">=":
            coefficients *= -1
            value *= -1

        a_ub.append(coefficients)
        b_ub.append(value)

    return a_eq or None, b_eq or None, a_ub or None, b_ub or None
