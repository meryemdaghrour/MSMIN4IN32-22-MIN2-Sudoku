using Sudoku.Shared;

namespace Sudoku.DemoZ3;
    public class DemoZ3EmptySolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
