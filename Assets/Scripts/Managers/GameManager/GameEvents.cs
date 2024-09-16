using BloodWork.Entity.EventParams;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodWork.Manager.GameManager
{
    public struct GameEvents
    {
        public Action<GamePauseParams> OnGamePause;
    }
}
