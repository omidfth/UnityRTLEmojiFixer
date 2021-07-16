using RTLEmoji.Scripts;
using RTLTMPro;
using UnityEngine;

namespace RTLEmoji.Example.Scripts.Views
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro _textMeshPro;
        [SerializeField] private RTLEmojiFixer _emojiFixer;

        public void Set(string message)
        {
            _textMeshPro.text = message;
            _emojiFixer.Fix();
        }
    }
}
