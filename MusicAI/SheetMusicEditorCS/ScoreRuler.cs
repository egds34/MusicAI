//this class takes an enumerator representing paper size. It will determine what is the most appropriate margin, pizel size, and staff size(along with space between lines but NOT space below staff)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetMusicEditorCS
{
    internal class ScoreRuler
    {
        public double leftMargin { get; set; }
        public double rightMargin { get; set; }
        public double topMargin { get; set; }
        public double botMargin { get; set; }

        public double paperHeight;
        public double paperWidth;

        public enum PaperSize { S9X12 = 0, S85X11, S11X13, S11X17, S11X14 };

        public enum Orientation { Portrait, Landscape };

        public ScoreRuler(PaperSize paperSize, Orientation orientation)
        {
            SetPaperMeasurements(paperSize, orientation);
        }

        public void SetPaperMeasurements(PaperSize paperSize, Orientation orientation)
        {
            switch (paperSize)
            {
                case PaperSize.S9X12:
                    leftMargin = 72.0;
                    rightMargin = 72.0;
                    topMargin = 48.0;
                    botMargin = 48.0;

                    paperHeight = 1152.0;
                    paperWidth = 864.0;
                    break;

                case PaperSize.S11X17:
                    leftMargin = 72.0;
                    rightMargin = 72.0;
                    topMargin = 48.0;
                    botMargin = 48.0;

                    paperHeight = 1632.0;
                    paperWidth = 1056.0;
                    break;

                case PaperSize.S11X14:
                    leftMargin = 72.0;
                    rightMargin = 72.0;
                    topMargin = 48.0;
                    botMargin = 48.0;

                    paperHeight = 1344.0;
                    paperWidth = 1056.0;
                    break;

                case PaperSize.S85X11:
                    leftMargin = 72.0;
                    rightMargin = 72.0;
                    topMargin = 48.0;
                    botMargin = 48.0;

                    paperHeight = 1056.0;
                    paperWidth = 816.0;
                    break;

                case PaperSize.S11X13:
                    leftMargin = 72.0;
                    rightMargin = 72.0;
                    topMargin = 48.0;
                    botMargin = 48.0;

                    paperHeight = 1248.0;
                    paperWidth = 1056.0;
                    break;
            }

            if (orientation == Orientation.Landscape)
            {
                double temp = paperHeight;
                paperHeight = paperWidth;
                paperWidth = temp;
            }
        }

        private double GetPaperHeight()
        {
            return paperHeight;
        }

        private double GetPaperWidth()
        {
            return paperWidth;
        }
    }
}