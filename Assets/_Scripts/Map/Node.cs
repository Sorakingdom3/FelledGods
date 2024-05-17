using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Completed = false;

    public int Row;
    public int Column;
    public Vector2 Position;
    public Enums.NodeType Type;
    public List<Node> Parents { get; set; }
    public List<Node> Children { get; set; }

    public Node(int row, int column)
    {
        Row = row;
        Column = column;
        Children = new List<Node>();
        Parents = new List<Node>();
        Type = Enums.NodeType.Undefined;
    }

    // Método para añadir un hijo al nodo actual
    public void AddChild(Node child)
    {
        Children.Add(child);
        child.Parents.Add(this);
    }

    public override string ToString()
    {
        string result = $"{Column} ({Type})";
        return result;
    }
}