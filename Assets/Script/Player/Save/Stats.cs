using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [Header("Input Setup")]
    [SerializeField] bool jumpscroll_toggle;
    [SerializeField] float scroll_jumpCooldown = 0.5f;

    [Header("Physics")]
    [SerializeField] float extragravity = 14;
    [SerializeField] float Multiplier = 80;
    [SerializeField] float countermovement = 400;
    [SerializeField] float MaxSpeed = 28;

    [Space(5)]
    [Header("Movement Stats")]
    [Space(10)]

    [Header("Base")]
    //float currentSpeed;
    [SerializeField] float jumpforce = 820;
    [SerializeField] float walkspeed = 25;
    [SerializeField] float KickMoveSpeed = 10;

    [Header("Crouch")]
    [SerializeField] float crouchheight = 1.25f;
    [SerializeField] float normalheight = 2;
    [SerializeField] float crouchspeed = 15;

    [Header("Slide")]
    [SerializeField] float SpeedRequired_Slide = 17.5f;
    [SerializeField] float slideforce = 2;
    [SerializeField] float slideModifier = 1;
    [SerializeField] float SlideTime_Default = 0.8f;
    [SerializeField] float SlideDelayTimer_Default = 0.3f;

    [Header("Stair & Slope")]
    [SerializeField] float maxStepHeight = 0.25f;
    [SerializeField] int stepDetail = 1;
    [SerializeField] float maxSlopeAngle = 70;

    [Header("Parkour")]
    [SerializeField] float ClimbTime = 0.38f;

    // Store the default values in a separate instance
    Stats defaultStats;

    private void Awake()
    {
        // Save the default values
        defaultStats = new Stats();
        defaultStats.scroll_jumpCooldown = 0.5f;

        defaultStats.extragravity = 14;
        defaultStats.Multiplier = 80;
        defaultStats.countermovement = 400;
        defaultStats.MaxSpeed = 28;

        defaultStats.jumpforce = 820;
        defaultStats.walkspeed = 25;
        defaultStats.KickMoveSpeed = 10;

        defaultStats.crouchheight = 1.25f;
        defaultStats.normalheight = 2;
        defaultStats.crouchspeed = 15;

        defaultStats.SpeedRequired_Slide = 17.5f;
        defaultStats.slideforce = 2;
        defaultStats.slideModifier = 1;
        defaultStats.SlideTime_Default = 0.8f;
        defaultStats.SlideDelayTimer_Default = 0.3f;

        defaultStats.maxStepHeight = 0.25f;
        defaultStats.stepDetail = 1;
        defaultStats.maxSlopeAngle = 70;

        defaultStats.ClimbTime = 0.38f;
    }


    Player p;
    public void ApplyOnPlayer()
    {
        p.jumpscroll_toggle = jumpscroll_toggle;

        // Physics
        p.extragravity = extragravity;
        p.Multiplier = Multiplier;
        p.countermovement = countermovement;
        p.MaxSpeed = MaxSpeed;

        // Movement stats
        p.jumpforce = jumpforce;
        p.walkspeed = walkspeed;
        p.KickMoveSpeed = KickMoveSpeed;

        // Crouch stats
        p.crouchheight = crouchheight;
        p.normalheight = normalheight;
        p.crouchspeed = crouchspeed;

        // Slide stats
        p.SpeedRequired_Slide = SpeedRequired_Slide;
        p.slideforce = slideforce;
        p.slideModifier = slideModifier;
        p.SlideTime_Default = SlideTime_Default;
        p.SlideDelayTimer_Default = SlideDelayTimer_Default;

        // Stair & Slope stats
        p.maxStepHeight = maxStepHeight;
        p.stepDetail = stepDetail;
        p.maxSlopeAngle = maxSlopeAngle;

        // Parkour stats
        p.ClimbTime = ClimbTime;
    }
    public void RestoreAllDefault()
    {
        // Restore all values to default
        scroll_jumpCooldown = defaultStats.scroll_jumpCooldown;

        extragravity = defaultStats.extragravity;
        Multiplier = defaultStats.Multiplier;
        countermovement = defaultStats.countermovement;
        MaxSpeed = defaultStats.MaxSpeed;

        jumpforce = defaultStats.jumpforce;
        walkspeed = defaultStats.walkspeed;
        KickMoveSpeed = defaultStats.KickMoveSpeed;

        crouchheight = defaultStats.crouchheight;
        normalheight = defaultStats.normalheight;
        crouchspeed = defaultStats.crouchspeed;

        SpeedRequired_Slide = defaultStats.SpeedRequired_Slide;
        slideforce = defaultStats.slideforce;
        slideModifier = defaultStats.slideModifier;
        SlideTime_Default = defaultStats.SlideTime_Default;
        SlideDelayTimer_Default = defaultStats.SlideDelayTimer_Default;

        maxStepHeight = defaultStats.maxStepHeight;
        stepDetail = defaultStats.stepDetail;
        maxSlopeAngle = defaultStats.maxSlopeAngle;

        ClimbTime = defaultStats.ClimbTime;
    }
    //not mine, by chatgpt XD
    public void RestoreValueDefault(string name)
    {
        
    }

}
