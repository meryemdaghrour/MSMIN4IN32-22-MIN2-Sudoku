using Sudoku.Shared;
using System;
using System.Text;

namespace Sudoku.DlxLib
{
    class DlxLibSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            DlxSudokuSolver solver = new DlxSudokuSolver();
            solver.sudoku = s;
            solver.matrixBuilder();
            solver.solve();
            return solver.sudoku;
        }
    }
}