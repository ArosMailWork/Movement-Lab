using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	#region Variables
	//Input
	public Vector2 move;
	public bool CrouchingInput;
	public bool allowMove = true;

	//Movement Stats
	[SerializeField] float speed;
	public float jumpforce = 4f;
	public float walkspeed = 5f;
	public float KickMoveSpeed = 0.5f;
	public float airstrafe = 1f;

	[SerializeField] float GroundRemember;
	[SerializeField] private float GroundRememberTime;

	public float jumpCooldown_time;
	public float jumpCooldown;
	public bool jumpCD;

	//Crouch Stats
	public float crouchheight = 1.25f;
	public float crouchspeed = 15f;
	public float normalheight = 2f;

	//Slide Stats
	public float SpeedRequired_Slide = 17.5f;
	[HideInInspector] public bool sliding;

	public float slideforce = 2f;
	public float slideModifier = 1.1f;

	public float SlideTime_Default = 1.2f;
	public float SlideTimer = 0f;

	public float SlideDelayTimer_Default = 0.75f;
	public float SlideDelayTimer = 0f;

	//Stair
	[Header("Stair")]
	public float maxStepHeight = 1f;
	public int stepDetail = 10;
	public LayerMask stepMask;

	//Slope
	[Header("Slope")]
	public float maxSlopeAngle = 70f;
	[SerializeField] float SlopeAngle;
	[SerializeField] bool movingUpSlope;

	//State (need to fix)
	public bool isGrounded;
	private bool canJump;
	public bool intoWall;

	[SerializeField] bool canMoveFast;
	public bool canSlide;
	private enum State { Idle , Walk, Crouching , Sprinting , Sliding , Teleporting , PrepareKick }
	[Space(5)]
	private State currentState;
	private State lastState;

	public enum SlideState { Start, Stop, ToCrouch, Pause };
	private SlideState currentSlideState;
	public string currentState_string;
	public string lastState_string;
	public string currentSlideState_string() { return currentSlideState.ToString(); }
	public float PlayerCurrentSpeed;
	
	[Space(5)]


	//Physics
	float gravityvalue;
	public float extragravity = 14f;
	public float Multiplier = 80f;
	public float countermovement = 400f;
	public float MaxSpeed = 28f;

	public int wallCheckSensorCount;
	public float WallCheckLength = 1;

	//condition
	public static Vector3 PlayerVector;
	[HideInInspector] public Vector3 pmovement;
	[HideInInspector] Vector3 reflectVector;
	[HideInInspector] public bool isMovingForward;
	[HideInInspector] bool isGrappling;
	[SerializeField] bool ObjectOnTopDetected;
	float approachAngle;
	float VelocityForLanding;
	float t_parkour;
	float chosenParkourMoveTime;
	bool GoupDelay;
	bool canLanding;
	bool canHardLanding;
	bool WallInfront;
	public bool edgeBoostQueue1;
	public bool edgeBoostQueue2;
	private Vector2 moveForward;
	Vector3 old_direction = Vector3.zero; //slide
	//pk
	bool CanClimb;
	float IK_Rot_X;
	Vector3 RecordedMoveToPosition; //the position of the vault end point in world space to move the player to
	Vector3 RecordedStartPosition; // position of player right before vault

	[HideInInspector] public bool isReadyToKick;
	[HideInInspector] public bool Kicking;
	[HideInInspector] public bool IsParkour;
	private RaycastHit slopeHit;
	private bool exitingSlope;

	//bhop 
	float currentspeed;
	float addspeed;
	float accelspeed;
	Vector3 playerVelocity;
	[SerializeField] float strafe_time_holding = 1.5f;
	float last_x_move_key;
	float last_x_key;
	[SerializeField] bool Debug_strafe_limit;

	//Parkour Setup
	[SerializeField] DetectObs detectClimbObject; //checks for climb object
	[SerializeField] DetectObs detectClimbObstruction; //checks if theres somthing in front of the object e.g walls that will not allow the player to climb
	public bool Climbable = true;
	public float ClimbTime = 0.38f; //how long the vault takes
	public Transform ClimbEndPoint;
	[SerializeField] GameObject LedgeClimbTarget;
	public float edgeBoostForce = 10f;
	public float edgeBoostForceUp = 4f;
	[Range(0,1)] public float edgeBoostTime = 2/3;
	public float EdgeBoostDelay = 0.1f;

	//Setup
	private CapsuleCollider playercollider; //will change
	Animator HeadAnimator;
	_controls Move_control;
	Rigidbody PlayerBody;
	public DetectObs DetectWallHit;
	[SerializeField] Animator Leg_Animation;
	[SerializeField] GameObject CamHead;
	[SerializeField] PlayerInput PControls = null;
	[SerializeField] DetectObs GroundChecker;
	[SerializeField] Transform WallCheckPoint;
	[SerializeField] Camera PCam;
	[SerializeField] PlayerCam PCamScript;
	public LayerMask GroundMask;
	[SerializeField] bool allow_bhop_back;

	//Dashing
	[SerializeField] private float DashForce;
	[Range(0,1)] [SerializeField] private float DashMomentum = 0.75f;
	[SerializeField] private float DashCoolDown = 0.5f;
	[SerializeField] private float DashDuration = 0.5f;
	bool Dashing;
	[SerializeField] bool canDash;
	[SerializeField] bool EnableDash;
	
	//Air Jump
	[Range(0,5)][SerializeField] private int AirJumpAmount;
	[SerializeField] private float AirJumpForce;
	[SerializeField] private float AirJumpGravityReset = 1f;
	public int AirJumpRemains;
	public bool canAirJump;
	public bool EnableAirJump;
	
	//Slam
	[SerializeField] float SlamDistance;
	[SerializeField] private float SlamForce;
	[Range(0,5)][SerializeField] private float SlamDelay;
	[Range(0,1)][SerializeField] private float SlamShakeEffect;
	public bool Slaming;
	public bool canSlam;
	
	//Calculate var
	bool OnSlope()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playercollider.height * 0.5f + 0.3f, LayerMask.GetMask("Ground")))
		{
			float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
			SlopeAngle = angle;
			if (Mathf.Round(PlayerBody.velocity.y) > 0f)
			{
				movingUpSlope = true;
			}
			else
			{
				movingUpSlope = false;
			}

			return angle < maxSlopeAngle && angle != 0;
		}

		movingUpSlope = false;
		return false;
	}
	bool Diagonal_move()
	{
		if (move.y > 0 && move.x != 0 && !allow_bhop_back) return true;
		else if (move.y != 0 && move.x != 0 && allow_bhop_back) return true;
		else return false;
	}
	public bool CoolDownFunction()
	{
		if (jumpCooldown > 0)
		{
			jumpCooldown -= Time.deltaTime;
			return false;
		}
		else return true;
	}
	
	//Effect
	private CamUlts CamEffect;
	
	#endregion

    #region Input
    bool overiding;
	Vector2 InputOveride;
	public void InputOveride_Bool(InputAction.CallbackContext ctx)
	{
		overiding = ctx.performed;
		InputOveride = ctx.ReadValue<Vector2>();
	}
    void Input()
	{
		if (PCamScript.Paused || !allowMove) return;

		if (!overiding) move = Vector3.zero; // read
		else move = InputOveride;

		pmovement = (move.y * transform.forward) + (move.x * transform.right);

		if (move.x == 1 || move.x == -1)
		{
			last_x_move_key = move.x;

			//if last_x_move_key change -> reset

			StrafeTime_reset(1.5f, last_x_move_key);
		}
	}

	[SerializeField] float scroll;
	public bool jumpscroll_toggle;
	float scroll_jumpTimer = 0f; // Add a timer variable to prevent multiple jumps
	[SerializeField] float scroll_jumpCooldown = 0.3f; // Set the cooldown time to half a second
	public bool JumpInput;
	public void Jump_Scroll()
	{
		if (!jumpscroll_toggle) return;

		Vector2 vec = Mouse.current.scroll.ReadValue();
		scroll = vec.y;

		jumpCD = CoolDownFunction();
		if (scroll_jumpTimer > 0)
		{
			JumpInput = false;
			scroll_jumpTimer -= Time.deltaTime;
		}

		if (scroll != 0 && jumpCD && scroll_jumpTimer <= 0)
		{
			JumpInput = true;
			Jump();
			scroll_jumpTimer = scroll_jumpCooldown;
		}
	}

	public void Jump_Input(InputAction.CallbackContext ctx)
	{
		JumpInput = ctx.performed;
		if (ctx.performed && scroll == 0) Jump();
	}
	public void Crouch_Input(InputAction.CallbackContext ctx)
	{
		if (PCamScript.Paused)
		{ return; }

		CrouchingInput = ctx.performed;
	}
	#endregion

	#region Sliding State
	void StateMachine()
	{
		if (SlideDelayTimer > 0f) SlideDelayTimer -= Time.deltaTime;

		if (move == Vector2.zero && PlayerBody.velocity.magnitude < 1f)
		{
			currentState = State.Idle;
		}
		else if (move != Vector2.zero && PlayerBody.velocity.magnitude < SpeedRequired_Slide)
		{
			currentState = State.Walk;
		}
		else if (PlayerBody.velocity.magnitude >= SpeedRequired_Slide)
		{
			currentState = State.Sprinting;
		}

		if (CrouchingInput && !canSlide || ObjectOnTopDetected) currentState = State.Crouching;

		if (canSlide && CrouchingInput && isGrounded ||
			 currentState == State.Crouching && !movingUpSlope && OnSlope() && PlayerBody.velocity.magnitude > SpeedRequired_Slide)
			sliding = true;

		if (CrouchingInput && sliding)
		{
			currentState = State.Sliding;
		}
		else
		{
			sliding = false;
		}


		if (currentState == State.Sprinting && SlideDelayTimer <= 0f) canSlide = true;
		else canSlide = false;

		currentState_string = currentState.ToString();

		StateApply();
	}

	Vector3 perpendicularToFloor;
	void StateApply()
	{
		if (!isGrounded) airstrafe = 0.8f;
		else airstrafe = 1.0f;

		if (move.y == -1) canMoveFast = false;

		if (currentState == State.PrepareKick) speed = KickMoveSpeed;
		else if (!canMoveFast && CrouchingInput) speed = walkspeed / 2.5f;
		else speed = walkspeed;

		//Slide Vector Update
		if (currentState != State.Sliding)// can change to test
		{
			old_direction = new Vector3(move.x, 0, move.y); //slide vector off
			old_direction.Normalize();
			old_direction = transform.TransformDirection(old_direction);

			float slopeAngleInRadians = SlopeAngle * Mathf.Deg2Rad;
			float normalY = Mathf.Cos(slopeAngleInRadians);
			Vector3 floorNormal = new Vector3(old_direction.x, -normalY, old_direction.z);

			perpendicularToFloor = old_direction - Vector3.ProjectOnPlane(old_direction, floorNormal);
		}
		else
		{
			PlayerBody.AddForce(perpendicularToFloor.normalized * (slideforce * Multiplier * 1.75f), ForceMode.Acceleration);
			SlideDelayTimer = SlideDelayTimer_Default; //active delay
		}
		

		//add a timer to delay canSlide (if u want)
		Slide_Stop_Time();

		//set height
		if (CrouchingInput && !Slaming && allowMove || ObjectOnTopDetected && !Slaming && allowMove) CrouchHeight();
		else GoUp();
	}



	void Slide_Stop_Time()
	{
		if (!OnSlope() && (currentState == State.Sliding) || movingUpSlope)
		{
			if (SlideTimer > 0) SlideTimer -= Time.deltaTime;
		}
		else
		{
			SlideTimer = SlideTime_Default;
		}

		if (SlideTimer < 0) EndSlide();
	}

	void EndSlide()
	{
		sliding = false;
		
		currentState = State.Walk;
	}


	#endregion

	#region Logic movement stuffs, ewww
	void LimitSpeed()
	{
		Mathf.Clamp(speed, 0, MaxSpeed);
		if (PlayerBody.velocity.magnitude > MaxSpeed)
		{
			PlayerBody.velocity = PlayerBody.velocity.normalized * MaxSpeed;
		}
	}
	void Stair()
	{
		bool isFirstStepCheck = false;

		for (int i = stepDetail; i >= 1; i--)
		{
			Collider[] c = Physics.OverlapBox(transform.position + new Vector3(0, i * maxStepHeight / stepDetail - transform.localScale.y, 0), new Vector3(1.05f, maxStepHeight / stepDetail / 2, 1.05f), Quaternion.identity, stepMask);

			if (currentState != State.Idle)
			{
				if (c.Length > 0 && i == stepDetail)
				{
					isFirstStepCheck = true;
				}

				if (c.Length > 0 && !isFirstStepCheck)
				{
					PlayerBody.transform.position += new Vector3(0, i * maxStepHeight / stepDetail, 0);
				}
			}
		}
	}
	void Slope()
	{
		if (OnSlope() && !exitingSlope)
		{
			PlayerBody.AddForce(speed * 2f * GetSlopeMoveDirection(), ForceMode.Force);

			if (PlayerBody.velocity.y > 0)
				PlayerBody.AddForce(Vector3.down * 10f, ForceMode.Force);

			if (!ObjectOnTopDetected)
			{
				canJump = true;
			}
		}
	}

	private Vector3 GetSlopeMoveDirection()
	{
		return Vector3.ProjectOnPlane(PlayerVector, slopeHit.normal).normalized;
	}

	void GroundCheck()
	{
		if (!isGrounded && OnSlope()) isGrounded = true;
		else isGrounded = GroundChecker.Obstruction;

		if (isGrounded) AirJumpRemains = AirJumpAmount;

		int layer_mask = LayerMask.GetMask("Ground", "GroundMap");
		ObjectOnTopDetected = Physics.Raycast(PlayerBody.transform.position, Vector3.up, 1.25f, layer_mask);

		if (isGrounded) GroundRemember = GroundRememberTime;
		else GroundRemember -= Time.deltaTime;

		if (GroundRemember >= 0 || canAirJump) canJump = true;
		else canJump = false;

		if (ObjectOnTopDetected && isGrounded && sliding) GoupDelay = true;
		else GoupDelay = false;

		if (Slaming && isGrounded)
		{
			StartCoroutine(SlamStop());
		}
	}

	void Move()
	{
		if (currentState == State.PrepareKick || currentState == State.Crouching)
		{
			PlayerVector = pmovement * (speed * airstrafe * Time.deltaTime);
		}
		else if (currentState != State.Sliding && currentState != State.Idle)
		{
			if (!Diagonal_move())
			{
				Accelerate(pmovement, speed);
			}

			playerVelocity.y = pmovement.y;
			PlayerVector = playerVelocity * (airstrafe * Time.deltaTime);
		}

		if (move == Vector2.zero && currentState == State.Idle)
		{
			//do nothing, lol
		}
		else if (move.y <= 0 || currentState == State.Crouching)
		{
			PlayerBody.AddForce(Multiplier * 1.75f * PlayerVector, ForceMode.Force);
		}
		else
		{
			PlayerBody.AddForce(pmovement.normalized * Multiplier / 2, ForceMode.Acceleration);
		}

		CounterMovement();
	}
	//wish dir is input
	public void Accelerate(Vector3 wishdir, float accel)
	{
		if (wishdir == Vector3.zero)
		{
			playerVelocity = Vector3.zero;
		}

		wishdir.Normalize();

		float wishspeed = wishdir.magnitude;
		wishspeed = speed;

		currentspeed = Vector3.Dot(playerVelocity, wishdir);
		addspeed = wishspeed - currentspeed;
		if (addspeed <= 0)
			return;
		accelspeed = accel * Time.deltaTime * wishspeed * 0.5f;
		if (accelspeed > addspeed)
			accelspeed = addspeed;

		playerVelocity.x += accelspeed * wishdir.x;
		playerVelocity.z += accelspeed * wishdir.z;


		if (currentState == State.Idle)
		{
			Debug_strafe_limit = false;
			return;
		}

		if (!Diagonal_move() && strafe_time_holding >= 0 && move.x != 0)
		{
			//allow more velocity
			playerVelocity = Vector3.ClampMagnitude(playerVelocity, MaxSpeed * 1.5f);
			Debug_strafe_limit = false;
		}
		else
		{
			Debug_strafe_limit = true;
			playerVelocity = Vector3.ClampMagnitude(playerVelocity, MaxSpeed);
		}
	}
	void StrafeTime_reset(float time_remain, float new_input)
	{
		if (last_x_key == new_input) return;

		//if different, reset
		last_x_key = new_input;
		strafe_time_holding = time_remain;
	}
	void Strafetimer()
	{
		if (strafe_time_holding <= 0) return;
		if (strafe_time_holding > 0) strafe_time_holding -= Time.deltaTime;
	}

	

	void Jump()
	{
		if(Slaming || !allowMove) return;
		if (intoWall && !isGrounded)
		{
			//Wall Bounce
			Vector3 boost = Vector3.ProjectOnPlane(pmovement * (speed * 5 * (approachAngle / 90f) * Time.fixedDeltaTime), reflectVector);
			Vector3 WallBounceVec = (boost + reflectVector * 1.5f).normalized * 1.1f;

			Vector3 WallBounceVector = PlayerBody.velocity.magnitude / 10 * ((WallBounceVec) + (Vector3.up * 1.5f));

			PlayerBody.velocity = Vector3.zero;

			if (!Debug_strafe_limit)
			{
				PlayerBody.AddForce(WallBounceVector * jumpforce / 10, ForceMode.Impulse);
			}
			else
			{
				PlayerBody.AddForce(WallBounceVector * jumpforce / 20, ForceMode.Impulse);
			}


			Climbable = false;
			Invoke("Reset_Climb_State", 0.1f);
			Debug.DrawRay(transform.position, reflectVector, Color.yellow, 60f);
			Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(pmovement * (speed * 5 * (approachAngle / 90f) * Time.fixedDeltaTime), reflectVector), Color.red, 60f);
			jumpCooldown = jumpCooldown_time;

			if (AirJumpRemains >= 1)
				AirJumpRemains = 1;
			else
				AirJumpRemains--;
			Debug.Log("Wall Bounce");
			return;
		}

		if (!intoWall && !isGrounded && canAirJump)
		{
			if (EnableAirJump)
			{
				int JumpNo = AirJumpAmount - AirJumpRemains + 1;
				Debug.Log("Air Jump " + JumpNo);
				PlayerBody.AddForce(Vector2.up * AirJumpForce, ForceMode.Acceleration);
			}

			
			//make calgravity higher after every single Jump
			float calgravity = (AirJumpGravityReset * 0.75f) + (AirJumpGravityReset * (1 - (AirJumpRemains / AirJumpAmount))); 
			gravityvalue = calgravity;
			AirJumpRemains--;
			return;
		}

		if (!canJump) return;
		
		if (currentState == State.Sliding && !ObjectOnTopDetected)
		{
			//slide jump
			if (!OnSlope())
			{
				Debug.Log("Slide Jump");
				PlayerBody.AddForce(Vector3.up * (jumpforce * 1.2f) + (old_direction * 25f), ForceMode.Acceleration);
			}
			else
			{ 
				Debug.Log("Slope Jump");
				PlayerBody.AddForce(Vector3.up * (jumpforce * 2) + (old_direction * 20f), ForceMode.Acceleration);
			}

			if (OnSlope())
			{ 
				CrouchingInput = false;
			}
			EndSlide();
		}
		else if (currentState == State.Sprinting || currentState == State.Walk || currentState == State.Idle)
		{
			//normal jump
			//Debug.Log("Normal Jump");
			PlayerBody.AddForce(Vector2.up * jumpforce, ForceMode.Acceleration);
		}
		else if (currentState == State.PrepareKick)
		{
			//kick jump
			if (PCamScript.yRotation >= 0)
			{
				return;
			}
			else
			{
				Vector3 JumpVector = PCam.transform.TransformDirection(Vector3.forward) * 2 + Vector3.up;
				PlayerBody.AddForce(JumpVector * jumpforce / 50, ForceMode.Impulse);
			}
		}
		else if (currentState == State.Crouching && !ObjectOnTopDetected)
		{
			//crouch jump
			Debug.Log("Crouch Jump");
			PlayerBody.AddForce(Vector2.up * (jumpforce * 1.25f), ForceMode.Acceleration);
			sliding = false;
			//EndSlide();
		}
		
	}

	public void Reset_Climb_State()
	{
		Climbable = true;
	}

	void GoUp()
	{
		SlideTimer = SlideTime_Default;
		
		playercollider.height = Mathf.Lerp(playercollider.height, normalheight, Time.deltaTime * crouchspeed);

		//SlideTrail.Stop(); stop particle here
	}

	void CrouchHeight()
	{
		playercollider.height = Mathf.Lerp(playercollider.height, crouchheight, Time.deltaTime * crouchspeed);
	}
	#endregion

	#region Parkour stuffs

	void Parkour()
	{
		if (!Climbable)
		{
			return;
		}

		float edgeBoostTimer = ClimbTime * edgeBoostTime;

		if (CanClimb)
		{
			Debug.Log("Climbing");
			CanClimb = false; // so this is only called once
			PlayerBody.isKinematic = true; //ensure physics do not interrupt the vault
			RecordedMoveToPosition = ClimbEndPoint.position;
			RecordedStartPosition = transform.position;
			IsParkour = true;
			chosenParkourMoveTime = ClimbTime;

			/*if (RightHand_Animator.isActiveAndEnabled)
			{
				RightHand_Animator.SetBool("Climbing", true);
			}

			LeftHandIK.enabled = true;
			RightHandIK.enabled = true;*/
		}


		//Parkour movement
		if (IsParkour && t_parkour < 1f)
		{
			t_parkour += Time.deltaTime / chosenParkourMoveTime;
			transform.position = Vector3.Lerp(RecordedStartPosition, RecordedMoveToPosition, t_parkour);

			IK_Rot_X = Mathf.Lerp(90f, 100f, t_parkour);
			LedgeClimbTarget.transform.localRotation = Quaternion.Euler(IK_Rot_X, transform.localRotation.eulerAngles.y, 2);

			
			//apply edge boost if applicable
			if (edgeBoostTimer > 0f && t_parkour >= edgeBoostTimer)
			{
				if (JumpInput && move.y > 0f)
				{
					edgeBoostQueue2 = true;
				}

				edgeBoostTimer = 0f; //reset edge boost time
			}


			if (t_parkour >= 1f)
			{
				IsParkour = false;
				t_parkour = 0f;
				PlayerBody.isKinematic = false;

				//DisableHandIK();
				Invoke("EdgeBoostApply", EdgeBoostDelay);
			}
		}
	}

	void EdgeBoostApply()
	{
		if (!edgeBoostQueue2) return;

		//Edge Boosting will work based on Player Camera Direction
		//It will bring some skill gap by looking at the best direction
		Debug.Log("EdgeBoost");
		Vector3 EdgeBoostVector = Vector3.ProjectOnPlane(PCam.transform.forward, Vector3.up);

		PlayerBody.AddForce((EdgeBoostVector + Vector3.up * edgeBoostForceUp) * edgeBoostForce, ForceMode.Impulse);
		edgeBoostQueue2 = false;
	}

	#endregion
	
	#region Physics and detects, etc
	void ExtraGravity()
	{
		//Extra Gravity
		if (!isGrounded && !isGrappling && !Dashing)
		{
			gravityvalue += Time.fixedDeltaTime;

			PlayerBody.AddForce(Vector3.down * (extragravity * 2f * gravityvalue), ForceMode.Acceleration);
		}
		else if (isGrounded && !Dashing || !isGrounded && isGrappling && !Dashing)
		{
			gravityvalue = 1;
			PlayerBody.AddForce(Vector3.down * extragravity, ForceMode.Acceleration);
		}

		if (Dashing)
		{
			//do nothing owo
		}
	}
	void Landing()
	{
		if (!isGrounded && PlayerBody.velocity.y < -VelocityForLanding && !sliding) canLanding = true;
		else canLanding = false;
	}
	void CounterMovement()
	{
		if (!isGrounded) return;

		if (move == Vector2.zero && PlayerBody.velocity.magnitude >= 2f)
		{ 
			PlayerBody.AddForce(-PlayerBody.velocity * (countermovement * Time.fixedDeltaTime));
		}
	}
	void AngleChecker()
	{
		//https://www.youtube.com/watch?v=EAnvz0CFuRk&list=LL&index=8&t=891s 

		float rotationStep = (360f / wallCheckSensorCount);

		int validHits = 0;
		Vector3 Average = Vector3.zero;

		GameObject sensorHub = new GameObject("WCheck Hub");
		GameObject sensorPoint = new GameObject("WCheck Point");

		sensorHub.transform.position = transform.position + (Vector3.up * 0.8f);

		for (int b = 0; b < wallCheckSensorCount; ++b)
		{
			sensorPoint.transform.position = sensorHub.transform.position;
			sensorHub.transform.rotation = Quaternion.Euler(0, rotationStep * b, 0);

			sensorPoint.transform.position = sensorHub.transform.position;

			Ray ray = new Ray(sensorPoint.transform.position, sensorHub.transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, WallCheckLength))
			{
				if (hit.transform.gameObject.tag == "Ground")
				{
					++validHits;

					Average += -sensorHub.transform.forward.normalized;

					Debug.DrawRay(sensorPoint.transform.position, sensorHub.transform.forward * WallCheckLength, Color.green);
				}
			}
		}

		if (validHits != 0)
		{
			Average = Average / (validHits);
			reflectVector = Average.normalized;
			Average += (transform.up);
		}
		else
		{
			Average = Vector3.zero;
		}


		Debug.DrawRay(transform.position, reflectVector * 2, Color.black);

		approachAngle = Vector3.Angle(pmovement, -reflectVector);
		float maxAngle = 85;

		if (approachAngle < maxAngle && validHits > 0)
		{
			intoWall = true;
		}
		else
		{
			intoWall = false;
		}

		Destroy(sensorHub);
		Destroy(sensorPoint);
	}
	#endregion

	#region Special Movements

	public void Teleport()
	{
		Teleport_Apply(new Vector3(-45.39f, 63.58f, -53.48f));
	}

	void Teleport_Apply(Vector3 pos)
	{
		PlayerBody.transform.position = pos;
	}

	public void Dash()
	{
		if(!EnableDash || Slaming) return;
		StartCoroutine(DashApply());
	}

	IEnumerator DashApply()
	{
		if (!canDash) yield break;

		Vector3 VelBuffer = pmovement.normalized * (DashForce * DashMomentum);

		//Start Dashing
		PlayerBody.useGravity = false;
		PlayerBody.velocity = Vector3.zero;
		
		//PlayerBody.AddForce(Camera.main.transform.forward.normalized * DashForce, ForceMode.Acceleration);
		if (pmovement == Vector3.zero) pmovement = Camera.main.transform.forward;
		
		if(Physics.Raycast(transform.position, pmovement, 2f))
		PlayerBody.AddForce(pmovement.normalized * DashForce/5, ForceMode.Acceleration);
		else
		PlayerBody.AddForce(pmovement.normalized * DashForce, ForceMode.Acceleration);
		
		Dashing = true;
		canDash = false;

		yield return new WaitForSeconds(DashDuration);
		
		//Stop Dashing
		PlayerBody.useGravity = true;
		PlayerBody.velocity = VelBuffer; //Vector3.zero;
		Dashing = false;
 
		//Cooldown Dash
		yield return new WaitForSeconds(DashCoolDown);
 
		canDash = true;
	}

	void AirJumpCheck()
	{
		if (AirJumpRemains > 0)
		{
			canAirJump = true;
			return;
		}

		canAirJump = false;
	}


	public void Slam()
	{
		if(Slaming || !canSlam || !allowMove || isGrounded) return;
		StartCoroutine(SlamApply());
	}

	IEnumerator SlamApply()
	{
		RaycastHit hit;
		if (!Physics.SphereCast(transform.position, 1f, Vector3.down, out hit, SlamDistance))
		{
			Debug.Log("Slam!!");
			PlayerBody.velocity = Vector3.zero;
			yield return new WaitForSeconds(0.1f);
			Slaming = true;
			allowMove = false;
			//Play some particle and AOE here
		}
		
		yield return null;
	}

	void SlamForceApply()
	{
		if (Slaming) 
		{
			PlayerBody.AddForce(Vector3.down * SlamForce, ForceMode.Acceleration);
		}
	}

	IEnumerator SlamStop()
	{
		//add Shake Effect
		CamEffect.Shake(SlamShakeEffect);
		Slaming = false;
		yield return new WaitForSeconds(0.1f);
		PlayerBody.velocity = Vector3.zero;
		yield return new WaitForSeconds(SlamDelay);
		allowMove = true;
		yield return null;
	}

	#endregion
	
	#region Unity-based Func
	private void Awake()
    {
		PCamScript = GetComponentInChildren<PlayerCam>();
		PlayerBody = GetComponent<Rigidbody>();
		playercollider = GetComponent<CapsuleCollider>();
		CamEffect = GetComponentInChildren<CamUlts>();
    }

	private void Start()
	{
		allowMove = true;
		canDash = true;
	}

	private void FixedUpdate()
    {
		if (PCamScript.Paused) return;

		AirJumpCheck();
		Parkour();
		ExtraGravity();
		Landing();

		LimitSpeed();
		StateMachine();

		Strafetimer();
		AngleChecker();
		Move();


		if (currentState != State.Sliding)
		{
			Stair();
			Slope();
		}

		SlamForceApply();
		
		PlayerCurrentSpeed = PlayerBody.velocity.magnitude;
    }

	void Update()
	{
		GroundCheck();
		WallInfront = Physics.Linecast(transform.position, WallCheckPoint.position);

		Input();
	}

    private void LateUpdate()
    {
		Jump_Scroll();
	}

	[SerializeField] LayerMask ClimbableLayer;
	private void OnCollisionStay(Collision collision)
	{
		if (!WallInfront || !detectClimbObject.Obstruction || detectClimbObstruction.Obstruction)
		{
			return;
		}

		//Climb
		Vector3 normal = collision.GetContact(0).normal;

		if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) >= 0.1f)
			return;

		Vector3 horForward = PCam.transform.forward;

		RaycastHit rctest;
		RaycastHit rc2;
		Vector3 vector = PCam.transform.position + Vector3.up * 0.15f;
		while (!Physics.Raycast(vector, -normal, 1f, ClimbableLayer))
		{
			vector += Vector3.down * 0.1f;
			if (vector.y >= PCam.transform.position.y - 2f)
			{
				break;
			}
		}

		horForward.y = 0;
		horForward.Normalize();


		if (Vector3.Angle(horForward, -normal) <= 45)
		{
			bool LedgeAvail = true;
			RaycastHit hit;
			if (Physics.Raycast(PCam.transform.position + Vector3.up * 0.15f, -normal, out hit, 1, ClimbableLayer))
			{
				LedgeAvail = false;
			}


			if (LedgeAvail && !sliding && Climbable)
			{
				CanClimb = true;

				Vector3 a = vector - normal / 1.5f;

				Physics.Raycast(a, Vector3.down, out rc2, 1f, ClimbableLayer);

				//this is to fix when the ray down didn't recognize anything for a mystery reason that I dont know, lol
				if (rc2.point == Vector3.zero)
				{
					rc2.point = a;
					Physics.Raycast(rc2.point, Vector3.down, out rctest, 10f, ClimbableLayer);

					Vector3 LookVec = WallCheckPoint.transform.position - transform.position;
					LookVec = LookVec.normalized * 0.1f;

					Vector3 OffsetVec = new Vector3(rctest.point.x + LookVec.x, rctest.point.y + 5f, rctest.point.z + LookVec.z);

					Physics.Raycast(OffsetVec, Vector3.down, out rc2, 5f, ClimbableLayer);
				}

				LedgeClimbTarget.transform.position = rc2.point + PCam.transform.right * 0.75f;

				//Rap_parent = false;

				//LedgeClimbTarget.transform.localRotation = Quaternion.Euler(100f, transform.localRotation.eulerAngles.y, 100f);

				//modify lerp 100 to 130 by climbing time (target hand) 
				//IK_Rot_X

				//HeadAnimator.Play("Climb");
				//HeadAnimator.CrossFade("Climb", 0.05f);
			}
		}
	}
	#endregion
}