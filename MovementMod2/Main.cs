using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("com.vilatik", "Mod Menu", "1.0.0")]
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

    private Harmony _harmony;
    private IInputSystem _input;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError("An instance of Mod already exists!");
            return;
        }



        Instance = this;
        _harmony = new Harmony("com.vilatik");
        _harmony.PatchAll();

        _input = BepInEx.UnityInput.Current;
        if (_input == null)
        {
            Logger.LogError("BepInEx Input system failed to initialize.");
        }


        GameObject guiManager = new GameObject("GUI_Manager");
        guiManager.AddComponent<GUIManager>().Initialize(this);
        GameObject.DontDestroyOnLoad(guiManager);
    }
}
