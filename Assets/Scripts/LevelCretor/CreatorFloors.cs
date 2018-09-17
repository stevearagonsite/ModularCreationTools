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
    public int areaWidth { get; private set; }
    public int areaHeight { get; private set; }
    public float separationWidth { get; private set; }
    public float separationHeight { get; private set; }

    private void Update()
    {
        EditorStart();
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

    public void CreationElements(int valueWidth, int valueHeight)
    {
        if (valueWidth != areaWidth || valueHeight != areaHeight)
        {
            areaWidth = valueWidth;
            areaHeight = valueHeight;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _creationGameObject.transform.position);

        Gizmos.color = Color.green;

        var sideA = new Vector3(_creationGameObject.transform.position.x, transform.position.y, _creationGameObject.transform.position.z);
        var sideB = new Vector3(_creationGameObject.transform.position.x + areaWidth, transform.position.y, _creationGameObject.transform.position.z);
        var sideC = new Vector3(_creationGameObject.transform.position.x + areaWidth, transform.position.y, _creationGameObject.transform.position.z + areaHeight);
        var sideD = new Vector3(_creationGameObject.transform.position.x, transform.position.y, _creationGameObject.transform.position.z + areaHeight);

        Gizmos.DrawLine(sideA, sideB);
        Gizmos.DrawLine(sideB, sideC);
        Gizmos.DrawLine(sideC, sideD);
        Gizmos.DrawLine(sideD, sideA);
    }
}
