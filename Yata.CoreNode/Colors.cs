using System.Drawing;

namespace Yata.CoreNode
{
    public interface IColorSceme
    {
        Color NodeColor { get; }
        Color NodeOnlyInputs { get; }
        Color NodeOnlyOutputsColor { get; }
        Color NodeNoIO { get; }

        Color ImagePreviewEmpty { get; }
        Color Font { get; }
        Color InputConnector { get; }
        Color OutputConnector { get; }
        Color ConnectorLines { get; }

        Color BackGround { get; }
        Color DragPreviewLine { get; }

        Color WindowBackground { get; }
        Color WindowMenuSelect { get; }
        Color WindowMenuSeperator { get; }
        Color WindowFont { get; }
    }

    internal class DarkSceme : IColorSceme
    {
        public Color NodeColor => Color.FromArgb(104, 104, 104);
        public Color NodeOnlyInputs => Color.FromArgb(104, 104, 104);
        public Color NodeOnlyOutputsColor => Color.FromArgb(104, 104, 104);
        public Color NodeNoIO => Color.FromArgb(200, 48, 48);

        public Color ImagePreviewEmpty => Color.BlanchedAlmond;
        public Color Font => Color.FromArgb(210, 207, 174);
        public Color InputConnector => Color.FromArgb(36, 99, 187);
        public Color OutputConnector => Color.FromArgb(56, 185, 174);
        public Color ConnectorLines => Color.FromArgb(104, 104, 104);

        public Color BackGround => Color.FromArgb(30, 30, 30);
        public Color DragPreviewLine => Color.FromArgb(255, 106, 0);


        public Color WindowBackground => Color.FromArgb(45, 45, 48);
        public Color WindowMenuSelect => Color.FromArgb(45, 45, 45);
        public Color WindowMenuSeperator => Color.FromArgb(104, 104, 104);
        public Color WindowFont => Color.FromArgb(230, 230, 230);
    }


    public sealed class Colors
    {
        private static IColorSceme sceme = new DarkSceme();

        public static Color Background => sceme.BackGround;

        public static Color Node => sceme.NodeColor;
        public static Color NodeOnlyInputs => sceme.NodeOnlyInputs;
        public static Color NodeOnlyOutputsColor => sceme.NodeOnlyOutputsColor;
        public static Color NodeNoIO => sceme.NodeNoIO;
        
        public static Color ImagePreviewEmpty => sceme.ImagePreviewEmpty;
        public static Color Font => sceme.Font;
        public static Color InputConnector => sceme.InputConnector;
        public static Color OutputConnector => sceme.OutputConnector;
        public static Color ConnectorLines => sceme.ConnectorLines;

        public static Color DragPreviewLine => sceme.DragPreviewLine;

        public static Color WindowBackground => sceme.WindowBackground;
        public static Color WindowMenuSelect => sceme.WindowMenuSelect;
        public static Color WindowMenuSeperator => sceme.WindowMenuSeperator;
        public static Color WindowFont => sceme.WindowFont;
    }
}
