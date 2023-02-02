using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToTitleScreenButton : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    public void ReturnToMainScreen()
    {
        List<Monster> target_monsters = GameManager.GetTargetMonsters();
        foreach (Monster monster in target_monsters)
        {
            monster.SendMonsterToPool();
        }
        EventManager.TriggerEvent(GameConstants.GameEvents.RETURN_TO_TITLE_SCREEN, new EventParam());
    }
}
