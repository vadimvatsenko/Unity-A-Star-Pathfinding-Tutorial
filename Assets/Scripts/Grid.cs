using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // �������� �� ��c��� ������ �� ����� ������� ��������� � ������ ����� � ��������� (0,0,0)
    [SerializeField] Transform StartPosition; // ������� ������
    [SerializeField] LayerMask WallMask; // ����� ����� ������� ����� Wall � ����������
    [SerializeField] Vector2 GridWorldSize; // ������� ����� ����� 30 �� 30 
    [SerializeField] float NodeRadius; // ������ ������ ����
    [SerializeField] float Distance; // ��������� ����� ������

    private Node[,] GridArray; // ���������� ������ �� �����
    public List<Node> FinalPath; // ������ �� ����� ���������� ����

    private float NodeDiametr; // ������� ������ ����
    private int GridCountsX, GridCountsY; // ������� ����� ������ ����� 

    

    private void Start()
    {
        NodeDiametr = NodeRadius * 2; // ������� ������ ����, �� ������� ������� = ������ * 2
        GridCountsX = Mathf.RoundToInt(GridWorldSize.x / NodeDiametr); // ���������� ������ �� X. ����� ������ �� � / �� ������� ������(���� ������)        
        GridCountsY = Mathf.RoundToInt(GridWorldSize.y / NodeDiametr); // ���������� ������ �� Y. ����� ������ �� Y / �� ������� ������(���� ������)

        CreateGrid();
    }

    private void CreateGrid()
    {
        GridArray = new Node[GridCountsX, GridCountsY]; // �������������� ������

        Vector3 bottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2; // �������� ����� ������ ���������� ����
        // 1. transform.position(0,0,0) - (Vector3.right(1,0,0) * GridWorldSize.x(30) / 2) - Vector3.forward(0,0,1) * GridWorldSize.y(30) / 2 
        // 2. transform.position(0,0,0) - ��� ������� ����� �� ������, ��� = (0,0,0)
        // 3. Vector3.right(1,0,0) * GridWorldSize.x(30) / 2 = (15,0,0)
        // 4. ������������� ���� (0,0,0) - (15,0,0) = (-15,0,0)
        // 5. Vector3.forward(0,0,1) * GridWorldSize.y(30) / 2 = (0,0,15)
        // 6. �������� ���� ����� 4� �������� ����� 5  (-15,0,0) - (0,0,15) = (-15, 0, -15) 
        // 7. Vector3.right � Vector3.forward ��� ����, ����� �� ��������� � ������ �����������

        for (int x = 0; x < GridCountsX; x++) // ���������� � ��������� ���������� ��� ����� �����
        {
            for (int y = 0; y < GridCountsY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * NodeDiametr + NodeRadius) + Vector3.forward * (y * NodeDiametr + NodeRadius); // ��� ����������� ������� ����
                // 1. (-15,0,-15) + (1,0,0) * (i=0 * 1 + 0.5) + (0,0,1) * (y=0 * 1 + 0,5) = (-15,0,-15) + (0.5, 0, 0) + (0,0, 0.5) = (-14.5, 0, -14.5)
                // 2. (-15,0,-15) + (1,0,0) * (1=0 * 1 + 0.5) + (0,0,1) * (y=1 * 1 + 0,5) = (-15,0,-15) + (0.5, 0, 0) + (0,0, 0.5) = (-14.5, 0, -13.5)
                // 3 ���
                bool Wall = true;
               

                if (Physics.CheckSphere(worldPoint, NodeRadius, WallMask)) // �������� �� ������������ �� ����� �����, ���� ������� ���� � �������� �������� ��� ������ ������� ��������� � ���� WallMask
                {

                    Wall = false;
                }
                
                GridArray[x,y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition) // ��������������� ������� � ������� ����
    {
        float xPoint = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float yPoint = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((GridCountsX - 1) * xPoint);
        int y = Mathf.RoundToInt((GridCountsY - 1) * yPoint);

        return GridArray[x, y];
    }

    //OLD
    /*public List<Node> GetNeighboringNodes(Node currentNode) 
    {
        List<Node> neighboringNodes = new List<Node>();
        int xCheck, yCheck;

        //RightSide
        xCheck = currentNode.GridX + 1;
        yCheck = currentNode.GridY;
        if (xCheck >= 0 && xCheck < GridCountsX)
        {
            if (yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }

        //LeftSide
        xCheck = currentNode.GridX - 1;
        yCheck = currentNode.GridY;
        if (xCheck >= 0 && xCheck < GridCountsX)
        {
            if (yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }

        //TopSide
        xCheck = currentNode.GridX;
        yCheck = currentNode.GridY + 1;
        if (xCheck >= 0 && xCheck < GridCountsX)
        {
            if (yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }

        //BottomSide
        xCheck = currentNode.GridX;
        yCheck = currentNode.GridY - 1;
        if (xCheck >= 0 && xCheck < GridCountsX)
        {
            if (yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }

        return neighboringNodes;
    }*/

    public List<Node> GetNeighboringNodes(Node currentNode)
    {
        List<Node> neighboringNodes = new List<Node>();
        int xCheck, yCheck;

        // Directions for straight neighbors (right, left, top, bottom)
        int[,] straightDirections = new int[,]
        {
        { 1, 0 },  // Right
        { -1, 0 }, // Left
        { 0, 1 },  // Top
        { 0, -1 }  // Bottom
        };

        // Directions for diagonal neighbors
        int[,] diagonalDirections = new int[,]
        {
        { 1, 1 },   // Top-Right
        { 1, -1 },  // Bottom-Right
        { -1, 1 },  // Top-Left
        { -1, -1 }  // Bottom-Left
        };

        // Check straight neighbors
        for (int i = 0; i < straightDirections.GetLength(0); i++)
        {
            xCheck = currentNode.GridX + straightDirections[i, 0];
            yCheck = currentNode.GridY + straightDirections[i, 1];

            if (xCheck >= 0 && xCheck < GridCountsX && yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }

        // Check diagonal neighbors
        for (int i = 0; i < diagonalDirections.GetLength(0); i++)
        {
            xCheck = currentNode.GridX + diagonalDirections[i, 0];
            yCheck = currentNode.GridY + diagonalDirections[i, 1];

            if (xCheck >= 0 && xCheck < GridCountsX && yCheck >= 0 && yCheck < GridCountsY)
            {
                neighboringNodes.Add(GridArray[xCheck, yCheck]);
            }
        }
        return neighboringNodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y)); // ������ ���� � ��������� �����
        if(GridArray != null)
        {
            foreach (Node node in GridArray)
            {
                if (node.IsWall)
                {
                    Gizmos.color = Color.white;
                } else
                {
                    Gizmos.color = Color.yellow;
                }

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }
                    
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (NodeDiametr - Distance)); // �������� ���, ������� ����, ������ ���� (1,1,1) * (������� - ��������� ����� ������)
            }
        }
    }    
}
