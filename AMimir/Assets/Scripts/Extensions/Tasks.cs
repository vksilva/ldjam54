using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Busta.Extensions
{
    public static class Tasks
    {
        private static TaskAwaiter TaskRunner
        {
            get
            {
                if (!taskRunner)
                {
                    var go = new GameObject("Task Runner");
                    taskRunner = go.AddComponent<TaskAwaiter>();
                    Object.DontDestroyOnLoad(go);
                }

                return taskRunner;
            }
        }

        private static TaskAwaiter taskRunner;
        
        public static async Task WaitUntil(Func<bool> condition)
        {
            if (condition == null)
            {
                return;
            }

            while (!condition.Invoke())
            {
                await WaitUntilNextFrame();
            }
        }

        public static async Task WaitForSeconds(float seconds)
        {
#if UNITY_WEBGL
            var finished = false;

            IEnumerator WaitRoutine()
            {
                yield return new WaitForSeconds(seconds);
                finished = true;
            }

            TaskRunner.StartCoroutine(WaitRoutine());

            await WaitUntil(() => finished);
#else
            await Task.Delay(Mathf.RoundToInt(seconds * 1000));
#endif
        }

        public static async Task WaitUntilNextFrame()
        {
            await Task.Yield();
        }

        public static async Task Await(this CustomYieldInstruction yieldInstruction)
        {
            await WaitUntil(() => !yieldInstruction.keepWaiting);
        }
    }
}