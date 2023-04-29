using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game {

    public static class Events {
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
        public static WallPassedEvent WallPassedEvent = new WallPassedEvent();
    }

    public class GameOverEvent : GameEvent {
        
    }

    public class WallPassedEvent : GameEvent {
        public int Score;
    }

    public class DisplayMessageEvent : GameEvent {
        public string Message;
    }
}
