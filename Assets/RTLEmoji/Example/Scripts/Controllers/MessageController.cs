using RTLEmoji.Example.Scripts.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RTLEmoji.Example.Scripts.Controllers
{
    public class MessageController : MonoBehaviour
    {
        [SerializeField] private Message _messagePrefab;
        [SerializeField] private Transform _messageParent;
        [SerializeField] private TMP_InputField _inputField;
        public void Send()
        {
            if (!string.IsNullOrEmpty(_inputField.text))
            {
                var message = Instantiate(_messagePrefab, _messageParent);
                message.Set(_inputField.text);
                _inputField.text = "";
            }
        }
    }
}
