using System;
using UnityEngine;

namespace UI
{
    public class PreStartPanel : MonoBehaviour
    {
        private void Start()
        {
            Time.timeScale = 0;
        }

        private void Update()
        {
            StartGame();
        }

        private void StartGame()
        {
            if(!Input.anyKey) return;
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
