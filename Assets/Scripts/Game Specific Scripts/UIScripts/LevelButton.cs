using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour
{
    [SerializeField] private int  LevelIndex;
    [SerializeField] TextMeshProUGUI LevelButtonInfo;
    [SerializeField] private Button Button;
    
    void Start()
    {
        LevelIndex = transform.GetSiblingIndex() + 1;
        LevelButtonInfo.text = "Level " + LevelIndex.ToString();
        
    }

  

    public void SetInteractable(bool state)
    {
        Button.interactable = state;
    }


    public void LoadSpesificLevel()
    {
        LevelIndex = transform.GetSiblingIndex();
        
        EventParam level_param = new EventParam(LevelIndex);
        EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.SPESIFIC_LEVEL, level_param);

    }
}
