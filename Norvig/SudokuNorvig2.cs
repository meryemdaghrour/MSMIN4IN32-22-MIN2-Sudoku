using Sudoku.Shared;

public class SudokuNorvig2 : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {

        int[][] tab = s.Cells;

        String line = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {

                line += tab[i][j];
            }

        }

        Sudoku2.Init();

        

        
        Sudoku2 S = new Sudoku2(line); //trouver cmt inclure line dans Sudoku2 : adapter constructeur
        //tout marche bien ici ! 
        //->application des contraintes bonne, le probleme est pour les resolutions complexes
        
        

        SolveResult sol = SolveResult.Solve(S); //probleme
        S = sol.Result;

        SudokuGrid sudoku = new SudokuGrid();
        int[][] tab2 = new int[9][];
        for (int i = 0; i < tab2.Length; i++)
        {
            tab2[i] = new int[9];
        }

        if (S.IsSolved())
        {
            int cpt = 0;
            int c = 0;
            for (int l = 0; l < 81; l++)
            {
                //S._cells[l];//liste
                int result = 0;
                {
                    for (int j = 0; j < 9; j++)
                    {
                        
                        if (S._cells[l]._b[j])
                        {
                            result = j+1;
                        }
                    }

                    
                    tab2[c][cpt] = result;




                    cpt++;

                    if (cpt == 9)
                    {
                        cpt = 0;
                        c++;
                    }

                }
                //remplir une ligne dans un tableau 2 dim
            }
        }
        else
        {
            Console.WriteLine("No solution");//return s.CloneSudoku();
        }
            

        sudoku.Cells = tab2;

        //a remplir en utilisant la méthode solve dans solveresult
        return sudoku;
        // de Sudoku 2 à SudokuGrid : mettre des true sur le n° _cell correspondant
    }

}



class Possible
{
    public bool[] _b;

    public Possible()
    {
        _b = new bool[9] { true, true, true, true, true, true, true, true, true };
    }

    public bool is_on(int i)
    {
        return _b[i - 1];
    }
    

    public int count()
    {
        return _b.Count(x => x == true);
    }

    public void eliminate(int i)
    {
        _b[i - 1] = false;
    }

    public int val()
    {
        for (int i = 0; i < 9; i++)
        {
            if (_b[i] == true)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public string str(int width)
    {
        char[] s = new char[width];
        for (int i = 0; i < width; i++)
        {
            s[i] = ' ';
        }

        int k = 0;
        for (int i = 1; i <= 9; i++)
        {
            if (is_on(i))
            {
                s[k++] = (char)('0' + i);
            }
        }

        return new string(s);
    }
}
class Sudoku2
{
    public List<Possible> _cells = new List<Possible>();

    /*
            {
                new Possible(),
            }*/
    private static List<List<int>> _group;
    private static List<List<int>> _neighbors;
    private static List<List<int>> _groups_of;



    private bool Eliminate(int k, int val)

    {
        //Console.WriteLine(k);
        //Console.WriteLine("test pour case : " + k+" valeur : "+val);
        Possible cell = _cells[k];
        if (!cell.is_on(val))
        {
            return true;
        }

        cell.eliminate(val);
        int N = cell.count();

        if (N == 0)
        {
            
            //Console.WriteLine("test pour case : " + k + " valeur : " + val);
            return false;
        }
        else if (N == 1)
        {
            int v = cell.val();
            for (int i = 0; i < _neighbors[k].Count; i++)
            {
                if (!Eliminate(_neighbors[k][i], v)) return false;
            }
        }

        for (int i = 0; i < _groups_of[k].Count; i++)
        {
            int x = _groups_of[k][i];
            int n = 0, ks = 0;
            for (int j = 0; j < 9; j++)
            {
                int p = _group[x][j];
                if (_cells[p].is_on(val))
                {
                    n++;
                    ks = p;
                }
            }

            if (n == 0)
            {
                return false;
            }
            else if (n == 1)
            {
                if (!Assign(ks, val)) return false;
            }
        }
        return true;
    }

    static public Boolean pb = false;

    public Sudoku2(String s) 
    {
        pb = false;
        
        _cells = new List<Possible>
        {
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),new Possible(),
        new Possible()
        };
        //modifier _cells !!!!
        int k = 0;
        //int test = 0;
        //Console.WriteLine(s[78]);
        
        for (int i = 0; i < s.Length; i++)
        {
            /*
            if (test == 1)
            {
                //Console.WriteLine("ERREUR ");
                pb = true;
            }
            test = 1;*/
            //Console.WriteLine(s[i]);
            //Console.WriteLine(k);
            //OK
            if (s[i] >= '1' && s[i] <= '9')
            {
                //ok

                if (!Assign(k, s[i] - '0'))
                {
                    //Console.WriteLine("i = " + i);
                    //Console.WriteLine("k = " + k);
                    //Console.WriteLine(s[i] - '0');
                    pb = true;
                }
                k++;
            }
            else if (s[i] == '0' || s[i] == '.')
            {
                k++;
                //ok
            }
            //test = 2;
           // Console.WriteLine(i);
            //Console.WriteLine("k = "+k);


        }

        


    }

    public Sudoku2()
    {
    }

    public Sudoku2(Sudoku2 S)
    {
        this._cells = S._cells;
    }

    public String Line (int k, int i)
    {
        String line = "";

        //modification de la case

        for (int l = 0; l < 9; l++)
        {
            this._cells[k]._b[l] = false;
        }
        this._cells[k]._b[i-1] = true;
        

        //Conversion en String
        for (int l = 0; l < 81; l++)
        {
            int m = _cells[l].count();

            if (m == 1)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_cells[l]._b[j] == true)
                    {
                        line += j + 1;
                    }
                }
            }
            else
            {
                line += "0";
            }

        }
        
        return line;

    }


    public Boolean Success()
    {
        for (int i=0; i < _cells.Count; i++)
        {
            int m = _cells[i].count();  
            if (m != 1)
            {
                return false;
            }
        }

        return true; 
    }

    public static void Init()
    {

        _group = new List<List<int>>
            {
                new List<int> {}, new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{}
             };
        _neighbors = new List<List<int>>
            {
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{}
            };
        _groups_of = new List<List<int>>
            {
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},new List<int>{},
                new List<int>{}
            };
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int k = i * 9 + j;
                int[] x = new int[3] { i, 9 + j, 18 + (i / 3) * 3 + j / 3 };
                for (int g = 0; g < 3; g++)
                {

                    _group[x[g]].Add(k);
                    _groups_of[k].Add(x[g]);

                }
            }
        }
        for (int k = 0; k < _neighbors.Count; k++)
        {
            for (int x = 0; x < _groups_of[k].Count; x++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int k2 = _group[_groups_of[k][x]][j];
                    if (k2 != k) _neighbors[k].Add(k2);
                }
            }
        }
        
    }

    

    public Possible Possible(int k)
    {
        return this._cells[k];
    }

    public bool IsSolved()
    {
        for (int k = 0; k < _cells.Count; k++)
        {
            if (_cells[k].count() != 1)
            {
                return false;
            }
        }
        return true;
    }

    public bool Assign(int k, int val)
    {
        
        for (int i = 1; i <= 9; i++)
        {
            if (i != val)
            {
                if (!Eliminate(k, i)) return false;
            }
        }
        return true;
    }

    public int LeastCount()
    {
        int k = -1;
        int min = int.MaxValue;
        for (int i = 0; i < _cells.Count; i++)
        {
            //Console.WriteLine(i);
            //Console.WriteLine(k);
            int m = _cells[i].count();
            //Console.WriteLine(m);


            if (m > 1 && (k == -1 || m < min))
            {
                min = m;
                k = i;
            }
            //Console.WriteLine();
        }
        //Console.WriteLine("k = "+k);
        return k;
    } //

    public void Write(TextWriter o)
    {

        int width = 1;
        for (int k = 0; k < _cells.Count(); k++)
        {
            width = Math.Max(width, 1 + _cells[k].count());
        }
        string sep = new string('-', 3 * width);
        for (int i = 0; i < 9; i++)
        {
            if (i == 3 || i == 6)
            {
                o.WriteLine(sep + "+" + sep + "+" + sep);
            }
            for (int j = 0; j < 9; j++)
            {
                if (j == 3 || j == 6) o.Write("| ");
                o.Write(_cells[i * 9 + j].str(width));
            }
            o.WriteLine();
        }
    }



}
class myClass : ICloneable
{
    public String test;
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
class SolveResult
{

    public Sudoku2 Result { get; set; }
    public bool IsSuccess { get; set; }


    public static SolveResult Solve(Sudoku2 S)
    {
        

        //cells OK
        //grille de sudoku sudoku2
        //solveresult renvoie un booleen qui dit si la resolution est un sicces o non et une grille de sudoku résolue.
        if (S == null || S.IsSolved())
        {
            return new SolveResult() { Result = S, IsSuccess = true };
        }

        //si pas de grille ou grille résolue, on renvoie la grille initiale
        int k = S.LeastCount(); //k=8 : premiere case à seulement 2 possibilités
        //permettrait de connaitre la premiere case qui a plusieurs true
        //Console.WriteLine("k = " + k);
        Possible p = S.Possible(k);
        List<Boolean> tab = new List<Boolean>() { false,false,false,false,false,false,false,false,false  };

        for (int i = 1; i <= 9; i++)
        {
            tab[i-1] = p.is_on(i);

        }


            //for (int i = 1; i <= 9; i++)
            for (int i = 1; i <= 9; i++)
        {

            //repérer la premiere possibilité
            //Console.WriteLine(p.is_on(i-1));

            //if (p.is_on(i)) //si le booléen est true
                             //if (p.is_on(i))
            if (tab[i-1]==true)
            {

                
                Boolean ok = false;

                //faire un clone avec une seule difference de S

                String line = S.Line(k, i);
                Sudoku2.Init();
                Sudoku2 S1 = new Sudoku2(line);
                //clone S
                
                //Console.WriteLine("cells de S1");
                
                ok = Sudoku2.pb;
                Boolean cont = false;
                if (!ok)
                {
                    cont = S1.Assign(k, i);
                }



                
                //if (S1.Assign(k, i))
                if (cont)

                {
                    SolveResult result = Solve(S1);


                    if (result.IsSuccess)
                    {
                        return result;
                    }
                }
            }
            //Console.WriteLine();
        }
        //je suis ici
        return new SolveResult() { Result = null, IsSuccess = false };
    }



}
