using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GenericsMethods;
using System;

[CustomEditor(typeof(CreatorFloors))]
public class EditorCreatorFloor : Editor {

    private CreatorFloors _creator;

    private void OnEnable()
    {
        _creator = (CreatorFloors)target;
        if (_creator.childs == null)
            _creator.childs = new List<GameObject>();
    }

    public override void OnInspectorGUI()
    {

        //We can stop the editing in execution mode.
        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("this can't edited in run-mode", MessageType.Error);
            return;
        }

        GenericsEditor.Spaces(2);
        GenericsEditor.Rect(3, Color.black);

        Title();

        GenericsEditor.Spaces(1);
        GenericsEditor.Rect(2, Color.black);
        GenericsEditor.Spaces(2);

        ListOfElements();

        if (_creator.childs.Count <= 0)
        {
            EditorGUILayout.HelpBox("The list of elements is empty.", MessageType.Warning);
            return;
        }

        foreach (var element in _creator.childs)
        {
            if (element == null || _creator.childs.Count <= 0)
            {
                EditorGUILayout.HelpBox("Some of the elements to create is empty.", MessageType.Warning);
                break;
            }
        }

        GenericsEditor.Spaces(1);
        GenericsEditor.Rect(2, Color.black);
        GenericsEditor.Spaces(2);

        ElementSettings();


        GenericsEditor.Spaces(1);
        GenericsEditor.Rect(2, Color.black);
        GenericsEditor.Spaces(2);

        AreaOfCreation();

        GenericsEditor.Spaces(2);
        GenericsEditor.Rect(3, Color.black);
        GenericsEditor.Spaces(2);

        var name = (_creator.childsInScene != null && _creator.childsInScene.Count > 0) ? "update" : "create";
        if (GUILayout.Button(name))
        {
            _creator.ExecutionUpdateChilds();
            Repaint();
            return;
        }
    }

    private void ElementSettings()
    {
        //Initial message.
        var style = TextStyles.h4;
        style.padding = new RectOffset(1, 0, 1, 0);
        GUI.TextArea(GUILayoutUtility.GetRect(0, 36), ConstEditor.CREATOR_FLOORS_AREA_OF_ELEMENTS, style);

        //Row 2
        EditorGUILayout.BeginHorizontal();
        GUI.Label(GUILayoutUtility.GetRect(80, 25), "Area ");

        GUI.Label(GUILayoutUtility.GetRect(40, 25), "Width:  ");
        var AreaWidth = EditorGUILayout.FloatField(_creator.areaWidthElements);
        GUI.Label(GUILayoutUtility.GetRect(40, 25), "Height: ");
        var AreaHeight = EditorGUILayout.FloatField(_creator.areaHeightElements);

        _creator.CreationSizes(AreaWidth, AreaHeight);

        EditorGUILayout.EndHorizontal();
    }

    private void AreaOfCreation()
    {
        //Initial message.
        var style = TextStyles.h4;
        style.padding = new RectOffset(1, 0, 1, 0);
        GUI.TextArea(GUILayoutUtility.GetRect(0, 36), ConstEditor.CREATOR_FLOORS_AREA_TO_CREATE, style);

        //Row 1
        EditorGUILayout.BeginHorizontal();
        GUI.Label(GUILayoutUtility.GetRect(80, 25), "Begin zone");

        GUI.Label(GUILayoutUtility.GetRect(15, 25), "X: ");
        var x = EditorGUILayout.FloatField(_creator.creationPosition.x);
        GUI.Label(GUILayoutUtility.GetRect(15, 25), "Z: ");
        var z = EditorGUILayout.FloatField(_creator.creationPosition.z);

        _creator.UpdatePosition(x, z);

        EditorGUILayout.EndHorizontal();


        //Row 2
        EditorGUILayout.BeginHorizontal();
        GUI.Label(GUILayoutUtility.GetRect(80, 25), "Area ");

        GUI.Label(GUILayoutUtility.GetRect(40, 25), "Width:  ");
        var AreaWidth = EditorGUILayout.IntField(_creator.creationCountX);
        GUI.Label(GUILayoutUtility.GetRect(40, 25), "Height: ");
        var AreaHeight = EditorGUILayout.IntField(_creator.creationCountZ);

        _creator.CreationElements(AreaWidth, AreaHeight);

        EditorGUILayout.EndHorizontal();

        //Row 3
        EditorGUILayout.BeginHorizontal();
        GUI.Label(GUILayoutUtility.GetRect(80, 25), "Separation ");

        GUI.Label(GUILayoutUtility.GetRect(80, 25), "In Width:  ");
        var InWidth = EditorGUILayout.FloatField(_creator.separationWidth);
        GUI.Label(GUILayoutUtility.GetRect(80, 25), "In Height: ");
        var InHeight = EditorGUILayout.FloatField(_creator.separationHeight);

        _creator.SeparationUpdate(InWidth, InHeight);

        EditorGUILayout.EndHorizontal();
    }

    private void ListOfElements()
    {
        //Initial message.
        var style = TextStyles.h4;
        style.padding = new RectOffset(1, 0, 1, 0);
        GUI.TextArea(GUILayoutUtility.GetRect(0, 36), ConstEditor.CREATOR_FLOORS_OBJECTS_TO_CREATE + " : " + _creator.childs.Count, style);

        //Elements to create (Delete of list).
        for (int i = 0; i < _creator.childs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            var element = _creator.childs[i];
            _creator.childs[i] = (GameObject)EditorGUILayout.ObjectField("The object: ", element, typeof(GameObject), true);
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove"))
            {
                _creator.childs.RemoveAt(i);
                Repaint();
                return;
            }
            EditorGUILayout.EndHorizontal();
        }

        //Add space for new elements.
        GenericsEditor.Spaces(2);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            _creator.childs.Add(null);
            Repaint();
            return;
        }
        

        EditorGUILayout.EndHorizontal();
    }

    private void Title()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        
        //Icons.
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(30));
        GUI.DrawTexture(GUILayoutUtility.GetRect(36, 36), Resources.Load<Texture2D>(ConstEditor.PATH_ICON_FLOOR), ScaleMode.StretchToFill);
        EditorGUILayout.EndHorizontal();

        //Title.
        var style = TextStyles.h3;
        style.padding = new RectOffset(1, 0, 1, 0);
        GUI.TextArea(GUILayoutUtility.GetRect(0, 36), ConstEditor.CREATOR_FLOOR_TITLE, style);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }
}
