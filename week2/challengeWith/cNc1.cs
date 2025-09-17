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
        string o = Console.ReadLine();
        if(o == "Scissors")
        {
            Console.WriteLine("Stone");
        }
        else if(o == "Hand")
        {
            Console.WriteLine("Scissors");
        }
        else if(o == "Stone")
        {
            Console.WriteLine("Hand");
        }
        else
        {
            Console.WriteLine("Error");
        }




        // Write an answer using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        
    }
}