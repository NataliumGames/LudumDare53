using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game {

    public static class Events {
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
        public static WallPassedEvent WallPassedEvent = new WallPassedEvent();
        public static TimerTimeOutEvent TimerTimeOutEvent = new TimerTimeOutEvent();
        public static EngagementChangeEvent EngagementChangeEvent = new EngagementChangeEvent();
        public static HittedByEnemyEvent HittedByEnemyEvent = new HittedByEnemyEvent();
        public static ObjectRepairedEvent ObjectRepairedEvent = new ObjectRepairedEvent();
        public static MinigameFinishedEvent MinigameFinishedEvent = new MinigameFinishedEvent();
        public static AttackEvent AttackEvent = new AttackEvent();
        public static EnemySpawnedEvent EnemySpawnedEvent = new EnemySpawnedEvent();
    }

    public class MinigameFinishedEvent : GameEvent {
        public string Minigame;
        public string Recap;
        public float Engagement;
        public int Score;
        public float Time;
    }

    public class EnemySpawnedEvent : GameEvent {
        public GameObject Enemy;
    }
    
    public class AttackEvent : GameEvent {
    
    }

    public class GameOverEvent : GameEvent {
        
    }

    public class TimerTimeOutEvent : GameEvent {
        
    }

    public class HittedByEnemyEvent : GameEvent {
        
    }

    public class ObjectRepairedEvent : GameEvent {
        public GameObject Object;
    }

    public class EngagementChangeEvent : GameEvent {
        public float Value;
    }

    public class WallPassedEvent : GameEvent {
        public int Score;
    }

    public class DisplayMessageEvent : GameEvent {
        public string Message;
    }
}
