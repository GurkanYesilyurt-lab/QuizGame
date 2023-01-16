using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class LevelEditor : OdinEditorWindow
{
    [MenuItem("Level Tools/Level Editor")]
    private static void OpenWindow()
    {
        GetWindow<LevelEditor>().Show();
    }

    [PropertyOrder(-10)]
    [HorizontalGroup]
    [Button(ButtonSizes.Large)]
    public void LogIn()
    {
    }
}