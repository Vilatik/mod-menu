using System;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private Mod _plugin;
    private Rect _windowRect = new Rect(20, 20, 400, 300);
    private int _currentLayout = 0;
    private string[] _layoutNames = { "Player", "Camera", "Weapon" };


    public void Initialize(Mod plugin)
    {
        _plugin = plugin;

    }

    private void Update()
    {
        if (_plugin == null) return;

        if (_plugin.Fireball == null || _plugin.Turtle == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript != null && _plugin.Fireball == null)
                {
                    _plugin.Fireball = playerScript.fireballPrefab;
                }

                if (_plugin.Turtle == null)
                {
                    _plugin.Turtle = GameObject.Find("TurtleShell");
                    
                    _plugin.Turtle.transform.position = new Vector3(0,0, 0);
                }
            }
        }

        if (BepInEx.UnityInput.Current.GetKeyDown(KeyCode.F5))
        {
            _plugin.ShowWindow = !_plugin.ShowWindow;
            Cursor.lockState = _plugin.ShowWindow ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _plugin.ShowWindow;
        }
    }



    Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = color;
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(pix);
        tex.Apply();
        return tex;
    }
    private void OnGUI()
    {
        GUI.skin.box.normal.background = MakeTex(2, 2, Color.black);
        GUI.skin.button.normal.background = MakeTex(2, 2, new Color(49f / 255f, 89f / 255f, 139f / 255f));

        GUI.skin.button.hover.background = MakeTex(2, 2, new Color(67f / 255f, 122f / 255f, 190f / 255f));
        GUI.skin.button.active.background = MakeTex(2, 2, new Color(67f / 255f, 122f / 255f, 190f / 255f));
        GUI.backgroundColor = Color.black;
        
        GUI.skin.button.normal.textColor = Color.white;
        GUI.skin.label.normal.textColor = Color.white;

        if (!_plugin.ShowWindow) return;
        _windowRect = GUI.Window(0, _windowRect, DrawWindow, "Mod Menu");
    }

    private void DrawWindow(int windowId)
    {
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical(GUILayout.Width(150));
        for (int i = 0; i < _layoutNames.Length; i++)
        {
            if (GUILayout.Button(_layoutNames[i]))
            {
                _currentLayout = i;
            }
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        DrawCurrentLayout();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        PlayerController.ApplyChanges(_plugin);
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private void DrawCurrentLayout()
    {
        switch (_currentLayout)
        {
            case 0: DrawPlayerLayout(); break;
            case 1: DrawCameraLayout(); break;
            case 2: DrawWeaponLayout(); break;
            default: GUILayout.Label("Unknown Layout"); break;
        }
    }

    private void DrawPlayerLayout()
    {
        GUILayout.Label($"Speed: {_plugin.PlayerSpeed:F1}");
        _plugin.PlayerSpeed = GUILayout.HorizontalSlider(_plugin.PlayerSpeed, 1f, 64f);

        GUILayout.Label($"Jump Power: {_plugin.PlayerJump:F1}");
        _plugin.PlayerJump = GUILayout.HorizontalSlider(_plugin.PlayerJump, 1f, 32f);
        _plugin.isFlying = GUILayout.Toggle(_plugin.isFlying, "Fly");
        if (_plugin.isFlying)
        {
            GUILayout.Label($"Fly speed: {_plugin.flySpeed:F1}");
            _plugin.flySpeed = GUILayout.HorizontalSlider(_plugin.flySpeed, 1f, 10f);
        }
    }
    private void DrawCameraLayout()
    {
        GUILayout.Label($"FOV: {_plugin.FOV:F1}");
        _plugin.FOV = GUILayout.HorizontalSlider(_plugin.FOV, 30f, 120f);
    }
    private void DrawWeaponLayout()
    {
        _plugin.fastShooting = GUILayout.Toggle(_plugin.fastShooting, "Fast fire");
        if (_plugin.fastShooting)
        {
            GUILayout.Label($"Firerate: {_plugin.fireRate:F4}");
            _plugin.fireRate = GUILayout.HorizontalSlider(_plugin.fireRate, 0.0005f, 0.1f);
        }

        _plugin.TurtleMode = GUILayout.Toggle(_plugin.TurtleMode, "Use Turtles");
    }

}
