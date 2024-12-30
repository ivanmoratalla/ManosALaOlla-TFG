using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter
{
    public void StartHighlight(GameObject go)
    {
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>(); ;

        if (meshRenderer)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                Material[] materials;
                (materials = meshRenderer.materials)[i].EnableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                meshRenderer.materials[i].SetColor("_EmissionColor", new Color(0.4f, 0.4f, 0.4f));
            }
        }
    }

    public void StopHighlight(GameObject go)
    {
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>(); ;

        if (meshRenderer)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                //_meshRenderer.materials[i].color = _baseColors[i];
                Material[] materials;
                (materials = meshRenderer.materials)[i].DisableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                meshRenderer.materials[i].SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
