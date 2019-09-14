using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    public Material PP_effect;

    [ExecuteInEditMode]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (PP_effect)
        {
            Graphics.Blit(source, destination, PP_effect);
        }
        else
        {
            Graphics.Blit(source, destination);
            Debug.Log("No Post Proccessing Effect");
        }
    }
}
