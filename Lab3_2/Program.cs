using System.Collections;
using System.Drawing;
namespace Lab3_2;

public class Program
{
    private static readonly RadiusVector3D _v1 = RadiusVector3D.Create(1, 3, 5, Color.Red);
    private static readonly RadiusVector3D _v2 = RadiusVector3D.Create(2, -4, 6, Color.Green);
    private static readonly RadiusVector3D _v3 = RadiusVector3D.Create(3, 4, -5, Color.Blue);
    private static readonly RadiusVector3D _v4 = RadiusVector3D.Create(-4, -2, -5, Color.Yellow);

    public static void Main(string[] args)
    {
        Part1();
        Part2();
        Part3_4();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void Part1()
    {
        var v = RadiusVector3D.Create(1, 3, 5, Color.Red);

        Console.WriteLine("Vector1 info:");
        Console.WriteLine($"{v.Info()}\n");

        const double segmentLength = 5.9;
        v.IncreaseBy(segmentLength);
        Console.WriteLine($"Vector1 info after increasing by a segment of length {segmentLength}:");
        Console.WriteLine($"{v.Info()}\n");
    }

    private static void Part2()
    {
        var genericList = new List<RadiusVector3D> { _v1, _v2, _v3 };
        var arrayList = new ArrayList { _v1, _v2, _v3 };
        var array = new RadiusVector3D[] { _v1, _v2, _v3 };
        Console.WriteLine("Original arrays:");
        PrintCollections(genericList, arrayList, array);

        //Add element to collections
        var indexToInsert = 1;
        genericList.Insert(indexToInsert, _v4);
        arrayList.Insert(indexToInsert, _v4);
        array = [.. array[..indexToInsert], _v4, .. array[indexToInsert..]];
        Console.WriteLine($"Collections after insert new item at index {indexToInsert}:");
        PrintCollections(genericList, arrayList, array);

        //Remove element from collections
        var indexToRemove = 2;
        genericList.Remove(_v2);
        arrayList.RemoveAt(indexToRemove);
        array = [.. array[..indexToRemove], .. array[(indexToRemove + 1)..]];
        Console.WriteLine($"Collections after remove item at index {indexToRemove}:");
        PrintCollections(genericList, arrayList, array);

        //Update element in collections
        var indexToUpdate = 1;
        genericList[indexToUpdate] = _v2;
        arrayList[indexToUpdate] = _v2;
        array[indexToUpdate] = _v2;
        Console.WriteLine($"Collections after update item at index {indexToUpdate}:");
        PrintCollections(genericList, arrayList, array);

        //Find element in collections
        Console.WriteLine("Elements search:");

        var v2Index = genericList.FindIndex(v => v.LineColor == Color.Green);
        Console.WriteLine($"Green vector in the generic list is at index {v2Index}");

        v2Index = arrayList.IndexOf(_v2);
        Console.WriteLine($"Vector v2 in the array list is at index {v2Index}");

        v2Index = array.IndexOf(_v2);
        Console.WriteLine($"Vector v2 in the array is at index {v2Index}");

        Console.WriteLine();
    }

    private static void Part3_4()
    {
        var binaryTree = BinaryTree<RadiusVector3D>.Create();
        binaryTree.Insert(_v2);
        binaryTree.Insert(_v1);
        binaryTree.Insert(_v3);

        Console.WriteLine("Binary tree elements:");

        foreach (var v in binaryTree)
            Console.WriteLine($"\t-Vector: info: {v.Info()}");
    }

    private static void PrintCollections(List<RadiusVector3D> genericList, ArrayList arrayList, RadiusVector3D[] array)
    {
        Console.WriteLine("Generic list of vectors:");
        for (var i = 0; i < genericList.Count; i++)
            Console.WriteLine($"\t-Vector{i} info: {genericList[i].Info()}");

        Console.WriteLine("Array list of vectors:");
        for (var i = 0; i < arrayList.Count; i++)
            Console.WriteLine($"\t-Vector{i} info: {(arrayList[i] as RadiusVector3D)?.Info() ?? "undefined"}");

        Console.WriteLine("Array of vectors:");
        for (var i = 0; i < array.Length; i++)
            Console.WriteLine($"\t-Vector{i} info: {array[i].Info()}");

        Console.WriteLine();
    }
}
