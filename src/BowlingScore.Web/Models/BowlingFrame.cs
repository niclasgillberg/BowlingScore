namespace BowlingScore.Web.Models
{
    public class BowlingFrame
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Extra { get; set; }

        public FrameState State
        {
            get
            {
                if(First == 10)
                    return FrameState.Strike;
                if(First + Second == 10)
                    return FrameState.Spare;
                return FrameState.Open;
            }
        }
    }
}