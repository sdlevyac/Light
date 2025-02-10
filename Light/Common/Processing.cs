using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        public static Vector2 ray(Vector2 pos, Vector2 dir, int width, int height, int[,] grid_space, int lifetime, ref int[,] grid_light)
        {
            Vector2 startPos = new Vector2(pos.X, pos.Y);
            bool hit = false;
            int dim = 0;
            int length = 0;
            while (!hit)
            {
                if (dim == 0)
                {
                    pos += new Vector2(0, dir.Y);
                }
                else
                {
                    pos += new Vector2(dir.X, 0);
                }
                length++;
                if (pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height)
                {
                    grid_light[(int)pos.X, (int)pos.Y] = (int)(255 - (255 * (hyp(pos,startPos) / lifetime)));
                    if (grid_space[(int)pos.X, (int)pos.Y] == 1)
                    {
                        hit = true;
                    }
                    if (hyp(pos,startPos) >= lifetime)
                    {
                        hit = true;
                    }
                    dim = (dim + 1) % 2;
                }
                else
                {
                    break;
                }
            }
            return pos;
        }
        public static float hyp(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)); 
        }
        public static float manhattan(Vector2 a, Vector2 b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
        }
    }
}
