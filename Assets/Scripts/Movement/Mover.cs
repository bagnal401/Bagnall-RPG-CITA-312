using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        // Configurations.
        [SerializeField] Transform target;

        // Assigning NavMeshAgent a variable.
        NavMeshAgent navMeshAgent;
        Health health;

        // Start calls the NavMeshAgent component.
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame and calls the UpdateAnimator method.
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        // StartMoveAction calls the ActionScheduler to start its moving schedule.
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        // If the navMeshAgent is not stopped, then the player moves to the destination.
        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
            navMeshAgent.isStopped = false;
        }

        // If the navMeshAgent is stopped, then the player's movement will be cancelled.
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        // UpdateAnimator checks the player's velocity with the navMeshAgent.
        // Player's local velocity is used to determine the animator's forward speed. 
        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
