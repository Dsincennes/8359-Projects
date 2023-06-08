/*
 * Class: CST8359 LAB
 * Student: Donald Sincennes
 * Lab: Lab01
 * Date: May/10/2023
 */

using _8359_DonaldSincennes_Lab01;

bool cont = true;
List<string> list = new();

do
{
    Console.Write("""
        Choose an option:
        1 - Import Words from File
        2 - Bubble Sort words
        3 - LINQ/Lambda sort words
        4 - Count the Distinct Words
        5 - Take the last 50 words
        6 - Reverse print the words
        7 - Get and display words that end with 'd' and display the count
        8 - Get and display words that start with 'r' and display the count
        9 - Get and display words that are more than 3 characters long and include the letter 'a', and display the count
        x - Exit

        Select an option: 
        """);

    switch (Console.ReadLine())
    {
        case "1":
            list = Lab01.ReadFile();
            break;
        case "2":
            Lab01.BubbleSort(list);
            break;
        case "3":
            Lab01.LinqSort(list);
            break;
        case "4":
            Lab01.DistinctCount(list);
            break;
        case "5":
            Lab01.LastFifty(list);
            break;
        case "6":
            Lab01.ListBackwards(list);
            break;
        case "7":
            Lab01.EndsWithD(list);
            break;
        case "8":
            Lab01.StartsWithR(list);
            break;
        case "9":
            Lab01.BiggerThanThreeAndContainA(list);
            break;
        case "x":
        case "X":
            Console.WriteLine("Goodbye.");
            cont = false;
            break;
        default:
            Console.WriteLine("Incorrect Selection. Try Again!");
            break;
    }
} while (cont is not false);
