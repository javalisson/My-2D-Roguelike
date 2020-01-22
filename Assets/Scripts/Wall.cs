using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
  public Sprite dmgSprite;
  public int hp = 4;

  public AudioClip chopAudio1;
  public AudioClip chopAudio2;

  private SpriteRenderer spriteRenderer;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void DamageWall(int loss)
  {
    spriteRenderer.sprite = dmgSprite;
    hp -= loss;
    SoundManager.instance.RandomizeSfx(chopAudio1, chopAudio2);
    if (hp <= 0){
      gameObject.SetActive(false);
    }
  }
}
