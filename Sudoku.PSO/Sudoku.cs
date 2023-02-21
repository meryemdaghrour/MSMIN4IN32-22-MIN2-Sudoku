using System.Text;

namespace Sudoku.PSO
{
  public class Sudoku
  {
    public Sudoku(int[,] cellValues)
    {
      CellValues = cellValues;
    }

    public static Sudoku New(int[,] cellValues)
    {
      return new Sudoku(MatrixHelper.DuplicateMatrix(cellValues));
    }

    public static Sudoku New(Sudoku sudoku)
    {
      return new Sudoku(MatrixHelper.DuplicateMatrix(sudoku.CellValues));
    }

    public int[,] CellValues { get; }

    public int Error
    {
      get
      {
        return CountErrors(true) + CountErrors(false);

        int CountErrors(bool countByRow)
        {
          var errors = 0;
          for (var i = 0; i < MatrixHelper.SIZE; ++i)
          {
            var counts = new int[MatrixHelper.SIZE];
            for (var j = 0; j < MatrixHelper.SIZE; ++j)
            {
              var cellValue = countByRow ? CellValues[i, j] : CellValues[j, i];
              ++counts[cellValue - 1];
            }

            for (var k = 0; k < MatrixHelper.SIZE; ++k)
            {
              if (counts[k] == 0)
                ++errors;
            }
          }
          return errors;
        }
      }
    }
  }
}