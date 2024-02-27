using UnityEngine;
using UnityEditor;
using System.IO;

public class Spriting : MonoBehaviour
{
    [MenuItem("Tools/Convert Images to Sprites")]
    static void ConvertImagesToSprites()
    {
        string folderPath = "Assets/Sprites/Idle animation"; // Path to your folder of images
        var imagePaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories); // Adjust the search pattern if necessary

        foreach (var imagePath in imagePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(imagePath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single; // Or use SpriteImportMode.Multiple for sprite sheets
                importer.SaveAndReimport();
            }
        }

        Debug.Log("Converted all images in folder to sprites.");
    }
}
