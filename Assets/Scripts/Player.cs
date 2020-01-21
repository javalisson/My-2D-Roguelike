using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
  public int wallDamage = 1;
  public int pointsPerFood = 10;
  public int pointsPerSoda = 20;
  public float restartLevelDelay = 1f;

  private Animator animator;
  private int food;

  protected override void Start()
  {
    animator = GetComponent<Animator>();

    food = GameManager.instance.playerFoodPoints;
    base.Start();
  }

  public void OnDisable()
  {
    GameManager.instance.playerFoodPoints = food;
  }

  void Update()
  {
    if (!GameManager.instance.playersTurn)
    {
      return;
    }

    int horizontal = 0,
        vertical = 0;

    horizontal = (int)Input.GetAxisRaw("Horizontal");
    vertical = (int)Input.GetAxisRaw("Vertical");

    if (horizontal != 0) vertical = 0;

    if (horizontal != 0 || vertical != 0)
    {
      AttemptMove<Wall>(horizontal, vertical);
    }

  }

  protected override void AttemptMove<T>(int xDir, int yDir)
  {
    food--;
    base.AttemptMove<T>(xDir, yDir);

    // RaycastHit2D hit2D;

    CheckIfGameOver();

    GameManager.instance.playersTurn = false;

  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Exit")
    {
      Invoke("Restart", restartLevelDelay);
      enabled = false;
    }
    else if (other.tag == "Food")
    {
      food += pointsPerFood;
      other.gameObject.SetActive(false);
    }
    else if (other.tag == "Soda")
    {
      food += pointsPerSoda;
      other.gameObject.SetActive(false);
    }
  }

  protected override void OnCantMove<T>(T component)
  {
    Wall hitWall = component as Wall;
    hitWall.DamageWall(wallDamage);
    animator.SetTrigger("PlayerChop");
  }

  private void Restart()
  {
    Debug.Log(SceneManager.GetActiveScene().name);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
  }

  public void LoseFood(int loss)
  {
    animator.SetTrigger("PlayerHit");
    food -= loss;
    CheckIfGameOver();
  }

  private void CheckIfGameOver()
  {
    if (food <= 0)
    {
      GameManager.instance.GameOver();
    }
  }


}
