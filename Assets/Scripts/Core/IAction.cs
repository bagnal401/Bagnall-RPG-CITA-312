using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    // IAction is the interface for Mover and Fighter scripts, 
    // to work alongside ActionScheduler.
    public interface IAction
    {
        // Contains the Cancel method.
        void Cancel();
    }
}
