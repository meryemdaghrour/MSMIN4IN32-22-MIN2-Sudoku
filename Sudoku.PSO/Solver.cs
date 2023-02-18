using Sudoku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.PSO
{
    public class PSOSolver : ISudokuSolver
    {
        private int populationSize;
        private double[,] particles;
        private double[,] velocities;
        private double[] particleFitness;

        private Random rand = new Random();

        public SudokuGrid Solve(SudokuGrid s)
        {
            // Convert initial Sudoku grid to particle
            double[] initialParticle = ConvertToVector(s);
            int numVariables = initialParticle.Length;

            // Initialize particles and velocities
            populationSize = 100;
            particles = new double[populationSize, numVariables];
            velocities = new double[populationSize, numVariables];
            particleFitness = new double[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    particles[i, j] = rand.Next(1, 10);
                    velocities[i, j] = rand.NextDouble() * 2 - 1;
                }
                particleFitness[i] = double.PositiveInfinity;
            }

            // Run optimization loop
            double c1 = 1.5;
            double c2 = 1.5;
            double w = 0.7;
            int maxIterations = 1000;
            double[] globalBest = new double[numVariables];
            double globalBestFitness = double.PositiveInfinity;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // Evaluate particle fitness in parallel
                Parallel.For(0, populationSize, i =>
                {
                    double[,] matrix = ConvertToMatrix(particles, i);
                    particleFitness[i] = EvaluateMatrix(matrix);
                });

                // Update global best
                int bestIndex = GetBestParticleIndex();
                if (particleFitness[bestIndex] < globalBestFitness)
                {
                    globalBest = ConvertToVector(particles, bestIndex);
                    globalBestFitness = particleFitness[bestIndex];
                }

                // Update particle velocities and positions
                for (int i = 0; i < populationSize; i++)
                {
                    for (int j = 0; j < numVariables; j++)
                    {
                        double r1 = rand.NextDouble();
                        double r2 = rand.NextDouble();
                        velocities[i, j] = w * velocities[i, j] +
                            c1 * r1 * (GetBestParticleValue(j) - particles[i, j]) +
                            c2 * r2 * (GetBestNeighborhoodValue(i, j) - particles[i, j]);
                        particles[i, j] += velocities[i, j];

                        // Clamp particle position to valid range
                        if (particles[i, j] < 1)
                        {
                            particles[i, j] = 1;
                            velocities[i, j] = 0;
                        }
                        else if (particles[i, j] > 9)
                        {
                            particles[i, j] = 9;
                            velocities[i, j] = 0;
                        }
                    }
                }

                // Update w for next iteration
                w *= 0.99;
            }

            return ConvertToSudokuGrid(globalBest);
        }

        private int GetBestParticleIndex()
        {
            int bestIndex = 0;
            double bestFitness = particleFitness[0];

            // Loop through all particles and find the one with the best fitness
            for (int i = 1; i < populationSize; i++)
            {
                if (particleFitness[i] < bestFitness)
                {
                    bestFitness = particleFitness[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        private double GetBestParticleValue(int index)
        {
            int bestIndex = GetBestParticleIndex();
            return particles[bestIndex, index];
        }

        private double GetBestNeighborhoodValue(int particleIndex, int variableIndex)
        {
            int neighborhoodSize = 5;
            int startIndex = Math.Max(0, particleIndex - neighborhoodSize / 2);
            int endIndex = Math.Min(populationSize - 1, particleIndex + neighborhoodSize / 2);

            double bestValue = particles[startIndex, variableIndex];
            double bestFitness = particleFitness[startIndex];

            for (int i = startIndex + 1; i <= endIndex; i++)
            {
                if (particleFitness[i] < bestFitness)
                {
                    bestFitness = particleFitness[i];
                    bestValue = particles[i, variableIndex];
                }
            }

            return bestValue;
        }

        private double[] ConvertToVector(double[,] matrix, int rowIndex)
        {
            int numVariables = matrix.GetLength(1);
            double[] vector = new double[numVariables];

            for (int i = 0; i < numVariables; i++)
            {
                vector[i] = matrix[rowIndex, i];
            }

            return vector;
        }

        private double[] ConvertToVector(SudokuGrid s)
        {
            double[] vector = new double[81];
            int index = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int value = s.GetCell(i, j);
                    vector[index] = value;
                    index++;
                }
            }

            return vector;
        }

        private double[,] ConvertToMatrix(double[] vector)
        {
            double[,] matrix = new double[9, 9];
            int index = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    matrix[i, j] = vector[index];
                    index++;
                }
            }

            return matrix;
        }

        private double[,] ConvertToMatrix(double[,] particleMatrix, int rowIndex)
        {
            double[] particle = ConvertToVector(particleMatrix, rowIndex);
            double[,] matrix = ConvertToMatrix(particle);
            return matrix;
        }

        private SudokuGrid ConvertToSudokuGrid(double[] vector)
        {
            SudokuGrid s = new SudokuGrid();

            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int value = (int)Math.Round(vector[index]);
                    s.SetCell(i, j, value);
                    index++;
                }
            }

            return s;
        }

        private double EvaluateMatrix(double[,] matrix)
        {
            SudokuGrid s = ConvertToSudokuGrid(matrix);
            int numErrors = s.CountErrors();

            if (numErrors == 0)
            {
                return 0;
            }
            else
            {
                return 1.0 / numErrors;
            }
        }
    }
}