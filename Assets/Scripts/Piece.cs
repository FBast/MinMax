using System;
using System.Collections.Generic;

namespace MinMax.Scripts {
    public abstract class Piece : ICloneable {

        public PlayerColor Owner;
        public Coordinate CurrentCoordinate;

        protected Piece(PlayerColor owner, Coordinate currentCoordinate) {
            Owner = owner;
            CurrentCoordinate = currentCoordinate;
        }

        public abstract List<Coordinate> AvailableMoves(Board board);
        public abstract int EvaluateMove(Board board, Coordinate destination);
        public abstract void ExecuteMove(Board board, Coordinate destination);
        public abstract object Clone();
        
    }
}