using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameButton : MonoBehaviour
{
    public void SaveGame()
    {
        GameData temp_game_data = new GameData();
        EventParam save_data_event_param = new EventParam(temp_game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.SAVE_GAME_BUTTON_CLICKED,save_data_event_param);
    }
}
