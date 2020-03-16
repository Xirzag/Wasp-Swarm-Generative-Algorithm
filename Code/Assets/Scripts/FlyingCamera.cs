using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FlyingCamera : MonoBehaviour
{

    [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
    [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    [Range(0f, 10f)] [SerializeField] private float m_TurnYSpeed = .4f; 

    [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
    [SerializeField] private bool m_LockCursor = false;                   // Whether the cursor should be hidden and locked.


    private float m_LookAngle;                    // The rig's y axis rotation.
    private float m_TiltAngle;                    // The pivot's x axis rotation.
    private Rigidbody rb;

    protected void Awake()
        {
            // Lock or unlock the cursor.
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;

            rb = GetComponent<Rigidbody>();
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotationMovement();
        HandleMovement();
        if (m_LockCursor && Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
    }

    private void HandleMovement() {
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float upDown = (Input.GetKey(KeyCode.Space)? 1 : 0) + (Input.GetKey(KeyCode.LeftShift)? -1 : 0);
        rb.velocity = (transform.forward * v + transform.right * h + Vector3.up * upDown) * m_MoveSpeed;
    }

    private void HandleRotationMovement()
        {

            // Read the user input
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = -CrossPlatformInputManager.GetAxis("Mouse Y");

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            m_LookAngle += x*m_TurnSpeed;
            m_TiltAngle = Mathf.Clamp(m_TiltAngle + y * m_TurnYSpeed, m_TiltMin, m_TiltMax);

            // Rotate the rig (the root object) around Y axis only:
            transform.localRotation = Quaternion.Euler(m_TiltAngle, m_LookAngle, 0f);
        }

}
