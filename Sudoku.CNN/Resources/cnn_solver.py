#! /usr/bin/env python3

import copy

import numpy as np
from tensorflow import keras
# from scripts.inference import inference_sudoku, norm
# from scripts.validate_game import validate_solution

# Starting for the Sudoku.Benchmark directory
load_model_location = "../Sudoku.CNN/Resources/sudoku-model.h5"

print(f"Chargement du modèle depuis {load_model_location}")
model = keras.models.load_model(load_model_location)


def norm(a: np.numarray) -> np.numarray:
    return (a / 9) - .5


def denorm(a: np.numarray) -> np.numarray:
    return (a + .5) * 9


def inference_sudoku(model: keras.models.Sequential, sample: np.numarray) -> np.numarray:
    """
        This function solves the sudoku by filling blank positions one by one,
        keeping the highest confidence one from the network’s answer.
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

def solve_sudoku_str(grid: str):
    grid = grid.replace('\n', '')
    grid = grid.replace(' ', '')
    grid = np.array([int(j) for j in grid]).reshape((9, 9, 1))
    grid = norm(grid)
    grid = inference_sudoku(model, grid)
    return grid

def solve_sudoku_np(grid: np.array):
    grid = grid.reshape((9, 9, 1))
    grid = norm(grid)
    grid = inference_sudoku(model, grid)
    return grid


X = [0]*9
for i in range(9):
    X[i] = [0]*9

for i in range(9):
    for j in range(9):
        X[i][j] == instance[i][j]

game = np.array(X).reshape((9, 9, 1))

game = solve_sudoku_np(game)

print("Puzzle résolu :")
print(game)

r = game.tolist()

# validate_solution(game)