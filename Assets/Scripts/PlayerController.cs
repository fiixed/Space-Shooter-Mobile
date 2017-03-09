using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public float fireDelta = 0.5F;
    public Boundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public SimpleTouchPad touchPad;

    private Rigidbody rb;
    private AudioSource audiosource;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;
    private Quaternion calibrationQuarternion;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        CalibrateAccelerometer();
    }

    private void Update() {
        myTime = myTime + Time.deltaTime;
        if (Input.GetButton("Fire1") && myTime > nextFire) {
            nextFire = myTime + fireDelta;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

            nextFire = nextFire - myTime;
            myTime = 0.0F;
            audiosource.Play();
        }
    }

    void CalibrateAccelerometer() {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuarternion = Quaternion.Inverse(rotateQuaternion);
    }

    Vector3 FixAcceleration(Vector3 acceleration) {
        Vector3 fixedAcceleration = calibrationQuarternion * acceleration;
        return fixedAcceleration;
    }

    private void FixedUpdate() {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement  = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector2 direction = touchPad.GetDirection();
        Vector3 accelerationRaw = Input.acceleration;
        Vector3 acceleration = FixAcceleration(accelerationRaw);
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, -rb.velocity.x * tilt);
    }

    
}
