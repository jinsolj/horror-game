using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    FOLLOW,
    MANUAL,
}

public class CameraControl : MonoBehaviour {

    public float followTime = 0.2f;
    Vector2 followVelocity;

    CameraMode currentMode;
    PlayerManager player;
    float zDepth;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerManager>();
        zDepth = transform.position.z;

        currentMode = CameraMode.FOLLOW;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentMode)
        {
            case CameraMode.FOLLOW:
                transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, 
                    ref followVelocity, followTime, Mathf.Infinity, Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, zDepth);
                break;
            case CameraMode.MANUAL:
                break;
        }
	}

    public void ChangeMode(CameraMode mode)
    {
        currentMode = mode;
    }

    public void MoveToPosition(Vector2 position, float time)
    {
        StopCoroutine("MoveToPositionCoroutine");
        StartCoroutine(MoveToPositionCoroutine(position, time));
    }

    // Should be a Fungus command
    IEnumerator MoveToPositionCoroutine(Vector2 position, float time)
    {
        if (time <= 0)
        {
            Debug.LogError("Time cannot be zero or negative.");
        }
        else
        {
            Vector3 startPosition = transform.position;
            float timePassed = 0;

            while (timePassed < time)
            {
                timePassed += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, position, timePassed / time);
                yield return null;
            }
        }
    }
}
