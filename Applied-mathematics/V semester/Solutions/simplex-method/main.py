import json
import typing

import numpy as np


class NoAnswerError(Exception):
    def __init__(self):
        super().__init__('No answer')


class Simplex:
    def __init__(self):
        self.goal: str = "min"
        self.restrictions: typing.List[Restriction] = []
        self.variables: typing.List[Var] = []

    def add_non_original_variable(self):
        self.variables.append(Var(0, False))

    def add_original_variable(self, k: float):
        self.variables.append(Var(k, True))


class Var:
    def __init__(self, k: float, is_original: bool):
        self.k = k
        self.is_original = is_original


class Restriction:
    def __init__(
        self,
        coefficients: typing.List[float],
        answer: float,
        additional_variable_number: int,
    ):
        self.coefficients = coefficients
        self.answer = answer
        self.additional_variable_number = additional_variable_number
        self.has_variable = additional_variable_number != -1

    @staticmethod
    def create_equal_restriction(
        coefficients: typing.List[float],
        answer: float,
    ):
        return Restriction(coefficients, answer, -1)

    @staticmethod
    def create_greater_restriction(
        coefficients: typing.List[float],
        answer: float,
        variable_number: int,
    ):
        coefficients = [-i for i in coefficients] + [1]
        answer = -answer
        return Restriction(coefficients, answer, variable_number)

    @staticmethod
    def create_less_restriction(
        coefficients: typing.List[float],
        answer: float,
        variable_number: int,
    ):
        coefficients.append(1)
        return Restriction(coefficients, answer, variable_number)

    def update_for_variables(self, variables: typing.List[Var]):
        for v in range(len(variables)):
            if (
                not variables[v].is_original and
                self.has_variable and
                self.additional_variable_number != v
            ):
                self.coefficients.insert(v, 0)


class Table:
    def __init__(self, simplex: Simplex):
        self.si = simplex
        self.coefficients = np.array([
            r.coefficients + [r.answer]
            for r in self.si.restrictions
        ], dtype=float)
        self.basis = []
        self.delta = []

    def get_default_basis(self) -> typing.List[int]:
        return [
            -1 if not r.has_variable else r.additional_variable_number
            for r in self.si.restrictions
        ]

    def no_answer_condition(self, idx: int) -> bool:
        return np.max(self.coefficients[:, idx]) <= 0

    def check_optimality(self, idx_worst_argument: int) -> bool:
        if self.si.goal == "min":
            return self.delta[idx_worst_argument] <= 0
        elif self.si.goal == "max":
            return self.delta[idx_worst_argument] >= 0

    def get_index_worst_argument(self) -> int:
        if self.si.goal == "min":
            return int(np.argmax(self.delta[:-1]))
        elif self.si.goal == "max":
            return int(np.argmin(self.delta[:-1]))

    def create_answer(self):
        answer = np.zeros(len(self.si.variables) - 1, dtype=float)
        for bi in range(len(self.basis)):
            answer[self.get_basic_element(bi)] = self.coefficients[bi, -1]

        return [True, answer, self.delta[-1]]

    def calc_delta(self):
        self.delta = []
        for v in range(len(self.si.variables)):
            self.delta.append(-self.si.variables[v].k)
            for r in range(len(self.si.restrictions)):
                self.delta[v] += self.coefficients[r, v] * self.si.variables[self.basis[r]].k

    def get_basic_element(self, idx: int):
        if self.basis[idx] != -1:
            return self.basis[idx]

        self.basis[idx] += 1

        while self.coefficients[idx, self.basis[idx]] == 0:
            self.basis[idx] += 1

        return self.basis[idx]

    def make_new_basic(self, idx: int):
        if self.no_answer_condition(idx):
            raise NoAnswerError()

        i = np.argmax(self.coefficients[:, idx])
        self.basis[i] = idx

    def solve(self):
        self.basis = self.get_default_basis()

        while True:
            for bi in range(len(self.basis)):
                be = self.get_basic_element(bi)
                coef = self.coefficients[bi, be]

                for ei in range(len(self.coefficients[bi, :])):
                    self.coefficients[bi, ei] /= coef

                for bdi in range(len(self.basis)):
                    if bdi == bi:
                        continue

                    coef = self.coefficients[bdi, be]
                    self.coefficients[bdi, :] -= coef * self.coefficients[bi, :]

                self.calc_delta()
                wdi = self.get_index_worst_argument()

                if self.check_optimality(wdi):
                    return self.create_answer()

                try:
                    self.make_new_basic(wdi)
                except NoAnswerError:
                    return [False, None, None]


def from_json(
    path: str,
) -> typing.List[Simplex]:
    with open(path, 'r') as f:
        data = json.load(f)

    return [
        parse_test(test)
        for test in data['tests']
    ]


def parse_test(
    test: typing.Dict[str, typing.Any],
) -> Simplex:
    si = Simplex()
    si.goal = test["goal"]["case"]

    for k in test["goal"]["coefs"]:
        si.add_original_variable(k)

    for r in test['restrictions']:
        c, a, v = r['coefs'], r['answer'], len(si.variables)
        if r['case'] == 'equal':
            si.restrictions.append(Restriction.create_equal_restriction(c, a))
            continue

        if r['case'] == 'greater':
            si.restrictions.append(Restriction.create_greater_restriction(c, a, v))
        elif r['case'] == 'less':
            si.restrictions.append(Restriction.create_less_restriction(c, a, v))

        si.add_non_original_variable()

    for r in range(len(si.restrictions)):
        si.restrictions[r].update_for_variables(si.variables)

    si.add_original_variable(0)
    return si


def solve_from_json(
    path: str,
) -> typing.List[typing.List[typing.Any]]:
    sis = from_json(path)
    return [Table(si).solve() for si in sis]


if __name__ == '__main__':
    np.set_printoptions(precision=3, suppress=True)
    for s in solve_from_json('test.json'):
        print(s)
