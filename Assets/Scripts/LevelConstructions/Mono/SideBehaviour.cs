using UnityEngine;

namespace Treasury.LevelConstructions.Mono
{
    public class SideBehaviour : MonoBehaviour
    {
        public void RePaint(Material newMaterial)
        {
            GetComponent<Renderer>().material = newMaterial;
        }
    }
}
