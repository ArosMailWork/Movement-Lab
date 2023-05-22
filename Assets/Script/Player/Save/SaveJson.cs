using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;
//using UnityEngine.UI;

public class SaveJson : MonoBehaviour
{
    public void SaveToJson()
    {
        Stats data = new Stats();
        //data.Id = idInputField.text;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/WeaponDataFile.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/WeaponDataFile.json");
        Stats data = JsonUtility.FromJson<Stats>(json);

        //idInputField.text = data.Id;
    }
}