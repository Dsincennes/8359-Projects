/*
 * Class: CST8359 LAB
 * Student: Donald Sincennes
 * Lab: Lab01
 * Date: May/10/2023
 */

using System.Diagnostics;

namespace _8359_DonaldSincennes_Lab01;

class Lab01
{
    // Check List
    public static bool CheckList(List<string> wordList)
    {
        if (!wordList.Any())
        {
            Console.WriteLine("Please import a file first");
            return false;
        }
        return true;
    }

    // Read words from a file
    public static List<string> ReadFile()
    {
        List<string> wordList = new();
        try
        {
            string[] words = File.ReadAllLines("Words.txt");
            foreach (string word in words)
            {
                wordList.Add(word);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error reading File.");
        }
        Console.WriteLine($"The number of words is {wordList.Count}");
        return wordList;
    }

    // Bubble sort words
    public static void BubbleSort(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = new List<string>(wordList);
        Stopwatch sw = new();
        sw.Start();
        for (int i = 0; i < newList.Count - 1; i++)
        {
            for (int j = 0; j < i - 1; j++)
            {
                if (newList[j].CompareTo(newList[j + 1]) > 0)
                {
                    (newList[j + 1], newList[j]) = (newList[j], newList[j + 1]); // using tuple to swap
                }
            }
        }
        sw.Stop();
        Console.WriteLine($"Bubblesorting the list took {sw.ElapsedMilliseconds} milliseconds.");
        return;
    }

    // Linq Sort a list
    public static void LinqSort(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        Stopwatch sw = new();
        sw.Start();
        var sortedNames = wordList.OrderBy(x => x).ToList();
        sw.Stop();
        Console.WriteLine($"Linq Sorting the list took {sw.ElapsedMilliseconds} milliseconds.");
        return;
    }

    // Count distinct words
    public static void DistinctCount(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        Console.WriteLine($"Distinct Count is {wordList.Distinct().Count()}");
        return;
    }

    // Take last 50 words
    public static void LastFifty(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = wordList.SkipWhile((value, index) => index < wordList.Count - 50).ToList();
        foreach (var word in newList)
        {
            Console.WriteLine(word);
        }
        return;
    }

    // Print entire list backwards
    public static void ListBackwards(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = new List<string>(wordList);
        newList.Reverse();
        foreach (var word in newList)
        {
            Console.WriteLine(word);
        }
        return;
    }

    // Display words that end with D, count words.
    public static void EndsWithD(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = new List<string>(wordList);
        int count = 0;
        foreach (var word in newList.Where(x => x.EndsWith('d')))
        {
            Console.WriteLine(word);
            count++;
        }
        Console.WriteLine($"{count} words ended with 'd'");
    }

    // Display words that start with r, count words.
    public static void StartsWithR(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = new List<string>(wordList);
        int count = 0;
        foreach (var word in newList.Where(x => x.StartsWith('r')))
        {
            Console.WriteLine(word);
            count++;
        }
        Console.WriteLine($"{count} words started with 'r'");
    }

    // Display words that are bigger then 3 and contain a
    public static void BiggerThanThreeAndContainA(List<string> wordList)
    {
        if (!CheckList(wordList))
        {
            return;
        }
        var newList = new List<string>(wordList);
        int count = 0;
        foreach (var word in newList.Where(x => x.Contains('a') && x.Length > 3))
        {
            count++;
            Console.WriteLine(word);
        }
        Console.WriteLine($"{count} words contain 'a' and are greater than 3 in length");
        return;
    }

}
