using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInfo : CharacterStat
    {
        public static PlayerInfo GetPlayerInfo()
        {
            return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        }

        protected override void Awake()
        {
            base.Awake();
            _playerHP = _hp;
            playershowhp = _hp;
            
        }
        public int playershowhp;
        public static float CapsuleHeight = 1.8f;
        public static float CapsuleRadius = 0.44f;
        public static float CapsuleCenterY = 0.9f;

        public static int _playerHP;
        public Vector3 CapsuleCenterPos {
            get { return new Vector3(0, PlayerInfo.CapsuleCenterY, 0); }
        }

        [Range(0,1)] public float JumpMoveSpeedDecentRatio = 0.6f;
        [Range(0,1)] public float AimMoveSpeedDecentRatio = 0.3f;

        private void Update()
        {
        }
    }

}