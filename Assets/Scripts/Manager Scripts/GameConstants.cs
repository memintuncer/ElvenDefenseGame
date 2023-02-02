using UnityEngine;

public class GameConstants
{
  

    public struct GameEvents
    {
        
        public static string GAME_STARTED = "GAME_STARTED";
        public static string MONSTER_REACHED_TO_CASTLE = "MONSTER_REACHED_TO_CASTLE";
        public static string CHANGE_ENEMY_TARGET = "CHANGE_ENEMY_TARGET";
        public static string KILLED_MONSTER = "KILLED_MONSTER";
        public static string ENEMY_REACHED_TO_CASTLE = "ENEMY_REACHED_TO_CASTLE";
        public static string CASTLE_DESTROYED = "CASTLE_DESTROYED";
        public static string SURVIVAL_MODE = "SURVIVAL_MODE";
        public static string RETURN_TO_TITLE_SCREEN = "RETURN_TO_TITLE_SCREEN";
        public static string NEW_WAVE = "NEW_WAVE";



    }

    public struct LEVEL_EVENTS
    {
        public static string OBJECTIVE_COMPLETED = "OBJECTIVE_COMPLETED";
        public static string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
        public static string LEVEL_COMPLETED = "LEVEL_COMPLETED";
        public static string LEVEL_FAILED = "LEVEL_FAILED";
        public static string LEVEL_SUCCESSED = "LEVEL_SUCCESSED";
        public static string LEVEL_STARTED = "LEVEL_STARTED";
        public static string LEVEL_FINISHED = "LEVEL_FINISHED";
        public static string RESTART_LEVEL = "RESTART_LEVEL";
        public static string NEXT_LEVEL = "NEXT_LEVEL";
        public static string SAME_LEVEL = "SAME_LEVEL";
        public static string SPESIFIC_LEVEL = "SPESIFIC_LEVEL";

    }


    public struct SAVE_LOAD_EVENTS
    {
        //Saving
        public static string SAVE_GAME_BUTTON_CLICKED = "SAVE_GAME_BUTTON_CLICKED";
        public static string SAVE_GAME = "SAVE_GAME";
        public static string MONSTERS_SAVED_TO_POOL = "MONSTERS_SAVED_TO_POOL";
        public static string GAME_MANAGER_DATA_SAVED = "GAME_MANAGER_DATA_SAVED";
        public static string DEFENDER_GENERATOR_DATA_SAVED = "DEFENDER_GENERATOR_DATA_SAVED";

        //Loading
        public static string START_LOADING_SAVED_GAME = "START_LOADING_SAVED_GAME";
        public static string LOAD_POOL_DATA = "LOAD_POOL_DATA";
        public static string LOAD_SURVIVAL_LEVEL = "LOAD_SURVIVAL_LEVEL";
        public static string LOAD_GAME_MANAGER_DATA = "LOAD_GAME_MANAGER_DATA";
        public static string LOAD_DEFENDER_GENERATOR_DATA = "LOAD_DEFENDER_GENERATOR_DATA";

    }
  
}