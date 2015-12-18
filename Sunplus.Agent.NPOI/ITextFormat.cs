using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace Sunplus.Agent.NPOI
{
    public interface ITextFormat
    {
        Dictionary<string, IFont> Fonts { get; }
        string Text { get; }
    }
}
