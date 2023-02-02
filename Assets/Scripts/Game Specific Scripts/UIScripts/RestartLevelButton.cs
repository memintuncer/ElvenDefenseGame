using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevelButton : MonoBehaviour
{

    [SerializeField] GameManager GameManager;
    public void RestartLevel()
    {
        List<Monster> target_monsters = GameManager.GetTargetMonsters();
        foreach(Monster monster in target_monsters)
        {
            monster.SendMonsterToPool();
        }
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.SAME_LEVEL, new EventParam());
        
    }
}
