using System;
using System.Collections.Generic;

using Sudoku.Shared;
using Sudoku.Z3;

namespace Sudoku.Z3
{
    public class Z3Solver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {



            return s.CloneSudoku();

        }
    

    }
}