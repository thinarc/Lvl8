using UnityEngine;

namespace _Project.Scripts.Player
{
    [CreateAssetMenu]
    public class ScriptablePlayer : ScriptableObject
    {
        [Header("Layers")]
        public LayerMask groundLayer;
        public LayerMask playerLayer;
        
        [Header("Input")]
        public bool snapInput = true;
        public float horizontalDeadZoneThreshold = 0.1f;
        public float verticalDeadZoneThreshold = 0.2f;
        
        [Header("Movement")]
        public float maxSpeed = 14;
        public float acceleration = 120;
        public float groundDeceleration = 60;
        public float airDeceleration = 30;
        
        [Header("Gravity")]
        public float groundingForce = -1.5f;
        public float grounderDistance = 0.05f;
        
        [Header("Flying")]
        public float flyPower = 36;
        public float maxFallSpeed = 40;
        public float fallAcceleration = 110;
        public float flyEndEarlyGravityModifier = 3;
    }
}