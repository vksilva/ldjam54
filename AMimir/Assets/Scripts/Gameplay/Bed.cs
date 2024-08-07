using UnityEngine;

namespace Busta.Gameplay
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] private bool isDoubleBed = false;

        public bool IsDoubleBed()
        {
            return isDoubleBed;
        }
    }
}
