using DrawableLine;
using HarmonyLib;
using HitboxExtender.API.Features.Utils;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Firearms.Modules.Misc;
using UnityEngine;

namespace HitboxExtender.Patches;

[HarmonyPatch(typeof(HitscanHitregModuleBase), nameof(HitscanHitregModuleBase.ServerAppendPrescan))]
public class ServerAppendPrescanPrefix
{
    private static bool Prefix(HitscanHitregModuleBase __instance, Ray targetRay, HitscanResult toAppend)
    {
        if (HitboxExtenderPlugin.Instance == null)
            return true;
        
        if (!HitboxExtenderPlugin.Instance.Config.UseOldHitbox)
            return true;

        float maxDistance = __instance.DamageFalloffDistance + __instance.FullDamageDistance;
        float radius = HitboxExtenderPlugin.Instance.Config.HitboxSize;

        Vector3 origin = targetRay.origin + targetRay.direction * 0.1f;
        Vector3 castEnd = targetRay.origin + targetRay.direction * 1.1f;

        DrawableLinesUtils.GenerateCapsule(origin, castEnd, radius, Color.magenta, 5f);

        RaycastHit hit;
        
        Collider[] overlaps = Physics.OverlapCapsule(
            origin,
            castEnd,
            radius,
            HitscanHitregModuleBase.HitregMask,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider c in overlaps)
        {

            if (!c.TryGetComponent<IDestructible>(out var d))
            {
                continue;
            }

            Vector3 point = c.ClosestPoint(origin);

            if (!__instance.ValidateTarget(d, toAppend))
            {
                continue;
            }

            DrawableLines.GenerateSphere(point, radius, Color.magenta);
            DrawableLines.GenerateLine(5f, Color.yellow, origin, point);

            RaycastHit fakeHit = new RaycastHit
            {
                point = point,
                normal = -targetRay.direction,
                distance = 0f
            };

            toAppend.Destructibles.Add(new DestructibleHitPair(d, fakeHit, new Ray(origin, targetRay.direction)));
            return false;
        }
        
        bool didHit = Physics.CapsuleCast(
            origin,
            castEnd,
            radius,
            targetRay.direction,
            out hit,
            maxDistance,
            HitscanHitregModuleBase.HitregMask,
            QueryTriggerInteraction.Ignore
        );

        if (didHit && Vector3.Dot(hit.normal, -targetRay.direction) < 0.35f)
        {
            didHit = false;
        }
        
        if (!didHit)
        {
            RaycastHit[] allHits = Physics.CapsuleCastAll(
                origin,
                castEnd,
                radius,
                targetRay.direction,
                maxDistance,
                HitscanHitregModuleBase.HitregMask,
                QueryTriggerInteraction.Ignore
            );

            RaycastHit? closest = null;
            foreach (var h in allHits)
            {
                if (Vector3.Dot(h.normal, -targetRay.direction) < 0.35f)
                    continue;

                if (closest == null || h.distance < closest.Value.distance)
                    closest = h;
            }

            if (closest.HasValue)
            {
                hit = closest.Value;
                didHit = true;
            }
        }

        if (!didHit)
            return false;

        DrawableLines.GenerateLine(5f, Color.yellow, origin, hit.point);

        if (!hit.collider.TryGetComponent<IDestructible>(out var destructible))
        {
            toAppend.Obstacles.Add(new HitRayPair(new Ray(origin, targetRay.direction), hit));
            return false;
        }

        if (!__instance.ValidateTarget(destructible, toAppend))
            return false;

        DrawableLines.GenerateSphere(hit.point, radius, Color.cyan);
        toAppend.Destructibles.Add(new DestructibleHitPair(destructible, hit, new Ray(origin, targetRay.direction)));

        return false;
    }
}