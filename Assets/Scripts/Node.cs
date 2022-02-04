using System;
using System.Collections.Generic;
using System.Linq;

public class Node {

    public Board Board;
    public PlayerColor Player;
    public PlayerColor Opponent;
    public Coordinate MoveOrigin;
    public Coordinate MoveDestination;
    public int HeuristicValue;
        
    public PlayerColor OtherPlayerTurn => Opponent == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    public bool IsTerminal => !Children.Any();
    
    public IEnumerable<Node> Children {
        get {
            foreach (Piece availablePiece in Board.GetPieces(OtherPlayerTurn)) {
                foreach (Coordinate availableMove in availablePiece.BaseMoves(Board)) {
                    yield return new Node(Board, Player, OtherPlayerTurn, availablePiece.CurrentCoordinate, availableMove);
                }
            }
        }
    }
    
    public Node(Board board, PlayerColor player, PlayerColor opponent, Coordinate moveOrigin, Coordinate moveDestination) {
        Board = (Board) board.Clone();
        Player = player;
        Opponent = opponent;
        MoveOrigin = moveOrigin;
        MoveDestination = moveDestination;
        Piece piece = Board.GetPiece(MoveOrigin);
        if (piece == null) throw new Exception("Cannot get piece on origin : " + moveOrigin.Row + " " + moveOrigin.Column);
        Board.GetPiece(MoveOrigin).ExecuteMove(Board, MoveDestination);
        HeuristicValue = Board.Evaluate(Player, Opponent);
    }
        
}