using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;

    private Vector3 _input;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E)) playerAnimator.SetTrigger("Bash");
        Look();
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (_input == Vector3.zero) playerAnimator.SetBool("isMoving", false);
        else playerAnimator.SetBool("isMoving", true);
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    void MovePosition(Vector3 position)
    {
        Vector3 oldVel = playerRigidbody.velocity;
        Vector3 delta = position - playerRigidbody.position;
        Vector3 vel = delta / Time.deltaTime;
        vel.y = oldVel.y;
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;
        playerRigidbody.velocity = vel;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}


