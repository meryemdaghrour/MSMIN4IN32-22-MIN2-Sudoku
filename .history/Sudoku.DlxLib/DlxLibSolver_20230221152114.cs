using Sudoku.Shared;
using System;
using System.Text;

namespace SudokuIA.DlxLib
{
    class DlxLibSolver : ISudokuSolver
    {
        public Sudoku Sudoku { get; set; }
        public string Name { get; }

        private DlxSudokuSolver s = new DlxSudokuSolver(); 
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