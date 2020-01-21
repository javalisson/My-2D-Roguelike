using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public float turnDelay = 0.1f;
  public static GameManager instance = null;
  public BoardManager boardScript;
  public int playerFoodPoints = 100;
  [HideInInspector] public bool playersTurn = true;

  private int level = 3;
  private List<Enemy> enemies;
  private bool enemiesMoving;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
    enemies = new List<Enemy>();
    boardScript = GetComponent<BoardManager>();
    InitGame();
  }

  //this is called only once, and the paramter tell it to be called only after the scene was loaded
  //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  static public void CallbackInitialization()
  {
    //register the callback to be called everytime the scene is loaded
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  //This is called each time a scene is loaded.
  static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
  {
    instance.level++;
    instance.InitGame();
  }

  void InitGame()
  {
    enemies.Clear();
    boardScript.SetupScene(level);
  }

  public void GameOver()
  {
    enabled = false;
  }

  void Update()
  {
    if (playersTurn || enemiesMoving)
    {
      return;
    }

    StartCoroutine(MoveEnemies());
  }

  public void AddEnemyToList(Enemy script)
  {
    enemies.Add(script);
  }

  IEnumerator MoveEnemies()
  {
    enemiesMoving = true;
    yield return new WaitForSeconds(turnDelay);
    if (enemies.Count == 0)
    {
      yield return new WaitForSeconds(turnDelay);
    }

    for (int i = 0; i < enemies.Count; i++)
    {
      enemies[i].MoveEnemy();
      yield return new WaitForSeconds(enemies[i].moveTime);
    }

    playersTurn = true;
    enemiesMoving = false;
  }
}
