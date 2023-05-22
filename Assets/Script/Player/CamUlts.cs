using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamUlts : MonoBehaviour
{
	private void Awake()
	{
		CamShakeObj = GetComponentInChildren<CamShake>();
	}

	#region HeadTilt
	public float HeadTiltSmoothSpeed;
	public float SlideHeadTiltSmoothSpeed;
	public float HeadDegree = 1f;
	public float SlideHeadDegree = 1f;
	public float HeadTiltValue;

	[SerializeField] Player player;
	[SerializeField] Rigidbody PlayerBody;

	float direct;

    private void LateUpdate()
    {
		HeadTilt();
    }
    void HeadTilt()
	{
		if (PlayerBody.velocity.magnitude < 0.25f)
		{
			HeadTiltValue = Mathf.Lerp(HeadTiltValue, 0, HeadTiltSmoothSpeed * Time.deltaTime);
			return;
		}

		if (!player.sliding)
		{
			HeadTiltValue = Mathf.Lerp(HeadTiltValue, -player.move.x * HeadDegree, HeadTiltSmoothSpeed * Time.deltaTime);
		}
		else
        {
			direct = player.move.x;

			if (direct == 0) direct = -1; 
			HeadTiltValue = Mathf.Lerp(HeadTiltValue, direct * SlideHeadDegree, SlideHeadTiltSmoothSpeed * Time.deltaTime);
        }
	}
    #endregion

    #region CamShaker

    private CamShake CamShakeObj;
    public void Shake(float Trauma)
    {
	    CamShakeObj.Trauma = Trauma;
    }
    

    #endregion
}
