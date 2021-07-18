using System;
using System.Linq;
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
                    string emoji = HexToString(emojiUnicode);
                    _textMeshPro.text = _textMeshPro.OriginalText.Replace(emoji, $"<sprite={property.Item1}>");
                    Fix();
                }
            }
        }

        private (int,string) GetSpriteProperty(string hex)
        {
            for (int i = 0; i < TMP_Settings.defaultSpriteAsset.spriteCharacterTable.Count; i++)
            {
                var spriteName = TMP_Settings.defaultSpriteAsset.spriteCharacterTable[i].name;
                if (hex.Contains(spriteName))
                {
                    return (i,spriteName);
                }
            }

            return (-1,"");
        }

        private string StringToHex(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] inputByte = Encoding.UTF8.GetBytes(str);
            foreach (byte b in inputByte)
            {
                sb.Append(string.Format("{0:x2}", b));
            }

            return sb.ToString().ToUpper();
        }
        

        private string HexToString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
