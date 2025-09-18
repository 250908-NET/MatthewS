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
    static int getVolume(int length, int width, int height)
    {
        if(length * width * height <= 0)
        {
            throw new Exception();
        }
        return length * width * height;
    }
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int length = int.Parse(inputs[0]);
        int width = int.Parse(inputs[1]);
        int height = int.Parse(inputs[2]);
        int volume;

        // Write an answer using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        try{
            volume = getVolume(length, width, height);
            Console.WriteLine($"The quantity of water in the room is {volume} cubic feet.");
        }
        catch(Exception e)
        {
            Console.WriteLine("Invalid dimension");
        }

    }
    
}