using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float LevelStartDelay = 2;
    public float TurnDelay = 0.1f;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;

    [HideInInspector]
    public bool PlayersTurn = true;

    private Text _levelText;
    private GameObject _levelImage;
    private int _level = 1;
    private List<Enemy> _enemies;
    private bool _enemiesMoving;
    private bool _doingSetup;

    public void GameOver()
    {
        _levelText.text = "After " + _level + " days, you starved.";
        _levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        _enemies.Add(script);
    }

    private void Awake()
    {
        // Ensure that the GameManager can only be a singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Ensure the GameManager persists inbetween scenes
        DontDestroyOnLoad(gameObject);

        BoardScript = GetComponent<BoardManager>();
        _enemies = new List<Enemy>();
        InitGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayersTurn || _enemiesMoving || _doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    private void InitGame()
    {
        _doingSetup = true;

        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.text = "Day " + _level;
        _levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        _enemies.Clear();
        BoardScript.SetupScene(_level);

        _doingSetup = false;
    }

    private IEnumerator MoveEnemies()
    {
        _enemiesMoving = true;
        yield return new WaitForSeconds(TurnDelay);

        if (_enemies.Count == 0)
            yield return new WaitForSeconds(TurnDelay);

        foreach (Enemy enemy in _enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.MoveTime);
        }

        PlayersTurn = true;
        _enemiesMoving = false;
    }

    private void OnLevelWasLoaded(int index)
    {
        _level++;
        InitGame();
    }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _doingSetup = false;
    }
}