using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid GridScript;
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        GridScript = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node StartNode = GridScript.NodeFromWorldPosition(startPosition);
        Node TargetNode = GridScript.NodeFromWorldPosition(targetPosition);

        List<Node> OpenList = new List<Node>() { StartNode };
        HashSet<Node> ClosedList = new HashSet<Node>(); // Хеш-таблицы предоставляют коллекцию, которая не допускает дубликатов и обеспечивает быстрый доступ к элементам.

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].HCost < CurrentNode.HCost)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if(CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach(Node NeighborNode in GridScript.GetNeighboringNodes(CurrentNode)) 
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode)) continue;

                int moveCost = CurrentNode.GCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if(moveCost < NeighborNode.GCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.GCost = moveCost;
                    NeighborNode.HCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private void GetFinalPath(Node startNode, Node targetNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = targetNode;

        while (CurrentNode != startNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();

        GridScript.FinalPath = FinalPath;        
    }

    private int GetManhattenDistance(Node currentNode, Node neighborNode) // Расстояние городских кварталов
    {
        int ix = Mathf.Abs(currentNode.GridX - neighborNode.GridX);
        int iy = Mathf.Abs(currentNode.GridY - neighborNode.GridY);

        return ix + iy;
    }
}
