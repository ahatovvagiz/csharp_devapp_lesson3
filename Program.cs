//Доработайте приложение поиска пути в лабиринте, но на этот раз вам нужно определить сколько всего выходов имеется в лабиринте:

//int[,] labirynth1 = new int[,]
//{
//{1, 1, 1, 1, 1, 1, 1 },
//{1, 0, 0, 0, 0, 0, 1 },
//{1, 0, 1, 1, 1, 0, 1 },
//{0, 0, 0, 0, 1, 0, 0 },
//{1, 1, 0, 0, 1, 1, 1 },
//{1, 1, 1, 0, 1, 1, 1 },
//{1, 1, 1, 0, 1, 1, 1 }
//};

//Сигнатура метода:

//static int HasExit(int startI, int startJ, int[,] l)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

public class Labyrinth
{
    private static int[,] l;
    private static int LengthRow;
    private static int LengthCol;
    private static List<Tuple<int, int>> EntranceList = new List<Tuple<int, int>>();

    public Labyrinth(int[,] paramL)
    {
        l = paramL;
        LengthRow = l.GetLength(0);
        LengthCol = l.GetLength(1);
        Init();
    }

    public Labyrinth()
    {
        l = new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 1},
            {1, 0, 1, 1, 1, 0, 1},
            {0, 0, 0, 0, 1, 0, 0},
            {1, 1, 0, 0, 1, 1, 1},
            {1, 1, 1, 0, 1, 1, 1},
            {1, 1, 1, 0, 1, 1, 1}
        };
        Init();
    }

    private static void Init()
    {
        //запомним длинну по вертикали
        LengthRow = l.GetLength(0);
        //запомним длинну по горизонтали
        LengthCol = l.GetLength(1);
        //входы сверху
        for (int i = 0; i < LengthRow; i++)
        {
            if (l[0, i] == 0)
            {
                EntranceList.Add(new Tuple<int, int>(0, i));
            }
            //входы снизу
            if (l[LengthCol - 1, i] == 0)
            {
                EntranceList.Add(new Tuple<int, int>(LengthCol - 1, i));
            }
        }
        //входы слева
        for (int i = 1; i < LengthCol - 1; i++)
        {
            if (l[i, 0] == 0)
            {
                EntranceList.Add(new Tuple<int, int>(i, 0));
            }
            //входы справа
            if (l[i, LengthCol - 1] == 0)
            {
                EntranceList.Add(new Tuple<int, int>(i, LengthCol - 1));
            }
        }
    }

    public static void PrintEntrance()
    {
        int i = 0;
        foreach (var e in EntranceList)
        {
            i++;
            Console.WriteLine($"{i}: {FromIndexToCoord(e)}");
        }

        Console.WriteLine($"Общее количество входов/выходов: {i}");
    }

    public static void PrintArray()
    {
        for (int i = 0; i < LengthRow; i++)
        {
            for (int j = 0; j < LengthCol; j++)
            {
                Console.Write(l[i, j]+" ");
            }
            Console.WriteLine();
        }
    }

    public static void GetWays()
    {
        bool Error = false;
        PrintArray();
        Console.WriteLine("Введите номер точки входа из списка ниже (0 - вывести пути для всех точек входа)");
        PrintEntrance();
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            if (n == 0) 
            {
                for (int i = 0; i < EntranceList.Count; i++)
                {
                    Start(i);
                }
            }
            else
            {
                int i = n - 1;
                if (i < EntranceList.Count)
                {
                    Start(i);
                }
                else
                {
                    Error = true;
                }
            }
        }
        else
        {
            Error = true;
        }

        if (Error)
        {
            Console.WriteLine("Неверное значение");
        }

    }

    private static void Start(int i)
    {
        var point = EntranceList[i]; 
        Console.WriteLine($"{i+1}:({point.Item1 + 1}, {point.Item2 + 1})");
        var q = new Stack<Tuple<int, int>>();
        HasExit(point, point, q);
    }

    private static void HasExit(Tuple<int, int> startpoint, Tuple<int, int> point, Stack<Tuple<int, int>> stck)
    {
        var arr = stck.ToArray();

        if (stck.Contains(point))
        {
            stck.Clear();
            return;
        }

        if (EntranceList.Contains(point) & !startpoint.Equals(point))
        {
            Console.Write("[");
            var dispstack = new Stack<Tuple<int, int>>(arr);
            while (dispstack.TryPop(out Tuple<int, int>? a))
            {
                Console.Write(FromIndexToCoord(a));
            }
            Console.WriteLine(FromIndexToCoord(point) + "]");
            return;
        }
        
        Array.Reverse(arr);
        //смотрим слева
        if (point.Item1 - 1 >= 0)
        {
            TryMove(new Tuple<int, int>(point.Item1 - 1, point.Item2), point, startpoint, arr);
        }
        //смотрим сверху
        if (point.Item2 - 1 >= 0)
        {
            TryMove(new Tuple<int, int>(point.Item1, point.Item2 - 1), point, startpoint, arr);
        }
        //смотрим справа
        if (point.Item1 + 1 < LengthRow)
        {
            TryMove(new Tuple<int, int>(point.Item1 + 1, point.Item2), point, startpoint, arr);
        }
        //смотрим снизу
        if (point.Item2 + 1 < LengthCol)
        {
            TryMove(new Tuple<int, int>(point.Item1, point.Item2 + 1), point, startpoint, arr);
        }
    }

    private static string FromIndexToCoord(Tuple<int, int> point)
    {
        return ($"({point.Item1 + 1}, {point.Item2 + 1})");
    }

    private static void TryMove(Tuple<int, int> nextpoint, Tuple<int, int> point, Tuple<int, int> startpoint, Tuple<int, int>[] arr)
    {
        if (l[nextpoint.Item1, nextpoint.Item2] == 0)
        {
            var newstack = new Stack<Tuple<int, int>>(arr);
            newstack.Push(point);
            HasExit(startpoint, nextpoint, newstack);
        }

    }
}



public class Program
{
    public static void Main()
    {
        Labyrinth lab = new Labyrinth();
        //Labyrinth.PrintEntrance();
        Labyrinth.GetWays();
    }
}
