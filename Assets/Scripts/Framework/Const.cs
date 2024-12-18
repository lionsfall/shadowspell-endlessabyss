using UnityEngine;

namespace Dogabeey
{
    public enum Screens
    {
        MainMenu,
        LevelList,
        WorldList,
        WinScreen,
        LoseScreen,
        SettingScreen,
    }
    public struct Const
    {

        public struct Values
        {
            public const float TICK_TIME = 1f;
            public const float INVINCIBILITY_FRAME = 1f;
            public const float PROJECTILE_SPEED = 4f;
            public const float ENEMY_SPEED_MULTIPLIER = 1f;
        }

        public struct TAGS
        {
            public const string PLAYER = "Player";
            public const string ENEMY = "Enemy";
            public const string COLLECTIBLE = "Collectible";
            public const string GROUND = "Ground";
        }

        public struct BindingNames
        {
            public const string KEYBOARD = "Keyboard";
            public const string GAMEPAD = "Gamepad";
        }
        public struct SOUNDS
        {
            public struct MUSICS
            {
                public const string MAIN_MENU = "MainMenu";
                public const string GAMEPLAY = "Gameplay";
            }
            public struct EFFECTS
            {
                public const string TYPEWRITER = "Typewriter";
                public const string JUMP = "Jump";
                public const string DEATH = "Death";
                public const string PICKUP = "Pickup";
                public const string LEVEL_COMPLETE = "LevelComplete";
                public const string LEVEL_FAILED = "LevelFailed";
            }
        }

        public struct GameEvents
        {
            public const string PLAYER_HEALTH_CHANGED = "PLAYER_HEALTH_CHANGED";
            public const string PLAYER_MANA_CHANGED = "PLAYER_MANA_CHANGED";
            public const string PLAYER_ENTERED_ROOM = "PLAYER_ENTERED_ROOM";
            public const string PLAYER_ESSENCES_CHANGED = "PLAYER_ESSENCES_CHANGED";

            public const string COLLECTIBLE_EARNED = "COLLECTIBLE_EARNED";
            public const string OBJECTIVE_COMPLETED = "OBJECTIVE_COMPLETED";
            public const string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";

            public const string LEVEL_COMPLETED = "LEVEL_COMPLETED";
            public const string LEVEL_FAILED = "LEVEL_FAILED";
            public const string LEVEL_STARTED = "LEVEL_STARTED";

            public const string CURRENT_WORLD_CHANGED = "CURRENT_WORLD_CHANGED";

            public const string CREATURE_DEATH = "CREATURE_DEATH";
            public const string CREATURE_JUMP = "CREATURE_JUMP";
            public const string CREATURE_DAMAGE = "CREATURE_DAMAGE";
            public const string CREATURE_ATTACK = "CREATURE_ATTACK";
        }
    }
}