using System;
using System.Collections.Generic;

public abstract class Piece : ICloneable {

    public Coordinate CurrentCoordinate;
    public PlayerColor Player;
    
    public PlayerColor OtherPlayer => Player == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
    
    public abstract int Value { get; }
    
    protected Piece(Coordinate currentCoordinate, PlayerColor player) {
        CurrentCoordinate = currentCoordinate;
        Player = player;
    }
    
    public abstract IEnumerable<Coordinate> BaseMoves(Board board);
    public abstract void ExecuteMove(Board board, Coordinate destination);
    public abstract object Clone();
        
    // public IEnumerable<Coordinate> PossibleMoves(Board board) {
    //     foreach (Coordinate coordinate in BaseMoves(board)) {
    //         if (!IsMoveProvokeCheck(board, coordinate)) yield return coordinate;
    //     }
    // }
    //
    // public bool IsMoveProvokeCheck(Board board, Coordinate coordinate) {
    //     Board simulationBoard = (Board) board.Clone();
    //     Piece chessPiece = simulationBoard.GetPiece(CurrentCoordinate);
    //     chessPiece.ExecuteMove(simulationBoard, coordinate);
    //     return ChessRules.Check(simulationBoard, Player, OtherPlayer);
    // }
    
}