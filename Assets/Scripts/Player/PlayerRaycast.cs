using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public float mouseRayDistance;

    private RaycastHit mouseHit;
    private Ray mouseRay;
    private Vector3 mousePosition;
    private LayerMask interactiveMask;

    void Awake()
    {
        interactiveMask = LayerMask.GetMask("Interactive");
    }

    void Update()
    {
        PlayerRaycastManager();
        PlayerInteraction();
    }

    void PlayerRaycastManager()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mousePosition, mouseRay.direction * mouseRayDistance, Color.green);
    }

    public void PlayerInteraction()
    {
        if (Physics.Raycast(mouseRay,out mouseHit,mouseRayDistance,interactiveMask))
        {
            print("hit");
        }
    }
}
