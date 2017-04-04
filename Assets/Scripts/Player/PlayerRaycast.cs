using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public float mouseRayDistance;
    public RaycastHit mouseHit;
    public Ray mouseRay;
    public Vector3 mousePosition;
    public GameObject hit;

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
        hit = mouseHit.transform.gameObject;
        return hit;
    }

    public Vector3 hitObjectTransform()
    {
        return hit.transform.position;
    }
}
