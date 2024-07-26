using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Helper
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDic = new();

        public static WaitForSeconds GetWaitForSeconds(float value)
        {
            if (WaitForSecondsDic.TryGetValue(value, out WaitForSeconds waitForSeconds))
            {
                return waitForSeconds;
            }

            var wait = new WaitForSeconds(value);
            WaitForSecondsDic.TryAdd(value, wait);
            return wait;
        }
    }
}