using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;

public class DataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Data Editor")]
    private static void OpenWindow()
    {
        var window = GetWindow<DataEditor>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    [AssetList(Path = "/Data/")]
    public List<ScriptableObjectBase> AssetList;

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);

        tree.AddAllAssetsAtPath("Assets", "Data", typeof(ScriptableObjectBase), true);
        tree.SortMenuItemsByName();
        return tree;
    }
}
