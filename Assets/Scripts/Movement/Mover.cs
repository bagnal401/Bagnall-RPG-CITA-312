using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        // Configurations.
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

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
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        // If the navMeshAgent is not stopped, then the player moves to the destination.
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            GetComponent<NavMeshAgent>().destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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

        public object CaptureState()
        {
            /*Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);*/
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // Dictionary<string, object> data = (Dictionary<string, object>)state;
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = ((SerializableVector3)data["position"]).ToVector();
            // transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
