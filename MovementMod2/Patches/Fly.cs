using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(FirstPersonMovement), "FixedUpdate")]
public class FlyPatch
{
    static bool Prefix(FirstPersonMovement __instance)
    {
        if (Mod.Instance != null && Mod.Instance.isFlying)
        {
            Rigidbody rb = Traverse.Create(__instance).Field("rigidbody").GetValue<Rigidbody>();
            float baseSpeed = __instance.runSpeed;
            float flySpeed = baseSpeed * Mod.Instance.flySpeed;

            Vector3 moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) moveDirection += __instance.transform.forward;
            if (Input.GetKey(KeyCode.S)) moveDirection -= __instance.transform.forward;
            if (Input.GetKey(KeyCode.A)) moveDirection -= __instance.transform.right;
            if (Input.GetKey(KeyCode.D)) moveDirection += __instance.transform.right;
            if (Input.GetKey(KeyCode.Space)) moveDirection += Vector3.up;
            if (Input.GetKey(KeyCode.LeftControl)) moveDirection -= Vector3.up;

            rb.velocity = moveDirection.normalized * flySpeed;
            return false; // Skip original movement logic
        }
        return true; // Run normal movement if not flying
    }
}

[HarmonyPatch(typeof(Jump), "LateUpdate")]
public class DisableJumpPatch
{
    static bool Prefix()
    {
        return Mod.Instance == null || !Mod.Instance.isFlying; // Disable jump when flying
    }
}
