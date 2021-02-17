using System;
using System.Collections.Generic;

public abstract class Piece : ICloneable {

    public PlayerColor Player;
    public Coordinate CurrentCoordinate;
    
    public PlayerColor OtherPlayer => Player == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
    
    public abstract int Value { get; }
    
    protected Piece(PlayerColor player, Coordinate currentCoordinate) {
        Player = player;
        CurrentCoordinate = currentCoordinate;
    }

    public abstract List<Coordinate> AvailableMoves(Board board);
    public abstract int EvaluateMove(Board board, Coordinate destination);
    public abstract void ExecuteMove(Board board, Coordinate destination);
    public abstract object Clone();
        
}