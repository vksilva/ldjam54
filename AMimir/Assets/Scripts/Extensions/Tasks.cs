using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Busta.Extensions
{
    public static class Tasks
    {
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
            await Task.Delay(Mathf.RoundToInt(seconds*1000));
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