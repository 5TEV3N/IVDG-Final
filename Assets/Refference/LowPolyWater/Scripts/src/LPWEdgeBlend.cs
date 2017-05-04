using UnityEngine;

[ExecuteInEditMode]
public class LPWEdgeBlend : MonoBehaviour {

    public void OnWillRenderObject() {
        if (!enabled || !GetComponent<Renderer>()) return;
        var material = GetComponent<Renderer>().sharedMaterial;
        if (!material) return;

        Camera cam = Camera.current;
        if (!cam) return;

        if (!material.HasProperty("_EdgeBlend")) return;

        if (material.GetFloat("_EdgeBlend") > 0.1f)
            cam.depthTextureMode |= DepthTextureMode.Depth;
    }

}
