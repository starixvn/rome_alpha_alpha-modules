using NPOI.SS.UserModel;

namespace Sunplus.Agent.NPOI
{
    public class TextBlock
    {
        private string _text;
        private IFont _font;

        public string Text
        {
            get { return _text; }
        }

        public IFont Font
        {
            get { return _font; }
        }

        public TextBlock(string text)
        {
            _text = text;
        }

        public TextBlock(string text, IFont font)
        {
            _text = text;
            _font = font;
        }
    }
}
