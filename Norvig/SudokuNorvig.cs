using Sudoku.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Norvig
{
    public class SudokuNorvig : ISudokuSolver
    {

        static string rows = "ABCDEFGHI";
        static string cols = "123456789";
        static string digits = "123456789";
        static string[] squares = cross(rows, cols);
        static Dictionary<string, IEnumerable<string>> peers;
        static Dictionary<string, IGrouping<string, string[]>> units;

        /*
         * def cross(A, B):
         *   return [a+b for a in A for b in B]
         */


        static string[] cross(string A, string B)
        {
            return (from a in A from b in B select "" + a + b).ToArray();
        }

        public SudokuNorvig()
        {
            /*
             * unitlist = ([cross(rows, c) for c in cols] +
             *           [cross(r, cols) for r in rows] +
             *           [cross(rs, cs) for rs in ('ABC','DEF','GHI') for cs in ('123','456','789')])
             */
            var unitlist = ((from c in cols select cross(rows, c.ToString()))
                               .Concat(from r in rows select cross(r.ToString(), cols))
                               .Concat(from rs in (new[] { "ABC", "DEF", "GHI" }) from cs in (new[] { "123", "456", "789" }) select cross(rs, cs)));

            /*
             * units = dict((s, [u for u in unitlist if s in u]) 
             *   for s in squares)
             */
            units = (from s in squares from u in unitlist where u.Contains(s) group u by s into g select g).ToDictionary(g => g.Key);

            /*
             * peers = dict((s, set(s2 for u in units[s] for s2 in u if s2 != s))
             *   for s in squares)
             */
            peers = (from s in squares from u in units[s] from s2 in u where s2 != s group s2 by s into g select g).ToDictionary(g => g.Key, g => g.Distinct());

        }
        /* [Javascript1.8]
         * function zip(A, B) {
         *   let z = []
         *   let n = Math.min(A.length, B.length)
         *   for (let i = 0; i < n; i++)
         *     z.push([A[i], B[i]])
         *   return z
         * }
         */

        static string[][] zip(string[] A, string[] B)
        {
            var n = Math.Min(A.Length, B.Length);
            string[][] sd = new string[n][];
            for (var i = 0; i < n; i++)
            {
                sd[i] = new string[] { A[i].ToString(), B[i].ToString() };
            }
            return sd;
        }

        /*
    def parse_grid(grid):
        "Given a string of 81 digits (or . or 0 or -), return a dict of {cell:values}"
        grid = [c for c in grid if c in '0.-123456789']
        values = dict((s, digits) for s in squares) ## To start, every square can be any digit
        for s,d in zip(squares, grid):
            if d in digits and not assign(values, s, d):
            return False
        return values
    */
        /// <summary>Given a string of 81 digits (or . or 0 or -), return a dict of {cell:values}</summary>
        public Dictionary<string, string> parseGrid (String grid)
        {

            
            var grid2 = from c in grid where "0.-123456789".Contains(c) select c;
            var values = squares.ToDictionary(s => s, s => digits); //To start, every square can be any digit

            foreach (var sd in zip(squares, (from s in grid select s.ToString()).ToArray()))
            {
                var s = sd[0];
                var d = sd[1];

                if (digits.Contains(d) && assign(values, s, d) == null)
                {
                    return null;
                }

            }


            return values;
        }

        public SudokuGrid Solve(SudokuGrid su) {
            int[][] tab = su.Cells;

            String grid = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    grid += tab[i][j];
                }

            }

            Dictionary<string, string> solve = search(parseGrid(grid));

            SudokuGrid sudoku = new SudokuGrid();

            

            int[][] tab2 = new int[9][];
            for (int i = 0; i < tab2.Length; i++)
            {
                tab2[i] = new int[9];
            }
            int cpt = 0;
            int c3 = 0;

            foreach (var v in solve)
            {
                String test = v.Key.ToString();
                Char c1 = test[0];
                int c2 = test[1] - '1';

                String test2 = v.Value.ToString();

                int t = int.Parse(test2);

                tab2[c3][c2] = t;


                cpt++;
                if (cpt == 9)
                {
                    cpt = 0;
                    c3 += 1;
                }
            }

            sudoku.Cells = tab2;

            return sudoku;
             }


        /*
     * def search(values):
     *   "Using depth-first search and propagation, try all possible values."
     *   if values is False:
     *     return False ## Failed earlier
     *   if all(len(values[s]) == 1 for s in squares): 
     *     return values ## Solved!
     *   ## Chose the unfilled square s with the fewest possibilities
     *   _,s = min((len(values[s]), s) for s in squares if len(values[s]) > 1)
     *   return some(search(assign(values.copy(), s, d)) 
     *           for d in values[s])
     */
        /// <summary>Using depth-first search and propagation, try all possible values.</summary>
        public static Dictionary<string, string> search(Dictionary<string, string> values)
        {
            if (values == null)
            {
                return null; // Failed earlier
            }
            if (all(from s in squares select values[s].Length == 1 ? "" : null))
            {
                return values; // Solved!
            }

            // Chose the unfilled square s with the fewest possibilities
            var s2 = (from s in squares where values[s].Length > 1 orderby values[s].Length ascending select s).First();







            return some(from d in values[s2]
                        select search(assign(new Dictionary<string, string>(values), s2, d.ToString())));
        }



        /*
     * def assign(values, s, d):
     *   "Eliminate all the other values (except d) from values[s] and propagate."
     *   if all(eliminate(values, s, d2) for d2 in values[s] if d2 != d):
     *     return values
     *   else:
     *     return False
     */
        /// <summary>Eliminate all the other values (except d) from values[s] and propagate.</summary>
        static Dictionary<string, string> assign(Dictionary<string, string> values, string s, string d)
        {
            if (all(
                    from d2 in values[s]
                    where d2.ToString() != d
                    select eliminate(values, s, d2.ToString())))
            {
                return values;
            }
            return null;
        }

        // Eliminate d from values[s]; propagate when values or places <= 2.
        /* def eliminate(values, s, d):
         *   "Eliminate d from values[s]; propagate when values or places <= 2."
         *   if d not in values[s]:
         *       return values ## Already eliminated
         *   values[s] = values[s].replace(d,'')
         *   if len(values[s]) == 0:
         *       return False ## Contradiction: removed last value
         *   elif len(values[s]) == 1:
         *       ## If there is only one value (d2) left in square, remove it from peers
         *       d2, = values[s]
         *       if not all(eliminate(values, s2, d2) for s2 in peers[s]):
         *           return False
         *   ## Now check the places where d appears in the units of s
         *   for u in units[s]:
         *       dplaces = [s for s in u if d in values[s]]
         *       if len(dplaces) == 0:
         *           return False
         *       elif len(dplaces) == 1:
         *           # d can only be in one place in unit; assign it there
         *           if not assign(values, dplaces[0], d):
         *               return False
         *   return values
         */
        /// <summary>Eliminate d from values[s]; propagate when values or places &lt;= 2.</summary>
        static Dictionary<string, string> eliminate(Dictionary<string, string> values, string s, string d)
        {
            if (!values[s].Contains(d))
            {
                return values;
            }
            values[s] = values[s].Replace(d, "");
            if (values[s].Length == 0)
            {
                return null; //Contradiction: removed last value
            }
            else if (values[s].Length == 1)
            {
                //If there is only one value (d2) left in square, remove it from peers
                var d2 = values[s];
                if (!all(from s2 in peers[s] select eliminate(values, s2, d2)))
                {
                    return null;
                }
            }

            //Now check the places where d appears in the units of s
            foreach (var u in units[s])
            {
                var dplaces = from s2 in u where values[s2].Contains(d) select s2;
                if (dplaces.Count() == 0)
                {
                    return null;
                }
                else if (dplaces.Count() == 1)
                {
                    // d can only be in one place in unit; assign it there
                    if (assign(values, dplaces.First(), d) == null)
                    {
                        return null;
                    }
                }
            }
            return values;
        }


        /*
     * def all(seq):
     *   for e in seq:
     *     if not e: return False
     *   return True
     */
        static bool all<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e == null) return false;
            }
            return true;
        }

        /*
         * def some(seq):
         *   for e in seq:
         *     if e: return e
         *  return False
         */
        static T some<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e != null) return e;
            }
            return default(T);
        }
        /*
         * def center(s, width):
         *   n = width - len(s)
         *   if n <= 0: return s
         *   half = n/2
         *   if n%2 and width%2:
         *     half = half+1
         *   return ' '*half +  s + ' '*(n-half)
         */



        static string Center( string s, int width)
        {
            var n = width - s.Length;
            if (n <= 0) return s;
            var half = n / 2;

            if (n % 2 > 0 && width % 2 > 0) half++;

            return new string(' ', half) + s + new String(' ', n - half);
        }
        /*
         * def printboard(values):
         *   "Used for debugging."
         *   width = 1+max(len(values[s]) for s in squares)
         *   line = '\n' + '+'.join(['-'*(width*3)]*3)
         *   for r in rows:
         *     print ''.join(values[r+c].center(width)+(c in '36' and '|' or '')
         *            for c in cols) + (r in 'CF' and line or '')
         *   print
         *   return values
         */
        /// <summary>Used for debugging.</summary>
        static Dictionary<string, string> print_board(Dictionary<string, string> values)
        {
            if (values == null) return null;

            var width = 1 + (from s in squares select values[s].Length).Max();
            var line = "\n" + String.Join("+", Enumerable.Repeat(new String('-', width * 3), 3).ToArray());

            foreach (var r in rows)
            {
                Console.WriteLine(String.Join("",
                    (from c in cols
                     select Center(values["" + r + c], width)
                     + ("36".Contains(c) ? "|" : "")).ToArray())
                        + ("CF".Contains(r) ? line : ""));

            }

            Console.WriteLine();
            return values;
        }

        /*
        public SudokuGrid Solve(SudokuGrid s)

        {
            /*
            int[][] tab = s.Cells;
            
            String grid = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    
                    grid+= tab[i][j];
                }
                
            }
            Console.WriteLine(grid);
            *//*

            var values = squares.ToDictionary(s => s, s => digits); //To start, every square can be any digit
            SudokuGrid sudoku= new SudokuGrid();
            int[][] tab = new int[9][];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = new int[10];
            }
            int cpt = 0;
            int c = 0;
            
            foreach (var v in values)
            {
                String test = v.Key.ToString();
                Char c1 = test[0];
                int c2 = test[1]-'1';

                Console.WriteLine(c+" "+c2);
                String test2 = v.Value.ToString();

                int t = int.Parse(test2);

                tab[c][c2] = t;
                Console.WriteLine(tab[c][c2]);


                cpt++;
                if (cpt == 9)
                {
                    cpt= 0;
                    c += 1;
                }
            }

            sudoku.Cells= tab;

            return sudoku;
        }*/
        
    }
}