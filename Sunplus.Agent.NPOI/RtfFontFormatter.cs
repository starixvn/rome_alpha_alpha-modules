using NPOI.SS.UserModel;

namespace Sunplus.Agent.NPOI
{
    public interface IFontFormatter
    {
        void Format(IFont font);
    }

    public class RtfFontFormatter : IFontFormatter
    {
        private string _fontName;
        private short _fontHeightInPoints;

        public RtfFontFormatter(string fontName, short fontHeightInPoints)
        {
            _fontName = fontName;
            _fontHeightInPoints = fontHeightInPoints;
        }

        public RtfFontFormatter(IFont font)
        {
            _fontName = font.FontName;
            _fontHeightInPoints = font.FontHeightInPoints;
        }

        public void Format(IFont font)
        {
            font.FontName = _fontName;
            font.FontHeightInPoints = _fontHeightInPoints;
        }
    }
}
