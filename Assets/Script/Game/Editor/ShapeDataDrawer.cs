using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;


[CustomEditor(typeof(ShapeData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    ShapeData shapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoard();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (shapeDataInstance.board != null && shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(shapeDataInstance);
        }

    }


    void ClearBoard()
    {
        if (GUILayout.Button("Clear Board"))
        {
            shapeDataInstance.Clear();
            EditorUtility.SetDirty(shapeDataInstance);
        }
    }

    void DrawColumnsInputFields()
    {
        var columnTemp = shapeDataInstance.columns;
        var rowTemp = shapeDataInstance.rows;

        shapeDataInstance.columns = EditorGUILayout.IntField("Columns", shapeDataInstance.columns);
        shapeDataInstance.rows = EditorGUILayout.IntField("Rows", shapeDataInstance.rows);


        if (columnTemp != shapeDataInstance.columns || rowTemp != shapeDataInstance.rows && shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            shapeDataInstance.CreateNewBoard();
        }

    }

    void DrawBoardTable()
    {
        var tableStyle = new GUIStyle(GUI.skin.box);
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;
        headerColumnStyle.fixedWidth = 65;

        var rowsType = new GUIStyle();
        rowsType.fixedHeight = 25;
        rowsType.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row=0 ; row < shapeDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for (var column = 0; column < shapeDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowsType);
                var data = EditorGUILayout.Toggle(shapeDataInstance.board[row].columns[column], dataFieldStyle);
                shapeDataInstance.board[row].columns[column] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();


        }
    }


}
