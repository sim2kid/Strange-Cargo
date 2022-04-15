using Newtonsoft.Json;
using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class Settings : MonoBehaviour
{
    private string SettingsLocation;

    public static Settings Instance { get; private set; }
    [SerializeField]
    public SettingsData Values;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SettingsLocation = Utility.PathUtil.SanitizePath($"{Application.persistentDataPath}/Settings.json");
            Values = new SettingsData();
            LoadSettings();
        }
        else 
        {
            Console.LogError("Only one Settings object allowed in the scene. Deleting the extra one.");
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        if (!File.Exists(SettingsLocation))
        { 
            File.Create(SettingsLocation);
        }
        string json = JsonConvert.SerializeObject(Values, Formatting.Indented);
        File.WriteAllText(SettingsLocation, json);
    }

    public void LoadSettings()
    {
        if (!File.Exists(SettingsLocation))
        {
            SaveSettings();
        }
        string json = File.ReadAllText(SettingsLocation);
        Values = JsonConvert.DeserializeObject<SettingsData>(json);
    }
}