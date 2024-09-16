using BloodWork.Entity.EventParams;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodWork.Manager.GameManager
{
    public class GameManager : MonoBehaviour
    {

        public GameEvents Events;

        private static GameManager Instance;

        public static GameManager Instantiate() 
        { 
            return Instance;
        }

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
        }


        private void OnEnable()
        {
            Events.OnGamePause += OnPauseGame;
        }

        private void OnDisable()
        {
            Events.OnGamePause -= OnPauseGame;
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1f;
        }

        private void OnPauseGame(GamePauseParams gamePauseParams)
        {
            if (gamePauseParams.Pause)
                PauseGame();
            else
                UnpauseGame();
        }

        public bool IsGamePaused()
        {
            float paused = 0f;
            return Time.timeScale == paused;
        }
    }
}
