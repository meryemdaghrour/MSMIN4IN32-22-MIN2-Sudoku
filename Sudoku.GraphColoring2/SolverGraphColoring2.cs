using Sudoku.Shared;

namespace Sudoku.GraphColoring2
{
    public class SolverGraphColoring2 : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}