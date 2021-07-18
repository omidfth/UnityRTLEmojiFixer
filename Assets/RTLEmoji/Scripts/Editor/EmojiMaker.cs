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
    public TextAsset utfText;
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
        var utfData = JsonConvert.DeserializeObject<List<UTFData>>(utfText.text);

        foreach (var data in datas)
        {
            var bytes = GetByte(data.unified, utfData);
            if (bytes == null)
            {
                bytes = data.short_name;
            }
            SpriteMetaData meta = new SpriteMetaData
            {
                rect = new Rect((sliceSize + padding) * data.sheet_x,
                    imageSize - sliceSize - (sliceSize + padding) * data.sheet_y, sliceSize, sliceSize),
                name = bytes
            };
            metaData.Add(meta);
        }

        importer.spritesheet = metaData.ToArray();
        importer.SaveAndReimport();
    }


    public void TestUnicode()
    {
        TextAsset ta = text;
        var datas = JsonConvert.DeserializeObject<EmojiData[]>(ta.text);
        var utfData = JsonConvert.DeserializeObject<List<UTFData>>(utfText.text);
        foreach (var data in datas)
        {
            GetByte(data.unified, utfData);
        }
    }

    public string GetByte(string unicode,List<UTFData> utfData)
    {
        var upperUnicode = unicode.ToUpper();
        foreach (var data in utfData)
        {
            var currentUnicode = data.Unicode;
            currentUnicode.Replace(" ", "-");
            if (upperUnicode == currentUnicode)
            {
                Debug.Log(upperUnicode+" -OK- " + data.Bytes);
                return data.Bytes;
            }
        }

        return null;
    }


}
