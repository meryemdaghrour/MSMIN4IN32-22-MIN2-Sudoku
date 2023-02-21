using Sudoku.Shared;
using System;
using System.Text;

namespace Sudoku.DlxLib
{
    class DlxLibSolver : ISudokuSolver
    {

        private DlxSudokuSolver Solver = new DlxSudokuSolver(); 
        public SudokuGrid Solve(SudokuGrid s)
        {
            Solver.sudoku = s;
            Solver.matrixBuilder();
            Solver.solve();
            return Solver.sudoku;
        }
    }
}