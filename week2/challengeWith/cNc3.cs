using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int n = int.Parse(inputs[0]);
        int m = int.Parse(inputs[1]);
        int mines = 0;
        for (int i = 0; i < n; i++)
        {
            string row = Console.ReadLine();
            if(row.Contains('M'))
            {
                for (int j = 0; j < row.Length; j++)
                {
                    if(row[j] == 'M')
                    {
                        mines += 1;
                    }
                }
            }
        }

        // Write an answer using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(mines);
    }
}