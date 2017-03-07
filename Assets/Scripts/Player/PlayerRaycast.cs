using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public float mouseRayDistance;
    public RaycastHit mouseHit;
    public Ray mouseRay;
    public Vector3 mousePosition;

    private LayerMask interactiveMask;

    void Awake()
    {
        interactiveMask = LayerMask.GetMask("Interactive");
    }

    void Update()
    {
        DebugRaycast();
        PlayerInteraction();
    }

    void DebugRaycast()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mousePosition, mouseRay.direction * mouseRayDistance, Color.green);
    }

    public bool PlayerInteraction()
    {
        return Physics.Raycast(mouseRay, out mouseHit, mouseRayDistance, interactiveMask);
    }

    public GameObject hitObject()
    {
        GameObject hit = mouseHit.transform.gameObject;
        return hit;
    }
}
