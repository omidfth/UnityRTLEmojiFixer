using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

    [Header("SpriteAsset")] public TMP_SpriteAsset spriteAsset;

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
            var utfName = GetUTF8(data.unified);
            SpriteMetaData meta = new SpriteMetaData
            {
                rect = new Rect((sliceSize + padding) * data.sheet_x,
                    imageSize - sliceSize - (sliceSize + padding) * data.sheet_y, sliceSize, sliceSize),
                name = utfName
            };
            metaData.Add(meta);
        }

        importer.spritesheet = metaData.ToArray();
        importer.SaveAndReimport();
    }

    private string GetUTF8(string unified)
    {
        var splitStrings = unified.Split('-');
        StringBuilder sb = new StringBuilder();
        foreach (var splitString in splitStrings)
        {
            int unicode = int.Parse(splitString, NumberStyles.HexNumber);
            var x = char.ConvertFromUtf32(unicode);
            var enc = new UTF8Encoding();
            var bytes = enc.GetBytes(x);
            var hex = new StringBuilder();
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            sb.Append(hex);
        }

        return sb.ToString().ToUpper();
    }

    public void FixUnicode()
    {
        var datas = JsonConvert.DeserializeObject<EmojiData[]>(text.text);
        foreach (var data in datas)
        {
            if (!data.unified.Contains("-"))
            {
                var utfName = GetUTF8(data.unified);
                var index = spriteAsset.spriteCharacterTable.FindIndex(t => t.name == utfName);
                spriteAsset.spriteCharacterTable[index].unicode = uint.Parse(data.unified, NumberStyles.HexNumber);
            }
        }
    }
}