using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModelTiler : MonoBehaviour {

    public GameObject modelToTile;

    public int tileX = 1;
    int m_tileX;
    public float offsetX = 1;
    float m_offsetX;

    public int tileY = 1;
    int m_tileY;
    public float offsetY = 1;
    float m_offsetY;

    List<GameObject> tiledModels = new List<GameObject> ();

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (m_tileX != tileX || m_tileY != tileY || m_offsetX != offsetX || m_offsetY != offsetY) {
            Tile();
        }
    }

    void Tile() {
        m_tileX = tileX;
        m_offsetX = offsetX;
        m_tileY = tileY;
        m_offsetY = offsetY;

        foreach (GameObject model in tiledModels) {
            DestroyImmediate (model);
        }

        tiledModels.Clear ();

        GameObject tempObj;
        for (int i = 0; i < m_tileX; i++) {
            for (int j = 0; j < m_tileY; j++) {
                tempObj = Instantiate(modelToTile, transform);
                tempObj.transform.localPosition = new Vector3(offsetX * i, 0, offsetY * j);
                tiledModels.Add(tempObj);
            }
        }
    }
}
