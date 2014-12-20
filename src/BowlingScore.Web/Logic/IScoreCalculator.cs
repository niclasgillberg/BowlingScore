using System.Collections.Generic;
using BowlingScore.Web.Models;

namespace BowlingScore.Web.Logic
{
    public interface IScoreCalculator
    {
        Models.BowlingScore CalculateScore(IEnumerable<BowlingFrame> frames);
    }

    public class ScoreCalculator : IScoreCalculator
    {
        public Models.BowlingScore CalculateScore(IEnumerable<BowlingFrame> frames)
        {
            var score = new Models.BowlingScore();

            foreach (var frame in frames)
            {
                var frameResult = frame.First + frame.Second;
                score.Score += frameResult;
            }

            return score;
        }
    }
}
