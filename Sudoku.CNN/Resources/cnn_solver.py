#! /usr/bin/env python3

import copy

import numpy as np
from tensorflow import keras

# Starting for the Sudoku.Benchmark directory
# load_model_location = "../Sudoku.CNN/Resources/sudoku-model.h5"

print(f"Chargement du modèle depuis {load_model_location}")
model = keras.models.load_model(load_model_location)


def norm(a: np.numarray) -> np.numarray:
    return (a / 9) - .5


def denorm(a: np.numarray) -> np.numarray:
    return (a + .5) * 9


def inference_sudoku(model: keras.models.Sequential, sample: np.numarray) -> np.numarray:
    """
        This function solves the sudoku by filling blank positions one by one,
        keeping the one with highest confidence from the network’s answer.
    """

    feat = copy.copy(sample)

    while 1:

        out = model.predict(feat.reshape((1, 9, 9, 1)))
        out = out.squeeze()

        pred = np.argmax(out, axis=1).reshape((9, 9)) + 1
        prob = np.around(np.max(out, axis=1).reshape((9, 9)), 2)

        feat = denorm(feat).reshape((9, 9))
        mask = (feat == 0)

        if mask.sum() == 0:
            break

        prob_new = prob * mask

        ind = np.argmax(prob_new)
        x, y = (ind // 9), (ind % 9)

        val = pred[x][y]
        feat[x][y] = val
        feat = norm(feat)

    return pred


def solve_sudoku(grid: np.array):
    grid = grid.reshape((9, 9, 1))
    grid = norm(grid)
    grid = inference_sudoku(model, grid)
    return grid


# Import
imported = []
for i in range(9):
    imported.append([])
    for j in range(9):
        imported[i].append(instance[i][j])

game = np.array(imported).reshape((9, 9))

game = solve_sudoku(game)

# Export
r = game.tolist()
