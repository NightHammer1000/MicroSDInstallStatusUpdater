
# MicroSD Install Status Updater for Playnite (Designed for Legion GO)
A Playnite Plugin to update the Installation Status of the Entire Game Database if the SDCard is removed or inserted. Works based on the Installation Folder. So also works with Steam, EA, Ubisoft and so on.
Currently, hard-coded for the SDCard Reader name of the Legion Go. PR for a Universal Solutions are quite welcome!

## How does it work?

Its simple. The Plugin uses an WMI Event Watcher to watch for the PNP Event of the SDCard Reader with the Model Name "SDXC Card".
If it detects the Removal or Insertion of a card it iterates through every Game in the Library, testing if the Path is available or not.
If it's not, the Game gets set to Uninstalled.
If the Path is there, the Game gets set to installed.

Simple as that.

## How to Install?
Drop the Folder from the downloaded .zip into your Playnites Extension folder.
That's it. No more Settings. If your Device uses the "SDXC Card" Model Name for the Device, it should work!

## Say thanks
Like it? Buy me a coffee. It's my fuel and it will keep me alive long enough to further support this :3 

<a href="https://www.buymeacoffee.com/n1ghtstorm" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
