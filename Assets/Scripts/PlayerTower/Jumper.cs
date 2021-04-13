using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private int _jumpHeight;
    private bool _isGrounded;
    private Rigidbody _rigidbody;

    private Vector3 GetJumpForce
    {
        get
        {
            return Vector3.up * _jumpForce * (_jumpHeight / 7f + 1);
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _jumpHeight = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isGrounded)
        {
            _isGrounded = false;
            _rigidbody.AddForce(GetJumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Road road))
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ExtraJumpPoint extraJumpPoint))
        {
            _jumpHeight += extraJumpPoint.CountBuster;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ExtraJumpPoint extraJumpPoint))
        {
            _jumpHeight = 1;
        }
    }
}
