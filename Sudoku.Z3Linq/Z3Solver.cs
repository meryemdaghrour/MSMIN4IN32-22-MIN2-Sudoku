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

            //reprendre ici la partie résolution de sudoku de program.cs dans demo2

            return s.CloneSudoku();

        }


		//Reprendre ici les 2 méthodes creates theorem de SudokuAsArray en utilisant  SudokuGridCells[i,j] et plutôt que SudokuAsArray.Cells[]




	}
}