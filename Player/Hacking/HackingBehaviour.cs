using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;

public class HackingBehaviour : MonoBehaviour
{
    public static int RayTargetLayerMask = 
        ~(1 << LayerMask.NameToLayer("Ignore Raycast") + 
        1 << LayerMask.NameToLayer("Player"));

    private static float _RayBoxSize = 0.5f;
    public static float RayBoxSize { get => _RayBoxSize; set => _RayBoxSize = value; }

    private static float _RayMaxDistance = 100.0f;
    public static float RayMaxDIstance { get => _RayMaxDistance; set => _RayMaxDistance = value; }

    public static GameObject TakingAim(Transform curCamera)
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * 100.0f, Color.red, 1.0f);

        GameObject aimtarget = null;

        Vector3 rayBoxSize = Vector3.one * RayBoxSize;

        RaycastHit[] hitInfos = Physics.BoxCastAll(
            rayOrigin, rayBoxSize, rayDirection,
            Quaternion.identity, RayMaxDIstance,
            RayTargetLayerMask, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hitInfo in hitInfos)
        {
            if(hitInfo.collider.CompareTag("Object"))
            {
                aimtarget = hitInfo.transform.gameObject;
                return aimtarget;
            }
        }

        return aimtarget;
    }


}
