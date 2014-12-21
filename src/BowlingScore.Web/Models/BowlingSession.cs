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
                        if(frame.State == FrameState.Spare)
                            frameResult = 10 + nextFrame.First;
                        else
                        {
                            if (nextFrame.State == FrameState.Strike)
                            {
                                if (Frames.Count <= i + 2)
                                    continue;

                                var secondNextFrame = Frames[i + 2];

                                frameResult = 10 + 10 + secondNextFrame.First;
                            }
                            else
                                frameResult = 10 + nextFrame.First + nextFrame.Second;
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