using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    internal static class Processing
    {
        private static List<int[]> directions = new List<int[]>
        {
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { 1, 0 },
            new int[] {-1, 0}
        };
        private static List<string> hit;
        public static int[,] diffuse(int[,] input, int passes)
        {
            hit = new List<string>();
            for (int i = 0; i < passes; i++)
            {
                input = applyDiffusion(input);
            }
            return input;
        }
        private static int[,] applyDiffusion(int[,] input)
        {
            int[,] buffer = new int[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i,j] != 0)
                    {
                        buffer[i, j] = input[i, j];
                    }
                    int val = 0;
                    int vals = 1;
                    foreach (int[] dir in directions)
                    {
                        int di = i + dir[0], dj = j + dir[1];

                        if (di >= 0 && di < input.GetLength(0) && dj >= 0 && dj < input.GetLength(1) && input[di,dj] != 0)
                        {
                            val += input[di, dj];
                            vals++;
                        }
                    }
                    if (val != 0 && input[i, j] != 255 && !hit.Contains($"{i},{j}"))
                    {
                        buffer[i, j] = (int)((val / vals) * 1);
                        hit.Add($"{i},{j}");
                    }
                }
            }
            return buffer;
        }
    }
}
