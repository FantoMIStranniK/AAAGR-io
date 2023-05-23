
namespace AAAGR_io
{
    public abstract class GameLoop
    {
        public void LaunchGame()
        {
            while (Render.window.IsOpen)
            {
                Time.UpdateSystemTime();

                if (Time.totalTimeBeforeUpdate >= 1 / Render.wantedFrameRate)
                {
                    Time.ResetTimeBeforeUpdate();

                    DoGameStep();

                    Time.UpdateTime();
                }

                Render.TryClose();
            }

        }
        protected abstract void DoGameStep();
    }
}
