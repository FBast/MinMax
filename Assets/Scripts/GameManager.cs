using System;
using System.Collections.Generic;
using Chess;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : SerializedMonoBehaviour {

    [Header("Parameters")] 
    [SerializeField] private Algorithm _algorithm;
    [SerializeField] private bool _autoPlay;
    [SerializeField] private bool _useTestingBoard;
    [Range(0, 8), SerializeField] private int _depth;
    
    [Header("Board")]
    [SerializeField] private Transform _piecesContent;
    [SerializeField] private GameObject _blankPrefab;
    [SerializeField] private GameObject _whitePawnPrefab;
    [SerializeField] private GameObject _blackPawnPrefab;
    [SerializeField] private GameObject _whiteKnightPrefab;
    [SerializeField] private GameObject _blackKnightPrefab;
    [SerializeField] private GameObject _whiteRookPrefab;
    [SerializeField] private GameObject _blackRookPrefab;
    [SerializeField] private GameObject _whiteBishopPrefab;
    [SerializeField] private GameObject _blackBishopPrefab;
    [SerializeField] private GameObject _whiteQueenPrefab;
    [SerializeField] private GameObject _blackQueenPrefab;
    [SerializeField] private GameObject _whiteKingPrefab;
    [SerializeField] private GameObject _blackKingPrefab;
    
    [Header("Matrix")]
    [TableMatrix(HorizontalTitle = "ChessBoard"), SerializeField] private readonly Pieces[,] _chessBoard = new Pieces[8,8];
    [TableMatrix(HorizontalTitle = "TestingBoard"), SerializeField] private readonly Pieces[,] _testingBoard = new Pieces[8,8];

    private Board _board;
    private readonly Queue<AIBrain> _inQueueBrains = new();
    private AIBrain _currentPlayer;
    private bool _isPlaying;

    private void Awake() {
        _board = new Board(8, 8);
        _board.ConvertHandyMatrix(_useTestingBoard ? _testingBoard : _chessBoard);
        CreateAI();
        UpdateBoard(_board);
    }

    private void Update() {
        if ((Input.GetButtonUp("Jump") || _autoPlay) && !_isPlaying) {
            Invoke(nameof(PlayAI), 0);
        }
    }

    private void PlayAI() {
        _isPlaying = true; 
        _currentPlayer.Think(_algorithm);
        _currentPlayer.Act();
        UpdateBoard(_board);
        _inQueueBrains.Enqueue(_currentPlayer);
        _currentPlayer = _inQueueBrains.Dequeue();
        _isPlaying = false;
    }

    private void CreateAI() {
        _currentPlayer = new AIBrain(_board, PlayerColor.White, _depth, _algorithm);
        _inQueueBrains.Enqueue(new AIBrain(_board, PlayerColor.Black, _depth, _algorithm));
    }

    private void UpdateBoard(Board board) {
        // Clear previous pieces
        foreach (Transform child in _piecesContent) {
            Destroy(child.gameObject);
        }
        // Rebuild all pieces
        foreach (Piece piece in board.Matrix) {
            Instantiate(GetPhysicalPiece(piece),_piecesContent);
        }
    }

    private GameObject GetPhysicalPiece(Piece piece) {
        if (piece == null) return _blankPrefab;
        return piece switch {
            Pawn _ => piece.Player == PlayerColor.White ? _whitePawnPrefab : _blackPawnPrefab,
            Knight _ => piece.Player == PlayerColor.White ? _whiteKnightPrefab : _blackKnightPrefab,
            Rook _ => piece.Player == PlayerColor.White ? _whiteRookPrefab : _blackRookPrefab,
            Bishop _ => piece.Player == PlayerColor.White ? _whiteBishopPrefab : _blackBishopPrefab,
            Queen _ => piece.Player == PlayerColor.White ? _whiteQueenPrefab : _blackQueenPrefab,
            King _ => piece.Player == PlayerColor.White ? _whiteKingPrefab : _blackKingPrefab,
            _ => throw new Exception("Unknown piece type : " + piece.GetType())
        };
    }
        
}