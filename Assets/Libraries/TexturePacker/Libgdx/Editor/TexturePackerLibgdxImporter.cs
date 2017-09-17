/*
 *  TexturePackerLibgdx Importer
 *  Phuong Dong Tran
 * 
 *  Use this script to import sprite sheets generated with TexturePacker of Libgdx.
 *  For more information see https://github.com/libgdx/libgdx/wiki/Texture-packer
 *  and http://code.google.com/p/libgdx-texturepacker-gui/
 * 
 *  Thanks to aurelien.ribon for providing the GUI
 *
 */

using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class TexturePackerLibgdxImporter : AssetPostprocessor
{
	const string IMPORTER_VERSION = "1.0.0";

	static string[] textureExtensions = {
		".png",
		".jpg",
		".jpeg",
		".tiff",
		".tga",
		".bmp"
	};


	/*
	 *  Trigger a texture file re-import each time the .tpsheet file changes (or is manually re-imported)
	 */
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (var asset in importedAssets) {
			if (!Path.GetExtension (asset).Equals (".atlas"))
				continue;
			foreach (string ext in textureExtensions) {
				string pathToTexture = Path.ChangeExtension (asset, ext);
				if (File.Exists (pathToTexture)) {
					AssetDatabase.ImportAsset (pathToTexture, ImportAssetOptions.ForceUpdate);
					break;
				}
			}
		}
	}


	/*
	 *  Trigger a sprite sheet update each time the texture file changes (or is manually re-imported)
	 */
	void OnPreprocessTexture ()
	{
		TextureImporter importer = assetImporter as TextureImporter;

        string pathToData = Path.ChangeExtension(assetPath, ".atlas");
		if (File.Exists (pathToData)) {
			updateSpriteMetaData (importer, pathToData);
		}
	}

	static void updateSpriteMetaData (TextureImporter importer, string pathToData)
	{
		if (importer.textureType != TextureImporterType.Default) {
			importer.textureType = TextureImporterType.Sprite;
		}
		importer.maxTextureSize = 4096;
        importer.mipmapEnabled = false;
		importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
        
        int width, height;
        GetImageSize(importer, out width, out height);

		string[] dataFileContent = File.ReadAllLines(pathToData);
		Dictionary<string, SpriteMetaData> existingSprites = new Dictionary<string, SpriteMetaData>();
        for (int i = 0; i < importer.spritesheet.Length; i++)
        {
            SpriteMetaData sprite = importer.spritesheet[i];
            if (existingSprites.ContainsKey(sprite.name)){
                Debug.LogError("Same key: " + sprite.name);
            }
            else
                existingSprites.Add(sprite.name, sprite);
        }
        
		List<SpriteMetaData> metaData = new List<SpriteMetaData> ();

        int numofSprites = (int)Math.Round((dataFileContent.Length - 6) / 7f, 0);
        int cursor = 5;
        
        for (int i = 0; i < numofSprites; i++)
        {
            string name = dataFileContent[cursor + 0].Trim();
            float x, y;
            GetParams(dataFileContent[cursor + 2].Trim(), out x, out y);
            float w, h;
            GetParams(dataFileContent[cursor + 3].Trim(), out w, out h);
            float px = 0.5f, py = 0.5f;
            y = height - y - h;

            SpriteMetaData smd = new SpriteMetaData();
            smd.name = name;
            smd.rect = new UnityEngine.Rect(x, y, w, h);

            if (existingSprites.ContainsKey(smd.name))
            {
                SpriteMetaData sprite = existingSprites[smd.name];
                smd.pivot = sprite.pivot;
                smd.alignment = sprite.alignment;
                smd.border = sprite.border;
            }

            if (!existingSprites.ContainsKey(smd.name))
            {
                smd.pivot = new UnityEngine.Vector2(px, py);

                if (px == 0 && py == 0)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.BottomLeft;
                else if (px == 0.5 && py == 0)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.BottomCenter;
                else if (px == 1 && py == 0)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.BottomRight;
                else if (px == 0 && py == 0.5)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.LeftCenter;
                else if (px == 0.5 && py == 0.5)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.Center;
                else if (px == 1 && py == 0.5)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.RightCenter;
                else if (px == 0 && py == 1)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.TopLeft;
                else if (px == 0.5 && py == 1)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.TopCenter;
                else if (px == 1 && py == 1)
                    smd.alignment = (int)UnityEngine.SpriteAlignment.TopRight;
                else
                    smd.alignment = (int)UnityEngine.SpriteAlignment.Custom;
            }
            metaData.Add(smd);
            cursor += 7;
        }

		importer.spritesheet = metaData.ToArray();
	}

    private static void GetParams(string line, out float param1, out float param2){
        string right = line.Split(':')[1].Trim();
        string [] arr = right.Split(',');
        param1 = float.Parse(arr[0].Trim());
        param2 = float.Parse(arr[1].Trim());
    }

    private static string GetNewName(Dictionary<string, SpriteMetaData> existingSprites, string oldName)
    {
        int index = 1;
        while (true)
        {
            string newName = oldName + "_" + index;
            if (!existingSprites.ContainsKey(newName))
            {
                return newName;
            }
            index++;
        }
    }

    private static bool GetImageSize(TextureImporter importer, out int width, out int height)
    {
            if (importer != null)
            {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);

                width = (int)args[0];
                height = (int)args[1];

                return true;
            }

        height = width = 0;
        return false;
    }
}
