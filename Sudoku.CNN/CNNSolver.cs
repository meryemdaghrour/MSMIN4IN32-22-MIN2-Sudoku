using Sudoku.Shared;

namespace Sudoku.CNN
{
    public class CNNSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}