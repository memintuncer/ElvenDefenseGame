using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelSpecificTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentGoldText, DefenderRequiredGoldText,LevelInfoText,KilledMonsterCountText;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameLevel GameLevel;
    [SerializeField] private DefenderGenerator DefenderGenerator;


    private void Start()
    {
        
        SetLevelNumberText();
    }

    void Update()
    {
        CurrentGoldText.text = GameManager.GetCurrentGold().ToString();
        DefenderRequiredGoldText.text = DefenderGenerator.GetRequiredGold().ToString();
        KilledMonsterCountText.text = GameManager.GetKilledEnemyCount().ToString();
    }


    void SetLevelNumberText()
    {
        bool is_survival_mode = GameManager.GetIsSurvivalMode();

        switch (is_survival_mode)
        {
            case true:
                LevelInfoText.text = "Survival Mod";
                break;
            case false:
                LevelInfoText.text = "Level " + GameLevel.GetLevelIndex().ToString();
                break;
        }
    }
}
