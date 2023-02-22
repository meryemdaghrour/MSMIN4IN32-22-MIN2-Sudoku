using Sudoku.Shared;
using SudokuSolver;
using System.Diagnostics;

namespace DSaturProject
{
    public class GraphColorationSolverDS : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            int[,] sudoku=new int[9,9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (s.Cells[i][j] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        sudoku[i, j] = s.Cells[i][j];
                    }
                }
            }

            Console.WriteLine(sudoku);
            Console.WriteLine("test");
            var stopWatch = Stopwatch.StartNew();

            //Solving
            SudokuSolver solver = new SudokuSolver(sudoku);
            int[,] solved = solver.StartSolving();

            stopWatch.Stop();


            Console.WriteLine("Elapsed Time: {0}", stopWatch.ElapsedMilliseconds / 1000.0);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    s.Cells[i][j] = (int)solved[i, j];
                }
            }

            return s.CloneSudoku();

            

        }
    }
}