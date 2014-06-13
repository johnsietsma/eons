using UnityEngine;
using UnityEditor;

public class CreateEmptyAtSelection : ScriptableObject {

    [MenuItem("GameObject/Create Empty at Selection &n")]
    static void CreateEmpty() {
        GameObject go = new GameObject( "GameObject" );

        if (Selection.activeTransform != null) {
            go.transform.parent = Selection.activeTransform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

        Selection.activeGameObject = go;
    }
}
