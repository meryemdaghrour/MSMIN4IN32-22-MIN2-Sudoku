using Sudoku.Shared;

namespace Norvig
{
    public class SudokuNorvig : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}