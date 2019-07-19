## Starbound Asset Unpacker
So while trying to find the item names for a mod
I got off the Steam, I got to experience the joy of
using the CLI Starbound Asset Unpacker which is bundled
with the game install, and even though I am already done
and probably won't need to unpack anything else, I still
feel the need to make this tool. So here you go.

## Features
Configuration is saved to the registry automatically, 
as well as auto-detection of the Starbound workshop folder 
and CLI Asset Unpacker paths.  
Also you can check for updates through the tool itself.

## Usage
This tool is for the *Steam version of Starbound.*  
In the future, I plan on expanding it to be usable with
non-steam version.  
  
Just run the tool, and if it can find your Steam path
along with the Starbound folder and workshop folder,
it will automatically list all detected .pak files.  
  
From there, select the folder you want to spit out
the unpacked assets into, and then you can pick and choose
which .pak file to unpack, or you can unpack every .pak
file the tool has found.  
  
The tool also saves the paths, so no need to keep
putting them in every time you restart the tool.  

## Preview
![alt text](https://i.imgur.com/10iwQZL.png)

## Note before downloading source
This project uses my [Registrar](https://github.com/AWilliams17/Registrar) library, which is not current on Nuget.org at this current time.  
You'll have to download and import it if you want to mess around with the source.
