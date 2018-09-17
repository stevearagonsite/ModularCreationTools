using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour {

    public GameObject[] prefabs;
    public static Factory Instance;
    
	public enum FactoryID{NetFX}

	void Awake () {
        if (Instance == null) Instance = this;
        else Destroy(this);
	}

    public GameObject Create(FactoryID id){
        GameObject go = (GameObject)Instantiate(prefabs[(int)id]);
        return go;
    }

    public GameObject Create(FactoryID ID, Vector3 position, Quaternion rotation){
        var go = (GameObject)Instantiate(prefabs[(int)ID], position, rotation);
        return go;
    }
}
