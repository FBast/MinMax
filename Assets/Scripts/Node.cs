using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node {

    public Board Board;
    public PlayerColor PlayerColor;
    public Coordinate MoveOrigin;
    public Coordinate MoveDestination;
    public int HeuristicValue;
        
    public List<Node> Childs {
        get {
            List<Node> childs = new List<Node>();
            PlayerColor childPlayerColor = PlayerColor == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
            foreach (Piece availablePiece in Board.AvailablePieces(childPlayerColor)) {
                foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                    childs.Add(new Node(Board, childPlayerColor, availablePiece.CurrentCoordinate, availableMove));
                }
            }
            return childs;
        }
    }

    public bool IsTerminal {
        get { return Board.AvailablePieces(PlayerColor).Sum(piece => piece.AvailableMoves(Board).Count) > 0; }
    }

    public Node(Board board, PlayerColor playerColor, Coordinate moveOrigin, Coordinate moveDestination) {
        Board = (Board) board.Clone();
        PlayerColor = playerColor;
        MoveOrigin = moveOrigin;
        MoveDestination = moveDestination;
        Debug.Log("Creating Node for " + playerColor + 
                  " with move : " + moveOrigin.X + " " + moveOrigin.Y + 
                  " to " + moveDestination.X + " " + moveDestination.Y);
        Piece piece = Board.GetPiece(MoveOrigin);
        if (piece == null) throw new Exception("Cannot get piece on origin : " + moveOrigin.X + " " + moveOrigin.Y);
        HeuristicValue = piece.EvaluateMove(Board, MoveDestination);
        Board.GetPiece(MoveOrigin).ExecuteMove(Board, MoveDestination);
    }
        
}