# L2dotNET
A server emulator for Lineage2 Interlude written in C#

Join the chat: [![Join the chat at https://gitter.im/Elfocrash/L2dotNET](https://badges.gitter.im/Elfocrash/L2dotNET.svg)](https://gitter.im/Elfocrash/L2dotNET?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### CI Status

[![Build status](https://ci.appveyor.com/api/projects/status/wfiyl4ve9uj0rhtx?svg=true)](https://ci.appveyor.com/project/Elfocrash/l2dotnet)


## What is this?
L2dotNET is a server emulator for L2. The files are based on some Java-like Russian files from 2008 due to the networking and encryption code included. However i ended up rewritting them anyway.
Current goal is to get all the basic functionality and the packet support fully working in order to move on to accurate game features implementation. 

## Progress
- [x] Downgrade most of the packets to C6
- [x] Implement basic known object functionality
- [x] Refactor the code for the crappy java-like structure to proper C#
- [x] Refactor LoginService to use the new Packet structure
- [x] Creating loading and saving players
- [x] Creating loading and saving items
- [x] Refactor GameService to use the new Packet structure
- [x] Rework the movement alorithm
- [ ] Add Geodata support

## Can i contribute?
Yes please. This is not something small. This will take a while before it is playable.
If you really want to profit by using the project one day, please fork it and do as many pull requests
as possible. As long as you use the coding style i am coding on, i will approve your pulls.
There is plenty of things to do for any level of developer.

Special thanks to [JetBrains](https://www.jetbrains.com/) for providing the .NET Rider and Resharper licenses

