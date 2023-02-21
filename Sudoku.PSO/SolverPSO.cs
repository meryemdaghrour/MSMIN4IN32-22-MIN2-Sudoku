using Sudoku.Shared;

namespace Sudoku.PSO;

public class SolverPSO : ISudokuSolver
{
    private Random _rnd;

    public SudokuGrid Solve(SudokuGrid s)
    {
        //initialisation
        const int numOrganisms = 200;
        const int maxEpochs = 5000;
        const int maxRestarts = 20;
        //Affichage des paramètres de base dans la console
        Console.WriteLine($"Setting numOrganisms: {numOrganisms}");
        Console.WriteLine($"Setting maxEpochs: {maxEpochs}");
        Console.WriteLine($"Setting maxRestarts: {maxRestarts}");

        //Convertir un SudokuGrid en Sudoku
        int[,] CellsSolver = new int[9,9];
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                CellsSolver[i,j] = s.Cells[i][j];
            }
        }

        //Création du nouveau Sudoku à partir de SudokuGrid
        var sudoku = new Sudoku(CellsSolver);
        //Résolution
        var solvedSudoku = Solve(sudoku, numOrganisms, maxEpochs, maxRestarts);

        //Convertir un Sudoku en SudokuGrid
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                s.Cells[i][j] = solvedSudoku.CellValues[i,j];
            }
        }

        //Retour d'un Sudoku au format SudokuGrid
        return s;
    }

    public Sudoku Solve(Sudoku sudoku, int numOrganisms, int maxEpochs, int maxRestarts)
    {
        var error = int.MaxValue;
        Sudoku bestSolution = null;
        var attempt = 0;
        while (error != 0 && attempt < maxRestarts)//Continuer temps que le nombre d'essais max n'est pas atteint
        {
            Console.WriteLine($"Attempt: {attempt}");//Affichage du numéro de l'essai dans la console
            _rnd = new Random(attempt);
            bestSolution = SolveInternal(sudoku, numOrganisms, maxEpochs);
            error = bestSolution.Error;
            ++attempt;
        }

        return bestSolution;
    }

    private Sudoku SolveInternal(Sudoku sudoku, int numOrganisms, int maxEpochs)
    {
        //Initialisation
        var numberOfWorkers = (int)(numOrganisms * 0.90);
        var hive = new Organism[numOrganisms];

        var bestError = int.MaxValue;
        Sudoku bestSolution = null;

        for (var i = 0; i < numOrganisms; ++i)//Pour chaque organisme
        {
            var organismType = i < numberOfWorkers
              ? OrganismType.Worker
              : OrganismType.Explorer;

            var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));//Remplis la grille de sudoku de manière aléatoire
            var err = randomSudoku.Error;//Calcule le nombre d'erreure sur cette grille

            hive[i] = new Organism(organismType, randomSudoku.CellValues, err, 0);

            if (err >= bestError) continue;//Si err est supérieur ou égale au meilleur nombre d'erreur continuer sans effectuer les 2 dernières lignes
            bestError = err;
            bestSolution = Sudoku.New(randomSudoku);
        }

        var epoch = 0;
        while (epoch < maxEpochs)//Temps que l'époque est inferieur à l'époque max
        {
            if (epoch % 1000 == 0)//Toutes les 1000 époque afficher la meilleure erreur
                Console.WriteLine($"Epoch: {epoch}, Best error: {bestError}");

            if (bestError == 0)//Si l'erreur = 0 tout arreter
                break;

            for (var i = 0; i < numOrganisms; ++i) //Pour chaque organisme
            {
                if (hive[i].Type == OrganismType.Worker)//Si l'organisme est un worker
                {
                    var neighbor = MatrixHelper.NeighborMatrix(_rnd, sudoku.CellValues, hive[i].Matrix);
                    var neighborSudoku = Sudoku.New(neighbor);
                    var neighborError = neighborSudoku.Error;

                    var p = _rnd.NextDouble();
                    if (neighborError < hive[i].Error || p < 0.001)
                    {
                        hive[i].Matrix = MatrixHelper.DuplicateMatrix(neighbor);
                        hive[i].Error = neighborError;
                        if (neighborError < hive[i].Error) hive[i].Age = 0;

                        if (neighborError >= bestError) continue;
                        bestError = neighborError;
                        bestSolution = neighborSudoku;
                    }
                    else
                    {
                        hive[i].Age++;
                        if (hive[i].Age <= 1000) continue;
                        var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));
                        hive[i] = new Organism(0, randomSudoku.CellValues, randomSudoku.Error, 0);
                    }
                }
                else//Si l'organism est un Explorer
                {
                    var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));
                    hive[i].Matrix = MatrixHelper.DuplicateMatrix(randomSudoku.CellValues);
                    hive[i].Error = randomSudoku.Error;

                    if (hive[i].Error >= bestError) continue;
                    bestError = hive[i].Error;
                    bestSolution = randomSudoku;
                }
            }

            // merge best worker with best explorer into worst worker
            var bestWorkerIndex = 0;
            var smallestWorkerError = hive[0].Error;
            for (var i = 0; i < numberOfWorkers; ++i)
            {
                if (hive[i].Error >= smallestWorkerError) continue;
                smallestWorkerError = hive[i].Error;
                bestWorkerIndex = i;
            }

            var bestExplorerIndex = numberOfWorkers;
            var smallestExplorerError = hive[numberOfWorkers].Error;
            for (var i = numberOfWorkers; i < numOrganisms; ++i)
            {
                if (hive[i].Error >= smallestExplorerError) continue;
                smallestExplorerError = hive[i].Error;
                bestExplorerIndex = i;
            }

            var worstWorkerIndex = 0;
            var largestWorkerError = hive[0].Error;
            for (var i = 0; i < numberOfWorkers; ++i)
            {
                if (hive[i].Error <= largestWorkerError) continue;
                largestWorkerError = hive[i].Error;
                worstWorkerIndex = i;
            }

            var merged = MatrixHelper.MergeMatrices(_rnd, hive[bestWorkerIndex].Matrix, hive[bestExplorerIndex].Matrix);
            var mergedSudoku = Sudoku.New(merged);

            hive[worstWorkerIndex] = new Organism(0, merged, mergedSudoku.Error, 0);
            if (hive[worstWorkerIndex].Error < bestError)
            {
                bestError = hive[worstWorkerIndex].Error;
                bestSolution = mergedSudoku;
            }

            ++epoch;
        }

        return bestSolution;
    }

}

