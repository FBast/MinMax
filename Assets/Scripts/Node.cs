using System;
using System.Collections.Generic;

public class Node {

    public Board Board;
    public PlayerColor PlayerEval;
    public PlayerColor PlayerTurn;
    public Coordinate MoveOrigin;
    public Coordinate MoveDestination;
    public int HeuristicValue;
        
    public PlayerColor OtherPlayerTurn => PlayerTurn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

    private List<Node> _children;
    public List<Node> Children {
        get {
            if (_children == null) {
                _children = new List<Node>();
                foreach (Piece availablePiece in Board.AvailablePieces(OtherPlayerTurn)) {
                    foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                        _children.Add(new Node(Board, PlayerEval, OtherPlayerTurn, availablePiece.CurrentCoordinate, availableMove));
                    }
                }
            } 
            return _children;
        }
    }

    public bool IsTerminal {
        get { return Children.Count == 0; }
    }

    public Node(Board board, PlayerColor playerEval, PlayerColor playerTurn, Coordinate moveOrigin, Coordinate moveDestination) {
        Board = (Board) board.Clone();
        PlayerEval = playerEval;
        PlayerTurn = playerTurn;
        MoveOrigin = moveOrigin;
        MoveDestination = moveDestination;
        Piece piece = Board.GetPiece(MoveOrigin);
        if (piece == null) throw new Exception("Cannot get piece on origin : " + moveOrigin.Row + " " + moveOrigin.Column);
        Board.GetPiece(MoveOrigin).ExecuteMove(Board, MoveDestination);
        HeuristicValue = Board.Evaluate(PlayerEval);
    }
        
}