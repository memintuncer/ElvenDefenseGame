using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileDataHandler
{
    private string SaveDataFileDirection = "";
    private string SaveDataFileName = "";
   

    public FileDataHandler(string save_data_file_direction, string save_data_file_name)
    {
        this.SaveDataFileDirection = save_data_file_direction;
        this.SaveDataFileName = save_data_file_name;
    }

    public GameData LoadSaveData()
    {
        
        string full_path = Path.Combine(SaveDataFileDirection, SaveDataFileName);
        GameData loaded_data = null;
        if (File.Exists(full_path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(full_path, FileMode.Open);

            loaded_data = formatter.Deserialize(stream) as GameData;
            stream.Close();
        }
        
       return loaded_data;
    }

    public void SaveGameData(GameData game_data)
    {
        int x = 5;
        string full_path = Path.Combine(SaveDataFileDirection,SaveDataFileName);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(full_path, FileMode.Create);
        formatter.Serialize(stream, game_data);
        stream.Close();
    }

}
