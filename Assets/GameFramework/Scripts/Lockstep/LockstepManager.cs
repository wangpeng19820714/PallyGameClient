using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

namespace GameFramework.Lockstep
{
    public class LockstepManager
    {
        public const int FrameRate = 60;
        public const int InfluenceResolution = 2;
        public const long DeltaTime = FixedMath.One / FrameRate;
        public const float DeltaTimeF = DeltaTime / FixedMath.OneF;

        private static int InfluenceCount;

        public static int InfluenceFrameCount { get; private set; }

        /// <summary>
        /// Number of frames that have passed. FrameCount/FrameRate = duration of game session in seconds.
        /// </summary>
        /// <value>The frame count.</value>
        public static int FrameCount { get; private set; }

        public static bool GameStarted { get; private set; }

        public static bool Loaded { get; private set; }

        /// <summary>
		/// Number of frames that have passed. FrameCount/FrameRate = duration of game session in seconds.
		/// </summary>
		/// <value>The frame count.</value>

        //for testing purposes
        public static bool PoolingEnabled = true;

        public static event Action onSetup;
        public static event Action onInitialize;

        public static int PauseCount { get; private set; }

        public static bool IsPaused { get { return PauseCount > 0; } }

        public static void Pause()
        {
            PauseCount++;
        }

        public static void Unpause()
        {
            PauseCount--;
        }

        public static void Setup()
        {
            Time.fixedDeltaTime = DeltaTimeF;
            Time.maximumDeltaTime = Time.fixedDeltaTime * 2;

            if (onSetup != null)
                onSetup();
        }

        private static long _playRate = FixedMath.One;
        public static long PlayRate
        {
            get
            {
                return _playRate;
            }
            set
            {
                if (value != _playRate)
                {
                    _playRate = value;
                    Time.timeScale = PlayRate.ToFloat();
                }
            }
        }

        public static float FloatPlayRate
        {
            get { return _playRate.ToFloat(); }
            set
            {
                PlayRate = FixedMath.Create(value);
            }
        }

        public static void Initialize()
        {
            {
                PlayRate = FixedMath.One;

                if (!Loaded)
                {
                    Setup();
                    Loaded = true;
                }
            }
        }

        private static void GameStart()
        {
            GameStarted = true;
        }

        public static void Simulate()
        {

        }

        public static void LateSimulate()
        {

        }

        public static void InfluenceSimulate()
        {

        }

        public static void Execute()
        {

        }

        public static void Visualize()
        {

        }

        public static void LateVisualize()
        {

        }

        public static void Deactivate()
        {

        }

        public static void Quit()
        {

        }

        public static int GetStateHash()
        {
            int hash = 0;
            return hash;
        }
    }
}

