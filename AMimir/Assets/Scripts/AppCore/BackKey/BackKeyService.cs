using System;
using System.Collections.Generic;
using UnityEngine;

namespace Busta.AppCore.BackKey
{
    public class BackKeyService
    {
        private Stack<Action> backKeyActions = new();

        public void Init(LifecycleService lifecycleService) {
            lifecycleService.OnUpdate.AddListener(OnUpdate);
        }
        
        private void OnUpdate()
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