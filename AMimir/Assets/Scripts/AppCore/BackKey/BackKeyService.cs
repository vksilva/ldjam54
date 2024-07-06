using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppCore.BackKey
{
    public class BackKeyService : MonoBehaviour
    {
        private Stack<Action> backKeyActions = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!backKeyActions.TryPeek(out var action))
                {
                    Debug.LogError("backKeyActions does not have an action to execute");
                    return;
                }

                action.Invoke();
            }
        }

        public void PushAction(Action action)
        {
            backKeyActions.Push(action);
        }

        public void PopAction()
        {
            backKeyActions.Pop();
        }

        public void CleanActions()
        {
            backKeyActions.Clear();
        }
    }
}