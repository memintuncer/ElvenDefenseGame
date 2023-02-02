using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
   public void LoadNextLevel()
    {
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.NEXT_LEVEL, new EventParam());
    }
}
