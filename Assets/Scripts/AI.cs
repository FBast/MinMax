using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI {

    public Board Board;
    public PlayerColor PlayerColor;
    public int Depth = 4;
        
    private Dictionary<Node, int> _ThinkResults = new Dictionary<Node, int>();

    public AI(Board board, PlayerColor playerColor) {
        Board = board;
        PlayerColor = playerColor;
    }

    public void Think() {
        float startingTime = Time.realtimeSinceStartup;
        _ThinkResults.Clear();
        Debug.Log("Thinking...");
        foreach (Piece piece in Board.AvailablePieces(PlayerColor)) {
            foreach (Coordinate availableMove in piece.AvailableMoves(Board)) {
                Node node = new Node(Board, PlayerColor, piece.CurrentCoordinate, availableMove);
//                int result = MinMax(node, Depth, false);
                int result = AlphaBetaMinMax(node, Depth, int.MinValue, int.MaxValue, false);
//                int result = NegaMax(node, Depth, -1);
//                int result = AlphaBetaNegaMax(node, Depth, int.MinValue, int.MaxValue, -1);
                _ThinkResults.Add(node, result);
            }
        }
        Debug.Log("Reflexion took about : " + (Time.realtimeSinceStartup - startingTime) + " seconds");
        foreach (KeyValuePair<Node,int> thinkResult in _ThinkResults) {
            if (thinkResult.Value > 0) {
                Debug.Log(thinkResult.Key.MoveOrigin + " move to " +
                          thinkResult.Key.MoveDestination + " with heuristic of " + thinkResult.Value);
            }
        }
    }

    public void Act() {
        if (_ThinkResults.Count == 0) throw new Exception("MinMax results is empty");
        int bestValue = _ThinkResults.OrderByDescending(pair => pair.Value).First().Value;
        _ThinkResults = _ThinkResults.Where(pair => pair.Value == bestValue)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        KeyValuePair<Node,int> elementAt = _ThinkResults.ElementAt(Random.Range(0, _ThinkResults.Count));
        Board.GetPiece(elementAt.Key.MoveOrigin).ExecuteMove(Board, elementAt.Key.MoveDestination);
    }
        
    private int MinMax(Node node, int depth, bool isMax) {
        if (depth == 0 || node.IsTerminal) {
            return node.HeuristicValue;
        }
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Childs) {
                value = Mathf.Max(value, MinMax(child, depth - 1, false));
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Childs) {
                value = Mathf.Min(value, MinMax(child, depth - 1, true));
            }
        }
        return value + node.HeuristicValue;
    }

    // int result = AlphaBetaMinMax(node, Depth, int.MinValue, int.MaxValue, false);
    private int AlphaBetaMinMax(Node node, int depth, int alpha, int beta, bool isMax) {
        if (depth == 0 || node.IsTerminal) {
            return node.HeuristicValue;
        }
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Childs) {
                value = Mathf.Max(value, AlphaBetaMinMax(child, depth - 1, alpha, beta, false));
                if (value >= beta) return value + node.HeuristicValue;
                alpha = Mathf.Max(alpha, value);
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Childs) {
                value = Mathf.Min(value, AlphaBetaMinMax(child, depth - 1, alpha, beta, true));
                if (alpha >= value) return value + node.HeuristicValue;
                beta = Mathf.Min(beta, value);
            }
        }
        return value + node.HeuristicValue;
    }
    
    private int NegaMax(Node node, int depth, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Childs) {
            value = Mathf.Max(value, -NegaMax(child, depth - 1, -color));
        }
        return value + node.HeuristicValue * color;
    }
    
    private int AlphaBetaNegaMax(Node node, int depth, int alpha, int beta, int sign) {
        if (depth == 0 || node.IsTerminal)
            return sign * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Childs) {
            value = Mathf.Max(value, -AlphaBetaNegaMax(child, depth - 1, -beta, -alpha, -sign));
            alpha = Mathf.Max(alpha, value);
            if (alpha >= beta) break;
        }
        return value + node.HeuristicValue * sign;
    }
    
}