using UnityEngine;
using System.Collections;



public class SmoothFollow : MonoBehaviour
{
	public GameObject target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CorgiController2D _playerController;
	private Vector3 _smoothDampVelocity;
	
	
	void Awake()
	{
		transform = gameObject.transform;

	}

	void Start(){
		target = GameObject.FindGameObjectWithTag("Player");
		_playerController = target.GetComponent<CorgiController2D>();
	}
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.transform.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.Velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.transform.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.transform.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
	}
	
}
