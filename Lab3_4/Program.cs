namespace Lab3_4;

public class Program
{
    public static void Main(string[] args)
    {
        //Part1();
        new Part2().Run();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    private static void Part1()
    {
        var testArray = new[,] { { 1, 3, 5 }, { 2, 4, 6 }, { 7, 8, 9 } };

        Console.WriteLine("Initial array:");

        for (int i = 0; i < testArray.GetLength(0); i++)
        {
            for (int j = 0; j < testArray.GetLength(1); j++)
                Console.Write(testArray[i, j] + "\t");

            Console.WriteLine();
        }

        ArraySumDelegate anonymousArraySum = delegate (int[,] array)
        {
            int sum = 0;

            foreach (var v in array)
                sum += v;

            return sum;
        };

        var sum = anonymousArraySum(testArray);
        Console.WriteLine($"Array sum (anonymous method result): {sum}");

        ArraySumDelegate lambdaArraySum = array => array.OfType<int>().Sum();
        sum = lambdaArraySum(testArray);
        Console.WriteLine($"Array sum (lambda method result): {sum}");
    }
}
