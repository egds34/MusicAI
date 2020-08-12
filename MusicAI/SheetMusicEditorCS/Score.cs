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

        public Engraver engraver;

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

        public Score(ScoreRuler paper)
        {
            engraver = new Engraver(paper);
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