using UnityEngine;
public class UvConfirm : MonoBehaviour
{
    void Awake()
    {
        MeshFilter meshRend = GetComponent<MeshFilter>();
        if (meshRend != null)
        {
            LogUVs(meshRend.sharedMesh.uv, 0);
            LogUVs(meshRend.sharedMesh.uv1, 1);
            LogUVs(meshRend.sharedMesh.uv2, 2);
        }
    }

    private void LogUVs(Vector2[] uvs, int channel)
    {
        string output = "ArrayData: (";
        for (int i = 0; i < uvs.Length; i++)
        {
            output += uvs[i].ToString();
            if (i < uvs.Length - 1) output += ", ";
        }
        output += ")";
        Debug.Log("UV Channel " + channel.ToString() + ": " + output);
    }
}