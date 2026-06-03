using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TerrainTools;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _cellSize = 3f;

    [SerializeField]
    private GameObject _endFlag;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {

                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x * _cellSize, 0, z * _cellSize), Quaternion.identity);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        MazeCell endCell = FindFarthestCell(_mazeGrid[0, 0]);
        SpawnEnd(endCell);

        if (_player != null)
        {
            _player.position = new Vector3(0, 1, 0);
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = Mathf.RoundToInt(currentCell.transform.position.x / _cellSize);  // ← divide
        int z = Mathf.RoundToInt(currentCell.transform.position.z / _cellSize);  // ← divide

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (cellToRight.IsVisited == false)
                yield return cellToRight;
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited == false)
                yield return cellToLeft;
        }
        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (cellToFront.IsVisited == false)
                yield return cellToFront;
        }
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (cellToBack.IsVisited == false)
                yield return cellToBack;
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }


    private MazeCell FindFarthestCell(MazeCell start)
    {
        var visited = new HashSet<MazeCell>();
        var queue = new Queue<(MazeCell cell ,int dist)>();

        queue.Enqueue((start, 0));
        visited.Add(start);

        MazeCell farthest = start;
        int maxDist = 0;

        while(queue.Count > 0)
        {
            var (current, dist) = queue.Dequeue();

            if(dist > maxDist)
            {
                maxDist = dist;
                farthest = current;
            }

            foreach(MazeCell neighbour in GetConnectedNeighbours(current))
            {
                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue((neighbour, dist + 1));
                }
            }
        }
        return farthest;
    }

    private IEnumerable<MazeCell> GetConnectedNeighbours(MazeCell cell)
    {
        int x = Mathf.RoundToInt(cell.transform.position.x / _cellSize);
        int z = Mathf.RoundToInt(cell.transform.position.z / _cellSize);

        if (x + 1 < _mazeWidth && !cell.hasRightWall)
            yield return _mazeGrid[x + 1, z];

        if (x - 1 >= 0 && !cell.hasLeftWall)
            yield return _mazeGrid[x - 1, z];

        if (z + 1 < _mazeDepth && !cell.hasFrontWall)
            yield return _mazeGrid[x, z + 1];

        if (z - 1 >= 0 && !cell.hasBackWall)
            yield return _mazeGrid[x, z - 1];

    }

    private void SpawnEnd(MazeCell cell)
    {
        if (_endFlag != null)
        {
            GameObject end = Instantiate(_endFlag, cell.transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("End spawned at: " + end.transform.position);
        }
    }

}

