using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using System;
using static Spine.AnimationState;

public class SpineAnimManager : Singleton<SpineAnimManager>
{
    private TrackEntryDelegate ac = null;

    public void PlayAnim(SkeletonGraphic skeleton, Action func, int trackIndex, string animName, bool loop)
    {
        if (skeleton != null)
        {            
            PlayAnim(skeleton, trackIndex, animName, loop);
            ac = delegate
            {
                skeleton.AnimationState.Complete -= ac;
                ac = null;
                if (func != null)
                {
                    func?.Invoke();
                }                
            };
            skeleton.AnimationState.Complete += ac;
        }
    }

    public void PlayAnim(SkeletonGraphic skeleton, int trackIndex, string animName, bool loop)
    {
        if (skeleton != null)
        {
            if (!skeleton.IsValid)
            {
                skeleton.Initialize(true);
            }
            skeleton.AnimationState.SetAnimation(trackIndex, animName, loop);
        }
    }

    public void StopAnim(SkeletonGraphic sg, int trackIndex, float mixDuration)
    {
        sg.AnimationState.SetEmptyAnimation(trackIndex, mixDuration);
    }

}
