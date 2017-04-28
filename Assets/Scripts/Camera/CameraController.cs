using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject winks;
    public CameraState cameraState;
    
	void Start()
	{
        cameraState = CameraState.Stationary;
	}

    void LateUpdate()
    {
        float offset = Camera.main.orthographicSize * Camera.main.aspect / 2;
        Vector3 winksScreenPosition = Camera.main.WorldToViewportPoint(winks.transform.position);

        if (winksScreenPosition.x < 0.25f || winksScreenPosition.x > 0.75f)
            cameraState = CameraState.Following;

        if (cameraState == CameraState.Following && PlayerState.Instance.Horizontal == Horizontal.Idle)
            cameraState = CameraState.Recentering;
        else if (cameraState == CameraState.Following)
            transform.position = new Vector3(winks.transform.position.x - offset * (int)PlayerState.Instance.DirectionFacing, transform.position.y, transform.position.z);

        if (cameraState == CameraState.Recentering)
        {
            float x = Mathf.Lerp(transform.position.x, winks.transform.position.x, 0.02f * Time.deltaTime * 60);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

            if (Math.Round(winksScreenPosition.x, 1) == 0.5f)
                cameraState = CameraState.Stationary;
        }
	}
}

public enum CameraState
{
    Stationary,
    Following,
    Recentering
}