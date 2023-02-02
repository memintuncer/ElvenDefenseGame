using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private Dictionary<string, Action<EventParam>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<EventParam>>();
        }
    }

    public static void StartListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
           
            thisEvent += listener;

            
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<EventParam> listener)
    {
        if (eventManager == null) return;
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            
            thisEvent -= listener;

           
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            
        }
    }
}


public class EventParam
{
        
   
    public int HealthReduction;
    public Dictionary<string, object> paramDictionary;

    private Monster MonsterParam;
    private float MonsterDamage;
    private Monster Killed_Monster;
    private int SpecificLevelIndex;
    private GameData GameData;
    
    public GameData GetGameData()
    {
        return GameData;
    }

    

    public int GetSpecificLevelIndex()
    {
        return SpecificLevelIndex;
    }

    public void SetSpecificLevelIndex(int spesific_level_index)
    {
        SpecificLevelIndex = spesific_level_index;
    }

    public Monster GetKilledMonster()
    {
        return Killed_Monster;
    }
    public Monster GetMonsterParam()
    {
        return MonsterParam;
    }

    public float GetMonsterDamage()
    {
        return MonsterDamage;
    }


    public EventParam()
    {

    }
      public EventParam(int level_index)
    {
        SpecificLevelIndex = level_index;
    }

    public EventParam(Monster monster,float monster_damage)
    {
        MonsterParam = monster;
        MonsterDamage = monster_damage;
    }

    public EventParam(Monster killed_monster)
    {
        Killed_Monster = killed_monster;
    }

    public EventParam(Dictionary<string, object> paramDictionary)
    {
        this.paramDictionary = paramDictionary;
    }

    public EventParam(GameData game_data)
    {
        this.GameData = game_data;
    }
   
}



