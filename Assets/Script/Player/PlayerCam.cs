using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Stats")]
	public float MouseSens = 100f;
	public float ControllerSens = 2f;

	public float ControllerMultiplier = 0.2f;
	public float MouseMultiplier = 0.5f;

	[Header("PlayerRotateSmooth Properties")]
	public bool UseSmoothCam;
	[Space(10)]
	[SerializeField] private float _smoothTime;
	[SerializeField] private Transform Cam_Predict;

	[Header("State")]
	public bool using_WP;
	public bool Paused = false;
    [SerializeField] bool LockCursor = true;

	[Header("Setup")]
	//[SerializeField] Transform WP;
    [SerializeField] private Transform PlayerBody;

	//public val to use
	[HideInInspector] public float yRotation = 0;
	[HideInInspector] public Vector2 look;
	[HideInInspector] public Vector2 ControllerLook;
	[HideInInspector] public float mouseY;
	[HideInInspector] public float ControllerY;
	[HideInInspector] public float mouseX;
	[HideInInspector] public float ControllerX;
	float x_angularVel, y_angularVel, yRotation_old;
	CamUlts CamUlt;
	private _controls PControls;

	void Awake()
	{
		PControls = new _controls();

		CamUlt = GetComponentInParent<CamUlts>();

		if (LockCursor) Cursor.lockState = CursorLockMode.Locked;
	}

	void OnEnable()
	{
		PControls.Enable();
	}

	void OnDisable()
	{
		PControls.Disable();
	}

	void Start()
	{
		Cam_Predict.localRotation = transform.localRotation;
	}

	void FixedUpdate()
	{
        #region
        /*if (WP.gameObject.activeSelf)
		{
			using_WP = true;
		}
		else
			using_WP = false;*/
        #endregion

        if (!using_WP && !Paused)
		{
			if (!UseSmoothCam) Basic_Rotation();
			else Rotation_Smooth();

			MouseInput();
			ControllerInput();
		}
	}

	void MouseInput()
	{
		look = PControls.Player.Look.ReadValue<Vector2>();

		mouseY = look.y * MouseSens * MouseMultiplier * Time.fixedDeltaTime;
		mouseX = look.x * MouseSens * MouseMultiplier * Time.fixedDeltaTime;

		yRotation -= mouseY;
		yRotation = Mathf.Clamp(yRotation, -89f, 85f);
	}


	void ControllerInput()
	{
		ControllerLook = PControls.Player.ControllerLook.ReadValue<Vector2>();

		ControllerY = ControllerLook.y * ControllerSens * ControllerMultiplier * Time.fixedDeltaTime;
		ControllerX = ControllerLook.x * ControllerSens * ControllerMultiplier * Time.fixedDeltaTime;

		yRotation -= ControllerY;
		yRotation = Mathf.Clamp(yRotation, -89f, 85f);
	}

	void Rotation_Smooth()
	{

		//horizontal
		Cam_Predict.Rotate(Vector3.up * mouseX, Space.Self); //rotate cam predict
		Cam_Predict.Rotate(Vector3.up * ControllerX, Space.Self); //rotate cam predict
		PlayerBody.transform.localRotation = Quaternion.Euler(0f, Mathf.SmoothDampAngle(PlayerBody.transform.localEulerAngles.y, Cam_Predict.localEulerAngles.y, ref x_angularVel, _smoothTime), 0f);

		//verticle
		yRotation_old = yRotation;
		yRotation = Mathf.SmoothDampAngle(yRotation_old, yRotation, ref y_angularVel, _smoothTime);
		transform.localRotation = Quaternion.Euler(yRotation, 0, CamUlt.HeadTiltValue);
	}

	void Basic_Rotation()
	{
		//verticle
		yRotation = Mathf.Clamp(yRotation, -89f, 85f);
		transform.localRotation = Quaternion.Euler(yRotation, 0, CamUlt.HeadTiltValue);

		//horizontal
		PlayerBody.transform.Rotate(Vector3.up * mouseX); // rotate Body
		PlayerBody.transform.Rotate(Vector3.up * ControllerX); // rotate Body
	}
}
