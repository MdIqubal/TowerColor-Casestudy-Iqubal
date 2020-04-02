using UnityEngine;

/// <summary>
/// Controls a ball.
/// Operation : ball throw, block hit detection.
/// </summary>
public class BallScript : MonoBehaviour {

    [SerializeField]
	public Transform _followTarget;

	private Vector3 _offset;
	private Rigidbody _rBody;
	public MeshRenderer _renderer;
	private Collider _collider;
	private bool _isObjectHit;
	private float _disableTime;
	private bool _targetAcquired;

	// Use this for initialization
	void Awake () {
		_rBody = GetComponent<Rigidbody>();
		_collider = GetComponent<Collider>();
	}

	private void OnEnable()
	{
		CancelInvoke("DisableSelf");
	}

	public void SetUp(Vector3 offset, float disableTime, Transform followTarget, Material mat)
	{
		_offset = offset;
		_disableTime = disableTime;
		_followTarget = followTarget;
		if (_rBody == null) {
			_rBody = GetComponent<Rigidbody>();
		}
		_rBody.isKinematic = true;
		_isObjectHit = false;
		_renderer.sharedMaterial = mat;
		_targetAcquired = false;
		_collider.enabled = false;
	}

	public void Shoot(Vector3 target)
	{
		_collider.enabled = true;
		_targetAcquired = true;
		_isObjectHit = false;
		_rBody.isKinematic = false;
		Vector3 vel = PhysicsUtil.GetParabolaInitVelocity(transform.position, target, Physics.gravity.y,heightOff : Mathf.Abs(transform.position.y - target.y) < 1 ? 1 : 0);
		_rBody.velocity = vel;
		Invoke("DisableSelf", _disableTime);
	}

	private void LateUpdate()
	{
		if (!_targetAcquired)
		{
			transform.position = _followTarget.position + _followTarget.forward * _offset.z + Vector3.up * _offset.y;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
		if (_isObjectHit) {
			return;

		}
		BlockScript block = collision.gameObject.GetComponent<BlockScript>();
		if ( block != null &&  block.CompareColor(_renderer.sharedMaterial))
		{
			AndroidVibration.Vibrate(20);
			block.Hit();
			gameObject.SetActive(false);
        }
		_isObjectHit = true;
	}


	private void DisableSelf() {
		gameObject.SetActive(false);
    }
}