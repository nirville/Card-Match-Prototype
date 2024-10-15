using UnityEngine;

namespace Nirville.Core
{
    public class GameManager : MonoBehaviour 
    {
        internal static GameManager Instance {get; private set;}

        public string LastContentSelected { get; set; }

        public bool isLogMessages;

        private void Awake() 
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void Logger(string msg)
        {
            if(isLogMessages)
                Debug.Log(msg);
        }
    }
}