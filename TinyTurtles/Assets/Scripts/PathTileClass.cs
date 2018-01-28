using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTileClass : MonoBehaviour {

    [SerializeField]
    Camera PathCameraPrefab; //camera that watches the path tile
    [SerializeField]
    GameManager GameManager;

    int[,] Paths;
    bool isCam = false;

    // Start initial methods
    void Start () {
    //    if (isCam == false)CreateCamera();
	}

    void CreateCamera()
    {
        Camera Cam = (Camera)
            Instantiate(
            PathCameraPrefab,
            new Vector3(0, 0, 0),
            Quaternion.Euler(0f, 0f, 0f),
            this.transform
            );
        Cam.GetComponent<Transform>().position = new Vector3(0f, -10f, -3f);
        isCam = true;
    }
}