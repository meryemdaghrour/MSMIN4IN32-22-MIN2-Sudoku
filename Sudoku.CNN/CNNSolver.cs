using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.CNN
{
    internal class CNNPythonSolver : PythonSolverBase
    {


        public override Shared.SudokuGrid Solve(Shared.SudokuGrid s)
        {

            //using (Py.GIL())
            //{
            // create a Python scope
            using (PyModule scope = Py.CreateScope())
            {
				// On embarque le modèle dans les ressources pour être sûr de maîtriser son emplacement
				var modelPath = Path.Combine(Environment.CurrentDirectory, @"sudoku-model.h5");
				if (!File.Exists(modelPath))
				{
				   File.WriteAllBytes(modelPath,Resource1.sudoku_model);
				}
				//Transformation du chemin du modèle en python
				PyObject pyModelPath = modelPath.ToPython();
				//assignation à une variable pour le script
				scope.Set("load_model_location", pyModelPath);


				// convert the Person object to a PyObject
				PyObject pyCells = s.Cells.ToPython();

                // create a Python variable "person"
                scope.Set("instance", pyCells);

                // the person object may now be used in Python
                string code = Resource1.cnn_solver_py;
                scope.Exec(code);
                var result = scope.Get("r");
                var managedResult = result.As<int[][]>();
                //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
                return new Shared.SudokuGrid() { Cells = managedResult };
            }
            //}

        }

        protected override void InitializePythonComponents()
        {
            //InstallPipModule("z3-solver");
            InstallPipModule("tensorflow");
            base.InitializePythonComponents();
        }



    }
}