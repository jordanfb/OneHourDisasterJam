using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoiceMenu : MonoBehaviour {

    public Character c;
    public Text characterStatsText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void SetCharacter(Character cha)
    {
        c = cha;
        gameObject.SetActive(true);
        string t = "Character Stats:\nHealth: " + c.health + "\nFood: " + c.food + "\n Water: " + c.water + "\nSleep: " + c.sleep;
        characterStatsText.text = t;
    }

    public void SetCharacterChoice(LevelManager.Actions action)
    {
        c.action = action;
        gameObject.SetActive(false);
        c.gameObject.SetActive(false);
    }

    public void SetFood()
    {
        c.action = LevelManager.Actions.Eat;
        c.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GatherWood()
    {
        c.action = LevelManager.Actions.GatherWood;
        c.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void GetFood()
    {
        c.action = LevelManager.Actions.GatherFood;
        c.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void waterFood()
    {
        c.action = LevelManager.Actions.GatherWater;
        c.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void sleep()
    {
        c.action = LevelManager.Actions.Sleep;
        c.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
