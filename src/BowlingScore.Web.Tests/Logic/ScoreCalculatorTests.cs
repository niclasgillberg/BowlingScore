using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BowlingScore.Web.Logic;
using BowlingScore.Web.Models;
using Shouldly;
using Xunit;

namespace BowlingScore.Web.Tests.Logic
{
    public class ScoreCalculatorTests
    {
        public class CalculateScore
        {
            [Fact]
            public void WhenNoFramesAreProvided_ItReturnsZero()
            {
                var calculator = new ScoreCalculator();

                var result = calculator.CalculateScore(new List<BowlingFrame>());

                result.Score.ShouldBe(0);
            }

            [Theory]
            [InlineData(0, 0, 0)]
            [InlineData(9, 0, 9)]
            [InlineData(9, 9, 0)]
            [InlineData(8, 4, 4)]
            public void WhenOneOpenFrameIsProvided_ItReturnsTheSumOfThatFrame(int expected, int first, int second)
            {
                var calculator = new ScoreCalculator();
                var frames = new List<BowlingFrame>
                {
                    new BowlingFrame
                    {
                        First = first,
                        Second = second
                    }
                };

                var result = calculator.CalculateScore(frames);

                result.Score.ShouldBe(expected);
            }

            [Theory]
            [InlineData(0, new []{0,0}, new []{0,0})]
            [InlineData(10, new []{5,0}, new []{0,5})]
            [InlineData(14, new []{5,2}, new []{2,5})]
            [InlineData(9, new []{5,4}, new []{0,0})]
            public void WhenTwoOpenFramesAreProvided_ItReturnsTheSumOfThoseFrames(int expected, int[] firstFrame, int[] secondFrame)
            {
                var calculator = new ScoreCalculator();
                var frames = new List<BowlingFrame>
                {
                    new BowlingFrame {First = firstFrame[0], Second = firstFrame[1]},
                    new BowlingFrame {First = secondFrame[0], Second = secondFrame[1]},
                };

                var result = calculator.CalculateScore(frames);

                result.Score.ShouldBe(expected);
            }
        }
    }
}