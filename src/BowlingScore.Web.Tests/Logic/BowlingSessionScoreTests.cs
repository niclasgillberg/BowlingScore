using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BowlingScore.Web.Models;
using Shouldly;
using Xunit;

namespace BowlingScore.Web.Tests.Logic
{
    public class BowlingSessionScoreTests
    {
        [Fact]
        public void WhenNoFramesAreProvided_ItReturnsZero()
        {
            var session = new BowlingSession();

            session.Score.ShouldBe(0);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(9, 0, 9)]
        [InlineData(9, 9, 0)]
        [InlineData(8, 4, 4)]
        public void WhenOneOpenFrameIsProvided_ItReturnsTheSumOfThatFrame(int expected, int first, int second)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame{First = first, Second = second});

            session.Score.ShouldBe(expected);
        }

        [Fact]
        public void WhenOneStrikeFrameIsProvided_ItReturnsZero()
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = 10});

            session.Score.ShouldBe(0);
        }

        [Theory]
        [InlineData(5,5)]
        [InlineData(1,9)]
        [InlineData(7,3)]
        public void WhenOneSpareFrameIsProvided_ItReturnsZero(int first, int second)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame { First = first, Second = second});

            session.Score.ShouldBe(0);
        }

        [Theory]
        [InlineData(0, new[] { 0, 0 }, new[] { 0, 0 })]
        [InlineData(10, new[] { 5, 0 }, new[] { 0, 5 })]
        [InlineData(14, new[] { 5, 2 }, new[] { 2, 5 })]
        [InlineData(9, new[] { 5, 4 }, new[] { 0, 0 })]
        public void WhenTwoOpenFramesAreProvided_ItReturnsTheSumOfThoseFrames(int expected, int[] firstFrame, int[] secondFrame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = firstFrame[0], Second = firstFrame[1]});
            session.Frames.Add(new BowlingFrame {First = secondFrame[0], Second = secondFrame[1]});
                
            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(10, new[] { 0, 0 })]
        [InlineData(22, new[] { 3, 3 })]
        [InlineData(28, new[] { 5, 4 })]
        public void WhenTwoFramesIsProvidedAndTheFirstsIsAStrike_ItReturnsTheCorrectResult(int expected, int[] secondFrame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = 10});
            session.Frames.Add(new BowlingFrame {First = secondFrame[0], Second = secondFrame[1]});

            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, new []{0,0})]
        [InlineData(5, new []{2,3})]
        [InlineData(9, new []{5,4})]
        public void WhenTwoFramesAreProvidedAndTheSecondIsAStrike_ItReturnsTheSumOfTheFirstFrame(int expected, int[] frame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = frame[0], Second = frame[1]});
            session.Frames.Add(new BowlingFrame {First = 10});

            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(10, new[]{0,0})]
        [InlineData(20, new[]{5,0})]
        [InlineData(24, new[]{5,4})]
        public void WhenTwoFramesAreProvidedAndTheFirstIsASpare_ItReturnsTheCorrectResult(int expected, int[] frame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = 5, Second = 5});
            session.Frames.Add(new BowlingFrame {First = frame[0], Second = frame[1]});

            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, new []{0,0})]
        [InlineData(6, new []{2,4})]
        [InlineData(9, new []{5,4})]
        public void WhenTwoFramesAreProvidedAndTheSecondIsASpare_ItReturnsTheSumOfTheFirstFrame(int expected, int[] frame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = frame[0], Second = frame[1]});
            session.Frames.Add(new BowlingFrame {First = 5, Second = 5});

            session.Score.ShouldBe(expected);
        }

        [Fact]
        public void WhenOneSpareAndOneStrikeFrameAreProvided_ItReturnsTwenty()
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = 5, Second = 5});
            session.Frames.Add(new BowlingFrame {First = 10});

            session.Score.ShouldBe(20);
        }

        [Theory]
        [InlineData(45, 5, 0)]
        [InlineData(51, 7, 0)]
        [InlineData(47, 1, 7)]
        public void WhenTwoConsecutiveStrikesAndOneOpenAreProvided_ItReturnsTheCorrectResult(int expected, int first, int second)
        {
            var session = new BowlingSession();

            session.Frames = BuildFrames(new[]
            {
                new[] {10, 0},
                new[] {10, 0},
                new[] {first, second}
            });

            session.Score.ShouldBe(expected);
        }

        [Fact]
        public void WhenThreeStrikeFramesAreProvided_ItReturnsThirty()
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame {First = 10});
            session.Frames.Add(new BowlingFrame {First = 10});
            session.Frames.Add(new BowlingFrame {First = 10});

            session.Score.ShouldBe(30);
        }

        [Theory]
        [InlineData(15, new[]{5,5})]
        [InlineData(11, new[]{1,9})]
        [InlineData(19, new[]{9,1})]
        public void WhenTwoSparesAreProvided_ItReturnsTheCorrectSum(int expected, int[] frame)
        {
            var session = new BowlingSession();

            session.Frames.Add(new BowlingFrame{First = 5, Second = 5});
            session.Frames.Add(new BowlingFrame{First = frame[0], Second = frame[1]});

            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, new[]{0,0}, new[]{0,0}, new[]{0,0}, new[]{0,0}, new[]{0,0})] // Only gutters
        [InlineData(40, new[]{4,4}, new[]{4,4}, new[]{4,4}, new[]{4,4}, new[]{4,4})]
        [InlineData(35, new[]{1,6}, new[]{7,0}, new[]{3,4}, new[]{5,2}, new[]{0,7})]
        [InlineData(28, new[]{1,6}, new[]{7,0}, new[]{3,4}, new[]{5,2}, new[]{10,0})] // Last is a strike
        [InlineData(28, new[]{1,6}, new[]{7,0}, new[]{3,4}, new[]{5,2}, new[]{5,5})] // Last is a spare
        [InlineData(46, new[]{10, 0}, new[]{7,0}, new[]{3,4}, new[]{5,2}, new[]{4,4})] // First is a strike
        [InlineData(46, new[]{7, 3}, new[]{7,0}, new[]{3,4}, new[]{5,2}, new[]{4,4})] // First is a spare
        [InlineData(42, new[]{7, 3}, new[]{3,4}, new[]{7,0}, new[]{5,2}, new[]{4,4})] // First is a spare
        [InlineData(44, new[]{1, 6}, new[]{3,4}, new[]{7,3}, new[]{5,2}, new[]{4,4})] // Third is a spare
        [InlineData(41, new[]{1, 6}, new[]{3,4}, new[]{7,3}, new[]{2,5}, new[]{4,4})] // Third is a spare
        [InlineData(46, new[]{1, 6}, new[]{3,4}, new[]{10,0}, new[]{2,5}, new[]{4,4})] // Third is a strike
        [InlineData(58, new[]{7, 3}, new[]{1,9}, new[]{5,5}, new[]{4,6}, new[]{8,2})] // All spares
        [InlineData(90, new[]{10,0}, new[]{10,0}, new[]{10,0}, new[]{10,0}, new[]{10,0})] // All strikes
        [InlineData(117, new[]{10,0}, new[]{10,0}, new[]{10,0}, new[]{10,0}, new[]{9,0})] // All but last are strikes
        public void WhenFiveFramesAreProvided_ItReturnsTheCorrectResult(int expected, int[] first, int[] second, int[] third, int[] fourth, int[] fifth)
        {
            var session = new BowlingSession();

            session.Frames = BuildFrames(new[]
            {
                first,
                second,
                third,
                fourth,
                fifth
            });

            session.Score.ShouldBe(expected);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(15, 5)]
        [InlineData(20, 10)]
        public void WhenTenthFrameIsASpare_ItCountsTheExtraRoll(int expected, int extraRoll)
        {
            var session = new BowlingSession();

            session.Frames = BuildFrames(new[]
            {
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {0, 0},
                new[] {5, 5, extraRoll}
            });

            session.Score.ShouldBe(expected);
        }

        [Fact]
        public void WhenThePerferctGameIsProvided_ItReturnsThePerfectScore()
        {
            var session = new BowlingSession();

            session.Frames = BuildFrames(new[]
            {
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 0},
                new[] {10, 10, 10}
            });

            session.Score.ShouldBe(300);
        }

        private List<BowlingFrame> BuildFrames(int[][] framesValues)
        {
            return framesValues.Select(frame => new BowlingFrame
            {
                First = frame[0],
                Second = frame[1],
                Extra = frame.Length == 3 ? frame[2] : 0
            }).ToList();
        }
    }
}