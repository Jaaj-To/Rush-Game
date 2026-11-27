using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Manager
{
    public enum GameStatus { Playing, Won, Lost }

    public class GameManager : MonoBehaviour
    {
        public const float MIN_TICK_DURATION = 0.1f;
        public const float MAX_TICK_DURATION = 3f;

        [Range(MIN_TICK_DURATION, MAX_TICK_DURATION)] [SerializeField] private float tickDuration = 1f;

        [SerializeField] private Level level;

        private const float COLLISION_PAUSE_RATIO = 0.5f;

        private float elapsedTime = 0f;
        private float ratio = 0f;
        private bool isPaused = false;
        private GameStatus status = GameStatus.Playing;
        private int totalCubes = 0;
        private int finishedCubes = 0;
        private List<CubeController> cubes = new List<CubeController>();
        private CubeCollisionChecker collisionChecker = new CubeCollisionChecker();

        public static GameManager Instance { get; private set; }
        public float TickDuration => tickDuration;
        public bool IsPaused => isPaused;
        public GameStatus Status => status;
        public int TotalCubes => totalCubes;
        public int FinishedCubes => finishedCubes;

        public event Action<List<CubeController>> OnCubesCollided;
        public event Action OnGameWon;
        public event Action OnGameLost;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            foreach (CubeSpawner lSpawner in level.Spawners)
            {
                totalCubes += lSpawner.CubeCount;
            }
        }

        private void Update()
        {
            Tick();
            foreach (CubeController lCube in cubes)
            {
                lCube.UpdateState(ratio);
            }
        }
    
        public void SetTickDuration(float pDuration)
        {
            float lProgress = ratio;
            tickDuration = Mathf.Clamp(pDuration, MIN_TICK_DURATION, MAX_TICK_DURATION);
            elapsedTime = lProgress * tickDuration;
        }

        private void Tick()
        {
            if (elapsedTime > tickDuration && status == GameStatus.Playing)
            {
                DoTick();
                elapsedTime = 0f;
            }
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / tickDuration;
            if (ratio > 1f)
            {
                ratio = 1f;
            }
            if (isPaused && ratio > COLLISION_PAUSE_RATIO)
            {
                ratio = COLLISION_PAUSE_RATIO;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void DoTick()
        {
            foreach (CubeController lCube in cubes)
            {
                lCube.TickPrep(level);
            }

            SupprCubes();
            if (totalCubes > 0 && finishedCubes >= totalCubes)
            {
                Win();
                return;
            }

            SpawnCubes();
            CheckOutsideCubes();
            CheckCollisions();

            foreach (CubeController lCube in cubes)
            {
                lCube.OnTick();
            }
        }

        private void SpawnCubes()
        {
            foreach (CubeSpawner lSpawner in level.Spawners)
            {
                CubeController lNewCube = lSpawner.TrySpawn(level);
                if (lNewCube != null)
                {
                    cubes.Add(lNewCube);
                }
            }
        }

        private void SupprCubes()
        {
            for (int i = cubes.Count - 1; i >= 0; i--)
            {
                if (cubes[i].IsFinished)
                {
                    Destroy(cubes[i].gameObject);
                    cubes.RemoveAt(i);
                    finishedCubes++;
                }
            }
        }

        private void CheckOutsideCubes()
        {
            foreach (CubeController lCube in cubes)
            {
                if (lCube.IsLost)
                {
                    Lose();
                    return;
                }
            }
        }

        private void CheckCollisions()
        {
            List<CubeController> lCollidingCubes = collisionChecker.FindCollidingCubes(cubes);
            if (lCollidingCubes.Count == 0)
            {
                return;
            }

            isPaused = true;
            OnCubesCollided?.Invoke(lCollidingCubes);
            Lose();
        }

        private void Win()
        {
            status = GameStatus.Won;
            OnGameWon?.Invoke();
        }

        private void Lose()
        {
            if (status != GameStatus.Playing)
            {
                return;
            }

            status = GameStatus.Lost;
            OnGameLost?.Invoke();
        }
    }
}