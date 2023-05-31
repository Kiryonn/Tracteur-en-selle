using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenderTextureToFile : MonoBehaviour
{
    public List<RenderTexture> renderTextureList;
    public float delay = 3f;

    private void Start()
    {

        Invoke("OnStart", delay);
    }

    public void OnStart()
    {
        int i = 0;
        foreach (RenderTexture item in renderTextureList)
        {
            string renderPath = AssetDatabase.GetAssetPath(item);
            int idx = renderPath.LastIndexOf('/');
            string filePath = renderPath[..idx] + "/RenderTexture" + i + ".png";
            i++;

            SaveRenderTextureToFile(item, filePath);
        }
    }

    // L'appel � cette fonction enregistrera la Render Texture en tant qu'image brute
    public void SaveRenderTextureToFile(RenderTexture renderTexture, string filePath)
    {
        // Assurez-vous que la Render Texture a �t� correctement d�finie
        if (renderTexture == null)
        {
            Debug.LogError("Render Texture non d�finie.");
            return;
        }
        renderTexture.Create();
        // Cr�ez une nouvelle texture 2D avec les m�mes dimensions que la Render Texture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Activez la Render Texture pour la lire
        RenderTexture.active = renderTexture;

        // Lisez les donn�es de la Render Texture dans la texture 2D
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // D�sactivez la Render Texture
        RenderTexture.active = null;

        // Convertissez la texture 2D en tableau d'octets
        byte[] bytes = texture.EncodeToPNG();

        // Enregistrez les octets sous forme de fichier
        System.IO.File.WriteAllBytes(filePath, bytes);

        // D�truisez la texture 2D pour lib�rer la m�moire
        Destroy(texture);

        Debug.Log("Render Texture enregistr�e en tant qu'image brute : " + filePath);
    }
}

