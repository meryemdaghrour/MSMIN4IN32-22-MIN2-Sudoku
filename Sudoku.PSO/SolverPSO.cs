using Sudoku.Shared;

namespace Sudoku.PSO;



public class SolverPSO : ISudokuSolver
{
        public SudokuGrid Solve(SudokuGrid s)
        {   
            int[,] CellsSolver = new int[9,9];
            for(int i=0; i<9; i++){
                for(int j=0; j<9; j++){
                    CellsSolver[i,j] = s.Cells[i][j];
                }
            }

            const int numOrganisms = 200;
            const int maxEpochs = 5000;
            const int maxRestarts = 40;
            Console.WriteLine($"Setting numOrganisms: {numOrganisms}");
            Console.WriteLine($"Setting maxEpochs: {maxEpochs}");
            Console.WriteLine($"Setting maxRestarts: {maxRestarts}");

            var solver = new SudokuSolver();
            var sudoku = new Sudoku(CellsSolver);
            var solvedSudoku = solver.Solve(sudoku, numOrganisms, maxEpochs, maxRestarts);

            // ToStringPSO(solvedSudoku);

            for(int i=0; i<9; i++){
                for(int j=0; j<9; j++){
                    s.Cells[i][j] = solvedSudoku.CellValues[i,j];
                }
            }
            return s;
        }

        // public void ToStringPSO(Sudoku s){
        //     Console.Write("Sudoku be like :");
        //     int[,] CellsSolver = new int[9,9];
        //     for(int i=0; i<9; i++){
        //         Console.Write("\r\n ");
        //         for(int j=0; j<9; j++){
        //             Console.Write(s.CellValues[i,j].ToString());
        //         }
        //     }
        // }
}

