===========================================
Solveur par réseau de neurones convolutifs
===========================================

Ce solveur utilise un réseau de neurones convolutif pour résoudre une grille
de Sudoku.

Il est implémenté en Python, qui est exécuté par Pythonnet depuis le code C#.
La bibliothèque *Tensorflow* est utilisée aux fins du modèle.

Architecture
-------------

Ressources incluses
--------------------

::

	CNNSolver.cs
	README.rst
	Resources/
	├ solver.py
	└ sudoku-model.h5

Dépendances
------------

* `Tensorflow <https://www.tensorflow.org/>`_
* `Pythonnet <https://pythonnet.github.io/>`_

Références
-----------

Ce solveur est dérivé de celui créé par  Shiva Verma, disponible sur
`Github <https://github.com/shivaverma/Sudoku-Solver>`_. Son approche est
décrite dans un article nommé « `Solving Sudoku with Convolution Neural Network
| Keras
<https://towardsdatascience.com/solving-sudoku-with-convolution-neural-network-keras-655ba4be3b11>`_ ».

Le jeu de données utilisé « 1 million Sudoku games » est disponible sur
`Kaggle <https://www.kaggle.com/datasets/bryanpark/sudoku>`_.
