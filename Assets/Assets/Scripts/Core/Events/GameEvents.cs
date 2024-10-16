using System;
using UnityEngine;

namespace Nirville.Core
{
    public class GameEvents : MonoBehaviour 
    {
        public static GameEvents current;

        void Awake() => current = this;
        public event Action<Vector2Int> BoardSizeSelect;
        public event Action GameStart;
        public event Action NextLevel;
        public event Action GameEnd;
        public event Action SaveGame;

        public void SetBoardSize(Vector2Int size) => BoardSizeSelect?.Invoke(size);
        public void StartGameplay() => GameStart?.Invoke();
        public void StartNextLevel() => NextLevel?.Invoke();
        public void EndGameplay() => GameEnd?.Invoke();

        public void SaveBoard() => SaveGame?.Invoke();
    }
}