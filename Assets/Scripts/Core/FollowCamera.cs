using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        // Configurations
        [SerializeField] Transform target;

        // LateUpdate is called once per frame. LateUpdate will check the position of the player
        // and will change its position to follow the player.
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
