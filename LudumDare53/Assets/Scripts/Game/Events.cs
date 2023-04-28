using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game {

    public static class Events {
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    }

    public class GameOverEvent : GameEvent {
        
    }

    public class DisplayMessageEvent : GameEvent {
        public string Message;
    }
}
