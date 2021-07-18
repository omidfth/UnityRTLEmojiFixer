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
        bool testUnicode = GUILayout.Button("Test Unicode");
        if (testUnicode)
        {
            myTarget.TestUnicode();
        }
    }
}
