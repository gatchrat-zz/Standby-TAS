# Standby-TAS

Simple TAS Tool for the game Standby strongly based on [this](https://github.com/ShootMe/SplasherTAS) TAS from [this](https://github.com/ShootMe) awesome guy

## Installation

..* Put a TAS file named "Standby.tas" in the same folder as the .exe of the game
..* Download all files from [here](../Installstuff) and place them into the ...STANDBY\Standby_Data\Managed folder 
(backing up the files which are being replaced is recommended)

## Controls

..* Playback: Left Trigger + Right Trigger + Right Stick
..* Stop: Left Trigger + Right Trigger + DPad Down
..* Record: Left Trigger + RIght Trigger + Left Stick (Recording is discouraged, you should just edit the .tas file)
..* Frame Step: DPad Up
..* While Frame Stepping:
	..* Next frame: DPad Up
	..* Stop Frame Stepping: DPad Down
	..* Frame step continuously: Right Stick X+
	
## Creating/Editing the TAS

The file is a simple textfile, you can edit it with any text-editor.
Format for the input file is (How many frames the Actions should be executed),(Actions)
I.E : 20,R,W,J 

###Actions

..* R = Right
..* L = Left
..* X = Select (Instant respawn)
..* S = Start
..* J = Jump
..* W = Slide/Shoot/Stomp 

##Other

If you want to know what i changed in the Assembly-CSharp.dll, then you can look that up in the Whatchanged.txt




