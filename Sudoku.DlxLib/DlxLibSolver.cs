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
            //Call the private matrix builder method from the DlxSudokuSolver class
            
            // typeof(DlxSudokuSolver).GetMethod("matrixBuilder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(Solver, null);
            // typeof.GetMethod("solve", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(Solver, null);

            Solver.matrixBuilder();
            Solver.Solve();
            return Solver.sudoku;
        }
    }
}