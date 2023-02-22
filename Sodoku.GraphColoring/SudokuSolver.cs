using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSaturProject
{
    class SudokuSolver
    {

        String fileName = "";
        KeyValuePair<int, KeyValuePair<int[,], LinkedList<int>[]>> data = new KeyValuePair<int, KeyValuePair<int[,], LinkedList<int>[]>>();
        LinkedList<int>[] adjacentVerticesArray;
        int[,] fullData;
        int[,] sudoku;



        #region SUDOKU EXAMPLE
        int[,] sudoku1 = {
                {0, 0, 0, 0, 0, 0, 5, 0, 0 },
                {3, 0, 2, 0, 7, 0, 9, 1, 0 },
                {6, 0, 0, 9, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 2, 6 },
                {0, 2, 0, 3, 0, 0, 1, 5, 9 },
                {7, 9, 0, 6, 0, 5, 0, 8, 0 },
                {1, 0, 9, 7, 0, 0, 0, 0, 0 },
                {4, 5, 0, 0, 0, 0, 2, 3, 0 },
                {0, 3, 8, 4, 5, 0, 6, 0, 0 }
            };
        int[,] sudoku2 = {
                {0, 7, 0, 0, 0, 0, 0, 5, 0 },
                {0, 9, 3, 0, 0, 0, 1, 6, 0 },
                {1, 0, 5, 0, 7, 0, 4, 0, 3 },
                {9, 0, 0, 2, 1, 6, 0, 0, 4 },
                {4, 0, 0, 3, 0, 9, 0, 0, 1 },
                {0, 1, 0, 8, 0, 7, 0, 2, 0 },
                {0, 0, 8, 0, 0, 0, 5, 0, 0 },
                {0, 6, 1, 0, 9, 0, 7, 8, 0 },
                {0, 4, 0, 0, 8, 0, 0, 1, 0 }
            };
        int[,] sudoku3 = {
                {0, 0, 5, 0, 0, 0, 1, 0, 0 },
                {0, 1, 9, 0, 0, 5, 0, 4, 0 },
                {0, 0, 0, 8, 0, 2, 0, 9, 3 },
                {8, 0, 0, 1, 0, 0, 0, 0, 6 },
                {1, 3, 0, 2, 4, 8, 0, 5, 7 },
                {4, 0, 0, 0, 0, 6, 0, 0, 1 },
                {5, 7, 0, 3, 0, 9, 0, 0, 0 },
                {0, 2, 0, 4, 0, 0, 7, 6, 0 },
                {0, 0, 1, 0, 0, 0, 8, 0, 0 }
            };
        int[,] sudoku4 = {
                {0, 0, 0, 0, 8, 9, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 6, 8 },
                {3, 0, 0, 5, 7, 0, 0, 0, 9 },
                {0, 5, 0, 4, 0, 0, 7, 0, 1 },
                {0, 0, 8, 0, 0, 0, 2, 0, 0 },
                {6, 0, 1, 0, 0, 8, 0, 4, 0 },
                {4, 0, 0, 0, 5, 3, 0, 0, 2 },
                {1, 7, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 2, 6, 0, 0, 0, 0 }
            };
        int[,] sudoku5 = {
                {3, 0, 2, 1, 0, 0, 0, 0, 6 },
                {0, 0, 0, 6, 4, 5, 0, 0, 0 },
                {0, 7, 0, 0, 3, 0, 0, 0, 0 },
                {0, 0, 0, 0, 1, 0, 0, 4, 9 },
                {0, 0, 6, 0, 0, 0, 8, 0, 0 },
                {5, 1, 0, 0, 2, 0, 0, 0, 0 },
                {0, 0, 0, 0, 8, 0, 0, 5, 0 },
                {0, 0, 0, 7, 9, 4, 0, 0, 0 },
                {7, 0, 0, 0, 0, 1, 2, 0, 4 }
            };
        #endregion

        public SudokuSolver(int[,] sudoku)
        {
            this.sudoku = sudoku;
        }

        public int[,] StartSolving()
        {

            Console.WriteLine("Lancement solver");
            createData();
            SudokuSolution sudokuSolution = new SudokuSolution(81, this.adjacentVerticesArray, 9, 20, sudoku);
            int[,] tempData = sudokuSolution.solution();
            fullData = createSudoku(tempData);

            return fullData;
        }

        public void createData()
        {
            int[,] matrix = new int[9, 9]; //matrica na osnovu koje se kreiraju susjedi
            LinkedList<int>[] adjacentVertices = new LinkedList<int>[81]; //lista susjeda

            //kreiranje liste listi
            for (int i = 0; i < adjacentVertices.Length; i++)
            {
                adjacentVertices[i] = new LinkedList<int>();
            }

            //popunjavanje matrice
            matrix = fillMatrix();

            //popunjavanje liste listi..za susjede
            adjacentVertices = createAdjacent(matrix, adjacentVertices);

            this.adjacentVerticesArray = adjacentVertices;
        }

        public int[,] fillMatrix()
        {
            int[,] tempMatrix = new int[9, 9];
            int num = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempMatrix[i, j] = num;
                    num++;
                }
            }
            return tempMatrix;
        }

        public LinkedList<int>[] createAdjacent(int[,] matrix, LinkedList<int>[] tempAdjacentList)
        {
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // dodavanje iz reda
                    for (int k = 0; k < 9; k++)
                    {
                        if (k != j)
                        {
                            tempAdjacentList[index].AddLast(matrix[i, k]);
                        }
                    }
                    // dodavanje iz kolone
                    for (int k = 0; k < 9; k++)
                    {
                        if (k != i)
                        {
                            tempAdjacentList[index].AddLast(matrix[k, j]);
                        }
                    }
                    // dodavanje 4 preostala polja iz 3x3
                    if (i % 3 == 0)
                    {
                        if (j % 3 == 0)
                        {
                            tempAdjacentList[index].AddLast(matrix[i + 1, j + 1]);
                            tempAdjacentList[index].AddLast(matrix[i + 1, j + 2]);
                            tempAdjacentList[index].AddLast(matrix[i + 2, j + 1]);
                            tempAdjacentList[index].AddLast(matrix[i + 2, j + 2]);
                        }
                        else
                        {
                            if (j % 3 == 1)
                            {
                                tempAdjacentList[index].AddLast(matrix[i + 1, j - 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 1, j + 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 2, j - 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 2, j + 1]);
                            }
                            else
                            {
                                tempAdjacentList[index].AddLast(matrix[i + 1, j - 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 1, j - 2]);
                                tempAdjacentList[index].AddLast(matrix[i + 2, j - 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 2, j - 2]);
                            }
                        }
                    }
                    else
                    {
                        if (i % 3 == 1)
                        {
                            if (j % 3 == 0)
                            {
                                tempAdjacentList[index].AddLast(matrix[i + 1, j + 1]);
                                tempAdjacentList[index].AddLast(matrix[i + 1, j + 2]);
                                tempAdjacentList[index].AddLast(matrix[i - 1, j + 1]);
                                tempAdjacentList[index].AddLast(matrix[i - 1, j + 2]);
                            }
                            else
                            {
                                if (j % 3 == 1)
                                {
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j + 1]);
                                    tempAdjacentList[index].AddLast(matrix[i + 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i + 1, j + 1]);
                                }
                                else
                                {
                                    tempAdjacentList[index].AddLast(matrix[i + 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i + 1, j - 2]);
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 2]);
                                }
                            }
                        }
                        else
                        {
                            if (j % 3 == 0)
                            {
                                tempAdjacentList[index].AddLast(matrix[i - 1, j + 1]);
                                tempAdjacentList[index].AddLast(matrix[i - 1, j + 2]);
                                tempAdjacentList[index].AddLast(matrix[i - 2, j + 1]);
                                tempAdjacentList[index].AddLast(matrix[i - 2, j + 2]);
                            }
                            else
                            {
                                if (j % 3 == 1)
                                {
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j + 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 2, j + 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 2, j - 1]);
                                }
                                else
                                {
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 1, j - 2]);
                                    tempAdjacentList[index].AddLast(matrix[i - 2, j - 1]);
                                    tempAdjacentList[index].AddLast(matrix[i - 2, j - 2]);
                                }
                            }
                        }
                    }

                    // prelazak na sledeci element
                    index++;
                }
            }


            return tempAdjacentList;
        }

        public int[,] createSudoku(int[,] data)
        {
            int[,] tempData = new int[9, 9];
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempData[i, j] = data[1, index];
                    index++;
                }
            }
            return tempData;
        }

        public void printSolution(int[,] sudoku) {
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    Console.Write(sudoku[i, j] + " ");
                }
                Console.WriteLine();
            }

        }



    }
}
