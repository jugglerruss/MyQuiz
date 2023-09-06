using UnityEngine;

namespace Plugins.Audio.Utils
{
    public class AudioAutoPause : MonoBehaviour
    {
        [SerializeField] private bool _pauseInEdiitor = false;

        private static AudioAutoPause _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (Application.isEditor && _pauseInEdiitor == false)
            {
                return;
            }

            if (hasFocus)
            {
                Debug.Log("Focus");
            }
            else
            {
                Debug.Log("Unfocus");
            }

            AudioListener.pause = hasFocus == false;
        }
    }
}