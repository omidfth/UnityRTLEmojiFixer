using  UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmojiMaker))]
public class EmojiMakerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var myTarget = (EmojiMaker) target;
        bool slice = GUILayout.Button("Slice");
        if (slice)
        {
            myTarget.Slice();
        }

        bool fixUnicode = GUILayout.Button("Fix Unicode");
        if (fixUnicode)
        {
            myTarget.FixUnicode();
        }
    }
}
