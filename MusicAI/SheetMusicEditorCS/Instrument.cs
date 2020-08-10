using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetMusicEditorCS
{
    internal class Instrument : Object
    {
        public enum Instruments
        {
            Piccolo, Flute, Oboe, Clarinet, BassClarinet, AltoSax, Basoon, ContraBasson, Horn, Trumpet, Trombone, BassTrombone, Euphonium, Tuba, Timpani,
            OtherPerc, OtherInst, GenericStaff, Harp, Piano, ChoralSaprano, ChoralAlto, ChoralTenor, ChoralBass, Violin, Viola, Cello, StringBass
        };

        public Instrument(Instrument instrumentToCopy)
        {
            instrumentName = instrumentToCopy.instrumentName;
            instrumentType = instrumentToCopy.instrumentType;
        }

        public Instrument(Instruments type, String name)
        {
            instrumentType = type;
            instrumentName = name;
        }

        public override string ToString()
        {
            return this.instrumentName + " (" + Enum.GetName(typeof(Instruments), instrumentType) + ")";
        }

        public String instrumentName; //you can rename instruments (Flue I and Flute II)
        public Instruments instrumentType;
        public LinkedList<Measure> MeausreList;
    }
}