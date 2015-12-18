using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Sunplus.Agent.NPOI
{
    public class RtfTextRender
    {
        private IFont _font;
        private List<TextBlock> _blocks = new List<TextBlock>();

        public RtfTextRender()
        {
        }

        public RtfTextRender(IFont font)
            : base()
        {
            _font = font;
        }

        public void Clear()
        {
            _blocks.Clear();
        }

        public IRichTextString Render()
        {
            string s = "";
            foreach (var block in _blocks)
            {
                s += block.Text;
            }

            IRichTextString result = new HSSFRichTextString(s);
            int textIndex = 0;
            foreach (var block in _blocks)
            {
                IFont font = _font;
                if (font == null)
                {
                    font = block.Font;
                }
                else
                {
                    if (block.Font != null)
                    {
                        font = block.Font;
                    }
                }
                if (font != null)
                {
                    int startIndex = textIndex;
                    int endIndex = textIndex + block.Text.Length;
                    result.ApplyFont(textIndex, endIndex, font);
                }
                textIndex += block.Text.Length;
            }

            return result;
        }

        public void AddText(TextBlock block)
        {
            _blocks.Add(block);
        }

        public void AddText(string text)
        {
            _blocks.Add(new TextBlock(text));
        }

        public void AddText(string text, IFont font)
        {
            _blocks.Add(new TextBlock(text, font));
        }
    }
}
