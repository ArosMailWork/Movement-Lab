using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilCam : MonoBehaviour
{
    //Rotations
    Vector3 currentRot;
    Vector3 targetRot;

    //Recoil (Hipfire)
    [SerializeField] float recoilX = -4f;
    [SerializeField] float recoilY = 4f;
    [SerializeField] float recoilZ = 0.7f;
    
    //Recoil (ADS)
    [SerializeField] float ADS_recoilX = -3f;
    [SerializeField] float ADS_recoilY = 2;
    [SerializeField] float ADS_recoilZ = 0.6f;

    //Settings
    float snappiness = 6;
    [SerializeField] float snappiness_Hipfire = 6;
    [SerializeField] float snappiness_ADS = 3;
    [SerializeField] float moving_assist = 0.8f;
    [SerializeField] float returnSpeed = 2.5f;

    //[SerializeField] Player player;

    // Update is called once per frame
    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, snappiness * Time.fixedDeltaTime);

        this.transform.localRotation = Quaternion.Euler(currentRot);
    }


    //Horizontal_input is movement input
    public void RecoilFire(bool Aiming, int Horizontal_input)
    {
        if(!Aiming && Horizontal_input == 0)
        {
            snappiness = snappiness_Hipfire;
            targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
            return;
        }

        if(Aiming && Horizontal_input == 0)
        {
            snappiness = snappiness_ADS;
            targetRot += new Vector3(ADS_recoilX, Random.Range(-ADS_recoilY, ADS_recoilY), Random.Range(-ADS_recoilZ, ADS_recoilZ));
            return;
        }

        if(!Aiming && Horizontal_input != 0)
        {
            snappiness = snappiness_Hipfire * moving_assist;
            targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
            return;
        }

        if (Aiming && Horizontal_input != 0)
        {
            snappiness = snappiness_ADS * moving_assist;
            targetRot += new Vector3(ADS_recoilX, Random.Range(-ADS_recoilY, ADS_recoilY), Random.Range(-ADS_recoilZ, ADS_recoilZ));
            return;
        }
    }
}
