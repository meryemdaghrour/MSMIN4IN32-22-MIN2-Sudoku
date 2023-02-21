using Sudoku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver;
using System.Diagnostics;


namespace Sodoku.GraphColoring
{
    public class ColoratSolionSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var puzzle = new SudokuPuzzle();
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (s.Cells[i - 1][j - 1] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        puzzle[i, j] = s.Cells[i - 1][j - 1];
                    }
                }
            }
            Console.WriteLine(puzzle);
            var stopWatch = Stopwatch.StartNew();
            var result = puzzle.Solve();
            stopWatch.Stop();

            if (result)
                Console.WriteLine(puzzle);
            else { 
                Console.WriteLine("Puzzle couldn't be solved");
                return s.CloneSudoku();
            }

            Console.WriteLine("Elapsed Time: {0}", stopWatch.ElapsedMilliseconds / 1000.0);

            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    s.Cells[i - 1][j - 1] = (int)puzzle[i, j];
                }
            }

            return s.CloneSudoku();
        }
    }

}