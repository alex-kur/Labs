using System.Collections;

namespace Lab3_2;

public class BinaryTree<T> : IEnumerable<T> where T : class, IComparable<T>
{
    private BinaryTree() { }
    public BinaryTreeNode<T>? Root { get; private set; }
    public static BinaryTree<T> Create() => new() { };

    public void Insert(T value)
    {
        Root = InsertImpl(Root, value);
    }

    private BinaryTreeNode<T> InsertImpl(BinaryTreeNode<T>? node, T value)
    {
        if (node == null)
            return BinaryTreeNode<T>.Create(value);

        if (value.CompareTo(node.Value) < 0)
            node.Left = InsertImpl(node.Left, value);
        else
            node.Right = InsertImpl(node.Right, value);

        return node;
    }

    public IEnumerator<T> GetEnumerator() => PreOrderTraversal(Root);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerator<T> PreOrderTraversal(BinaryTreeNode<T>? node)
    {
        if (node == null)
            yield break;

        yield return node.Value;

        using (var leftEnum = PreOrderTraversal(node.Left))
        {
            while (leftEnum.MoveNext())
                yield return leftEnum.Current;
        }

        using (var rightEnum = PreOrderTraversal(node.Right))
        {
            while (rightEnum.MoveNext())
                yield return rightEnum.Current;
        }
    }
}

public class BinaryTreeNode<T> where T : class, IComparable<T>
{
    private BinaryTreeNode() { }
    public required T Value { get; set; }
    public BinaryTreeNode<T>? Left { get; set; }
    public BinaryTreeNode<T>? Right { get; set; }
    public static BinaryTreeNode<T> Create(T value) => new() { Value = value };
}
