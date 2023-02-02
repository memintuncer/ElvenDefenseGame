using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSurvivalModButton : MonoBehaviour
{
    public void LoadingSurvivalMod()
    {
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.START_LOADING_SAVED_GAME,new EventParam());
    }
}
