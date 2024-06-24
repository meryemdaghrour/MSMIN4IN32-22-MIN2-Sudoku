using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSaturProject
{
    class SudokuSolution
    {
        #region PODACI

        int[,] dataMatrix = null;
        LinkedList<int>[] adjacentVertices = null;
        HashSet<int> colorSet = new HashSet<int>();
        int maxDeg = -1;
        int[,] matrix = null;

        #endregion

        public SudokuSolution(int nodeNumber, LinkedList<int>[] adjacentVertices, int numberColor, int maxDegree, int[,] matrix)
        {
            this.dataMatrix = new int[3, nodeNumber];
            this.adjacentVertices = adjacentVertices;
            int[] randomCol = randomArray();
            for (int i = 0; i < randomCol.GetLength(0); i++)
            {
                colorSet.Add(randomCol[i]);
            }
            for (int i = 0; i < nodeNumber; i++)
            {
                this.dataMatrix[0, i] = maxDegree;
            }
            this.maxDeg = maxDegree;
            this.matrix = matrix;
            fillColorMatrix(matrix);
        }

        public int[] randomArray()
        {
            Random r = new Random();
            int[] arr = new int[9];
            int k = 0;
            while (k < 9)
            {
                arr[k] = r.Next(1, 10);
                int j = 0;
                while (j < k)
                {
                    if (arr[j] == arr[k])
                    {
                        k--;
                        break;
                    }
                    j++;
                }
                k++;
            }
            return arr;
        }

        public void fillColorMatrix(int[,] matrix)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        int column = i * 9 + j;
                        dataMatrix[1, column] = matrix[i, j];
                    }
                }
            }
        }

        public bool uncolored(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[1, i] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public HashSet<int> setUncolAdj(int[,] arr)
        {
            HashSet<int> set = new HashSet<int>();
            int max = arr[2, 0];
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[2, i] > max)
                {
                    max = arr[2, i];
                }
            }
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[2, i] == max)
                {
                    set.Add(i); //u skup dodajemo index
                }
            }
            return set;
        }

        public void printSolution(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                Console.Write(matrix[1, i] + " ");
                if ((i + 1) % 9 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        public int[,] solution()
        {
            int cpt = 0;
            bool uncol = uncolored(dataMatrix); //ako je true znaci da ima neobojanih..u suprotnom je false
            while (uncol && cpt < 999999)
            {
                cpt++;
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                {
                    if (dataMatrix[1, i] == 0)
                    {
                        HashSet<int> temp = new HashSet<int>();
                        foreach (int w in adjacentVertices[i])
                        {
                            temp.Add(dataMatrix[1, w]);
                        }
                        dataMatrix[2, i] = temp.Count;
                    }
                }
                HashSet<int> setuncoladj = setUncolAdj(dataMatrix);
                int index = -1;
                if (setuncoladj.Count == 1)
                {
                    index = setuncoladj.First();
                }
                else
                {
                    index = setuncoladj.First();
                    int currMax = dataMatrix[0, index];
                    foreach (int id in setuncoladj)
                    {
                        int tempMax = dataMatrix[0, id];
                        if (tempMax > currMax)
                        {
                            index = id;
                        }
                    }
                }
                int control = -1;
                foreach (int col in colorSet)
                {
                    int count = 0;
                    foreach (int adj in adjacentVertices[index])
                    {
                        if (dataMatrix[1, adj] == col)
                        {
                            break;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    if (count == adjacentVertices[index].Count)
                    {
                        dataMatrix[1, index] = col;
                        control = 1;
                        break;
                    }
                }
                if (control == -1)
                {
                    //Console.WriteLine("Impossible à résoudre");
                    colorSet.Clear();
                    int[] randomCol = randomArray();
                    for (int i = 0; i < randomCol.GetLength(0); i++)
                    {
                        colorSet.Add(randomCol[i]);
                    }
                    for (int i = 0; i < 81; i++)
                    {
                        this.dataMatrix[0, i] = 20;
                        this.dataMatrix[1, i] = 0;
                    }
                    fillColorMatrix(this.matrix);
                }
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                {
                    dataMatrix[2, i] = 0;
                }
                uncol = uncolored(dataMatrix);
            }
            return dataMatrix;
        }

    }
}
