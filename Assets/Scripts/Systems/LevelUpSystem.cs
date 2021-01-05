using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{

    public int currentLevel;
    public int baseXP = 20;
    public int currentXP;

    public int xpForNextLevel;
    public int xpDifferenceToNextLevel;
    public int totalXpDifference;

    public float fillAmount;
    public float reverseFillAmount;

    public int statPoints;
    public int skillPoints;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AddXP", 1f, 1f);
    }

    public void AddXP()
    {
        CalculateLevel(5);
    }

    void CalculateLevel(int amount)
    {
        currentXP += amount;

        int temp_cur_level = (int)Mathf.Sqrt(currentXP / baseXP);

        if(currentLevel != temp_cur_level)
        {
            currentLevel = temp_cur_level;
        }

        xpForNextLevel = baseXP * (currentLevel + 1) * (currentLevel + 1);
        xpDifferenceToNextLevel = xpForNextLevel - currentXP;
        totalXpDifference = xpForNextLevel - (baseXP * currentLevel * currentLevel);

        fillAmount = (float)xpDifferenceToNextLevel / (float)totalXpDifference;
        reverseFillAmount = 1 - fillAmount;

        statPoints = 5 * currentLevel;
        skillPoints = 15 * currentLevel;
    }
}
