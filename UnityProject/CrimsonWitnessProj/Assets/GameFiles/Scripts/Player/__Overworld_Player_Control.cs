using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Overworld_Player_Control : MonoBehaviour {

    [Header("CameraSettings")]

    [SerializeField]
    [Tooltip("How many degrees will the camera turn in a second.")]
    private Vector2 cameraSpeed = new Vector2(20.0f,10.0f);

    [SerializeField]
    [Tooltip("Because otherwise '90% of player's won't play your game'.")]
    private bool invertCameraY = false;

    [SerializeField]
    [Tooltip("Clamp values of the camera in the X axis.")]
    private Vector2 cameraClampValues = new Vector2(-80, 10);

    [Header("MovementSettings")]

    [SerializeField]
    [Tooltip("How far does the player turn in a second?")]
    private float turnSpeed = 20.0f;

    [SerializeField]
    [Tooltip("How many unity units will the player move in a second.")]
    private float moveSpeed = 2.0f;

    [Header("Display Settings")]

    [SerializeField]
    [Tooltip("The character's mesh.")]
    private GameObject characterMesh;

    public bool Stopped = false;

    // The Camera's current Yaw value.
    private Vector3 cameraRotation = new Vector3(0,0,0);

    // Reference to the Player.
    private Rigidbody rb;

    // Called on create for initialization
    void Start()   
    {
        rb = this.GetComponent<Rigidbody>();
        this.transform.position = __Game_Manager.Instance.playerStoredPosition;
        cameraRotation.y = __Game_Manager.Instance.playerStoredYaw;
        this.transform.eulerAngles = cameraRotation;
    }

    // Update is called once per frame
    void Update () {
        if(!Stopped)
            Control();
    } 

    private void Control()
    {
        // Movement        
        float ControlX, ControlY;
        ControlX = Input.GetAxis("Horizontal"); ControlY = Input.GetAxis("Vertical");
        if (ControlX > -0.5 && ControlX < 0.5) { ControlX = 0; } else { ControlX = Mathf.Sign(ControlX); }
        if (ControlY > -0.5 && ControlY < 0.5) { ControlY = 0; } else { ControlY = Mathf.Sign(ControlY); }
        if (ControlX != 0 || ControlY != 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, (Mathf.Atan2(ControlX, ControlY) * 180 / Mathf.PI + cameraRotation.y), 0));
            rb.MovePosition(this.transform.position + (this.transform.forward * moveSpeed * Time.deltaTime));
        }

        // Mesh update
        characterMesh.transform.position = this.transform.position;
        characterMesh.transform.rotation = Quaternion.Lerp(characterMesh.transform.rotation, this.transform.rotation, turnSpeed * Time.deltaTime);

        // Camera Rotation
        float LookX, LookY;
        LookX = (Input.GetAxis("Mouse X") * 1.2f) + Input.GetAxis("JoyLook X"); LookY = (-Input.GetAxis("Mouse Y") * 1.2f) + Input.GetAxis("JoyLook Y");
        if (LookX > -0.5 && LookX < 0.5) { LookX = 0; }
        if (LookY > -0.5 && LookY < 0.5) { LookY = 0; }

        float invert = 1.0f; if (invertCameraY) { invert = -1.0f; }
        
        cameraRotation += new Vector3(LookY * cameraSpeed.y, LookX * cameraSpeed.x * invert, 0.0f);
        if (cameraRotation.x < cameraClampValues.x) { cameraRotation.x = cameraClampValues.x; } else if (cameraRotation.x > cameraClampValues.y) { cameraRotation.x = cameraClampValues.y; } 
    }

    public Vector3 getCameraRotation()
    {
        return cameraRotation;
    }
}
