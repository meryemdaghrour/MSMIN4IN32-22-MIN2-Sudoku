using System;
using System.Diagnostics;
using aima.core.search.csp;
using Sudoku.Shared;


namespace Sudoku.CSPSolver
{
    public abstract class CSPSolverBase : ISudokuSolver
    {

        public CSPSolverBase()
        {
            _Strategy = GetStrategy();
        }

        private readonly SolutionStrategy _Strategy;

        // Vous n'avez plus qu'à suivre les commentaires, ça devrait aller vite.
        public SudokuGrid Solve(SudokuGrid s)
        {
            //Construction du CSP en utilisant CspHelper


            // Utilisation de la stratégie pour résoudre le CSP


            //Utilisation de CSPHelper pour traduire l'assignation en SudokuGrid


            return s;
        }

        protected abstract SolutionStrategy GetStrategy();

    }

    //Voilà un premier solver, je vous laisse implémenter les autres avec différentes combinaisons de paramètres pour pouvoir contraster leurs performances
    public class CSPMRVDegLCVFCSolver : CSPSolverBase
    {
        protected override SolutionStrategy GetStrategy()
        {
            var objStrategyInfo = new CSPStrategyInfo
            {
                EnableLCV = true,
                Inference = CSPInference.ForwardChecking,
                Selection = CSPSelection.MRVDeg,
                StrategyType = CSPStrategy.ImprovedBacktrackingStrategy,
                MaxSteps = 5000
            };
            return objStrategyInfo.GetStrategy();
        }
    }



}