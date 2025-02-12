using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot; //pivot point for the ball
    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachDelay;

    private Rigidbody2D currentBallRb;
    private SpringJoint2D currentBallSpringJoint;

    private Camera mainCam;
    private bool isDragging;
    void Start()
    {
        mainCam = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRb == null) { return; }
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
                Invoke(nameof(SpawnNewBall), respawnDelay);
            }
            isDragging = false;
            
            return;
        }

        isDragging = true;
        currentBallRb.isKinematic = true;

        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPos = mainCam.ScreenToWorldPoint(touchPos);
        Debug.Log(worldPos);

        currentBallRb.position = worldPos;

    }

    private void LaunchBall()
    {
        currentBallRb.isKinematic = false;
        currentBallRb = null;

        Invoke(nameof(DetachBall), detachDelay);
       
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
    }


    private void SpawnNewBall()
    {

        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRb = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }
}
