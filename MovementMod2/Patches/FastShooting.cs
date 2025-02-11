using HarmonyLib;
using UnityEngine;
using UnityEngine.Windows;

[HarmonyPatch(typeof(Player))]
[HarmonyPatch("Update")]
public class PlayerUpdatePatch
{
    private static float lastFireTime = 0f;

    static void Prefix(Player __instance)
    {
        float fireRate = Mod.Instance.fireRate;
        bool fireEnabled = Mod.Instance.fastShooting;


        if (fireEnabled)
        {
            if (BepInEx.UnityInput.Current.GetMouseButton(0))
            {
                float currentTime = Time.time;
                if (currentTime - lastFireTime >= fireRate)
                {
                    Object.Instantiate(__instance.fireballPrefab, __instance.attackPoint.position, __instance.attackPoint.rotation);
                    lastFireTime = currentTime;
                }


            }
        }
        else
        {
            if (BepInEx.UnityInput.Current.GetMouseButtonDown(0))
            {
                Object.Instantiate(__instance.fireballPrefab, __instance.attackPoint.position, __instance.attackPoint.rotation);
            }
        }
    }
}
