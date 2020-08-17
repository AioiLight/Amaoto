namespace Amaoto
{
    public interface IPlayable
    {
        void Play(bool playFromBegin);
        void Stop();

        bool IsPlaying { get; }
        double Time { get; set; }
        double Volume { get; set; }

    }
}
