using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    #region SerializedProperty

    SerializedProperty PlayerState;
    SerializedProperty PlayerLastState;
    SerializedProperty PlayerSlideState;
    SerializedProperty PlayerCurrentSpeed;
    SerializedProperty CrouchingInput;
    SerializedProperty scroll;
    SerializedProperty isGrounded;
    SerializedProperty isMovingForward;
    SerializedProperty move;
    SerializedProperty PlayerVector;
    SerializedProperty airstrafe;

    SerializedProperty strafe_time_holding;
    SerializedProperty Debug_strafe_limit;
    SerializedProperty allowMove;

    SerializedProperty IsParkour;
    SerializedProperty isReadyToKick;
    SerializedProperty Kicking;

    SerializedProperty currentSpeed;
    SerializedProperty jumpforce;
    SerializedProperty walkspeed;
    SerializedProperty KickMoveSpeed;
    
    SerializedProperty AirJumpAmount;
    SerializedProperty AirJumpForce;
    SerializedProperty AirJumpGravityReset;

    SerializedProperty crouchheight;
    SerializedProperty crouchspeed;
    SerializedProperty normalheight;
    
    SerializedProperty canSlide;
    SerializedProperty sliding;
    SerializedProperty SpeedRequired;
    SerializedProperty slideforce;
    SerializedProperty slideModifier;
    SerializedProperty SlideTime_Default;
    SerializedProperty SlideTimer;
    SerializedProperty SlideDelayTimer;
    SerializedProperty SlideDelayTimer_Default;


    SerializedProperty maxStepHeight;
    SerializedProperty stepDetail;
    SerializedProperty stepMask;
    SerializedProperty maxSlopeAngle;
    SerializedProperty SlopeAngle;
    SerializedProperty movingUpSlope;

    SerializedProperty extragravity;
    SerializedProperty Multiplier;
    SerializedProperty countermovement;
    SerializedProperty MaxSpeed;

    SerializedProperty GroundChecker;
    SerializedProperty GroundMask;
    SerializedProperty ObjectOnTopDetected;
    SerializedProperty JumpInput;
    SerializedProperty WallCheckPoint;
    SerializedProperty DetectWallHit;
    SerializedProperty wallCheckSensorCount;
    SerializedProperty WallCheckLength;
    SerializedProperty jumpscroll_toggle;
    SerializedProperty Climbable;
    SerializedProperty scroll_jumpCooldown;

    SerializedProperty ClimbTime;
    SerializedProperty edgeBoostForce;
    SerializedProperty edgeBoostForceUp;
    SerializedProperty edgeBoostTime;
    SerializedProperty EdgeBoostDelay;
    SerializedProperty edgeBoostQueue;
    
    SerializedProperty AirJumpRemains;
    SerializedProperty canAirJump;
    
    SerializedProperty canDash;
    SerializedProperty DashForce;
    SerializedProperty DashMomentum;
    SerializedProperty DashDuration;
    SerializedProperty DashCoolDown;
    SerializedProperty EnableDash;
    SerializedProperty EnableAirJump;
    SerializedProperty canSlam;
    
    SerializedProperty Slaming;
    SerializedProperty SlamDistance;
    SerializedProperty SlamShakeEffect;
    SerializedProperty SlamDelay;
    SerializedProperty SlamForce;
    
    SerializedProperty LedgeClimbTarget;
    SerializedProperty ClimbEndPoint;
    SerializedProperty detectClimbObject;
    SerializedProperty detectClimbObstruction;
    SerializedProperty PCam;

    private void OnEnable()
    {
        isGrounded = serializedObject.FindProperty("isGrounded");
        isMovingForward = serializedObject.FindProperty("isMovingForward");
        PlayerState = serializedObject.FindProperty("currentState_string");
        PlayerLastState = serializedObject.FindProperty("lastState_string");
        PlayerSlideState = serializedObject.FindProperty("currentSlideState_string()");
        PlayerCurrentSpeed = serializedObject.FindProperty("PlayerCurrentSpeed");
        CrouchingInput = serializedObject.FindProperty("CrouchingInput");
        scroll = serializedObject.FindProperty("scroll");
        move = serializedObject.FindProperty("move");
        airstrafe = serializedObject.FindProperty("airstrafe");
        movingUpSlope = serializedObject.FindProperty("movingUpSlope");

        strafe_time_holding = serializedObject.FindProperty("strafe_time_holding");
        Debug_strafe_limit = serializedObject.FindProperty("Debug_strafe_limit");
        allowMove = serializedObject.FindProperty("allowMove");

        IsParkour = serializedObject.FindProperty("IsParkour");
        isReadyToKick = serializedObject.FindProperty("isReadyToKick");
        Kicking = serializedObject.FindProperty("Kicking");

        currentSpeed = serializedObject.FindProperty("speed");
        jumpforce = serializedObject.FindProperty("jumpforce");
        walkspeed = serializedObject.FindProperty("walkspeed");
        KickMoveSpeed = serializedObject.FindProperty("KickMoveSpeed");
        
        AirJumpAmount = serializedObject.FindProperty("AirJumpAmount");
        AirJumpForce = serializedObject.FindProperty("AirJumpForce");
        AirJumpGravityReset = serializedObject.FindProperty("AirJumpGravityReset");

        crouchheight = serializedObject.FindProperty("crouchheight");
        normalheight = serializedObject.FindProperty("normalheight");
        crouchspeed = serializedObject.FindProperty("crouchspeed");

        canSlide = serializedObject.FindProperty("canSlide");
        sliding = serializedObject.FindProperty("sliding");
        SpeedRequired = serializedObject.FindProperty("SpeedRequired_Slide");
        slideforce = serializedObject.FindProperty("slideforce");
        slideModifier = serializedObject.FindProperty("slideModifier");
        SlideTime_Default = serializedObject.FindProperty("SlideTime_Default");
        SlideTimer = serializedObject.FindProperty("SlideTimer");
        SlideDelayTimer = serializedObject.FindProperty("SlideDelayTimer");
        SlideDelayTimer_Default = serializedObject.FindProperty("SlideDelayTimer_Default");

        maxStepHeight = serializedObject.FindProperty("maxStepHeight");
        stepDetail = serializedObject.FindProperty("stepDetail");
        stepMask = serializedObject.FindProperty("stepMask");
        maxSlopeAngle = serializedObject.FindProperty("maxSlopeAngle");
        SlopeAngle = serializedObject.FindProperty("SlopeAngle");
        movingUpSlope = serializedObject.FindProperty("movingUpSlope");

        extragravity = serializedObject.FindProperty("extragravity");
        Multiplier = serializedObject.FindProperty("Multiplier");
        countermovement = serializedObject.FindProperty("countermovement");
        MaxSpeed = serializedObject.FindProperty("MaxSpeed");

        GroundChecker = serializedObject.FindProperty("GroundChecker");
        GroundMask = serializedObject.FindProperty("GroundMask");
        ObjectOnTopDetected = serializedObject.FindProperty("ObjectOnTopDetected");
        JumpInput = serializedObject.FindProperty("JumpInput");
        WallCheckPoint = serializedObject.FindProperty("WallCheckPoint");
        DetectWallHit = serializedObject.FindProperty("DetectWallHit");
        wallCheckSensorCount = serializedObject.FindProperty("wallCheckSensorCount");
        WallCheckLength = serializedObject.FindProperty("WallCheckLength");
        jumpscroll_toggle = serializedObject.FindProperty("jumpscroll_toggle");
        scroll_jumpCooldown = serializedObject.FindProperty("scroll_jumpCooldown");

        Climbable = serializedObject.FindProperty("Climbable");
        ClimbTime = serializedObject.FindProperty("ClimbTime");
        edgeBoostForce = serializedObject.FindProperty("edgeBoostForce");
        edgeBoostForceUp = serializedObject.FindProperty("edgeBoostForceUp");
        edgeBoostTime = serializedObject.FindProperty("edgeBoostTime");
        EdgeBoostDelay = serializedObject.FindProperty("EdgeBoostDelay");
        edgeBoostQueue = serializedObject.FindProperty("edgeBoostQueue2");
        
        canAirJump = serializedObject.FindProperty("canAirJump");
        AirJumpRemains = serializedObject.FindProperty("AirJumpRemains");
        
        canDash = serializedObject.FindProperty("canDash");
        DashForce = serializedObject.FindProperty("DashForce");
        DashMomentum = serializedObject.FindProperty("DashMomentum");
        DashDuration = serializedObject.FindProperty("DashDuration");
        DashCoolDown = serializedObject.FindProperty("DashCoolDown");
        EnableDash = serializedObject.FindProperty("EnableDash");
        EnableAirJump = serializedObject.FindProperty("EnableAirJump");
        
        Slaming = serializedObject.FindProperty("Slaming");
        SlamDistance = serializedObject.FindProperty("SlamDistance");
        SlamForce = serializedObject.FindProperty("SlamForce");
        SlamDelay = serializedObject.FindProperty("SlamDelay");
        canSlam = serializedObject.FindProperty("canSlam");
        SlamShakeEffect = serializedObject.FindProperty("SlamShakeEffect");
        
        LedgeClimbTarget = serializedObject.FindProperty("LedgeClimbTarget");
        ClimbEndPoint = serializedObject.FindProperty("ClimbEndPoint");
        detectClimbObject = serializedObject.FindProperty("detectClimbObject");
        detectClimbObstruction = serializedObject.FindProperty("detectClimbObstruction");
        PCam = serializedObject.FindProperty("PCam");
    }

    bool States = true;
    bool Setup, ParkourSetup, Physics;
    bool MovementStats, SlideStats, CrouchStats, StairAndSlope, ParkourStats;
    bool AirJump, Dash, Slam;
    bool Stats;
    #endregion

    //----------------------------------------------------------------------------------\\

    public override void OnInspectorGUI()
    {
        Player _player = (Player)target;

        serializedObject.Update();

        States = EditorGUILayout.Toggle("Player States", States);
        if (States)
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("States: ", _player.currentState_string);
            EditorGUILayout.PropertyField(PlayerCurrentSpeed);
            //EditorGUILayout.LabelField("States: ", _player.currentSlideState_string());
            EditorGUILayout.LabelField("Last States: ", _player.lastState_string);
            EditorGUILayout.PropertyField(CrouchingInput);
            EditorGUILayout.PropertyField(scroll);
            EditorGUILayout.PropertyField(move);
            EditorGUILayout.PropertyField(airstrafe);
            EditorGUILayout.PropertyField(movingUpSlope);
            EditorGUILayout.PropertyField(isGrounded);
            EditorGUILayout.PropertyField(isMovingForward);
            EditorGUILayout.PropertyField(MaxSpeed);
            EditorGUILayout.PropertyField(SlideDelayTimer);
            EditorGUILayout.PropertyField(ObjectOnTopDetected);
            EditorGUILayout.PropertyField(JumpInput);
            EditorGUILayout.PropertyField(edgeBoostQueue);
            
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(AirJumpRemains);
            EditorGUILayout.PropertyField(canAirJump);
            EditorGUILayout.PropertyField(canDash);
            EditorGUILayout.PropertyField(Slaming);
            EditorGUILayout.PropertyField(allowMove);
            GUI.enabled = true;
                EditorGUILayout.PropertyField(SlideDelayTimer_Default);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("ADVANCED", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(strafe_time_holding);
            EditorGUILayout.PropertyField(Debug_strafe_limit);

            EditorGUILayout.PropertyField(IsParkour);
            EditorGUILayout.PropertyField(isReadyToKick);
            EditorGUILayout.PropertyField(Kicking);
        }

        Stats = EditorGUILayout.Toggle("Player Stats", Stats);
        if (Stats)
        {
            MovementStats = EditorGUILayout.BeginFoldoutHeaderGroup(MovementStats, "Movements");
            if (MovementStats)
            {
                EditorGUILayout.PropertyField(currentSpeed);
                EditorGUILayout.PropertyField(jumpforce);
                EditorGUILayout.PropertyField(walkspeed);
                EditorGUILayout.PropertyField(KickMoveSpeed);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            AirJump = EditorGUILayout.BeginFoldoutHeaderGroup(AirJump, "Air Jump");
            if (AirJump)
            {
                EditorGUILayout.PropertyField(AirJumpAmount);
                EditorGUILayout.PropertyField(AirJumpForce);
                EditorGUILayout.PropertyField(AirJumpGravityReset);
            }
            EditorGUILayout.EndFoldoutHeaderGroup(); 
            
            Dash = EditorGUILayout.BeginFoldoutHeaderGroup(Dash, "Dash");
            if (Dash)
            {
                EditorGUILayout.PropertyField(DashForce);
                EditorGUILayout.PropertyField(DashMomentum);
                EditorGUILayout.PropertyField(DashDuration);
                EditorGUILayout.PropertyField(DashCoolDown);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            Slam = EditorGUILayout.BeginFoldoutHeaderGroup(Slam, "Slam");
            if (Slam)
            {
                EditorGUILayout.PropertyField(SlamShakeEffect);
                EditorGUILayout.PropertyField(SlamDistance);
                EditorGUILayout.PropertyField(SlamForce);
                EditorGUILayout.PropertyField(SlamDelay);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            CrouchStats = EditorGUILayout.BeginFoldoutHeaderGroup(CrouchStats, "Crouch");
            if (CrouchStats)
            {
                EditorGUILayout.PropertyField(crouchheight);
                EditorGUILayout.PropertyField(normalheight);
                EditorGUILayout.PropertyField(crouchspeed);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            SlideStats = EditorGUILayout.BeginFoldoutHeaderGroup(SlideStats, "Slide");
            if (SlideStats)
            {
                EditorGUILayout.PropertyField(canSlide);
                EditorGUILayout.PropertyField(sliding);
                GUI.enabled = false;
                EditorGUILayout.PropertyField(SlideTimer);
                EditorGUILayout.PropertyField(SlideDelayTimer);
                GUI.enabled = true;

                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(SpeedRequired);
                EditorGUILayout.PropertyField(slideforce);
                EditorGUILayout.PropertyField(slideModifier);
                EditorGUILayout.PropertyField(SlideTime_Default);
                EditorGUILayout.PropertyField(SlideDelayTimer_Default);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            StairAndSlope = EditorGUILayout.BeginFoldoutHeaderGroup(StairAndSlope, "Stair & Slope");
            if (StairAndSlope)
            {
                EditorGUILayout.PropertyField(movingUpSlope);
                EditorGUILayout.PropertyField(SlopeAngle);
                EditorGUILayout.Space(10);

                EditorGUILayout.PropertyField(maxStepHeight);
                EditorGUILayout.PropertyField(stepDetail);
                EditorGUILayout.PropertyField(stepMask);
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(maxSlopeAngle);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            ParkourStats = EditorGUILayout.BeginFoldoutHeaderGroup(ParkourStats, "Parkour Stats");
            if (ParkourStats)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(Climbable);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(ClimbTime);
                EditorGUILayout.PropertyField(edgeBoostForce);
                EditorGUILayout.PropertyField(edgeBoostForceUp);
                EditorGUILayout.PropertyField(edgeBoostTime);
                EditorGUILayout.PropertyField(EdgeBoostDelay);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        Physics = EditorGUILayout.BeginFoldoutHeaderGroup(Physics, "Physics");
        if (Physics)
        {
            EditorGUILayout.PropertyField(extragravity);
            EditorGUILayout.PropertyField(Multiplier);
            EditorGUILayout.PropertyField(countermovement);
            EditorGUILayout.PropertyField(MaxSpeed);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        Setup = EditorGUILayout.Toggle("Setup Menu", Setup);
        if (Setup)
        { 
            EditorGUILayout.PropertyField(canSlam);
            EditorGUILayout.PropertyField(EnableDash);
            EditorGUILayout.PropertyField(EnableAirJump);
            EditorGUILayout.PropertyField(jumpscroll_toggle);
            EditorGUILayout.PropertyField(scroll_jumpCooldown);
            EditorGUILayout.PropertyField(GroundChecker);
            EditorGUILayout.PropertyField(WallCheckPoint);
            EditorGUILayout.PropertyField(DetectWallHit);
            EditorGUILayout.PropertyField(wallCheckSensorCount);
            EditorGUILayout.PropertyField(WallCheckLength);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(LedgeClimbTarget);
            EditorGUILayout.PropertyField(ClimbEndPoint);
            EditorGUILayout.PropertyField(detectClimbObject);
            EditorGUILayout.PropertyField(detectClimbObstruction);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(PCam);
            EditorGUILayout.PropertyField(GroundMask);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
