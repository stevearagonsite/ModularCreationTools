using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreatorFloors : MonoBehaviour {

    private Transform _localPosition;
    private GameObject _creationGameObject;

    public List<GameObject> childs { get; set; }
    public List<GameObject> childsInScene { get; private set; }
    public Vector3 creationPosition { get { return _creationGameObject.transform.localPosition; } }
    public int areaCreationWidth { get; private set; }
    public int areaCreationHeight { get; private set; }
    public float areaWidthElements { get; private set; }
    public float areaHeightElements { get; private set; }
    public float separationWidth { get; private set; }
    public float separationHeight { get; private set; }

    private void Start()
    {
        EditorStart();
        this.CreationSizes(2, 2);
    }

    private void EditorStart()
    {
        if (_creationGameObject == null)
        {
            var newGameObject = new GameObject();
            newGameObject.transform.SetParent(this.transform);
            newGameObject.name = "CreationPoint";

            _creationGameObject = newGameObject;
        }
    }

    public void CreationSizes(float valueWidth, float valueHeight)
    {
        if (valueWidth != areaWidthElements || valueHeight != areaHeightElements)
        {
            areaWidthElements = valueWidth;
            areaHeightElements = valueHeight;
            Debug.Log("Sizes of elements");
        }
    }

    public void CreationElements(int valueWidth, int valueHeight)
    {
        if (valueWidth != areaCreationWidth || valueHeight != areaCreationHeight)
        {
            areaCreationWidth = valueWidth;
            areaCreationHeight = valueHeight;
            Debug.Log("Creation Elements");
        }
    }

    public void SeparationUpdate(float valueSeparationWidth, float valueSeparationHeight)
    {
        if (valueSeparationWidth != separationWidth || valueSeparationHeight != separationHeight)
        {
            separationWidth = valueSeparationWidth;
            separationHeight = valueSeparationHeight;
            Debug.Log("Update separation");
        }
    }

    public void UpdatePosition(float valueX, float valueZ)
    {
        _creationGameObject.transform.localPosition = new Vector3(valueX, transform.position.y, valueZ);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon( transform.position, ConstGizmos.PATH_GIZMO_FLOOR, true);
    }

    public void ExecutionUpdateChilds()
    {
        Debug.Log("Run update");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _creationGameObject.transform.position);

        Gizmos.color = Color.green;

        //If this view in center.
        Gizmos.matrix = _creationGameObject.transform.localToWorldMatrix;

        var separationAreaWidth = (areaCreationWidth > 1 ? areaCreationWidth - 1 : areaCreationWidth) * separationWidth;
        var separationAreaHeight = (areaCreationHeight > 1 ? areaCreationHeight - 1 : areaCreationHeight) * separationHeight;
        var areaX = (areaCreationWidth * areaWidthElements) + separationAreaWidth;
        var areaZ = (areaCreationHeight * areaHeightElements) + separationAreaHeight;

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaX, 0, areaZ));
    }
}
