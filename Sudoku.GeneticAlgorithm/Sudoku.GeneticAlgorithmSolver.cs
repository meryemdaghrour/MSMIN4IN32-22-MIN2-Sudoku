using GeneticSharp;
using GeneticSharp.Extensions;
using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithm
{
    public class GeneticAlgorithmSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            SudokuBoard sudokuToSolve = new SudokuBoard(s);
            var popSize = 1000;
            var maxGeneration = 100;

            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation();
            var fitness = new SudokuFitness(sudokuToSolve);
            var chromosome = new SudokuCellsChromosome(sudokuToSolve);
            var population = new Population(popSize, 10*popSize, chromosome);

            var ga = new GeneticSharp.GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            var genNBTermination = new GenerationNumberTermination(maxGeneration);
            var fitnessThreshTermination = new FitnessThresholdTermination(0);
            ga.Termination = new OrTermination(genNBTermination, fitnessThreshTermination);

            Console.WriteLine("Genetic Algorithm running...");
            ga.Start();

            Console.WriteLine("Best solution found has {0} fitness, final generation is {1}.", ga.BestChromosome.Fitness, ga.GenerationsNumber);

            var chromosomeWinner = (SudokuCellsChromosome) ga.BestChromosome;
            var sudokuWinner = chromosomeWinner.GetSudokus()[0];

            return sudokuWinner.toSudokuGrid();
        }
    }
}
