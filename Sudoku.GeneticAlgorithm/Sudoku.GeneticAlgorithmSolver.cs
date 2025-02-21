﻿using GeneticSharp;
using GeneticSharp.Extensions;
using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithm
{
    public abstract class GeneticAlgorithmSolverBase : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            SudokuBoard sudokuToSolve = new SudokuBoard(s);
            var popSize = 200;
            var maxStagnantGeneration = 20;

			//Critères de terminaison (0 fitness pour un sudoku résolution, stagnation = effondrement de la diversité génétique)
            var fitnessThreshTermination = new FitnessThresholdTermination(0);
            var stagnantGenerationsTermination = new FitnessStagnationTermination(maxStagnantGeneration);
			var compoundTermination = new OrTermination(stagnantGenerationsTermination, fitnessThreshTermination);

			var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation();
            var fitness = new SudokuFitness(sudokuToSolve);


			//Possibilité de tester plusieurs types de chromosomes
			// Le permutation chromosome devrait permettre de résoudre tous les sudokus easy, mais reste insuffisant pour les plus difficiles
			// Il doit être possible de faire mieux, en essayant d'éviter l'effondrement de la diversité génétique.
			var chromosome = CreateChromosome(sudokuToSolve);

			//Parallélisation de l'évaluation et des opérateurs génétiques
            var gaTaskExecutor = new TplTaskExecutor();
            var gaOperatorsStrategy = new TplOperatorsStrategy();
			
            SudokuChromosomeBase chromosomeWinner = null;
			//Augmentation progressive de la population si aucune solution n'est trouvée
			do
            {
				var population = new Population(popSize, 10 * popSize, chromosome);

				var ga = new GeneticSharp.GeneticAlgorithm(population, fitness, selection, crossover, mutation);
				ga.Termination = compoundTermination;
				ga.TaskExecutor = gaTaskExecutor;
				ga.OperatorsStrategy = gaOperatorsStrategy;

				Console.WriteLine("Genetic Algorithm running...");
				ga.Start();

				Console.WriteLine("Best solution found has {0} fitness, final generation is {1}.", ga.BestChromosome.Fitness, ga.GenerationsNumber);

				chromosomeWinner = (SudokuChromosomeBase)ga.BestChromosome;
				popSize *= 3;

            } while (chromosomeWinner.Fitness < 0);
			var sudokuWinner = chromosomeWinner.GetSudokus()[0];


			return sudokuWinner.toSudokuGrid();
        }


        protected abstract SudokuChromosomeBase CreateChromosome(SudokuBoard s);


    }

	public class GeneticAlgorithmCellsSolver : GeneticAlgorithmSolverBase
	{
		protected override SudokuChromosomeBase CreateChromosome(SudokuBoard s)
		{
			return new SudokuCellsChromosome(s);
		}
	}

	public class GeneticAlgorithmPermutationsSolver : GeneticAlgorithmSolverBase
	{
		protected override SudokuChromosomeBase CreateChromosome(SudokuBoard s)
		{
			return new SudokuPermutationsChromosome(s);
		}
	}



}
