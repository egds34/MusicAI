//this class serves as a sort of container for instruments.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SheetMusicEditorCS
{
    internal class Score
    {
        public LinkedList<Instrument> InstrumentList;

        private Engraver engraver;

        private Thickness margins;
        private String title;
        private String subTitle;
        private String composer;
        private String arranger;

        public double header;
        public double indent;

        public double staveSpace;
        public double lineSpace; //lines inside the staff

        public int numMeasures;

        public double paperHeight;
        public double paperWidth;

        private Score(Thickness margins, String title, String subTitle, String composer, String arranger)
        {
            engraver = new Engraver();

            this.margins = margins;
            this.title = title;
            this.subTitle = subTitle;
            this.composer = composer;
            this.arranger = arranger;

            paperHeight = 1123;
            paperWidth = 794;
        }

        ~Score()
        {
        }

        //Sets the proper indent, stave space and line space relative to paper height and width
        public void ScaleStaves()
        {
            int numInstruments = InstrumentList.Count();
        }
    }
}