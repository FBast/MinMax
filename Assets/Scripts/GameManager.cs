using System;
using System.Collections.Generic;
using Checkers;
using Chess;
using UnityEngine;

public class GameManager : MonoBehaviour {

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
    public Transform[,] PhysicalMatrix;
    public Board Board = new Board();
    public Transform PiecesContent;

    private AI _whiteAI;
    private AI _blackAI;
    private bool _isBlackTurn;

    private void Awake() { 
        GeneratePositionMatrix();
        Board.SetupChessBoard();
        CreateAI();
        UpdatePhysicalBoard();
    }

    private void Update() {
        if (Input.GetButtonUp("Jump")) {
            if (_isBlackTurn) {
                _blackAI.Think();
                _blackAI.Act();
                _isBlackTurn = false;
                UpdatePhysicalBoard();
            }
            else {
                _whiteAI.Think();
                _whiteAI.Act();
                _isBlackTurn = true;
                UpdatePhysicalBoard();
            }
        }
    }

    private void GeneratePositionMatrix() {
        PhysicalMatrix = new Transform[(int) Mathf.Sqrt(PositionList.Count),(int) Mathf.Sqrt(PositionList.Count)];
        foreach (Transform cellTransform in PositionList) {
            int row = Convert.ToInt32(cellTransform.name.Split('.')[0]);
            int column = Convert.ToInt32(cellTransform.name.Split('.')[1]);
            PhysicalMatrix[row, column] = cellTransform;
        }
    }

    public void CreateAI() {
        _whiteAI = new AI(Board, PlayerColor.White);
        _blackAI = new AI(Board, PlayerColor.Black);
    }
        
    private void UpdatePhysicalBoard() {
        // Clear previous pieces
        foreach (Transform child in PiecesContent) {
            Destroy(child.gameObject);
        }
        // Rebuild all pieces
        for (int i = 0; i < Board.Matrix.GetLength(0); i++) {
            for (int j = 0; j < Board.Matrix.GetLength(1); j++) {
                if (Board.Matrix[i, j] == null) continue;
                Piece piece = Board.Matrix[i, j];
                Instantiate(GetPhysicalPiece(piece, piece.Player), PhysicalMatrix[i, j].position, Quaternion.identity,PiecesContent);
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