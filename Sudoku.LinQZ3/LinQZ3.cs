using Sudoku.Shared;

namespace Sudoku.LinQZ3
{
    public class LinQZ3 : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }

    }

    
}