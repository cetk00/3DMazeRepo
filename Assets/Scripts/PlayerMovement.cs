using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float gdrag;

    [SerializeField]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public GameObject projectilePrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        myInput();
        rb.linearDamping = gdrag;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            bullet.transform.SetParent(null);

            MoveForward mf = bullet.GetComponent<MoveForward>();
            if (mf != null)
                mf.setDirection(orientation.forward);
            else
                Debug.LogWarning("Bullet not found");

        }
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void myInput()
    {
        var kb = Keyboard.current;
        horizontalInput = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
        verticalInput = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
    }
}