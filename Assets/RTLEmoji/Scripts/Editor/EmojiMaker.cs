using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RTLEmoji.Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Emoji Maker", menuName = "RTLEmoji")]

public class EmojiMaker : ScriptableObject
{
    public Texture2D texture;
    public TextAsset text;
    public int imageSize;
    public int sliceSize;
    public int padding;

    [Header("Unicode")] public TMP_SpriteAsset spriteAsset;

    public void Slice()
    {
        TextAsset ta = text;
        string path = AssetDatabase.GetAssetPath(this.texture);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer is null) return;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 1;
        List<SpriteMetaData> metaData = new List<SpriteMetaData>();
        var datas = JsonConvert.DeserializeObject<EmojiData[]>(ta.text);
        foreach (var data in datas)
        {
            SpriteMetaData meta = new SpriteMetaData
            {
                rect = new Rect((sliceSize + padding) * data.sheet_x,
                    imageSize - sliceSize - (sliceSize + padding) * data.sheet_y, sliceSize, sliceSize),
                name = data.unified
            };
            metaData.Add(meta);
        }

        importer.spritesheet = metaData.ToArray();
        importer.SaveAndReimport();
    }


    public void FixUnicode()
    {
        TextAsset ta = text;
        var data = JsonConvert.DeserializeObject<EmojiData[]>(ta.text);
        for (int i = 0; i < spriteAsset.spriteCharacterTable.Count; i++)
        {
            if (!data[i].unified.Contains('-'))
            {
             //   var hex = data[i].unified.Replace("-", "");
                Debug.Log($"index:{i}, data:{data[i].unified}, convertData:{Convert.ToUInt64(data[i].unified, 16)}");
                spriteAsset.spriteCharacterTable[i].unicode = Convert.ToUInt32(data[i].unified, 16);
                spriteAsset.spriteCharacterTable[i].name = data[i].unified;
            }
        }
    }

}
