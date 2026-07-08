
namespace GBSCompiler
{
	internal class Default
	{
		static List<string> lines = new List<string>();
		private static void emit(string line)
		{
			lines.Add(line +"\n");
		}
		public static List<string> GetDefaults(){
			emit("INCLUDE \"hardware.inc\"");
			emit("SECTION \"Header\", ROM0[$100]");
			emit("nop");
			emit("jp Init");
			emit("ds $150 - @, 0");
			emit("");
			
			emit("SECTION \"VBlank Interrupt\", ROM0[$40]");	
			emit("VBlankInterrupt:");	
			emit("push af\npush bc\npush de\npush hl");	
			emit("jp VBlankHandler");
			emit("");

			emit("SECTION \"Code\", ROM0[$150]");
			emit("Init:");
			emit("ld sp, $FFFE");
			emit("di");
			emit("call TurnOffLCD");
			emit("CopyRunDMA:");
			emit("ld hl, RunDMA ;copy RunDMA to HRAM");
			emit("ld de, $FF80");
			emit("ld bc, EndDMA - RunDMA");
			emit("call MemCopy");
			emit("");
			emit("call ZeroVRAM");
			emit("INITI:");
			emit("END_INITI:");


			emit("jp Start");
			emit("");

			emit("SCODI:");
			emit("END_SCODI:");
			emit("");

			emit("Start:");
			
			emit("call TurnOnLCDPalette0");
			emit("InitVars:");
			emit("ld a, 0");
			emit("ld [wCurKeys], a");
			emit("ld [wNewKeys], a");
			emit("ld [GameState], a");
			emit("ei");
			emit("jp Main");
			emit("");

			emit("Main:");
			emit("halt");
			emit(";NMAIN");
			emit("jp Main");
			emit("");
			emit(";NPMAIN");
			emit("PMI:");
			emit("END_PMI:");
			emit("");

			emit("VBlankHandler:");
			emit("call UpdateKeys");
			emit("call $FF80 ;RunDMA in HRAM");
			emit(";NVBLANK");
			emit("VBHI:");
			emit("END_VBHI:");
			emit("pop af");
			emit("pop bc");
			emit("pop de");
			emit("pop hl");
			emit("reti");
			emit("");

			emit("UpdateKeys:");
			emit("ld a, JOYP_GET_BUTTONS");
			emit("call .onenibble");
			emit("ld b, a");
			emit("ld a, JOYP_GET_CTRL_PAD");
			emit("call .onenibble");
			emit("swap a");
			emit("xor a, b");
			emit("ld b, a");
			emit("ld a, JOYP_GET_NONE");
			emit("ldh [rJOYP], a");
			emit("ld a, [wCurKeys]");
			emit("xor a, b");
			emit("and a, b");
			emit("ld [wNewKeys], a");
			emit("ld a, b");
			emit("ld [wCurKeys], a");
			emit("ret");
			emit(".onenibble");
			emit("ldh [rJOYP], a");
			emit("call .knownret");
			emit("ldh a, [rJOYP]");
			emit("ldh a, [rJOYP]");
			emit("ldh a, [rJOYP]");
			emit("or a, $F0");
			emit(".knownret");
			emit("ret");
			emit("");

			emit("MemCopy:");
			emit("ld a, [de]");
			emit("ld [hli], a");
			emit("inc de");
			emit("dec bc");
			emit("ld a, b");
			emit("or a, c");
			emit("jp nz, MemCopy");
			emit("ret");
			emit("");

			emit("ZeroVRAM:");
			emit("ld hl, $8000");
			emit("ld bc, $9FFF - $8000");
			emit(".zero:");
			emit("ld a, 0");
			emit("ld [hli], a");
			emit("dec bc");
			emit("ld a, b");
			emit("or a, c");
			emit("jp nz, .zero");
			emit("ret");

			emit("RunDMA:");
			emit("ld a, HIGH(OAMBuffer)");
			emit("ldh [rDMA], a");
			emit("ld a, 40");
			emit(".wait");
			emit("dec a");
			emit("jr nz, .wait");
			emit("ret");
			emit("EndDMA:");
			emit("");

			emit("TurnOffLCD:");
			emit("ld a, [rLCDC]");
			emit("bit 7, a");
			emit("jr z, .off");
			emit(".wait:");
			emit("ld a, [rLY]");
			emit("cp 144");
			emit("jr c, .wait");
			emit(".off");
			emit("xor a");
			emit("ld [rLCDC], a");
			emit("ret");
			emit("");

			emit("TurnOnLCDPalette0:");
			emit(";default palette, turn on screen");
			emit("ld a, LCDC_ON | LCDC_BG_ON | LCDC_OBJ_ON");
			emit("ld [rLCDC], a");
			emit("ld a, %11100100");
			emit("ld [rOBP0], a");
			emit("ld [rBGP], a");
			emit("ret");
			emit("");

			emit("SECTION \"Variables\", WRAM0");
			emit("wCurKeys: db");
			emit("wNewKeys: db");
			emit("GameState: db");
			emit("OAMBuffer: ds 160");
			emit(";NVAR");

			emit("BI:");
			emit("ENDBI:");
			return lines;
		}
	}
}
