using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee , Gun , Magic };
    public WeaponType weaponType;

    public enum FireType { Single , Burst , Auto };
    [Header("Fire Mode")]
    [Space(2)]
    public FireType fireType;

    [Header("Weapon Stats")]
    [SerializeField] float FireRate;
    [SerializeField] float Damage;

    [Space(10)]
    [Header("Aiming")]
    public bool Aiming;
    private float defaultFOV;
    public float AimSpeed = 10;
    public float ZoomSpeed = 10;
    public float ZoomRatio = 1.5f;

    [Header("Gun Settings")]
    public int clipSize = 30;
    public int DefaultCapacity = 270; //ammo display

    public enum BulletType { Hitscan , Projectile };
    [Header("Bullet Type")]
    [SerializeField] ObjectPool bullet;
    public float Speed;
    [Range(0, 120)] public int _currentAmmoInClip;
    public int AmmoRemain; //default ammo


    [Header("Dev")]
    private bool _canShoot;
    [SerializeField]bool shooting;
    bool reloading;
    Vector3 BulletDestination;

    [Header("Setup")]
    [SerializeField] GameObject ADS_Pos;
    [SerializeField] GameObject Default_Pos;
    [SerializeField] GameObject Slide_Rot;
    [SerializeField] GameObject Holder;

    private InputAction PControls;
    [SerializeField] RecoilCam recoil;
    [SerializeField] AnimationUlt EffectManager;
    public Transform GunTip;
    public Animator gun_animator;
    public Camera PCam;
    public Camera WeapCam;
    [SerializeField] GameObject HeadTilt;
    [SerializeField] PlayerCam PCamScript;

    public void Fire_Input(InputAction.CallbackContext ctx)
    {
        if (!this.gameObject.activeSelf)
        { return; }

        if (PCamScript.Paused)
        { return; }

        shooting = ctx.performed;
    }

    public void Aim_Input(InputAction.CallbackContext ctx)
    {
        if (!this.gameObject.activeSelf)
        { return; }

        if (PCamScript.Paused)
        { return; }

        if (!ctx.performed)
        { return; }

        ToggleAim();
    }

    public void Reload_Input(InputAction.CallbackContext ctx)
    {
        if (!this.gameObject.activeSelf)
        { return; }

        if (PCamScript.Paused)
        { return; }

        if (!ctx.performed)
        { return; }

        StartReload();
    }

    void ToggleAim()
    {
        if (PCamScript.Paused)
        { return; }

        if (!Aiming)
        {
            Aiming = true;
            gun_animator.SetBool("Aiming", true);
        }
        else
        {
            Aiming = false;
            gun_animator.SetBool("Aiming", false);
        }
    }

    void StartReload()
    {
        if (_currentAmmoInClip == clipSize || AmmoRemain <= 0)
        {
            Debug.Log("no, haha, u dumb ?");
        }
        else
        {
            Aiming = false;
            gun_animator.SetBool("Shooting", false);
            gun_animator.SetBool("Reload", true);
            Invoke("LockReload", 0.25f);
        }
    }
}
