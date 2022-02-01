using UnityEngine;

/// <summary>Controls camera movement and looking</summary>
public class CameraOrbitController : MonoBehaviour
{
    #region Variables

    // movement related vars
    [Header("Camera movement settings")] 
    [Tooltip("Movement speed (forward and left/right)")] 
    [SerializeField]
    private int movementSpeed = 25;

    // Rotation related variables
    [Tooltip("Rotation sensitivity on X axis")] 
    [SerializeField]
    private int sensitivityX = 5;

    [Tooltip("Rotation sensitivity on Y axis")] 
    [SerializeField]
    private int sensitivityY = 5;

    [Tooltip("Min X rotation (in angles)")] 
    [SerializeField]
    private float minimumX = -360f;

    [Tooltip("Max X rotation (in angles)")] 
    [SerializeField]
    private float maximumX = 360f;

    [Tooltip("Min Y rotation (in angles)")] 
    [SerializeField]
    private float minimumY = -60f;

    [Tooltip("Max Y rotation (in angles)")] 
    [SerializeField]
    private float maximumY = 60f;

    private float _rotationX = 0F;
    private float _rotationY = 0F;
    private Quaternion _originalRotation;

    #endregion

    #region LifeCycle methods

    private void Update()
    {
        CheckMovement();
        CheckRotation();
    }

    void Start()
    {
        _originalRotation = transform.localRotation;
    }

    #endregion

    #region Movement Handling

    /// <summary>
    /// Handles camera rotation
    /// </summary>
    private void CheckRotation()
    {
        _rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        _rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

        // clamp angle to prevent values to infinite angle (keep the angle between maximum and minimum)
        _rotationX = ClampAngle(_rotationX, minimumX, maximumX);
        _rotationY = ClampAngle(_rotationY, minimumY, maximumY);

        var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up); // rotate around X axis
        var yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right); // rotate around Y axis

        transform.localRotation = _originalRotation * xQuaternion * yQuaternion;
    }

    /// <summary>
    /// Handles camera movement
    /// </summary>
    private void CheckMovement()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");
        var nextMovementSpeed = (Time.deltaTime / Time.timeScale) * movementSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            nextMovementSpeed *= 2f;
        }

        if (verticalMovement != 0f)
        {
            transform.Translate(Vector3.forward * (nextMovementSpeed * verticalMovement));
        }

        if (horizontalMovement != 0f)
        {
            transform.Translate(Vector3.right * (nextMovementSpeed * horizontalMovement));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // move up
            transform.Translate(Vector3.up * nextMovementSpeed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            // move up
            transform.Translate(Vector3.down * nextMovementSpeed);
        }
    }

    #endregion

    #region Utils

    /// <summary>
    /// Clamp the angle between minimum and maximum values for both axes
    /// </summary>
    /// <param name="angle">Current angle</param>
    /// <param name="min">Min value</param>
    /// <param name="max">Max value</param>
    /// <returns>The clamped angle</returns>
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        else if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }

    #endregion
}