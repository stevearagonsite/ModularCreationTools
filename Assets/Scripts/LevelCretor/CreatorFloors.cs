﻿using System;
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
    public int creationCountX { get; private set; }
    public int creationCountZ { get; private set; }
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
        if (valueWidth != creationCountX || valueHeight != creationCountZ)
        {
            creationCountX = valueWidth;
            creationCountZ = valueHeight;
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

        //If this view in center.
        Gizmos.color = Color.green;
        Gizmos.matrix = _creationGameObject.transform.localToWorldMatrix;

        var separationAreaWidth = (creationCountX > 1 ? creationCountX - 1 : creationCountX) * separationWidth;
        var separationAreaHeight = (creationCountZ > 1 ? creationCountZ - 1 : creationCountZ) * separationHeight;
        var areaX = (creationCountX * areaWidthElements) + separationAreaWidth;
        var areaZ = (creationCountZ * areaHeightElements) + separationAreaHeight;

        //The modules in area creation.
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaX, -0.1f, areaZ));

        //Create elements view.
        Gizmos.color = Color.blue;
        var elements = GetPositions(-((areaX / 2) - areaWidthElements / 2), -((areaZ / 2) - areaHeightElements / 2));

        foreach (var element in elements)
        {
            Gizmos.DrawWireCube(
                new Vector3(element.Item1, 0, element.Item2),
                new Vector3(areaWidthElements, 0, areaHeightElements
                ));
        }
    }

    public IEnumerable<Tuple<float, float>> GetPositions(float pivotPointX, float pivotPointZ)
    {
        for (int i = 0; i < creationCountX; i++)
        {
            var newPositionX = pivotPointX + ((areaWidthElements + separationWidth) * i);
            for (int j = 0; j < creationCountZ; j++)
            {
                var newPositionZ = pivotPointZ + ((areaHeightElements + separationHeight) * j);

                yield return Tuple.Create(newPositionX, newPositionZ);
            }
        }
    }
}
