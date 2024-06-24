#! /usr/bin/env python3

import copy

import numpy as np
import pandas as pd
import tensorflow as tf
from keras.layers import Activation, Conv2D, BatchNormalization, Dense, Flatten, Reshape
from keras.models import Sequential
from tensorflow import keras
from sklearn.model_selection import train_test_split

input_data = "sudoku.csv"
output_location = "sudoku-model.h5"
use_gpu = False  # If you have a discrete GPU to use

if use_gpu:
    physical_devices = tf.config.list_physical_devices("GPU")
    tf.config.experimental.set_memory_growth(physical_devices[0], True)
    print("Using the GPU")


def get_data(file):
    data = pd.read_csv(file)

    feat_raw = data['quizzes']
    label_raw = data['solutions']

    feat = []
    label = []

    for i in feat_raw:
        x = np.array([int(j) for j in i]).reshape((9, 9, 1))
        feat.append(x)

    feat = np.array(feat)
    feat = feat / 9
    feat -= .5

    for i in label_raw:
        x = np.array([int(j) for j in i]).reshape((81, 1)) - 1
        label.append(x)

    label = np.array(label)

    del feat_raw
    del label_raw

    x_train, x_test, y_train, y_test = train_test_split(feat, label, test_size=0.2,
                                                        random_state=42)

    return x_train, x_test, y_train, y_test


def get_model():
    model = keras.models.Sequential()

    model.add(Conv2D(64, kernel_size=(3, 3), activation='relu', padding='same',
                     input_shape=(9, 9, 1)))
    model.add(BatchNormalization())
    model.add(Conv2D(64, kernel_size=(3, 3), activation='relu', padding='same'))
    model.add(BatchNormalization())
    model.add(Conv2D(128, kernel_size=(1, 1), activation='relu', padding='same'))

    model.add(Flatten())
    model.add(Dense(81 * 9))
    model.add(Reshape((-1, 9)))
    model.add(Activation('softmax'))

    return model


def norm(a: np.numarray) -> np.numarray:
    return (a / 9) - .5


def denorm(a: np.numarray) -> np.numarray:
    return (a + .5) * 9


def inference_sudoku(model: Sequential, sample: np.numarray) -> np.numarray:
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


print("Loading data...")
x_train, x_test, y_train, y_test = get_data(input_data)

print("Creating the model...")
model = get_model()

adam = keras.optimizers.Adam(learning_rate=.001)
model.compile(loss='sparse_categorical_crossentropy', optimizer=adam)

model.fit(x_train, y_train, batch_size=32, epochs=2)


# Print a summary of the model's layers
# model.summary(expand_nested=True, show_trainable=True)


def test_accuracy(feats, labels):
    correct = 0

    for i, feat in enumerate(feats):
        print(f"Test {i}\n", end="\t")

        pred = inference_sudoku(model, feat)

        true = labels[i].reshape((9, 9)) + 1

        if abs(true - pred).sum() == 0:
            correct += 1

    print(f"Accuracy: {correct / feats.shape[0]} ({correct} for {feats.shape[0]} tests)")


print("Testing the model")
test_accuracy(x_test[:100], y_test[:100])

print("Saving the model")
model.save(output_location, save_format='h5')
