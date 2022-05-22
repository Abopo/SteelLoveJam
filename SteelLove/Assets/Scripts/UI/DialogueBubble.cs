using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class DialogueBubble : MonoBehaviour {

    [SerializeField] Vector3 relativePos;
    [SerializeField] Transform background;

    Camera _mainCamera;


    // Start is called before the first frame update
    void Start() {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        GetComponent<DialogueRunner>().onNodeStart.AddListener(CheckPositionRelativeToCamera);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPositionRelativeToCamera(string node) {
        relativePos = _mainCamera.transform.InverseTransformPoint(transform.position - _mainCamera.transform.position);

        if(relativePos.x < -7) {
            // We're too far left, so swap to the right
            MoveToRight();
        }
    }

    void MoveToRight() {
        transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), 
                                         transform.localPosition.y, 
                                         transform.localPosition.z);

        background.transform.localScale = new Vector3(-Mathf.Abs(background.transform.localScale.x),
                                                        background.transform.localScale.y,
                                                        background.transform.localScale.z);
    }
}
