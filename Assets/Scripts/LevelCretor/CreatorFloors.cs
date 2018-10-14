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
    public Vector3 creationPosition { get { return _creationGameObject ? _creationGameObject.transform.localPosition : transform.position; } }

    private float _totalSeparationAreaWidth = 2;
    private float _totalSeparationAreaHeight = 2;
    private float _areaX = 2;
    private float _areaZ = 2;

    public int creationCountX { get; private set; }
    public int creationCountZ { get; private set; }
    public float areaWidthElements { get; private set; }
    public float areaHeightElements { get; private set; }
    public float separationWidth { get; private set; }
    public float separationHeight { get; private set; }

    private void OnEnable()
    {
        EditorStart();
        this.CreationSizes(2, 2);
    }

    private void Update()
    {
        //Security execution.
        if (transform.childCount == 0 || _creationGameObject == null) EditorStart();
        if (transform.eulerAngles != Vector3.zero) transform.eulerAngles = Vector3.zero;
        if (_creationGameObject && _creationGameObject.transform.eulerAngles != Vector3.zero) _creationGameObject.transform.eulerAngles = Vector3.zero;

    }

    private void EditorStart()
    {
        var childsCount = transform.childCount;
        Debug.Log("sarasasa " + childsCount + _creationGameObject);

        if (childsCount <= 0){
            var newGameObject = new GameObject();
            newGameObject.name = "CreationPoint";
            newGameObject.transform.SetParent(this.transform);

            _creationGameObject = newGameObject;
        }
        else if(childsCount > 0 && !_creationGameObject){

            var list = transform.GetComponentInChildren<GameObject>();
            Debug.Log("ss" + list);
            for (int i = childsCount - 1; i < 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).GetComponent<GameObject>());
            }

            var newGameObject = new GameObject();
            newGameObject.name = "CreationPoint";
            newGameObject.transform.SetParent(this.transform);

            _creationGameObject = newGameObject;
        }else if (childsCount == 1 && !_creationGameObject)
        {
            _creationGameObject = transform.GetChild(0).GetComponent<GameObject>();
        }
    }

    public void CreationSizes(float valueWidth, float valueHeight)
    {
        if (valueWidth != areaWidthElements || valueHeight != areaHeightElements)
        {
            areaWidthElements = valueWidth;
            areaHeightElements = valueHeight;

            UpdateGeneralValues();
        }
    }

    public void CreationElements(int valueWidth, int valueHeight)
    {
        if (valueWidth != creationCountX || valueHeight != creationCountZ || childs.Count > 0)
        {
            creationCountX = valueWidth;
            creationCountZ = valueHeight;

            UpdateGeneralValues();
        }
    }

    public void SeparationUpdate(float valueSeparationWidth, float valueSeparationHeight)
    {
        if (valueSeparationWidth != separationWidth || valueSeparationHeight != separationHeight)
        {
            separationWidth = valueSeparationWidth;
            separationHeight = valueSeparationHeight;

            UpdateGeneralValues();
        }
    }

    public void UpdatePosition(float valueX, float valueZ)
    {
        if (_creationGameObject) _creationGameObject.transform.localPosition = new Vector3(valueX, transform.position.y, valueZ);
    }

    //Online execution when the user set the action.
    public void ExecutionUpdateChilds()
    {
        //_creationGameObject.transform.lo = transform.rotation;

        var elements = GetPositions(
            -((_areaX / 2) - areaWidthElements / 2) + _creationGameObject.transform.position.x + transform.position.x,
            -((_areaZ / 2) - areaHeightElements / 2) + _creationGameObject.transform.position.z + transform.position.z);

        //Reset the scene for update the scene.
        childsInScene.ForEach(element => DestroyImmediate(element));
        childsInScene = null;
        childsInScene = new List<GameObject>();

        //Create new elements for update the scene.
        foreach (var element in elements)
        {
            foreach (var child in childs)
            {
                var newChildInScene = Instantiate(child);
                childsInScene.Add(newChildInScene);

                newChildInScene.transform.position = new Vector3(element.Item1, _creationGameObject.transform.position.y, element.Item2);
                newChildInScene.transform.parent = _creationGameObject.transform;
            }
        }
    }

    //Calculation the area of creation.
    public void UpdateGeneralValues()
    {
        _totalSeparationAreaWidth = (creationCountX > 1 ? creationCountX - 1 : creationCountX) * separationWidth;
        _totalSeparationAreaHeight = (creationCountZ > 1 ? creationCountZ - 1 : creationCountZ) * separationHeight;
        _areaX = (creationCountX * areaWidthElements) + _totalSeparationAreaWidth;
        _areaZ = (creationCountZ * areaHeightElements) + _totalSeparationAreaHeight;
    }

    //Create matrix position with a list of positions.
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

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, ConstGizmos.PATH_GIZMO_FLOOR, true);
    }

    private void OnDrawGizmosSelected()
    {
        if (_creationGameObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _creationGameObject.transform.position);

            //If this view in center.
            Gizmos.color = Color.green;
            Gizmos.matrix = _creationGameObject.transform.localToWorldMatrix;

            //The modules in area creation.
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_areaX, -0.1f, _areaZ));

            //Create elements view.
            Gizmos.color = Color.blue;
            var elements = GetPositions(-((_areaX / 2) - areaWidthElements / 2), -((_areaZ / 2) - areaHeightElements / 2));

            foreach (var element in elements)
            {
                Gizmos.DrawWireCube(
                    new Vector3(element.Item1, 0, element.Item2),
                    new Vector3(areaWidthElements, 0, areaHeightElements
                    ));
            }
        }
    }
}
