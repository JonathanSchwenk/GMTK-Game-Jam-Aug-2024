using UnityEngine;
using System;

public class SaveManager : MonoBehaviour, ISaveManager
{
    public SaveData saveData {get; set;}
    public Action<int>OnSave {get; set;}

    // Save the whole state of this saveState script to the player
    public void Save() {
        Debug.Log("Saving Data Now");
        PlayerPrefs.SetString("save", SaveHelper.Serialize<SaveData>(saveData));
        OnSave?.Invoke(1);
    }

    // Load the previous saved state from the player prefs
    public void Load() {
        if (PlayerPrefs.HasKey("save")) {
            Debug.Log("already saved");
            saveData = SaveHelper.Deserialize<SaveData>(PlayerPrefs.GetString("save"));
        } else {
            saveData = new SaveData();
            Debug.Log("No save file found, creating new one");
        }
    }

    public void DeleteSavedData() {
        if (PlayerPrefs.HasKey("save")) {
            Debug.Log("Key Found, Deleteing Now");
            PlayerPrefs.DeleteKey("save");
        }
    }
}
