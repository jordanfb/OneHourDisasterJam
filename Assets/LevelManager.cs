using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Text resources;
    public Text turnCounterText;

    public List<Character> characters;

    public CharacterChoiceMenu characterUI;

    public int wood = 10;
    public int water = 100;
    public int food = 100;
    public float fire = 1;

    public GameObject deathPanel;
    public Text deathText;

    public int numTurnsSurvived = 0;

    public int numCharacters = 0;

    public List<GameObject> characterPrefabs;

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        numTurnsSurvived = 0;
        wood = 10;
        water = 100;
        food = 100;
        fire = 1;
        foreach(Character c in characters)
        {
            Destroy(c.gameObject);
        }
        deathPanel.SetActive(false);
        characters = new List<Character>();
        numCharacters = Random.Range(1, 5);
        for (int i = 0; i < numCharacters; i++)
        {
            GameObject go = Instantiate(characterPrefabs[Random.Range(0, characterPrefabs.Count - 1)]);
            Character chara = go.GetComponent<Character>();
            chara.Randomize();
            characters.Add(chara);
            switch(i)
            {
                case 1:
                    chara.transform.position = Vector2.up * 3;
                    break;
                case 2:
                    chara.transform.position = Vector2.right * 3;
                    break;
                case 3:
                    chara.transform.position = Vector2.down * 3;
                    break;
                case 0:
                    chara.transform.position = Vector2.left * 3;
                    break;
            }
        }
        DisplayResrouces();
    }

    public void DisplayResrouces()
    {
        string fireOn = (fire == 1) ? "buring " : " off";
        string t = "Resources: \nWater: " + water + "\nFood: " + food + "\nFire: " + fireOn + "\n Wood: " + wood;
        resources.text = t;

        turnCounterText.text = "Turn " + numTurnsSurvived;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !deathPanel.activeInHierarchy && !characterUI.gameObject.activeInHierarchy)
        {
            TakeTurn();
        }
    }

    public void TakeTurn()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].health <= 0)
            {
                continue;
            }
            // change their stats
            float deltaHealth = -.05f;
            if (fire == 0)
            {
                deltaHealth += .25f;
            }
            characters[i].sleep -= .125f;
            if (characters[i].sleep < 0)
            {
                deltaHealth -= characters[i].sleep;
                characters[i].sleep = 0;
            }
            characters[i].food -= .04f;
            if (characters[i].food < 0)
            {
                deltaHealth -= characters[i].food;
                characters[i].food = 0;
            }
            characters[i].water -= .125f;
            if (characters[i].water < 0)
            {
                deltaHealth -= characters[i].water;
                characters[i].water = 0;
            }
            characters[i].health -= deltaHealth;
            characters[i].health = Mathf.Clamp(characters[i].health, 0, 1);
        }

        for (int i = 0; i < characters.Count; i++)
        {
            TakeAction(characters[i]);
        }
        if (wood <= 0)
        {
            fire = 0;
        }
        else if (wood > 0)
        {
            wood -= 1;
            fire = 1;
        }

        bool survived = false;
        for (int i = 0; i < characters.Count; i++)
        {
            // change their stats
            if (characters[i].health > 0)
            {
                survived = true;
                numTurnsSurvived++;
                break;
            } else
            {
                characters[i].Die();
            }
        }
        if (!survived)
        {
            EndGame();
        }
        foreach(Character c in characters)
        {
            c.gameObject.SetActive(true);
        }
        DisplayResrouces();
    }

    public void EndGame()
    {
        deathPanel.SetActive(true);
        deathText.text = "YOU SURVIVED " + numTurnsSurvived + " TURNS";
        int highScore = 0;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        if (numTurnsSurvived > highScore)
        {
            PlayerPrefs.SetInt("HighScore", numTurnsSurvived);
            deathText.text += "\nNEW HIGH SCORE!";
        } else
        {
            deathText.text += "\nHigh Score: " + highScore;
        }
    }

    public void CharacterAction(Character c)
    {
        characterUI.SetCharacter(c);
    }

    private void TakeAction(Character c)
    {
        if (c.health <= 0)
        {
            return;
        }
        switch (c.action)
        {
            case Actions.GatherFood:
                food += Random.Range(0, 4);
                break;
            case Actions.GatherWater:
                water += Random.Range(0, 8);
                break;
            case Actions.GatherWood:
                wood += Random.Range(2, 5);
                break;
            case Actions.Sleep:
                c.sleep = Mathf.Clamp(c.sleep + .5f, 0, 1);
                break;
            case Actions.Eat:
                if (water >= 1)
                {
                    float effect = (water >= 4) ? 1 : (float)water / 4;
                    c.water = Mathf.Clamp(c.water + 1*effect, 0, 1.5f);
                    water -= 4;
                }
                if (food >= 1)
                {
                    c.food = Mathf.Clamp(c.food + .1f, 0, 1.5f);
                    food -= 1;
                }
                break;
            default:
                break;
        }
    }

    public enum Actions
    {
        Sleep, GatherFood, GatherWater, GatherWood, Eat,
    }
}
