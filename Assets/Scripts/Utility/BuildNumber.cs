using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class BuildNumber : MonoBehaviour
    {
        TMPro.TMP_Text tmp;
        private void Start()
        {
            tmp = GetComponent<TMPro.TMP_Text>();
            tmp.text = tmp.text.Replace("#.#.#", Application.version);
        }
    }
}