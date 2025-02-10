using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("your.unique.guid", "My GUI Mod", "1.0.0")]
public class Mod : BaseUnityPlugin
{
    public static Mod Instance { get; private set; }
    public bool ShowWindow { get; set; } = false;
    public float PlayerSpeed { get; set; } = 5f;
    public float PlayerJump { get; set; } = 2f;
    public float FOV { get; set; } = 60f;
    public bool TurtleMode { get; set; } = false;
    public GameObject Fireball { get; set; }
    public GameObject Turtle { get; set; }
    public bool fastShooting { get; set; } = false;
    public float fireRate { get; set; } = 0.1f;

    public bool isFlying = false;
    public float flySpeed = 1f;

    public bool isESP = true;

    private Harmony _harmony;
    private IInputSystem _input;

    //private ESP esp;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError("An instance of Mod already exists!");
            return;
        }

        

        Instance = this;
        _harmony = new Harmony("com.your.guimod");
        _harmony.PatchAll();

        _input = BepInEx.UnityInput.Current;
        if (_input == null)
        {
            Logger.LogError("BepInEx Input system failed to initialize.");
        }


        GameObject guiManager = new GameObject("GUI_Manager");
        guiManager.AddComponent<GUIManager>().Initialize(this);
        GameObject.DontDestroyOnLoad(guiManager);
        //if (esp == null)
        //{
        //    esp = new ESP();
        //    Logger.LogInfo("ESP plugin loaded!");
        //}

        //// Add initial object types
        //esp.AddObjectName("Coin", Color.green);

    }
    //void Update()
    //{
    //    List<string> objectTypes = esp.GetObjectNames();
    //    esp.UpdateObjects(objectTypes);
    //    esp.RenderESP();
    //}
}
