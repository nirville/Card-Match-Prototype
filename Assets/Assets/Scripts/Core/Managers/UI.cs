using System.Collections;
using System.Collections.Generic;
using Nirville.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Canvas boardCanvas;
    [SerializeField] Canvas menuCanvas;

    [SerializeField] Button playBtn;
    [SerializeField] Button nextBtn;
    [SerializeField] Button saveBtn;
    [SerializeField] Button quitBtn;

    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text moves;
    [SerializeField] TMP_Text play;

    [SerializeField] Button _2x2;
    [SerializeField] Button _2x3;
    [SerializeField] Button _4x4;
    [SerializeField] Button _5x6;

    public Button NextButton => nextBtn;
    public void PlayText(string str) => play.text = str.ToString();

    private void Awake() 
    {
        playBtn.onClick.AddListener(()=>
            SwitchToCanvas(boardCanvas)

        );

        _2x2.onClick.AddListener(()=> GameEvents.current.SetBoardSize(new Vector2Int(2, 2)));
        _2x3.onClick.AddListener(()=> GameEvents.current.SetBoardSize(new Vector2Int(2, 3)));
        _2x3.Select();
        _4x4.onClick.AddListener(()=> GameEvents.current.SetBoardSize(new Vector2Int(4, 4)));
        _5x6.onClick.AddListener(() => GameEvents.current.SetBoardSize(new Vector2Int(5, 6)));
    }

    private void Start() {
        SwitchToCanvas(menuCanvas);
        playBtn.onClick.AddListener(() =>
        {
            SwitchToCanvas(boardCanvas);
            GameEvents.current.StartGameplay();
        });

        quitBtn.onClick.AddListener(() =>
        {
            SwitchToCanvas(menuCanvas);
            GameEvents.current.EndGameplay();
        });

       nextBtn.interactable = false;
        nextBtn.onClick.AddListener(() =>
        {
            GameEvents.current.StartNextLevel();
        });

       saveBtn.onClick.AddListener(()=>
       {
            GameEvents.current.SaveBoard();
       });
    }

    void SwitchToCanvas(Canvas canvas)
    {
        boardCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);

        canvas.gameObject.SetActive(true);
    }

    internal void SetScore(int val) => score.text = val.ToString();
    internal void SetMoves(int val) => moves.text = val.ToString();
}
