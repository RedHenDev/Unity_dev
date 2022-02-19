using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snap : MonoBehaviour
{
    private static snap instance;
    
    private Camera cam; 
    
    private bool takeScreenSnap = false;
    public bool saveFile = false;

    public GameObject myDisplay;

    public static void takeSnap_static(int width, int height){
        instance.takeSnap(width, height);
    }

    void Start()
    {
        instance = this;
        cam = this.gameObject.GetComponent<Camera>();
    }

    private void takeSnap(int width, int height){
        cam.targetTexture =
            RenderTexture.GetTemporary(width,height,16);
        takeScreenSnap = true;
    }

    private void OnPostRender() {
        if (takeScreenSnap==true){
            
            takeScreenSnap = false;

            RenderTexture rendTex = 
                cam.targetTexture;

            // Retrieve temporary render as texture.
            Texture2D renderResult = 
                new Texture2D(  rendTex.width,
                                rendTex.height,
                                TextureFormat.ARGB32, 
                                false);

            // Gather pixels from texture.
            Rect rect = 
                new Rect(   0,0,
                            rendTex.width,
                            rendTex.height);
            renderResult.ReadPixels(rect,
                                    0,0);
            
            // Apply pixels.
            renderResult.Apply();

            // Apply texture.
            myDisplay.GetComponent<Renderer>().
            material.mainTexture = renderResult;

            // Save to file.
            if (saveFile==true){
            byte[] dog = renderResult.EncodeToPNG();
            System.IO.File.
                WriteAllBytes("/Users/bnew/Desktop/snap.png", dog);
            }

            // Reset camera.
            RenderTexture.ReleaseTemporary(rendTex);
            cam.targetTexture = null;

        }

    }
}
