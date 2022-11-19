import numpy as np
import pytest

from main import simplex_method


@pytest.mark.parametrize(
    "extremum_type, objective_function, constraints, constraints_values, signs, expected",
    [
        (
            "min",
            np.array([-6, -1, -4, 5]),
            np.array([
                [3, 1, -1, 1],
                [5, 1, 1, -1],
            ]),
            np.array([4, 4]),
            np.array(["==", "=="]),
            -4,
        ),
        (
            "min",
            np.array([-1, -2, -3, 1]),
            np.array([
                [1, -3, -1, -2],
                [1, -1, 1, 0],
            ]),
            np.array([-4, 0]),
            np.array(["==", "=="]),
            ...  # todo
        ),
        (
            "min",
            np.array([-1, -2, -1, 3, -1]),
            np.array([
                [1, 1, 0, 2, 1],
                [1, 1, 1, 3, 2],
            ]),
            np.array([5, 9]),
            np.array(["==", "=="]),
            ...  # todo
        ),
        (
            "min",
            np.array([-1, -1, -1, 1, -1]),
            np.array([
                [1, 1, 2, 0, 0],
                [0, -2, -2, 1, -1],
            ]),
            np.array([4, -6]),
            np.array(["==", "=="]),
            ...  # todo
        ),
        (
            "min",
            np.array([-1, 4, -3, 10]),
            np.array([
                [1, 1, -1, -10],
                [1, 14, 10, -10],
            ]),
            np.array([0, 11]),
            np.array(["==", "=="]),
            ...  # todo
        ),
        (
            "min",
            np.array([-1, 5, 1, -1]),
            np.array([
                [1, 3, 3, 1],
                [2, 0, 3, -1],
            ]),
            np.array([3, 4]),
            np.array(["<=", "<="]),
            -3,  # 0 0 0 3
        ),
        (
            "min",
            np.array([-1, -1, 1, -1, 2]),
            np.array([
                [3, 1, 1, 1, -2],
                [6, 1, 2, 3, -4],
                [10, 1, 3, 6, -7]
            ]),
            np.array([10, 20, 30]),
            np.array(["==", "==", "=="]),
            10,  # 0 0 10 0 0
        ),
    ],
)
def test_simplex_method(
    extremum_type: str,
    objective_function: np.ndarray[float],
    constraints: np.ndarray[np.ndarray[float]],
    constraints_values: np.ndarray[float],
    signs: np.ndarray[str],
    expected: np.ndarray[float],
):
    if expected == ...:
        pytest.skip("todo")

    result = simplex_method(
        extremum_type,
        objective_function,
        constraints,
        constraints_values,
        signs,
    )
    assert int(result[-1]) == int(expected)


@pytest.mark.parametrize(
    "extremum_type, objective_function, constraints, constraints_values, signs, expected",
    [
        (
            "max",
            np.array([40, 30]),
            np.array([
                [1, 2],
                [1, 1],
                [3, 2],
            ]),
            np.array([16, 9, 24]),
            np.array(["<=", "<=", "<="]),
            330,  # 6 3
        ),
        (
            "max",
            np.array([40, 50]),
            np.array([
                [24, 8],
                [8, 8],
                [3, 8],
            ]),
            np.array([600, 480, 240]),
            np.array(["<=", "<=", "<="]),
            1864.1
        )
    ],
)
def test_simplex_method_custom(
    extremum_type: str,
    objective_function: np.ndarray[float],
    constraints: np.ndarray[np.ndarray[float]],
    constraints_values: np.ndarray[float],
    signs: np.ndarray[str],
    expected: np.ndarray[float],
):
    # Those tests were taken from some websites with examples of simplex method
    result = simplex_method(
        extremum_type,
        objective_function,
        constraints,
        constraints_values,
        signs,
    )
    assert int(result[-1]) == int(expected)
