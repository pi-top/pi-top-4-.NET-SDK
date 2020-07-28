using System.Drawing.Imaging;

namespace PiTop
{
    public class HeadlessDisplay : Display
    {
        public HeadlessDisplay(int width, int height) : base(width, height)
        {
        }

        public override void Show()
        {
            
        }

        public override void Hide()
        {
            
        }

        protected override void CommitBuffer()
        {
            
        }
    }
}