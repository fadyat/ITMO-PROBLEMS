import numpy as np


def simplex_method(
    extremum_type: str,
    objective_function: np.ndarray[float],
    constraints: np.ndarray[np.ndarray[float]],
    constraints_values: np.ndarray[float],
    signs: np.ndarray[str],
):
    # fixme
    if extremum_type == "min":
        objective_function = -objective_function

    # fixme
    for i, sign in enumerate(signs):
        if sign == ">=":
            constraints[i] = -constraints[i]
            constraints_values[i] = -constraints_values[i]

    # making objective function canonical
    objective_function = -objective_function

    # adding objective function to constraints
    constraints = np.vstack((constraints, objective_function))

    # adding slack variables
    slack_variables = np.eye(len(constraints))
    constraints = np.hstack((constraints, slack_variables))
    constraints_values = np.hstack((constraints_values, 0))

    constraints = np.array([
        np.append(constraints[i], cv)
        for i, cv in enumerate(constraints_values)
    ])

    # fixme
    iter = 0
    while True:
        pivot_column = np.argmin(constraints[-1, :-1])
        if constraints[-1, pivot_column] >= 0:
            break

        pivot_row = np.argmin([
            constraints[i, -1] / constraints[i, pivot_column]
            for i in range(len(constraints) - 1)
            if constraints[i, pivot_column] != 0
        ])

        pivot = constraints[pivot_row, pivot_column]
        constraints[pivot_row] /= pivot

        for i in range(len(constraints)):
            if i == pivot_row:
                continue

            constraints[i] -= constraints[pivot_row] * constraints[i, pivot_column]

        # fixme
        iter += 1
        if iter > 10:
            break

    # fixme
    return constraints[:, -1]
