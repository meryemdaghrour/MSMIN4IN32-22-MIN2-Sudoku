Bienvenue sur le dépôt du groupe MIN2 composé par Guillaume BODARD, Maxence DUMONTIER et Clément GANDILLON

Sujet J: Coloration de graphe


## Contexte
L'objectif a été de résoudre un sudoku à l'aide d'une intelligence artificielle. En effet, nous avions disposition plusieurs méthodes pour notre problème. Nous pouvons citer les algorithmes génétiques, les résolution "à la Norvig" ou bien à l'aide d'un réseau de neurones.  Nous avons opter la coloration de graphe.

## Coloration de graphe
La coloration de graphe est une théorie qui qui consiste à attribuer une couleur à chaque sommet d'un graphe de telle sorte que deux sommets adjacents n'aient jamais la même couleur. Un graphe est composé de nœuds (sommet) qui représentent les éléments, ainsi que des liaisons qui relient les Noeuds entre eux. Chaque noeud doit être colorié de sorte à ce que deux nœuds adjacents (liés) n'aient pas la même couleur. Cette théorie est utilisé en géographie ainsi que pour les plannings. L'objectif est donc d'avoir un nombre de couleur minimal (nombre chromatique).

Dans notre cas, nous cherchons à utiliser 9 couleurs (une couleur pour chaque chiffre). Chaque chiffre est lié aux chiffres de sa colonne, de sa ligne et de son bloc. Nous pouvons appliquer différents algorithmes pour sa résolution.

## Algorithmes
Dans un premier temps, nous avons implémenté un algorithme assez simple de résolution par coloration de graphe en C#. Celui-ci résout les sudokus les plus faciles. Pour son incorporation dans le benchmark, nous avons adapté le format du sudoku d'entrée de type SudokuGrid afin qu'il corresponde au format d'entrée de notre algorithme (Array 9x9). Pour cela, une double boucle for est utilisé dans notre solver "ColorationGraphes".
Voici les liens de l'article et du dépôt sur lesquels nous nous sommes appuyés:
- [Dépot](https://github.com/MostafaEissa/SudokuSolver)
- [Article](https://www.codeproject.com/Articles/801268/A-Sudoku-Solver-using-Graph-Coloring)

Une fois que nous avons vu les limites de celui-ci, nous avons décidé d'implémenter une autre solution en C# basé sur le degré de saturation. Cette solution était tout d'abord graphique. Dans une première partie, nous avons retiré la partie graphique du solver et conservé uniquementles méthodes utiles à la résolution de sudoku. Une nouvelle classe solver contenant les méthodes participant à la résolution a été créée. Comme pour le premier solver, nous avons du adapter le format du sudoku à l'aide du même procédé. Nous avons pu constater que celui-ci était plus performant que notre premier solver. Il est plus rapide et permet de résoudre des sudokus plus complexe.
Voici le lien du dépôt sur lequel nous nous sommes appuyés:
- [Dépot](https://github.com/radoman1996/DSaturProject/blob/master/DSaturProject/DSatur.cs)

## Comparaison des résultats
Pour prouver l'effecacité de nos algorithmes, nous les avons comparer aux autres travaux. Nous avons pu constater que le deuxième a fini 3ème lors des phases de comparaisons dans la résolution des sudokus faciles mais que le premier lui a fini 13ème. Nous n'avons pu réaliser le même test pour les niveaux supérieurs car les algorithmes ne pouvaient donner de solution à tous les sudokus.
Nous avons pu comparer le solver utilisant le degré de saturation à d'autres solver du groupe de TD sur certains sudokus de niveau Moyen, prouvant son efficacité.

