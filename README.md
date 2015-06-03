DbContext2Yuml
==============

Visualizes EntityFramework models with [yUML diagrams](http://yuml.me).

The tool provides a WPF based UI but could also be used from the command line.  
Usage: `DbContext2Yuml.exe PATH_OF_DLL`

## Known limitations
* Relations are only added to graph if navigation properties exist on both related entities
* Several relations between two entities are not handled correctly

Author: Daniel Palme  
Blog: [www.palmmedia.de](http://www.palmmedia.de)  
Twitter: [@danielpalme](http://twitter.com/danielpalme)