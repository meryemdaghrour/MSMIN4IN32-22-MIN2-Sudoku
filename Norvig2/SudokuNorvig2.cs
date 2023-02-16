using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
class Possible
{
    private bool[] _b;

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
    private List<Possible> _cells=new List<Possible>();
    private static List<List<int>> _group;
    private static List<List<int>> _neighbors;
    private static List<List<int>> _groups_of;



    private bool Eliminate(int k, int val)
    {
        Possible cell = _cells[k];
        if (!cell.is_on(val))
        {
            return true;
        }

        cell.eliminate(val);
        int N = cell.count();

        if (N == 0)
        {
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

    public Sudoku2(string s)
    {
        // Implementation not provided
    }

    public static void Init()
    {
        static void Init()
        {
            _group = new List<List<int>>(27);
            _neighbors = new List<List<int>>(81);
            _groups_of = new List<List<int>>(81);
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
            int m = _cells[i].count();
            if (m > 1 && (k == -1 || m < min))
            {
                min = m;
                k = i;
            }
        }
        return k;
    }

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
        if (S == null || S.IsSolved())
        {
            return new SolveResult() { Result = S, IsSuccess = true };
        }

        int k = S.LeastCount();
        Possible p = S.Possible(k);
        for (int i = 1; i <= 9; i++)
        {
            if (p.is_on(i))
            {
                Sudoku2 S1 = new Sudoku2(S);
                if (S1.Assign(k, i))
                {
                    SolveResult result = Solve(S1);
                    if (result.IsSuccess)
                    {
                        return result;
                    }
                }
            }
        }
        return new SolveResult() { Result = null, IsSuccess = false };
    }

}
class Program2
{
    static void Main(string[] args)
    {
        Sudoku2.Init();
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            Sudoku2 S = new Sudoku2(line);
            if (S.IsSolved())
            {
                S.Write(Console.Out);
            }
            else
            {
                Console.WriteLine("No solution");
            }
            Console.WriteLine();
        }
    }
}
