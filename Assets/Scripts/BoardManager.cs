using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count (int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public int Columns = 8;
    public int Rows = 8;
    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();


    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);
        LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);

        // Determine the enemies based on level
        int enemyCount = (int)Math.Log(level, 2f);
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);

        // Create the exit which is always in the top right of the board
        Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
    }


    private void InitialiseList()
    {
        _gridPositions.Clear();
        for (int x = 1; x < Columns - 1; x++)
        {
            for (int y = 1; y < Rows - 1; y++)
            {
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Provide outerwall and floor tiles
    private void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < Columns + 1; x++)
        {
            for (int y = -1; y < Rows + 1; y++)
            {
                GameObject prefabTile = FloorTiles[Random.Range(0, FloorTiles.Length)];

                // If outer ring, then override with outer wall tile
                if (IsOuterRing(x, y))
                    prefabTile = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];

                GameObject instance = Instantiate(prefabTile, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(_boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPostion = _gridPositions[randomIndex];

        // Make sure the position is no longer available, as now full
        _gridPositions.RemoveAt(randomIndex);

        return randomPostion;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int x = 0; x < objectCount; x++) {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    private bool IsOuterRing(int x, int y)
    {
        if (x == -1)
            return true;

        if (x == Columns)
            return true;

        if (y == -1)
            return true;

        if (y == Rows)
            return true;

        return false;
    }
}
