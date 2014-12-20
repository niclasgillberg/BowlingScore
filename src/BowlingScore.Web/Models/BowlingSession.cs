using System.Collections.Generic;

namespace BowlingScore.Web.Models
{
    public class BowlingSession
    {
        public BowlingSession()
        {
            Frames = new List<BowlingFrame>();
        }

        public int Score
        {
            get
            {
                var score = 0;

                for (int i = 0; i < Frames.Count; i++)
                {
                    var frame = Frames[i];
                    int frameResult = 0;

                    if (frame.State == FrameState.Open)
                        frameResult = frame.First + frame.Second;
                    else
                    {
                        if (Frames.Count <= i + 1)
                            continue;

                        var nextFrame = Frames[i + 1];
                        frameResult = 10 + nextFrame.First;
                        if (frame.State == FrameState.Strike)
                        {
                            frameResult += nextFrame.Second;
                        }
                    }
                    
                    score += frameResult;
                }

                return score;
            }
        }

        public List<BowlingFrame> Frames { get; set; } 
    }
}