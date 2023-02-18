using Sudoku.Shared;

namespace Test
{
    public class Z3Solver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {



            return s.CloneSudoku();

        }


    }
}