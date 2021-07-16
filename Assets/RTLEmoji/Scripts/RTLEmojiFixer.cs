using System;
using System.Text;
using RTLTMPro;
using TMPro;
using UnityEngine;

namespace RTLEmoji.Scripts
{
    [RequireComponent(typeof(RTLTextMeshPro))]
    public class RTLEmojiFixer : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro _textMeshPro;

        private void Awake()
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<RTLTextMeshPro>();
            }
        }

        public void Fix()
        {
            if (_textMeshPro != null)
            {
                string hexString = StringToHex(_textMeshPro.OriginalText);
                print(hexString);
                var property = GetSpriteProperty(hexString);
                if (property.Item1 > -1)
                {
                    var emojiUnicode = property.Item2.Replace("-", "");
                    print(emojiUnicode);
                    string emoji = HexToString(emojiUnicode);
                    print("pre: " + _textMeshPro.text);
                    _textMeshPro.text = _textMeshPro.OriginalText.Replace(emoji, $"<sprite={property.Item1}>");
                    print(_textMeshPro.text);
                    Fix();
                }
            }
        }

        private (int,string) GetSpriteProperty(string hex)
        {
            for (int i = 0; i < TMP_Settings.defaultSpriteAsset.spriteCharacterTable.Count; i++)
            {
                if (hex.Contains(TMP_Settings.defaultSpriteAsset.spriteCharacterTable[i].name))
                {
                    return (i,TMP_Settings.defaultSpriteAsset.spriteCharacterTable[i].name);
                }
            }

            return (-1,"");
        }
    
        private string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            { 
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("X4"));
                sb.Append("-");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    
        private string HexToString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.BigEndianUnicode.GetString(bytes);
        }
    }
}
