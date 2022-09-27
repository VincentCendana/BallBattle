using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimation : MonoBehaviour
{
    [Header("Animation Components")]
    public Animation anim;                  // soldier's animation component
    public AnimationClip currentClip;       // soldier's currently played animation

    [Header("Animation Clips")]
    public AnimationClip idleClip;          // soldier's idle clip
    public AnimationClip runningClip;       // soldier's running clip
    public AnimationClip tackleClip;        // soldier's catching defender animation
    public AnimationClip passClip;          // soldier's ball passing animation
    public AnimationClip tripClip;          // soldier's caught animation
    public AnimationClip headerClip;        // soldier's enter fence animation

    private void Start()
    {
        // play idle animation at the start
        PlayAnimation(idleClip);
    }

    // play selected animation clip
    public void PlayAnimation(AnimationClip clip)
    {
        // set current clip status
        currentClip = clip;

        // set and play animation
        anim.clip = clip;
        anim.Play();
    }

    // stop animation clips
    public void StopAnimation()
    {
        anim.Stop();
    }
}
