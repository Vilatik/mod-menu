using UnityEngine;

public static class PlayerController
{
    public static void ApplyChanges(Mod plugin)
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        FirstPersonMovement movement = player.GetComponent<FirstPersonMovement>();
        if (movement != null)
        {
            movement.speed = plugin.PlayerSpeed;
        }

        Jump jump = player.GetComponent<Jump>();
        if (jump != null)
        {
            jump.jumpStrength = plugin.PlayerJump;
        }

        GameObject camera = GameObject.Find("First Person Camera");
        if (camera != null)
        {
            Zoom zoom = camera.GetComponent<Zoom>();
            if (zoom != null)
            {
                zoom.defaultFOV = plugin.FOV;
            }
        }

        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.fireballPrefab = plugin.TurtleMode ? plugin.Turtle : plugin.Fireball;
        }
    }
}
