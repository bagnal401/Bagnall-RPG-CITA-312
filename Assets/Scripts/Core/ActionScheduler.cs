using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        // Calls the IAction interface.
        IAction currentAction;

        // StartAction occurs when determining whether to stop combat or stop movement.
        public void StartAction(IAction action)
        {
            // If the fighting target still exists or the player is still moving
            // along the nav mesh, then the action will continue.
            if (currentAction == action) return;
            // If the player is no longer fighting or moving, it will cancel the action.
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
