using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetMusicEditorCS
{
    internal class Note
    {
        public enum KeySignature { None = 0, C, F, Bb, Eb, Ab, Db, Gb, Cb, G, D, A, E, B, Fs, Cs };

        public enum ClefName { None = 0, Treble, Bass, Alto };

        public enum NoteName
        {
            SixteenthRest = -6, EighthRest, QuarterRest, HalfRest, WholeRest, restHold, None = 0, noteHold, WholeNote, HalfNote, QuarterNote, EighthNote, SixteenthNote,
        };
    }
}