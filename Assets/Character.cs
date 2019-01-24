using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float health = 1;
    public float food = 1;
    public float water = 1;
    public float sleep = 1;

    public int turnsSinceSlept = 0;

    public LevelManager.Actions action;

    public List<Sprite> sprites;


    public void Randomize()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnMouseDown()
    {
        if (health > 0)
        {
            GameObject.FindObjectOfType<LevelManager>().CharacterAction(this);
        }
    }
}
