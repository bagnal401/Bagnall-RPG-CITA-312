using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // Configurations
        [SerializeField] float weaponRange = 2f;

        // Target should be able to change position at least.
        Transform target;

        private void Update()
        {
            // If the target is null, like outside the boundaries of the terrain,
            // then movement will cancel.
            if (target == null) return;

            // If the player is not in range of the target after clicking attack,
            // then the player will move to be in range.
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            // Otherwise the player's movement towards the target will cancel
            // and regular movement will begin.
            else
            {
                GetComponent<Mover>().Cancel();
            }
        }

        // Calls GetIsInRange, which wil determine if the player's position is
        // less than the weapon range.
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        // Calls the Attack method, which will call the ActionScheduler component
        // to begin combat at this target.
        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        // Calls the Cancel method, which will cancel movement when the target is null.
        public void Cancel()
        {
            target = null;
        }
    }
}
