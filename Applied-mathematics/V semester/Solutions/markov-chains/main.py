import typing

import networkx as nx
import numpy as np
from matplotlib import pyplot as plt

ergodic_markov_chain = np.array([
    np.array([0.2, 0, 0.4, 0.1, 0, 0, 0.15, 0.15], dtype=float),
    np.array([0, 0.1, 0, 0.2, 0.5, 0, 0.2, 0], dtype=float),
    np.array([0.1, 0, 0.1, 0, 0, 0.4, 0.3, 0.1], dtype=float),
    np.array([0, 0.1, 0, 0.3, 0, 0, 0.3, 0.3], dtype=float),
    np.array([0.4, 0, 0.2, 0.2, 0.1, 0.1, 0, 0], dtype=float),
    np.array([0, 0.3, 0.15, 0, 0.4, 0.1, 0, 0.05], dtype=float),
    np.array([0.1, 0.2, 0.05, 0.1, 0, 0.2, 0.05, 0.3], dtype=float),
    np.array([0.2, 0.3, 0.1, 0.1, 0, 0.2, 0, 0.1], dtype=float),
])

initial_states = np.array([
    np.array([1, 0, 0, 0, 0, 0, 0, 0], dtype=float),
    np.array([0, 1, 0, 0, 0, 0, 0, 0], dtype=float),
])

different_steps = np.array([5, 10, 50])


def show_chain(
    chain: np.ndarray[np.ndarray[float]],
    vx_names: np.ndarray[str],
):
    g = nx.DiGraph()
    for i, row in enumerate(chain):
        for j, val in enumerate(row):
            if val <= 0:
                continue

            g.add_edge(vx_names[i], vx_names[j], weight=val)

    pos = nx.circular_layout(g)
    nx.draw_networkx_nodes(g, pos, node_size=200, node_color='black')
    nx.draw_networkx_labels(g, pos, font_size=7, font_color='white')
    nx.draw_networkx_edges(g, pos, width=1, edge_color='black')
    nx.draw_networkx_edge_labels(g, pos, nx.get_edge_attributes(g, 'weight'), font_size=6)
    plt.show()


def state_dist_numeric(
    chain: np.ndarray[np.ndarray[float]],
    initial_state: np.ndarray[float],
    eps: float = 1e-6,
    steps: int = 1000,
) -> typing.Tuple[np.ndarray[float], np.ndarray[float]]:
    std = np.array([np.std(initial_state)])
    while steps > 0:
        initial_state = np.matmul(initial_state, chain)
        cur_std = np.std(initial_state)
        if np.abs(std[-1] - cur_std) < eps:
            break

        std = np.append(std, cur_std)
        steps -= 1

    return initial_state, std


def plot_std(
    eps: float = 1e-6,
    *args,
):
    for std in args:
        plt.plot(np.arange(std.shape[0]), std)

    plt.xlabel('Steps')
    plt.ylabel('Std')
    plt.title(f'Std of state distribution. Eps: {eps}')
    plt.show()


def state_dist_analytic(
    chain: np.ndarray[np.ndarray[float]],
):
    chain = chain - np.eye(chain.shape[0])
    chain[-1] = np.ones(chain.shape[0])
    b = np.zeros(chain.shape[0])
    b[-1] = 1
    return np.linalg.solve(chain, b)


if __name__ == '__main__':
    np.set_printoptions(precision=3, suppress=True)
    # show_chain(ergodic_markov_chain, np.array(['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H']))
    stds = []
    print('Numeric:')
    for step in different_steps:
        for initial_state in initial_states:
            state, std = state_dist_numeric(
                ergodic_markov_chain,
                initial_state,
                steps=step,
            )
            print(
                f'State distribution after {step} steps: {state},'
                f' for initial state: {initial_state}'
            )
            stds.append(std)

    plot_std(1e-6, *stds)
    print(f'\nAnalytic:\n{state_dist_analytic(ergodic_markov_chain)}')
