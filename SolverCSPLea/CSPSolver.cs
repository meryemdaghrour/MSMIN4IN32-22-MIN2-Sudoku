using Sudoku.Shared;

namespace SolverCSPLea
{
    public class CSPSolver : ISudokuSolver
    {
        //test
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
    // bonjour ceci est un test pour tester 
}