using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{ 
    public class PlayerSound : MonoBehaviour
    {
        public AK.Wwise.Event _FootstepSFX = new AK.Wwise.Event();
        public void PlayerFootstepSFX(bool isPlay) { if (isPlay) _FootstepSFX.Post(gameObject); }
        public void PlayerFootstepSFXAnim() { _FootstepSFX.Post(gameObject); Debug.Log("Step"); }

        public AK.Wwise.Event _FirstJumpSFX = new AK.Wwise.Event();
        public void PlayerFirstJumpSFX(bool isPlay) { if (isPlay) _FirstJumpSFX.Post(gameObject); }
        public void PlayerFirstJumpSFXAnim() { _FirstJumpSFX.Post(gameObject); }

        public AK.Wwise.Event _SecondJumpSFX = new AK.Wwise.Event();
        public void PlayerSecondJumpSFX(bool isPlay) { if (isPlay) _SecondJumpSFX.Post(gameObject); }
        public void PlayerSecondJumpSFXAnim() { _SecondJumpSFX.Post(gameObject); }

        public AK.Wwise.Event _LandingSFX = new AK.Wwise.Event();
        public void PlayerLandingSFX(bool isPlay) { if (isPlay) _LandingSFX.Post(gameObject); }

        public AK.Wwise.Event _HackStartSFK = new AK.Wwise.Event();
        public void PlayerHackStartSFX(bool isPlay) { if (isPlay) _HackStartSFK.Post(gameObject); }

        public AK.Wwise.Event _HackingSFX = new AK.Wwise.Event();
        public void PlayerHackingSFX(bool isPlay) { if (isPlay) _HackingSFX.Post(gameObject); }
        public void PlayerHackingSFXCancle(bool isPlay) { if (isPlay) _HackingSFX.Stop(gameObject); }

        public AK.Wwise.Event _HackingSuccessSFX = new AK.Wwise.Event();
        public void PlayerHackingSuccessSFX(bool isPlay) { if (isPlay) _HackingSuccessSFX.Post(gameObject); }

        public AK.Wwise.Event _HackingFailSFX = new AK.Wwise.Event();
        public void PlayerHackingFailSFX(bool isPlay) { if (isPlay) _HackingFailSFX.Post(gameObject); }

        public AK.Wwise.Event _DamagedSFX = new AK.Wwise.Event();
        public void PlayerDamagedSFX(bool isPlay) { if (isPlay) _DamagedSFX.Post(gameObject); }

        public AK.Wwise.Event _SkillHologramSFX = new AK.Wwise.Event();
        public void PlayerSkillHologramSFX(bool isPlay) { if (isPlay) _SkillHologramSFX.Post(gameObject); }


        public GameObject _RunEffect;
        public void RunEffect()
        {
            Instantiate(_RunEffect, transform.position, transform.rotation);
        }
    }
}