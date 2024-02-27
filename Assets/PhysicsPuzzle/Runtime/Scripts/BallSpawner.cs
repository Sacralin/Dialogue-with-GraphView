using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    private Vector3 ballDefaultPosition;
    private Quaternion ballDefaultRotation;
    private float ballResetTime = 3f;
    private float currentResetTime;
    bool hasBallBeenReset;
    private Rigidbody ballRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        hasBallBeenReset = false;
        currentResetTime = 0;
        ballDefaultPosition = ball.transform.position;
        ballDefaultRotation = ball.transform.rotation;
        ballRigidbody = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBallBeenReset == true)
        {
            ResetBall();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            hasBallBeenReset = true;
        }
    }

    private void ResetBall()
    {

        if(currentResetTime < ballResetTime)
        {
            ball.transform.position = ballDefaultPosition;
            ball.transform.rotation = ballDefaultRotation;
            ballRigidbody.velocity = new Vector3(0f, 0f, 0f);
            currentResetTime += 1f * Time.deltaTime;
        }
        else
        {
            currentResetTime = 0;
            hasBallBeenReset = false;
            
        }

        

    }
}
