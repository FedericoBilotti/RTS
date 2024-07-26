using Manager;
using UnityEngine;

namespace Structures
{
    public class Center : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.Instance.AddCenter(this);
        }

        private void OnDisable()
        {
            GameManager.Instance.RemoveCenter(this);
        }
    }
}
