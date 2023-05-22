using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AnimationUlt : MonoBehaviour
{
    public SFX[] SFX;


    public VFX[] VFX;
    Dictionary<string, VFX> VFXDict;

    void Awake()
	{
		foreach (SFX s in SFX)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;
		}

        VFXDict = new Dictionary<string, VFX>();
        foreach (VFX vfx in VFX)
        {
            VFXDict[vfx.VFX_name] = vfx;
        }
    }

    public void PlaySFX(string name)
    {
        // Find all SFX objects matching the specified sound name
        SFX[] sounds = Array.FindAll(SFX, item => item.SFX_name == name);

        if (sounds.Length == 0)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        float totalPercentage = 0f;
        foreach (SFX s in sounds)
        {
            totalPercentage += s.percentage;
        }

        float randomPercentage = UnityEngine.Random.Range(0f, 100f);
        float percentageSum = 0f;
        SFX selectedSFX = null;

        foreach (SFX s in sounds)
        {
            percentageSum += s.percentage;
            if (percentageSum >= randomPercentage)
            {
                selectedSFX = s;
                break;
            }
        }

        if (selectedSFX == null)
        {
            Debug.LogWarning("Could not select SFX to play");
            return;
        }

        selectedSFX.source.volume = selectedSFX.volume;
        selectedSFX.source.PlayOneShot(selectedSFX.clip);
    }

    public void PlayVFX(string name)
    {
        if (VFXDict.TryGetValue(name, out VFX vfx))
        {
            vfx.vfx.Play();
        }
        else
        {
            Debug.LogWarning("VFX " + name + " not found!");
        }
    }

    public void PlayVFX_Pos(string name, Transform pos)
    {
        if (VFXDict.TryGetValue(name, out VFX vfx))
        {
            vfx.vfx.transform.position = pos.position;
            vfx.vfx.Play();
        }
        else
        {
            Debug.LogWarning("VFX " + name + " not found!");
        }
    }

    public void StopVFX(string name)
    {
        if (VFXDict.TryGetValue(name, out VFX vfx))
        {
            vfx.vfx.Stop();
        }
        else
        {
            Debug.LogWarning("VFX " + name + " not found!");
        }
    }
}
