﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // Configurations
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Weapon defaultWeapon = null;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;


        // Target should be able to change position at least.
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon;

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // If the target is null, like outside the boundaries of the terrain,
            // then movement will cancel.
            if (target == null) return;

            if (target.IsDead()) return;

            // If the player is not in range of the target after clicking attack,
            // then the player will move to be in range.
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            // Otherwise the player's movement towards the target will cancel
            // and regular movement will begin.
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit event.
                TriggerAttack();
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
        }

        // Animation event
        void Hit()
        {
            if (target == null)
            {
                return;
            }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetDamage());
            }

            target.TakeDamage(currentWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
        }

        // Calls GetIsInRange, which wil determine if the player's position is
        // less than the weapon range.
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        // Calls the Attack method, which will call the ActionScheduler component
        // to begin combat at this target.
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // Calls the Cancel method, which will cancel movement when the target is null.
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
