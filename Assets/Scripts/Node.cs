using System;
using System.Collections.Generic;
using System.Linq;

public class Node {

    public Board Board;
    public PlayerColor PlayerTurn;
    public Coordinate MoveOrigin;
    public Coordinate MoveDestination;
    public int HeuristicValue;
        
    public PlayerColor OtherPlayerColor => PlayerTurn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    
    public List<Node> Children {
        get {
            List<Node> children = new List<Node>();
            foreach (Piece availablePiece in Board.AvailablePieces(OtherPlayerColor)) {
                foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                    children.Add(new Node(Board, OtherPlayerColor, availablePiece.CurrentCoordinate, availableMove));
                }
            }
            return children;
        }
    }

    public bool IsTerminal => false;
    
//    public bool IsTerminal {
//        get { return Board.AvailablePieces(OtherPlayerColor).Sum(piece => piece.AvailableMoves(Board).Count) > 0; }
//    }

    public Node(Board board, PlayerColor playerTurn, Coordinate moveOrigin, Coordinate moveDestination) {
        Board = (Board) board.Clone();
        PlayerTurn = playerTurn;
        MoveOrigin = moveOrigin;
        MoveDestination = moveDestination;
        Piece piece = Board.GetPiece(MoveOrigin);
        if (piece == null) throw new Exception("Cannot get piece on origin : " + moveOrigin.Row + " " + moveOrigin.Column);
        HeuristicValue = piece.MoveEvaluation(Board, MoveDestination);
        Board.GetPiece(MoveOrigin).ExecuteMove(Board, MoveDestination);
    }
        
}