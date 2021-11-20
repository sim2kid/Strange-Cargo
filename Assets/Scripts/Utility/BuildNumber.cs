using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class BuildNumber : MonoBehaviour
    {
        TMPro.TextMeshProUGUI tmp;
        private void Start()
        {
            tmp = GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = tmp.text.Replace("#.#.#", Application.version);
        }
    }
}