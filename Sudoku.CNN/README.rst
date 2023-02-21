===========================================
Solveur par réseau de neurones convolutifs
===========================================

Ce solveur utilise un réseau de neurones convolutif pour résoudre une grille
de Sudoku.

Il est implémenté en Python, qui est exécuté par Pythonnet depuis le code C#.
La bibliothèque *Tensorflow* est utilisée aux fins du modèle.

Architecture
-------------

Ce modèle utilise un réseau de neurones convolutifs. Il est composé de neuf
couches, dont trois de convolution.

=============  ==================  ======
Couche         Taille de résultat  Filtre
=============  ==================  ======
Convolution    9×9                  3×3
Normalisation  9×9                  N/A
Convolution    9×9                  3×3
Normalisation  9×9                  N/A
Convolution    9×9                  1×1
Mise à plat    1                    N/A
Densification  1                    N/A
Mise en forme  81                   N/A
Activation     81                   N/A
=============  ==================  ======

Mohamed Ghanem a comparé l'utilisation d'une architecture `U-Net
<https://fr.wikipedia.org/wiki/U-Net>`_ et d'une architecture classique `dans
un projet publié sur Github <https://github.com/Oschart/Neural-Sudoku>`_. Il
conclut que l'architecture classique est plus efficace que l'U-Net pour
résoudre les Sudoku. C'est doc celle que nous utilisons.

Ressources incluses
--------------------

::

	CNNSolver.cs        ← point d'entrée C#
	README.rst          ← ce document
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
