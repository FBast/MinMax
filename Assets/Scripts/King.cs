using System.Collections.Generic;

namespace MinMax.Scripts {
    public class King : Piece {
        
        public King(PlayerColor owner, Coordinate currentCoordinate) : base(owner, currentCoordinate) { }

        public override List<Coordinate> AvailableMoves(Board board) {
            throw new System.NotImplementedException();
        }

        public override int EvaluateMove(Board board, Coordinate destination) {
            throw new System.NotImplementedException();
        }

        public override void ExecuteMove(Board board, Coordinate destination) {
            throw new System.NotImplementedException();
        }

        public override object Clone() {
            return new King(Owner, CurrentCoordinate);
        }
        
    }
}