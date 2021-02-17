using System.Collections.Generic;

namespace Checkers {
    public class CheckersKing : Piece {
        
        public CheckersKing(PlayerColor player, Coordinate currentCoordinate) : base(player, currentCoordinate) { }

        public override int Value => 5;

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
            return new CheckersKing(Player, CurrentCoordinate);
        }
        
    }
}