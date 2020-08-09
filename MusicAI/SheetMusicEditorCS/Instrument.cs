using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetMusicEditorCS
{
    internal class Instrument
    {
        public enum Intruments
        {
            Piccolo, Flute, Oboe, Clarinet, BassClarinet, AltoSax, Basoon, ContraBasson, Horn, Trumpet, Trombone, BassTrombone, Euphonium, Tuba, Timpani,
            OtherPerc, OtherInst, GenericStaff, Harp, Piano, ChoralSaprano, ChoralAlto, ChoralTenor, ChoralBass, Violin, Viola, Cello, StringBass
        };

        public String InstrumentName;

        public LinkedList<Measure> MeausreList;
    }
}