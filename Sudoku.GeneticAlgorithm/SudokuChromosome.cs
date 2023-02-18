using GeneticSharp;
using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithmSolver
{
    public abstract class SudokuChromosome : ChromosomeBase
    {
        private SudokuGrid _gridToSolve;

        protected SudokuChromosome(SudokuGrid gridToSolve, int length) : base(length)
        {
            gridToSolve = _gridToSolve;
            CreateGenes();
        }
    }
}
