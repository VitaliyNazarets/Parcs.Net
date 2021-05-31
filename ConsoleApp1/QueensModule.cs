using Parcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queens
{
    class QueensModule : IModule
    {
        private static bool Safe(int new_row, int new_col, List<int> sol)
        {
            for (int col = 0; col < sol.Count; col++)
            {
                if ((sol[col] == new_row) ||
                        Math.Abs(col - new_col) == Math.Abs(sol[col] - new_row))
                {
                    return false;
                }
            }
            return true;
        }

        private static int CountQ(int n_col, int width, List<int> sol)
        {
            int all_res = 0;
            if (sol.Count == width)
            {
                all_res += 1;
            }
            else
            {
                for (int n_row = 0; n_row < width; n_row++)
                {
                    if (Safe(n_row, n_col, sol))
                    {
                        List<int> new_list = sol;
                        sol.Add(n_row);
                        all_res += CountQ(n_col + 1, width, new_list);
                    }
                }
            }
            return all_res;
        }

        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            int a = info.Parent.ReadInt();
            int b = info.Parent.ReadInt();
            int n = info.Parent.ReadInt();

            double res = 0;
            for (int i = a; i <= b; i++)
            {
                List<int> list = new List<int>();
                list.Add(i);
                res += CountQ(1, n, list);
            }
            info.Parent.WriteData(res);
        }
    }
}
