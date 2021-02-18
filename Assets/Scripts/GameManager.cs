using System;
using System.Collections.Generic;
using Checkers;
using Chess;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : SerializedMonoBehaviour {

    [Header("Checkers")]
    public GameObject CheckersWhiteMenPrefab;
    public GameObject CheckersWhiteKingPrefab;
    public GameObject CheckersBlackMenPrefab;
    public GameObject CheckersBlackKingPrefab;

    [Header("Chess")] 
    public GameObject ChessWhitePawnPrefab;
    public GameObject ChessBlackPawnPrefab;
    public GameObject ChessWhiteKnightPrefab;
    public GameObject ChessBlackKnightPrefab;
    public GameObject ChessWhiteRookPrefab;
    public GameObject ChessBlackRookPrefab;
    public GameObject ChessWhiteBishopPrefab;
    public GameObject ChessBlackBishopPrefab;
    public GameObject ChessWhiteQueenPrefab;
    public GameObject ChessBlackQueenPrefab;
    public GameObject ChessWhiteKingPrefab;
    public GameObject ChessBlackKingPrefab;
    
    [Header("Parameters")]
    public List<Transform> PositionList;
    public Transform PiecesContent;
    public bool UseTestingBoard;

    [Header("Matrix")] 
    [TableMatrix(HorizontalTitle = "ChessBoard")] public Pieces[,] ChessBoard = new Pieces[8,8];
    [TableMatrix(HorizontalTitle = "TestingBoard")] public Pieces[,] TestingBoard = new Pieces[8,8];

    private readonly Board _board = new Board();
    private Transform[,] _physicalMatrix;
    private AI _whiteAI;
    private AI _blackAI;
    private bool _isBlackTurn;

    private void Awake() {
        GeneratePositionMatrix();
        _board.ConvertHandyMatrix(UseTestingBoard ? TestingBoard : ChessBoard);
        CreateAI();
        UpdatePhysicalBoard(_board);
    }

    private void Update() {
        if (Input.GetButtonUp("Jump")) {
            if (_isBlackTurn) {
                _blackAI.Think();
                _blackAI.Act();
                _isBlackTurn = false;
                UpdatePhysicalBoard(_board);
            }
            else {
                _whiteAI.Think();
                _whiteAI.Act();
                _isBlackTurn = true;
                UpdatePhysicalBoard(_board);
            }
        }
    }

    private void GeneratePositionMatrix() {
        _physicalMatrix = new Transform[(int) Mathf.Sqrt(PositionList.Count),(int) Mathf.Sqrt(PositionList.Count)];
        foreach (Transform cellTransform in PositionList) {
            int row = Convert.ToInt32(cellTransform.name.Split('.')[0]);
            int column = Convert.ToInt32(cellTransform.name.Split('.')[1]);
            _physicalMatrix[row, column] = cellTransform;
        }
    }

    public void CreateAI() {
        _whiteAI = new AI(_board, PlayerColor.White);
        _blackAI = new AI(_board, PlayerColor.Black);
    }
        
    public void UpdatePhysicalBoard(Board board) {
        // Clear previous pieces
        foreach (Transform child in PiecesContent) {
            Destroy(child.gameObject);
        }
        // Rebuild all pieces
        for (int i = 0; i < board.Matrix.GetLength(0); i++) {
            for (int j = 0; j < board.Matrix.GetLength(1); j++) {
                if (board.Matrix[i, j] == null) continue;
                Piece piece = board.Matrix[i, j];
                Instantiate(GetPhysicalPiece(piece, piece.Player), _physicalMatrix[i, j].position, Quaternion.identity,PiecesContent);
            }
        }
    }

    private GameObject GetPhysicalPiece(Piece piece, PlayerColor playerColor) {
        switch (piece) {
            case CheckersMen _ :
                return playerColor == PlayerColor.White ? CheckersWhiteMenPrefab : CheckersBlackMenPrefab;
            case CheckersKing _ :
                return playerColor == PlayerColor.White ? CheckersWhiteKingPrefab : CheckersBlackKingPrefab;
            case ChessPawn _ :
                return playerColor == PlayerColor.White ? ChessWhitePawnPrefab : ChessBlackPawnPrefab;
            case ChessKnight _ :
                return playerColor == PlayerColor.White ? ChessWhiteKnightPrefab : ChessBlackKnightPrefab;
            case ChessRook _ :
                return playerColor == PlayerColor.White ? ChessWhiteRookPrefab : ChessBlackRookPrefab;
            case ChessBishop _ :
                return playerColor == PlayerColor.White ? ChessWhiteBishopPrefab : ChessBlackBishopPrefab;
            case ChessQueen _ :
                return playerColor == PlayerColor.White ? ChessWhiteQueenPrefab : ChessBlackQueenPrefab;
            case ChessKing _ :
                return playerColor == PlayerColor.White ? ChessWhiteKingPrefab : ChessBlackKingPrefab;
            default:
                throw new Exception("Unknown piece type : " + piece.GetType());
        }
    }
        
}