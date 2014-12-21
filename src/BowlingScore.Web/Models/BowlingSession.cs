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

                var frames = new List<BowlingFrame>(Frames);

                if (frames.Count == 10)
                {
                    if(frames[9].State == FrameState.Spare && frames[9].Extra.HasValue)
                        frames.Add(new BowlingFrame {First = frames[9].Extra.Value});
                    if (frames[9].State == FrameState.Strike)
                    {
                        if (frames[9].Second.HasValue)
                        {
                            var extraFrame = new BowlingFrame {First = frames[9].Second.Value};
                            frames.Add(extraFrame);
                            if (extraFrame.State != FrameState.Strike)
                                extraFrame.Second = frames[9].Extra;
                            else if(frames[9].Extra.HasValue)
                                frames.Add(new BowlingFrame {First = frames[9].Extra.Value});
                        }
                    }

                }

                for (int i = 0; i < Frames.Count; i++)
                {
                    var frame = frames[i];

                    if(frame.State != FrameState.Strike && !frame.Second.HasValue)
                        continue;

                    int frameResult;

                    if (frame.State == FrameState.Open)
                        frameResult = frame.First + frame.Second.Value;
                    else
                    {
                        if (frames.Count <= i + 1)
                            continue;

                        var nextFrame = frames[i + 1];
                        if(frame.State == FrameState.Spare)
                            frameResult = 10 + nextFrame.First;
                        else
                        {
                            if (nextFrame.State == FrameState.Strike)
                            {
                                if (frames.Count <= i + 2)
                                    continue;

                                var secondNextFrame = frames[i + 2];

                                frameResult = 10 + 10 + secondNextFrame.First;
                            }
                            else
                                frameResult = 10 + nextFrame.First + nextFrame.Second.Value;
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